// node tools/harness/cache-check.mjs <url | build dir> <out dir> [--keep-http-cache] [--no-lounge] [--one-replica]
// Return-visit gate for the web build: does a second visit download the game again?
//   Visit 1: fresh Chrome profile, open the page, wait for the title and for the network to go quiet, seed the
//            post-tutorial save (tools/harness/plan-rep.json step "seeded"), reload, Continue into the lounge, close.
//   Between: visit 2 starts with a new empty HTTP cache (--disk-cache-dir) (the worst case: a 456 MB game does not fit the HTTP cache
//            budget, so the gate must pass on IndexedDB alone; --keep-http-cache keeps them).
//   Visit 2: same profile, a new Chrome process, open the page, wait for the title, Continue into the lounge.
// Every response's bytes over the network (CDP Network.loadingFinished encodedDataLength: headers + body as received,
// 0 for a disk-cache hit, a few hundred for a 304) go to <out>/visit-<n>.json, summed per kind in <out>/report.txt.
// For a local build the server's own count of body bytes per path is what the gate reads (CDP misreported engine files
// as cache hits); for a URL, CDP's count. Gate (visit 2), counting only files visit 1 already downloaded: the .data, .wasm, .framework.js, the Addressables
// bundles, the catalog and the StreamingAssets pack each come again in under 64 KB over the network in total, and the
// lounge scene loads. Exit 0 = PASS, 1 = FAIL, 2 = harness error.
// A build dir is served like GitHub Pages: Cache-Control max-age=600, ETag + Last-Modified from two replicas one second
// apart (as measured on Pages), 304 on If-None-Match / If-Modified-Since, gzip on the fly for text types (.txt .js .html .json, weak ETag then), no Content-Encoding for
// .unityweb (the loader's decompression fallback unpacks them), HEAD supported. The server also counts body bytes sent.
import { spawn, execFileSync } from 'node:child_process';
import { createServer } from 'node:http';
import { gzipSync } from 'node:zlib';
import { createHash } from 'node:crypto';
import { mkdirSync, writeFileSync, readFileSync, statSync, rmSync, existsSync } from 'node:fs';
import { join, extname, normalize, dirname } from 'node:path';
import { fileURLToPath } from 'node:url';

const args = process.argv.slice(2).filter(a => !a.startsWith('--'));
const flags = new Set(process.argv.slice(2).filter(a => a.startsWith('--')));
const [target, outDir] = args;
if (!target || !outDir) { console.error('usage: node tools/harness/cache-check.mjs <url | build dir> <out dir> [--keep-http-cache] [--no-lounge] [--one-replica]'); process.exit(2); }
const here = dirname(fileURLToPath(import.meta.url));
rmSync(outDir, { recursive: true, force: true });
mkdirSync(outDir, { recursive: true });
const sleep = ms => new Promise(r => setTimeout(r, ms));
const t0 = Date.now();
const say = s => { const l = `${((Date.now() - t0) / 1000).toFixed(1).padStart(7)}s ${s}`; console.log(l); report.push(l); };
const report = [];

// ---- GitHub-Pages-like server for a local build dir
let url = target, served = null, phase = 0;
if (existsSync(target) && statSync(target).isDirectory()) {
  const TYPES = { '.html': 'text/html; charset=utf-8', '.js': 'application/javascript; charset=utf-8', '.json': 'application/json; charset=utf-8',
    '.txt': 'text/plain; charset=utf-8', '.css': 'text/css; charset=utf-8', '.png': 'image/png', '.ico': 'image/x-icon', '.wasm': 'application/wasm' };
  const TEXT = new Set(['.html', '.js', '.json', '.txt', '.css']);
  const gz = new Map();
  let requests = 0;
  served = [];
  const server = createServer((req, res) => {
    const path = normalize(decodeURIComponent(req.url.split('?')[0])).replace(/^(\.\.[/\\])+/, '');
    let file = join(target, path.endsWith('/') ? path + 'index.html' : path);
    let st;
    try { st = statSync(file); if (st.isDirectory()) { file = join(file, 'index.html'); st = statSync(file); } } catch { res.writeHead(404); res.end(); return; }
    const ext = extname(file);
    const zip = TEXT.has(ext) && /gzip/.test(req.headers['accept-encoding'] || '');
    // Two replicas whose copies are one second apart, answering in turn (GitHub Pages, measured 2026-11-03: the same
    // bundle came with Last-Modified 05:12:18 and 05:12:19, ETag 6ae96d32 / 6ae96d33). --one-replica turns it off.
    const replica = flags.has('--one-replica') ? 0 : (requests++ % 2);
    const mtime = Math.floor(st.mtimeMs / 1000) + replica;
    const tag = `"${mtime.toString(16)}-${st.size.toString(16)}"`;
    const etag = zip ? 'W/' + tag : tag;
    const lastModified = new Date(mtime * 1000).toUTCString();
    const headers = { 'Content-Type': TYPES[ext] ?? 'application/octet-stream', 'Cache-Control': 'max-age=600', ETag: etag,
      'Last-Modified': lastModified, Vary: 'Accept-Encoding', 'Access-Control-Allow-Origin': '*' };
    const inm = req.headers['if-none-match'], ims = req.headers['if-modified-since'];
    const fresh = inm ? inm.split(',').map(s => s.trim().replace(/^W\//, '')).includes(tag) : (ims && Date.parse(ims) >= Date.parse(lastModified));
    if (fresh) { res.writeHead(304, headers); res.end(); served.push({ phase, path, status: 304, bytes: 0 }); return; }
    let body = readFileSync(file);
    if (zip) { const k = file + st.mtimeMs; if (!gz.has(k)) gz.set(k, gzipSync(body)); body = gz.get(k); headers['Content-Encoding'] = 'gzip'; }
    headers['Content-Length'] = body.length;
    res.writeHead(200, headers);
    if (req.method === 'HEAD') { res.end(); served.push({ phase, path, status: 200, bytes: 0, head: true }); return; }
    res.end(body);
    served.push({ phase, path, status: 200, bytes: body.length });
  });
  await new Promise(r => server.listen(0, '127.0.0.1', r));
  url = `http://127.0.0.1:${server.address().port}/`;
  process.on('exit', () => server.close());
}
// No log uploads from the harness; ?debug=1 makes the page's log collector list every request too.
url += (url.includes('?') ? '&' : '?') + 'vdlog=0&debug=1';

// ---- Chrome over CDP
const own = u => { try { return /^https?:/.test(u) && new URL(u).origin === new URL(url).origin; } catch { return false; } }; // blob: URLs carry the page origin too
const chromeBin = process.env.CHROME ?? process.env.HOME + '/.cache/ms-playwright/chromium-1243/chrome-linux64/chrome';
const profile = join(outDir, 'profile');
const profileInUse = () => execFileSync('ps', ['-eo', 'args'], { encoding: 'utf8' }).split('\n').some(l => l.includes('--user-data-dir=' + profile + ' ') || l.endsWith('--user-data-dir=' + profile));

async function openChrome(visit) {
  const port = 9400 + Math.floor(Math.random() * 500);
  const proc = spawn(chromeBin, ['--headless=new', '--no-sandbox', `--remote-debugging-port=${port}`, '--no-first-run',
    '--no-default-browser-check', `--user-data-dir=${profile}`, '--window-size=1280,900', '--lang=en-US',
    '--use-angle=gl-egl', '--use-gl=angle', '--enable-gpu', '--ignore-gpu-blocklist',
    // Each visit gets its own empty HTTP cache unless --keep-http-cache: deleting the profile's cache folder between
    // visits did not empty it (the .wasm still came from the disk cache, 2026-11-03).
    `--disk-cache-dir=${join(outDir, flags.has('--keep-http-cache') ? 'http-cache' : 'http-cache-' + visit)}`, 'about:blank'], { stdio: 'ignore' });
  let page;
  for (let i = 0; i < 75 && !page; i++) { await sleep(200); try { page = (await (await fetch(`http://127.0.0.1:${port}/json`)).json()).find(t => t.type === 'page'); } catch {} }
  if (!page) { proc.kill('SIGKILL'); throw new Error('chrome devtools endpoint not reachable on port ' + port); }
  const ws = new WebSocket(page.webSocketDebuggerUrl);
  await new Promise(r => ws.addEventListener('open', r));
  let id = 0; const pending = new Map(); const log = []; const reqs = new Map();
  const ts = () => ((Date.now() - t0) / 1000).toFixed(1);
  const c = { proc, ws, log, reqs, lastNet: Date.now() };
  ws.addEventListener('message', ev => {
    const m = JSON.parse(ev.data), p = m.params;
    if (m.id && pending.has(m.id)) { pending.get(m.id)(m); pending.delete(m.id); return; }
    if (m.method === 'Runtime.consoleAPICalled') log.push(`${ts()} [${p.type}] ` + p.args.map(a => a.value ?? a.description ?? '').join(' '));
    else if (m.method === 'Runtime.exceptionThrown') log.push(`${ts()} [exception] ${p.exceptionDetails.exception?.description ?? p.exceptionDetails.text}`);
    else if (m.method === 'Network.requestWillBeSent') { reqs.set(p.requestId, { url: p.request.url, method: p.request.method, bytes: 0, t: +((Date.now() - t0) / 1000).toFixed(1) }); if (own(p.request.url)) c.lastNet = Date.now(); }
    else if (m.method === 'Network.responseReceived') { const r = reqs.get(p.requestId); if (r) { r.status = p.response.status; r.fromDiskCache = p.response.fromDiskCache; r.fromServiceWorker = p.response.fromServiceWorker; } }
    else if (m.method === 'Network.requestServedFromCache') { const r = reqs.get(p.requestId); if (r) r.fromMemoryOrDisk = true; }
    else if (m.method === 'Network.loadingFinished') { const r = reqs.get(p.requestId); if (r) { r.bytes = p.encodedDataLength; r.done = true; if (own(r.url)) c.lastNet = Date.now(); } }
    else if (m.method === 'Network.loadingFailed') { const r = reqs.get(p.requestId); if (r) { r.failed = p.errorText; r.done = true; if (own(r.url)) c.lastNet = Date.now(); } }
  });
  c.send = (method, params = {}) => new Promise(r => { const i = ++id; pending.set(i, r); ws.send(JSON.stringify({ id: i, method, params })); });
  await c.send('Runtime.enable'); await c.send('Page.enable');
  await c.send('Network.enable', { maxTotalBufferSize: 0, maxResourceBufferSize: 0 });
  c.waitLog = async (re, seconds, from = 0) => { const until = Date.now() + seconds * 1000; while (Date.now() < until) { if (log.slice(from).some(l => re.test(l))) return true; await sleep(500); } return false; };
  // Quiet = no request to the game's own site open or finished within quietMs. Requests to other sites do not count:
  // a tracker (cdp.platform.loadcomplete.com/v1/track) posts every few seconds and some of its requests never end.
  c.waitQuiet = async (quietMs, maxSeconds) => { const until = Date.now() + maxSeconds * 1000; while (Date.now() < until) { const open = [...reqs.values()].some(r => !r.done && own(r.url)); if (!open && Date.now() - c.lastNet > quietMs) return true; await sleep(500); } return false; };
  c.click = async (fx, fy) => {
    const r = await c.send('Runtime.evaluate', { returnByValue: true, expression: "(() => { const r = document.querySelector('#unity-canvas').getBoundingClientRect(); return [r.x, r.y, r.width, r.height]; })()" });
    const [x0, y0, w, h] = r.result.result.value, x = x0 + fx * w, y = y0 + fy * h;
    await c.send('Input.dispatchMouseEvent', { type: 'mouseMoved', x, y });
    await c.send('Input.dispatchMouseEvent', { type: 'mousePressed', x, y, button: 'left', clickCount: 1 });
    await sleep(80);
    await c.send('Input.dispatchMouseEvent', { type: 'mouseReleased', x, y, button: 'left', clickCount: 1 });
  };
  c.shot = async name => { const r = await c.send('Page.captureScreenshot', { format: 'png' }); if (r.result?.data) writeFileSync(join(outDir, name + '.png'), Buffer.from(r.result.data, 'base64')); };
  // Closed = no process left with this profile: Chrome's helpers outlive the main process for a while and write the
  // HTTP cache index on their way out, which put a deleted cache back (seen 2026-11-03).
  c.close = async () => {
    try { await c.send('Browser.close'); } catch {}
    for (let i = 0; i < 60 && profileInUse(); i++) await sleep(500);
    try { proc.kill('SIGKILL'); } catch {}
    for (let i = 0; i < 20 && profileInUse(); i++) await sleep(500);
    if (profileInUse()) throw new Error('chrome processes still use ' + profile + ' after closing');
  };
  return c;
}

function kind(u) {
  const p = u.split('?')[0];
  if (/\.data(\.unityweb|\.br|\.gz)?$/.test(p)) return 'data';
  if (/\.wasm(\.unityweb|\.br|\.gz)?$/.test(p)) return 'wasm';
  if (/\.framework\.js(\.unityweb|\.br|\.gz)?$/.test(p)) return 'framework';
  if (/\.loader\.js$/.test(p)) return 'loader';
  if (/\.bundle(\.txt)?$/.test(p)) return 'bundle';
  if (/vd-streaming-pack/.test(p)) return 'streaming-pack';
  if (/\/aa\/(catalog|settings)/.test(p)) return 'aa-catalog';
  if (/supabase/.test(p)) return 'log-upload';
  return 'other';
}
const GATED = ['data', 'wasm', 'framework', 'bundle', 'aa-catalog', 'streaming-pack'];
const LIMIT = 64 * 1024;

function summarize(n, c, titleSecs) {
  const list = [...c.reqs.values()].filter(r => /^https?:/.test(r.url));
  writeFileSync(join(outDir, `visit-${n}.json`), JSON.stringify(list, null, 1));
  const by = {};
  for (const r of list) {
    const k = kind(r.url); const b = by[k] ??= { requests: 0, bytes: 0, s200: 0, s304: 0, cacheHits: 0 };
    b.requests++; b.bytes += r.bytes || 0;
    if (r.status === 304) b.s304++; else if (r.status === 200 && !r.fromDiskCache && !r.fromMemoryOrDisk) b.s200++;
    if (r.fromDiskCache || r.fromMemoryOrDisk) b.cacheHits++;
  }
  const total = Object.values(by).reduce((s, b) => s + b.bytes, 0);
  say(`visit ${n}: ${(total / 1048576).toFixed(1)} MB over the network in ${list.length} requests; title after ${titleSecs ?? '?'} s`);
  for (const [k, b] of Object.entries(by).sort((a, b) => b[1].bytes - a[1].bytes))
    say(`  ${k.padEnd(15)} ${String(b.requests).padStart(4)} req  ${(b.bytes / 1048576).toFixed(2).padStart(8)} MB  200:${b.s200} 304:${b.s304} http-cache:${b.cacheHits}`);
  const big = list.filter(r => r.bytes > 1048576).sort((a, b) => b.bytes - a.bytes).slice(0, 12);
  for (const r of big) say(`    ${(r.bytes / 1048576).toFixed(1).padStart(6)} MB ${r.status} ${r.url.split('/').pop().split('?')[0]}`);
  return { kinds: by, list };
}

function titleSeconds(log) {
  const l = log.find(x => /\[vd-loading\] title after/.test(x));
  const m = l && /title after ([\d.]+) s/.exec(l);
  return m ? m[1] : null;
}

// Title -> Continue -> lounge (clicks from tools/harness/gate-confirm.sh, measured 2026-11-03 at 1280x900).
async function toLounge(c, from) {
  await sleep(8000);
  for (const [x, y, w] of [[0.5, 0.5, 5], [0.6, 0.3, 2.5], [0.12, 0.65, 8], [0.425, 0.66, 1]]) { await c.click(x, y); await sleep(w * 1000); }
  const ok = await c.waitLog(/\[VD:Scene\] loaded LoungeScene/, 180, from);
  await sleep(10000);
  return ok;
}

let ok = true;
try {
  // Visit 1
  phase = 1;
  say(`visit 1: ${url} (fresh profile)`);
  let c = await openChrome(1);
  await c.send('Page.navigate', { url });
  if (!await c.waitLog(/\[vd-loading\] title after/, 900)) throw new Error('visit 1: the title never came (no "[vd-loading] title after" in 900 s)');
  await c.waitQuiet(20000, 600);
  const seedStep = JSON.parse(readFileSync(join(here, 'plan-rep.json'), 'utf8')).find(s => s.eval && s.eval.includes('seeded'));
  const seeded = await c.send('Runtime.evaluate', { returnByValue: true, awaitPromise: true, expression: seedStep.eval });
  say(`visit 1: save ${seeded.result?.result?.value}`);
  await c.shot('visit-1-title');
  // Into the lounge on visit 1 as well, so visit 2 asks for no file visit 1 did not already download.
  if (!flags.has('--no-lounge')) {
    await c.send('Page.reload');
    c.log.push('--- reload after the save seed');
    const mark = c.log.length;
    if (!await c.waitLog(/\[vd-loading\] title after/, 600, mark)) throw new Error('visit 1: no title after the reload');
    if (!await toLounge(c, mark)) throw new Error('visit 1: the lounge scene never loaded');
    await c.waitQuiet(15000, 180);
    await c.shot('visit-1-lounge');
  }
  writeFileSync(join(outDir, 'visit-1-console.log'), c.log.join('\n') + '\n');
  const visit1 = summarize(1, c, titleSeconds(c.log));
  await c.close();
  if (!flags.has('--keep-http-cache')) {
    // The HTTP cache and the compiled-code cache live in these folders; IndexedDB ("IndexedDB") stays.
    for (const d of ['Cache', 'Code Cache', 'GPUCache']) rmSync(join(profile, 'Default', d), { recursive: true, force: true });
    say('between visits: visit 2 gets a new empty HTTP cache (IndexedDB kept)');
  }

  // Visit 2
  phase = 2;
  c = await openChrome(2);
  say('visit 2: same profile, new Chrome process');
  await c.send('Page.navigate', { url });
  if (!await c.waitLog(/\[vd-loading\] title after/, 900)) { say('FAIL visit 2: the title never came'); ok = false; }
  let lounge = false;
  if (ok && !flags.has('--no-lounge')) { lounge = await toLounge(c, 0); await c.shot('visit-2-lounge'); }
  await c.waitQuiet(15000, 120);
  writeFileSync(join(outDir, 'visit-2-console.log'), c.log.join('\n') + '\n');
  const by = summarize(2, c, titleSeconds(c.log));
  await c.close();

  // A local build is judged on what the server sent (body bytes per path): CDP reported the old loader's .wasm and
  // .framework.js as 0-byte disk-cache hits on a new empty HTTP cache while the server had sent all 15 MB (2026-11-03).
  // A URL is judged on CDP's encodedDataLength, the only count there is.
  const source = served ? 'server' : 'CDP';
  say(`--- gate (visit 2): bytes sent again for files visit 1 already downloaded (counted by the ${source})`);
  const path = u => { try { return new URL(u).pathname; } catch { return u.split('?')[0]; } };
  const seen = served ? new Set(served.filter(s => s.phase === 1 && s.bytes > 0).map(s => s.path))
    : new Set(visit1.list.map(r => path(r.url)));
  const again = {};
  const sentAgain = served ? served.filter(s => s.phase === 2 && s.bytes > 0).map(s => ({ url: s.path, bytes: s.bytes }))
    : by.list.filter(r => r.bytes > 0).map(r => ({ url: path(r.url), bytes: r.bytes }));
  for (const r of sentAgain) {
    if (!seen.has(r.url)) continue;
    const k = kind(r.url); (again[k] ??= []).push(r);
  }
  for (const k of GATED) {
    const rs = again[k] ?? [], bytes = rs.reduce((a, r) => a + r.bytes, 0), asked = by.kinds[k]?.requests ?? 0;
    const pass = bytes < LIMIT;
    say(`${pass ? 'PASS' : 'FAIL'} ${k}: ${bytes} bytes sent again (${rs.length} files; ${asked} requests seen by the page; limit ${LIMIT})`);
    ok &&= pass;
  }
  for (const [k, rs] of Object.entries(again)) {
    if (GATED.includes(k)) continue;
    for (const r of rs) if (r.bytes >= 4096) say(`INFO not gated, downloaded again: ${k} ${r.bytes} bytes ${r.url.split('/').pop().split('?')[0]}`);
  }
  const fresh = sentAgain.filter(r => !seen.has(r.url));
  if (fresh.length) say(`INFO first-time downloads on visit 2 (not asked on visit 1): ${fresh.length} files, ${(fresh.reduce((a, r) => a + r.bytes, 0) / 1048576).toFixed(1)} MB`);
  if (!flags.has('--no-lounge')) { say(`${lounge ? 'PASS' : 'FAIL'} lounge scene loaded`); ok &&= lounge; }
  const hits = c.log.filter(l => /\[UnityCache\].*(served from the browser cache|revalidated)/.test(l)).length;
  say(`INFO UnityCache served ${hits} files from IndexedDB on visit 2`);
  if (served) {
    const sent = p => served.filter(s => s.phase === p).reduce((a, s) => a + s.bytes, 0);
    say(`INFO server sent body bytes: visit 1 ${(sent(1) / 1048576).toFixed(1)} MB, visit 2 ${(sent(2) / 1048576).toFixed(2)} MB`);
    writeFileSync(join(outDir, 'server.json'), JSON.stringify(served, null, 1));
  }
} catch (e) {
  say('HARNESS ERROR ' + (e.stack || e));
  writeFileSync(join(outDir, 'report.txt'), report.join('\n') + '\n');
  process.exit(2);
}
say(ok ? 'GATE PASS' : 'GATE FAIL');
writeFileSync(join(outDir, 'report.txt'), report.join('\n') + '\n');
for (const d of ['profile', 'http-cache', 'http-cache-1', 'http-cache-2']) rmSync(join(outDir, d), { recursive: true, force: true });
process.exit(ok ? 0 : 1);
