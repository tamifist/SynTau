/// <reference path="../../typings/jquery/jquery.d.ts" />
/// <reference path="../../typings/kendo/kendo.all.d.ts" />
/// <reference path="../../typings/autogene/autogene.d.ts" />
var __extends = (this && this.__extends) || (function () {
    var extendStatics = Object.setPrototypeOf ||
        ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
        function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
var BaseViewModel = (function (_super) {
    __extends(BaseViewModel, _super);
    function BaseViewModel(contentAreaSelector, jsonObject) {
        var _this = _super.call(this) || this;
        _this.contentAreaSelector = contentAreaSelector;
        _this.jsonObject = jsonObject;
        _this.setModel(jsonObject);
        return _this;
    }
    BaseViewModel.prototype.setModel = function (jsonObject) {
        $.extend(this, new kendo.data.Model(jsonObject));
    };
    BaseViewModel.prototype.bindModel = function () {
        kendo.bind($(this.contentAreaSelector), this);
    };
    BaseViewModel.prototype.setIsDirty = function (isDirty) {
        this.dirty = isDirty;
    };
    BaseViewModel.prototype.cancelChanges = function () {
        this.setModel(this.jsonObject);
        //$.extend(this, new kendo.data.Model(this.jsonObject));
    };
    return BaseViewModel;
}(kendo.data.Model));
//# sourceMappingURL=BaseViewModel.js.map