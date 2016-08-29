(function (global) {
    var map = {
        "app": "app",
        "rxjs": "lib/rxjs",
        "@angular": "lib/@angular"
    };

    var packages = {
        "app": { main: "main.js", defaultExtension: "js" },
    };

    var meta = {

    };

    var packageNames = [
        "@angular/common",
        "@angular/compiler",
        "@angular/core",
        "@angular/http",
        "@angular/forms",
        "@angular/platform-browser",
        "@angular/platform-browser-dynamic",
        "@angular/router"
    ];

    packageNames.forEach(function (pkgName) {
        packages[pkgName] = { main: "index.js", defaultExtension: "js" };
        meta[pkgName + "/index.js"] = { build: false };
    });

    System.config({
        baseURL: "/",
        defaultJSExtensions: true,
        packages: packages,
        map: map,
        meta: meta
    });
})(this);
