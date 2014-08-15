'use strict';
app.controller('list2Controller', [
    '$scope', 'listService', function($scope, listService) {
        $scope.lista = [];

        listService.getList2().then(function(result) {
            $scope.lista = result.data;
        }, function(error) {
            alert(error.data.message);
        });
    }
]);