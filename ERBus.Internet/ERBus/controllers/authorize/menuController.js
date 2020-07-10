define(['ui-bootstrap'], function () {
    'use strict';
    var app = angular.module('menuModule', ['ui.bootstrap']);
    app.factory('menuService', ['$http', 'configService', function ($http, configService) {
        var serviceUrl = configService.rootUrlWebApi + '/Authorize/Menu';
        var result = {
            getAllNhomQuyenMenu: function (params) {
                return $http.get(serviceUrl + '/GetAllNhomQuyenMenu/' + params);
            },
            getAllQuyenMenu: function (params) {
                return $http.get(serviceUrl + '/GetAllQuyenMenu/' + params);
            }
        }
        return result;
    }]);
    return app;
});