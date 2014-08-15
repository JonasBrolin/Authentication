'use strict';
app.controller('indexController', ['$scope', '$location', 'authService', function($scope, $location, authService) {
    var controller = {};

    $scope.logOut = function () {
        authService.logOut();
        $location.path('/home');
    }

    $scope.authentication = authService.authentication;

    var _updateAuth = function() {
        $scope.authentication = authService.authentication;
    }

    controller.updateAuth = _updateAuth;
    return controller;
}]);