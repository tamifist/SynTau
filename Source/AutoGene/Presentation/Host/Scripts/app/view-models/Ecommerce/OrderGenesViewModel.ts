/// <reference path="../../../typings/jquery/jquery.d.ts" />
/// <reference path="../../../typings/kendo/kendo.all.d.ts" />
/// <reference path="../../../typings/autogene/autogene.d.ts" />

class OrderGenesViewModel extends BaseViewModel {
    private validator: kendo.ui.Validator;

    private geneFragmentsGridSelector: string;
    private geneFragmentsGrid: kendo.ui.Grid;

    private geneListGridSelector: string;
    private geneListGrid: kendo.ui.Grid;

    constructor(contentAreaSelector: string, geneEditorJsonObject: JSON, validator: kendo.ui.Validator, geneFragmentsGridSelector: string, geneListGridSelector: string) {
        super(contentAreaSelector, geneEditorJsonObject);

        this.validator = validator;

        this.geneFragmentsGridSelector = geneFragmentsGridSelector;
        this.geneListGridSelector = geneListGridSelector;
    }

    public createNewGene(): void {
        this.setMode(false, true);
    }

    public optimizeGene(): void {
        if (!this.validator.validate()) {
            return;
        }

        var postData: any = <any>this.toJSON();

        var self = this;

        $.ajax({
            type: "POST",
            url: actions.geneEditor.optimizeGene,
            dataType: "json",
            traditional: true,
            data: postData,
            success: function (response) {
                if (response.Code == JsonResponseResult.Success) {

                    self.set("OptimizedDNASequence", response.Data.OptimizedDNASequence);
                    self.set("IsGeneOptimized", response.Data.IsGeneOptimized);
                    self.set("GeneId", response.Data.GeneId);
                    self.set("GeneFragmentLength", response.Data.GeneFragmentLength);
                    self.set("GeneFragmentOverlappingLength", response.Data.GeneFragmentOverlappingLength);
                    self.set("SelectedOrganismName", response.Data.SelectedOrganismName);

                    self.createGeneFragmentsGrid(response.Data.GeneId);
                    self.updateGeneFragments();

                    self.geneListGrid.dataSource.read();
                } else if (response.Code == JsonResponseResult.Error) {
                    alert("Initial sequence is wrong");
                }
            },
        });
    }

    public createGeneListGrid() {
        if (!this.geneListGrid) {
            this.geneListGrid = $(this.geneListGridSelector).kendoGrid({
                dataSource: gridDataSource(actions.geneEditor.geneList, 10, "CreatedAt", "desc"),
                sortable: true,
                scrollable: true,
                filterable: true,
                resizable: true,
                selectable: "row",
                //height: 720,
                pageable: {
                    pageSizes: [10, 20, 50],
                    refresh: true,
                },
                toolbar: [{ name: "create", text: "Add New Step" }],
                columns: [
                    { field: "Name", width: "500px" },
                    { command: [{ text: { edit: "Edit" }, click: this.editGene }, { text: { edit: "Delete" }, click: this.deleteGene }], title: " ", width: "110px" },
                ],
            }).data().kendoGrid;
        }
    }

    private editGene(e: any): void {
        e.preventDefault();

        var dataItem: any = (<any>this).dataItem($(e.currentTarget).closest("tr"));
        var url = actions.geneEditor.getGeneEditorViewModel + "?geneId=" + dataItem.Id;

        var self = (<any>window).geneEditorViewModel;

        $.ajax({
            type: "GET",
            url: url,
            contentType: "application/json;charset=utf-8",
            dataType: "json",
            success: function (response) {
                //var geneEditorViewModel = new GeneEditorViewModel(response.Data, '#gene-fragments-grid', '#gene-list-grid');
                self.setModel(response.Data);
                self.setMode(false, true);
                self.bindModel();

                self.createGeneFragmentsGrid(response.Data.GeneId);

                self.setIsDirty(false);
            },
            error: function (response) {
            }
        });
    }

    private deleteGene(e: any): void {
        e.preventDefault();

        var dataItem: any = (<any>this).dataItem($(e.currentTarget).closest("tr"));
        var url = actions.geneEditor.deleteGene + "?geneId=" + dataItem.Id;
        var self = (<any>window).geneEditorViewModel;
        $.ajax({
            type: "GET",
            url: url,
            contentType: "application/json;charset=utf-8",
            dataType: "json",
            success: function (response) {
                self.geneListGrid.dataSource.read();
            },
            error: function (response) {
            }
        });
    }

    public createGeneFragmentsGrid(geneId: string): void {

        var self = this;
        if (!this.geneFragmentsGrid) {
            this.geneFragmentsGrid = $(this.geneFragmentsGridSelector).kendoGrid({
                dataSource: new kendo.data.DataSource({
                    batch: true,
                    pageSize: 10,
                    schema: {
                        model: {
                            id: "Id",
                            fields: {
                                Id: { type: "string", editable: false },
                                GeneId: { type: "string", editable: false },
                                FragmentNumber: { type: "number", editable: false },
                                OligoSequence: { type: "string", editable: true },
                                OligoLength: { type: "number", editable: false },
                                OverlappingLength: { type: "number", editable: true },
                                Tm: { type: "number", editable: false },
                            }
                        }
                    },
                    transport: {
                        read: {
                            url: actions.common.rootUrl + "api/GeneFragment" + "/?geneId=" + geneId,
                            type: "Get"
                        },
                        create: {
                            url: actions.common.rootUrl + "api/GeneFragment",
                            type: "PUT",
                            contentType: 'application/json;charset=utf-8'
                        },
                        update: {
                            url: actions.common.rootUrl + "api/GeneFragment",
                            type: "PUT",
                            contentType: 'application/json;charset=utf-8'
                        },
                        destroy: {
                            url: actions.common.rootUrl + "api/GeneFragment",
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
                            kendo.ui.progress($(self.geneFragmentsGridSelector), false);
                        }
                    },
                    change: function (e) {
                        if (e.action == "itemchange") {
                            kendo.ui.progress($(self.geneFragmentsGridSelector), true);
                            self.geneFragmentsGrid.saveChanges();
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
                //height: 600,
                pageable: {
                    pageSizes: [10, 20, 50],
                    refresh: true,
                },
                columns: [
                    {
                        field: "FragmentNumber",
                        title: "#",
                        width: "40px",
                    },
                    {
                        field: "OligoSequence",
                        title: "Oligonucleotide",
                        width: "350px",
                    },
                    {
                        field: "OligoLength",
                        title: "Oligo length",
                        width: "80px",
                    },
                    {
                        field: "OverlappingLength",
                        title: "Overlapping length",
                        width: "80px",
                    },
                    {
                        field: "Tm",
                        title: "Tm",
                        width: "80px",
                    },
                ]
            }).data().kendoGrid;
        } else {
            (<any>this.geneFragmentsGrid.dataSource).transport.options.read.url = actions.common.rootUrl + "api/GeneFragment" + "/?geneId=" + geneId;
            this.geneFragmentsGrid.dataSource.read();
        }
    }

    public updateGeneFragments(): void {

        kendo.ui.progress($(this.geneFragmentsGridSelector), true);

        var postData: any = <any>this.toJSON();

        var self = this;

        //save the changes
        $.ajax({
            type: "POST",
            url: actions.geneEditor.updateGeneFragments,
            dataType: "json",
            traditional: true,
            data: postData,
            success: function (response) {
                kendo.ui.progress($(self.geneFragmentsGridSelector), false);
                self.geneFragmentsGrid.dataSource.read();
            }
        });
    }

    public backToGenes(): void {
        this.cancelChanges();
        this.setMode(true, false);
        this.bindModel();
    }

    public setMode(isGeneListVisible: boolean, isCreateOrUpdateGeneVisible: boolean): void {
        this.set("IsGeneListVisible", isGeneListVisible);
        this.set("IsCreateOrUpdateGeneVisible", isCreateOrUpdateGeneVisible);
    }

    public initialSequenceTypeChanged(e: any) {
        this.set("InitialSequence", "");
    }

    public initialSequenceChanged(e: any) {
        var initialSequence = this.get("InitialSequence");
        if (this.get("SelectedInitialSequenceType") == InitialSequenceType.ProteinInitialSequence) {
            this.set("InitialSequence", initialSequence.replace(/[^a-zA-Z]/g, "").toUpperCase().replace(/(.{10})/g, "$1 "));
        } else if (this.get("SelectedInitialSequenceType") == InitialSequenceType.DNAInitialSequence) {
            this.set("InitialSequence", initialSequence.replace(/[^a-zA-Z]/g, "").toUpperCase().replace(/(.{4})/g, "$1 "));
        }

    }
} 