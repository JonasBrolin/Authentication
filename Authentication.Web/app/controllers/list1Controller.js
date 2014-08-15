'use strict';
app.controller('list1Controller', ['$scope', 'listService', function ($scope, listService) {

    $scope.lista = ["mjölk", "apa"];

    listService.getList1().then(function (result) {
        $scope.lista = result.data;
    }, function (error) {
        alert(error.data.message);
    });

}]);