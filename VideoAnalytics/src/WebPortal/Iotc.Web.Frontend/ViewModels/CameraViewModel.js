function CameraViewModel() {

    self = this;

    self.drawMessageList = function () {
        self.table = $('#Camera-messageTable').DataTable({
            lengthChange: true,
            ordering: false,
            "paging": false,
            "searching": false,
            "info": false,
            "columns": [
                { "title": "Id", "data": "id", "width": "40px" },
                { "title": "Name", "data": "name" },
                { "title": "Pipeline", "data": "pipeline" },
                { "title": "Device", "data": "device" },
                { "title": "Stream", "data": "steam" },
                { "title": "Status", "data": "", "width": "40px" },
                { "title": "Delete", "data": "", "width": "40px" },
            ],
            columnDefs: [{
                // puts a button in the last column
                targets: [-2],
                render: function (a, b, data, d) {
                    if (data.status === 'NotActive') {
                        return "<button>Activate</button>";
                    }
                    else {
                        return "<button>Deactivate</button>";
                    }
                }
            },
                {
                    targets: [-1],
                    render: function (a, b, data, d) {
                        if (data.status === 'NotActive') {
                            return "<button>Delete</button>";
                        }
                        else {
                            return "<button disabled='disabled'>Delete</button>";
                        }
                    }
            }],

            data: self.MessageList,
            "drawCallback": function (settings) {

            }
        });

        $('#Camera-messageTable tbody').on('click', 'button', function (e) {
            var id = self.table.row($(this).parents('tr')).data().id;
            if ($(this).text() === 'Delete') {
                var status = self.table.row($(this).parents('tr')).data().status;
                if (status === "NotActive") {
                    self.table.row($(this).parents('tr')).remove().draw(false);
                    self.deleteCamera(id);
                }
                else {
                    alert("You must firstly deactivate this camera in order to delete");
                }
            }else if ($(this).text() === 'Activate'){
                $.ajax({
                    method: "POST",
                    url: "/api/Cameras/Activate/"+id,
                }).done(function (msg) {
                    self.updateTable();
                    alert("active sucess");

                    }).fail(function (err) {
                        alert("active failed!" + err.responseText);
                });
            } else if ($(this).text() === 'Deactivate') {
                $.ajax({
                    method: "POST",
                    url: "/api/Cameras/Deactivate/" + id,
                }).done(function (msg) {
                    self.updateTable();
                    alert("Deactivate sucess")

                }).fail(function (err) {
                    alert("Deactivate failed!" + err.responseText);
                });
            }
        });

    }

    self.loadTableData = function () {

    }

    self.updateTable = function () {
        self.MessageList = [];
        $.getJSON("api/Cameras/GetCameras", function (data) {

            try {
                $.each(data, function (index, item) {
                    self.MessageList.push(
                        {
                            id: item.Id,
                            name: item.Name,
                            pipeline: item.Pipeline,
                            device: item.HostingDevice,
                            steam: item.Stream,
                            status: item.Status
                        });
                });
                if (self.table) {
                    self.table.destroy();
                }

                self.drawMessageList();
            }
            catch (e) {
                alert(e);
            }



        });
    };

    self.deleteCamera = function (id) {
        $.ajax({
            method: "DELETE",
            url: "/api/Cameras/DeleteCamera/" + id
        }).done(function (msg) {
            window.location = "#Camera";
        }).fail(function (err) {
            alert("failed!" + err.responseText);
        });
    }

    self.bindDevices = function () {
        $.getJSON("/api/EdgeDevices/GetEdgeDevices", function (data) {
            $.each(data, function (index, item) {
                $('#addcamera-device').append($('<option>', { value: item.Id, text: item.Name + "(" + item.OSType + ")" }))
            });
        })
    }
};

function AddCamera() {
    var name = $('#addcamera-name').val();
    var desc = $('#addcamera-desc').val();
    var stream = $('#addcamera-stream').val();
    var width = $('#addcamera-width').val();
    var height = $('#addcamera-height').val();
    var fps = $('#addcamera-fps').val();
    var pipeline = $('#addcamera-pipeline').val();
    var device = $('#addcamera-device').val();

    var message = validateStr(name, "Camera Name");
    message += validateStr(stream, "Video Stream");
    message += validateStr(width, "Width");
    message += validateStr(height, "Height");
    message += validateStr(fps, "Frame Rate");
    message += validateStr(pipeline, "Video Analytics Pipeline");
    message += validateStr(device, "Hosting Device");

    if (message && message.length > 0) {
        alert(message);
    }

    var camera = {
        Name: name,
        Description: desc,
        Stream: stream,
        Pipeline: pipeline, 
        HostingDevice: device,
        Width: parseInt(width),
        Height: parseInt(height),
        FPS: parseInt(fps), 
        Status: 'NotActive'
    };

        $.ajax({
            method: "POST",
            url: "/api/Cameras/PostCamera",
            data: camera
        }).done(function (msg) {
            window.location = "#Camera";
        }).fail(function (err) {
            alert("failed!" + err.responseText);
        });
}

function validateStr(str, name) {
    if (!str || str.length === 0) {
        return name + " is neccessary!\n";
    }
}

(function () {
    var onLoadCallback = function (viewModel, subview) {

        if (subview === 'add') {
            viewModel.bindDevices()
        }
        else {
            viewModel.updateTable();
        }
    }

    // Register view with main app view model
    app.registerView("Camera", { displayName: "Camera", icon: "Image/icons/report.png" }, CameraViewModel, onLoadCallback);
})();