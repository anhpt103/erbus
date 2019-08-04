define(['ui-bootstrap'], function () {
    'use strict';
    var app = angular.module('cauHinhLoaiPhongModule', ['ui.bootstrap']);
    app.factory('cauHinhLoaiPhongService', ['$http', 'configService', function ($http, configService) {
        var serviceUrl = configService.rootUrlWebApi + '/Catalog/CauHinhLoaiPhong';
        var selectedData = [];
        var result = {
            post: function (data) {
                return $http.post(serviceUrl + '/Post', data);
            },
            postCost: function (listData) {
                return $http.post(serviceUrl + '/PostCost', listData);
            },
            getCostByLoaiPhong: function (maLoaiPhong, maHang) {
                return $http.get(serviceUrl + '/GetCostByLoaiPhong/' + maLoaiPhong + '/' + maHang);
            },
            getAllData: function () {
                return $http.get(serviceUrl + '/GetAllData');
            }
        }
        return result;
    }]);
    return app;
});