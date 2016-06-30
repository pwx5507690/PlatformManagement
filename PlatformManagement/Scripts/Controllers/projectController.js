define(['angular', 'webApp'], function (angular, app) {

    'use strict';

    app.registerController('project', function ($scope, $http, $location, $param, $dialog, $progress, $base, $util) {

        var paramKey = "project";

        $scope.projectViewInit = function () {
            var view = $base.getListView();
            // title name
            $scope.titleCName = "工程";
            $scope.titleEName = "Project";
            // overwrite  
            view.getTitle = function () {
                return [
                    { "name": '工程名称', "key": "Name" },
                    { "name": '开始时间', "key": "StartTime" },
                    { "name": '进度', "key": "Progress" },
                    { "name": '耗时', "key": "RunTime" },
                    { "name": '总时', "key": "TotalTime" }
                ];
            };
            // overwrite  
            view.getDataKey = function () {
                return "Guid";
            };
            // overwrite  
            view.getDataSourceName = function () {
                return "result";
            };
            // overwrite  
            view.getDataSoucreApiURL = function () {
                return "/api/Project/GetBageProject";
            };
            // overwrite 
            view.goToInfo = function (guid, model) {
                $param.set(paramKey, model);
                $location.url("/addProject");
            };
            // overwrite  
            view.getDeletApiUrl = function () {
                return "/api/Project/Delete";
            };
            // overwrite  
            view.getSearchParam = function () {
                return function (value) {
                    return {
                        params: {
                            name: value,
                            currentPage: 0,
                            pageSize: 10
                        },
                        url: "/api/Project/GetProjectByName"
                    };
                };
            };

            // overwrite  
            view.add = function () {
                return function () {
                    $location.url("/addProject");
                };
            };

            view.init({
                scope: $scope,
                dialog: $dialog,
                http: $http,
                angular: angular
            });
        };

        $scope.projectOptionInit = function () {
            var model = $param.get(paramKey, true);

            if (model) {
                $scope.titleCName = "编辑工程";
                $scope.titleEName = "Edit Project";

            } else {
                // title name
                $scope.titleCName = "新增工程";
                $scope.titleEName = "Add Project";
            }

            $scope.createModel = function () {
                $dialog.prompt.modal({
                    title: "创建模块"
                }, {
                    onConfirm: function (e) {
                        if (e.data) {
                            $scope.model = $scope.model == null ? e.data : $scope.model + "," + e.data;
                            $scope.$apply();
                            $dialog.prompt.removeValue();
                        }
                    }
                });
            };

            var optionView = $base.getOptionView();

            optionView.getFillFormData = function () {

            };

            optionView.getValidateFormData = function () {
                return [{
                    "name": "projectName",
                    "method": "isNotEmptyValue", 
                    "message": "请填写工程名称!!!"
                }]
            };

            optionView.init({
                scope: $scope,
                dialog: $dialog,
                http: $http,
                util: $util,
                angular: angular
            });

            $scope.goBack = function () {
                $location.url("/project");
            };

            optionView.request({
                method: 'GET',
                url: "/api/Member/GetAllAccount",

            }, function (data) {
                console.log($scope.projectName);
                $scope.members = data;
                
                $progress.setSelect({
                    btnWidth: "100%"
                });
            }, function (message) {
                console.log(message);
            });

            $scope.getTask = function () {
                if (!$scope.model) {
                    return null;
                }
                var workTask = new Array();
                var models = $scope.model.split(',');
                for (var i = 0; i < models.length; i++) {
                    workTask.push({
                        "PojectId": model,
                        "Name": models[i],
                        "IsComplete": false
                    });
                }
                return workTask;
            };

            $scope.sub = function () {
                if (optionView.validateFormValue()) {
                    var workTask =  $scope.getTask();
        
                    optionView.request({
                        data: {
                            "Name": $scope.projectName,
                            "Money": $scope.money,
                            "TotalTime": $scope.totalTime,
                            "Task": workTask
                        },
                        method: "POST",
                        url: "/api/Project/AddProject"
                    }, function (data) {
                        $scope.saveMessage = "工程保存修改成功！！！";
                        $scope.saveShow = { "display": "" };
                        $scope.saveTip = "am-alert am-alert-success";

                    }, function (message) {
                        $scope.saveMessage = "保存修改失败请联系管理员！！！";
                        $scope.saveTip = "am-alert am-alert-warning";
                        $scope.saveShow = { "display": "" };
                    });
                }
            };
        };

        $scope.projectInfo = function () {

        }
    });
});