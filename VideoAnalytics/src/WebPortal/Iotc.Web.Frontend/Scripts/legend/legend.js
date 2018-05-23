// draw legend bar.
(function () {
    var c = document.getElementById("legendbar");
    var ctx = c.getContext("2d");
    var grd = ctx.createLinearGradient(0, 0, 150, 0);
    grd.addColorStop(1, "#C8605B");     // red
    grd.addColorStop(0.5, "#829269");   // gray
    grd.addColorStop(0, "#40C178");     // green
    ctx.fillStyle = grd;
    ctx.fillRect(0, 0, 200, 12);
})();