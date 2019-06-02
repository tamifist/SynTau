/// <reference path="../../typings/jquery/jquery.d.ts" />
/// <reference path="../../typings/kendo/kendo.all.d.ts" />

interface IViewModel extends kendo.data.ObservableObject {
}

interface IViewModelGeneric<T extends IViewModel> extends IViewModel {
    CurrentEntity: T;
    SetCurrentEntity(entity: T);
}

interface IEntityViewModel extends IViewModel {
    Id: string;
    Version: number;
    IsDirty(): boolean;
}

declare var amplify;
declare var actions;
declare var gridDataSource;