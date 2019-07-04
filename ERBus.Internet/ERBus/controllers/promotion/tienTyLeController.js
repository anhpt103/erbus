define(['ui-bootstrap', 'controllers/catalog/nhaCungCapController', 'controllers/catalog/donViTinhController', 'controllers/catalog/thueController', 'controllers/authorize/thamSoHeThongController', 'controllers/catalog/khoHangController', 'controllers/authorize/kyKeToanController', 'controllers/catalog/matHangController', 'controllers/authorize/cuaHangController'], function () {
    'use strict';
    var app = angular.module('tienTyLeModule', ['ui.bootstrap', 'nhaCungCapModule', 'donViTinhModule', 'thueModule', 'thamSoHeThongModule', 'khoHangModule', 'kyKeToanModule', 'matHangModule', 'cuaHangModule']);
    app.factory('tienTyLeService', ['$http', 'configService', function ($http, configService) {
        var serviceUrl = configService.rootUrlWebApi + '/Promotion/TienTyLe';
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
            getDetails: function (ID) {
                return $http.get(serviceUrl + '/GetDetails/' + ID);
            },
            postApproval: function (data) {
                return $http.post(serviceUrl + '/PostApproval', data);
            },
            postUnApproval: function (data) {
                return $http.post(serviceUrl + '/PostUnApproval', data);
            }
        }
        return result;
    }]);
    /* controller list */
    app.controller('TienTyLe_Ctrl', ['$scope', '$http', 'configService', 'tienTyLeService', 'tempDataService', '$filter', '$uibModal', '$log', 'securityService', 'khoHangService',
        function ($scope, $http, configService, service, tempDataService, $filter, $uibModal, $log, securityService, khoHangService) {
            $scope.config = angular.copy(configService);
            $scope.paged = angular.copy(configService.pageDefault);
            $scope.filtered = angular.copy(configService.filterDefault);
            $scope.tempData = tempDataService.tempData;
            $scope.title = function () { return 'Khuyến mãi tiền tỷ lệ' };
            $scope.data = [];
            $scope.setPage = function (pageNo) {
                $scope.paged.CurrentPage = pageNo;
                filterData();
            };
            $scope.sortType = 'MA_KHUYENMAI';
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
            //Function load data catalog KhoHang
            function loadDataKhoHang() {
                $scope.khoHang = [];
                if (!tempDataService.tempData('khoHang')) {
                    khoHangService.getAllData().then(function (successRes) {
                        if (successRes && successRes.status === 200 && successRes.data && successRes.data.Status && successRes.data.Data && successRes.data.Data.length > 0) {
                            tempDataService.putTempData('khoHang', successRes.data.Data);
                            $scope.khoHang = successRes.data.Data;
                        }
                    }, function (errorRes) {
                        console.log('errorRes', errorRes);
                    });
                } else {
                    $scope.khoHang = tempDataService.tempData('khoHang');
                }
            };
            loadDataKhoHang();
            //end
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
                securityService.getAccessList('TienTyLe').then(function (successRes) {
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
                    windowClass: 'promotion-window',
                    templateUrl: configService.buildUrl('promotion/TienTyLe', 'create'),
                    controller: 'tienTyLeCreate_Ctrl',
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
                    windowClass: 'promotion-window',
                    templateUrl: configService.buildUrl('promotion/TienTyLe', 'detail'),
                    controller: 'tienTyLeDetail_Ctrl',
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

            $scope.approval = function (target) {
                var obj = {
                    ID: target.ID
                };
                service.postApproval(obj).then(function (refundedData) {
                    if (refundedData && refundedData.status === 200 && refundedData.data && refundedData.data.Status && refundedData.data.Data) {
                        $scope.refresh();
                        Lobibox.notify('success', {
                            title: 'Thông báo',
                            width: 400,
                            msg: refundedData.data.Message,
                            delay: 2000
                        });
                    } else if (refundedData && refundedData.status === 200 && refundedData.data && !refundedData.data.Status && refundedData.data.Message === 'ISAPROVAL') {
                        Lobibox.notify('error', {
                            title: 'Cảnh báo',
                            width: 400,
                            msg: 'Không thể kích hoạt! Chương trình này đã được kích hoạt rồi',
                            delay: 3000
                        });
                    }
                    else {
                        Lobibox.notify('error', {
                            title: 'Không thành công',
                            msg: 'Chương trình khuyến mãi chưa được áp dụng',
                            delay: 3000
                        });
                    }
                });
            };

            $scope.unapproval = function (target) {
                var obj = {
                    ID: target.ID
                };
                service.postUnApproval(obj).then(function (refundedData) {
                    if (refundedData && refundedData.status === 200 && refundedData.data && refundedData.data.Status && refundedData.data.Data) {
                        $scope.refresh();
                        Lobibox.notify('success', {
                            title: 'Thông báo',
                            width: 400,
                            msg: refundedData.data.Message,
                            delay: 2000
                        });
                    } else if (refundedData && refundedData.status === 200 && refundedData.data && !refundedData.data.Status && refundedData.data.Message === 'ISUNAPROVAL') {
                        Lobibox.notify('error', {
                            title: 'Cảnh báo',
                            width: 400,
                            msg: 'Không thể bỏ kích hoạt !',
                            delay: 3000
                        });
                    }
                    else {
                        Lobibox.notify('error', {
                            title: 'Không thành công',
                            msg: 'Chương trình khuyến mãi chưa được áp dụng',
                            delay: 3000
                        });
                    }
                });
            };
            

            /* Function edit item */
            $scope.edit = function (target) {
                var modalInstance = $uibModal.open({
                    backdrop: 'static',
                    animation: true,
                    windowClass: 'promotion-window',
                    templateUrl: configService.buildUrl('promotion/TienTyLe', 'edit'),
                    controller: 'tienTyLeEdit_Ctrl',
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
                    templateUrl: configService.buildUrl('promotion/TienTyLe', 'delete'),
                    controller: 'tienTyLeDelete_Ctrl',
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

    app.controller('tienTyLeCreate_Ctrl', ['$scope', '$uibModalInstance', '$http', 'configService', 'tienTyLeService', 'tempDataService', '$filter', '$uibModal', '$log', 'donViTinhService', 'khoHangService', 'kyKeToanService', 'matHangService', 'userService',
    function ($scope, $uibModalInstance, $http, configService, service, tempDataService, $filter, $uibModal, $log,  donViTinhService, khoHangService, kyKeToanService, matHangService, userService) {
        $scope.config = angular.copy(configService);
        $scope.tempData = tempDataService.tempData;
        $scope.paged = angular.copy(configService.pageDefault);
        var currentUser = userService.GetCurrentUser();
        var unitCode = currentUser.unitCode;
        $scope.title = function () { return 'Thêm chương trình khuyến mãi tiền tỷ lệ'; };
        $scope.target = {
            TUNGAY: new Date(),
            DENNGAY: new Date(),
            TRANGTHAI: 0,
            TUGIO: '00:00',
            DENGIO: '23:59',
            DataDetails: [],
        };
        $scope.addItem = {
            MAHANG: '',
            TENHANG: '',
            MADONVITINH: '',
            GIABANLE_VAT: 0,
            SOLUONG: 1,
            GIATRI_KHUYENMAI: 0,
            THANHTIEN: 0,
            INDEX: 0
        };
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

        //Function load data catalog KhoHang
        function loadDataKhoHang() {
            $scope.khoHang = [];
            if (!tempDataService.tempData('khoHang')) {
                khoHangService.getAllData().then(function (successRes) {
                    if (successRes && successRes.status === 200 && successRes.data && successRes.data.Status && successRes.data.Data && successRes.data.Data.length > 0) {
                        tempDataService.putTempData('khoHang', successRes.data.Data);
                        $scope.khoHang = successRes.data.Data;
                    }
                }, function (errorRes) {
                    console.log('errorRes', errorRes);
                });
            } else {
                $scope.khoHang = tempDataService.tempData('khoHang');
            }
        };
        loadDataKhoHang();
        //end

        //Tạo mới mã phiếu
        service.buildNewCode().then(function (successRes) {
            if (successRes && successRes.status == 200 && successRes.data) {
                $scope.target.MA_KHUYENMAI = successRes.data;
            }
        });
        //end

        function caculatorThanhTien(addItem) {
            if (addItem && addItem.MAHANG) {
                addItem.THANHTIEN = Math.round(100 * ((addItem.SOLUONG * addItem.GIABANLE_VAT) - (addItem.GIATRI_KHUYENMAI * (addItem.SOLUONG * addItem.GIABANLE_VAT)) / 100)) / 100;
                addItem.INDEX = $scope.target.DataDetails.length + 1;
            }
        };

        function errorFocusMaHang() {
            focus('_maHangAddItem');
            document.getElementById('_maHangAddItem').focus();
            document.getElementById('_maHangAddItem').select();
        };
        //sự kiện click ESC exit modal
        document.addEventListener('keyup', function (e) {
            if (e.keyCode == 27) {
                errorFocusMaHang();
            }
        });
        //end 
        //function search mathang
        $scope.searchMatHang = function (strKey) {
            if (strKey) {
                var modalInstance = $uibModal.open({
                    backdrop: 'static',
                    templateUrl: configService.buildUrl('catalog/MatHang', 'search'),
                    controller: 'matHangSearch_Ctrl',
                    windowClass: 'search-window-mathang',
                    resolve: {
                        filterObject: function () {
                            return {
                                ISSELECT_POST: strKey,
                            };
                        }
                    }
                });
                modalInstance.result.then(function (refundedData) {
                    if (refundedData && refundedData.length > 0) {
                        angular.forEach(refundedData, function (v, k) {
                            $scope.addItem = {
                                MAHANG: v.MAHANG,
                                TENHANG: v.TENHANG,
                                MADONVITINH: v.MADONVITINH,
                                GIABANLE_VAT: v.GIABANLE_VAT,
                                SOLUONG: 1,
                                GIATRI_KHUYENMAI: 0,
                                THANHTIEN: Math.round(100 * ((v.SOLUONG * v.GIABANLE_VAT) - (v.GIATRI_KHUYENMAI * (v.SOLUONG * v.GIABANLE_VAT)) / 100)) / 100,
                                INDEX: $scope.target.DataDetails.length + 1
                            };
                            if ($scope.addItem && $scope.addItem.MAHANG !== '') {
                                var exist = $filter('filter')($scope.target.DataDetails, { MAHANG: $scope.addItem.MAHANG }, true);
                                if (exist && exist.length == 1) {
                                    exist[0].SOLUONG = 1;
                                    exist[0].THANHTIEN = Math.round(100 * ((exist[0].SOLUONG * exist[0].GIABANLE_VAT) - (exist[0].GIATRI_KHUYENMAI * (exist[0].SOLUONG * exist[0].GIABANLE_VAT)) / 100)) / 100;
                                    Lobibox.notify('success', {
                                        title: 'Thông báo',
                                        width: 400,
                                        msg: "Đã tồn tại mã hàng " + exist[0].MAHANG + ".",
                                        delay: 1500
                                    });
                                }
                                else {
                                    $scope.target.DataDetails.push($scope.addItem);
                                }
                            }
                            caculatorThanhTien($scope.addItem);
                            $scope.pageChanged();
                        });
                    }
                    errorFocusMaHang();
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
                    MAKHO_KHUYENMAI: $scope.target.MAKHO_NHAP,
                    UNITCODE: unitCode
                }
                matHangService.getMatHangNhapMuaTheoMaKho(obj).then(function (successRes) {
                    if (successRes && successRes.status === 200 && successRes.data && successRes.data.Status && successRes.data.Data) {
                        $scope.addItem.MAHANG = successRes.data.Data.MAHANG;
                        $scope.addItem.TENHANG = successRes.data.Data.TENHANG;
                        $scope.addItem.MADONVITINH = successRes.data.Data.MADONVITINH;
                        $scope.addItem.GIABANLE_VAT = successRes.data.Data.GIABANLE_VAT;
                        $scope.addItem.SOLUONG = 1;
                        $scope.addItem.GIATRI_KHUYENMAI = 0;
                        $scope.addItem.THANHTIEN = Math.round(100 * (($scope.addItem.SOLUONG * $scope.addItem.GIABANLE_VAT) - ($scope.addItem.GIATRI_KHUYENMAI * ($scope.addItem.SOLUONG * $scope.addItem.GIABANLE_VAT)) / 100)) / 100;
                        $scope.addItem.INDEX = $scope.target.DataDetails.length + 1;
                        document.getElementById('_giaTriKhuyenMaiAddItem').focus();
                        document.getElementById('_giaTriKhuyenMaiAddItem').select();
                    }
                    else {
                        //bật lên modal tìm kiếm mặt hàng
                        $scope.searchMatHang(maHang);
                    }
                });
            }
        };

        $scope.changedGiamGia = function (addItem) {
            if (addItem) {
                if (!addItem.SOLUONG) addItem.SOLUONG = 1;
                if (!addItem.GIATRI_KHUYENMAI) addItem.GIATRI_KHUYENMAI = 0;
                if (addItem.GIATRI_KHUYENMAI <= 100) {
                    //giảm giá theo %
                    addItem.THANHTIEN = Math.round(100 * ((addItem.SOLUONG * addItem.GIABANLE_VAT) - (addItem.GIATRI_KHUYENMAI * (addItem.SOLUONG * addItem.GIABANLE_VAT)) / 100)) / 100;
                }
                else {
                    //giảm giá theo số tiền
                    addItem.THANHTIEN = Math.round(100 * ((addItem.SOLUONG * addItem.GIABANLE_VAT) - addItem.GIATRI_KHUYENMAI)) / 100;
                }
                $scope.$watch("target.DataDetails", function (newValue, oldValue) {
                    $scope.target.TONG_SOLUONG = TinhTongTien($scope.target.DataDetails, 'SOLUONG');
                    $scope.target.TONG_TIEN = TinhTongTien($scope.target.DataDetails, 'THANHTIEN');
                }, true);
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

        //add Row
        $scope.addRow = function () {
            if ($scope.addItem && $scope.addItem.MAHANG !== '') {
                var exist = $filter('filter')($scope.target.DataDetails, { MAHANG: $scope.addItem.MAHANG }, true);
                if (exist && exist.length == 1) {
                    exist[0].SOLUONG = 1;
                    exist[0].THANHTIEN = Math.round(100 * ((exist[0].SOLUONG * exist[0].GIABANLE_VAT) - (exist[0].GIATRI_KHUYENMAI * (exist[0].SOLUONG * exist[0].GIABANLE_VAT)) / 100)) / 100;
                    Lobibox.notify('success', {
                        title: 'Thông báo',
                        width: 400,
                        msg: "Đã tồn tại mã hàng " + exist[0].MAHANG + ".",
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
                    GIABANLE_VAT: 0,
                    SOLUONG: 1,
                    GIATRI_KHUYENMAI: 0,
                    THANHTIEN: 0,
                    INDEX: 0
                };
                errorFocusMaHang();
            } else {
                errorFocusMaHang();
            }
        };
        //end add row
        //END CHANGED MAHANG ADD ITEM
        $scope.removeItem = function (index) {
            var currentPage = $scope.paged.CurrentPage;
            var itemsPerPage = 10;
            var currentPageIndex = (currentPage - 1) * itemsPerPage + index;
            $scope.target.DataDetails.splice(currentPageIndex, 1);
            $scope.pageChanged();
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
        function listenerDataDetails() {
            $scope.$watch("target.DataDetails", function (newValue, oldValue) {
                $scope.target.TONG_SOLUONG = TinhTongTien($scope.target.DataDetails, 'SOLUONG');
                $scope.target.TONG_TIEN = TinhTongTien($scope.target.DataDetails, 'THANHTIEN');
            }, true);
        };
        listenerDataDetails();

        $scope.save = function () {
            if (!$scope.target.MA_KHUYENMAI || !$scope.target.TUNGAY || !$scope.target.DENNGAY || !$scope.target.MAKHO_KHUYENMAI || $scope.target.DataDetails.length <= 0) {
                Lobibox.notify('warning', {
                    title: 'Thiếu thông tin',
                    msg: 'Thông tin đầu vào không hợp lệ! Dữ liệu (*) không được để trống',
                    delay: 4000
                });
            } else {
                if ($scope.addItem.MAHANG && $scope.addItem.MAHANG.length > 0 && $scope.addItem.TENHANG && $scope.addItem.TENHANG.length > 0 && $scope.addItem.THANHTIEN && $scope.addItem.THANHTIEN != 0) {
                    Lobibox.notify('default', {
                        title: 'Nhắc nhở',
                        msg: 'Dòng hàng [' + $scope.addItem.MAHANG + '] chưa được thêm mới xuống danh sách, Bạn hãy thêm hoặc xóa trước khi lưu',
                        delay: 4000
                    });
                }
                else {
                    $scope.target.TUNGAY = $scope.config.moment($scope.target.TUNGAY).format();
                    $scope.target.DENNGAY = $scope.config.moment($scope.target.DENNGAY).format();
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
            }
        };
        $scope.cancel = function () {
            $uibModalInstance.close();
        };
    }]);


    app.controller('tienTyLeDetail_Ctrl', ['$scope', '$uibModalInstance', '$http', 'configService', 'tienTyLeService', 'targetData', 'tempDataService', '$filter', '$uibModal', '$log', 'thueService',
        function ($scope, $uibModalInstance, $http, configService, service, targetData, tempDataService, $filter, $uibModal, $log, thueService) {
            $scope.config = angular.copy(configService);
            $scope.paged = angular.copy(configService.pageDefault);
            $scope.tempData = tempDataService.tempData;
            $scope.target = {};
            $scope.target = angular.copy(targetData);
            $scope.title = function () { return 'Thông tin chương trình khuyến mãi tiền tỷ lệ [' + targetData.MA_KHUYENMAI + ']'; };
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

            function caculatorThanhTien(addItem) {
                if (addItem && addItem.MAHANG) {
                    addItem.THANHTIEN = Math.round(100 * ((addItem.SOLUONG * addItem.GIABANLE_VAT) - (addItem.GIATRI_KHUYENMAI * (addItem.SOLUONG * addItem.GIABANLE_VAT)) / 100)) / 100;
                    addItem.INDEX = $scope.target.DataDetails.length + 1;
                }
            };

            function errorFocusMaHang() {
                focus('_maHangAddItem');
                document.getElementById('_maHangAddItem').focus();
                document.getElementById('_maHangAddItem').select();
            };
            //sự kiện click ESC exit modal
            document.addEventListener('keyup', function (e) {
                if (e.keyCode == 27) {
                    errorFocusMaHang();
                }
            });
            //end 

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
            function listenerDataDetails() {
                $scope.$watch("target.DataDetails", function (newValue, oldValue) {
                    $scope.target.TONG_SOLUONG = TinhTongTien($scope.target.DataDetails, 'SOLUONG');
                    $scope.target.TONG_TIEN = TinhTongTien($scope.target.DataDetails, 'THANHTIEN');
                }, true);
            };
            listenerDataDetails();

            function filterData() {
                service.getDetails($scope.target.ID).then(function (sucessRes) {
                    if (sucessRes && sucessRes.status === 200 && sucessRes.data && sucessRes.data.Status && sucessRes.data.Data) {
                        $scope.target = sucessRes.data.Data;
                        $scope.target.TUNGAY = new Date($scope.target.TUNGAY);
                        $scope.target.DENNGAY = new Date($scope.target.DENNGAY);
                        if ($scope.target.DataDetails && $scope.target.DataDetails.length > 0) {
                            angular.forEach($scope.target.DataDetails, function (value, idx) {
                                value.THANHTIEN = Math.round(100 * ((value.SOLUONG * value.GIABANLE_VAT) - (value.GIATRI_KHUYENMAI * (value.SOLUONG * value.GIABANLE_VAT)) / 100)) / 100;
                            });
                        }
                        $scope.pageChanged();
                    }
                });
            };
            filterData();
            $scope.cancel = function () {
                $uibModalInstance.close();
            };
        }]);

    app.controller('tienTyLeEdit_Ctrl', ['$scope', '$uibModalInstance', '$http', 'configService', 'tienTyLeService', 'targetData', 'tempDataService', '$filter', '$uibModal', '$log', 'matHangService', 'userService',
        function ($scope, $uibModalInstance, $http, configService, service, targetData, tempDataService, $filter, $uibModal, $log, matHangService, userService) {
            $scope.config = angular.copy(configService);
            $scope.paged = angular.copy(configService.pageDefault);
            $scope.tempData = tempDataService.tempData;
            var currentUser = userService.GetCurrentUser();
            var unitCode = currentUser.unitCode;
            $scope.target = {};
            $scope.target = angular.copy(targetData);
            $scope.addItem = {
                MAHANG: '',
                TENHANG: '',
                MADONVITINH: '',
                GIABANLE_VAT: 0,
                SOLUONG: 1,
                GIATRI_KHUYENMAI: 0,
                THANHTIEN: 0,
                INDEX: 0
            };
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

            function caculatorThanhTien(addItem) {
                if (addItem && addItem.MAHANG) {
                    addItem.THANHTIEN = Math.round(100 * ((addItem.SOLUONG * addItem.GIABANLE_VAT) - (addItem.GIATRI_KHUYENMAI * (addItem.SOLUONG * addItem.GIABANLE_VAT)) / 100)) / 100;
                    addItem.INDEX = $scope.target.DataDetails.length + 1;
                }
            };

            function errorFocusMaHang() {
                focus('_maHangAddItem');
                document.getElementById('_maHangAddItem').focus();
                document.getElementById('_maHangAddItem').select();
            };
            //sự kiện click ESC exit modal
            document.addEventListener('keyup', function (e) {
                if (e.keyCode == 27) {
                    errorFocusMaHang();
                }
            });
            //end 
            //function search mathang
            $scope.searchMatHang = function (strKey) {
                if (strKey) {
                    var modalInstance = $uibModal.open({
                        backdrop: 'static',
                        templateUrl: configService.buildUrl('catalog/MatHang', 'search'),
                        controller: 'matHangSearch_Ctrl',
                        windowClass: 'search-window-mathang',
                        resolve: {
                            filterObject: function () {
                                return {
                                    ISSELECT_POST: strKey,
                                };
                            }
                        }
                    });
                    modalInstance.result.then(function (refundedData) {
                        if (refundedData && refundedData.length > 0) {
                            angular.forEach(refundedData, function (v, k) {
                                $scope.addItem = {
                                    MAHANG: v.MAHANG,
                                    TENHANG: v.TENHANG,
                                    MADONVITINH: v.MADONVITINH,
                                    GIABANLE_VAT: v.GIABANLE_VAT,
                                    SOLUONG: 1,
                                    GIATRI_KHUYENMAI: 0,
                                    THANHTIEN: Math.round(100 * ((v.SOLUONG * v.GIABANLE_VAT) - (v.GIATRI_KHUYENMAI * (v.SOLUONG * v.GIABANLE_VAT)) / 100)) / 100,
                                    INDEX: $scope.target.DataDetails.length + 1
                                };
                                if ($scope.addItem && $scope.addItem.MAHANG !== '') {
                                    var exist = $filter('filter')($scope.target.DataDetails, { MAHANG: $scope.addItem.MAHANG }, true);
                                    if (exist && exist.length == 1) {
                                        exist[0].SOLUONG = 1;
                                        exist[0].THANHTIEN = Math.round(100 * ((exist[0].SOLUONG * exist[0].GIABANLE_VAT) - (exist[0].GIATRI_KHUYENMAI * (exist[0].SOLUONG * exist[0].GIABANLE_VAT)) / 100)) / 100;
                                        Lobibox.notify('success', {
                                            title: 'Thông báo',
                                            width: 400,
                                            msg: "Đã tồn tại mã hàng " + exist[0].MAHANG + ".",
                                            delay: 1500
                                        });
                                    }
                                    else {
                                        $scope.target.DataDetails.push($scope.addItem);
                                    }
                                }
                                caculatorThanhTien($scope.addItem);
                                $scope.pageChanged();
                            });
                        }
                        errorFocusMaHang();
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
                        MAKHO_KHUYENMAI: $scope.target.MAKHO_NHAP,
                        UNITCODE: unitCode
                    }
                    matHangService.getMatHangNhapMuaTheoMaKho(obj).then(function (successRes) {
                        if (successRes && successRes.status === 200 && successRes.data && successRes.data.Status && successRes.data.Data) {
                            $scope.addItem.MAHANG = successRes.data.Data.MAHANG;
                            $scope.addItem.TENHANG = successRes.data.Data.TENHANG;
                            $scope.addItem.MADONVITINH = successRes.data.Data.MADONVITINH;
                            $scope.addItem.GIABANLE_VAT = successRes.data.Data.GIABANLE_VAT;
                            $scope.addItem.SOLUONG = 1;
                            $scope.addItem.GIATRI_KHUYENMAI = 0;
                            $scope.addItem.THANHTIEN = Math.round(100 * (($scope.addItem.SOLUONG * $scope.addItem.GIABANLE_VAT) - ($scope.addItem.GIATRI_KHUYENMAI * ($scope.addItem.SOLUONG * $scope.addItem.GIABANLE_VAT)) / 100)) / 100;
                            $scope.addItem.INDEX = $scope.target.DataDetails.length + 2;
                            document.getElementById('_giaTriKhuyenMaiAddItem').focus();
                            document.getElementById('_giaTriKhuyenMaiAddItem').select();
                        }
                        else {
                            //bật lên modal tìm kiếm mặt hàng
                            $scope.searchMatHang(maHang);
                        }
                    });
                }
            };

            $scope.changedGiamGia = function (addItem) {
                if (addItem) {
                    if (!addItem.SOLUONG) addItem.SOLUONG = 1;
                    if (!addItem.GIATRI_KHUYENMAI) addItem.GIATRI_KHUYENMAI = 0;
                    if (addItem.GIATRI_KHUYENMAI <= 100) {
                        //giảm giá theo %
                        addItem.THANHTIEN = Math.round(100 * ((addItem.SOLUONG * addItem.GIABANLE_VAT) - (addItem.GIATRI_KHUYENMAI * (addItem.SOLUONG * addItem.GIABANLE_VAT)) / 100)) / 100;
                    }
                    else {
                        //giảm giá theo số tiền
                        addItem.THANHTIEN = Math.round(100 * ((addItem.SOLUONG * addItem.GIABANLE_VAT) - addItem.GIATRI_KHUYENMAI)) / 100;
                    }
                    $scope.$watch("target.DataDetails", function (newValue, oldValue) {
                        $scope.target.TONG_SOLUONG = TinhTongTien($scope.target.DataDetails, 'SOLUONG');
                        $scope.target.TONG_TIEN = TinhTongTien($scope.target.DataDetails, 'THANHTIEN');
                    }, true);
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

            //add Row
            $scope.addRow = function () {
                if ($scope.addItem && $scope.addItem.MAHANG !== '') {
                    var exist = $filter('filter')($scope.target.DataDetails, { MAHANG: $scope.addItem.MAHANG }, true);
                    if (exist && exist.length == 1) {
                        exist[0].SOLUONG = 1;
                        exist[0].THANHTIEN = Math.round(100 * ((exist[0].SOLUONG * exist[0].GIABANLE_VAT) - (exist[0].GIATRI_KHUYENMAI * (exist[0].SOLUONG * exist[0].GIABANLE_VAT)) / 100)) / 100;
                        Lobibox.notify('success', {
                            title: 'Thông báo',
                            width: 400,
                            msg: "Đã tồn tại mã hàng " + exist[0].MAHANG + ".",
                            delay: 1500
                        });
                    }
                    else {
                        $scope.addItem.INDEX = $scope.target.DataDetails.length + 2;
                        $scope.target.DataDetails.push($scope.addItem);
                    }
                    $scope.pageChanged();
                    $scope.addItem = {
                        MAHANG: '',
                        TENHANG: '',
                        MADONVITINH: '',
                        GIABANLE_VAT: 0,
                        SOLUONG: 1,
                        GIATRI_KHUYENMAI: 0,
                        THANHTIEN: 0,
                        INDEX: $scope.target.DataDetails.length + 2
                    };
                    errorFocusMaHang();
                } else {
                    errorFocusMaHang();
                }
            };
            //end add row
            //END CHANGED MAHANG ADD ITEM
            $scope.removeItem = function (index) {
                var currentPage = $scope.paged.CurrentPage;
                var itemsPerPage = 10;
                var currentPageIndex = (currentPage - 1) * itemsPerPage + index;
                $scope.target.DataDetails.splice(currentPageIndex, 1);
                $scope.pageChanged();
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
            function listenerDataDetails() {
                $scope.$watch("target.DataDetails", function (newValue, oldValue) {
                    $scope.target.TONG_SOLUONG = TinhTongTien($scope.target.DataDetails, 'SOLUONG');
                    $scope.target.TONG_TIEN = TinhTongTien($scope.target.DataDetails, 'THANHTIEN');
                }, true);
            };
            listenerDataDetails();

            function filterData() {
                service.getDetails($scope.target.ID).then(function (sucessRes) {
                    if (sucessRes && sucessRes.status === 200 && sucessRes.data && sucessRes.data.Status && sucessRes.data.Data) {
                        $scope.target = sucessRes.data.Data;
                        $scope.target.TUNGAY = new Date($scope.target.TUNGAY);
                        $scope.target.DENNGAY = new Date($scope.target.DENNGAY);
                        if ($scope.target.DataDetails && $scope.target.DataDetails.length > 0) {
                            angular.forEach($scope.target.DataDetails, function (value, idx) {
                                value.THANHTIEN = Math.round(100 * ((value.SOLUONG * value.GIABANLE_VAT) - (value.GIATRI_KHUYENMAI * (value.SOLUONG * value.GIABANLE_VAT)) / 100)) / 100;
                            });
                        }
                        $scope.pageChanged();
                    }
                });
            };
            filterData();

            $scope.save = function () {
                if (!$scope.target.MA_KHUYENMAI || !$scope.target.TUNGAY || !$scope.target.DENNGAY || !$scope.target.MAKHO_KHUYENMAI || $scope.target.DataDetails.length <= 0) {
                    Lobibox.notify('warning', {
                        title: 'Thiếu thông tin',
                        msg: 'Thông tin đầu vào không hợp lệ! Dữ liệu (*) không được để trống',
                        delay: 4000
                    });
                } else {
                    if ($scope.addItem.MAHANG && $scope.addItem.MAHANG.length > 0 && $scope.addItem.TENHANG && $scope.addItem.TENHANG.length > 0 && $scope.addItem.THANHTIEN && $scope.addItem.THANHTIEN != 0) {
                        Lobibox.notify('default', {
                            title: 'Nhắc nhở',
                            msg: 'Dòng hàng [' + $scope.addItem.MAHANG + '] chưa được thêm mới xuống danh sách, Bạn hãy thêm hoặc xóa trước khi lưu',
                            delay: 4000
                        });
                    }
                    else {
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
                }
            };
            $scope.cancel = function () {
                $uibModalInstance.close();
            };
        }]);

    app.controller('tienTyLeDelete_Ctrl', ['$scope', '$uibModalInstance', '$http', 'configService', 'tienTyLeService', 'targetData', 'tempDataService', '$filter', '$uibModal', '$log',
       function ($scope, $uibModalInstance, $http, configService, service, targetData, tempDataService, $filter, $uibModal, $log) {
           $scope.config = angular.copy(configService);
           $scope.tempData = tempDataService.tempData;
           $scope.target = {};
           $scope.target = angular.copy(targetData);;
           $scope.title = function () { return 'Xóa chương trình khuyến mãi tiền tỷ lệ [' + targetData.MA_KHUYENMAI + ']'; };
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