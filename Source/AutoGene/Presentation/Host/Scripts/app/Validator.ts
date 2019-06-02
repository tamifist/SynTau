/// <reference path="../typings/jquery/jquery.d.ts" />
/// <reference path="../typings/kendo/kendo.all.d.ts" />

class Validator {
    private kendoValidator: kendo.ui.Validator;

    constructor(element: JQuery) {
        var self = this;

        var options = {
            rules: {
                equalto: function (input) {
                    if (input.filter("[data-val-equalto-other]").length) {
                        var otherField = self.getOtherField(input, "data-val-equalto-other");
                        return input.val() == otherField.val();
                    }
                    return true;
                },
            },
            messages: {
                equalto: function (input) {
                    return input.attr("data-val-equalto");
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