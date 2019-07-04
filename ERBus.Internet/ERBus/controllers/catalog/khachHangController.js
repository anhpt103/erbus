define(['ui-bootstrap'], function () {
    'use strict';
    var app = angular.module('khachHangModule', ['ui.bootstrap']);
    app.factory('khachHangService', ['$http', 'configService', function ($http, configService) {
        var serviceUrl = configService.rootUrlWebApi + '/Catalog/KhachHang';
        var selectedData = [];
        var result = {
            getAllData: function () {
                return $http.get(serviceUrl + '/GetAllData');
            },
            postQuery: function (data) {
                return $http.post(serviceUrl + '/PostQuery', data);
            },
            buildNewCode: function () {
                return $http.get(serviceUrl + '/BuildNewCode');
            },
            post: function (data) {
                return $http.post(serviceUrl + '/Post', data);
            },
            update: function (params) {
                return $http.put(serviceUrl + '/' + params.ID, params);
            },
            delete: function (params) {
                return $http.delete(serviceUrl + '/' + params.ID, params);
            }
        }
        return result;
    }]);
    /* controller list */
    app.controller('KhachHang_Ctrl', ['$scope', '$http', 'configService', 'khachHangService', 'tempDataService', '$filter', '$uibModal', '$log', 'securityService',
        function ($scope, $http, configService, service, tempDataService, $filter, $uibModal, $log, securityService) {
            $scope.config = angular.copy(configService);
            $scope.paged = angular.copy(configService.pageDefault);
            $scope.filtered = angular.copy(configService.filterDefault);
            $scope.tempData = tempDataService.tempData;
            $scope.title = function () { return 'Khách hàng' };
            $scope.data = [];
            $scope.setPage = function (pageNo) {
                $scope.paged.CurrentPage = pageNo;
                filterData();
            };
            $scope.sortType = 'MAKHACHHANG';
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
                securityService.getAccessList('KhachHang').then(function (successRes) {
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
                    //windowClass: 'catalog-window',
                    size: 'lg',
                    templateUrl: configService.buildUrl('catalog/KhachHang', 'create'),
                    controller: 'khachHangCreate_Ctrl',
                    resolve: {}
                });
                modalInstance.result.then(function (refundedData) {
                    $scope.refresh();
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
                    templateUrl: configService.buildUrl('catalog/KhachHang', 'detail'),
                    controller: 'khachHangDetail_Ctrl',
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
                    templateUrl: configService.buildUrl('catalog/KhachHang', 'edit'),
                    controller: 'khachHangEdit_Ctrl',
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
                    templateUrl: configService.buildUrl('catalog/KhachHang', 'delete'),
                    controller: 'khachHangDelete_Ctrl',
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

    app.controller('khachHangCreate_Ctrl', ['$scope', '$uibModalInstance', '$http', 'configService', 'khachHangService', 'tempDataService', '$filter', '$uibModal', '$log',
        function ($scope, $uibModalInstance, $http, configService, service, tempDataService, $filter, $uibModal, $log) {
            $scope.config = angular.copy(configService);
            $scope.tempData = tempDataService.tempData;
            $scope.title = function () { return 'Thêm khách hàng'; };
            $scope.target = {};
            //Tạo mới mã loại
            service.buildNewCode().then(function (successRes) {
                if (successRes && successRes.status == 200 && successRes.data) {
                    $scope.target.MAKHACHHANG = successRes.data;
                    $scope.target.MATHE = $scope.target.MAKHACHHANGs;
                }
            });
            //end
            $scope.save = function () {
                if (!$scope.target.MAKHACHHANG || !$scope.target.TENKHACHHANG ) {
                    Lobibox.notify('warning', {
                        title: 'Thiếu thông tin',
                        msg: 'Thông tin đầu vào không hợp lệ! Dữ liệu (*) không được để trống',
                        delay: 4000
                    });
                } else {
                    $scope.target.NGAYSINH = $scope.config.moment($scope.target.NGAYSINH).format();
                    $scope.target.NGAYDACBIET = $scope.config.moment($scope.target.NGAYDACBIET).format();
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
    app.controller('khachHangDetail_Ctrl', ['$scope', '$uibModalInstance', '$http', 'configService', 'khachHangService', 'targetData', 'tempDataService', '$filter', '$uibModal', '$log',
        function ($scope, $uibModalInstance, $http, configService, service, targetData, tempDataService, $filter, $uibModal, $log) {
            $scope.config = angular.copy(configService);
            $scope.tempData = tempDataService.tempData;
            $scope.target = {};
            $scope.target = angular.copy(targetData);
            $scope.target.NGAYSINH = new Date($scope.target.NGAYSINH);
            $scope.target.NGAYDACBIET = new Date($scope.target.NGAYDACBIET);
            $scope.title = function () { return 'Thông tin khách hàng [' + targetData.MAKHACHHANG + ']'; };
            $scope.cancel = function () {
                $uibModalInstance.close();
            };
        }]);
    app.controller('khachHangEdit_Ctrl', ['$scope', '$uibModalInstance', '$http', 'configService', 'khachHangService', 'targetData', 'tempDataService', '$filter', '$uibModal', '$log',
        function ($scope, $uibModalInstance, $http, configService, service, targetData, tempDataService, $filter, $uibModal, $log) {
            $scope.config = angular.copy(configService);
            $scope.tempData = tempDataService.tempData;
            $scope.target = {};
            $scope.target = angular.copy(targetData);
            $scope.target.NGAYSINH = new Date($scope.target.NGAYSINH);
            $scope.target.NGAYDACBIET = new Date($scope.target.NGAYDACBIET);
            $scope.title = function () { return 'Chỉnh sửa khách hàng'; };
            $scope.save = function () {
                if (!$scope.target.MAKHACHHANG || !$scope.target.TENKHACHHANG ) {
                    Lobibox.notify('warning', {
                        title: 'Thiếu thông tin',
                        msg: 'Thông tin đầu vào không hợp lệ! Dữ liệu (*) không được để trống',
                        delay: 4000
                    });
                } else {
                    $scope.target.NGAYSINH = $scope.config.moment($scope.target.NGAYSINH).format();
                    $scope.target.NGAYDACBIET = $scope.config.moment($scope.target.NGAYDACBIET).format();
                    service.update($scope.target).then(function (successRes) {
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

    app.controller('khachHangDelete_Ctrl', ['$scope', '$uibModalInstance', '$http', 'configService', 'khachHangService', 'targetData', 'tempDataService', '$filter', '$uibModal', '$log',
       function ($scope, $uibModalInstance, $http, configService, service, targetData, tempDataService, $filter, $uibModal, $log) {
           $scope.config = angular.copy(configService);
           $scope.tempData = tempDataService.tempData;
           $scope.target = {};
           $scope.target = angular.copy(targetData);;
           $scope.title = function () { return 'Xóa khách hàng [' + targetData.MAKHACHHANG + ']'; };
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

    app.controller('khachHangSearch_Ctrl', ['$scope', '$uibModalInstance', '$http', 'configService', 'khachHangService', 'tempDataService', '$filter', '$uibModal', 'filterObject',
        function ($scope, $uibModalInstance, $http, configService, service, tempDataService, $filter, $uibModal, filterObject) {
            $scope.config = angular.copy(configService);;
            $scope.paged = angular.copy(configService.pageDefault);
            $scope.filtered = angular.copy(configService.filterDefault);
            $scope.title = function () { return 'Danh sách khách hàng'; };
            $scope.isLoading = false;
            $scope.sortType = 'MAKHACHHANG';
            $scope.sortReverse = false;
            $scope.listSelectedData = [];
            if (filterObject && filterObject.ISSELECT_POST && filterObject.ISSELECT_POST.length > 0) {
                angular.forEach(filterObject.ISSELECT_POST, function (v, k) {
                    var obj = {
                        MAKHACHHANG: v.VALUE,
                        TENKHACHHANG: v.DESCRIPTION,
                        ISSELECT: true
                    };
                    $scope.listSelectedData.push(obj);
                });
            }
            function filterData() {
                $scope.isLoading = true;
                var postdata = { paged: $scope.paged, filtered: $scope.filtered };
                service.postQuery(postdata).then(function (successRes) {
                    if (successRes && successRes.status === 200 && successRes.data && successRes.data.Status && successRes.data.Data && successRes.data.Data.Data) {
                        $scope.isLoading = false;
                        $scope.data = successRes.data.Data.Data;
                        angular.forEach($scope.data, function (v, k) {
                            var isSelected = $filter('filter')($scope.listSelectedData, { MAKHACHHANG: v.MAKHACHHANG }, true);
                            if (isSelected && isSelected.length === 1) {
                                $scope.data[k].ISSELECT = isSelected[0].ISSELECT;
                            } else {
                                $scope.data[k].ISSELECT = false;
                            }
                        });
                        angular.extend($scope.paged, successRes.data.Data);
                    }
                }, function (errorRes) {
                    $scope.isLoading = false;
                    console.log(errorRes);
                });
            };
            filterData();
            $scope.setPage = function (pageNo) {
                $scope.paged.currentPage = pageNo;
                filterData();
            };
            $scope.doSearch = function () {
                $scope.paged.currentPage = 1;
                filterData();
            };
            $scope.pageChanged = function () {
                filterData();
            };
            $scope.refresh = function () {
                $scope.setPage($scope.paged.currentPage);
            };
            $scope.doCheck = function (item) {
                if (item) {
                    var checkExistList = $filter('filter')($scope.listSelectedData, { MAKHACHHANG: item.MAKHACHHANG }, true);
                    if (checkExistList && checkExistList.length === 1) {
                        checkExistList[0].ISSELECT = item.ISSELECT;
                    } else {
                        $scope.listSelectedData.push(item);
                    }
                }
            };
            $scope.choice = function () {
                var listChaged = $filter('filter')($scope.data, { ISSELECT: true }, false);
                $uibModalInstance.close(listChaged);
            };
            $scope.cancel = function () {
                $uibModalInstance.close();
            };
        }]);
    return app;
});