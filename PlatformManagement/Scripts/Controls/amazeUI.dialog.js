(function (jQuery, factory) {
    if (typeof define === "function" && define.amd) {
        define(["jquery", "common"], function (jQuery, common) {
            return factory(jQuery, common);
        });
    } else {
        if (window.controls == null) {
            window.controls = new Object();
        }
        window.controls = jQuery.extend(window.controls, factory(jQuery, window.common));
    }
})($, function (jQuery, common) {
    'use strict';

    var dialog = function () {
        this.elmTarget = {
            container: common.util.getUuid(),
            title: common.util.getUuid(),
            context: common.util.getUuid(),
            button: common.util.getUuid()
        };

        this.getElment = function (args) {
            if (typeof args == "string") {
                return jQuery("#" + args);
            } else {
                var elmMap = new Object();
                for (var item in args) {
                    elmMap[item] = jQuery("#" + args[item]);
                }
                return elmMap;
            }
        };

        this.isExist = function () {
            return this.getElment(this.elmTarget.container).is("div");
        };

        this.getTemp = function () {
            var temp = '<div class="am-modal am-modal-alert" tabindex="-1" id="{0}">';
            temp += '<div class="am-modal-dialog">';
            temp += '<div class="am-modal-hd" id={1}></div>';
            temp += '<div class="am-modal-bd" id={2}>';
            temp += '</div>';
            temp += '<div class="am-modal-footer">';
            temp += '<span class="am-modal-btn" id={3}></span>';
            temp += '</div>';
            temp += '</div>';
            temp += '</div>';
            return temp;
        };

        this.createDialog = function () {
            var temp = this.getTemp();
            var html = common.util.format(temp, this.elmTarget.container,
                this.elmTarget.title, this.elmTarget.context, this.elmTarget.button);
            jQuery(html).appendTo($("body"));
        };

        this.setParam = function (args) {
            var param = {
                title: "错误",
                button: "确定"
            };
            if (typeof args == "string") {
                param.context = args;
            } else {
                param = jQuery.extend(param, args);
            }
            return param;
        };

        this.close = function () {
               this.getElment(this.elmTarget).container.modal('close');
        };

        this.modal = function (args, modalParam) {
            var param = this.setParam(args);
            if (!this.isExist()) {
                this.createDialog();
            }
            var map = this.getElment(this.elmTarget);
            for (var item in param) {
                if (map[item] == null || param[item] == null) {
                    continue;
                }
                map[item].html(param[item]);
            }
            map.container.modal(modalParam);
        };
    };

    // loading exend dialog overwrite getTemp  
    var loading = function () {
        this.getTemp = function () {
            var temp = '<div class="am-modal am-modal-loading am-modal-no-btn" tabindex="-1" id="{0}">';
            temp += '<div class="am-modal-dialog">';
            temp += '<div class="am-modal-hd">loading...</div>';
            temp += '<div class="am-modal-bd">';
            temp += '<span class="am-icon-spinner am-icon-spin"></span>';
            temp += '</div>';
            temp += '</div>';
            temp += '</div>';
            return temp;
        };

        this.setParam = function () {
            return {};
        };
    };
    // prompt exend dialog overwrite getTemp  
    var prompt = function () {
        this.getTemp = function () {
            var temp = '<div class="am-modal am-modal-prompt" id="{0}"><div class="am-modal-dialog">';
            temp += '<div class="am-modal-hd" id={1}></div>';
            temp += '<div class="am-modal-bd"><input id={2} type="text" class="am-modal-prompt-input"></div>';
            temp += '<div class="am-modal-footer">';
            temp += ' <span class="am-modal-btn" data-am-modal-cancel>取消</span>';
            temp += ' <span class="am-modal-btn" data-am-modal-confirm>确定</span>';
            temp += '</div>';
            temp += '</div></div></div>';
            return temp;
        };

        this.removeValue = function () {
          var map = this.getElment(this.elmTarget);
            map.context.val('');
        };
    };
 
    return {
        loading: jQuery.extend(new dialog(), new loading()),
        dialog: new dialog(),
        prompt:jQuery.extend(new dialog(), new prompt())
    };
});
