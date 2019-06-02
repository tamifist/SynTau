/// <reference path="../../../typings/jquery/jquery.d.ts" />
/// <reference path="../../../typings/kendo/kendo.all.d.ts" />
/// <reference path="../../../typings/autogene/autogene.d.ts" />

class CycleEditorViewModel extends BaseViewModel {
    private cycleStepsGridSelector: string;
    private cycleStepsGrid: kendo.ui.Grid;

    private synthesisCyclesGridSelector: string;
    private synthesisCyclesGrid: kendo.ui.Grid;

    constructor(contentAreaSelector: string, cycleEditorJsonObject: JSON, cycleStepsGridSelector: string, synthesisCyclesGridSelector: string) {
        super(contentAreaSelector, cycleEditorJsonObject);

        this.cycleStepsGridSelector = cycleStepsGridSelector;
        this.synthesisCyclesGridSelector = synthesisCyclesGridSelector;
    }

    public createSynthesisCyclesGrid() {
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
                    { command: [{ text: "Edit", click: this.editSynthesisCycle }, { text: "Delete", click: this.deleteSynthesisCycle }], title: " ", width: "110px" },
                ]
            }).data().kendoGrid;
        } else {
            (<any>this.synthesisCyclesGrid.dataSource).transport.options.read.url = actions.cycleEditor.synthesisCycleList;
            this.synthesisCyclesGrid.dataSource.read();
        }
    }

    private editSynthesisCycle(e: any): void {
        e.preventDefault();

        var dataItem: any = (<any>this).dataItem($(e.currentTarget).closest("tr"));

        var self = (<any>window).cycleEditorViewModel;
        self.set("SynthesisCycleId", dataItem.Id);
        self.set("SynthesisCycleName", dataItem.Name);
        self.setMode(false, true);
        self.createCycleStepsGrid(dataItem.Id);
    }

    private deleteSynthesisCycle(e: any): void {
        e.preventDefault();

        var self = (<any>window).cycleEditorViewModel;

        var dataItem: any = (<any>this).dataItem($(e.currentTarget).closest("tr"));
        self.synthesisCyclesGrid.dataSource.remove(dataItem);
        self.synthesisCyclesGrid.dataSource.sync();
    }

    public createCycleStepsGrid(synthesisCycleId: string): void {

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
                        parameterMap: function(data, operation) {
                            if (operation != "read") {
                                for (var i = 0; i < data.models.length; i++) {
                                    (<any>data.models[i]).SynthesisCycleId = self.get("SynthesisCycleId");
                                }

                                return kendo.stringify(data.models);
                            }
                        }
                    },
                    requestEnd: function(e) {
                        if (e.type == "create" || e.type == "update" || e.type == "destroy") {
                            this.read();
                            kendo.ui.progress($(self.cycleStepsGridSelector), false);
                        }
                    },
                    change: function(e) {
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
                    { command: [{ text: "Delete", click: this.deleteCycleStep }], title: " ", width: "90px" },
                ]
            }).data().kendoGrid;
        } else {
            (<any>this.cycleStepsGrid.dataSource).transport.options.read.url = actions.cycleEditor.cycleStepList + "/?synthesisCycleId=" + synthesisCycleId;
            this.cycleStepsGrid.dataSource.read();
        }
    }

    private deleteCycleStep(e: any): void {
        e.preventDefault();
        
        var self = (<any>window).cycleEditorViewModel;
        kendo.ui.progress($(self.cycleStepsGridSelector), true);
        var dataItem: any = (<any>this).dataItem($(e.currentTarget).closest("tr"));
        self.cycleStepsGrid.dataSource.remove(dataItem);
        self.cycleStepsGrid.dataSource.sync();
    }

    private cycleStepFunctionDropDownEditor(container: any, options: any): void {
        $('<input required data-text-field="Name" data-value-field="Id" data-bind="value:' + options.field + '"/>')
            .appendTo(container)
            .kendoDropDownList({
                autoBind: false,
                dataSource: gridDataSource(actions.cycleEditor.cycleStepFunctionList, 100, "Number", "asc")
            });
    }

    public backToCycles(): void {
        this.cancelChanges();
        this.createSynthesisCyclesGrid();
        this.setMode(true, false);
        this.bindModel();
    }

    public setMode(isCycleListVisible: boolean, isCreateOrUpdateCycleVisible: boolean): void {
        this.set("IsCycleListVisible", isCycleListVisible);
        this.set("IsCreateOrUpdateCycleVisible", isCreateOrUpdateCycleVisible);
    }
} 