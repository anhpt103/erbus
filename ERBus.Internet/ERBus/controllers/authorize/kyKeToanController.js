define(['ui-bootstrap'], function () {
    'use strict';
    var app = angular.module('kyKeToanModule', ['ui.bootstrap']);
    app.factory('kyKeToanService', ['$http', 'configService', function ($http, configService) {
        var serviceUrl = configService.rootUrlWebApi + '/Authorize/KyKeToan';
        var selectedData = [];
        var result = {
            getAllData: function () {
                return $http.get(serviceUrl + '/GetAllData');
            },
            getKyKeToan: function () {
                return $http.get(serviceUrl + '/GetKyKeToan');
            },
            postQuery: function (data) {
                return $http.post(serviceUrl + '/PostQuery', data);
            },
            post: function (data) {
                return $http.post(serviceUrl + '/Post', data);
            },
            update: function (params) {
                return $http.put(serviceUrl + '/' + params.ID, params);
            },
            closingOutPeriod: function (data) {
                return $http.post(serviceUrl + '/ClosingOutPeriod', data);
            },
            closingOutListPeriodNotLock: function (listPeriod) {
                return $http.post(serviceUrl + '/ClosingOutListPeriodNotLock', listPeriod);
            },
            getLastestPeriod: function () {
                return $http.get(serviceUrl + '/GetLastestPeriod');
            }
        }
        return result;
    }]);
    /* controller list */
    app.controller('KyKeToan_Ctrl', ['$scope', '$http', 'configService', 'kyKeToanService', 'tempDataService', '$filter', '$uibModal', '$log', 'securityService',
        function ($scope, $http, configService, service, tempDataService, $filter, $uibModal, $log, securityService) {
            $scope.config = angular.copy(configService);
            $scope.paged = angular.copy(configService.pageDefault);
            $scope.filtered = angular.copy(configService.filterDefault);
            $scope.tempData = tempDataService.tempData;
            $scope.title = function () { return 'Kỳ kế toán' };
            $scope.data = [];
            $scope.target = {};
            $scope.target.NAM = new Date().getFullYear();
            $scope.setPage = function (pageNo) {
                $scope.paged.CurrentPage = pageNo;
                filterData();
            };
            $scope.sortType = 'KY';
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
                securityService.getAccessList('KyKeToan').then(function (successRes) {
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
            $scope.create = function (year) {
                if (year) {
                    var modalInstance = $uibModal.open({
                        backdrop: 'static',
                        animation: true,
                        size: 'sm',
                        templateUrl: configService.buildUrl('authorize/KyKeToan', 'create'),
                        controller: 'kyKeToanCreate_Ctrl',
                        resolve: {
                            targetData: function () {
                                return year;
                            }
                        }
                    });
                    modalInstance.result.then(function (refundedData) {
                        $scope.refresh();
                    }, function () {
                        $log.info('Modal dismissed at: ' + new Date());
                    });
                }
            };

            /* Function details Item */
            $scope.detail = function (target) {
                var modalInstance = $uibModal.open({
                    backdrop: 'static',
                    animation: true,
                    size: 'lg',
                    templateUrl: configService.buildUrl('authorize/KyKeToan', 'detail'),
                    controller: 'kyKeToanDetail_Ctrl',
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
                    templateUrl: configService.buildUrl('authorize/KyKeToan', 'edit'),
                    controller: 'kyKeToanEdit_Ctrl',
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

            $scope.closingOut = function (target) {
                target.TUNGAY = $scope.config.moment(target.TUNGAY).format();
                target.DENNGAY = $scope.config.moment(target.DENNGAY).format();
                service.closingOutPeriod(target).then(function (successRes) {
                    if (successRes && successRes.status === 200 && successRes.data && successRes.data.Status) {
                        Lobibox.notify('success', {
                            title: 'Thông báo',
                            width: 400,
                            msg: successRes.data.Message,
                            delay: 2500
                        });
                        $scope.refresh();
                    }
                    else {
                        Lobibox.notify('error', {
                            title: 'Thông báo',
                            width: 400,
                            msg: successRes.data.Message,
                            delay: 4000
                        });
                    }
                });
            };
        }]);
    app.controller('kyKeToanCreate_Ctrl', ['$scope', '$uibModalInstance', '$http', 'configService', 'kyKeToanService', 'targetData', 'tempDataService', '$filter', '$uibModal', '$log',
        function ($scope, $uibModalInstance, $http, configService, service, targetData, tempDataService, $filter, $uibModal, $log) {
            $scope.config = angular.copy(configService);
            $scope.tempData = tempDataService.tempData;
            $scope.target = {};
            $scope.title = function () { return 'Tạo mới kỳ kế toán năm [' + targetData + ']'; };
            $scope.target.NAM = targetData;
            $scope.save = function () {
                if (!$scope.target.NAM) {
                    Lobibox.notify('warning', {
                        title: 'Thiếu thông tin',
                        msg: 'Thông tin đầu vào không hợp lệ! Dữ liệu (*) không được để trống',
                        delay: 4000
                    });
                } else {
                    service.post(JSON.stringify({ NAM: $scope.target.NAM })).then(function (successRes) {
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
    app.controller('kyKeToanDetail_Ctrl', ['$scope', '$uibModalInstance', '$http', 'configService', 'kyKeToanService', 'targetData', 'tempDataService', '$filter', '$uibModal', '$log',
        function ($scope, $uibModalInstance, $http, configService, service, targetData, tempDataService, $filter, $uibModal, $log) {
            $scope.config = angular.copy(configService);
            $scope.tempData = tempDataService.tempData;
            $scope.target = {};
            $scope.target = angular.copy(targetData);
            $scope.target.TUNGAY = new Date($scope.target.TUNGAY);
            $scope.title = function () { return 'Thông tin kỳ kế toán [' + targetData.KY + ']'; };
            $scope.cancel = function () {
                $uibModalInstance.close();
            };
        }]);

    app.controller('kyKeToanEdit_Ctrl', ['$scope', '$uibModalInstance', '$http', 'configService', 'kyKeToanService', 'targetData', 'tempDataService', '$filter', '$uibModal', '$log',
        function ($scope, $uibModalInstance, $http, configService, service, targetData, tempDataService, $filter, $uibModal, $log) {
            $scope.config = angular.copy(configService);
            $scope.tempData = tempDataService.tempData;
            $scope.target = {};
            $scope.target = angular.copy(targetData);
            $scope.target.TUNGAY = new Date($scope.target.TUNGAY);
            $scope.target.DENNGAY = new Date($scope.target.DENNGAY);
            $scope.title = function () { return 'Chỉnh sửa kỳ kế toán'; };
            $scope.save = function () {
                if (!$scope.target.KY || !$scope.target.TENKY) {
                    Lobibox.notify('warning', {
                        title: 'Thiếu thông tin',
                        msg: 'Thông tin đầu vào không hợp lệ! Dữ liệu (*) không được để trống',
                        delay: 4000
                    });
                } else {
                    $scope.target.TUNGAY = $scope.config.moment($scope.target.TUNGAY).format();
                    $scope.target.DENNGAY = $scope.config.moment($scope.target.DENNGAY).format();
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
    return app;
});