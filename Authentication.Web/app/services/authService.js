'use strict';
app.factory('authService', ['$q','$http', 'localStorageService', 'ngAuthSettings', function($q, $http, localStorageService, ngAuthSettings) {
    var serviceBase = ngAuthSettings.apiServiceBaseUri;
    var authServiceFactory = {};

    var _authentication = {
        isAuth: false,
        userName: ""
    };

    var _login = function(loginData) {
        var data = "grant_type=password&username=" + loginData.userName + "&password=" + loginData.password + "&client_id=" + ngAuthSettings.clientId;
        var deffered = $q.defer();

        $http.post(serviceBase + 'token', data, { headers: { 'Content-Type': 'application/x-www-form-urlencoded' } }).success(function(response) {
            localStorageService.set('authorizationData', { token: response.access_token, userName: loginData.userName, refreshToken: response.refresh_token, useRefreshTokens: true });
            _authentication.isAuth = true;
            _authentication.userName = loginData.userName;

            deffered.resolve(response);
        }).error(function(err, status) {
            deffered.reject(err);
        });

        return deffered.promise;
    };

    authServiceFactory.authentication = _authentication;
    authServiceFactory.login = _login;
    return authServiceFactory;

} ])