define(['controllers/authorize/menuController'], function () {
    'use strict';
    var app = angular.module('nguoiDungQuyenModule', ['menuModule']);
    app.factory('nguoiDungQuyenService', ['$http', 'configService', function ($http, configService) {
        var serviceUrl = configService.rootUrlWebApi + '/authorize/NguoiDungQuyen';
        var result = {
            postNguoiDungQuyen: function (data) {
                return $http.post(serviceUrl + '/PostNguoiDungQuyen', data);
            },
            getAllQuyenByUsername: function (userName, unitCode) {
                return $http.get(serviceUrl + '/GetAllQuyenByUsername/' + userName + '/' + unitCode);
            }
        }
        return result;
    }]);

    app.controller('AddNguoiDungQuyen_ctrl', ['$scope', '$uibModalInstance', '$http', 'configService', 'nguoiDungQuyenService', 'menuService', 'tempDataService', '$filter', '$uibModal', 'targetData', 'userService',
        function ($scope, $uibModalInstance, $http, configService, service, menuService, tempDataService, $filter, $uibModal, targetData, userService) {
            $scope.config = {
                label: angular.copy(configService.label)
            };
            var currentUser = userService.GetCurrentUser();
            $scope.title = function () { return 'Phân quyền người dùng [ ' + targetData.USERNAME + ' ]' };
            $scope.data = [];
            $scope.listMenu = [];
            $scope.lstAdd = [];
            $scope.lstEdit = [];
            $scope.lstDelete = [];

            function loadQuyen() {
                service.getAllQuyenByUsername(targetData.USERNAME, currentUser.unitCode).then(function (successRes) {
                    if (successRes && successRes.status == 200 && successRes.data.Status && successRes.data.Data) {
                        $scope.data = successRes.data.Data;
                    } 
                    return $scope.data;
                }, function (errorRes) {
                    Lobibox.notify('error', {
                        title: 'Xảy ra lỗi',
                        msg: errorRes.statusText,
                        delay: 3000
                    });
                }).then(function (data) {
                    if (data) {
                        menuService.getAllQuyenMenu(targetData.USERNAME).then(function (successRes) {
                            if (successRes && successRes.status == 200 && successRes.data.Status && successRes.data.Data) {
                                $scope.listMenu = successRes.data.Data;
                            }
                        }, function (errorRes) {
                            Lobibox.notify('error', {
                                title: 'Xảy ra lỗi',
                                msg: errorRes.statusText,
                                delay: 3000
                            });
                        });
                    }
                });
            }
            loadQuyen();

            $scope.selectChucNang = function (item) {
                var obj = angular.copy(item);
                obj.XEM = true;
                obj.USERNAME = targetData.USERNAME;
                $scope.lstAdd.push(obj);
                var filteredData = $filter('filter')($scope.listMenu, { MA_MENU: item.MA_MENU }, true);
                if (filteredData && filteredData.length > 0) {
                    var index = $scope.listMenu.indexOf(filteredData[0]);
                    if (index != -1) $scope.listMenu.splice(index, 1);
                }
            };

            $scope.deSelectChucNang = function (item) {
                $scope.listMenu.push({
                    MA_MENU: item.MA_MENU,
                    SAPXEP: item.SAPXEP,
                    TIEUDE: item.TIEUDE
                });
                var filteredData = $filter('filter')($scope.data, { MA_MENU: item.MA_MENU }, true);
                if (filteredData && filteredData.length > 0) {
                    var index = $scope.data.indexOf(filteredData[0]);
                    if (index != -1) $scope.data.splice(index, 1);
                    if (filteredData[0].ID && filteredData[0].ID.length > 0) {
                        $scope.lstDelete.push({
                            ID: filteredData[0].ID,
                            USERNAME: filteredData[0].USERNAME,
                            MA_MENU: filteredData[0].MA_MENU
                        });
                    }
                }
            };

            $scope.deSelectChucNangAdd = function (item) {
                $scope.listMenu.push({
                    MA_MENU: item.MA_MENU,
                    SAPXEP: item.SAPXEP,
                    TIEUDE: item.TIEUDE
                });
                var filteredData = $filter('filter')($scope.lstAdd, { MA_MENU: item.MA_MENU }, true);
                if (filteredData && filteredData.length > 0) {
                    var index = $scope.lstAdd.indexOf(filteredData[0]);
                    if (index != -1) $scope.lstAdd.splice(index, 1);
                }
            };

            $scope.modified = function (item) {
                var filteredData = $filter('filter')($scope.lstEdit, { MA_MENU: item.MA_MENU }, true);
                if (!filteredData || filteredData.length < 1) {
                    $scope.lstEdit.push(item);
                }
            };

            $scope.addAll = {};
            $scope.editAll = {};
            $scope.checkAll = function (listObj, objModel, value) {
                if ($scope[listObj] && $scope[listObj].length > 0) {
                    angular.forEach($scope[listObj], function (v, k) {
                        v[objModel] = value;
                    });
                }
            };

            $scope.save = function () {
                var obj = {
                    USERNAME: targetData.USERNAME,
                    UNITCODE: currentUser.unitCode,
                    LstAdd: $scope.lstAdd,
                    LstEdit: $scope.lstEdit,
                    LstDelete: $scope.lstDelete
                }
                service.postNguoiDungQuyen(obj).then(function (successRes) {
                    if (successRes && successRes.status == 200 && successRes.data.Status && successRes.data.Data) {
                        Lobibox.notify('success', {
                            title: 'Thông báo',
                            width: 400,
                            msg: successRes.data.Message,
                            delay: 2000
                        });
                        $uibModalInstance.close(successRes.data.Data);
                    } else {
                        Lobibox.notify('error', {
                            title: 'Xảy ra lỗi',
                            msg: successRes.data.Message,
                            delay: 3000
                        });
                    }
                }, function (errorRes) {
                    Lobibox.notify('error', {
                        title: 'Xảy ra lỗi',
                        msg: errorRes.statusText,
                        delay: 3000
                    });
                });
            };
            $scope.cancel = function () {
                $uibModalInstance.close();
            };
        }]);

    return app;
});


