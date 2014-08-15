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

    $routeProvider.otherwise({ redirectTo: "/home" });
});

var serviceBase = 'http://localhost:60241/';
//var serviceBase = 'http://ngauthenticationapi.azurewebsites.net/';
app.constant('ngAuthSettings', {
    apiServiceBaseUri: serviceBase,
    clientId: 'JavaApp'
});