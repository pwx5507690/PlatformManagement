(function (jQuery, factory) {
    if (typeof define === "function" && define.amd) {
        define(["jquery"], function (jQuery) {
            return factory(jQuery);
        });
    } else {
        if (window.controls == null) {
            window.controls = new Object();
        }
        window.controls.progress = factory(jQuery);
    }
})($, function (jQuery) {
    'use strict';

    var defaultConfig = {
        minimum: 0.08,
        easing: 'ease',
        positionUsing: '',
        speed: 200,
        trickle: true,
        trickleRate: 0.02,
        trickleSpeed: 800,
        showSpinner: true,
        barSelector: '[role="nprogress-bar"]',
        spinnerSelector: '[role="nprogress-spinner"]',
        parent: 'body',
        template: '<div class="nprogress-bar" role="nprogress-bar">' +
            '<div class="nprogress-peg"></div></div>' +
            '<div class="nprogress-spinner" role="nprogress-spinner">' +
            '<div class="nprogress-spinner-icon"></div></div>'
    };

    var setSelect = function (param) {
        jQuery('select').selected(jQuery.extend( {
            btnSize: 'sm'
        },param));
    };

    var fullscreen = function () {
        jQuery.AMUI.fullscreen.toggle();
        return jQuery.AMUI.fullscreen.isFullscreen;
    };

    var fadein = function () {
        for (var i = 0; i < arguments.length; i++) {
            jQuery("#" + arguments[i]).fadeIn(1000);
        }      
    };

    var setConfig = function (config) {
        jQuery.AMUI.progress.configure(jQuery.extend(defaultConfig, config));
    };

    return {
        fadein: fadein,
        fullscreen: fullscreen,
        progress: jQuery.AMUI.progress,
        setConfig: setConfig,
        setSelect: setSelect
    };
});
