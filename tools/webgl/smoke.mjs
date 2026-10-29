// node tools/webgl/smoke.mjs <build dir> <out dir> [seconds=240]
// Serves a WebGL build, opens it in headless Chrome (SwiftShader GL) and records what the player does: every console
// line and uncaught exception to <out>/console.log, and a screenshot every 20 s to <out>/shot-<s>.png. It drives no
// input; it answers "does the web player boot, and how far does it get on its own".
import { spawn } from "node:child_process";
import { createServer } from "node:http";
import { mkdirSync, writeFileSync, appendFileSync, readFileSync, statSync } from "node:fs";
import { join, extname, normalize } from "node:path";
import { tmpdir } from "node:os";

const [buildDir, outDir, secondsArg] = process.argv.slice(2);
if (!buildDir || !outDir) {
  console.error("usage: node tools/webgl/smoke.mjs <build dir> <out dir> [seconds]");
  process.exit(2);
}
const seconds = Number(secondsArg ?? 240);
mkdirSync(outDir, { recursive: true });
const logPath = join(outDir, "console.log");
writeFileSync(logPath, "");
const t0 = Date.now();
const log = (line) => {
  const s = `${((Date.now() - t0) / 1000).toFixed(1).padStart(6)}s ${line}`;
  appendFileSync(logPath, s + "\n");
  console.log(s);
};

const types = { ".html": "text/html", ".js": "application/javascript", ".wasm": "application/wasm", ".json": "application/json",
  ".css": "text/css", ".png": "image/png", ".ico": "image/x-icon" };
const server = createServer((req, res) => {
  const path = normalize(decodeURIComponent(req.url.split("?")[0])).replace(/^(\.\.[/\\])+/, "");
  let file = join(buildDir, path === "/" ? "index.html" : path);
  try {
    if (statSync(file).isDirectory()) file = join(file, "index.html");
    const body = readFileSync(file);
    // No Content-Encoding for .br: the build's decompression fallback unpacks them in JavaScript, as on a static host.
    const ext = extname(file.replace(/\.br$/, ""));
    res.writeHead(200, { "Content-Type": types[ext] ?? "application/octet-stream" });
    res.end(body);
  } catch {
    log(`HTTP 404 ${req.url}`);
    res.writeHead(404);
    res.end();
  }
});
await new Promise((r) => server.listen(0, "127.0.0.1", r));
const url = `http://127.0.0.1:${server.address().port}/`;

const port = 9300 + Math.floor(Math.random() * 600);
const chrome = spawn("google-chrome", [
  "--headless=new", `--remote-debugging-port=${port}`, `--user-data-dir=${join(tmpdir(), `vd-smoke-${port}`)}`,
  "--use-angle=swiftshader", "--enable-unsafe-swiftshader", "--no-first-run", "--no-default-browser-check",
  "--window-size=1280,720", "--autoplay-policy=no-user-gesture-required", "about:blank",
], { stdio: "ignore" });

let target;
for (let i = 0; i < 50 && !target; i++) {
  await new Promise((r) => setTimeout(r, 200));
  try {
    const list = await (await fetch(`http://127.0.0.1:${port}/json/list`)).json();
    target = list.find((t) => t.type === "page");
  } catch { /* not up yet */ }
}
if (!target) {
  log("chrome did not expose a page target");
  chrome.kill();
  server.close();
  process.exit(1);
}

const ws = new WebSocket(target.webSocketDebuggerUrl);
await new Promise((r) => ws.addEventListener("open", r));
let nextId = 1;
const pending = new Map();
const send = (method, params = {}) => new Promise((resolve) => {
  const id = nextId++;
  pending.set(id, resolve);
  ws.send(JSON.stringify({ id, method, params }));
});
ws.addEventListener("message", (ev) => {
  const msg = JSON.parse(ev.data);
  if (msg.id && pending.has(msg.id)) {
    pending.get(msg.id)(msg.result ?? msg.error);
    pending.delete(msg.id);
  } else if (msg.method === "Runtime.consoleAPICalled") {
    const text = msg.params.args.map((a) => a.value ?? a.description ?? a.type).join(" ");
    log(`console.${msg.params.type} ${text}`);
  } else if (msg.method === "Runtime.exceptionThrown") {
    const d = msg.params.exceptionDetails;
    log(`EXCEPTION ${d.exception?.description ?? d.text}`);
  } else if (msg.method === "Log.entryAdded") {
    log(`browser.${msg.params.entry.level} ${msg.params.entry.text} ${msg.params.entry.url ?? ""}`);
  }
});
await send("Runtime.enable");
await send("Log.enable");
await send("Page.enable");
log(`open ${url}`);
await send("Page.navigate", { url });

for (let s = 20; s <= seconds; s += 20) {
  await new Promise((r) => setTimeout(r, 20000));
  const shot = await send("Page.captureScreenshot", { format: "png" });
  if (shot?.data) writeFileSync(join(outDir, `shot-${String(s).padStart(3, "0")}.png`), Buffer.from(shot.data, "base64"));
}
log("done");
ws.close();
chrome.kill();
server.close();
process.exit(0);
