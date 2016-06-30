define(['angular'], function (angular) {

    'use strict';

    var http;

    var dialog;

    var util;

    // param is api server result object
    var getPagination = function (data) {
        if (!data.pager) {
            return [];
        }
        var pagination = [];
        var first = {
            name: "«"
        };
        var last = {
            name: "»"
        };
        for (var i = 0; i < data.pager; i++) {
            pagination.push({
                value: i,
                name: i + 1,
                className: data.currentPage == i + 1 ? "am-active" : ""
            });
        }
        if (data.pager == 1) {
            first.className = last.className = "am-disabled";
        } else if (data.currentPage == 1) {
            first.className = "am-disabled";
            last.className = "prev";
            last.value = data.currentPage;
        } else if (data.currentPage == data.pager) {
            last.className = "am-disabled";
            first.className = "prev";
            first.value = data.currentPage - 2;
        } else {
            last.className = first.className = "prev";
            last.value = data.currentPage;
            first.value = data.currentPage - 2;
        }

        pagination.splice(0, 0, first);
        pagination.push(last);

        return pagination;
    };

    var getSelected = function (model) {
        var selectedItem = new Array();
        for (item in model) {
            model[item].checked && selectedItem.push(item);
        }
        return selectedItem;
    };

    var getTempUrl = function () {
        // mobile view adapter
        //if (util.isMobile) {
        //    return "Temp/Mobile/tempTable.html";
        //}
        return "Temp/tempTable.html";
    };

    var table = {
        scope: {
            value: "=ngValue"
        },

        restrict: "AE",

        templateUrl: getTempUrl(),

        link: function (scope) {
            scope.request = function () {
                dialog.loading.modal();
                http(scope.value.requestParam).
                success(function (data, status, headers, config) {
                    var datasource = scope.value.gridName ? data[scope.value.gridName] : data;
                    console.log(datasource);
                    if (datasource == null || datasource.length == 0) {
                        scope.isData = true;
                        scope.context = [];
                        dialog.loading.close();
                        return;
                    }

                    if (scope.value.checkbox) {
                        scope.checkboxs = new Object();
                    }

                    scope.pager = getPagination(data);
                    scope.context = datasource;
                    dialog.loading.close();
                }).
                error(function (data, status, headers, config) {
                    dialog.loading.close();
                    scope.value.requestError(data, status, headers, config);
                });
            };

            scope.value.getModelByKey = function (key) {
                var m = null;
                for (var i = 0; i < scope.context.length; i++) {
                    if (scope.context[i][scope.value.key] == key) {
                        m = scope.context[i];
                        break;
                    }
                }
                return m;
            };

            scope.info = function (key) {
                var m = scope.value.getModelByKey(key);
                scope.value.action.info(key, m);
            };

            scope.value.deleteAll = function () {
                if (arguments.length) {
                    alert(arguments.length);
                    scope.value.action.delete(arguments[0]);
                } else {
                    var deleteIds = getSelected(scope.checkboxs).join(',');
                    if (!deleteIds) {
                        return dialog.dialog.modal("请选择要删除的项！！！");
                    }
                    scope.value.action.delete(getSelected(scope.checkboxs).join(','));
                }
            };

            scope.page = function (index) {
                if (index == null) {
                    return;
                }

                scope.value.requestParam.params.currentPage = index;
                scope.request();
            };

            scope.remove = function (value) {
                var newData = new Array();
                for (var i = 0; i < scope.context.length; i++) {
                    var item = scope[i];
                    if (item[scope.value.key] != value) {
                        newData.push(item);
                    }
                }
            };

            scope.value.reload = function () {
                scope.request();
            };

            scope.value.nofiy = function () {
                $scope.$apply();
            };

            scope.request();
        }
    };
    return function ($http, $dialog, $util) {
        http = $http;
        dialog = $dialog;
        util = $util;
        return table;
    };

});