define(['angular', 'webApp'], function (angular, app) {

    'use strict';

    app.registerController('account', function ($rootScope, $scope, $http, $location,
        $base, $window, $q, $account, $dialog, $util, $param) {
        var paramKey = "account";

        $scope.accountViewInit = function () {
            var view = $base.getListView();
            // title name
            $scope.titleCName = "团队成员";

            $scope.titleEName = "Member";

            //if ($rootScope.user.type == "1") {
            //    view.getToolbarAction = function () {
            //        return null;
            //    };
            //    view.getTableAction = function () {
            //        return null;
            //    }
            //}
            // overwrite  
            view.getTitle = function () {
                return [
                    {
                        "name": '昵称',
                        "key": "AccountName"
                    },
                    {
                        "name": '姓名',
                        "key": "UserName"
                    },
                    {
                        "name": '出生年月',
                        "key": "Age"
                    },
                    {
                        "name": '地址',
                        "key": "Address"
                    }
                ];
            };
            // overwrite  
            view.getDataKey = function () {
                return "Id";
            };
            // overwrite  
            view.getDataSourceName = function () {
                return "result";
            };
            // overwrite  
            view.getDataSoucreApiURL = function () {
                return "/api/Member/GetAccount";
            };

            // overwrite 
            view.goToInfo = function (guid, model) {
                $param.set(paramKey, model);
                $location.url("/addAccount");
            };

            // overwrite  
            view.getDeletApiUrl = function () {
                return "/api/Member/Delete";
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
                        url: "/api/Member/GetAccountByName"
                    };
                };
            };

            // overwrite  
            view.add = function () {
                return function () {
                    $location.url("/addAccount");
                };
            };

            view.init({
                scope: $scope,
                dialog: $dialog,
                http: $http,
                angular: angular
            });

            //if ($account.User.type != "Admin") {
            //    view.getToolbarAction = function () {
            //        return null;
            //    }
            //};
        };

        $scope.accountOptionInit = function () {

            var view = new $base.getOptionView();

            var model = $param.get(paramKey, true);

            if (model) {
                $scope.titleCName = "编辑成员";
                $scope.titleEName = "Edit Member";

            } else {
                // title name
                $scope.titleCName = "新增成员";
                $scope.titleEName = "Add Member";
            }

            var optionView = $base.getOptionView();

            var pageInit = function (isNotAdmin) {
                if (!isNotAdmin) {
                    $account.passWordStyle = { "display": "" };
                }

                optionView.getFillFormData = function () {

                };

                optionView.getValidateFormData = function () {
                    var val = [{
                        "name": "name",
                        "method": "isNotEmptyValue",
                        "message": "请填写昵称（登录名）!!!"
                    }, {
                        "name": "mail",
                        "method": "isMail",
                        "message": "请填写正确的邮箱名!!!"
                    }];

                    if (isNotAdmin) {
                        val.push({
                            "name": "password",
                            "method": "isNotEmptyValue",
                            "message": "请填写密码"
                        });
                        val.push({
                            "name": "conPassword",
                            "method": function (value) {
                                return value == $scope.password;
                            }, "message": "两次填写的密码不一致"
                        });
                    }

                    return val;
                };

                optionView.init({
                    scope: $scope,
                    dialog: $dialog,
                    http: $http,
                    util: $util,
                    angular: angular
                });

                $scope.goBack = function () {
                    $location.url("/account");
                };

                $scope.addAccount = function () {
                    optionView.request({
                        method: 'POST',
                        url: "/api/Member/AddAccountForAdminPage",
                        data: {
                            "AccountName": $scope.name,
                            "Img": $scope.img,
                            "Mail": $scope.mail,
                            "Password": isNotAdmin ? $scope.password : "-1",
                            "ConfirmPassword": isNotAdmin ? $scope.conPassword : "-1",
                            "Age": $scope.age,
                            "Address": $scope.address,
                            "AccountType": 1
                        }
                    }, function (data) {
                        $scope.saveMessage = "账号添加成功！！！";
                        $scope.saveShow = { "display": "" };
                        $scope.saveTip = "am-alert am-alert-success";
                    }, function () {
                        $scope.saveMessage = "保存修改账号失败请联系管理员！！！";
                        $scope.saveTip = "am-alert am-alert-warning";
                        $scope.saveShow = { "display": "" };
                    });
                }

                $scope.sub = function () {
                    if (!optionView.validateFormValue()) {
                        return;
                    }

                    optionView.request({
                        method: 'GET',
                        url: "/api/Member/ValidateMail?mail=" + $scope.mail,

                    }, function (data) {
                        console.log(data);
                        if (data) {
                            return $dialog.dialog.modal("该邮箱已被注册");
                        }
                        $scope.addAccount();
                    }, function (message) {
                        console.log(message);
                    });
                };
            };
            // $rootScope.user.type == "1"
            pageInit(true);

        }
    });
});