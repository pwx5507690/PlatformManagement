(function (factory) {
    if (typeof define === "function" && define.amd) {
        define(function () {
            return factory();
        });
    } else {
        window.common = factory();
    }
})(function () {
    'use strict';

    var config = {
        cacheCode: {
            httpRequestError: "httpRequestError"
        },
        errorPath: "Error.html"
    };

    var util = {
        getUrlParam: function () {
            var url = location.search;
            var theRequest = new Object();
            if (url.indexOf("?") != -1) {
                var str = url.substr(1);
                strs = str.split("&");
                for (var i = 0; i < strs.length; i++) {
                    theRequest[strs[i].split("=")[0]] = unescape(strs[i].split("=")[1]);
                }
            }
            return theRequest;
        },

        getUrlParamByName: function (name) {
            return util.getUrlParam()[name];
        },

        getUuid: function () {
            var s = [];
            var hexDigits = "0123456789abcdef";
            for (var i = 0; i < 36; i++) {
                s[i] = hexDigits.substr(Math.floor(Math.random() * 0x10), 1);
            }
            s[14] = "4";
            s[19] = hexDigits.substr((s[19] & 0x3) | 0x8, 1);
            s[8] = s[13] = s[18] = s[23] = "-";

            var uuid = s.join("");
            return uuid;
        },

        format: function () {
            if (arguments.length == 0)
                return null;
            var str = arguments[0];
            for (var i = 1; i < arguments.length; i++) {
                var re = new RegExp('\\{' + (i - 1) + '\\}', 'gm');
                str = str.replace(re, arguments[i]);
            }
            return str;
        },

        dateFormat: function (format, date) {
            var o = {
                "M+": date.getMonth() + 1,
                "d+": date.getDate(),
                "h+": date.getHours(),
                "m+": date.getMinutes(),
                "s+": date.getSeconds(),
                "q+": Math.floor((date.getMonth() + 3) / 3),
                "S": date.getMilliseconds()
            }
            if (/(y+)/.test(format))
                format = format.replace(RegExp.$1,
                (date.getFullYear() + "").substr(4 - RegExp.$1.length));
            for (var k in o)
                if (new RegExp("(" + k + ")").test(format))
                    format = format.replace(RegExp.$1,
                        RegExp.$1.length == 1 ? o[k] :
                        ("00" + o[k]).substr(("" + o[k]).length));
            return format;
        },

        getFileName: function (filepath) {
            if (filepath != "") {
                var names = filepath.split("\\");
                return names[names.length - 1];
            }
            throw Error("filename is empty");
        },

        isMail: function (value) {
            // /^(\w-*\.*)+@(\w-?)+(\.\w{2,})+$/
            return /^([a-zA-Z0-9]+[_|\_|\.]?)*[a-zA-Z0-9]+@([a-zA-Z0-9]+[_|\_|\.]?)*[a-zA-Z0-9]+\.[a-zA-Z]{2,3}$/.test(value);
        },

        isEmptyValue: function (value) {
            return !value;
        },

        isNotEmptyValue: function (value) {     
            return !util.isEmptyValue(value);
        },

        isEmptyObject: function (obj) {
            for (var name in obj) {
                return false;
            }
            return true;
        }
    };

    var http = {
        error: function (request, textStatus, errorThrown) {
            var param = {
                code: request.status,
                readyState: request.readyState,
                textStatus: textStatus
            };
            localStorage.setItem(config.cacheCode.httpRequestError, JSON.stringify(param));
            loaction.href(config.errorPath);
        },
        getRequestError: function () {
            return localStorage.setItem(config.cacheCode.httpRequestError);
        },
        removeRequestError: function () {
            localStorage.removeItem(config.cacheCode.httpRequestError);
        }
    };

    var bussiness = {
        loadJs: function (js) {
            return function ($q) {
                var def = $q.defer();
                require([js], function (controller) {
                    def.resolve(controller);
                });
                return def.promise;
            };
        }
    };

    return {
        config: config,
        http: http,
        util: util,
        bussiness: bussiness
    };
});
