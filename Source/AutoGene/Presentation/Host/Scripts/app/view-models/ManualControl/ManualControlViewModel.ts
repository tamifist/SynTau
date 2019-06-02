/// <reference path="../../../typings/jquery/jquery.d.ts" />
/// <reference path="../../../typings/kendo/kendo.all.d.ts" />
/// <reference path="../../../typings/autogene/autogene.d.ts" />

class ManualControlViewModel extends BaseViewModel {
    private valvesMultiSelectSelector: string;

    private activatedValvesGridSelector: string;
    private activatedValvesGrid: kendo.ui.Grid;

    constructor(contentAreaSelector: string, manualControlJsonObject: JSON, valvesMultiSelectSelector: string, activatedValvesGridSelector: string) {
        super(contentAreaSelector, manualControlJsonObject);

        this.valvesMultiSelectSelector = valvesMultiSelectSelector;
        this.activatedValvesGridSelector = activatedValvesGridSelector;

        this.set("IsSyringePumpVolumeVisible", false);

        if (this.get("IsValvesActivated")) {
            this.createActivatedValvesGrid();
        }
    }

    public syringePumpFunctionChanged(e: any) {

        if (this.get("SelectedSyringePumpFunction") == HardwareFunctionType.SyringePumpDraw) {
            this.set("IsSyringePumpVolumeVisible", true);
        } else {
            this.set("IsSyringePumpVolumeVisible", false);
        }
    }

    public setSyringePump(): void {

        var postData: any = <any>this.toJSON();

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

                } else if (response.Code == JsonResponseResult.Error) {
                    alert("Failed to set syringe pump: " + response.Data);
                }
            }
        });
    }

    public execGSValveFunction(): void {

        var postData: any = <any>this.toJSON();

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

                } else if (response.Code == JsonResponseResult.Error) {
                    alert("Error: " + response.Data);
                }
            }
        });
    }

    public createActivatedValvesGrid() {
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
        } else {
            this.activatedValvesGrid.dataSource.read();
        }
    }

    public activateValves() {
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
                    } else if (response.Code == JsonResponseResult.Error) {
                        alert("Error");
                    }
                },
            });
        }
    }

    public deactivateAllValves() {
        var self = this;
        $.ajax({
            url: actions.manualControl.deactivateAllValves,
            type: "POST",
            dataType: "json",
            contentType: "application/json",
            success: function (response) {
                if (response.Code == JsonResponseResult.Success) {
                    self.set("IsValvesActivated", false);
                } else if (response.Code == JsonResponseResult.Error) {
                    alert("Error");
                }
            },
        });
    }

    public trayOut() {
        var self = this;
        $.ajax({
            url: actions.manualControl.trayOut,
            type: "POST",
            dataType: "json",
            contentType: "application/json",
            success: function (response) {
                if (response.Code == JsonResponseResult.Success) {
                } else if (response.Code == JsonResponseResult.Error) {
                    alert("Error");
                }
            },
        });
    }

    public trayIn() {
        var self = this;
        $.ajax({
            url: actions.manualControl.trayIn,
            type: "POST",
            dataType: "json",
            contentType: "application/json",
            success: function (response) {
                if (response.Code == JsonResponseResult.Success) {
                } else if (response.Code == JsonResponseResult.Error) {
                    alert("Error");
                }
            },
        });
    }

    public trayLightOn() {
        var self = this;
        $.ajax({
            url: actions.manualControl.trayLightOn,
            type: "POST",
            dataType: "json",
            contentType: "application/json",
            success: function (response) {
                if (response.Code == JsonResponseResult.Success) {
                } else if (response.Code == JsonResponseResult.Error) {
                    alert("Error");
                }
            },
        });
    }

    public trayLightOff() {
        var self = this;
        $.ajax({
            url: actions.manualControl.trayLightOff,
            type: "POST",
            dataType: "json",
            contentType: "application/json",
            success: function (response) {
                if (response.Code == JsonResponseResult.Success) {
                } else if (response.Code == JsonResponseResult.Error) {
                    alert("Error");
                }
            },
        });
    }
} 