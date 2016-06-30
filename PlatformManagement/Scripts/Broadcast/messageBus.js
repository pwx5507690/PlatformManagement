define(['jquery', 'signalR'], function (jQuery) {

    'use strict';

    var client = new Object();

    var chat = null;

    var messageType = {
        LoginOut: "LoginOut",
        TaskInfo: "TaskInfo",
        NoticMessage: "NoticMessage"
    };

    jQuery.get("/signalr/hubs").done(function (context) {

        chat = jQuery.connection.chat;

        chat.client.sendMessage = function (message) {
            var mes = JSON.parse(message);
            for (item in client) {
                client[item][mes.type].call(mes);
            }
        };
    });

    var isInitChart = function () {
        if (!chat) {
            console.log("hub is not connection");
            return false;
        }
        return true;
    };

    var register = function (key, value) {
        if (!client[key]) {
            client[key] = value;
        }
    };

    var receives = function (message) {
        if (isInitChart()) {
            $.connection.hub.start().done(function () {
                chat.server.send(message);
            });
        }
    };

    var send = function (message) {
        if (isInitChart()) {
            $.connection.hub.start().done(function () {
                chat.server.sendMessage(message);
            });
        }
    };

    return {
        messageType: messageType,
        receives: receives,
        send: send,
        register: register
    };
});




