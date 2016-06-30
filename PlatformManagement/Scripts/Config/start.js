var config = {
    appName: "platformManagement"
};
require.config({
    paths: {
        'dialog': '../Controls/amazeUI.dialog',
        'progress': '../Controls/amazeUI.progress',
        'angular': '../angular',
        "angularRoutes": '../angular-route',
        'common': 'common',
        'routes': 'routes',
        'webApp': 'webApp',
        'sqlite': '../Cache/sqlite',
        'account': '../Cache/account',
        'jquery': '/AmazeUI/assets/js/jquery.min',
        'amazeui': '/AmazeUI/assets/js/amazeui.min',
        'signalR': "../jquery.signalR-2.0.0",
        'messageBus': "../Broadcast/messageBus",
        'grid': "../Controls/angularUI.table",
        'toolbar': "../Controls/angularUI.toolbar",
        'param': "../Controls/angular.routeParam",
        'base': "../Controllers/baseController"
    },
    waitSeconds: 0,

    shim: {
        'signalR': {
            "deps": ["jquery"]
        },
        'jquery': {
            'exports': '$'
        },
        "param": {
            "deps": ["common"]
        },
        "progress": {
            "deps": ["amazeui"]
        },
        "dialog": {
            "deps": ["amazeui"]
        },
        'amazeui': {
            "deps": ["jquery"]
        },
        'messageBus': {
            "deps": ['signalR']
        },
        'angular': {
            'exports': 'angular'
        },
        "account": {
            "deps": ["sqlite"]
        },
        'angularRoutes': {
            "deps": ["angular"]
        },
        'sqlite': {
            'exports': 'sqlite'
        },
        'common': {
            'exports': 'common'
        }
    },
    priority: [
        "angular"
    ],
    urlArgs: new Date().valueOf()// 'v=1.0.0.8'
});

require(['messageBus',
         'angular',
         'webApp',
         'routes',
         'angularRoutes',
         'amazeui'
], function (message, angular) {
    //    messageBus = message;
    angular.bootstrap(document, [config.appName]);
});