(function (jQuery, account) {
    
    var elmMap = {
        "$accuount": jQuery("#account"),
        "$password": jQuery("#password"),
        "$login": jQuery("#login"),
        "$remember": jQuery("#remember"),
        "$foudAccountBtn": jQuery("#foudAccountBtn"),
        "$loginContext": jQuery("#loginContext"),
        "$foudAccount": jQuery("#foudAccount")
    };

    var request = function (param, callback, callbackError) {
        window.controls.loading.modal();
        var url = "/api/Member/Login";
        jQuery.post(url, param)
            .success(function (data) {
                window.controls.loading.close();
                callback(data);
            })
            .error(function (request, textStatus, errorThrown) {
                window.controls.loading.close();
                callbackError(request, textStatus, errorThrown);
            });
    };

    var changeFoudAccount = function() {
        elmMap.$loginContext.hide();
        elmMap.$foudAccount.show();
    };

    var login = function () {
        var user = elmMap.$accuount.val();
        var password = elmMap.$password.val();
        if (!user) {
            window.controls.dialog.modal("请输入登录名!!!");
            return elmMap.$accuount.focus();
        }
        if (!password) {
            window.controls.dialog.modal("请输入登密码!!!");
            return elmMap.$password.focus();
        }
        var param = {
            "AccountName": user,
            "Password": password
        };
        request(param, function (data) {
            if (!data.result) {
                return window.controls.dialog.modal({
                    title: "登录失败",
                    context: data.message
                });
            }
            console.log(data.result);
            account.insert({
                "uuid": data.result.Key,
                "accountName": data.result.Value.AccountName,
                "password": data.result.Value.Password,
                "isCheck": elmMap.$remember.prop ? 1 : 0
            });
            location.href("Main.html");
        }, function (request, textStatus, errorThrown) {
            common.http.error(request, textStatus, errorThrown);
           // window.controls.dialog.modal(JSON.stringify(error));
           // console.error(JSON.stringify(error));
        });
    };

    var pageInit = function () {
        account.get(function (ret) {
            if (ret == "") {
                return;
            }
            if (+ret[0]["isCheck"] == 1) {
                elmMap.$accuount.val(ret[0]["name"]);
                elmMap.$password.val(ret[0]["password"]);
            }
        });
        elmMap.$login.on("click", login);
        elmMap.$foudAccountBtn.on("click", changeFoudAccount);
    };

    jQuery(function () {
        pageInit();
    });
})($, window.account);