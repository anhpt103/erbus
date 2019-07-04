define(['ui-bootstrap'], function () {
    'use strict';
    var app = angular.module('cuaHangModule', ['ui.bootstrap']);
    app.factory('cuaHangService', ['$http', 'configService', function ($http, configService) {
        var serviceUrl = configService.rootUrlWebApi + '/Authorize/CuaHang';
        var selectedData = [];
        var result = {
            getAllData: function () {
                return $http.get(serviceUrl + '/GetAllData');
            },
            getAllDataByUniCode: function (unitCode) {
                return $http.get(serviceUrl + '/GetAllDataByUniCode/' + unitCode);
            },
            getAllChildren: function (maCuaHangCha) {
                return $http.get(serviceUrl + '/GetAllChildren/' + maCuaHangCha);
            },
            postQuery: function (data) {
                return $http.post(serviceUrl + '/PostQuery', data);
            },
            buildNewCode: function () {
                return $http.get(serviceUrl + '/BuildNewCode');
            },
            buildNewCodeChildren: function (maCuaHang) {
                return $http.get(serviceUrl + '/BuildNewCodeChildren/' + maCuaHang);
            },
            post: function (data) {
                return $http.post(serviceUrl + '/Post', data);
            },
            update: function (params) {
                return $http.post(serviceUrl + '/Update', params);
            },
            delete: function (params) {
                return $http.delete(serviceUrl + '/' + params.ID, params);
            }
        }
        return result;
    }]);
    /* controller list */
    app.controller('CuaHang_Ctrl', ['$scope', '$http', 'configService', 'cuaHangService', 'tempDataService', '$filter', '$uibModal', '$log', 'securityService',
        function ($scope, $http, configService, service, tempDataService, $filter, $uibModal, $log, securityService) {
            $scope.config = angular.copy(configService);
            $scope.paged = angular.copy(configService.pageDefault);
            $scope.filtered = angular.copy(configService.filterDefault);
            $scope.tempData = tempDataService.tempData;
            $scope.title = function () { return 'Cửa hàng bán hàng' };
            $scope.data = [];
            $scope.setPage = function (pageNo) {
                $scope.paged.CurrentPage = pageNo;
                filterData();
            };
            $scope.sortType = 'MA_CUAHANG';
            $scope.sortReverse = false;
            $scope.doSearch = function () {
                $scope.paged.CurrentPage = 1;
                filterData();
            };
            $scope.pageChanged = function () {
                filterData();
            };
            $scope.goHome = function () {
                window.location.href = "#!/home";
            };
            $scope.refresh = function () {
                $scope.setPage($scope.paged.CurrentPage);
            };
            function filterData() {
                $scope.isLoading = true;
                if ($scope.accessList.XEM) {
                    var postdata = { paged: $scope.paged, filtered: $scope.filtered };
                    service.postQuery(postdata).then(function (successRes) {
                        if (successRes && successRes.status === 200 && successRes.data && successRes.data.Status && successRes.data.Data && successRes.data.Data.Data) {
                            $scope.isLoading = false;
                            $scope.data = successRes.data.Data.Data;
                            angular.extend($scope.paged, successRes.data.Data);
                        }
                    }, function (errorRes) {
                        $scope.isLoading = false;
                        console.log(errorRes);
                    });
                }
            };
            //check authorize
            function loadAccessList() {
                securityService.getAccessList('CuaHang').then(function (successRes) {
                    if (successRes && successRes.status == 200 && successRes.data) {
                        $scope.accessList = successRes.data;
                        if (!$scope.accessList.XEM) {
                            Lobibox.notify('error', {
                                position: 'bottom left',
                                msg: 'Không có quyền truy cập !'
                            });
                        } else {
                            filterData();
                        }
                    } else {
                        Lobibox.notify('error', {
                            position: 'bottom left',
                            msg: 'Không có quyền truy cập !'
                        });
                    }
                }, function (errorRes) {
                    console.log(errorRes);
                    Lobibox.notify('error', {
                        position: 'bottom left',
                        msg: 'Không có quyền truy cập !'
                    });
                    $scope.accessList = null;
                });
            };
            //end function loadAccessList()
            loadAccessList();

            /* Function create new item */
            $scope.create = function () {
                var modalInstance = $uibModal.open({
                    backdrop: 'static',
                    animation: true,
                    //windowClass: 'authorize-window',
                    size: 'lg',
                    templateUrl: configService.buildUrl('authorize/CuaHang', 'create'),
                    controller: 'cuaHangCreate_Ctrl',
                    resolve: {}
                });
                modalInstance.result.then(function (refundedData) {
                    $scope.refresh();
                    if (refundedData) {
                        var tempObj = {};
                        tempObj.VALUE = refundedData.MA_CUAHANG;
                        tempObj.TEXT = refundedData.MA_CUAHANG + " | " + refundedData.TEN_CUAHANG;
                        tempObj.DESCRIPTION = refundedData.TEN_CUAHANG;
                        if ($scope.tempData('cuaHang')) {
                            $scope.tempData('cuaHang').push(tempObj);
                        }
                        else {
                            var listObj = [];
                            listObj.push(tempObj);
                            tempDataService.putTempData('cuaHang', listObj);
                        }
                    }
                }, function () {
                    $log.info('Modal dismissed at: ' + new Date());
                });
            };

            /* Function details Item */
            $scope.detail = function (target) {
                var modalInstance = $uibModal.open({
                    backdrop: 'static',
                    animation: true,
                    size: 'lg',
                    templateUrl: configService.buildUrl('authorize/CuaHang', 'detail'),
                    controller: 'cuaHangDetail_Ctrl',
                    resolve: {
                        targetData: function () {
                            return target;
                        }
                    }
                });
                modalInstance.result.then(function (refundedData) {
                    $scope.refresh();
                }, function () {
                    $log.info('Modal dismissed at: ' + new Date());
                });
            };

            /* Function edit item */
            $scope.edit = function (target) {
                var modalInstance = $uibModal.open({
                    backdrop: 'static',
                    animation: true,
                    size: 'lg',
                    templateUrl: configService.buildUrl('authorize/CuaHang', 'edit'),
                    controller: 'cuaHangEdit_Ctrl',
                    resolve: {
                        targetData: function () {
                            return target;
                        }
                    }
                });
                modalInstance.result.then(function (refundedData) {
                    $scope.refresh();
                }, function () {
                    $log.info('Modal dismissed at: ' + new Date());
                });
            };

            /* Function delete item */
            $scope.delete = function (event, target) {
                var modalInstance = $uibModal.open({
                    backdrop: 'static',
                    animation: true,
                    windowClass: 'modal-delete',
                    templateUrl: configService.buildUrl('authorize/CuaHang', 'delete'),
                    controller: 'cuaHangDelete_Ctrl',
                    resolve: {
                        targetData: function () {
                            return target;
                        }
                    }
                });
                modalInstance.result.then(function (refundedData) {
                    $scope.refresh();
                }, function () {
                    $log.info('Modal dismissed at: ' + new Date());
                });
            };
        }]);

    app.controller('cuaHangCreate_Ctrl', ['$scope', '$uibModalInstance', '$http', 'configService', 'cuaHangService', 'tempDataService', '$filter', '$uibModal', '$log',
        function ($scope, $uibModalInstance, $http, configService, service, tempDataService, $filter, $uibModal, $log) {
            $scope.config = angular.copy(configService);
            $scope.tempData = tempDataService.tempData;
            $scope.title = function () { return 'Đăng ký cửa hàng'; };
            $scope.target = {};
            //Tạo mới mã cửa hàng
            service.buildNewCode().then(function (successRes) {
                if (successRes && successRes.status == 200 && successRes.data) {
                    $scope.target.MA_CUAHANG = successRes.data;
                }
            });
            //end
            $scope.save = function () {
                if (!$scope.target.MA_CUAHANG || !$scope.target.TEN_CUAHANG) {
                    Lobibox.notify('warning', {
                        title: 'Thiếu thông tin',
                        msg: 'Thông tin đầu vào không hợp lệ! Dữ liệu (*) không được để trống',
                        delay: 4000
                    });
                } else {
                    service.post($scope.target).then(function (successRes) {
                        if (successRes && successRes.status === 200 && successRes.data && successRes.data.Status && successRes.data.Data) {
                            Lobibox.notify('success', {
                                title: 'Thông báo',
                                width: 400,
                                msg: successRes.data.Message,
                                delay: 1500
                            });
                            $uibModalInstance.close($scope.target);
                        } else {
                            Lobibox.notify('error', {
                                title: 'Xảy ra lỗi',
                                msg: 'Đã xảy ra lỗi! Thao tác không thành công',
                                delay: 3000
                            });
                        }
                    },
                    function (errorRes) {
                        console.log('errorRes', errorRes);
                    });
                }
            };
            $scope.cancel = function () {
                $uibModalInstance.close();
            };
        }]);
    app.controller('cuaHangDetail_Ctrl', ['$scope', '$uibModalInstance', '$http', 'configService', 'cuaHangService', 'targetData', 'tempDataService', '$filter', '$uibModal', '$log',
        function ($scope, $uibModalInstance, $http, configService, service, targetData, tempDataService, $filter, $uibModal, $log) {
            $scope.config = angular.copy(configService);
            $scope.tempData = tempDataService.tempData;
            $scope.target = {};
            $scope.target = angular.copy(targetData);
            $scope.title = function () { return 'Thông tin cửa hàng [' + targetData.MA_CUAHANG + ']'; };
            $scope.cancel = function () {
                $uibModalInstance.close();
            };
        }]);
    app.controller('cuaHangEdit_Ctrl', ['$scope', '$uibModalInstance', '$http', 'configService', 'cuaHangService', 'targetData', 'tempDataService', '$filter', '$uibModal', '$log',
        function ($scope, $uibModalInstance, $http, configService, service, targetData, tempDataService, $filter, $uibModal, $log) {
            $scope.config = angular.copy(configService);
            $scope.tempData = tempDataService.tempData;
            $scope.target = {};
            $scope.targetChildren = {};
            $scope.LST_EDIT = [];
            $scope.LST_DELETE = [];
            $scope.target = angular.copy(targetData);
            //thêm cửa hàng con
            $scope.isAddChildren = false;
            $scope.addChildren = function () {
                if ($scope.isAddChildren) {
                    $scope.isAddChildren = false;
                    $scope.targetChildren.MA_CUAHANG = '';
                }
                else {
                    $scope.isAddChildren = true;
                    //sinh mã cửa hàng chi nhánh
                    service.buildNewCodeChildren($scope.target.MA_CUAHANG).then(function (successRes) {
                        if (successRes && successRes.status == 200 && successRes.data) {
                            $scope.targetChildren.MA_CUAHANG = successRes.data;
                            $scope.targetChildren.MA_CUAHANG_CHA = $scope.target.MA_CUAHANG;
                        }
                    });
                    //end
                }
            };
            //end

            //get thông tin cửa hàng con
            $scope.listChildren = [];
            service.getAllChildren($scope.target.MA_CUAHANG).then(function (successRes) {
                if (successRes && successRes.status == 200 && successRes.data && successRes.data.Data.length > 0) {
                    $scope.listChildren = successRes.data.Data;
                }
            });
            //end
            $scope.title = function () { return 'Cập nhật thông tin cửa hàng'; };

            $scope.changed = function (item) {
                if (item) {
                    var edit = $filter('filter')($scope.LST_EDIT, { MA_CUAHANG: item.MA_CUAHANG }, true);
                    if (edit && edit.length === 1) {
                        edit.MA_CUAHANG_CHA = item.MA_CUAHANG_CHA;
                        edit.MA_CUAHANG = item.MA_CUAHANG;
                        edit.TEN_CUAHANG = item.TEN_CUAHANG;
                        edit.SODIENTHOAI = item.SODIENTHOAI;
                        edit.DIACHI = item.DIACHI;
                    } else {
                        $scope.LST_EDIT.push(item);
                    }
                }
            };

            $scope.removeChildren = function (item) {
                if (item) {
                    var confirmDelete = confirm("Bạn có muốn xóa cửa hàng chi nhánh " + item.MA_CUAHANG + " !" );
                    if (confirmDelete) {
                        var index = $scope.listChildren.findIndex(x => x.MA_CUAHANG === item.MA_CUAHANG);
                        if (index !== -1) {
                            $scope.LST_DELETE.push(item);
                            $scope.listChildren.splice(index, 1);
                        }
                    }
                }
            };

            $scope.save = function () {
                if (!$scope.target.MA_CUAHANG || !$scope.target.TEN_CUAHANG) {
                    Lobibox.notify('warning', {
                        title: 'Thiếu thông tin',
                        msg: 'Thông tin đầu vào không hợp lệ! Dữ liệu (*) không được để trống',
                        delay: 4000
                    });
                } else if ($scope.isAddChildren && !$scope.targetChildren.MA_CUAHANG) {
                    Lobibox.notify('warning', {
                        title: 'Thiếu thông tin',
                        msg: 'Thiếu mã cửa hàng chi nhánh! Dữ liệu (*) không được để trống',
                        delay: 3000
                    });
                } else if ($scope.isAddChildren && !$scope.targetChildren.TEN_CUAHANG) {
                    Lobibox.notify('warning', {
                        title: 'Thiếu thông tin',
                        msg: 'Thiếu tên cửa hàng chi nhánh! Dữ liệu (*) không được để trống',
                        delay: 3000
                    });
                } else if ($scope.isAddChildren && !$scope.targetChildren.SODIENTHOAI) {
                    Lobibox.notify('warning', {
                        title: 'Thiếu thông tin',
                        msg: 'Thiếu số điện thoại cửa hàng chi nhánh! Dữ liệu (*) không được để trống',
                        delay: 3000
                    });
                } else if ($scope.isAddChildren && !$scope.targetChildren.DIACHI) {
                    Lobibox.notify('warning', {
                        title: 'Thiếu thông tin',
                        msg: 'Thiếu địa chỉ cửa hàng chi nhánh! Dữ liệu (*) không được để trống',
                        delay: 3000
                    });
                } else {
                    var obj = {
                        LST_EDIT: $scope.LST_EDIT,
                        LST_DELETE: $scope.LST_DELETE,
                        RECORD_ADD: $scope.targetChildren
                    };
                    service.update(obj).then(function (successRes) {
                        if (successRes && successRes.status === 200 && successRes.data && successRes.data.Status && successRes.data.Data) {
                            Lobibox.notify('success', {
                                title: 'Thông báo',
                                width: 400,
                                msg: successRes.data.Message,
                                delay: 1500
                            });
                            $uibModalInstance.close($scope.target);
                        } else {
                            Lobibox.notify('error', {
                                title: 'Xảy ra lỗi',
                                msg: 'Đã xảy ra lỗi! Thao tác không thành công',
                                delay: 3000
                            });
                        }
                    },
                    function (errorRes) {
                        console.log('errorRes', errorRes);
                    });
                }
            };
            $scope.cancel = function () {
                $uibModalInstance.close();
            };
        }]);

    app.controller('cuaHangDelete_Ctrl', ['$scope', '$uibModalInstance', '$http', 'configService', 'cuaHangService', 'targetData', 'tempDataService', '$filter', '$uibModal', '$log',
       function ($scope, $uibModalInstance, $http, configService, service, targetData, tempDataService, $filter, $uibModal, $log) {
           $scope.config = angular.copy(configService);
           $scope.tempData = tempDataService.tempData;
           $scope.target = {};
           $scope.target = angular.copy(targetData);;
           $scope.title = function () { return 'Xóa cửa hàng [' + targetData.MA_CUAHANG + ']'; };
           $scope.delete = function () {
               service.delete($scope.target).then(function (successRes) {
                   if (successRes && successRes.status === 200 && successRes.data && successRes.data.Data) {
                       Lobibox.notify('success', {
                           title: 'Thông báo',
                           width: 400,
                           msg: successRes.data.Message,
                           delay: 1500
                       });
                       $uibModalInstance.close($scope.target);
                   } else {
                       Lobibox.notify('error', {
                           title: 'Xảy ra lỗi',
                           msg: 'Đã xảy ra lỗi! Thao tác không thành công',
                           delay: 3000
                       });
                   }
               },
               function (errorRes) {
                   console.log('errorRes', errorRes);
               });
           };
           $scope.cancel = function () {
               $uibModalInstance.close();
           };
       }]);
    return app;
});