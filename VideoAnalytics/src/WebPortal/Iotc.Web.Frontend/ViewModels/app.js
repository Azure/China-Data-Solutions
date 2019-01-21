'use strict';

ko.bindingHandlers.FadeInOutText = {
    'update': function (element, valueAccessor) {
        var t = $(element).text();
        var m = ko.unwrap(valueAccessor());
        if (t == m || t===m) {
            return;
        }
        $(element).hide('fade', 300, function () {
            $(element).text(m).show('fade', 150);
        });
    }
}

function showAddListItem(elem, idx, val) {
    if (elem.nodeType === 1) {
        $(elem).hide();
        setTimeout(function() {
            $(elem).fadeIn();
        }, idx * 150);
    }
}

function removeListItem(elem, idx, val) {
    if (elem.nodeType === 1) {
        $(elem).fadeOut().hide();
    }
}

//
// Helpers
//
function loadUrl(url, container, callback) {
    $.ajax({
        type: "GET",
        url: url,
        dataType: "html",
        beforeSend: function () {
            container.html("<h4>Loading...</h4>");
        },
        success: function (data) {
            container.css({
                opacity: '0.0'
            }).html(data).delay(50).animate({
                opacity: '1.0'
            }, 300);

            if (callback) {
                callback();
            }
        },
        error: function () {
            container.html("<h4>Error!</h4>");
        },
        async: false
    });
}

function getInternetExplorerVersion() {
    var rv = -1;
    if (navigator.appName == 'Microsoft Internet Explorer') {
        var ua = navigator.userAgent;
        var re = new RegExp("MSIE ([0-9]{1,}[\.0-9]{0,})");
        if (re.exec(ua) != null)
            rv = parseFloat(RegExp.$1);
    }
    else if (navigator.appName == 'Netscape') {
        var ua = navigator.userAgent;
        var re = new RegExp("Trident/.*rv:([0-9]{1,}[\.0-9]{0,})");
        if (re.exec(ua) != null)
            rv = parseFloat(RegExp.$1);
    }
    return rv;
}

//
// Main application view model
//
function AppViewModel() {
    var self = this;

    self.viewsPath = "";

    // views contains metadata that describes each view (viewName, options, viewModel)
    self.views = ko.observableArray();

    // selectedView is used to highlight the currently selected view in the nav bar
    self.selectedViewName = ko.observable();
    self.selectedViewModel = null; // viewmodel for the currently selected view

    // Display Name is used to show the page title
    self.selectedViewDisplayName = ko.observable();
    self.selectedViewIsSubView = ko.observable();

    // username to show in the main menu bar
    self.username = ko.observable();

    self.GoBack = function () {
        if (self.selectedViewModel && self.selectedViewModel.unloadCallback) {
            self.selectedViewModel.unloadCallback(null, null);
        }
        window.history.back();
    }

    self.registerView = function (viewName, options, viewModel, callback) {
        if (!viewName || typeof (viewName) !== "string")
            throw "invalid viewName";
        if (!viewModel || typeof(viewModel) !== "function")
            throw "invalid viewmodel, must be a constructor";

        self.views.push({
            viewName: viewName,
            options: options,
            viewModel: viewModel,
            callback: callback
        });
    };

    self.loadView = function (viewName) {
        if (!viewName)
            throw "invalid view name";

        var view = $.grep(self.views(), function (o) { return o.viewName === viewName; });
        if (view.length !== 1)
            throw "invalid view name";
        view = view[0];

        // do some clean up before change current view.
        if (self.selectedViewName() != viewName) {
            if (self.selectedViewModel && self.selectedViewModel.unloadCallback) {
                self.selectedViewModel.unloadCallback(viewName, null);
            }
        }
        
        // Update selected view
        self.selectedViewName(viewName);
        self.selectedViewDisplayName(view.options.displayName);
        self.selectedViewIsSubView(false);

        var content = $("#content");
        // Clean up knockout bindings for content div
        ko.cleanNode(content[0]);

        self.selectedViewModel = new view.viewModel();

        // Save reference to viewmodel
        var viewModel = self.selectedViewModel;

        // AJAX the view into content div
        loadUrl(self.viewsPath + "/" + viewName + ".html", content, function() {
            // Apply KO bindings
            ko.applyBindings(viewModel, content[0]);

            // Do some standard jQuery initialization
            // Set up sortable grid
            //$(".sortable").sortable({
            //    revert: 100,
            //    opacity: 0.8,
            //    forcePlaceholderSize: true,
            //    placeholder: "sortable-placeholder"
            //});
            //$(".sortable").disableSelection();

            window.scrollTo(0, 0);

            $(".sidebar-nav a").blur();

            if (window.appInsights) {
                window.appInsights.logPageView(window.location.hostname + "/" + viewName + "/");
            }

            if (view.callback) {
                view.callback(viewModel);
            }
        });
    };

    // TODO: merge this with loadView
    self.loadSubview = function(viewName, subviewName) {
        if (!viewName)
            throw "invalid view name";
        if (!subviewName)
            throw "invalid subview name";

        var view = $.grep(self.views(), function (o) { return o.viewName === viewName; });
        if (view.length !== 1)
            throw "invalid view name";
        view = view[0];

        // It's possible that we are navigating cross-view
        if (self.selectedViewName() !== viewName) {
            if (self.selectedViewModel && self.selectedViewModel.unloadCallback) {
                self.selectedViewModel.unloadCallback(viewName, null);
            }
            self.selectedViewModel = null;
        }

        // Update selected view
        self.selectedViewName(viewName);

        self.selectedViewDisplayName(view.options.displayName);
        if (view.options.subviews) {
            for (var i = 0; i < view.options.subviews.length; i++) {
                var subview = view.options.subviews[i];
                if (subview.name === subviewName) {
                    self.selectedViewDisplayName(subview.displayName);
                    break;
                }
            }
        }
        self.selectedViewIsSubView(true);

        var content = $("#content");
        // Clean up knockout bindings for content div
        ko.cleanNode(content[0]);

        // Save reference to viewmodel
        if (self.selectedViewModel == null) {
            self.selectedViewModel = new view.viewModel();
        }

        // Save reference to viewmodel
        var viewModel = self.selectedViewModel;

        loadUrl(self.viewsPath + "/" + viewName + "-" + subviewName + ".html", content, function () {
            // Apply KO bindings
            ko.applyBindings(viewModel, content[0]);

            // Do some standard jQuery initialization
            // Set up sortable grid
            //$(".sortable").sortable({
            //    revert: 100,
            //    opacity: 0.8,
            //    forcePlaceholderSize: true,
            //    placeholder: "sortable-placeholder"
            //});
            //$(".sortable").disableSelection();

            window.scrollTo(0, 0);

            if (window.appInsights) {
                window.appInsights.logPageView(window.location.hostname + "/" + viewName + "/" + subviewName);
            }
            if (view.callback) {
                view.callback(viewModel, subviewName);
            }
        });
    };

    self.ResetStatus = function () {
        if (self.selectedViewName() == "Monitor") {
          
                self.selectedViewModel.ResetPage();

        }
    }

};

var app = new AppViewModel();

$(document).ready(function () {
    // All view html files should be placed under the Views folder
    app.viewsPath = "views";

      // Apply knockout bindings
    ko.applyBindings(app);

    // Set up routing
    Sammy(function () {
        // See Sammyjs docs for an overview of how routing works
        this.get("#:view", function () {
            app.loadView(this.params.view);
        });

        this.get("#:view/:subview", function () {
            app.loadSubview(this.params.view, this.params.subview);
        });
         
        // Default to first view in views array
        this.get("", function() {
            this.app.runRoute("get", "#" + app.views()[0].viewName);
        });

        if (getInternetExplorerVersion() > -1) {
            //disable TML5 History API pushState since IE doesn't support it well...
            this.disable_push_state = true;
        }
    }).run();
});