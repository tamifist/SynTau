/// <reference path="../typings/jquery/jquery.d.ts" />
/// <reference path="../typings/kendo/kendo.all.d.ts" />
var Validator = /** @class */ (function () {
    function Validator(element) {
        var self = this;
        var options = {
            rules: {
                minlength: function (input) {
                    if (input.filter("[data-minlength]").length) {
                        var minLength = input.attr("data-minlength");
                        return input.val().length >= minLength;
                    }
                    return true;
                },
                equalto: function (input) {
                    if (input.filter("[data-equalto]").length) {
                        var otherField = self.getOtherField(input, "data-equalto");
                        return input.val() == otherField.val();
                    }
                    return true;
                },
            },
            messages: {
                minlength: function (input) {
                    return input.attr("data-minlength-msg");
                },
                equalto: function (input) {
                    return input.attr("data-equalto-msg");
                },
            }
        };
        this.kendoValidator = element.kendoValidator(options).data("kendoValidator");
    }
    Validator.prototype.validate = function () {
        return this.kendoValidator.validate();
    };
    Validator.prototype.getOtherField = function (input, attributeName) {
        var otherField = input.attr(attributeName);
        otherField = otherField.substr(otherField.lastIndexOf(".") + 1);
        return $("#" + otherField);
    };
    return Validator;
}());
//# sourceMappingURL=Validator.js.map