var app = angular.module('authApp', ['ngRoute', 'LocalStorageModule']);

app.config(function($routeProvider) {
    $routeProvider.when("/home",
    {
        controller: "homeController",
        templateUrl: "app/views/home.html"
    });

    $routeProvider.when("/login",
    {
        controller: "loginController",
        templateUrl: "/app/views/login.html"
    });

    $routeProvider.when("/list1",
    {
        controller: "list1Controller",
        templateUrl: "/app/views/list1.html"
    });

    $routeProvider.when("/list2",
    {
        controller: "list2Controller",
        templateUrl: "/app/views/list2.html"
    });

    $routeProvider.otherwise({ redirectTo: "/home" });
});

var serviceBase = 'http://localhost:60241/';
//var serviceBase = 'http://ngauthenticationapi.azurewebsites.net/';
app.constant('ngAuthSettings', {
    apiServiceBaseUri: serviceBase,
    clientId: 'JavaApp'
});

app.config(function ($httpProvider) {
    $httpProvider.interceptors.push('authInterceptorService');
});

app.run(['authService', function (authService) {
    authService.fillAuthData();
}]);