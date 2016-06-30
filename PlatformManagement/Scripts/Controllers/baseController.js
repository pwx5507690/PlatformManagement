define(function () {

    'use strict';

    var request = function (param, success, error) {
        var self = this;

        self.dialog.loading.modal();

        self.http(param).success(function (data) {
            success(data) || self.dialog.loading.close();
        })
        .error(function (data, status, headers, config) {
            error(data, status, headers, config) || self.dialog.loading.close();
        });
    };

    var listView = function () {

        var self = this;

        this.goToInfo =

        this.getDataKey =

        this.getTitle =

        this.getDataSourceName =

        this.getDataSoucreApiURL =

        this.getDeletApiUrl =

        this.add =

        this.changeRequest =

        this.getSearchParam =

        function () {
            return null;
        };

        this.getEmptyMessage = function () {
            return "暂无数据";
        };

        this.getErrorMessage = function () {
            return "服务器异常";
        };

        this.getDeleteRequestType = function () {
            return "POST";
        };

        this.getDataRequestType = function () {
            return "GET";
        };

        this.delete = function () {
            return function () {
                self.scope.value.deleteAll();
            };
        };

        this.getToolBarSeach = function () {
            return {
                request: self.searchRequest(),
                changeRequest: self.changeRequest()
            };
        };

        this.getToolbarAction = function () {
            return {
                "add": self.add(),
                "delete": self.delete()
            };
        };

        this.searchRequest = function () {
            var seachFunction = self.getSearchParam();
            if (seachFunction != null) {
                return function (value) {
                    if (value) {
                        var seachParam = seachFunction(value);
                        self.angular.extend(self.scope.value.requestParam, seachParam);
                    } else {
                        self.scope.value.requestParam = self.getRequestParam();
                    }
                    self.scope.value.reload && self.scope.value.reload();
                }
            } else {
                return null;
            }
        };

        this.createToolbar = function () {
            self.scope.toolbar = {
                action: self.getToolbarAction(),
                seach: self.getToolBarSeach()
            };
        };

        this.getRequestParam = function () {
            return {
                params: {
                    pageSize: 10, currentPage: 0
                },
                method: self.getDataRequestType(),
                url: self.getDataSoucreApiURL()
            };
        };

        this.createCotext = function () {
            self.scope.value = {
                key: self.getDataKey(),
                gridName: self.getDataSourceName(),
                empty: self.getEmptyMessage(),
                title: self.getTitle(),
                requestParam: self.getRequestParam(),
                requestError: self.getRequestError(),
                action: self.getTableAction()
            };
        };

        this.getTableAction = function () {
            return {
                "delete": function (guid) {
                    self.request({
                        data: { guid: guid },
                        method: self.getDeleteRequestType(),
                        url: self.getDeletApiUrl()
                    }, function (data) {
                        self.scope.value.requestParam.currentPage = 0;
                        self.scope.value.reload();
                    }, function (data, status, headers, config) {
                        self.scope.value.requestError(data, status, headers, config);
                    });
                },
                "info": function (guid, m) {
                    self.goToInfo(guid, m);
                }
            }
        };

        this.getRequestError = function () {
            return function (message) {
                console.log(message);
                self.dialog.dialog.modal(self.getErrorMessage());
            };
        };

        this.init = function (param) {
            this.scope = param.scope;
            this.dialog = param.dialog;
            this.http = param.http;
            this.angular = param.angular;
            this.request = request;
            this.createCotext();
            this.createToolbar();
        };
    };

    var optionView = function () {

        var self = this;

        this.getFillFormData =

        this.getValidateFormData =

        function () {
            return null;
        };

        this.validateFormValue = function () {
            var validateFormModel = self.getValidateFormData();
            if (!validateFormModel) {
                return true;
            }
            console.log(self.util);
            for (var item in validateFormModel) {
                var value = self.scope[validateFormModel[item]["name"]];
                var method = validateFormModel[item]["method"];
                console.log(method);
                method = typeof method === "string" ? self.util[method] : method;
                var result = method(value);
                if (!result) {
                    self.dialog.dialog.modal(validateFormModel[item]["message"]);
                    return false;
                }
            }
            return true;
        };

        this.fillForm = function () {
            var formModel = self.getFillFormData();
            if (!formModel) {
                return;
            }
            for (var item in formModel) {
                slef.scope[item] = formModel[item];
            }
        };

        this.init = function (param) {
            this.http = param.http;
            this.scope = param.scope;
            this.angular = param.angular;
            this.dialog = param.dialog;
            this.request = request;
            this.util = param.util;
            this.fillForm();
        };
    };

    return {
        getListView: function () {
            return new listView();
        },
        getOptionView: function () {
            return new optionView();
        }
    }
});