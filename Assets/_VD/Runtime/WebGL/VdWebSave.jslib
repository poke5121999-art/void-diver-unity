// Web autosave hooks for PlayerManager (web only, see its WebAutosave comment).
mergeInto(LibraryManager.library, {
  // Calls back when the page is hidden (tab switch, minimise, closing) or left.
  VdOnPageHide: function (callback) {
    var fire = function () { {{{ makeDynCall('v', 'callback') }}}(); };
    document.addEventListener('visibilitychange', function () {
      if (document.visibilityState === 'hidden') fire();
    });
    window.addEventListener('pagehide', fire);
  },

  // IndexedDB writes are asynchronous, and a closing page drops them (seen 2026-11-02: a step saved on pagehide was
  // gone after reopening). localStorage writes are synchronous: every web autosave also copies the save files (*.sav
  // of the save folder, a few KB) there, and VdRestoreSaves puts them back at boot when they are newer.
  VdMirrorSaves__deps: ['$FS'],
  VdMirrorSaves: function (dirPtr) {
    try {
      var dir = UTF8ToString(dirPtr);
      var mirror = { t: Date.now(), files: {} };
      FS.readdir(dir).forEach(function (name) {
        if (!/\.sav$/.test(name)) return;
        var data = FS.readFile(dir + '/' + name), s = '';
        for (var i = 0; i < data.length; i++) s += String.fromCharCode(data[i]);
        mirror.files[dir + '/' + name] = btoa(s);
      });
      localStorage.setItem('vd-save-mirror', JSON.stringify(mirror));
    } catch (e) {
      console.warn('[WebAutosave] copy to localStorage failed: ' + e);
    }
  },

  // At boot, before the game reads its saves: a file of the localStorage copy wins when the file on IndexedDB is
  // missing or more than a second older than the copy. Returns how many files it wrote back (-1 on an error).
  VdRestoreSaves__deps: ['$FS'],
  VdRestoreSaves: function () {
    try {
      var raw = localStorage.getItem('vd-save-mirror');
      if (!raw) return 0;
      var mirror = JSON.parse(raw), restored = 0;
      for (var path in mirror.files) {
        var stale = true;
        try { stale = FS.stat(path).mtime.getTime() < mirror.t - 1000; } catch (e) { stale = true; }
        if (!stale) continue;
        var s = atob(mirror.files[path]), bytes = new Uint8Array(s.length);
        for (var i = 0; i < s.length; i++) bytes[i] = s.charCodeAt(i);
        try { FS.mkdirTree(path.substring(0, path.lastIndexOf('/'))); } catch (e) {}
        FS.writeFile(path, bytes);
        restored++;
      }
      return restored;
    } catch (e) {
      console.warn('[WebAutosave] restore from localStorage failed: ' + e);
      return -1;
    }
  },

  // Writes persistentDataPath to IndexedDB now. With autoSyncPersistentDataPath Unity queues that write on a
  // setTimeout(0) after each changed file, and a page being closed may never run it: start it here, in the event.
  VdPersistNow__deps: ['$IDBFS'],
  VdPersistNow: function () {
    var mount = Module.__unityIdbfsMount && Module.__unityIdbfsMount.mount;
    if (!mount) return;
    if (typeof mount.idbPersistState === 'number' && mount.idbPersistState !== 0) {
      clearTimeout(mount.idbPersistState);
      mount.idbPersistState = 0;
    }
    if (!mount.idbPersistState) {
      mount.idbPersistState = 'idb';
      IDBFS.syncfs(mount, false, function () {
        var again = mount.idbPersistState === 'again';
        mount.idbPersistState = 0;
        if (again) IDBFS.queuePersist(mount);
      });
    } else if (mount.idbPersistState === 'idb') {
      mount.idbPersistState = 'again';
    }
  },
});
