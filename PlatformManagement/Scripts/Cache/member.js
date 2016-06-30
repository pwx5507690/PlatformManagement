(function (sqlite, factory) {
    if (typeof define === "function" && define.amd) {
        define(["sqlite"], function (sqlite) {
            return factory(sqlite);
        });
    } else {
        window.account = factory(window.sqlite);
    }
})(window.sqlite, function (sqlite) {

    'use strict';

    var loaclMemberCacheName = "ACCOUNT";

    var drop = function () {
        sqlite.query("drop table " + loaclMemberCacheName);
    };

    var remove = function () {
        sqlite.query("delete  from " + loaclMemberCacheName);
    };
     //public string Id { get; set; }
     //   public string UserName { get; set; }
     //   public string Img { get; set; }
     //   public string Mail { get; set; }
     //   public string Password { get; set; }
     //   public string ConfirmPassword { get; set; }
     //   public string AccountName { get; set; }
     //   public bool IsUse { get; set; }
     //   public DateTime Age { get; set; }
     //   public string Address { get; set; }
     //   public AccountType AccountType { get; set; }
    var insert = function (model) {
        sqlite.insert(loaclMemberCacheName,
    {
        "ids": model.Id,
        "name": model.UserName,
        "img": model.Img,
        "mail": model.Mail,
        "age": model.Age,
        "address": model.Address,
        "accountType": model.AccountType,
    });
    };

    var create = function () {
        sqlite.query("CREATE TABLE " + loaclMemberCacheName
                    + " (id integer primary key autoincrement ,ids text, name text,img text,mail text,age text,address text,accountType text)");
    };

    var get = function (callback) {
        sqlite.fetchAll("select * from " + loaclMemberCacheName, callback);
    };

    var init = function () {
        sqlite.showTables(loaclMemberCacheName, function (result) {
            if (result == "") {
                create();
            }
           
        });
    };

    return {
        get: get,
        init: init,
        insert: insert,
        remove: remove 
    };
});