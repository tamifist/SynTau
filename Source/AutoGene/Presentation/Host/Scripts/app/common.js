function gridDataSource(url, pageSize, sortField, sortDir) {
    return new kendo.data.DataSource({
        type: "odata",
        transport: {
            read: {
                url: url,
                dataType: "json",
                cache: false
            }
        },
        sort: { field: sortField, dir: sortDir },
        schema: {
            data: function (response) {
                if (response.value !== undefined)
                    return response.value;
                else {
                    delete response["odata.metadata"];
                    return response;
                }
            },

            total: function (response) {
                return response['odata.count'];
            }
        },
        pageSize: pageSize,
        serverPaging: true,
        serverFiltering: true,
        serverSorting: true
    });
}