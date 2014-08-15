'use strict';
app.factory('authService', ['$q','$http', '$injector', 'localStorageService', 'ngAuthSettings', function($q, $http, $injector, localStorageService, ngAuthSettings) {
    var serviceBase = ngAuthSettings.apiServiceBaseUri;
    var authServiceFactory = {};

    var _authentication = {
        isAuth: false,
        userName: "",
        admin: false,
        user: false
    };

    var _login = function(loginData) {
        var data = "grant_type=password&username=" + loginData.userName + "&password=" + loginData.password + "&client_id=" + ngAuthSettings.clientId;
        var deffered = $q.defer();

        $http.post(serviceBase + 'token', data, { headers: { 'Content-Type': 'application/x-www-form-urlencoded' } }).success(function(response) {
            localStorageService.set('authorizationData', { token: response.access_token, userName: loginData.userName, refreshToken: response.refresh_token, useRefreshTokens: true });
            _authentication.isAuth = true;
            _authentication.userName = loginData.userName;

            deffered.resolve(response);
        }).error(function (err, status) {
            _logOut();
            deffered.reject(err);
        });

        return deffered.promise;
    };

      var _refreshToken = function () {
        var deferred = $q.defer();

        var authData = localStorageService.get('authorizationData');

        if (authData) {

            var data = "grant_type=refresh_token&refresh_token=" + authData.refreshToken + "&client_id=" + ngAuthSettings.clientId;

            localStorageService.remove('authorizationData');
            
            $http.post(serviceBase + 'token', data, { headers: { 'Content-Type': 'application/x-www-form-urlencoded' } }).success(function (response) {

                localStorageService.set('authorizationData', { token: response.access_token, userName: response.userName, refreshToken: response.refresh_token, useRefreshTokens: true });
                _authentication.isAuth = true;
                _authentication.userName = response.userName;
                deferred.resolve(response);

            }).error(function (err, status) {
                _logOut();
                deferred.reject(err);
            });
        } else {
            deferred.reject();
        }

        return deferred.promise;
    };

    var _logOut = function () {

        localStorageService.remove('authorizationData');

        _authentication.isAuth = false;
        _authentication.userName = "";
        _authentication.useRefreshTokens = false;

    };

    var _fillAuthData = function () {

        var authData = localStorageService.get('authorizationData');
        if (authData) {
            _authentication.isAuth = true;
            _authentication.userName = authData.userName;
            _authentication.useRefreshTokens = authData.useRefreshTokens;
        }

    };

    authServiceFactory.authentication = _authentication;
    authServiceFactory.login = _login;
    authServiceFactory.logOut = _logOut;
    authServiceFactory.fillAuthData = _fillAuthData;
    authServiceFactory.refreshToken = _refreshToken;
    
    return authServiceFactory;

} ])