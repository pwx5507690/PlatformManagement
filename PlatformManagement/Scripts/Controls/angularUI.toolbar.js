define(['angular'], function (angular) {

	'use strict';

	var progress;

	var toolbar = {
		restrict: "AE",

		scope: {
			toolbar: "=ngTool"
		},

		templateUrl: "Temp/tempToolbar.html",

		link: function (scope) {
			scope.selecteds = new Object();
			scope.changeRequest = function (name) {
				scope.toolbar.seach.changeRequest(scope.selecteds[name]);
			};
			scope.end = function ($last) {
                // page load end 
			    if ($last) {
			        // option default selcted
				    progress.setSelect();	
					for (var i = 0; i < scope.toolbar.seach.category.length; i++) {
					    var category = scope.toolbar.seach.category[i];                   
						scope.selecteds[category.name] = category.item[0].value;
					}				
				}
			};
		}
	};
	return function ($progress) {
		progress = $progress;
		return toolbar;
	};

});