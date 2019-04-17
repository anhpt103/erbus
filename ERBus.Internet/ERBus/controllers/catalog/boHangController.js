define(['ui-bootstrap'], function () {
    'use strict';
    var app = angular.module('boHangModule', ['ui.bootstrap']);
    app.factory('boHangService', ['$http', 'configService', function ($http, configService) {
        var serviceUrl = configService.rootUrlWebApi + '/Catalog/BoHang';
        var selectedData = [];
        var result = {
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
    app.controller('BoHang_Ctrl', ['$scope', '$http', 'configService', 'boHangService', 'tempDataService', '$filter', '$uibModal', '$log', 'securityService',
        function ($scope, $http, configService, service, tempDataService, $filter, $uibModal, $log, securityService) {
            $scope.config = angular.copy(configService);
            $scope.paged = angular.copy(configService.pageDefault);
            $scope.filtered = angular.copy(configService.filterDefault);
            $scope.tempData = tempDataService.tempData;
            $scope.title = function () { return 'Bó hàng' };
            $scope.data = [];
            $scope.setPage = function (pageNo) {
                $scope.paged.CurrentPage = pageNo;
                filterData();
            };
            $scope.sortType = 'MABOHANG';
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
                if ($scope.accessList.VIEW) {
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
                securityService.getAccessList('BoHang').then(function (successRes) {
                    if (successRes && successRes.status == 200 && successRes.data) {
                        $scope.accessList = successRes.data;
                        if (!$scope.accessList.VIEW) {
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
                    templateUrl: configService.buildUrl('catalog/BoHang', 'create'),
                    controller: 'boHangCreate_Ctrl',
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
                    templateUrl: configService.buildUrl('catalog/BoHang', 'detail'),
                    controller: 'boHangDetail_Ctrl',
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
                    templateUrl: configService.buildUrl('catalog/BoHang', 'edit'),
                    controller: 'boHangEdit_Ctrl',
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
                    templateUrl: configService.buildUrl('catalog/BoHang', 'delete'),
                    controller: 'boHangDelete_Ctrl',
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

    app.controller('boHangCreate_Ctrl', ['$scope', '$uibModalInstance', '$http', 'configService', 'boHangService', 'tempDataService', '$filter', '$uibModal', '$log',
        function ($scope, $uibModalInstance, $http, configService, service, tempDataService, $filter, $uibModal, $log) {
            $scope.config = angular.copy(configService);
            $scope.tempData = tempDataService.tempData;
            $scope.title = function () { return 'Thêm bó hàng sản phẩm'; };
            $scope.target = {};
            //Tạo mới mã loại
            service.buildNewCode().then(function (successRes) {
                if (successRes && successRes.status == 200 && successRes.data) {
                    $scope.target.MABOHANG = successRes.data;
                }
            });
            //end
            $scope.save = function () {
                if (!$scope.target.MABOHANG || !$scope.target.TENBOHANG ) {
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
    app.controller('boHangDetail_Ctrl', ['$scope', '$uibModalInstance', '$http', 'configService', 'boHangService', 'targetData', 'tempDataService', '$filter', '$uibModal', '$log',
        function ($scope, $uibModalInstance, $http, configService, service, targetData, tempDataService, $filter, $uibModal, $log) {
            $scope.config = angular.copy(configService);
            $scope.tempData = tempDataService.tempData;
            $scope.target = {};
            $scope.target = angular.copy(targetData);
            $scope.title = function () { return 'Thông tin bó hàng sản phẩm [' + targetData.MABOHANG + ']'; };
            $scope.cancel = function () {
                $uibModalInstance.close();
            };
        }]);
    app.controller('boHangEdit_Ctrl', ['$scope', '$uibModalInstance', '$http', 'configService', 'boHangService', 'targetData', 'tempDataService', '$filter', '$uibModal', '$log',
        function ($scope, $uibModalInstance, $http, configService, service, targetData, tempDataService, $filter, $uibModal, $log) {
            $scope.config = angular.copy(configService);
            $scope.tempData = tempDataService.tempData;
            $scope.target = {};
            $scope.target = angular.copy(targetData);
            $scope.title = function () { return 'Chỉnh sửa bó hàng sản phẩm'; };
            $scope.save = function () {
                if (!$scope.target.MABOHANG || !$scope.target.TENBOHANG ) {
                    Lobibox.notify('warning', {
                        title: 'Thiếu thông tin',
                        msg: 'Thông tin đầu vào không hợp lệ! Dữ liệu (*) không được để trống',
                        delay: 4000
                    });
                } else {
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

    app.controller('boHangDelete_Ctrl', ['$scope', '$uibModalInstance', '$http', 'configService', 'boHangService', 'targetData', 'tempDataService', '$filter', '$uibModal', '$log',
       function ($scope, $uibModalInstance, $http, configService, service, targetData, tempDataService, $filter, $uibModal, $log) {
           $scope.config = angular.copy(configService);
           $scope.tempData = tempDataService.tempData;
           $scope.target = {};
           $scope.target = angular.copy(targetData);;
           $scope.title = function () { return 'Xóa bó hàng sản phẩm [' + targetData.MABOHANG + ']'; };
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