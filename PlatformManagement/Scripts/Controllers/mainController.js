define(['angular'], function (angular) {

    'use strict';

    return function ($scope, $http, $rootScope, $window, $util, $progress, $account, $messageBus) {

        $scope.loginOut = function () {
            $account.removeToken();
            location.href = "login.html";
        };

        var user = $account.get();

        if(user!=null){
            $scope.acctountName = $rootScope.user.name;
        }

        $scope.full = "开启全屏";
        $rootScope.noityMessage = "暂无公告";

        var clinetMessage = new Object();

        clinetMessage[$messageBus.messageType.NoticMessage] = function (message) {
            $rootScope.noityMessage = message.body;
        };

        clinetMessage[$messageBus.messageType.LoginOut] = function () {
            $scope.loginOut();
        };

        $messageBus.register("MAIN", clinetMessage);

        $http({
            method: 'GET',
            url: "/api/Main/GetMainInitModel"
        }).success(function (data, status) {
            $rootScope.menuData = data;

            $rootScope.setMenuStyle = function () {
                for (var item in $rootScope.menuData) {
                    var name = $util.getFileName($rootScope.menuData[item].Url);
                    if (location.href.indexOf(name) != -1) {
                        $rootScope.menuData[item]["style"] = "active";
                    } else {
                        $rootScope.menuData[item]["style"] = "";
                    }
                    // console.log($rootScope.menuData[item]["Title"]);
                }

                $scope.menu = $rootScope.menuData;
                $progress.fadein("admin-offcanvas", "admin-context");
            };
            $rootScope.setMenuStyle();
        });


        //$rootScope.$watch('loadEnd', function (newValue, oldValue) {

        //}, true);

        $scope.fullWindow = function () {
            $progress.fullscreen() ? $scope.full = "退出全屏" : $scope.full = "开启全屏";
        };

    };
});