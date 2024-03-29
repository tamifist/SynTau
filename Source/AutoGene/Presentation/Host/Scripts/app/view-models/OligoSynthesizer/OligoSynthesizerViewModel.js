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
var OligoSynthesizerViewModel = /** @class */ (function (_super) {
    __extends(OligoSynthesizerViewModel, _super);
    function OligoSynthesizerViewModel(contentAreaSelector, currentSynthesisProcessJsonObject, synthesisActivitiesGridSelector) {
        var _this = _super.call(this, contentAreaSelector, currentSynthesisProcessJsonObject) || this;
        _this.synthesisActivitiesGridSelector = synthesisActivitiesGridSelector;
        if (_this.get("Status") == SynthesisProcessStatus.InProgress) {
            _this.set("IsSynthesisNotStarted", false);
            _this.set("IsSynthesisInProgress", true);
            _this.set("IsSynthesisSuspended", false);
        }
        else if (_this.get("Status") == SynthesisProcessStatus.Suspended) {
            _this.set("IsSynthesisNotStarted", false);
            _this.set("IsSynthesisInProgress", false);
            _this.set("IsSynthesisSuspended", true);
        }
        else {
            _this.set("IsSynthesisNotStarted", true);
            _this.set("IsSynthesisInProgress", false);
            _this.set("IsSynthesisSuspended", false);
        }
        return _this;
    }
    OligoSynthesizerViewModel.prototype.startSynthesis = function () {
        var postData = this.toJSON();
        var self = this;
        $.ajax({
            type: "POST",
            url: actions.oligoSynthesizer.startSynthesis,
            dataType: "json",
            traditional: true,
            data: postData,
            success: function (response) {
                if (response.Code == JsonResponseResult.Success) {
                    self.set("IsSynthesisNotStarted", false);
                    self.set("IsSynthesisInProgress", true);
                    self.set("IsSynthesisSuspended", false);
                    self.createSynthesisActivitiesGrid();
                }
                else if (response.Code == JsonResponseResult.Error) {
                    alert("Synthesis cannot be started.");
                }
            },
        });
    };
    OligoSynthesizerViewModel.prototype.stopSynthesis = function () {
        var postData = this.toJSON();
        var self = this;
        $.ajax({
            type: "POST",
            url: actions.oligoSynthesizer.stopSynthesis,
            dataType: "json",
            traditional: true,
            data: postData,
            success: function (response) {
                if (response.Code == JsonResponseResult.Success) {
                    self.set("IsSynthesisNotStarted", false);
                    self.set("IsSynthesisInProgress", false);
                    self.set("IsSynthesisSuspended", true);
                    //self.createSynthesisActivitiesGrid();
                }
                else if (response.Code == JsonResponseResult.Error) {
                    alert("Can not stop synthesis.");
                }
            },
        });
    };
    OligoSynthesizerViewModel.prototype.deleteCurrentSynthesisProcess = function () {
        var postData = this.toJSON();
        var self = this;
        $.ajax({
            type: "POST",
            url: actions.oligoSynthesizer.deleteSynthesisProcess,
            dataType: "json",
            traditional: true,
            data: postData,
            success: function (response) {
                if (response.Code == JsonResponseResult.Success) {
                    window.location.reload();
                    //self.set("IsSynthesisInProgress", false);
                    //self.createSynthesisActivitiesGrid();
                }
                else if (response.Code == JsonResponseResult.Error) {
                    alert("Can not delete synthesis process.");
                }
            },
        });
    };
    OligoSynthesizerViewModel.prototype.createSynthesisActivitiesGrid = function () {
        if (this.synthesisActivitiesGrid) {
            this.synthesisActivitiesGrid.destroy();
            $(this.synthesisActivitiesGridSelector).empty();
        }
        var self = this;
        if (!self.get("IsSynthesisInProgress") && !self.get("IsSynthesisSuspended")) {
            this.synthesisActivitiesGrid = $(this.synthesisActivitiesGridSelector).kendoGrid({
                dataSource: new kendo.data.DataSource({
                    batch: true,
                    pageSize: 10,
                    schema: {
                        model: {
                            id: "Id",
                            fields: {
                                Id: { type: "string", editable: false },
                                ChannelNumber: { type: "number", editable: false },
                                DNASequence: { type: "string", editable: true },
                                SynthesisCycle: { editable: true },
                                TotalTime: { type: "number", editable: false },
                            }
                        }
                    },
                    transport: {
                        read: {
                            url: actions.oligoSynthesizer.synthesisActivityList,
                            type: "Get"
                        },
                        create: {
                            url: actions.oligoSynthesizer.synthesisActivityList,
                            type: "PUT",
                            contentType: 'application/json;charset=utf-8'
                        },
                        update: {
                            url: actions.oligoSynthesizer.synthesisActivityList,
                            type: "PUT",
                            contentType: 'application/json;charset=utf-8'
                        },
                        destroy: {
                            url: actions.oligoSynthesizer.synthesisActivityList,
                            type: "DELETE",
                            contentType: 'application/json;charset=utf-8'
                        },
                        parameterMap: function (data, operation) {
                            if (operation != "read") {
                                return kendo.stringify(data.models);
                            }
                        }
                    },
                    requestEnd: function (e) {
                        if (e.type == "create" || e.type == "update") {
                            this.read();
                            kendo.ui.progress($(self.synthesisActivitiesGridSelector), false);
                        }
                    },
                    change: function (e) {
                        if (e.action == "itemchange") {
                            kendo.ui.progress($(self.synthesisActivitiesGridSelector), true);
                            self.synthesisActivitiesGrid.saveChanges();
                        }
                    }
                }),
                scrollable: true,
                navigatable: true,
                sortable: true,
                columnMenu: true,
                resizable: true,
                selectable: "row",
                editable: true,
                filterable: true,
                pageable: {
                    pageSizes: [10, 20, 50],
                    refresh: true,
                },
                toolbar: [{ name: "create", text: "Add" }],
                columns: [
                    {
                        field: "ChannelNumber",
                        title: "Channel",
                        width: "80px",
                    },
                    {
                        field: "DNASequence",
                        title: "DNA Sequence",
                        width: "350px",
                    },
                    {
                        field: "SynthesisCycle",
                        title: "Synthesis Cycle",
                        width: "120px",
                        editor: this.synthesisCycleDropDownEditor,
                        template: "#=SynthesisCycle.Name#",
                    },
                    {
                        field: "TotalTime",
                        title: "Total Time",
                        width: "100px",
                    },
                    { command: [{ text: "Delete", click: this.deleteSynthesisActivity }], title: " ", width: "110px" },
                ]
            }).data().kendoGrid;
        }
        else {
            this.synthesisActivitiesGrid = $(this.synthesisActivitiesGridSelector).kendoGrid({
                dataSource: new kendo.data.DataSource({
                    batch: true,
                    pageSize: 10,
                    schema: {
                        model: {
                            id: "Id",
                            fields: {
                                Id: { type: "string", editable: false },
                                ChannelNumber: { type: "number", editable: false },
                                DNASequence: { type: "string", editable: false },
                                SynthesisCycle: { editable: false },
                                TotalTime: { type: "number", editable: false },
                            }
                        }
                    },
                    transport: {
                        read: {
                            url: actions.oligoSynthesizer.synthesisActivityList,
                            type: "Get"
                        },
                        create: {
                            url: actions.oligoSynthesizer.synthesisActivityList,
                            type: "PUT",
                            contentType: 'application/json;charset=utf-8'
                        },
                        update: {
                            url: actions.oligoSynthesizer.synthesisActivityList,
                            type: "PUT",
                            contentType: 'application/json;charset=utf-8'
                        },
                        destroy: {
                            url: actions.oligoSynthesizer.synthesisActivityList,
                            type: "DELETE",
                            contentType: 'application/json;charset=utf-8'
                        },
                        parameterMap: function (data, operation) {
                            if (operation != "read") {
                                return kendo.stringify(data.models);
                            }
                        }
                    },
                    requestEnd: function (e) {
                        if (e.type == "create" || e.type == "update") {
                            this.read();
                            kendo.ui.progress($(self.synthesisActivitiesGridSelector), false);
                        }
                    },
                    change: function (e) {
                        if (e.action == "itemchange") {
                            kendo.ui.progress($(self.synthesisActivitiesGridSelector), true);
                            self.synthesisActivitiesGrid.saveChanges();
                        }
                    }
                }),
                scrollable: true,
                navigatable: true,
                sortable: false,
                columnMenu: false,
                resizable: true,
                selectable: "row",
                editable: false,
                filterable: false,
                pageable: {
                    pageSizes: [10, 20, 50],
                    refresh: true,
                },
                columns: [
                    {
                        field: "ChannelNumber",
                        title: "Channel",
                        width: "80px",
                    },
                    {
                        field: "DNASequence",
                        title: "DNA Sequence",
                        width: "350px",
                    },
                    {
                        field: "SynthesisCycle",
                        title: "Synthesis Cycle",
                        width: "120px",
                        editor: this.synthesisCycleDropDownEditor,
                        template: "#=SynthesisCycle.Name#",
                    },
                    {
                        field: "TotalTime",
                        title: "Total Time",
                        width: "100px",
                    },
                ]
            }).data().kendoGrid;
        }
    };
    OligoSynthesizerViewModel.prototype.deleteSynthesisActivity = function (e) {
        e.preventDefault();
        var self = window.oligoSynthesizerViewModel;
        var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
        self.synthesisActivitiesGrid.dataSource.remove(dataItem);
        self.synthesisActivitiesGrid.dataSource.sync();
    };
    OligoSynthesizerViewModel.prototype.synthesisCycleDropDownEditor = function (container, options) {
        $('<input required data-text-field="Name" data-value-field="Id" data-bind="value:' + options.field + '"/>')
            .appendTo(container)
            .kendoDropDownList({
            autoBind: false,
            dataSource: gridDataSource(actions.oligoSynthesizer.synthesisCycleList, 100, "CreatedAt", "desc")
        });
    };
    return OligoSynthesizerViewModel;
}(BaseViewModel));
//# sourceMappingURL=OligoSynthesizerViewModel.js.map