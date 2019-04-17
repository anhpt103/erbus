define(['ui-bootstrap'], function () {
    'use strict';
    var app = angular.module('menuModule', ['ui.bootstrap']);
    app.factory('menuService', ['$http', 'configService', function ($http, configService) {
        var serviceUrl = configService.rootUrlWebApi + '/Authorize/Menu';
        var result = {
            getMenu: function (data) {
                return $http.get(serviceUrl + '/GetMenu/' + data);
            },
            getAllNhomQuyenMenu: function (params) {
                return $http.get(serviceUrl + '/GetAllNhomQuyenMenu/' + params);
            },
            getAllQuyenMenu: function (params) {
                return $http.get(serviceUrl + '/GetAllQuyenMenu/' + params);
            }
        }
        return result;
    }]);
    app.controller('menu_ctrl', ['$scope', '$location', '$http', 'configService', 'menuService', 'tempDataService', '$filter', '$uibModal', '$log', 'securityService',
       function ($scope, $location, $http, configService, service, tempDataService, $filter, $uibModal, $log, securityService) {
       }]);
return app;
});