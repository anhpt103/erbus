define(['ui-bootstrap', 'controllers/catalog/nhaCungCapController', 'controllers/catalog/donViTinhController', 'controllers/catalog/thueController', 'controllers/authorize/thamSoHeThongController', 'controllers/catalog/khoHangController', 'controllers/authorize/kyKeToanController', 'controllers/catalog/matHangController', 'controllers/authorize/cuaHangController', 'controllers/catalog/khachHangController', 'controllers/authorize/thamSoHeThongController'], function () {
    'use strict';
    var app = angular.module('xuatBanModule', ['ui.bootstrap', 'nhaCungCapModule', 'donViTinhModule', 'thueModule', 'thamSoHeThongModule', 'khoHangModule', 'kyKeToanModule', 'matHangModule', 'cuaHangModule', 'khachHangModule', 'thamSoHeThongModule']);
    app.factory('xuatBanService', ['$http', 'configService', function ($http, configService) {
        var serviceUrl = configService.rootUrlWebApi + '/Knowledge/XuatBan';
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
            getDetails: function (ID, TABLE_NAME) {
                return $http.get(serviceUrl + '/GetDetails/' + ID + '/' + TABLE_NAME);
            },
            postApproval: function (data) {
                return $http.post(serviceUrl + '/PostApproval', data);
            }
        }
        return result;
    }]);
    /* controller list */
    app.controller('XuatBan_Ctrl', ['$scope', '$http', 'configService', 'xuatBanService', 'tempDataService', '$filter', '$uibModal', '$log', 'securityService', 'nhaCungCapService','thueService','khoHangService','khachHangService','userService',
    function ($scope, $http, configService, service, tempDataService, $filter, $uibModal, $log, securityService, nhaCungCapService, thueService, khoHangService, khachHangService, userService) {
            $scope.config = angular.copy(configService);
            $scope.paged = angular.copy(configService.pageDefault);
            $scope.filtered = angular.copy(configService.filterDefault);
            $scope.tempData = tempDataService.tempData;
            $scope.title = function () { return 'Xuất bán buôn' };
            $scope.data = [];
            $scope.setPage = function (pageNo) {
                $scope.paged.CurrentPage = pageNo;
                filterData();
            };
            $scope.sortType = 'MA_CHUNGTU';
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
            //Function load data catalog NhaCungCap
            function loadDataNhaCungCap() {
                $scope.nhaCungCap = [];
                if (!tempDataService.tempData('nhaCungCap')) {
                    nhaCungCapService.getAllData().then(function (successRes) {
                        if (successRes && successRes.status === 200 && successRes.data && successRes.data.Status && successRes.data.Data && successRes.data.Data.length > 0) {
                            tempDataService.putTempData('nhaCungCap', successRes.data.Data);
                            $scope.nhaCungCap = successRes.data.Data;
                        }
                    }, function (errorRes) {
                        console.log('errorRes', errorRes);
                    });
                } else {
                    $scope.nhaCungCap = tempDataService.tempData('nhaCungCap');
                }
            };
            loadDataNhaCungCap();
            //end
            //Function load data catalog Thue
            function loadDataThue() {
                $scope.thue = [];
                if (!tempDataService.tempData('thue')) {
                    thueService.getAllData().then(function (successRes) {
                        if (successRes && successRes.status === 200 && successRes.data && successRes.data.Status && successRes.data.Data && successRes.data.Data.length > 0) {
                            tempDataService.putTempData('thue', successRes.data.Data);
                            $scope.thue = successRes.data.Data;
                        }
                    }, function (errorRes) {
                        console.log('errorRes', errorRes);
                    });
                } else {
                    $scope.thue = tempDataService.tempData('thue');
                }
            };
            loadDataThue();
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

            //Function load data catalog KhoHang
            function loadDataKhachHang() {
                $scope.khachHang = [];
                if (!tempDataService.tempData('khachHang')) {
                    khachHangService.getAllData().then(function (successRes) {
                        if (successRes && successRes.status === 200 && successRes.data && successRes.data.Status && successRes.data.Data && successRes.data.Data.length > 0) {
                            tempDataService.putTempData('khachHang', successRes.data.Data);
                            $scope.khachHang = successRes.data.Data;
                        }
                    }, function (errorRes) {
                        console.log('errorRes', errorRes);
                    });
                } else {
                    $scope.khachHang = tempDataService.tempData('khachHang');
                }
            };
            loadDataKhachHang();
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
                var currentUser = userService.GetCurrentUser();
                var userName = currentUser.userName;
                var unitCodeParam = !currentUser.parentUnitCode ? currentUser.unitCode : currentUser.parentUnitCode;
                securityService.getAccessList('XuatBan', userName, unitCodeParam).then(function (successRes) {
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
                    windowClass: 'knowledge-window',
                    templateUrl: configService.buildUrl('knowledge/XuatBan', 'create'),
                    controller: 'xuatBanCreate_Ctrl',
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
                    windowClass: 'knowledge-window',
                    templateUrl: configService.buildUrl('knowledge/XuatBan', 'detail'),
                    controller: 'xuatBanDetail_Ctrl',
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
                var modalInstance = $uibModal.open({
                    backdrop: 'static',
                    animation: true,
                    windowClass: 'knowledge-printItemWindow',
                    templateUrl: configService.buildUrl('knowledge/XuatBan', 'approval'),
                    controller: 'xuatBanApproval_Ctrl',
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
                    windowClass: 'knowledge-window',
                    templateUrl: configService.buildUrl('knowledge/XuatBan', 'edit'),
                    controller: 'xuatBanEdit_Ctrl',
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
                    templateUrl: configService.buildUrl('knowledge/XuatBan', 'delete'),
                    controller: 'xuatBanDelete_Ctrl',
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

    app.controller('xuatBanCreate_Ctrl', ['$scope', '$uibModalInstance', '$http', 'configService', 'xuatBanService', 'tempDataService', '$filter', '$uibModal', '$log', 'nhaCungCapService', 'donViTinhService', 'thueService', 'khoHangService', 'kyKeToanService', 'matHangService', 'userService','khachHangService','thamSoHeThongService',
    function ($scope, $uibModalInstance, $http, configService, service, tempDataService, $filter, $uibModal, $log, nhaCungCapService, donViTinhService, thueService, khoHangService, kyKeToanService, matHangService, userService, khachHangService, thamSoHeThongService) {
        $scope.config = angular.copy(configService);
        $scope.tempData = tempDataService.tempData;
        $scope.paged = angular.copy(configService.pageDefault);
        var currentUser = userService.GetCurrentUser();
        var unitCode = currentUser.unitCode;
        $scope.title = function () { return 'Thêm phiếu xuất bán buôn'; };
        $scope.refresh = function () {
            tempDataService.refreshData();
        };
        $scope.target = {
            TONGTIEN_TRUOCTHUE: 0,
            TIEN_CHIETKHAU: 0,
            TONG_TIENTHUE: 0,
            TONGTIEN_SAUTHUE: 0,
            TRANGTHAI: 0,
            DataDetails: []
        };
        $scope.addItem = {
            MAHANG: '',
            TENHANG: '',
            MANHACUNGCAP: '',
            MADONVITINH: '',
            MATHUE_RA: '',
            MATHUE_RA: '',
            TYLE_LAILE: 0,
            SOLUONG: 1,
            TONCUOIKYSL: 0,
            GIAVON_VAT: 0,
            GIABANLE: 0,
            GIABANLE_VAT: 0,
            TIEN_GIAMGIA: 0,
            THANHTIEN: 0,
            THANHTIEN_VAT: 0,
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
        //Tham số cấu hình sử dụng chức năng
        thamSoHeThongService.getDataByMaThamSo().then(function (successRes) {
            if (successRes && successRes.status === 200 && successRes.data && successRes.data.Status && successRes.data.Data) {
                angular.forEach(successRes.data.Data, function (v, k) {
                    if (v.MA_THAMSO === 'DEFAULT_KHOXUAT' && v.GIATRI_SO === 10 && v.GIATRI_CHU.trim() != '') {
                        $scope.target.MAKHO_XUAT = v.GIATRI_CHU.trim();
                    }
                });
            }
        });
        //end tham số hệ thống
        //Function load data catalog NhaCungCap
        function loadDataNhaCungCap() {
            $scope.nhaCungCap = [];
            if (!tempDataService.tempData('nhaCungCap')) {
                nhaCungCapService.getAllData().then(function (successRes) {
                    if (successRes && successRes.status === 200 && successRes.data && successRes.data.Status && successRes.data.Data && successRes.data.Data.length > 0) {
                        tempDataService.putTempData('nhaCungCap', successRes.data.Data);
                        $scope.nhaCungCap = successRes.data.Data;
                    }
                }, function (errorRes) {
                    console.log('errorRes', errorRes);
                });
            } else {
                $scope.nhaCungCap = tempDataService.tempData('nhaCungCap');
            }
        };
        loadDataNhaCungCap();
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

        //Function load data catalog Thue
        function loadDataThue() {
            $scope.thue = [];
            if (!tempDataService.tempData('thue')) {
                thueService.getAllData().then(function (successRes) {
                    if (successRes && successRes.status === 200 && successRes.data && successRes.data.Status && successRes.data.Data && successRes.data.Data.length > 0) {
                        tempDataService.putTempData('thue', successRes.data.Data);
                        $scope.thue = successRes.data.Data;
                    }
                }, function (errorRes) {
                    console.log('errorRes', errorRes);
                });
            } else {
                $scope.thue = tempDataService.tempData('thue');
            }
        };
        loadDataThue();
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

        //Function load data catalog KhoHang
        function loadDataKhachHang() {
            $scope.khachHang = [];
            if (!tempDataService.tempData('khachHang')) {
                khachHangService.getAllData().then(function (successRes) {
                    if (successRes && successRes.status === 200 && successRes.data && successRes.data.Status && successRes.data.Data && successRes.data.Data.length > 0) {
                        tempDataService.putTempData('khachHang', successRes.data.Data);
                        $scope.khachHang = successRes.data.Data;
                    }
                }, function (errorRes) {
                    console.log('errorRes', errorRes);
                });
            } else {
                $scope.khachHang = tempDataService.tempData('khachHang');
            }
        };
        loadDataKhachHang();
        //end

        //Tạo mới mã phiếu
        service.buildNewCode().then(function (successRes) {
            if (successRes && successRes.status == 200 && successRes.data) {
                $scope.target.MA_CHUNGTU = successRes.data;
            }
        });
        //end

        //GET CURRENT PERIOD
        kyKeToanService.getKyKeToan().then(function (successRes) {
            if (successRes && successRes.status == 200 && successRes.data && successRes.data.Status && successRes.data.Data) {
                $scope.target.NGAY_CHUNGTU = new Date(successRes.data.Data.TUNGAY);
                $scope.target.TABLE_NAME = 'XNT_' + successRes.data.Data.NAM + '_KY_' + successRes.data.Data.KY;
            }
        });
        //END

        //Tính toán giá
        function getGiaTriVatRa(maThue) {
            var giaTri = 0;
            if (maThue) {
                if ($scope.tempData('thue') && $scope.tempData('thue').length > 0) {
                    var dataTax = $filter('filter')($scope.tempData('thue'), { VALUE: maThue }, true);
                    if (dataTax && dataTax.length === 1) {
                        giaTri = dataTax[0].GIATRI;
                    }
                }
                else {
                    thueService.getDataByMaThue(maThue).then(function (successRes) {
                        if (successRes && successRes.status === 200 && successRes.data && successRes.data.Status && successRes.data.Data) {
                            giaTri = successRes.data.Data.GIATRI;
                        }
                    });
                }
            }
            else {
                giaTri = 0;
            }
            return giaTri;
        };
        function caculatorThanhTien(addItem) {
            if (addItem && addItem.MAHANG) {
                addItem.THANHTIEN = Math.round(100 * (addItem.SOLUONG * addItem.GIABANLE_VAT)) / 100;
                addItem.THANHTIEN_VAT = Math.round(100 * (addItem.THANHTIEN * (1 + (getGiaTriVatRa(addItem.MATHUE_RA)) / 100))) / 100;
                addItem.INDEX = $scope.target.DataDetails.length + 1;
            }
        };
       
        function errorFocusMaHang() {
            if (document.getElementById('_maHangAddItem') != null) {
                focus('_maHangAddItem');
                document.getElementById('_maHangAddItem').focus();
                document.getElementById('_maHangAddItem').select();
            }
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
                    controller: 'matHangInventorySearch_Ctrl',
                    windowClass: 'search-window',
                    resolve: {
                        serviceSelectData: function () {
                            return matHangService;
                        },
                        filterObject: function () {
                            return {
                                keySearch: strKey,
                                tableName: $scope.target.TABLE_NAME,
                                maKho: $scope.target.MAKHO_XUAT
                            };
                        }
                    }
                });
                modalInstance.result.then(function (refundedData) { 
                    if (refundedData) {
                        $scope.addItem = {
                            MAHANG: refundedData[0].MAHANG,
                            TENHANG: refundedData[0].TENHANG,
                            MANHACUNGCAP: refundedData[0].MANHACUNGCAP,
                            MADONVITINH: refundedData[0].MADONVITINH,
                            MATHUE_VAO: refundedData[0].MATHUE_VAO,
                            MATHUE_RA: refundedData[0].MATHUE_RA,
                            TYLE_LAILE: refundedData[0].TYLE_LAILE,
                            SOLUONG: 1,
                            GIABANLE: refundedData[0].GIABANLE,
                            GIABANLE_VAT: refundedData[0].GIABANLE_VAT,
                            GIAVON: refundedData[0].GIAVON,
                            GIAVON_VAT: Math.round(100 * (refundedData[0].GIAVON * (1 + (getGiaTriVatRa(refundedData[0].MATHUE_RA)) / 100))) / 100,
                            TONCUOIKYSL: refundedData[0].TONCUOIKYSL,
                            TIEN_GIAMGIA: 0,
                            INDEX: 0
                        };
                        caculatorThanhTien($scope.addItem);
                        $scope.pageChanged();
                    } else { errorFocusMaHang(); }
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
                    MAKHO_XUAT: $scope.target.MAKHO_XUAT,
                    TABLE_NAME: $scope.target.TABLE_NAME,
                    UNITCODE: unitCode
                }
                matHangService.getMatHangXuatBanTheoMaKho(obj).then(function (successRes) {
                    if (successRes && successRes.status === 200 && successRes.data && successRes.data.Status && successRes.data.Data) {
                        $scope.addItem.BARCODE = successRes.data.Data.BARCODE;
                        $scope.addItem.MAHANG = successRes.data.Data.MAHANG;
                        $scope.addItem.TENHANG = successRes.data.Data.TENHANG;
                        $scope.addItem.MATHUE_VAO = successRes.data.Data.MATHUE_VAO;
                        $scope.addItem.MATHUE_RA = successRes.data.Data.MATHUE_RA;
                        $scope.addItem.MANHACUNGCAP = successRes.data.Data.MANHACUNGCAP;
                        $scope.addItem.MADONVITINH = successRes.data.Data.MADONVITINH;
                        $scope.addItem.SOLUONG = 1;
                        $scope.addItem.TONCUOIKYSL = successRes.data.Data.TONCUOIKYSL;
                        $scope.addItem.GIAVON = successRes.data.Data.GIAVON;
                        $scope.addItem.GIAVON_VAT = Math.round(100 * ($scope.addItem.GIAVON * (1 + (getGiaTriVatRa($scope.addItem.MATHUE_RA)) / 100))) / 100;
                        $scope.addItem.GIABANLE = successRes.data.Data.GIABANLE;
                        $scope.addItem.GIABANLE_VAT = successRes.data.Data.GIABANLE_VAT;
                        $scope.addItem.TIEN_GIAMGIA = 0;
                        $scope.addItem.TYLE_LAILE = successRes.data.Data.TYLE_LAILE;
                        $scope.addItem.THANHTIEN = Math.round(100 * ($scope.addItem.SOLUONG * $scope.addItem.GIABANLE_VAT)) / 100;
                        $scope.addItem.THANHTIEN_VAT = Math.round(100 * ($scope.addItem.THANHTIEN * (1 + (getGiaTriVatRa($scope.addItem.MATHUE_RA)) / 100))) / 100;
                        $scope.addItem.INDEX = $scope.target.DataDetails.length + 1;
                        document.getElementById('_soLuongAddItem').focus();
                        document.getElementById('_soLuongAddItem').select();
                    } else if (successRes && successRes.status === 200 && successRes.data && !successRes.data.Status && !successRes.data.Data && successRes.data.Message == 'NOTEXISTS_MAKHO_XUAT') {
                        Lobibox.notify('warning', {
                            title: 'Kiểm tra thông tin',
                            msg: 'Chưa chọn mã kho xuất',
                            delay: 2500
                        });
                        $scope.addItem.MAHANG = '';
                    } else if (successRes && successRes.status === 200 && successRes.data && !successRes.data.Status && !successRes.data.Data && successRes.data.Message == 'NOTEXISTS_MAHANG') {
                        Lobibox.notify('warning', {
                            title: 'Kiểm tra thông tin',
                            msg: 'Chưa chọn hàng',
                            delay: 2500
                        });
                        $scope.addItem.MAHANG = '';
                    } else if (successRes && successRes.status === 200 && successRes.data && !successRes.data.Status && !successRes.data.Data && successRes.data.Message == 'NOTEXISTS_TABLE_NAME') {
                        Lobibox.notify('warning', {
                            title: 'Kiểm tra thông tin',
                            msg: 'Kiểm tra dữ liệu hệ thống khóa sổ',
                            delay: 2500
                        });
                        $scope.addItem.MAHANG = '';
                    }
                    else {
                        //bật lên modal tìm kiếm mặt hàng
                        $scope.searchMatHang(maHang);
                    }
                });
            }
        };

        $scope.changedSoLuong = function (addItem) {
            if (addItem) {
                if (!addItem.SOLUONG) addItem.SOLUONG = 1;
                addItem.THANHTIEN = Math.round(100 * (addItem.SOLUONG * addItem.GIABANLE_VAT)) / 100;
                addItem.THANHTIEN_VAT = Math.round(100 * (addItem.THANHTIEN * (1 + (getGiaTriVatRa(addItem.MATHUE_RA)) / 100))) / 100;
            }
        };

        $scope.changedGiamGia = function (addItem) {
            if (addItem) {
                if (!addItem.SOLUONG) addItem.SOLUONG = 1;
                if (!addItem.TIEN_GIAMGIA) addItem.TIEN_GIAMGIA = 0;
                if (addItem.TIEN_GIAMGIA <= 100) {
                    //giảm giá theo %
                    addItem.THANHTIEN = Math.round(100 * ((addItem.SOLUONG * addItem.GIABANLE_VAT) - (addItem.TIEN_GIAMGIA * (addItem.SOLUONG * addItem.GIABANLE_VAT)) / 100)) / 100;
                }
                else {
                    //giảm giá theo số tiền
                    addItem.THANHTIEN = Math.round(100 * ((addItem.SOLUONG * addItem.GIABANLE_VAT) - addItem.TIEN_GIAMGIA)) / 100;
                }
                addItem.THANHTIEN_VAT = Math.round(100 * (addItem.THANHTIEN * (1 + (getGiaTriVatRa(addItem.MATHUE_RA)) / 100))) / 100;
                $scope.$watch("target.DataDetails", function (newValue, oldValue) {
                    $scope.target.TIEN_CHIETKHAU = TinhTongTien($scope.target.DataDetails, 'TIEN_GIAMGIA');
                }, true);
            }
        };
        
        $scope.changedGiaBanLeVat = function (addItem) {
            if (addItem) {
                if (!addItem.SOLUONG) addItem.SOLUONG = 1;
                if (!addItem.TIEN_GIAMGIA) addItem.TIEN_GIAMGIA = 0
                addItem.TIEN_GIAMGIA = addItem.GIABANLE_VAT - addItem.GIABANLE;
                addItem.THANHTIEN = addItem.SOLUONG * addItem.GIABANLE_VAT - addItem.TIEN_GIAMGIA;
                addItem.THANHTIEN_VAT = Math.round(100 * (addItem.THANHTIEN * (1 + (getGiaTriVatRa(addItem.MATHUE_RA)) / 100))) / 100;
            }
        };

        $scope.changedTienChietKhauTong = function (chietKhauTong) {
            if (!chietKhauTong) {
                chietKhauTong = 0;
                $scope.target.TIEN_CHIETKHAU = 0;
            }
            if (chietKhauTong <= 100) {
                //theo tỷ lệ
                if ($scope.target.DataDetails && $scope.target.DataDetails.length > 0) {
                    angular.forEach($scope.target.DataDetails, function (value, key) {
                        value.TIEN_GIAMGIA = Math.round(100 * ((chietKhauTong * value.SOLUONG * value.GIABANLE_VAT) / 100)) / 100;
                        value.THANHTIEN = Math.round(100 * ((value.SOLUONG * value.GIABANLE_VAT) - value.TIEN_GIAMGIA)) / 100;
                        value.THANHTIEN_VAT = Math.round(100 * (value.THANHTIEN * (1 + (getGiaTriVatRa(value.MATHUE_RA)) / 100))) / 100;
                    });
                }
            } else {
                //theo tiền
                if ($scope.target.DataDetails && $scope.target.DataDetails.length > 0) {
                    var TIEN_GIAMGIA_TUNGMATHANG = Math.round(100 * (chietKhauTong / $scope.target.DataDetails.length)) / 100;
                    angular.forEach($scope.target.DataDetails, function (value, key) {
                        value.TIEN_GIAMGIA = TIEN_GIAMGIA_TUNGMATHANG;
                        value.THANHTIEN = Math.round(100 * ((value.SOLUONG * value.GIABANLE_VAT) - value.TIEN_GIAMGIA)) / 100;
                        value.THANHTIEN_VAT = Math.round(100 * (value.THANHTIEN * (1 + (getGiaTriVatRa(value.MATHUE_RA)) / 100))) / 100;
                    });
                }
            }
        };

        $scope.pageChanged = function () {
            var currentPage = $scope.paged.CurrentPage;
            var itemsPerPage = 10;
            $scope.paged.totalItems = $scope.target.DataDetails.length;
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
                    exist[0].SOLUONG = exist[0].SOLUONG + $scope.addItem.SOLUONG;
                    exist[0].GIABANLE = $scope.addItem.GIABANLE;
                    exist[0].GIABANLE_VAT = $scope.addItem.GIABANLE_VAT;
                    exist[0].GIAVON_VAT = $scope.addItem.GIAVON_VAT;
                    exist[0].MATHUE_RA = $scope.addItem.MATHUE_RA;
                    exist[0].THANHTIEN = Math.round(100 * (exist[0].SOLUONG * exist[0].GIABANLE_VAT)) / 100;
                    exist[0].THANHTIEN_VAT = Math.round(100 * (exist[0].THANHTIEN * (1 + (getGiaTriVatRa(exist[0].MATHUE_RA)) / 100))) / 100;
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
                    MANHACUNGCAP: '',
                    MADONVITINH: '',
                    MATHUE_RA: '',
                    MATHUE_RA: '',
                    TYLE_LAILE: 0,
                    SOLUONG: 1,
                    TONCUOIKYSL: 0,
                    GIAVON_VAT: 0,
                    GIABANLE: 0,
                    GIABANLE_VAT: 0,
                    TIEN_GIAMGIA: 0,
                    THANHTIEN: 0,
                    THANHTIEN_VAT: 0,
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

        function TinhTongTienThue(listObj) {
            var total = 0;
            if (listObj && listObj.length > 0) {
                angular.forEach(listObj, function (obj, idx) {
                    var increase = obj['THANHTIEN_VAT'];
                    if (!increase) {
                        increase = 0;
                    }
                    var minus = obj['THANHTIEN'];
                    if (!minus) {
                        minus = 0;
                    }
                    total += (increase - minus);
                });
            }
            return total;
        };

        //export excel
        $scope.exportData = [];
        // Prepare Excel data:
        $scope.fileName = "XuatBanExport_" + $scope.target.MA_CHUNGTU;
        //end

        function listenerDataDetails() {
            $scope.$watch("target.DataDetails", function (newValue, oldValue) {
                $scope.target.TONGTIEN_TRUOCTHUE = TinhTongTien($scope.target.DataDetails, 'THANHTIEN_VAT');
                $scope.target.TONG_TIENTHUE = TinhTongTienThue($scope.target.DataDetails);
                $scope.target.TONGTIEN_SAUTHUE = TinhTongTien($scope.target.DataDetails, 'THANHTIEN');
                //add data list export excel
                if (newValue && newValue.length > 0) {
                    $scope.exportData = [];
                    // Headers:
                    $scope.exportData.push(["STT", "MAHANG", "TENHANG", "MADONVITINH", "MATHUE_RA", "TYLE_LAILE", "SOLUONG", "TONCUOIKYSL", "GIABANLE", "GIABANLE_VAT", "TIEN_GIAMGIA", "THANHTIEN"]);
                    angular.forEach(newValue, function (value, idx) {
                        $scope.exportData.push([value.INDEX, value.MAHANG, value.TENHANG, value.MADONVITINH, value.MATHUE_RA, value.TYLE_LAILE, value.SOLUONG, value.TONCUOIKYSL, value.GIABANLE, value.GIABANLE_VAT, value.TIEN_GIAMGIA, value.THANHTIEN]);
                    });
                }
            }, true);
        };
        listenerDataDetails();

        $scope.save = function () {
            if (!$scope.target.MA_CHUNGTU || !$scope.target.NGAY_CHUNGTU || !$scope.target.MAKHACHHANG || !$scope.target.MAKHO_XUAT || $scope.target.DataDetails.length <= 0) {
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
                    $scope.target.NGAY_CHUNGTU = $scope.config.moment($scope.target.NGAY_CHUNGTU).format();
                    $scope.target.TIEN_CHIETKHAU = TinhTongTien($scope.target.DataDetails, 'TIEN_GIAMGIA');
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


    app.controller('xuatBanDetail_Ctrl', ['$scope', '$uibModalInstance', '$http', 'configService', 'xuatBanService', 'targetData', 'tempDataService', '$filter', '$uibModal', '$log','thueService','kyKeToanService',
        function ($scope, $uibModalInstance, $http, configService, service, targetData, tempDataService, $filter, $uibModal, $log, thueService, kyKeToanService) {
            $scope.config = angular.copy(configService);
            $scope.tempData = tempDataService.tempData;
            $scope.paged = angular.copy(configService.pageDefault);
            $scope.target = {};
            $scope.target = angular.copy(targetData);
            //export excel
            $scope.exportData = [];
            // Prepare Excel data:
            $scope.fileName = "XuatBanExport_" + targetData.MA_CHUNGTU;
            $scope.exportData.push(["STT", "MAHANG", "TENHANG", "MADONVITINH", "MATHUE_RA", "TYLE_LAILE", "SOLUONG", "TONCUOIKYSL", "GIABANLE", "GIABANLE_VAT", "TIEN_GIAMGIA", "THANHTIEN"]);
            //end
            $scope.title = function () { return 'Thông tin phiếu xuất bán buôn [' + targetData.MA_CHUNGTU + ']'; };
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

            function TinhTongTienThue(listObj) {
                var total = 0;
                if (listObj && listObj.length > 0) {
                    angular.forEach(listObj, function (obj, idx) {
                        var increase = obj['THANHTIEN_VAT'];
                        if (!increase) {
                            increase = 0;
                        }
                        var minus = obj['THANHTIEN'];
                        if (!minus) {
                            minus = 0;
                        }
                        total += (increase - minus);
                    });
                }
                return total;
            };
            //Tính toán giá
            function getGiaTriVatRa(maThue) {
                var giaTri = 0;
                if (maThue) {
                    if ($scope.tempData('thue') && $scope.tempData('thue').length > 0) {
                        var dataTax = $filter('filter')($scope.tempData('thue'), { VALUE: maThue }, true);
                        if (dataTax && dataTax.length === 1) {
                            giaTri = dataTax[0].GIATRI;
                        }
                    }
                    else {
                        thueService.getDataByMaThue(maThue).then(function (successRes) {
                            if (successRes && successRes.status === 200 && successRes.data && successRes.data.Status && successRes.data.Data) {
                                giaTri = successRes.data.Data.GIATRI;
                            }
                        });
                    }
                }
                else {
                    giaTri = 0;
                }
                return giaTri;
            };
            //end
            function listenerDataDetails() {
                $scope.$watch("target.DataDetails", function (newValue, oldValue) {
                    $scope.target.TONGTIEN_TRUOCTHUE = TinhTongTien($scope.target.DataDetails, 'THANHTIEN_VAT');
                    $scope.target.TONG_TIENTHUE = TinhTongTienThue($scope.target.DataDetails);
                    $scope.target.TONGTIEN_SAUTHUE = TinhTongTien($scope.target.DataDetails, 'THANHTIEN');
                }, true);
            };
            listenerDataDetails();

           
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
                //GET PERIOD BY NGAY_CHUNGTU
                var obj = {
                    TUNGAY: $scope.config.moment(targetData.NGAY_CHUNGTU).format(),
                    DENNGAY: $scope.config.moment(targetData.NGAY_CHUNGTU).format()
                };
                kyKeToanService.getKyKeToanTheoNgay(obj).then(function (successRes) {
                    if (successRes && successRes.status == 200 && successRes.data && successRes.data.Status && successRes.data.Data) {
                        $scope.target.TABLE_NAME = 'XNT_' + successRes.data.Data.NAM + '_KY_' + successRes.data.Data.KY;
                        service.getDetails($scope.target.ID, $scope.target.TABLE_NAME).then(function (sucessRes) {
                            if (sucessRes && sucessRes.status === 200 && sucessRes.data && sucessRes.data.Status && sucessRes.data.Data) {
                                $scope.target = sucessRes.data.Data;
                                $scope.target.NGAY_CHUNGTU = new Date($scope.target.NGAY_CHUNGTU);
                                if ($scope.target.DataDetails && $scope.target.DataDetails.length > 0) {
                                    angular.forEach($scope.target.DataDetails, function (value, idx) {
                                        value.THANHTIEN_VAT = Math.round(100 * (value.THANHTIEN * (1 + (getGiaTriVatRa(value.MATHUE_RA)) / 100))) / 100;
                                        value.GIAVON_VAT = Math.round(100 * (value.GIAVON * (1 + (getGiaTriVatRa(value.MATHUE_RA)) / 100))) / 100;
                                        $scope.exportData.push([value.INDEX, value.MAHANG, value.TENHANG, value.MADONVITINH, value.MATHUE_RA, value.TYLE_LAILE, value.SOLUONG, value.TONCUOIKYSL, value.GIABANLE, value.GIABANLE_VAT, value.TIEN_GIAMGIA, value.THANHTIEN]);
                                    });
                                }
                                $scope.pageChanged();
                            }
                        });
                    }
                });
                //END
            };
            filterData();
            $scope.cancel = function () {
                $uibModalInstance.close();
            };
        }]);

    app.controller('xuatBanApproval_Ctrl', ['$scope', '$uibModalInstance', '$http', 'configService', 'xuatBanService', 'targetData', 'tempDataService', '$filter', '$uibModal', '$log', 'thueService', 'donViTinhService', 'userService', 'nhaCungCapService', 'cuaHangService', 'khachHangService', 'kyKeToanService', '$mdDialog',
        function ($scope, $uibModalInstance, $http, configService, service, targetData, tempDataService, $filter, $uibModal, $log, thueService, donViTinhService, userService, nhaCungCapService, cuaHangService, khachHangService, kyKeToanService, $mdDialog) {
            $scope.config = angular.copy(configService);
            $scope.tempData = tempDataService.tempData;
            $scope.currentUser = userService.GetCurrentUser();
            $scope.createDay = new Date();
            $scope.target = {};
            $scope.target = angular.copy(targetData);
            $scope.title = function () { return 'Duyệt phiếu xuất bán buôn [' + targetData.MA_CHUNGTU + ']'; };
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
            //Function load data catalog NhaCungCap
            function loadDataNhaCungCap() {
                $scope.nhaCungCap = [];
                if (!tempDataService.tempData('nhaCungCap')) {
                    nhaCungCapService.getAllData().then(function (successRes) {
                        if (successRes && successRes.status === 200 && successRes.data && successRes.data.Status && successRes.data.Data && successRes.data.Data.length > 0) {
                            tempDataService.putTempData('nhaCungCap', successRes.data.Data);
                            $scope.nhaCungCap = successRes.data.Data;
                        }
                    }, function (errorRes) {
                        console.log('errorRes', errorRes);
                    });
                } else {
                    $scope.nhaCungCap = tempDataService.tempData('nhaCungCap');
                }
            };
            loadDataNhaCungCap();
            //end

            //Function load data catalog KhoHang
            function loadDataKhachHang() {
                $scope.khachHang = [];
                if (!tempDataService.tempData('khachHang')) {
                    khachHangService.getAllData().then(function (successRes) {
                        if (successRes && successRes.status === 200 && successRes.data && successRes.data.Status && successRes.data.Data && successRes.data.Data.length > 0) {
                            tempDataService.putTempData('khachHang', successRes.data.Data);
                            $scope.khachHang = successRes.data.Data;
                        }
                    }, function (errorRes) {
                        console.log('errorRes', errorRes);
                    });
                } else {
                    $scope.khachHang = tempDataService.tempData('khachHang');
                }
            };
            loadDataKhachHang();
            //end

            //Function load data catalog NhaCungCap
            function loadDataCuaHang() {
                $scope.cuaHang = [];
                if (!tempDataService.tempData('cuaHang')) {
                    cuaHangService.getAllData().then(function (successRes) {
                        if (successRes && successRes.status === 200 && successRes.data && successRes.data.Status && successRes.data.Data && successRes.data.Data.length > 0) {
                            tempDataService.putTempData('cuaHang', successRes.data.Data);
                            $scope.cuaHang = successRes.data.Data;
                        }
                    }, function (errorRes) {
                        console.log('errorRes', errorRes);
                    });
                } else {
                    $scope.cuaHang = tempDataService.tempData('cuaHang');
                }
            };
            loadDataCuaHang();
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

            function TinhTongTienThue(listObj) {
                var total = 0;
                if (listObj && listObj.length > 0) {
                    angular.forEach(listObj, function (obj, idx) {
                        var increase = obj['THANHTIEN_VAT'];
                        if (!increase) {
                            increase = 0;
                        }
                        var minus = obj['THANHTIEN'];
                        if (!minus) {
                            minus = 0;
                        }
                        total += (increase - minus);
                    });
                }
                return total;
            };
            //Tính toán giá
            function getGiaTriVatRa(maThue) {
                var giaTri = 0;
                if (maThue) {
                    if ($scope.tempData('thue') && $scope.tempData('thue').length > 0) {
                        var dataTax = $filter('filter')($scope.tempData('thue'), { VALUE: maThue }, true);
                        if (dataTax && dataTax.length === 1) {
                            giaTri = dataTax[0].GIATRI;
                        }
                    }
                    else {
                        thueService.getDataByMaThue(maThue).then(function (successRes) {
                            if (successRes && successRes.status === 200 && successRes.data && successRes.data.Status && successRes.data.Data) {
                                giaTri = successRes.data.Data.GIATRI;
                            }
                        });
                    }
                }
                else {
                    giaTri = 0;
                }
                return giaTri;
            };
            //end
            function listenerDataDetails() {
                $scope.$watch("target.DataDetails", function (newValue, oldValue) {
                    $scope.target.TONGTIEN_TRUOCTHUE = TinhTongTien($scope.target.DataDetails, 'THANHTIEN_VAT');
                    $scope.target.TONG_TIENTHUE = TinhTongTienThue($scope.target.DataDetails);
                    $scope.target.TONGTIEN_SAUTHUE = TinhTongTien($scope.target.DataDetails, 'THANHTIEN');
                    $scope.target.TONG_SOLUONG = TinhTongTien($scope.target.DataDetails, 'SOLUONG');
                }, true);
            };
            listenerDataDetails();

            function filterData() {
                //GET PERIOD BY NGAY_CHUNGTU
                var obj = {
                    TUNGAY: $scope.config.moment(targetData.NGAY_CHUNGTU).format(),
                    DENNGAY: $scope.config.moment(targetData.NGAY_CHUNGTU).format()
                };
                kyKeToanService.getKyKeToanTheoNgay(obj).then(function (successRes) {
                    if (successRes && successRes.status == 200 && successRes.data && successRes.data.Status && successRes.data.Data) {
                        $scope.target.TABLE_NAME = 'XNT_' + successRes.data.Data.NAM + '_KY_' + successRes.data.Data.KY;
                        service.getDetails($scope.target.ID, $scope.target.TABLE_NAME).then(function (sucessRes) {
                            if (sucessRes && sucessRes.status === 200 && sucessRes.data && sucessRes.data.Status && sucessRes.data.Data) {
                                $scope.target = sucessRes.data.Data;
                                $scope.target.NGAY_CHUNGTU = new Date($scope.target.NGAY_CHUNGTU);
                                if ($scope.target.DataDetails && $scope.target.DataDetails.length > 0) {
                                    angular.forEach($scope.target.DataDetails, function (value, idx) {
                                        value.THANHTIEN_VAT = Math.round(100 * (value.THANHTIEN * (1 + (getGiaTriVatRa(value.MATHUE_RA)) / 100))) / 100;
                                        value.GIAVON_VAT = Math.round(100 * (value.GIAVON * (1 + (getGiaTriVatRa(value.MATHUE_RA)) / 100))) / 100;
                                    });
                                }
                                //get info KHACHHANG BY CHUNGTU
                                $scope.thongTinKhachHang = {};
                                var khachHang = $filter('filter')($scope.tempData('khachHang'), { VALUE: $scope.target.MAKHACHHANG }, true);
                                if (khachHang && khachHang.length === 1) {
                                    $scope.thongTinKhachHang = khachHang[0];
                                }
                                //end
                            }
                        });
                    }
                });
                //END
            };
            filterData();

            $scope.accept = function (ID) {
                if (ID && ID.length > 0) {
                    var obj = {
                        ID: ID
                    };
                    service.postApproval(obj).then(function (successRes) {
                        if (successRes && successRes.status === 200 && successRes.data && successRes.data.Status && successRes.data.Data) {
                            Lobibox.notify('success', {
                                title: 'Thông báo',
                                width: 400,
                                msg: successRes.data.Message,
                                delay: 2500
                            });
                            $uibModalInstance.close($scope.target);
                        } else if (successRes && successRes.status === 200 && successRes.data && !successRes.data.Status && successRes.data.Message === 'HAVENOT_CLOSINGOUT_PERIOD') {
                            $uibModalInstance.close($scope.target);
                            // Appending dialog to document.body to cover sidenav in docs app
                            var confirm = $mdDialog.confirm()
                                  .title('Tìm thấy kỳ kế toán chưa khóa ?')
                                  .textContent('Tồn tại kỳ kế toán chưa khóa! Bạn hãy khóa kỳ đến ngày hiện tại')
                                  .ariaLabel('Lucky day')
                                  .cancel('Bỏ qua')
                                  .ok('Khóa!');
                            $mdDialog.show(confirm).then(function () {
                                if (successRes.data.Data.length > 0) {
                                    var paramPost = [];
                                    angular.forEach(successRes.data.Data, function (v, k) {
                                        v.NGAYKETOAN = $scope.config.moment(v.NGAYKETOAN).format();
                                        paramPost.push(v);
                                    });
                                    kyKeToanService.closingOutListPeriodNotLock(paramPost).then(function (refundRes) {
                                        if (refundRes && refundRes.status === 200 && refundRes.data && refundRes.data.Status && refundRes.data.Message === 'CLOSING_ALLPERIOD_SUCCESS') {
                                            Lobibox.notify('success', {
                                                title: 'Thông báo',
                                                width: 400,
                                                msg: 'Khóa sổ nhiều kỳ thành công',
                                                delay: 2500
                                            });
                                        } else if (refundRes && refundRes.status === 200 && refundRes.data && !refundRes.data.Status && refundRes.data.Message === 'CLOSING_ALLPERIOD_NOTSUCCESS') {
                                            Lobibox.notify('warning', {
                                                title: 'Cảnh báo',
                                                msg: 'Khóa sổ nhiều kỳ không thành công',
                                                delay: 3000
                                            });
                                        } else if (refundRes && refundRes.status === 200 && refundRes.data && !refundRes.data.Status && refundRes.data.Message === 'CLOSING_ALLPERIOD_ERROR') {
                                            Lobibox.notify('error', {
                                                title: 'Xảy ra lỗi',
                                                msg: 'Đã xảy ra lỗi',
                                                delay: 3000
                                            });
                                        }
                                    });
                                }
                            }, function () {
                                $scope.status = 'You decided to keep your debt.';
                            });
                        } else {
                            Lobibox.notify('error', {
                                title: 'Xảy ra lỗi',
                                msg: successRes.data.Message,
                                delay: 3000
                            });
                        }
                    });
                } else {
                    Lobibox.notify('warning', {
                        title: 'Dữ liệu duyệt không hợp lệ',
                        msg: 'Thông tin ID phiếu không tồn tại',
                        delay: 3000
                    });
                }
            };
            $scope.cancel = function () {
                $uibModalInstance.close();
            };
        }]);

    app.controller('xuatBanEdit_Ctrl', ['$scope', '$uibModalInstance', '$http', 'configService', 'xuatBanService', 'targetData', 'tempDataService', '$filter', '$uibModal', '$log','thueService','matHangService','userService','kyKeToanService',
        function ($scope, $uibModalInstance, $http, configService, service, targetData, tempDataService, $filter, $uibModal, $log, thueService, matHangService, userService, kyKeToanService) {
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
                MANHACUNGCAP: '',
                MADONVITINH: '',
                MATHUE_RA: '',
                MATHUE_RA: '',
                TYLE_LAILE: 0,
                SOLUONG: 1,
                TONCUOIKYSL: 0,
                GIAVON_VAT: 0,
                GIABANLE: 0,
                GIABANLE_VAT: 0,
                TIEN_GIAMGIA: 0,
                THANHTIEN: 0,
                THANHTIEN_VAT: 0,
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
            $scope.title = function () { return 'Chỉnh sửa phiếu xuất bán buôn'; };
            $scope.refresh = function () {
                tempDataService.refreshData();
            };

            //Tính toán giá
            function getGiaTriVatRa(maThue) {
                var giaTri = 0;
                if (maThue) {
                    if ($scope.tempData('thue') && $scope.tempData('thue').length > 0) {
                        var dataTax = $filter('filter')($scope.tempData('thue'), { VALUE: maThue }, true);
                        if (dataTax && dataTax.length === 1) {
                            giaTri = dataTax[0].GIATRI;
                        }
                    }
                    else {
                        thueService.getDataByMaThue(maThue).then(function (successRes) {
                            if (successRes && successRes.status === 200 && successRes.data && successRes.data.Status && successRes.data.Data) {
                                giaTri = successRes.data.Data.GIATRI;
                            }
                        });
                    }
                }
                else {
                    giaTri = 0;
                }
                return giaTri;
            };
            function caculatorThanhTien(addItem) {
                if (addItem && addItem.MAHANG) {
                    addItem.THANHTIEN = Math.round(100 * (addItem.SOLUONG * addItem.GIABANLE_VAT)) / 100;
                    addItem.THANHTIEN_VAT = Math.round(100 * (addItem.THANHTIEN * (1 + (getGiaTriVatRa(addItem.MATHUE_RA)) / 100))) / 100;
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
                        controller: 'matHangInventorySearch_Ctrl',
                        windowClass: 'search-window',
                        resolve: {
                            serviceSelectData: function () {
                                return matHangService;
                            },
                            filterObject: function () {
                                return {
                                    keySearch: strKey,
                                    tableName: $scope.target.TABLE_NAME,
                                    maKho: $scope.target.MAKHO_XUAT
                                };
                            }
                        }
                    });
                    modalInstance.result.then(function (refundedData) {
                        if (refundedData) {
                            $scope.addItem = {
                                MAHANG: refundedData[0].MAHANG,
                                TENHANG: refundedData[0].TENHANG,
                                MANHACUNGCAP: refundedData[0].MANHACUNGCAP,
                                MADONVITINH: refundedData[0].MADONVITINH,
                                MATHUE_VAO: refundedData[0].MATHUE_VAO,
                                MATHUE_RA: refundedData[0].MATHUE_RA,
                                TYLE_LAILE: refundedData[0].TYLE_LAILE,
                                SOLUONG: 1,
                                GIABANLE: refundedData[0].GIABANLE,
                                GIABANLE_VAT: refundedData[0].GIABANLE_VAT,
                                GIAVON: refundedData[0].GIAVON,
                                GIAVON_VAT: Math.round(100 * (refundedData[0].GIAVON * (1 + (getGiaTriVatRa(refundedData[0].MATHUE_RA)) / 100))) / 100,
                                TONCUOIKYSL: refundedData[0].TONCUOIKYSL,
                                TIEN_GIAMGIA: 0,
                                INDEX: 0
                            };
                            caculatorThanhTien($scope.addItem);
                            $scope.pageChanged();
                        } else { errorFocusMaHang(); }
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
                        MAKHO_XUAT: $scope.target.MAKHO_XUAT,
                        TABLE_NAME: $scope.target.TABLE_NAME,
                        UNITCODE: unitCode
                    }
                    matHangService.getMatHangXuatBanTheoMaKho(obj).then(function (successRes) {
                        if (successRes && successRes.status === 200 && successRes.data && successRes.data.Status && successRes.data.Data) {
                            $scope.addItem.BARCODE = successRes.data.Data.BARCODE;
                            $scope.addItem.MAHANG = successRes.data.Data.MAHANG;
                            $scope.addItem.TENHANG = successRes.data.Data.TENHANG;
                            $scope.addItem.MATHUE_VAO = successRes.data.Data.MATHUE_VAO;
                            $scope.addItem.MATHUE_RA = successRes.data.Data.MATHUE_RA;
                            $scope.addItem.MANHACUNGCAP = successRes.data.Data.MANHACUNGCAP;
                            $scope.addItem.MADONVITINH = successRes.data.Data.MADONVITINH;
                            $scope.addItem.SOLUONG = 1;
                            $scope.addItem.TONCUOIKYSL = successRes.data.Data.TONCUOIKYSL;
                            $scope.addItem.GIAVON = successRes.data.Data.GIAVON;
                            $scope.addItem.GIAVON_VAT = Math.round(100 * ($scope.addItem.GIAVON * (1 + (getGiaTriVatRa($scope.addItem.MATHUE_RA)) / 100))) / 100;
                            $scope.addItem.GIABANLE = successRes.data.Data.GIABANLE;
                            $scope.addItem.GIABANLE_VAT = successRes.data.Data.GIABANLE_VAT;
                            $scope.addItem.TIEN_GIAMGIA = 0;
                            $scope.addItem.TYLE_LAILE = successRes.data.Data.TYLE_LAILE;
                            $scope.addItem.THANHTIEN = Math.round(100 * ($scope.addItem.SOLUONG * $scope.addItem.GIABANLE_VAT)) / 100;
                            $scope.addItem.THANHTIEN_VAT = Math.round(100 * ($scope.addItem.THANHTIEN * (1 + (getGiaTriVatRa($scope.addItem.MATHUE_RA)) / 100))) / 100;
                            $scope.addItem.INDEX = $scope.target.DataDetails.length + 1;
                            document.getElementById('_soLuongAddItem').focus();
                            document.getElementById('_soLuongAddItem').select();
                        } else if (successRes && successRes.status === 200 && successRes.data && !successRes.data.Status && !successRes.data.Data && successRes.data.Message == 'NOTEXISTS_MAKHO_XUAT') {
                            Lobibox.notify('warning', {
                                title: 'Kiểm tra thông tin',
                                msg: 'Chưa chọn mã kho xuất',
                                delay: 2500
                            });
                            $scope.addItem.MAHANG = '';
                        } else if (successRes && successRes.status === 200 && successRes.data && !successRes.data.Status && !successRes.data.Data && successRes.data.Message == 'NOTEXISTS_MAHANG') {
                            Lobibox.notify('warning', {
                                title: 'Kiểm tra thông tin',
                                msg: 'Chưa chọn hàng',
                                delay: 2500
                            });
                            $scope.addItem.MAHANG = '';
                        } else if (successRes && successRes.status === 200 && successRes.data && !successRes.data.Status && !successRes.data.Data && successRes.data.Message == 'NOTEXISTS_TABLE_NAME') {
                            Lobibox.notify('warning', {
                                title: 'Kiểm tra thông tin',
                                msg: 'Kiểm tra dữ liệu hệ thống khóa sổ',
                                delay: 2500
                            });
                            $scope.addItem.MAHANG = '';
                        }
                        else {
                            //bật lên modal tìm kiếm mặt hàng
                            $scope.searchMatHang(maHang);
                        }
                    });
                }
            };

            $scope.changedSoLuong = function (addItem) {
                if (addItem) {
                    if (!addItem.SOLUONG) addItem.SOLUONG = 1;
                    addItem.THANHTIEN = Math.round(100 * (addItem.SOLUONG * addItem.GIABANLE_VAT)) / 100;
                    addItem.THANHTIEN_VAT = Math.round(100 * (addItem.THANHTIEN * (1 + (getGiaTriVatRa(addItem.MATHUE_RA)) / 100))) / 100;
                }
            };

            $scope.changedGiamGia = function (addItem) {
                if (addItem) {
                    if (!addItem.SOLUONG) addItem.SOLUONG = 1;
                    if (!addItem.TIEN_GIAMGIA) addItem.TIEN_GIAMGIA = 0;
                    if (addItem.TIEN_GIAMGIA <= 100) {
                        //giảm giá theo %
                        addItem.THANHTIEN = Math.round(100 * ((addItem.SOLUONG * addItem.GIABANLE_VAT) - (addItem.TIEN_GIAMGIA * (addItem.SOLUONG * addItem.GIABANLE_VAT)) / 100)) / 100;
                    }
                    else {
                        //giảm giá theo số tiền
                        addItem.THANHTIEN = Math.round(100 * ((addItem.SOLUONG * addItem.GIABANLE_VAT) - addItem.TIEN_GIAMGIA)) / 100;
                    }
                    addItem.THANHTIEN_VAT = Math.round(100 * (addItem.THANHTIEN * (1 + (getGiaTriVatRa(addItem.MATHUE_RA)) / 100))) / 100;
                    $scope.$watch("target.DataDetails", function (newValue, oldValue) {
                        $scope.target.TIEN_CHIETKHAU = TinhTongTien($scope.target.DataDetails, 'TIEN_GIAMGIA');
                    }, true);
                }
            };

            $scope.changedGiaBanLeVat = function (addItem) {
                if (addItem) {
                    if (!addItem.SOLUONG) addItem.SOLUONG = 1;
                    if (!addItem.TIEN_GIAMGIA) addItem.TIEN_GIAMGIA = 0
                    addItem.TIEN_GIAMGIA = addItem.GIABANLE_VAT - addItem.GIABANLE;
                    addItem.THANHTIEN = addItem.SOLUONG * addItem.GIABANLE_VAT - addItem.TIEN_GIAMGIA;
                    addItem.THANHTIEN_VAT = Math.round(100 * (addItem.THANHTIEN * (1 + (getGiaTriVatRa(addItem.MATHUE_RA)) / 100))) / 100;
                }
            };

            $scope.changedTienChietKhauTong = function (chietKhauTong) {
                if (!chietKhauTong) {
                    chietKhauTong = 0;
                    $scope.target.TIEN_CHIETKHAU = 0;
                }
                if (chietKhauTong <= 100) {
                    //theo tỷ lệ
                    if ($scope.target.DataDetails && $scope.target.DataDetails.length > 0) {
                        angular.forEach($scope.target.DataDetails, function (value, key) {
                            value.TIEN_GIAMGIA = Math.round(100 * ((chietKhauTong * value.SOLUONG * value.GIABANLE_VAT) / 100)) / 100;
                            value.THANHTIEN = Math.round(100 * ((value.SOLUONG * value.GIABANLE_VAT) - value.TIEN_GIAMGIA)) / 100;
                            value.THANHTIEN_VAT = Math.round(100 * (value.THANHTIEN * (1 + (getGiaTriVatRa(value.MATHUE_RA)) / 100))) / 100;
                        });
                    }
                } else {
                    //theo tiền
                    if ($scope.target.DataDetails && $scope.target.DataDetails.length > 0) {
                        var TIEN_GIAMGIA_TUNGMATHANG = Math.round(100 * (chietKhauTong / $scope.target.DataDetails.length)) / 100;
                        angular.forEach($scope.target.DataDetails, function (value, key) {
                            value.TIEN_GIAMGIA = TIEN_GIAMGIA_TUNGMATHANG;
                            value.THANHTIEN = Math.round(100 * ((value.SOLUONG * value.GIABANLE_VAT) - value.TIEN_GIAMGIA)) / 100;
                            value.THANHTIEN_VAT = Math.round(100 * (value.THANHTIEN * (1 + (getGiaTriVatRa(value.MATHUE_RA)) / 100))) / 100;
                        });
                    }
                }
            };

            $scope.pageChanged = function () {
                var currentPage = $scope.paged.CurrentPage;
                var itemsPerPage = 10;
                $scope.paged.totalItems = $scope.target.DataDetails.length;
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
                        exist[0].SOLUONG = exist[0].SOLUONG + $scope.addItem.SOLUONG;
                        exist[0].GIABANLE = $scope.addItem.GIABANLE;
                        exist[0].GIABANLE_VAT = $scope.addItem.GIABANLE_VAT;
                        exist[0].GIAVON_VAT = $scope.addItem.GIAVON_VAT;
                        exist[0].MATHUE_RA = $scope.addItem.MATHUE_RA;
                        exist[0].THANHTIEN = Math.round(100 * (exist[0].SOLUONG * exist[0].GIABANLE_VAT)) / 100;
                        exist[0].THANHTIEN_VAT = Math.round(100 * (exist[0].THANHTIEN * (1 + (getGiaTriVatRa(exist[0].MATHUE_RA)) / 100))) / 100;
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
                        MANHACUNGCAP: '',
                        MADONVITINH: '',
                        MATHUE_RA: '',
                        MATHUE_RA: '',
                        TYLE_LAILE: 0,
                        SOLUONG: 1,
                        TONCUOIKYSL: 0,
                        GIAVON_VAT: 0,
                        GIABANLE: 0,
                        GIABANLE_VAT: 0,
                        TIEN_GIAMGIA: 0,
                        THANHTIEN: 0,
                        THANHTIEN_VAT: 0,
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

            function TinhTongTienThue(listObj) {
                var total = 0;
                if (listObj && listObj.length > 0) {
                    angular.forEach(listObj, function (obj, idx) {
                        var increase = obj['THANHTIEN_VAT'];
                        if (!increase) {
                            increase = 0;
                        }
                        var minus = obj['THANHTIEN'];
                        if (!minus) {
                            minus = 0;
                        }
                        total += (increase - minus);
                    });
                }
                return total;
            };
            //export excel
            $scope.exportData = [];
            // Prepare Excel data:
            $scope.fileName = "XuatBanExport_" + $scope.target.MA_CHUNGTU;
            //end
            function listenerDataDetails() {
                $scope.$watch("target.DataDetails", function (newValue, oldValue) {
                    $scope.target.TONGTIEN_TRUOCTHUE = TinhTongTien($scope.target.DataDetails, 'THANHTIEN_VAT');
                    $scope.target.TONG_TIENTHUE = TinhTongTienThue($scope.target.DataDetails);
                    $scope.target.TONGTIEN_SAUTHUE = TinhTongTien($scope.target.DataDetails, 'THANHTIEN');
                    //add data list export excel
                    if (newValue && newValue.length > 0) {
                        $scope.exportData = [];
                        // Headers:
                        $scope.exportData.push(["STT", "MAHANG", "TENHANG", "MADONVITINH", "MATHUE_RA", "TYLE_LAILE", "SOLUONG", "TONCUOIKYSL", "GIABANLE", "GIABANLE_VAT", "TIEN_GIAMGIA", "THANHTIEN"]);
                        angular.forEach(newValue, function (value, idx) {
                            $scope.exportData.push([value.INDEX, value.MAHANG, value.TENHANG, value.MADONVITINH, value.MATHUE_RA, value.TYLE_LAILE, value.SOLUONG, value.TONCUOIKYSL, value.GIABANLE, value.GIABANLE_VAT, value.TIEN_GIAMGIA, value.THANHTIEN]);
                        });
                    }
                }, true);
            };
            listenerDataDetails();

            function filterData() {
                //GET PERIOD BY NGAY_CHUNGTU
                var obj = {
                    TUNGAY: $scope.config.moment(targetData.NGAY_CHUNGTU).format(),
                    DENNGAY: $scope.config.moment(targetData.NGAY_CHUNGTU).format()
                };
                kyKeToanService.getKyKeToanTheoNgay(obj).then(function (successRes) {
                    if (successRes && successRes.status == 200 && successRes.data && successRes.data.Status && successRes.data.Data) {
                        $scope.target.TABLE_NAME = 'XNT_' + successRes.data.Data.NAM + '_KY_' + successRes.data.Data.KY;
                        service.getDetails($scope.target.ID, $scope.target.TABLE_NAME).then(function (sucessRes) {
                            if (sucessRes && sucessRes.status === 200 && sucessRes.data && sucessRes.data.Status && sucessRes.data.Data) {
                                $scope.target = sucessRes.data.Data;
                                $scope.target.NGAY_CHUNGTU = new Date($scope.target.NGAY_CHUNGTU);
                                if ($scope.target.DataDetails && $scope.target.DataDetails.length > 0) {
                                    angular.forEach($scope.target.DataDetails, function (value, idx) {
                                        value.THANHTIEN_VAT = Math.round(100 * (value.THANHTIEN * (1 + (getGiaTriVatRa(value.MATHUE_RA)) / 100))) / 100;
                                        value.GIAVON_VAT = Math.round(100 * (value.GIAVON * (1 + (getGiaTriVatRa(value.MATHUE_RA)) / 100))) / 100;
                                        $scope.exportData.push([value.INDEX, value.MAHANG, value.TENHANG, value.MADONVITINH, value.MATHUE_RA, value.TYLE_LAILE, value.SOLUONG, value.TONCUOIKYSL, value.GIABANLE, value.GIABANLE_VAT, value.TIEN_GIAMGIA, value.THANHTIEN]);
                                    });
                                }
                                $scope.pageChanged();
                            }
                        });
                    }
                });
                //END
            };
            filterData();

            $scope.save = function () {
                if (!$scope.target.MA_CHUNGTU || !$scope.target.NGAY_CHUNGTU || !$scope.target.MAKHACHHANG || !$scope.target.MAKHO_XUAT || $scope.target.DataDetails.length <= 0) {
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
                        $scope.target.NGAY_CHUNGTU = $scope.config.moment($scope.target.NGAY_CHUNGTU).format();
                        $scope.target.TIEN_CHIETKHAU = TinhTongTien($scope.target.DataDetails, 'TIEN_GIAMGIA');
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

    app.controller('xuatBanDelete_Ctrl', ['$scope', '$uibModalInstance', '$http', 'configService', 'xuatBanService', 'targetData', 'tempDataService', '$filter', '$uibModal', '$log',
       function ($scope, $uibModalInstance, $http, configService, service, targetData, tempDataService, $filter, $uibModal, $log) {
           $scope.config = angular.copy(configService);
           $scope.tempData = tempDataService.tempData;
           $scope.target = {};
           $scope.target = angular.copy(targetData);;
           $scope.title = function () { return 'Xóa phiếu xuất bán buôn [' + targetData.MA_CHUNGTU + ']'; };
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

    app.controller('XuatBanPrint_Ctrl', ['$scope', '$http', 'configService', 'xuatBanService', 'tempDataService', '$filter', '$uibModal', '$log', 'thueService', 'donViTinhService', 'userService', 'nhaCungCapService', 'cuaHangService', 'khachHangService', 'kyKeToanService','$state', '$stateParams','$window',
        function ($scope, $http, configService, service, tempDataService, $filter, $uibModal, $log, thueService, donViTinhService, userService, nhaCungCapService, cuaHangService, khachHangService, kyKeToanService, $state, $stateParams, $window) {
            $scope.config = angular.copy(configService);
            $scope.tempData = tempDataService.tempData;
            $scope.currentUser = userService.GetCurrentUser();
            $scope.createDay = new Date();
            $scope.target = {};
          
            $scope.title = function () { return 'Duyệt phiếu xuất bán buôn [' + targetData.MA_CHUNGTU + ']'; };
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
            //Function load data catalog NhaCungCap
            function loadDataNhaCungCap() {
                $scope.nhaCungCap = [];
                if (!tempDataService.tempData('nhaCungCap')) {
                    nhaCungCapService.getAllData().then(function (successRes) {
                        if (successRes && successRes.status === 200 && successRes.data && successRes.data.Status && successRes.data.Data && successRes.data.Data.length > 0) {
                            tempDataService.putTempData('nhaCungCap', successRes.data.Data);
                            $scope.nhaCungCap = successRes.data.Data;
                        }
                    }, function (errorRes) {
                        console.log('errorRes', errorRes);
                    });
                } else {
                    $scope.nhaCungCap = tempDataService.tempData('nhaCungCap');
                }
            };
            loadDataNhaCungCap();
            //end

            //Function load data catalog KhoHang
            function loadDataKhachHang() {
                $scope.khachHang = [];
                if (!tempDataService.tempData('khachHang')) {
                    khachHangService.getAllData().then(function (successRes) {
                        if (successRes && successRes.status === 200 && successRes.data && successRes.data.Status && successRes.data.Data && successRes.data.Data.length > 0) {
                            tempDataService.putTempData('khachHang', successRes.data.Data);
                            $scope.khachHang = successRes.data.Data;
                        }
                    }, function (errorRes) {
                        console.log('errorRes', errorRes);
                    });
                } else {
                    $scope.khachHang = tempDataService.tempData('khachHang');
                }
            };
            loadDataKhachHang();
            //end

            //Function load data catalog NhaCungCap
            function loadDataCuaHang() {
                $scope.cuaHang = [];
                if (!tempDataService.tempData('cuaHang')) {
                    cuaHangService.getAllData().then(function (successRes) {
                        if (successRes && successRes.status === 200 && successRes.data && successRes.data.Status && successRes.data.Data && successRes.data.Data.length > 0) {
                            tempDataService.putTempData('cuaHang', successRes.data.Data);
                            $scope.cuaHang = successRes.data.Data;
                        }
                    }, function (errorRes) {
                        console.log('errorRes', errorRes);
                    });
                } else {
                    $scope.cuaHang = tempDataService.tempData('cuaHang');
                }
            };
            loadDataCuaHang();
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

            function TinhTongTienThue(listObj) {
                var total = 0;
                if (listObj && listObj.length > 0) {
                    angular.forEach(listObj, function (obj, idx) {
                        var increase = obj['THANHTIEN_VAT'];
                        if (!increase) {
                            increase = 0;
                        }
                        var minus = obj['THANHTIEN'];
                        if (!minus) {
                            minus = 0;
                        }
                        total += (increase - minus);
                    });
                }
                return total;
            };
            //Tính toán giá
            function getGiaTriVatRa(maThue) {
                var giaTri = 0;
                if (maThue) {
                    if ($scope.tempData('thue') && $scope.tempData('thue').length > 0) {
                        var dataTax = $filter('filter')($scope.tempData('thue'), { VALUE: maThue }, true);
                        if (dataTax && dataTax.length === 1) {
                            giaTri = dataTax[0].GIATRI;
                        }
                    }
                    else {
                        thueService.getDataByMaThue(maThue).then(function (successRes) {
                            if (successRes && successRes.status === 200 && successRes.data && successRes.data.Status && successRes.data.Data) {
                                giaTri = successRes.data.Data.GIATRI;
                            }
                        });
                    }
                }
                else {
                    giaTri = 0;
                }
                return giaTri;
            };
            //end
            function listenerDataDetails() {
                $scope.$watch("target.DataDetails", function (newValue, oldValue) {
                    $scope.target.TONGTIEN_TRUOCTHUE = TinhTongTien($scope.target.DataDetails, 'THANHTIEN_VAT');
                    $scope.target.TONG_TIENTHUE = TinhTongTienThue($scope.target.DataDetails);
                    $scope.target.TONGTIEN_SAUTHUE = TinhTongTien($scope.target.DataDetails, 'THANHTIEN');
                    $scope.target.TONG_SOLUONG = TinhTongTien($scope.target.DataDetails, 'SOLUONG');
                }, true);
            };
            listenerDataDetails();

            function filterData() {
                //GET PERIOD BY NGAY_CHUNGTU
                var obj = {
                    TUNGAY: $scope.config.moment($stateParams.postParam.TUNGAY).format(),
                    DENNGAY: $scope.config.moment($stateParams.postParam.DENNGAY).format()
                };
                kyKeToanService.getKyKeToanTheoNgay(obj).then(function (successRes) {
                    if (successRes && successRes.status == 200 && successRes.data && successRes.data.Status && successRes.data.Data) {
                        $scope.target.TABLE_NAME = 'XNT_' + successRes.data.Data.NAM + '_KY_' + successRes.data.Data.KY;
                        service.getDetails($stateParams.postParam.ID, $scope.target.TABLE_NAME).then(function (sucessRes) {
                            if (sucessRes && sucessRes.status === 200 && sucessRes.data && sucessRes.data.Status && sucessRes.data.Data) {
                                $scope.target = sucessRes.data.Data;
                                $scope.target.NGAY_CHUNGTU = new Date($scope.target.NGAY_CHUNGTU);
                                if ($scope.target.DataDetails && $scope.target.DataDetails.length > 0) {
                                    angular.forEach($scope.target.DataDetails, function (value, idx) {
                                        value.THANHTIEN_VAT = Math.round(100 * (value.THANHTIEN * (1 + (getGiaTriVatRa(value.MATHUE_RA)) / 100))) / 100;
                                        value.GIAVON_VAT = Math.round(100 * (value.GIAVON * (1 + (getGiaTriVatRa(value.MATHUE_RA)) / 100))) / 100;
                                    });
                                }
                                //get info KHACHHANG BY CHUNGTU
                                $scope.thongTinKhachHang = {};
                                var khachHang = $filter('filter')($scope.tempData('khachHang'), { VALUE: $scope.target.MAKHACHHANG }, true);
                                if (khachHang && khachHang.length === 1) {
                                    $scope.thongTinKhachHang = khachHang[0];
                                }
                                //end
                            }
                        });
                    }
                });
                //END
            };
            filterData();

            $scope.print = function () {
                var table = document.getElementById('main_report').innerHTML;
                var myWindow = $window.open('', '', 'width=800, height=600');
                myWindow.document.write(table);
                myWindow.print();
            };

            $scope.printExcel = function () {
                var data = [document.getElementById('main_report').innerHTML];
                var fileName = "PhieuXuatBan_" + $scope.target.MA_CHUNGTU + ".xls";
                var filetype = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet;charset=utf-8";
                var ieEDGE = navigator.userAgent.match(/Edge/g);
                var ie = navigator.userAgent.match(/.NET/g); // IE 11+
                var oldIE = navigator.userAgent.match(/MSIE/g);
                if (ie || oldIE || ieEDGE) {
                    var blob = new window.Blob(data, { type: filetype });
                    window.navigator.msSaveBlob(blob, fileName);
                }
                else {
                    var a = $("<a style='display: none;'/>");
                    var url = window.webkitURL.createObjectURL(new Blob(data, { type: filetype }));
                    a.attr("href", url);
                    a.attr("download", fileName);
                    $("body").append(a);
                    a[0].click();
                    window.url.revokeObjectURL(url);
                    a.remove();
                }
            };
            $scope.cancel = function () {
                $state.go('XuatBan');
            }
        }]);
    return app;
});