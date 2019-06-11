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
var CycleEditorViewModel = /** @class */ (function (_super) {
    __extends(CycleEditorViewModel, _super);
    function CycleEditorViewModel(contentAreaSelector, cycleEditorJsonObject, cycleStepsGridSelector, synthesisCyclesGridSelector) {
        var _this = _super.call(this, contentAreaSelector, cycleEditorJsonObject) || this;
        _this.cycleStepsGridSelector = cycleStepsGridSelector;
        _this.synthesisCyclesGridSelector = synthesisCyclesGridSelector;
        return _this;
    }
    CycleEditorViewModel.prototype.createSynthesisCyclesGrid = function () {
        if (!this.synthesisCyclesGrid) {
            var self = this;
            this.synthesisCyclesGrid = $(this.synthesisCyclesGridSelector).kendoGrid({
                dataSource: new kendo.data.DataSource({
                    batch: true,
                    pageSize: 10,
                    schema: {
                        model: {
                            id: "Id",
                            fields: {
                                Id: { type: "string", editable: false },
                                Name: { type: "string", editable: true },
                                TotalSteps: { type: "number", editable: false },
                                TotalTime: { type: "number", editable: false },
                            }
                        }
                    },
                    transport: {
                        read: {
                            url: actions.cycleEditor.synthesisCycleList,
                            type: "Get"
                        },
                        create: {
                            url: actions.cycleEditor.synthesisCycleList,
                            type: "PUT",
                            contentType: 'application/json;charset=utf-8'
                        },
                        update: {
                            url: actions.cycleEditor.synthesisCycleList,
                            type: "PUT",
                            contentType: 'application/json;charset=utf-8'
                        },
                        destroy: {
                            url: actions.cycleEditor.synthesisCycleList,
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
                            kendo.ui.progress($(self.synthesisCyclesGridSelector), false);
                        }
                    },
                    change: function (e) {
                        if (e.action == "itemchange") {
                            kendo.ui.progress($(self.synthesisCyclesGridSelector), true);
                            self.synthesisCyclesGrid.saveChanges();
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
                toolbar: [{ name: "create", text: "Create New Cycle" }],
                columns: [
                    { field: "Name", title: "Name", width: "500px" },
                    { field: "TotalSteps", title: "Total Steps", width: "100px" },
                    { field: "TotalTime", title: "Total Time", width: "100px" },
                    { command: [{ text: { edit: "Edit" }, click: this.editSynthesisCycle }, { text: { edit: "Delete" }, click: this.deleteSynthesisCycle }], title: " ", width: "110px" },
                ]
            }).data().kendoGrid;
        }
        else {
            this.synthesisCyclesGrid.dataSource.transport.options.read.url = actions.cycleEditor.synthesisCycleList;
            this.synthesisCyclesGrid.dataSource.read();
        }
    };
    CycleEditorViewModel.prototype.editSynthesisCycle = function (e) {
        e.preventDefault();
        var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
        var self = window.cycleEditorViewModel;
        self.set("SynthesisCycleId", dataItem.Id);
        self.set("SynthesisCycleName", dataItem.Name);
        self.setMode(false, true);
        self.createCycleStepsGrid(dataItem.Id);
    };
    CycleEditorViewModel.prototype.deleteSynthesisCycle = function (e) {
        e.preventDefault();
        var self = window.cycleEditorViewModel;
        var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
        self.synthesisCyclesGrid.dataSource.remove(dataItem);
        self.synthesisCyclesGrid.dataSource.sync();
    };
    CycleEditorViewModel.prototype.createCycleStepsGrid = function (synthesisCycleId) {
        var self = this;
        if (!this.cycleStepsGrid) {
            this.cycleStepsGrid = $(this.cycleStepsGridSelector).kendoGrid({
                dataSource: new kendo.data.DataSource({
                    batch: true,
                    pageSize: 10,
                    schema: {
                        model: {
                            id: "CycleStepId",
                            fields: {
                                CycleStepId: { type: "string", editable: false },
                                SynthesisCycleId: { type: "string", editable: false },
                                HardwareFunctionId: { type: "string", editable: false },
                                StepNumber: { type: "number", editable: true },
                                FunctionNumber: { type: "number", editable: false },
                                //FunctionName: { type: "string", editable: true },
                                HardwareFunction: { editable: true },
                                StepTime: { type: "number", editable: true },
                                A: { type: "boolean", editable: true },
                                G: { type: "boolean", editable: true },
                                C: { type: "boolean", editable: true },
                                T: { type: "boolean", editable: true },
                                Five: { type: "boolean", editable: true },
                                Six: { type: "boolean", editable: true },
                                Seven: { type: "boolean", editable: true },
                                SafeStep: { type: "boolean", editable: true },
                            }
                        }
                    },
                    transport: {
                        read: {
                            url: actions.cycleEditor.cycleStepList + "/?synthesisCycleId=" + synthesisCycleId,
                            type: "Get"
                        },
                        create: {
                            url: actions.cycleEditor.cycleStepList,
                            type: "PUT",
                            contentType: 'application/json;charset=utf-8'
                        },
                        update: {
                            url: actions.cycleEditor.cycleStepList,
                            type: "PUT",
                            contentType: 'application/json;charset=utf-8'
                        },
                        destroy: {
                            url: actions.cycleEditor.cycleStepList,
                            type: "DELETE",
                            contentType: 'application/json;charset=utf-8'
                        },
                        parameterMap: function (data, operation) {
                            if (operation != "read") {
                                for (var i = 0; i < data.models.length; i++) {
                                    data.models[i].SynthesisCycleId = self.get("SynthesisCycleId");
                                }
                                return kendo.stringify(data.models);
                            }
                        }
                    },
                    requestEnd: function (e) {
                        if (e.type == "create" || e.type == "update" || e.type == "destroy") {
                            this.read();
                            kendo.ui.progress($(self.cycleStepsGridSelector), false);
                        }
                    },
                    change: function (e) {
                        if (e.action == "itemchange") {
                            kendo.ui.progress($(self.cycleStepsGridSelector), true);
                            self.cycleStepsGrid.saveChanges();
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
                    pageSizes: [10, 20, 50, 100],
                    refresh: true,
                },
                toolbar: [{ name: "create", text: "Add New Step" }],
                columns: [
                    {
                        field: "StepNumber",
                        title: "#",
                        width: "50px",
                    },
                    {
                        field: "FunctionNumber",
                        title: "Function Number",
                        width: "120px",
                    },
                    //                {
                    //                    field: "FunctionName",
                    //                    title: "Function Name",
                    //                    width: "90px",
                    //                },
                    {
                        field: "HardwareFunction",
                        title: "Function Name",
                        width: "120px",
                        editor: this.cycleStepFunctionDropDownEditor,
                        template: "#=HardwareFunction.Name#",
                    },
                    {
                        field: "StepTime",
                        title: "Step Time",
                        width: "90px",
                    },
                    {
                        field: "A",
                        title: "A",
                        width: "70px",
                    },
                    {
                        field: "G",
                        title: "G",
                        width: "70px",
                    },
                    {
                        field: "C",
                        title: "C",
                        width: "70px",
                    },
                    {
                        field: "T",
                        title: "T",
                        width: "70px",
                    },
                    {
                        field: "Five",
                        title: "5",
                        width: "70px",
                    },
                    {
                        field: "Six",
                        title: "6",
                        width: "70px",
                    },
                    {
                        field: "Seven",
                        title: "7",
                        width: "70px",
                    },
                    {
                        field: "SafeStep",
                        title: "Safe step",
                        width: "90px",
                    },
                    { command: [{ text: { edit: "Delete" }, click: this.deleteCycleStep }], title: " ", width: "90px" },
                ]
            }).data().kendoGrid;
        }
        else {
            this.cycleStepsGrid.dataSource.transport.options.read.url = actions.cycleEditor.cycleStepList + "/?synthesisCycleId=" + synthesisCycleId;
            this.cycleStepsGrid.dataSource.read();
        }
    };
    CycleEditorViewModel.prototype.deleteCycleStep = function (e) {
        e.preventDefault();
        var self = window.cycleEditorViewModel;
        kendo.ui.progress($(self.cycleStepsGridSelector), true);
        var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
        self.cycleStepsGrid.dataSource.remove(dataItem);
        self.cycleStepsGrid.dataSource.sync();
    };
    CycleEditorViewModel.prototype.cycleStepFunctionDropDownEditor = function (container, options) {
        $('<input required data-text-field="Name" data-value-field="Id" data-bind="value:' + options.field + '"/>')
            .appendTo(container)
            .kendoDropDownList({
            autoBind: false,
            dataSource: gridDataSource(actions.cycleEditor.cycleStepFunctionList, 100, "Number", "asc")
        });
    };
    CycleEditorViewModel.prototype.backToCycles = function () {
        this.cancelChanges();
        this.createSynthesisCyclesGrid();
        this.setMode(true, false);
        this.bindModel();
    };
    CycleEditorViewModel.prototype.setMode = function (isCycleListVisible, isCreateOrUpdateCycleVisible) {
        this.set("IsCycleListVisible", isCycleListVisible);
        this.set("IsCreateOrUpdateCycleVisible", isCreateOrUpdateCycleVisible);
    };
    return CycleEditorViewModel;
}(BaseViewModel));
//# sourceMappingURL=CycleEditorViewModel.js.map