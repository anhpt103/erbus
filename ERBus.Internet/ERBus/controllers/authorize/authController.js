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
                let obj = { 'username': user.username, 'password': user.password, 'grant_type': 'password' };
                Object.toparams = function ObjectsToParams(obj) {
                    var p = [];
                    for (var key in obj) {
                        p.push(key + '=' + encodeURIComponent(obj[key]));
                    }
                    return p.join('&');
                }
                var defer = $q.defer();
                try {
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
                } catch (error) {
                    console.log(error);
                }
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
        $scope.msg = "";

        $scope.login = function () {
            accountService.login($scope.user).then(function (response) {
                if (response && response.status === 200 && response.data) {
                    closingService.closingOutList();
                }
            }, function (response) {
                if (response && response.status !== 200 && response.data && response.data.error_description.length > 0) {
                    $scope.msg = 'Không thể kết nối tới Cơ sở dữ liệu! Kiểm tra kết nối Internet hoặc liên hệ Quản trị';
                    console.log(response);
                    return;
                }

                $scope.user = { username: '', password: '', cookie: false, grant_type: 'password' };
                $scope.focusUsername = true;
            });
        };
    }]);
    return app;
});

