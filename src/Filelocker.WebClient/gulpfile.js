/// <binding Clean='clean' />
"use strict";

var gulp = require("gulp"),
    rimraf = require("rimraf"),
    concat = require("gulp-concat"),
    cssmin = require("gulp-cssmin"),
    uglify = require("gulp-uglify"),
    sass = require("gulp-sass"),
    typescript = require("gulp-typescript"),
    sourcemaps = require('gulp-sourcemaps');

var paths = {
    webroot: "./wwwroot/",
    project: "./",
    node_modules: "node_modules/**/*.js"
};

paths.js = paths.webroot + "js/**/*.js";
paths.minJs = paths.webroot + "js/**/*.min.js";
paths.css = paths.webroot + "css/**/*.css";
paths.minCss = paths.webroot + "css/**/*.min.css";
paths.concatJsDest = paths.webroot + "js/site.min.js";
paths.concatCssDest = paths.webroot + "css/site.min.css";
paths.sass = paths.project + "styles/**/*.scss";
paths.typescripts = paths.project + "scripts/**/*.ts";
paths.typescriptTemplates = paths.project + "scripts/**/*.html";

gulp.task("clean:js", function (cb) {
    rimraf(paths.concatJsDest, cb);
});

gulp.task("clean:css", function (cb) {
    rimraf(paths.concatCssDest, cb);
});

gulp.task("clean", ["clean:js", "clean:css"]);

gulp.task("min:js", function () {
    return gulp.src([paths.js, "!" + paths.minJs], { base: "." })
      .pipe(concat(paths.concatJsDest))
      .pipe(uglify())
      .pipe(gulp.dest("."));
});

gulp.task("min:css", function () {
    return gulp.src([paths.css, "!" + paths.minCss])
      .pipe(concat(paths.concatCssDest))
      .pipe(cssmin())
      .pipe(gulp.dest("."));
});

gulp.task("min", ["min:js", "min:css"]);

gulp.task('sass', function () {
    return gulp.src(paths.sass)
      .pipe(sass())
      .pipe(gulp.dest(paths.webroot + 'css'));
});

gulp.task('sass:watch', function () {
    gulp.watch(paths.sass, ['sass']);
});

var tsProject = typescript.createProject(paths.project + 'scripts/tsconfig.json', { typescript: require('typescript') });
gulp.task('typescript', ['typescript:copytemplates'], function (done) {
    var tsResult = gulp.src([paths.typescripts])
        .pipe(sourcemaps.init()) // This means sourcemaps will be generated
        .pipe(tsProject());
    return tsResult.js
        .pipe(sourcemaps.write())
        .pipe(gulp.dest(paths.webroot + '/js'));
});

gulp.task('typescript:copytemplates', ['clean'], function () {
    return gulp.src(paths.typescriptTemplates)
        .pipe(gulp.dest(paths.webroot + '/js'));
});

gulp.task('typescript:watch', function () {
    gulp.watch([paths.typescriptTemplates, paths.typescripts], ['typescript']);
});

//Angular 2 Workflow from NPM
gulp.task("copy:lib", function () {
    return gulp.src(paths.node_modules)
        .pipe(gulp.dest(paths.webroot + '/node_modules'));
});