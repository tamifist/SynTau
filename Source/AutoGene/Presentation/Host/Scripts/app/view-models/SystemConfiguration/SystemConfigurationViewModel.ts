/// <reference path="../../../typings/jquery/jquery.d.ts" />
/// <reference path="../../../typings/kendo/kendo.all.d.ts" />
/// <reference path="../../../typings/autogene/autogene.d.ts" />

class SystemConfigurationViewModel extends BaseViewModel {
    private channelConfigurationsGridSelector: string;
    private channelConfigurationsGrid: kendo.ui.Grid;

    constructor(contentAreaSelector: string, jsonObject: JSON, channelConfigurationsGridSelector: string) {
        super(contentAreaSelector, jsonObject);

        this.channelConfigurationsGridSelector = channelConfigurationsGridSelector;
    }

    public createChannelConfigurationsGrid() {
        if (!this.channelConfigurationsGrid) {
            var self = this;
            this.channelConfigurationsGrid = $(this.channelConfigurationsGridSelector).kendoGrid({
                dataSource: new kendo.data.DataSource({
                    batch: true,
                    pageSize: 10,
                    schema: {
                        model: {
                            id: "Id",
                            fields: {
                                Id: { type: "string", editable: false },
                                ChannelNumber: { type: "number", editable: true },
                                StartNucleotide: { type: "string", editable: true },
                                HardwareFunction: { editable: true },
                            }
                        }
                    },
                    transport: {
                        read: {
                            url: actions.systemConfiguration.channelConfigurationList,
                            type: "Get"
                        },
                        create: {
                            url: actions.systemConfiguration.channelConfigurationList,
                            type: "PUT",
                            contentType: 'application/json;charset=utf-8'
                        },
                        update: {
                            url: actions.systemConfiguration.channelConfigurationList,
                            type: "PUT",
                            contentType: 'application/json;charset=utf-8'
                        },
                        destroy: {
                            url: actions.systemConfiguration.channelConfigurationList,
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
                            kendo.ui.progress($(self.channelConfigurationsGridSelector), false);
                        }
                    },
                    change: function (e) {
                        if (e.action == "itemchange") {
                            kendo.ui.progress($(self.channelConfigurationsGridSelector), true);
                            self.channelConfigurationsGrid.saveChanges();
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
                toolbar: [{ name: "create", text: "Add New Channel" }],
                columns: [
                    {
                        field: "ChannelNumber",
                        title: "Channel Number",
                        width: "200px"
                    },
                    {
                        field: "StartNucleotide",
                        title: "Start Nucleotide",
                        width: "200px"
                    },
                    {
                        field: "HardwareFunction",
                        title: "Api Function",
                        width: "120px",
                        editor: this.activateChannelFunctionDropDownEditor,
                        template: "#=HardwareFunction.Name#",
                    },
                    { command: [{ text: { edit: "Delete" }, click: this.deleteChannelConfiguration }], title: " ", width: "110px" },
                ]
            }).data().kendoGrid;
        }
    }
    
    private deleteChannelConfiguration(e: any): void {
        e.preventDefault();

        var self = (<any>window).systemConfigurationViewModel;

        var dataItem: any = (<any>this).dataItem($(e.currentTarget).closest("tr"));
        self.channelConfigurationsGrid.dataSource.remove(dataItem);
        self.channelConfigurationsGrid.dataSource.sync();
    }

    private activateChannelFunctionDropDownEditor(container: any, options: any): void {
        $('<input required data-text-field="Name" data-value-field="Id" data-bind="value:' + options.field + '"/>')
            .appendTo(container)
            .kendoDropDownList({
                autoBind: false,
                dataSource: gridDataSource(actions.systemConfiguration.activateChannelFunctionList, 100, "Number", "asc")
            });
    }
} 