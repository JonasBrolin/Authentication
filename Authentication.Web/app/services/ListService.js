'use strict';
app.factory('listService', ['$http', 'ngAuthSettings', function ($http, ngAuthSettings) {

    var serviceBase = ngAuthSettings.apiServiceBaseUri;

    var listServiceFactory = {};

    var _getList1 = function () {

        return $http.get(serviceBase + 'api/Todo').then(function (results) {
            return results;
        });
    };

    var _getList2 = function () {

        return $http.get(serviceBase + 'api/Todo/apa').then(function (results) {
            return results;
        });
    };

    listServiceFactory.getList1 = _getList1;
    listServiceFactory.getList2 = _getList2;

    return listServiceFactory;

}]);