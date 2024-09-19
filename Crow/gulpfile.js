const gulp = require('gulp');
const browserSync = require('browser-sync').create();
const exec = require('child_process').exec;

// Task to restart ASP.NET Core server when views change
gulp.task('restart-server', function (cb) {
    // Adjust the path to your ASP.NET Core app's .csproj file
    exec('dotnet build', function (err, stdout, stderr) {
        console.log(stdout);
        console.log(stderr);
        cb(err);
    });
});

// Task to serve the ASP.NET Core app using Browser Sync
gulp.task('serve', function () {
    // Start Browser Sync, proxying your ASP.NET Core app
    browserSync.init({
        proxy: "http://localhost:44318", // Your ASP.NET app URL
        files: [
            "wwwroot/**/*.*",         // Static files (CSS, JS, etc.)
            "Views/**/*.cshtml"       // Razor views
        ],
        port: 3000                    // Port for Browser Sync
    });

    // Watch for changes in Razor views
    gulp.watch("Views/**/*.cshtml", gulp.series('restart-server')).on('change', browserSync.reload);

    // Watch for changes in static files
    gulp.watch("wwwroot/**/*.*").on('change', browserSync.reload);
});

// Default task to run when calling `gulp`
gulp.task('default', gulp.series('serve'));
