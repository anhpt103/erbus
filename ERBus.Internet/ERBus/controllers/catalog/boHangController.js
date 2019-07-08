define(['ui-bootstrap', 'controllers/catalog/donViTinhController', 'controllers/catalog/matHangController'], function () {
    'use strict';
    var app = angular.module('boHangModule', ['ui.bootstrap', 'donViTinhModule', 'matHangModule']);
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
            },
            getAllData: function () {
                return $http.get(serviceUrl + '/GetAllData');
            },
            getDetails: function (ID) {
                return $http.get(serviceUrl + '/GetDetails/' + ID);
            },
            getMatHangTrongBo: function (maBoHang) {
                return $http.get(serviceUrl + '/GetMatHangTrongBo/' + maBoHang);
            }
        }
        return result;
    }]);
    /* controller list */
    app.controller('BoHang_Ctrl', ['$scope', '$http', 'configService', 'boHangService', 'tempDataService', '$filter', '$uibModal', '$log', 'securityService','userService',
        function ($scope, $http, configService, service, tempDataService, $filter, $uibModal, $log, securityService, userService) {
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
                var currentUser = userService.GetCurrentUser();
                var userName = currentUser.userName;
                var unitCodeParam = !currentUser.parentUnitCode ? currentUser.unitCode : currentUser.parentUnitCode;
                securityService.getAccessList('BoHang', userName, unitCodeParam).then(function (successRes) {
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
                    windowClass: 'modal-matHang',
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
                    windowClass: 'modal-matHang',
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
                    windowClass: 'modal-matHang',
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

    app.controller('boHangCreate_Ctrl', ['$scope', '$uibModalInstance', '$http', 'configService', 'boHangService', 'tempDataService', '$filter', '$uibModal', '$log', 'userService', 'matHangService', 'donViTinhService',
        function ($scope, $uibModalInstance, $http, configService, service, tempDataService, $filter, $uibModal, $log, userService, matHangService, donViTinhService) {
            $scope.config = angular.copy(configService);
            $scope.paged = angular.copy(configService.pageDefault);
            $scope.tempData = tempDataService.tempData;
            var currentUser = userService.GetCurrentUser();
            var unitCode = currentUser.unitCode;
            $scope.title = function () { return 'Thêm bó hàng sản phẩm'; };
            $scope.target = {
                TONGCHIETKHAU: 0,
                TONGTIEN: 0,
                TONGTIEN_TRUOCCHIETKHAU: 0,
                TRANGTHAI: 10,
                DataDetails: []
            };
            //Tạo mới mã loại
            service.buildNewCode().then(function (successRes) {
                if (successRes && successRes.status == 200 && successRes.data) {
                    $scope.target.MABOHANG = successRes.data;
                }
            });
            //end
            //Function load data catalog DonViTinh
            function loadDataDonViTinh() {
                $scope.donViTinh = [];
                if (!tempDataService.tempData('donViTinh')) {
                    donViTinhService.getAllData().then(function (successRes) {
                        if (successRes && successRes.status === 200 && successRes.data && successRes.data.Status && successRes.data.Data && successRes.data.Data.length > 0) {
                            tempDataService.putTempData('donViTinh', successRes.data.Data);
                            $scope.donViTinh = successRes.data.Data;
                        }
                    }, function (errorRes) {
                        console.log('errorRes', errorRes);
                    });
                } else {
                    $scope.donViTinh = tempDataService.tempData('donViTinh');
                }
            };
            loadDataDonViTinh();
            //end
            $scope.convertCodeToName = function (paraValue, moduleName) {
                if (paraValue) {
                    var tempCache = $filter('filter')($scope.tempData(moduleName), { VALUE: paraValue }, true);
                    if (tempCache && tempCache.length === 1) {
                        return tempCache[0].DESCRIPTION;
                    } else {
                        return paraValue;
                    }
                }
            };

            $scope.pageChanged = function () {
                var currentPage = $scope.paged.CurrentPage;
                var itemsPerPage = $scope.paged.ItemsPerPage;
                $scope.paged.TotalItems = $scope.target.DataDetails.length;
                $scope.data = [];
                if ($scope.target.DataDetails) {
                    for (var i = (currentPage - 1) * itemsPerPage; i < currentPage * itemsPerPage && i < $scope.target.DataDetails.length; i++) {
                        $scope.data.push($scope.target.DataDetails[i]);
                    }
                }
            };

            function TinhTongTien(listObj, name) {
                var total = 0;
                if (listObj && listObj.length > 0) {
                    angular.forEach(listObj, function (obj, idx) {
                        var increase = obj[name];
                        if (!increase) {
                            increase = 0;
                        }
                        total += increase;
                    });
                }
                return total;
            };

            $scope.changedSoLuong = function (item) {
                if (item) {
                    if (!item.SOLUONG) item.SOLUONG = 1;
                    if (!item.CHIETKHAU) item.CHIETKHAU = 0;
                    item.TONGTIEN_TRUOCCHIETKHAU = item.SOLUONG * item.GIABANLE_VAT;
                    if (item.CHIETKHAU <= 100) {
                        //chiết khấu theo tỷ lệ
                        item.TONGTIEN = item.SOLUONG * (item.GIABANLE_VAT - (item.GIABANLE_VAT * item.CHIETKHAU / 100));
                    } else {
                        //chiết khấu theo tiền
                        item.TONGTIEN = item.SOLUONG * (item.GIABANLE_VAT - item.CHIETKHAU);
                    }
                }
            };
            $scope.changedChietKhau = function (item) {
                if (item) {
                    if (!item.SOLUONG) item.SOLUONG = 1;
                    if (!item.CHIETKHAU) item.CHIETKHAU = 0;
                    item.TONGTIEN_TRUOCCHIETKHAU = item.SOLUONG * item.GIABANLE_VAT;
                    if (item.CHIETKHAU <= 100) {
                        //chiết khấu theo tỷ lệ
                        item.TONGTIEN = item.SOLUONG * (item.GIABANLE_VAT - (item.GIABANLE_VAT * item.CHIETKHAU / 100));
                    } else {
                        //chiết khấu theo tiền
                        item.TONGTIEN = item.SOLUONG * (item.GIABANLE_VAT - item.CHIETKHAU);
                    }
                }
                $scope.$watch("target.DataDetails", function (newValue, oldValue) {
                    $scope.target.TONGCHIETKHAU = TinhTongTien($scope.target.DataDetails, 'CHIETKHAU');
                }, true);
            };

            //function search mathang
            $scope.searchMatHang = function (strKey) {
                if (strKey) {
                    var modalInstance = $uibModal.open({
                        backdrop: 'static',
                        templateUrl: configService.buildUrl('catalog/MatHang', 'search'),
                        controller: 'matHangSearch_Ctrl',
                        windowClass: 'search-window',
                        resolve: {
                            serviceSelectData: function () {
                                return matHangService;
                            },
                            filterObject: function () {
                                return {
                                    keySearch: strKey,
                                };
                            }
                        }
                    });
                    modalInstance.result.then(function (refundedData) {
                        if (refundedData) {
                            $scope.addItem = {
                                MAHANG: refundedData.MAHANG,
                                TENHANG: refundedData.TENHANG,
                                MADONVITINH: refundedData.MADONVITINH,
                                GIABANLE_VAT: refundedData.GIABANLE_VAT,
                                CHIETKHAU: 0,
                                SOLUONG: 1,
                                TONGTIEN: SOLUONG * (GIABANLE_VAT - (GIABANLE_VAT * CHIETKHAU / 100)),
                                TONGTIEN_TRUOCCHIETKHAU: SOLUONG * GIABANLE_VAT,
                                INDEX: 0
                            };
                            $scope.pageChanged();
                        }
                    }, function () {
                    });
                }
            };
            //end function search

            //CHANGED MAHANG ADD ITEM
            $scope.changedMaHang = function (maHang) {
                if (maHang) {
                    var obj = {
                        MAHANG: maHang,
                        UNITCODE: unitCode
                    }
                    matHangService.getMatHangTheoDieuKien(obj).then(function (successRes) {
                        if (successRes && successRes.status === 200 && successRes.data && successRes.data.Status && successRes.data.Data) {
                            $scope.addItem.MAHANG = successRes.data.Data.MAHANG;
                            $scope.addItem.TENHANG = successRes.data.Data.TENHANG;
                            $scope.addItem.MADONVITINH = successRes.data.Data.MADONVITINH;
                            $scope.addItem.GIABANLE_VAT = successRes.data.Data.GIABANLE_VAT;
                            $scope.addItem.CHIETKHAU = 0;
                            $scope.addItem.SOLUONG = 1;
                            $scope.addItem.TONGTIEN = $scope.addItem.SOLUONG * ($scope.addItem.GIABANLE_VAT - ($scope.addItem.GIABANLE_VAT * $scope.addItem.CHIETKHAU / 100));
                            $scope.addItem.TONGTIEN_TRUOCCHIETKHAU = $scope.addItem.SOLUONG * $scope.addItem.GIABANLE_VAT;
                            document.getElementById('_soLuongAddItem').focus();
                            document.getElementById('_soLuongAddItem').select();
                        }
                        else {
                            //bật lên modal tìm kiếm mặt hàng
                            $scope.searchMatHang(maHang);
                        }
                    });
                }
            };

            //add Row
            $scope.addRow = function () {
                if ($scope.addItem && $scope.addItem.MAHANG !== '') {
                    var exist = $filter('filter')($scope.target.DataDetails, { MAHANG: $scope.addItem.MAHANG }, true);
                    if (exist && exist.length == 1) {
                        exist[0].SOLUONG = exist[0].SOLUONG + $scope.addItem.SOLUONG;
                        exist[0].GIABANLE_VAT = $scope.addItem.GIABANLE_VAT;
                        exist[0].TONGTIEN = Math.round(100 * (exist[0].SOLUONG * exist[0].GIABANLE_VAT)) / 100;
                        exist[0].TONGTIEN_TRUOCCHIETKHAU = exist[0].SOLUONG * exist[0].GIABANLE_VAT;
                        Lobibox.notify('success', {
                            title: 'Thông báo',
                            width: 400,
                            msg: "Đã tồn tại mã hàng " + exist[0].MAHANG + ". Cộng gộp!",
                            delay: 1500
                        });
                    }
                    else {
                        $scope.target.DataDetails.push($scope.addItem);
                    }
                    $scope.pageChanged();
                    $scope.addItem = {
                        MAHANG: '',
                        TENHANG: '',
                        MADONVITINH: '',
                        SOLUONG: 1,
                        GIABANLE_VAT: 0,
                        CHIETKHAU: 0,
                        TONGTIEN: 0,
                        TONGTIEN_TRUOCCHIETKHAU: 0
                    };
                    focus('_maHangAddItem');
                    document.getElementById('_maHangAddItem').focus();
                    document.getElementById('_maHangAddItem').select();
                }
            };
            //end add row

            $scope.removeItem = function (index) {
                var currentPage = $scope.paged.CurrentPage;
                var itemsPerPage = 10;
                var currentPageIndex = (currentPage - 1) * itemsPerPage + index;
                $scope.target.DataDetails.splice(currentPageIndex, 1);
                $scope.pageChanged();
            };

            function listenerDataDetails() {
                $scope.$watch("target.DataDetails", function (newValue, oldValue) {
                    $scope.target.TONGCHIETKHAU = TinhTongTien($scope.target.DataDetails, 'CHIETKHAU');
                    $scope.target.TONGTIEN = TinhTongTien($scope.target.DataDetails, 'TONGTIEN');
                }, true);
            };
            listenerDataDetails();

            $scope.save = function () {
                if (!$scope.target.MABOHANG || !$scope.target.TENBOHANG) {
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
            $scope.paged = angular.copy(configService.pageDefault);
            $scope.tempData = tempDataService.tempData;
            $scope.target = {};
            $scope.target = angular.copy(targetData);
            $scope.title = function () { return 'Thông tin bó hàng sản phẩm [' + targetData.MABOHANG + ']'; };
            $scope.convertCodeToName = function (paraValue, moduleName) {
                if (paraValue) {
                    var tempCache = $filter('filter')($scope.tempData(moduleName), { VALUE: paraValue }, true);
                    if (tempCache && tempCache.length === 1) {
                        return tempCache[0].DESCRIPTION;
                    } else {
                        return paraValue;
                    }
                }
            };
            function TinhTongTien(listObj, name) {
                var total = 0;
                if (listObj && listObj.length > 0) {
                    angular.forEach(listObj, function (obj, idx) {
                        var increase = obj[name];
                        if (!increase) {
                            increase = 0;
                        }
                        total += increase;
                    });
                }
                return total;
            };
            $scope.pageChanged = function () {
                var currentPage = $scope.paged.CurrentPage;
                var itemsPerPage = $scope.paged.ItemsPerPage;
                $scope.paged.TotalItems = $scope.target.DataDetails.length;
                $scope.data = [];
                if ($scope.target.DataDetails) {
                    for (var i = (currentPage - 1) * itemsPerPage; i < currentPage * itemsPerPage && i < $scope.target.DataDetails.length; i++) {
                        $scope.data.push($scope.target.DataDetails[i]);
                    }
                }
            };
            function filterData() {
                service.getDetails($scope.target.ID).then(function (sucessRes) {
                    if (sucessRes && sucessRes.status === 200 && sucessRes.data && sucessRes.data.Status && sucessRes.data.Data) {
                        $scope.target = sucessRes.data.Data;
                        if ($scope.target && $scope.target.DataDetails.length > 0) {
                            angular.forEach($scope.target.DataDetails, function (v, k) {
                                if (!v.CHIETKHAU) v.CHIETKHAU = 0;
                                v.TONGTIEN_TRUOCCHIETKHAU = v.SOLUONG * v.GIABANLE_VAT;
                                if (v.CHIETKHAU <= 100) {
                                    //chiết khấu theo tỷ lệ
                                    v.TONGTIEN = v.SOLUONG * (v.GIABANLE_VAT - (v.GIABANLE_VAT * v.CHIETKHAU / 100));
                                }else {
                                    //chiết khấu theo tiền
                                    v.TONGTIEN = v.SOLUONG * (v.GIABANLE_VAT - v.CHIETKHAU);
                                }
                            });
                        }
                        $scope.pageChanged();
                    }
                });
            };
            filterData();
            function listenerDataDetails() {
                $scope.$watch("target.DataDetails", function (newValue, oldValue) {
                    $scope.target.TONGCHIETKHAU = TinhTongTien($scope.target.DataDetails, 'CHIETKHAU');
                    $scope.target.TONGTIEN = TinhTongTien($scope.target.DataDetails, 'TONGTIEN');
                }, true);
            };
            listenerDataDetails();
            $scope.cancel = function () {
                $uibModalInstance.close();
            };
        }]);
    app.controller('boHangEdit_Ctrl', ['$scope', '$uibModalInstance', '$http', 'configService', 'boHangService', 'targetData', 'tempDataService', '$filter', '$uibModal', '$log', 'userService', 'matHangService', 'donViTinhService',
        function ($scope, $uibModalInstance, $http, configService, service, targetData, tempDataService, $filter, $uibModal, $log, userService, matHangService, donViTinhService) {
            $scope.config = angular.copy(configService);
            $scope.paged = angular.copy(configService.pageDefault);
            var currentUser = userService.GetCurrentUser();
            var unitCode = currentUser.unitCode;
            $scope.tempData = tempDataService.tempData;
            $scope.target = {};
            $scope.target = angular.copy(targetData);
            $scope.title = function () { return 'Chỉnh sửa bó hàng sản phẩm'; };
            //Function load data catalog DonViTinh
            function loadDataDonViTinh() {
                $scope.donViTinh = [];
                if (!tempDataService.tempData('donViTinh')) {
                    donViTinhService.getAllData().then(function (successRes) {
                        if (successRes && successRes.status === 200 && successRes.data && successRes.data.Status && successRes.data.Data && successRes.data.Data.length > 0) {
                            tempDataService.putTempData('donViTinh', successRes.data.Data);
                            $scope.donViTinh = successRes.data.Data;
                        }
                    }, function (errorRes) {
                        console.log('errorRes', errorRes);
                    });
                } else {
                    $scope.donViTinh = tempDataService.tempData('donViTinh');
                }
            };
            loadDataDonViTinh();
            //end
            $scope.convertCodeToName = function (paraValue, moduleName) {
                if (paraValue) {
                    var tempCache = $filter('filter')($scope.tempData(moduleName), { VALUE: paraValue }, true);
                    if (tempCache && tempCache.length === 1) {
                        return tempCache[0].DESCRIPTION;
                    } else {
                        return paraValue;
                    }
                }
            };

            $scope.pageChanged = function () {
                var currentPage = $scope.paged.CurrentPage;
                var itemsPerPage = $scope.paged.ItemsPerPage;
                $scope.paged.TotalItems = $scope.target.DataDetails.length;
                $scope.data = [];
                if ($scope.target.DataDetails) {
                    for (var i = (currentPage - 1) * itemsPerPage; i < currentPage * itemsPerPage && i < $scope.target.DataDetails.length; i++) {
                        $scope.data.push($scope.target.DataDetails[i]);
                    }
                }
            };

            function TinhTongTien(listObj, name) {
                var total = 0;
                if (listObj && listObj.length > 0) {
                    angular.forEach(listObj, function (obj, idx) {
                        var increase = obj[name];
                        if (!increase) {
                            increase = 0;
                        }
                        total += increase;
                    });
                }
                return total;
            };

            $scope.changedSoLuong = function (item) {
                if (item) {
                    if (!item.SOLUONG) item.SOLUONG = 1;
                    if (!item.CHIETKHAU) item.CHIETKHAU = 0;
                    item.TONGTIEN_TRUOCCHIETKHAU = item.SOLUONG * item.GIABANLE_VAT;
                    if (item.CHIETKHAU <= 100) {
                        //chiết khấu theo tỷ lệ
                        item.TONGTIEN = item.SOLUONG * (item.GIABANLE_VAT - (item.GIABANLE_VAT * item.CHIETKHAU / 100));
                    } else {
                        //chiết khấu theo tiền
                        item.TONGTIEN = item.SOLUONG * (item.GIABANLE_VAT - item.CHIETKHAU);
                    }
                }
            };
            $scope.changedChietKhau = function (item) {
                if (item) {
                    if (!item.SOLUONG) item.SOLUONG = 1;
                    if (!item.CHIETKHAU) item.CHIETKHAU = 0;
                    item.TONGTIEN_TRUOCCHIETKHAU = item.SOLUONG * item.GIABANLE_VAT;
                    if (item.CHIETKHAU <= 100) {
                        //chiết khấu theo tỷ lệ
                        item.TONGTIEN = item.SOLUONG * (item.GIABANLE_VAT - (item.GIABANLE_VAT * item.CHIETKHAU / 100));
                    } else {
                        //chiết khấu theo tiền
                        item.TONGTIEN = item.SOLUONG * (item.GIABANLE_VAT - item.CHIETKHAU);
                    }
                }
                $scope.$watch("target.DataDetails", function (newValue, oldValue) {
                    $scope.target.TONGCHIETKHAU = TinhTongTien($scope.target.DataDetails, 'CHIETKHAU');
                }, true);
            };

            //function search mathang
            $scope.searchMatHang = function (strKey) {
                if (strKey) {
                    var modalInstance = $uibModal.open({
                        backdrop: 'static',
                        templateUrl: configService.buildUrl('catalog/MatHang', 'search'),
                        controller: 'matHangSearch_Ctrl',
                        windowClass: 'search-window',
                        resolve: {
                            serviceSelectData: function () {
                                return matHangService;
                            },
                            filterObject: function () {
                                return {
                                    keySearch: strKey,
                                };
                            }
                        }
                    });
                    modalInstance.result.then(function (refundedData) {
                        if (refundedData) {
                            $scope.addItem = {
                                MAHANG: refundedData.MAHANG,
                                TENHANG: refundedData.TENHANG,
                                MADONVITINH: refundedData.MADONVITINH,
                                GIABANLE_VAT: refundedData.GIABANLE_VAT,
                                CHIETKHAU: 0,
                                SOLUONG: 1,
                                TONGTIEN: SOLUONG * (GIABANLE_VAT - (GIABANLE_VAT * CHIETKHAU / 100)),
                                TONGTIEN_TRUOCCHIETKHAU: SOLUONG * GIABANLE_VAT,
                                INDEX: 0
                            };
                            $scope.pageChanged();
                        }
                    }, function () {
                    });
                }
            };
            //end function search

            //CHANGED MAHANG ADD ITEM
            $scope.changedMaHang = function (maHang) {
                if (maHang) {
                    var obj = {
                        MAHANG: maHang,
                        UNITCODE: unitCode
                    }
                    matHangService.getMatHangTheoDieuKien(obj).then(function (successRes) {
                        if (successRes && successRes.status === 200 && successRes.data && successRes.data.Status && successRes.data.Data) {
                            $scope.addItem.MAHANG = successRes.data.Data.MAHANG;
                            $scope.addItem.TENHANG = successRes.data.Data.TENHANG;
                            $scope.addItem.MADONVITINH = successRes.data.Data.MADONVITINH;
                            $scope.addItem.GIABANLE_VAT = successRes.data.Data.GIABANLE_VAT;
                            $scope.addItem.CHIETKHAU = 0;
                            $scope.addItem.SOLUONG = 1;
                            $scope.addItem.TONGTIEN = $scope.addItem.SOLUONG * ($scope.addItem.GIABANLE_VAT - ($scope.addItem.GIABANLE_VAT * $scope.addItem.CHIETKHAU / 100));
                            $scope.addItem.TONGTIEN_TRUOCCHIETKHAU = $scope.addItem.SOLUONG * $scope.addItem.GIABANLE_VAT;
                            document.getElementById('_soLuongAddItem').focus();
                            document.getElementById('_soLuongAddItem').select();
                        }
                        else {
                            //bật lên modal tìm kiếm mặt hàng
                            $scope.searchMatHang(maHang);
                        }
                    });
                }
            };

            //add Row
            $scope.addRow = function () {
                if ($scope.addItem && $scope.addItem.MAHANG !== '') {
                    var exist = $filter('filter')($scope.target.DataDetails, { MAHANG: $scope.addItem.MAHANG }, true);
                    if (exist && exist.length == 1) {
                        exist[0].SOLUONG = exist[0].SOLUONG + $scope.addItem.SOLUONG;
                        exist[0].GIABANLE_VAT = $scope.addItem.GIABANLE_VAT;
                        exist[0].TONGTIEN = Math.round(100 * (exist[0].SOLUONG * exist[0].GIABANLE_VAT)) / 100;
                        exist[0].TONGTIEN_TRUOCCHIETKHAU = exist[0].SOLUONG * exist[0].GIABANLE_VAT;
                        Lobibox.notify('success', {
                            title: 'Thông báo',
                            width: 400,
                            msg: "Đã tồn tại mã hàng " + exist[0].MAHANG + ". Cộng gộp!",
                            delay: 1500
                        });
                    }
                    else {
                        $scope.target.DataDetails.push($scope.addItem);
                    }
                    $scope.pageChanged();
                    $scope.addItem = {
                        MAHANG: '',
                        TENHANG: '',
                        MADONVITINH: '',
                        SOLUONG: 1,
                        GIABANLE_VAT: 0,
                        CHIETKHAU: 0,
                        TONGTIEN: 0,
                        TONGTIEN_TRUOCCHIETKHAU: 0
                    };
                    focus('_maHangAddItem');
                    document.getElementById('_maHangAddItem').focus();
                    document.getElementById('_maHangAddItem').select();
                }
            };
            //end add row

            $scope.removeItem = function (index) {
                var currentPage = $scope.paged.CurrentPage;
                var itemsPerPage = 10;
                var currentPageIndex = (currentPage - 1) * itemsPerPage + index;
                $scope.target.DataDetails.splice(currentPageIndex, 1);
                $scope.pageChanged();
            };

            function filterData() {
                service.getDetails($scope.target.ID).then(function (sucessRes) {
                    if (sucessRes && sucessRes.status === 200 && sucessRes.data && sucessRes.data.Status && sucessRes.data.Data) {
                        $scope.target = sucessRes.data.Data;
                        if ($scope.target && $scope.target.DataDetails.length > 0) {
                            angular.forEach($scope.target.DataDetails, function (v, k) {
                                if (!v.CHIETKHAU) v.CHIETKHAU = 0;
                                v.TONGTIEN_TRUOCCHIETKHAU = v.SOLUONG * v.GIABANLE_VAT;
                                if (v.CHIETKHAU <= 100) {
                                    //chiết khấu theo tỷ lệ
                                    v.TONGTIEN = v.SOLUONG * (v.GIABANLE_VAT - (v.GIABANLE_VAT * v.CHIETKHAU / 100));
                                } else {
                                    //chiết khấu theo tiền
                                    v.TONGTIEN = v.SOLUONG * (v.GIABANLE_VAT - v.CHIETKHAU);
                                }
                            });
                        }
                        $scope.pageChanged();
                    }
                });
            };
            filterData();

            function listenerDataDetails() {
                $scope.$watch("target.DataDetails", function (newValue, oldValue) {
                    $scope.target.TONGCHIETKHAU = TinhTongTien($scope.target.DataDetails, 'CHIETKHAU');
                    $scope.target.TONGTIEN = TinhTongTien($scope.target.DataDetails, 'TONGTIEN');
                }, true);
            };
            listenerDataDetails();
            $scope.save = function () {
                if (!$scope.target.MABOHANG || !$scope.target.TENBOHANG) {
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