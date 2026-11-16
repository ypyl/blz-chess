// The Stockfish side of the app: one Web Worker, one UCI conversation at a time,
// and one promise per `bestmove`. Blazor never sees UCI.
//
// The worker is started lazily, on the first move the computer has to make, so a
// game against the weakest level may never download the engine at all.

const WORKER_URL = new URL('stockfish-19-lite-single.js', import.meta.url);

let worker = null;
let handshake = null;
let pending = null;
let queue = Promise.resolve();

function start() {
  if (worker) {
    return;
  }

  worker = new Worker(WORKER_URL);
  worker.onmessage = (event) => onLine(String(event.data));
  worker.onerror = (event) => fail(event.message ?? 'the engine failed to load');
}

function fail(message) {
  if (pending) {
    const request = pending;
    pending = null;
    request.reject(new Error(message));
  }

  handshake = null;

  if (worker) {
    worker.terminate();
    worker = null;
  }
}

function onLine(line) {
  if (line.startsWith('bestmove')) {
    const move = line.split(/\s+/)[1];
    if (pending) {
      const request = pending;
      pending = null;
      request.resolve(!move || move === '(none)' ? null : move);
    }
    return;
  }

  if (line === 'readyok' && handshake) {
    const done = handshake;
    handshake = null;
    done();
  }
}

function ready() {
  if (handshake) {
    return new Promise((resolve) => {
      const previous = handshake;
      handshake = () => {
        previous();
        resolve();
      };
    });
  }

  return new Promise((resolve) => {
    handshake = resolve;
    worker.postMessage('uci');
    worker.postMessage('isready');
  });
}

async function play(fen, skillLevel, depth, moveTimeMs) {
  start();
  await ready();

  worker.postMessage(`setoption name Skill Level value ${skillLevel}`);
  worker.postMessage(`position fen ${fen}`);

  const reply = new Promise((resolve, reject) => {
    pending = { resolve, reject };
  });

  worker.postMessage(`go depth ${depth} movetime ${moveTimeMs}`);
  return reply;
}

/// The engine's move in UCI form, or null when the engine has nothing to say.
export function bestMove(fen, skillLevel, depth, moveTimeMs) {
  // One request at a time: a second `go` while the first is running would mix
  // bestmove lines and hand one request another's answer.
  const run = queue.then(() => play(fen, skillLevel, depth, moveTimeMs));
  queue = run.catch(() => {});
  return run;
}

/// Stops the worker and forgets the handshake. Safe to call twice.
export function quit() {
  if (worker) {
    worker.terminate();
    worker = null;
  }

  handshake = null;
  pending = null;
  queue = Promise.resolve();
}
