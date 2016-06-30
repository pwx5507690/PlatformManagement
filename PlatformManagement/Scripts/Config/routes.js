define(['webApp'], function (app) {

    'use strict';

    app.run(function ($rootScope, $account, $progress) {
        $account.isExpired(function () {
            loaction.href = "login.html";
        });
        // when routeChangeStart  current model menu style  active
        $rootScope.$on('$routeChangeStart', function (event, toState, toParams, fromState, fromParams) {
            // current model menu style  active
            //  $rootScope.setMenuStyle && $rootScope.setMenuStyle();          
        });
        // when  routeChangeSuccess 
        $rootScope.$on('$routeChangeSuccess', function (event, toState, toParams, fromState, fromParams) {
            $progress.progress.done();
        });

        $rootScope.$on('$locationChangeStart', function () {
            $progress.progress.start();
            $rootScope.user = $account.get();
            if (!$rootScope.user) {
               // loaction.href = "login.html";
            }

            //$account.get(function (ret) {
            //    if (!ret) {

            //    }
            //});
        });
    });

    app.controller("mainController", function ($scope, $http, $window,
        $loadJs, $q, $util, $rootScope, $progress, $account, $messageBus) {
        $loadJs("/Scripts/Controllers/mainController.js")($q)
        .then(function (mainController) {
            mainController($scope, $http, $rootScope, $window, $util, $progress, $account, $messageBus);
        });
    });

    app.config(function ($routeProvider, $controllerProvider, $loadJs, $httpProvider, $account) {
        //$account.get(function (ret) {
        //    if (ret) {
        //        // token api server 
        //        $httpProvider.defaults.headers.common['UserToken'] = ret[0]["ids"];
        //    }
        //});

        console.log($httpProvider.defaults.headers.common);
        // $controllerProvider.register delivered  
        app.registerController = $controllerProvider.register;
        $routeProvider
            .when('/', {
                templateUrl: 'Temp/tempProject.html',
                resolve: {
                    load: $loadJs("/Scripts/Controllers/projectController.js")
                }
            })
            .when('/account', {
                templateUrl: 'Temp/tempAccount.html',
                resolve: {
                    load: $loadJs("/Scripts/Controllers/accountController.js")
                }
            })
            .when('/addAccount', {
                templateUrl: 'Temp/tempAddAccount.html',
                resolve: {
                    load: $loadJs("/Scripts/Controllers/accountController.js")
                }
            })
            .when('/project', {
                templateUrl: 'Temp/tempProject.html',
                resolve: {
                    load: $loadJs("/Scripts/Controllers/projectController.js")
                }
            })
            .when('/addProject', {
                templateUrl: 'Temp/tempAddProject.html',
                resolve: {
                    load: $loadJs("/Scripts/Controllers/projectController.js")
                }
            })
            .when('/task', {
                templateUrl: 'Temp/tempTask.html',
                resolve: {
                    load: $loadJs("/Scripts/Controllers/projectController.js")
                }
            });
        //.otherwise({
        //     redirectTo: '/'
        // });

    });
});

