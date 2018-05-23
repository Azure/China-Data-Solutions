﻿function MonitorViewModel() {
    var self = this;

    // All business logic goes here

    //self.currimg = null;
    //self.presshistory = false;
    self.currTime = null;

    self.selectedDevice = "ViewAll";

    self.selectedRow = null;

    self.setTimer = function () {
        if (self.hometimer === undefined) {
            self.hometimer = setInterval(function () {
                self.updateTable();
            }, 1000);
        }
    };

    self.ClearTimer = function () {
        if (self.hometimer !== undefined) {
            self.hometimer = window.clearInterval(self.hometimer);
        }
    };

    // the unload call back for this view model. this is an interface.
    self.unloadCallback = function (newViewModel, newSubview) {
        self.ClearTimer();
    };

    //self.setProgressBarTimer = function () {
    //    var al = 0;
    //    if (self.sim === undefined) {
    //        self.sim = setInterval(function () {
    //            self.progressBarSim(al);
    //            al += 10;
    //        }, 300);
    //        if (al >= 100) {
    //            self.clearProgressBarTimer();
    //        }
    //    }
    //};

    //self.clearProgressBarTimer = function () {
    //    if (self.sim !== undefined) {
    //        self.sim = window.clearInterval(self.sim);
    //    }
    //};

    //self.progressBarSim = function (al) {
    //    var bar = document.getElementById('progressBar');
    //    bar.value = al;
    //};

    self.updateImage = function () {

        $.getJSON("api/Insight/GetImageData", { time: self.currTime }, function (data) {
            var image1 = document.getElementById('wl1');

            if (image1 !== null) {

                if (data === null) {

                    image1.src = "Image/whitelist/user.png";

                    var blink = document.getElementById('blinkbox');
                    blink.style.display = "none"

                }
                else if (data !== null) {

                    image1.src = "data:image/jpeg;base64," + data;

                }
            }

        });
    };

    //self.updateStatus = function () {
    //    $.getJSON("api/Insight/GetStatus", function (data) {

    //                $.getJSON("api/Insight/GetLastData", function (data) {
    //                    if (data !== null && data.AlertTime !== "") {

    //                        self.updateTable();

    //                        var blink = document.getElementById('blinkbox');
    //                        var st = data.Status;
    //                        if (st === "Alert") {
    //                            blink.style.display = "block";
    //                            blink.style.borderColor = "red";
    //                        } else {
    //                            blink.style.borderColor = "green";
    //                            blink.style.display = "block";
    //                        }
    //                    }
    //                });

    //    });
    //};

    self.MessageList = [];

    self.fillData = function (data, fullRefresh) {
        $.each(data, function (index, msg) {
            self.MessageList.push(
                {
                    time: msg.Time,
                    device: msg.Source,
                    imageUrl: msg.ImageUrl,
                    videoUrl: msg.VideoUrl,
                });
        });
        $('#messageTable').DataTable().clear().rows.add(self.MessageList).draw();

        var selected = $('#messageTable').DataTable().$('tr.selected');
        if (fullRefresh || !self.selectedRow) {
            var image1 = document.getElementById('wl1');

            if (image1 !== null) {
                if (self.MessageList && self.MessageList.length > 0) {
                    image1.src = self.MessageList[0].imageUrl;
                }
                else {
                    image1.src = "Image/whitelist/user.png";
                }
            }
        }
    }

    self.updateTable = function (fullRefresh) {
        self.MessageList = [];
        if (self.selectedDevice !== "ViewAll") {
            $.getJSON("api/Events/GetEventsByCamera/" + self.selectedDevice, function (data) {
                self.fillData(data, fullRefresh);
            });
        }
        else {
            $.getJSON("api/Events/GetEvents", function (data) {
                self.fillData(data, fullRefresh);
            });
        }
    };


    self.drawMessageList = function () {
        self.table = $('#messageTable').DataTable({
            lengthChange: true,
            ordering: false,
            "autoWidth": false,
            "paging": false,
            "searching": false,
            "info": false,
            "columns": [
                { "title": "Alert Time", "data": "time", "width": "20%" },
                { "title": "Device", "data": "device", "width": "20%" },
                { "title": "Video Clip", "data": "videoUrl", "width": "60%" },
                { "title": "Image", "data": "imageUrl", "width": "1%" },
            ],
            data: self.MessageList
        });



        //Once row clicked, load pic
        $('#messageTable tbody').on('click', 'tr', function () {
            if ($(this).hasClass('selected')) {
                $(this).removeClass('selected');
            }
            else {
                $('#messageTable').DataTable().$('tr.selected').removeClass('selected');
                $(this).addClass('selected');
            }

            var imageUrl = $('#messageTable').DataTable().row(this).data().imageUrl;
            var image1 = document.getElementById('wl1');

            self.selectedRow = $('#messageTable').DataTable().row(this).data();
            if (image1 !== null) {
                image1.src = imageUrl;
            }

            var videoURL = $('#messageTable').DataTable().row(this).data().videoUrl;
            console.log(videoURL)

            var newPlayerHTML = '<embed type= "application/x-mplayer2" id="player" autoplay = "true" autostart= "true" loop = "false" controls= "false" allowFullscreen = "false" width = "470" height = "316" src = "' + videoURL + '" />';


            $('#player').remove();
            $('#playbackplayer').html(newPlayerHTML);



            //var time = $('#messageTable').DataTable().row(this).data().time;
            //if (time !== undefined && time !== "") {

            //    $.getJSON("api/Insight/GetHistoryData", { time: time }, function (data) {
            //        if (data !== null) {
            //            var image1 = document.getElementById('wl1');
            //            image1.src = "data:image/jpeg;base64," + data.Image;
            //            var dis = document.getElementById('imgDis');
            //            dis.textContent = "History: " + time;
            //            self.presshistory = true;
            //            self.setTimer();

            //            var blink = document.getElementById('blinkbox');
            //            var st = data.Status;
            //            if (st === "Alert") {
            //                blink.style.display = "block";
            //                blink.style.borderColor = "red";
            //            } else {
            //                blink.style.borderColor = "green";
            //                blink.style.display = "block";
            //            }
            //        }

            //    });
            //}

        });

        //$('#messageTable tbody').on('dblclick', 'tr', function () {
        //    self.selectedDevice = self.table.row(this).data().device;
        //    self.updateTable(true);
        //});

    };



    //self.redrawMessageList = function () {
    //    if ($('#messageTable').DataTable().rows()[0] === undefined)
    //        return;

    //    if ($('#messageTable').DataTable().rows()[0].length > 0)
    //        $('#messageTable').DataTable().clear().draw();
    //    $('#messageTable').DataTable().rows.add(self.MessageList).draw();

    //    $("td:contains('Alert')").css('color', "red");
    //    $("td:contains('Not Alert')").css('color', "green");
    //};


    self.ResetPage = function () {
        $.getJSON("api/Insight/Reset", function () {
            self.updateImage();
            self.updateTable();
        });



    };

    self.bindCameras = function () {
        $('#camera-filter').find('option').remove();
        $('#camera-filter').append('<option selected="selected" value="ViewAll"> View ALL </option>');
        $.getJSON("/api/Cameras/GetCameras", function (data) {
            $.each(data, function (index, item) {
                $('#camera-filter').append($('<option>', { value: item.Name, text: item.Name }));
            });
        });

        $('#camera-filter').change(function (e) {
            $("#camera-filter option:selected").each(function () {
                self.selectedDevice = $(this).val();
                self.updateTable(true);
            });
        });
    };
};

(function () {
    var onLoadCallback = function (viewModel, subview) {
        // All initialization logic goes here
        viewModel.bindCameras();
        viewModel.drawMessageList();
        viewModel.setTimer();

    }

    // Register view with main app view model
    app.registerView("Monitor", { displayName: "Workplace Safety", icon: "Image/icons/HOME.png" }, MonitorViewModel, onLoadCallback);
})();

function clearSelectCamera() {
    app.selectedViewModel.selectedDevice = null;
    app.selectedViewModel.updateTable();
};
