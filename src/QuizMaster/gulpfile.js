var gulp = require("gulp");
var del = require("del");
var fs = require("fs");
var runSequence = require("run-sequence");

var Config = (function () {
    this.paths = {};
    this.paths.web = { root: "./wwwroot" };

    this.paths.src = {
        npm: "./node_modules"
    };

    this.paths.dest = {
        lib: this.paths.web.root + "/lib"
    };

    return {
        nodePackagePaths: ["@angular", "bootstrap/dist", "core-js", "jquery", "jquery-validation", "jquery-validation-unobtrusive", "progressbar.js", "reflect-metadata", "rxjs", "systemjs/dist", "toastr/build", "zone.js/dist"],
        paths: this.paths
    };
})();

gulp.task("all", function () {
    runSequence("clean", ["lib", "build"]);
});

gulp.task("build", function () {

});

gulp.task("clean", function () {
    return del([Config.paths.dest.lib + "/**/*"]);
});

gulp.task("lib", function () {
    Config.nodePackagePaths.forEach(function (packagePath) {
        gulp.src(Config.paths.src.npm + "/" + packagePath + "/**/*").pipe(gulp.dest(Config.paths.dest.lib + "/" + packagePath));
    });
});