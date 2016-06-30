define(
    ['angular',
     'angularRoutes',
     'common',
     'grid',
     'dialog',
     'progress',
     'account',
     'toolbar',
     'messageBus',
     'param',
     "base"],
function (angular,
    angularRoutes,
    common,
    grid,
    dialog,
    progress,
    account,
    toolbar,
    messageBus,
    param,
    base) {

    'use strict';

    return angular.module(config.appName, ['ngRoute'])
    .constant("$loadJs", common.bussiness.loadJs)
    .constant("$account", account)
    .value("$dialog", dialog)
    .value("$util", common.util)
    .value("$progress", progress)   
    .value("$param", param)
    .value("$messageBus", messageBus)
    .value("$base", base)
    .directive("ngToolbar", toolbar)
    .directive("ngGrid", grid);

});


