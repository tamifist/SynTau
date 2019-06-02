/// <reference path="../../typings/jquery/jquery.d.ts" />
/// <reference path="../../typings/kendo/kendo.all.d.ts" />
/// <reference path="../../typings/autogene/autogene.d.ts" />

class BaseViewModel extends kendo.data.Model {
    protected jsonObject: JSON;
    protected  contentAreaSelector: string;

    constructor(contentAreaSelector: string, jsonObject: JSON) {
        super();

        this.contentAreaSelector = contentAreaSelector;
        this.jsonObject = jsonObject;

        this.setModel(jsonObject);
    }
    
    public setModel(jsonObject: JSON) {
        $.extend(this, new kendo.data.Model(jsonObject));
    }

    public bindModel() {
        kendo.bind($(this.contentAreaSelector), this);
    }

    public setIsDirty(isDirty: boolean): void {
        this.dirty = isDirty;
    }

    public cancelChanges() {
        this.setModel(this.jsonObject);
        //$.extend(this, new kendo.data.Model(this.jsonObject));
    }
} 