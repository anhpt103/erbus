define(['ui-bootstrap', 'controllers/catalog/boHangController'], function () {
    'use strict';
    var app = angular.module('sapXepMatHangModule', ['ui.bootstrap', 'boHangModule']);
    app.factory('sapXepMatHangService', ['$http', 'configService', function ($http, configService) {
        var serviceUrl = configService.rootUrlWebApi + '/Catalog/BoHang';
        var selectedData = [];
        var result = {
            orderByMerchandise: function (data) {
                return $http.post(serviceUrl + '/OrderByMerchandise', data);
            }
        }
        return result;
    }]);
    /* controller list */
    app.controller('SapXepMatHang_Ctrl', ['$scope', '$http', 'configService', 'sapXepMatHangService', 'tempDataService', '$filter', '$uibModal', 'userService', 'securityService', 'closingService','boHangService',
        function ($scope, $http, configService, service, tempDataService, $filter, $uibModal, userService, securityService, closingService, boHangService) {
            $scope.config = angular.copy(configService);
            $scope.paged = angular.copy(configService.pageDefault);
            $scope.filtered = angular.copy(configService.filterDefault);
            $scope.tempData = tempDataService.tempData;
            $scope.title = function () { return 'Sắp xếp mặt hàng' };
            $scope.setPage = function (pageNo) {
                $scope.paged.CurrentPage = pageNo;
                filterData();
            };
            var editFlg = false;
            $scope.target = {};
            $scope.sortType = 'MAHANG';
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

            function loadDataByPackageMerchandise(maBoHang) {
                boHangService.getMatHangTrongBo(maBoHang).then(function (response) {
                    if (response && response.status === 200 && response.data && response.data.Status) {
                        $scope.listOrder = {
                            selected: null,
                            listData: response.data.Data
                        };
                    }
                });
            };

            function setDefaultSelectDropDown(listBoHang) {
                if (listBoHang.length === 1) {
                    $scope.target.MABOHANG = listBoHang[0].VALUE;
                    loadDataByPackageMerchandise(listBoHang[0].VALUE);
                }
            };

            $scope.editData = function (index) { editFlg = true; };
            //Function load data catalog LoaiHang
            function loadDataBoHang() {
                $scope.boHang = [];
                if (!tempDataService.tempData('boHang')) {
                    boHangService.getAllData().then(function (successRes) {
                        if (successRes && successRes.status === 200 && successRes.data && successRes.data.Status && successRes.data.Data && successRes.data.Data.length > 0) {
                            tempDataService.putTempData('boHang', successRes.data.Data);
                            $scope.boHang = successRes.data.Data;
                            setDefaultSelectDropDown($scope.boHang);
                        }
                    }, function (errorRes) {
                        console.log('errorRes', errorRes);
                    });
                } else {
                    $scope.boHang = tempDataService.tempData('boHang');
                    setDefaultSelectDropDown($scope.boHang);
                }
            };
            //end

            $scope.selectionBoHang = function (maBoHang) {
                if (maBoHang) {
                    loadDataByPackageMerchandise(maBoHang);
                }
            };

            $scope.orderBy = function () {
                if (!$scope.listOrder || !$scope.listOrder.listData) {
                    Lobibox.notify('warning', {
                        position: 'bottom left',
                        msg: 'Không có dòng dữ liệu! Không thể sắp xếp'
                    });
                    return;
                }

                if (!editFlg && $scope.listOrder && $scope.listOrder.listData.length > 0) {
                    Lobibox.notify('info', {
                        position: 'bottom left',
                        msg: 'Bạn chưa thực hiện sắp xếp dữ liệu! Sắp xếp sau đó thực hiện lại thao tác này'
                    });
                    return;
                } else {
                    angular.forEach($scope.listOrder.listData, function (v, k) {
                        v.SAPXEP = k + 1;
                    });
                    service.orderByMerchandise($scope.listOrder.listData).then(function (response) {
                        if (response && response.status === 200 && response.data && response.data.Status) {
                            editFlg = false;
                            Lobibox.notify('success', {
                                position: 'bottom left',
                                msg: response.data.Message
                            });
                        } else {
                            Lobibox.notify('error', {
                                position: 'bottom left',
                                msg: response.data.Message
                            });
                        }
                    });
                }
            };

            function filterData() {
                if ($scope.accessList.XEM) {
                    loadDataBoHang();
                }
            };

            //check authorize
            function loadAccessList() {
                var currentUser = userService.GetCurrentUser();
                var userName = currentUser.userName;
                var unitCodeParam = !currentUser.parentUnitCode ? currentUser.unitCode : currentUser.parentUnitCode;
                securityService.getAccessList('SapXepMatHang', userName, unitCodeParam).then(function (successRes) {
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
                closingService.closingOutList().then(function (successRes) {
                    if (successRes && successRes.status === 200 && successRes.data) {
                        console.log('Khóa sổ thành công');
                    }
                });
            };
            //end function loadAccessList()
            loadAccessList();
        }]);
    return app;
});
