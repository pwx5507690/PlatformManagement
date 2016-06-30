(function ( factory) {
    if (typeof define === "function" && define.amd) {
        define(function () {
            return factory();
        });
    } else {
        window.account = factory();
    }
})(function () {

    'use strict';

    var interval = 300000;

    var timer = 6000;

    var loaclAccountCacheName = "ACCOUNT";

    var remove = function () {
        localStorage.removeItem(loaclAccountCacheName);
    };

    var user = null;

    var insert = function (model) {
        // current user
        remove();
        //var isExit = localStorage.get();
        //if (isExit) {

        //}
        localStorage.setItem(loaclAccountCacheName, JSON.stringify(
    {
        "ids": model.uuid,
        "name": model.accountName,
        "isCheck": model.isCheck,
        "password": model.password,
        "type": model.type,
        "interval": new Date().valueOf()
    }));
    };

    var removeToken = function () {
        var user = get();
        if (user != null) {
            user.uuid = "";
            insert(user);
        }
        //sqlite.update(loaclAccountCacheName, {
        //    "ids": ""
        //}, "id=?", [1], null);
    };

    var getItem = function () {
        return localStorage.getItem(loaclAccountCacheName);
    };

    var get = function () {
        var value = getItem();
        if (value && value != "undefined") {
            return JSON.parse(getItem());
        }
    };

    var isExpired = function (expiredCallback) {
        window.setInterval(function () {
            var user = get();
            if (user != null) {
                removeToken();
                expiredCallback();
            }
            //get(function (ret) {
            //    // ret == ""
            //    if (!ret) {
            //        return;
            //    }
            //    for (var i = 0; i < ret.length; i++) {
            //        if (+ret[i]["timer"] + interval > new Date().valueOf()) {
            //            removeToken();
            //            expiredCallback();
            //        }
            //        break;
            //    }
            //});
        }, timer);
    };

    return {
        get: get,
        isExpired: isExpired,
        insert: insert,
        remove: remove,
        removeToken: removeToken
    };
});