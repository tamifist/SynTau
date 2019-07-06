/// <reference path="../typings/jquery/jquery.d.ts" />
/// <reference path="../typings/kendo/kendo.all.d.ts" />

class Validator {
    private kendoValidator: kendo.ui.Validator;

    constructor(element: JQuery) {
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

    public validate(): boolean {
        return this.kendoValidator.validate();
    }

    private getOtherField(input, attributeName) {
        var otherField = input.attr(attributeName);
        otherField = otherField.substr(otherField.lastIndexOf(".") + 1);
        return $("#" + otherField);
    }
}