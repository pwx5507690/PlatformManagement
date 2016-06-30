define(["common"], function (common) {
	'use strict';

	var routeParam = new Object();

	var set = function (key, value) {
		routeParam[key] = value;
	};

	var get = function (key, isDelete) {
		var result = routeParam[key];
		if (isDelete) {
			delete routeParam[key];
		}
		return result;
	};

	var getUrlParam = function () {
		return common.util.getRequestParam();
	};

	return {
		getUrlParam: getUrlParam,
		get: get,
		set: set
	};
});