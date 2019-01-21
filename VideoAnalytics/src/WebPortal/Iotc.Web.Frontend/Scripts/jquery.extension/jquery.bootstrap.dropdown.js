// jQuery extension for knockout Dropdown

/*
-- HTML Template :

<div class="btn-group" id="someid" style="margin-bottom: 2pt">
    <button type="button" class="btn btn-default dropdown-toggle" data-toggle="dropdown">
        <span style="margin-right: 10pt">Loading...</span><span class="caret"></span>
    </button>
    <ul class="dropdown-menu" role="menu" data-bind="foreach: KnockoutObsevalbeArray">
        <li><a data-bind="attr: {value: value}, text: text"></a></li>
    </ul>
</div>


-- Javascript Knockout Binding:

self.KnockoutObsevalbeArray = ko.observableArray();

$.each(someAjaxData, function(index, item) {
    self.KnockoutObsevalbeArray.push({ text: item, value: item });
});


-- User selection changed:

$('#someid').AsDropdown().change(function () {
    var newValue = $('#someid').AsDropdown().val();
    --or--
    var newValue = this.val();
});


-- change selection:
    $('#someid').AsDropdown().val('some value');
});

*/

(function ($) {
    var BootStrapDropdown = function(element) {
        this.target = $(element);
    };

    $.extend(BootStrapDropdown.prototype, {
        val: function(val) {
            var buttonText = $('.btn span:first-child', this.target);
            if (buttonText.length == 0) {
                return (typeof val == 'undefined') ? "" : this.target;
            }

            if (typeof val == 'undefined') {
                return buttonText.attr('value');
            } else {
                $('ul li a', this.target).each(function (index, item) {
                    var i = $(item);
                    if (i.attr('value') == val) {
                        buttonText.attr('value', val);
                        buttonText.text(i.text());
                    }
                });
                return this.target;
            }
        },

        text: function () {
            var buttonText = $('.btn span:first-child', this.target);
            return buttonText.length == 0 ? "" : buttonText.text();
        },

        change: function (callback) {
            var ul = $('ul', this.target);
            if (ul.length > 0) {
                var self = this;
                $('ul', this.target).on('click', 'li a', function () {
                    self.val($(this).attr('value'));
                    callback.call(self);
                });
            }
            return this;
        },


        toggle: function (flag) {
            var button = $('.btn', this.target);
            if (button.length > 0) {
                if (typeof flag == 'undefined') {
                    if (button.hasClass('disabled')) {
                        button.removeClass('disabled');
                    } else {
                        button.addClass('disabled');
                    }
                } else if (flag) {
                    button.removeClass('disabled');
                } else {
                    button.addClass('disabled');
                }
            }
            return this;
        }
    });

    $.fn.AsDropdown = function() {
        return new BootStrapDropdown(this);
    };
})(jQuery);


