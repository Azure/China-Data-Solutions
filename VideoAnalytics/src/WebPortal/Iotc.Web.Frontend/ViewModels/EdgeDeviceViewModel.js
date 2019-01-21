function EdgeDeviceViewModel() {
    var self = this;

    self.deviceList = [];


    self.drawTable = function () {
        $('#edgeDeviceListTable').DataTable({
            lengthChange: true,
            ordering: false,
            'paging': false,
            'searching': false,
            'info': false,
            'columns': [
                { 'title': 'Id', 'data': 'Id' },
                { 'title': 'Name', 'data': 'Name' },
                { 'title': 'Description', 'data': 'Description' },
                { 'title': 'OSType', 'data': 'OSType' },
                { 'title': 'Capacity', 'data': 'Capacity' },
                { 'title': 'Status', 'data': 'Status' },
                { 'title': 'Delete', 'data': '', 'width': '40px' }
            ],
            data: self.deviceList,
            columnDefs: [
                {
                targets: [-1],
                defaultContent: '<button>Delete</button>'
                }
            ]
        });



        $('#edgeDeviceListTable tbody').on('click', 'button', function (e) {
            var id = $('#edgeDeviceListTable').DataTable().row($(this).parents('tr')).data().Id;
            if ($(this).text() === 'Delete') {
                if (name !== undefined) {
                    $.ajax({
                        method: 'DELETE',
                        url: '/api/EdgeDevices/DeleteEdgeDevice/' + id
                    }).done(function (msg) {

                        $('#edgeDeviceListTable').DataTable().row($(this).parents('tr')).remove().draw(false);

                        }.bind(this)).fail(function (err) {
                        alert('failed!\n' + err.responseJSON);
                    });

                }
            }
        });

    };


    self.UpdateAddSubView = function () {
        console.log('AddSubView');

        $('#AddEdgeDevice-config').hide();
        $('#AddEdgeDevice-AddButton').prop('disabled', false);

    };

    self.updateDeviceList = function () {
        $.ajax({
            method: 'GET',
            url: '/api/EdgeDevices/GetEdgeDevices'
        }).done(function (data) {
            self.deviceList = [];
            $.each(data, function (index, val) {
                self.deviceList.push(val);
            });

            $('#edgeDeviceListTable').DataTable().clear().rows.add(self.deviceList).draw();

        }.bind(this)).fail(function (err) {
            alert('failed!' + err.statusText);
            console.log(err.responseText);
        });
    };

    self.freshStatus = function () {
        console.log('refresh');
        $.ajax({
            method: 'GET',
            url: '/api/EdgeDevices/CheckEdgeDeviceStatus'
        }).done(function (data) {
            self.updateDeviceList();

        }).fail(function (err) {
            alert('freshStatus failed!' + err.statusText);
        });
    };

    self.addDevice = function () {
        location.href = '#EdgeDevice/Add';
    };

    self.addDeviceWithInfo = function () {
        var name = $('#AddEdgeDevice-DeviceName').val();
        var ostype = $('#AddEdgeDevice-OSType').val();
        var capacity = $('#AddEdgeDevice-Capacity').val();
        var description = $('#AddEdgeDevice-DeviceDescription').val();

        if (name === undefined || name === '') {
            alert('Device Name is neccessary!');
        }
        else {

            $.ajax({
                method: 'POST',
                url: '/api/EdgeDevices/AddEdgeDevice',
                data: { deviceName: name, deviceDescription: description, osType: ostype, capacity: capacity }
            }).done(function (msg) {
                if (ostype === 'Windows') {
                    var orgStr = EdgeDeviceConfigTemplateWindows.replace('{device_connection_string}', msg.ConnectString);
                    $('#AddEdgeDevice-config').html(orgStr);
                }
                else {
                    var orgStr = EdgeDeviceConfigTemplateLinux.replace('{device_connection_string}', msg.ConnectString);
                    $('#AddEdgeDevice-config').html(orgStr);
                }

                $('#AddEdgeDevice-config').show();
                $('#AddEdgeDevice-AddButton').prop('disabled', true);

            }).fail(function (err) {
                alert('failed!' + err.responseText);
            });
        }
    };

}



(function () {
    var onLoadCallback = function (viewModel, subview) {
        // All initialization logic goes here
        if (typeof subview === 'undefined') {
            // main view initialization
            viewModel.drawTable();
            viewModel.updateDeviceList();

        } else {
            switch (subview) {
                case 'Add':
                    viewModel.UpdateAddSubView();
                    break;
                default:
                    alert('EdgeDevice - invalid subview initialization: ' + subview);
                    break;
            }
        }
    };

    // Register view with main app view model
    app.registerView('EdgeDevice', {
        displayName: 'Edge Device', icon: 'Image/icons/OPERATION.png', color: '#8F8FE5',
        subviews: [
             { name: 'Add', displayName: 'Add Edge Device' }
        ]
    }, EdgeDeviceViewModel, onLoadCallback);
})();