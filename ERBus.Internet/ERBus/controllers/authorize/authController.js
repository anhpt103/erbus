// 'use strict';
define(['angular'], function (angular) {
    var app = angular.module('authModule', ['configModule']);
    app.service('userService', ['localStorageService', function (localStorageService) {
        var fac = {};
        fac.CurrentUser = null;
        fac.SetCurrentUser = function (user) {
            fac.CurrentUser = user;
            localStorageService.set('authorizeData', user);
        };
        fac.GetCurrentUser = function () {
            fac.CurrentUser = localStorageService.get('authorizeData');
            return fac.CurrentUser;
        };
        return fac;
    }]);

    app.service('accountService', ['configService', '$http', '$q', 'localStorageService', '$state', 'userService', function (configService, $http, $q, localStorageService, $state, userService) {
        var result = {
            login: function (user) {
                var obj = { 'username': user.username, 'password': user.password, 'grant_type': 'password' };
                Object.toparams = function ObjectsToParams(obj) {
                    var p = [];
                    for (var key in obj) {
                        p.push(key + '=' + encodeURIComponent(obj[key]));
                    }
                    return p.join('&');
                }
                var defer = $q.defer();
                $http({ method: 'post', url: configService.apiServiceBaseUri + "/token", data: Object.toparams(obj), headers: { 'Content-Type': 'application/x-www-form-urlencoded' } }).then(function (response) {
                    if (response && response.status === 200 && response.data && response.data.access_token) {
                        userService.SetCurrentUser(response.data);
                        $state.go('home');
                    }
                    defer.resolve(response);
                }, function (response) {
                    defer.reject(response);
                });
                return defer.promise;
            },
            logout: function () {
                localStorageService.cookie.clearAll();
                $state.go('login');
            }
        };
        return result;
    }]);

    app.controller('login_Ctrl', ['$scope', '$location', '$http', 'localStorageService', 'accountService', '$state', 'closingService', function ($scope, $location, $http, localStorageService, accountService, $state, closingService) {
        $scope.user = { username: '', password: '', cookie: false, grant_type: 'password' };
        var config = {
            headers: { 'Content-Type': 'application/x-www-form-urlencoded' }
        };
        $scope.returnMessage = "";
        function closing() {
            closingService.closingOutList().then(function (successRes) {
                if (successRes && successRes.status === 200 && successRes.data) {
                    console.log('Khóa sổ thành công');
                }
            });
        };
        $scope.login = function () {
            $scope.msg = null;
            accountService.login($scope.user).then(function (response) {
                if (response && response.status === 200 && response.data) {
                    closing();
                    console.log("Đăng nhập thành công!");
                }
            }, function (response) {
                if (response && response.data) {
                    $scope.returnMessage = response.data.error_description;
                }
                $scope.user = { username: '', password: '', cookie: false, grant_type: 'password' };
                $scope.focusUsername = true;
            });
        };
    }]);
    return app;
});

