define(['angular'], function (angular) {

    'use strict';

    var form = {
        restrict: "AE",

        scope: {
            form: "=ngForm"
        },

        templateUrl: "Temp/tempForm.html",

        link: function (scope) {
            
        }
    };
    return function ($progress) {
        return form;
    };

});