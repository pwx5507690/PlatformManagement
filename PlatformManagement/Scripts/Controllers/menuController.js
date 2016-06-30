define(['angular', 'webApp'], function (angular, app) {

    'use strict';

    var validation = function ($scope) {
        $scope.errorMessage1 = "标题不能为空";
        if (!$scope.title) {
            $scope.isShowTitle = true;
        } else {
            $scope.isShowTitle = false;
        }
        $scope.errorMessage2 = "路径不能为空";
        if (!$scope.url) {
            $scope.isShowUrl = true;
        } else {
            $scope.isShowUrl = false;
        }
        if ($scope.isShowTitle || $scope.isShowUrl) {
            return false;
        }
        return true;
    };

    var request = function ($scope, $http) {
        var url = "/api/menu/AddMenu";
        var data = {
            "Title": $scope.title,
            "Url": $scope.url,
            "IsUse": !!$scope.isUse
        };
        $http({
            method: 'POST',
            url: url,
            data: data
        })
            .success(function (data, status) {
                if (data.result) {
                    $scope.success = true;
                    $scope.successMes = "添加成功";
                } else {
                    $scope.error = true;
                    $scope.errorMes = data.message;
                }
                setTimeout(function () {
                    $scope.success = false;
                    $scope.error = false;
                }, 1000);
            }).error(function (data, status) {
                $scope.errorMes = status;
            });
    };

    app.registerController('menuController', function ($scope, $http) {
        $scope.init = function () {

        };
        $scope.submit = function () {
            if (validation($scope)) {
                request($scope, $http);
            }
        };
    });
});
