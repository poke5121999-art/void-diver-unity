// node drive.mjs <url> <outDir> [plan.json]
// Plays a Unity WebGL page in headless Chromium on the real GPU with real input events, step by step.
// Rebuilt 2026-11-02 after a reboot wiped /tmp (from the 10-30 drive.mjs + its patches + play.mjs).
// Plan steps: {"wait": s} | {"shot": name} | {"click": [fx, fy]} | {"hover": [fx, fy]} (fractions of the canvas) |
//   {"type": "text"} | {"key": "f" | "Enter" | "Escape" | "Control" | " " | any letter, "hold": s} |
//   {"focus": true} | {"repeat": n, "steps": [...]} | {"eval": "js", "gesture": true} (result logged) |
//   {"waitLog": "regex", "timeout": s} | {"viewport": [w, h]} (window size, fires resize) |
//   {"fullscreen": true|false} (browser fullscreen of the page, entered with a user gesture)
// PLAY=1: after the plan, keep the browser and run the step lines appended to <outDir>/cmd.jsonl until a line holds
//   [{"quit": true}]; each finished line writes its number to <outDir>/done.txt (see pcmd.sh).
// THROTTLE_MBPS=n limits the network; EXTRA_CHROME_ARGS adds Chrome flags (space separated).
import { spawn } from 'node:child_process';
import { writeFileSync, mkdirSync, readFileSync, rmSync, existsSync } from 'node:fs';

const [url, outDir, planPath] = process.argv.slice(2);
const plan = planPath ? JSON.parse(readFileSync(planPath, 'utf8')) : [];
mkdirSync(outDir, { recursive: true });
const chrome = process.env.HOME + '/.cache/ms-playwright/chromium-1243/chrome-linux64/chrome';
const port = 9400 + Math.floor(Math.random() * 500);
const proc = spawn(chrome, ['--headless=new', '--no-sandbox', `--remote-debugging-port=${port}`, '--no-first-run',
  '--no-default-browser-check', `--user-data-dir=${outDir}/profile`, '--window-size=1280,900', '--lang=en-US',
  '--use-angle=gl-egl', '--use-gl=angle', '--enable-gpu', '--ignore-gpu-blocklist',
  ...(process.env.EXTRA_CHROME_ARGS ? process.env.EXTRA_CHROME_ARGS.split(' ') : []), 'about:blank'], { stdio: 'ignore', detached: true });
const sleep = ms => new Promise(r => setTimeout(r, ms));
// Chrome must not outlive this script (an orphan once ran 5 h at 2 GB): kill it on any exit or signal.
const killChrome = () => { try { process.kill(-proc.pid, 'SIGKILL'); } catch {} try { proc.kill('SIGKILL'); } catch {} };
process.on('exit', killChrome);
for (const sig of ['SIGTERM', 'SIGINT', 'SIGHUP']) process.on(sig, () => { killChrome(); process.exit(143); });
let target;
for (let i = 0; i < 50 && !target; i++) { await sleep(200); try { target = (await (await fetch(`http://127.0.0.1:${port}/json`)).json()).find(t => t.type === 'page'); } catch {} }
if (!target) { console.log('chrome devtools endpoint not reachable'); proc.kill('SIGKILL'); process.exit(2); }
const ws = new WebSocket(target.webSocketDebuggerUrl);
await new Promise(r => ws.addEventListener('open', r));
let id = 0; const pending = new Map(); const log = []; const t0 = Date.now();
const ts = () => ((Date.now() - t0) / 1000).toFixed(1);
ws.addEventListener('message', ev => {
  const m = JSON.parse(ev.data);
  if (m.id && pending.has(m.id)) { pending.get(m.id)(m); pending.delete(m.id); return; }
  if (m.method === 'Runtime.consoleAPICalled') log.push(`${ts()} [${m.params.type}] ` + m.params.args.map(a => a.value ?? a.description ?? '').join(' '));
  else if (m.method === 'Runtime.exceptionThrown') log.push(`${ts()} [exception] ${m.params.exceptionDetails.exception?.description ?? m.params.exceptionDetails.text}`);
  else if (m.method === 'Log.entryAdded') log.push(`${ts()} [${m.params.entry.level}/${m.params.entry.source}] ${m.params.entry.text}`);
});
const send = (method, params = {}) => new Promise(r => { const i = ++id; pending.set(i, r); ws.send(JSON.stringify({ id: i, method, params })); });
await send('Runtime.enable'); await send('Log.enable'); await send('Page.enable');
if (process.env.INIT_SCRIPT) await send('Page.addScriptToEvaluateOnNewDocument', { source: readFileSync(process.env.INIT_SCRIPT, 'utf8') });
await send('Page.setDownloadBehavior', { behavior: 'allow', downloadPath: outDir + '/downloads' });
if (process.env.THROTTLE_MBPS) {
  await send('Network.enable');
  const bps = Number(process.env.THROTTLE_MBPS) * 125000;
  await send('Network.emulateNetworkConditions', { offline: false, latency: 30, downloadThroughput: bps, uploadThroughput: bps });
  log.push(`0.0 [drive] network throttled to ${process.env.THROTTLE_MBPS} Mbps`);
}
await send('Page.navigate', { url });

async function canvasRect() {
  const r = await send('Runtime.evaluate', { returnByValue: true, expression: "(() => { const c = document.querySelector('#unity-canvas') || document.querySelector('canvas'); const r = c.getBoundingClientRect(); return JSON.stringify({ x: r.x, y: r.y, width: r.width, height: r.height, scale: 1 }); })()" });
  return JSON.parse(r.result.result.value);
}
const KEYS = {
  f: { key: 'f', code: 'KeyF', windowsVirtualKeyCode: 70, text: 'f' },
  Enter: { key: 'Enter', code: 'Enter', windowsVirtualKeyCode: 13, text: '\r' },
  Escape: { key: 'Escape', code: 'Escape', windowsVirtualKeyCode: 27 },
  Control: { key: 'Control', code: 'ControlLeft', windowsVirtualKeyCode: 17 },
  Tab: { key: 'Tab', code: 'Tab', windowsVirtualKeyCode: 9 },
  ' ': { key: ' ', code: 'Space', windowsVirtualKeyCode: 32, text: ' ' },
  F8: { key: 'F8', code: 'F8', windowsVirtualKeyCode: 119 },
};
async function run(steps) {
  for (const s of steps) {
    if (s.wait) await sleep(s.wait * 1000);
    else if (s.shot) {
      const clip = s.page ? undefined : await canvasRect();
      const r = await send('Page.captureScreenshot', clip ? { format: 'png', clip } : { format: 'png' });
      if (r.result?.data) writeFileSync(`${outDir}/${s.shot}.png`, Buffer.from(r.result.data, 'base64'));
      log.push(`${ts()} [drive] shot ${s.shot}${r.result?.data ? '' : ' FAILED ' + JSON.stringify(r.error)}`);
    } else if (s.hover) {
      const c = await canvasRect(); const x = c.x + s.hover[0] * c.width, y = c.y + s.hover[1] * c.height;
      for (let i = 1; i <= 10; i++) { await send('Input.dispatchMouseEvent', { type: 'mouseMoved', x: x - (10 - i) * 8, y: y - (10 - i) * 4 }); await sleep(30); }
      log.push(`${ts()} [drive] hover ${s.hover}`);
    } else if (s.rclick) {
      const c = await canvasRect(); const x = c.x + s.rclick[0] * c.width, y = c.y + s.rclick[1] * c.height;
      await send('Input.dispatchMouseEvent', { type: 'mouseMoved', x, y });
      await send('Input.dispatchMouseEvent', { type: 'mousePressed', x, y, button: 'right', clickCount: 1 });
      await sleep(80);
      await send('Input.dispatchMouseEvent', { type: 'mouseReleased', x, y, button: 'right', clickCount: 1 });
      log.push(`${ts()} [drive] rclick ${s.rclick}`);
    } else if (s.click) {
      const c = await canvasRect(); const x = c.x + s.click[0] * c.width, y = c.y + s.click[1] * c.height;
      await send('Input.dispatchMouseEvent', { type: 'mouseMoved', x, y });
      await send('Input.dispatchMouseEvent', { type: 'mousePressed', x, y, button: 'left', clickCount: 1 });
      await sleep(80);
      await send('Input.dispatchMouseEvent', { type: 'mouseReleased', x, y, button: 'left', clickCount: 1 });
      log.push(`${ts()} [drive] click ${s.click}`);
    } else if (s.key) {
      const k = KEYS[s.key] ?? { key: s.key, code: 'Key' + s.key.toUpperCase(), windowsVirtualKeyCode: s.key.toUpperCase().charCodeAt(0), text: s.key };
      await send('Input.dispatchKeyEvent', { type: k.text ? 'keyDown' : 'rawKeyDown', ...k });
      await sleep(s.hold ? s.hold * 1000 : 120);
      await send('Input.dispatchKeyEvent', { type: 'keyUp', ...k, text: undefined });
      log.push(`${ts()} [drive] key ${s.key}`);
    } else if (s.type) {
      for (const ch of s.type) {
        const code = /[a-z]/i.test(ch) ? 'Key' + ch.toUpperCase() : (/[0-9]/.test(ch) ? 'Digit' + ch : '');
        const vk = ch.toUpperCase().charCodeAt(0);
        await send('Input.dispatchKeyEvent', { type: 'keyDown', key: ch, code, windowsVirtualKeyCode: vk, text: ch });
        await send('Input.dispatchKeyEvent', { type: 'keyUp', key: ch, code, windowsVirtualKeyCode: vk });
        await sleep(60);
      }
      log.push(`${ts()} [drive] type ${s.type}`);
    } else if (s.waitLog) {
      const re = new RegExp(s.waitLog), until = Date.now() + (s.timeout ?? 300) * 1000;
      while (Date.now() < until && !log.some(l => re.test(l))) await sleep(500);
      log.push(`${ts()} [drive] waitLog ${s.waitLog} ${log.some(l => re.test(l)) ? 'found' : 'TIMEOUT'}`);
    } else if (s.viewport) {
      const r = await send('Emulation.setDeviceMetricsOverride', { width: s.viewport[0], height: s.viewport[1], deviceScaleFactor: s.dpr ?? 1, mobile: false });
      log.push(`${ts()} [drive] viewport ${s.viewport} dpr=${s.dpr ?? 1} ${r.error ? JSON.stringify(r.error) : ''}`);
    } else if (s.fullscreen !== undefined) {
      const expr = s.fullscreen ? "document.documentElement.requestFullscreen().then(() => 'entered', e => 'refused ' + e)" : "document.exitFullscreen().then(() => 'left', e => 'refused ' + e)";
      const r = await send('Runtime.evaluate', { returnByValue: true, awaitPromise: true, userGesture: true, expression: expr });
      log.push(`${ts()} [drive] fullscreen ${s.fullscreen} => ${JSON.stringify(r.result?.result?.value)}`);
    } else if (s.focus) {
      await send('Runtime.evaluate', { expression: "(document.querySelector('#unity-canvas') || document.querySelector('canvas')).focus()" });
      log.push(`${ts()} [drive] canvas.focus()`);
    } else if (s.eval) {
      const r = await send('Runtime.evaluate', { returnByValue: true, awaitPromise: true, userGesture: !!s.gesture, expression: s.eval });
      log.push(`${ts()} [drive] eval ${s.eval} => ${JSON.stringify(r.result?.result?.value ?? r.result?.exceptionDetails?.text)}`);
    } else if (s.repeat) { for (let i = 0; i < s.repeat; i++) await run(s.steps); }
    writeFileSync(`${outDir}/console.log`, log.join('\n') + '\n');
  }
}
await run(plan);
if (process.env.PLAY) {
  const cmdFile = `${outDir}/cmd.jsonl`; let doneLines = 0; let quit = false;
  writeFileSync(`${outDir}/done.txt`, '0\n');
  while (!quit) {
    await sleep(250);
    if (!existsSync(cmdFile)) continue;
    const lines = readFileSync(cmdFile, 'utf8').split('\n').filter(l => l.trim());
    while (doneLines < lines.length) {
      const steps = JSON.parse(lines[doneLines]);
      if (steps.some(s => s.quit)) { quit = true; break; }
      log.push(`${ts()} [play] line ${doneLines + 1}`);
      await run(steps);
      doneLines++;
      writeFileSync(`${outDir}/done.txt`, `${doneLines}\n`);
    }
  }
}
writeFileSync(`${outDir}/console.log`, log.join('\n') + '\n');
ws.close(); proc.kill('SIGKILL');
// The profile holds the game's bundle cache (hundreds of MB) in a small tmpfs.
await sleep(1000); rmSync(`${outDir}/profile`, { recursive: true, force: true });
console.log('done', ts());
