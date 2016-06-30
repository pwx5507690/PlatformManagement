(function (jQuery, common) {

    var pageInit = function () {
        var errorMessage = JSON.parse(common.http.getRequestError());

        jQuery("#description").attr("content", errorMessage.textStatus);
        jQuery("#keywords").attr("content", errorMessage.code);
        jQuery("#code").html(errorMessage.code);
        jQuery("#tip").html(errorMessage.readyState);
        jQuery("#context").html(errorMessage.textStatus);

        common.http.removeRequestError();
    };

    jQuery(function () {
        pageInit();
    });

})($, common);