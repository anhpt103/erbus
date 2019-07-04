define(['controllers/authorize/nhomQuyenController', 'controllers/authorize/menuController'], function () {
    'use strict';
    var app = angular.module('nguoiDungNhomQuyenModule', ['nhomQuyenModule', 'menuModule']);
    app.factory('nguoiDungNhomQuyenService', ['$http', 'configService', function ($http, configService) {
        var serviceUrl = configService.rootUrlWebApi + '/authorize/NguoiDungNhomQuyen';
        var result = {
            getByMaNhomQuyen:function(data) {
                return $http.get(serviceUrl + '/GetByMaNhomQuyen/' + data);
            },
            postNguoiDungNhomQuyen: function (data) {
                return $http.post(serviceUrl + '/PostNguoiDungNhomQuyen', data);
            },
            getNhomQuyenByUsername: function (userName, unitCode) {
                return $http.get(serviceUrl + '/GetNhomQuyenByUsername/' + userName + '/' + unitCode);
            }
        }
        return result;
    }]);

    app.controller('AddNguoiDungNhomQuyen_ctrl', ['$scope', '$uibModalInstance', '$http', 'configService', 'nguoiDungNhomQuyenService', 'menuService', 'tempDataService', '$filter', '$uibModal', 'targetData','nhomQuyenService','userService',
        function ($scope, $uibModalInstance, $http, configService, service, menuService, tempDataService, $filter, $uibModal, targetData, nhomQuyenService, userService) {
            $scope.config = {
                label: angular.copy(configService.label)
            };
            var currentUser = userService.GetCurrentUser();
            $scope.title = function () { return 'Thêm phân quyền nhóm người dùng'; };
            $scope.data = [];
            $scope.lstNhomQuyen = [];
            $scope.lstAdd = [];
            $scope.lstEdit = [];
            $scope.lstDelete = [];
            function loadNhomQuyenByUser() {
                service.getNhomQuyenByUsername(targetData.USERNAME, currentUser.unitCode).then(function (successRes) {
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
                        nhomQuyenService.getNhomQuyenChuaCauHinh(targetData.USERNAME).then(function (successRes) {
                            if (successRes && successRes.status == 200 && successRes.data.Status && successRes.data.Data) {
                                $scope.lstNhomQuyen = successRes.data.Data;
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
                    }
                });
            };

            loadNhomQuyenByUser();

            $scope.selectNhomQuyen = function (item) {
                var obj = {
                    USERNAME: targetData.USERNAME,
                    MANHOMQUYEN: item.MANHOMQUYEN,
                    TENNHOMQUYEN: item.TENNHOMQUYEN
                };
                $scope.lstAdd.push(obj);
                var filteredData = $filter('filter')($scope.lstNhomQuyen, { MANHOMQUYEN: item.MANHOMQUYEN }, true);
                if (filteredData && filteredData.length > 0) {
                    var index = $scope.lstNhomQuyen.indexOf(filteredData[0]);
                    if (index != -1) $scope.lstNhomQuyen.splice(index, 1);
                }
            };

            $scope.deSelectNhomQuyen = function (item) {
                $scope.lstNhomQuyen.push({
                    MANHOMQUYEN: item.MANHOMQUYEN,
                    TENNHOMQUYEN: item.TENNHOMQUYEN
                });
                var filteredData = $filter('filter')($scope.data, { MANHOMQUYEN: item.MANHOMQUYEN }, true);
                if (filteredData && filteredData.length > 0) {
                    var index = $scope.data.indexOf(filteredData[0]);
                    if (index != -1) $scope.data.splice(index, 1);
                    if (filteredData[0].ID && filteredData[0].ID.length > 0) {
                        $scope.lstDelete.push({
                            ID: filteredData[0].ID,
                            MANHOMQUYEN: filteredData[0].MANHOMQUYEN,
                            USERNAME: filteredData[0].USERNAME
                        });
                    }
                }
            };

            $scope.deSelectNhomQuyenAdd = function (item) {
                $scope.lstNhomQuyen.push({
                    MANHOMQUYEN: item.MANHOMQUYEN,
                    TENNHOMQUYEN: item.TENNHOMQUYEN
                });
                var filteredData = $filter('filter')($scope.lstAdd, { MANHOMQUYEN: item.MANHOMQUYEN }, true);
                if (filteredData && filteredData.length > 0) {
                    var index = $scope.lstAdd.indexOf(filteredData[0]);
                    if (index != -1) $scope.lstAdd.splice(index, 1);
                }
            };

            $scope.save = function () {
                var obj = {
                    USERNAME: targetData.USERNAME,
                    UNITCODE: currentUser.unitCode,
                    LstAdd: $scope.lstAdd,
                    LstDelete: $scope.lstDelete
                }
                console.log(obj);
                service.postNguoiDungNhomQuyen(obj).then(function (successRes) {
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


