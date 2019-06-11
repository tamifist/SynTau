/// <reference path="../../../typings/jquery/jquery.d.ts" />
/// <reference path="../../../typings/kendo/kendo.all.d.ts" />
/// <reference path="../../../typings/autogene/autogene.d.ts" />
var __extends = (this && this.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
        return extendStatics(d, b);
    };
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
var ManualControlViewModel = /** @class */ (function (_super) {
    __extends(ManualControlViewModel, _super);
    function ManualControlViewModel(contentAreaSelector, manualControlJsonObject, valvesMultiSelectSelector, activatedValvesGridSelector) {
        var _this = _super.call(this, contentAreaSelector, manualControlJsonObject) || this;
        _this.valvesMultiSelectSelector = valvesMultiSelectSelector;
        _this.activatedValvesGridSelector = activatedValvesGridSelector;
        _this.set("IsSyringePumpVolumeVisible", false);
        if (_this.get("IsValvesActivated")) {
            _this.createActivatedValvesGrid();
        }
        return _this;
    }
    ManualControlViewModel.prototype.syringePumpFunctionChanged = function (e) {
        if (this.get("SelectedSyringePumpFunction") == HardwareFunctionType.SyringePumpDraw) {
            this.set("IsSyringePumpVolumeVisible", true);
        }
        else {
            this.set("IsSyringePumpVolumeVisible", false);
        }
    };
    ManualControlViewModel.prototype.setSyringePump = function () {
        var postData = this.toJSON();
        var self = this;
        //save the changes
        $.ajax({
            type: "POST",
            url: actions.manualControl.setSyringePump,
            dataType: "json",
            traditional: true,
            data: postData,
            success: function (response) {
                if (response.Code == JsonResponseResult.Success) {
                    alert("syringe pump set");
                }
                else if (response.Code == JsonResponseResult.Error) {
                    alert("Failed to set syringe pump: " + response.Data);
                }
            }
        });
    };
    ManualControlViewModel.prototype.execGSValveFunction = function () {
        var postData = this.toJSON();
        var self = this;
        //save the changes
        $.ajax({
            type: "POST",
            url: actions.manualControl.execGSValveFunction,
            dataType: "json",
            traditional: true,
            data: postData,
            success: function (response) {
                if (response.Code == JsonResponseResult.Success) {
                }
                else if (response.Code == JsonResponseResult.Error) {
                    alert("Error: " + response.Data);
                }
            }
        });
    };
    ManualControlViewModel.prototype.createActivatedValvesGrid = function () {
        if (!this.activatedValvesGrid) {
            this.activatedValvesGrid = $(this.activatedValvesGridSelector).kendoGrid({
                dataSource: gridDataSource(actions.manualControl.activatedValvesList, 10, "CreatedAt", "desc"),
                sortable: true,
                scrollable: true,
                filterable: true,
                resizable: true,
                selectable: "row",
                pageable: {
                    pageSizes: [10, 20, 50],
                    refresh: true,
                },
                columns: [
                    { field: "Name", width: "500px" },
                ],
            }).data().kendoGrid;
        }
        else {
            this.activatedValvesGrid.dataSource.read();
        }
    };
    ManualControlViewModel.prototype.activateValves = function () {
        var selectedValves = $(this.valvesMultiSelectSelector).data("kendoMultiSelect").dataItems();
        if (selectedValves && selectedValves.length > 0) {
            var self = this;
            $.ajax({
                url: actions.manualControl.activateValves,
                type: "POST",
                dataType: "json",
                contentType: "application/json",
                data: JSON.stringify(selectedValves),
                success: function (response) {
                    if (response.Code == JsonResponseResult.Success) {
                        //debugger;
                        self.set("IsValvesActivated", true);
                        self.createActivatedValvesGrid();
                    }
                    else if (response.Code == JsonResponseResult.Error) {
                        alert("Error");
                    }
                },
            });
        }
    };
    ManualControlViewModel.prototype.deactivateAllValves = function () {
        var self = this;
        $.ajax({
            url: actions.manualControl.deactivateAllValves,
            type: "POST",
            dataType: "json",
            contentType: "application/json",
            success: function (response) {
                if (response.Code == JsonResponseResult.Success) {
                    self.set("IsValvesActivated", false);
                }
                else if (response.Code == JsonResponseResult.Error) {
                    alert("Error");
                }
            },
        });
    };
    ManualControlViewModel.prototype.trayOut = function () {
        var self = this;
        $.ajax({
            url: actions.manualControl.trayOut,
            type: "POST",
            dataType: "json",
            contentType: "application/json",
            success: function (response) {
                if (response.Code == JsonResponseResult.Success) {
                }
                else if (response.Code == JsonResponseResult.Error) {
                    alert("Error");
                }
            },
        });
    };
    ManualControlViewModel.prototype.trayIn = function () {
        var self = this;
        $.ajax({
            url: actions.manualControl.trayIn,
            type: "POST",
            dataType: "json",
            contentType: "application/json",
            success: function (response) {
                if (response.Code == JsonResponseResult.Success) {
                }
                else if (response.Code == JsonResponseResult.Error) {
                    alert("Error");
                }
            },
        });
    };
    ManualControlViewModel.prototype.trayLightOn = function () {
        var self = this;
        $.ajax({
            url: actions.manualControl.trayLightOn,
            type: "POST",
            dataType: "json",
            contentType: "application/json",
            success: function (response) {
                if (response.Code == JsonResponseResult.Success) {
                }
                else if (response.Code == JsonResponseResult.Error) {
                    alert("Error");
                }
            },
        });
    };
    ManualControlViewModel.prototype.trayLightOff = function () {
        var self = this;
        $.ajax({
            url: actions.manualControl.trayLightOff,
            type: "POST",
            dataType: "json",
            contentType: "application/json",
            success: function (response) {
                if (response.Code == JsonResponseResult.Success) {
                }
                else if (response.Code == JsonResponseResult.Error) {
                    alert("Error");
                }
            },
        });
    };
    return ManualControlViewModel;
}(BaseViewModel));
//# sourceMappingURL=ManualControlViewModel.js.map