﻿define(['ui-bootstrap', 'controllers/catalog/khoHangController', 'controllers/catalog/loaiHangController', 'controllers/catalog/nhomHangController', 'controllers/catalog/nhaCungCapController', 'controllers/catalog/matHangController', 'controllers/authorize/cuaHangController', 'controllers/authorize/authController', 'controllers/authorize/kyKeToanController', 'controllers/knowledge/xuatBanLeThuNganController'], function () {
    'use strict';
    var app = angular.module('xuatBanLeReportModule', ['ui.bootstrap', 'khoHangModule', 'loaiHangModule', 'nhomHangModule', 'nhaCungCapModule', 'matHangModule', 'cuaHangModule', 'kyKeToanModule', 'xuatBanLeThuNganModule']);
    app.factory('xuatBanLeReportService', ['$http', 'configService', function ($http, configService) {
        var serviceUrl = configService.rootUrlWebApi + '/Report/XuatBanLe';
        var selectedData = [];
        var result = {
            getLastestPeriod: function () {
                return $http.get(serviceUrl + '/GetLastestPeriod');
            }
        }
        return result;
    }]);
    /* controller list */
    app.controller('BaoCaoXuatBanLe_Ctrl', ['$scope', '$http', 'configService', 'xuatBanLeReportService', 'tempDataService', '$filter', '$uibModal', '$log', 'securityService', 'khoHangService', 'loaiHangService', 'nhomHangService', 'nhaCungCapService', 'matHangService', 'cuaHangService', 'userService', 'kyKeToanService', 'xuatBanLeThuNganService',
        function ($scope, $http, configService, service, tempDataService, $filter, $uibModal, $log, securityService, khoHangService, loaiHangService, nhomHangService, nhaCungCapService, matHangService, cuaHangService, userService, kyKeToanService, xuatBanLeThuNganService) {
            $scope.config = angular.copy(configService);
            $scope.filtered = angular.copy(configService.filterDefault);
            $scope.tempData = tempDataService.tempData;
            var currentUser = userService.GetCurrentUser();
            $scope.title = function () { return 'Báo cáo xuất bán lẻ' };
            $scope.target = {
                MA_BAOCAO: '',
                MA_CUAHANG: currentUser.unitCode,
                TEN_CUAHANG: '',
                UNITCODE: '',
                USERNAME: '',
                FULLNAME: ''
            };
            //mặc định điều kiện nhóm
            $scope.target.DIEUKIEN_NHOM = 'KHOHANG';
            //end
            $scope.target.MA_BAOCAO = "BAOCAO_XUATBANLE";
            $scope.target.UNITCODE = currentUser.unitCode;
            $scope.target.USERNAME = currentUser.userName;
            $scope.target.FULLNAME = currentUser.fullName;
            function filterData() {
              
            };
            //check authorize
            function loadAccessList() {
                var userName = currentUser.userName;
                var unitCodeParam = !currentUser.parentUnitCode ? currentUser.unitCode : currentUser.parentUnitCode;
                securityService.getAccessList('BaoCaoXuatBanLe', userName, unitCodeParam).then(function (successRes) {
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

            //Function load data catalog LoaiHang
            function loadDataLoaiHang() {
                $scope.loaiHang = [];
                if (!tempDataService.tempData('loaiHang')) {
                    loaiHangService.getAllData().then(function (successRes) {
                        if (successRes && successRes.status === 200 && successRes.data && successRes.data.Status && successRes.data.Data && successRes.data.Data.length > 0) {
                            tempDataService.putTempData('loaiHang', successRes.data.Data);
                            $scope.loaiHang = successRes.data.Data;
                        }
                    }, function (errorRes) {
                        console.log('errorRes', errorRes);
                    });
                } else {
                    $scope.loaiHang = tempDataService.tempData('loaiHang');
                }
            };
            loadDataLoaiHang();
            //end

            //Function load data catalog NhomHang
            function loadDataNhomHang() {
                $scope.nhomHang = [];
                if (!tempDataService.tempData('nhomHang')) {
                    nhomHangService.getAllData().then(function (successRes) {
                        if (successRes && successRes.status === 200 && successRes.data && successRes.data.Status && successRes.data.Data && successRes.data.Data.length > 0) {
                            tempDataService.putTempData('nhomHang', successRes.data.Data);
                            $scope.nhomHang = successRes.data.Data;
                        }
                    }, function (errorRes) {
                        console.log('errorRes', errorRes);
                    });
                } else {
                    $scope.nhomHang = tempDataService.tempData('nhomHang');
                }
            };
            loadDataNhomHang();
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

            //Function load data catalog MatHang
            function loadDataMatHang() {
                $scope.matHang = [];
                if (!tempDataService.tempData('matHang')) {
                    matHangService.getAllData().then(function (successRes) {
                        if (successRes && successRes.status === 200 && successRes.data && successRes.data.Status && successRes.data.Data && successRes.data.Data.length > 0) {
                            tempDataService.putTempData('matHang', successRes.data.Data);
                            $scope.matHang = successRes.data.Data;
                        }
                    }, function (errorRes) {
                        console.log('errorRes', errorRes);
                    });
                } else {
                    $scope.matHang = tempDataService.tempData('matHang');
                }
            };
            loadDataMatHang();
            //end

            //Function load data catalog MatHang
            function loadDataGiaoDich() {
                $scope.giaoDich = [];
                if (!tempDataService.tempData('giaoDich')) {
                    xuatBanLeThuNganService.getAllData().then(function (successRes) {
                        if (successRes && successRes.status === 200 && successRes.data && successRes.data.Status && successRes.data.Data && successRes.data.Data.length > 0) {
                            tempDataService.putTempData('giaoDich', successRes.data.Data);
                            $scope.giaoDich = successRes.data.Data;
                        }
                    }, function (errorRes) {
                        console.log('errorRes', errorRes);
                    });
                } else {
                    $scope.giaoDich = tempDataService.tempData('giaoDich');
                }
            };
            loadDataGiaoDich();
            //end

            //Function load data catalog CuaHang
            function loadDataCuaHang() {
                $scope.cuaHang = [];
                if (!tempDataService.tempData('cuaHang')) {
                    cuaHangService.getAllDataByUniCode(currentUser.unitCode).then(function (successRes) {
                        if (successRes && successRes.status === 200 && successRes.data && successRes.data.Status && successRes.data.Data && successRes.data.Data.length > 0) {
                            tempDataService.putTempData('cuaHang', successRes.data.Data);
                            $scope.cuaHang = successRes.data.Data;
                            if ($scope.cuaHang && $scope.cuaHang.length === 1) {
                                $scope.target.TEN_CUAHANG = $scope.cuaHang[0].DESCRIPTION;
                            } else {
                                var filterStore = $filter('filter')($scope.cuaHang, { VALUE: $scope.target.MA_CUAHANG }, true);
                                if (filterStore && filterStore.length === 1) {
                                    $scope.target.TEN_CUAHANG = filterStore[0].DESCRIPTION;
                                }
                            }
                        }
                    }, function (errorRes) {
                        console.log('errorRes', errorRes);
                    });
                } else {
                    $scope.cuaHang = tempDataService.tempData('cuaHang');
                    if ($scope.cuaHang && $scope.cuaHang.length === 1) {
                        $scope.target.TEN_CUAHANG = $scope.cuaHang[0].DESCRIPTION;
                    } else {
                        var filterStore = $filter('filter')($scope.cuaHang, { VALUE: $scope.target.MA_CUAHANG }, true);
                        if (filterStore && filterStore.length === 1) {
                            $scope.target.TEN_CUAHANG = filterStore[0].DESCRIPTION;
                        }
                    }
                }

            };
            loadDataCuaHang();
            //end

            //khởi tạo thông tin cửa hàng
            $scope.changeCuaHang = function (maCuaHang) {
                if (maCuaHang) {
                    var filterStore = $filter('filter')($scope.cuaHang, { VALUE: maCuaHang }, true);
                    if (filterStore && filterStore.length === 1) {
                        $scope.target.TEN_CUAHANG = filterStore[0].DESCRIPTION;
                    }
                }
            };
            //end

            //get lastest KyKeTona
            kyKeToanService.getLastestPeriod().then(function (refundData) {
                if (refundData && refundData.status === 200 && refundData.data && refundData.data.Status && refundData.data.Data) {
                    $scope.target.TUNGAY = new Date(refundData.data.Data.NGAYKETOAN);
                    $scope.target.DENNGAY = new Date(refundData.data.Data.NGAYKETOAN);
                    $scope.target.TABLE_NAME = refundData.data.Data.TABLE_NAME;
                }
            });
            //end 

            //search by Mã giao dịch
            $scope.selectedMaGiaoDich = function () {
                var modalInstance = $uibModal.open({
                    backdrop: 'static',
                    templateUrl: configService.buildUrl('knowledge/XuatBanLeThuNgan', 'search'),
                    controller: 'xuatBanLeThuNganSearch_Ctrl',
                    windowClass: 'search-giaodich-window',
                    resolve: {
                        filterObject: function () {
                            return {
                                ISSELECT_POST: $scope.target.MA_GIAODICH,
                            };
                        }
                    }
                });
                modalInstance.result.then(function (updatedData) {
                    $scope.target.MA_GIAODICH = [];
                    if (updatedData && updatedData.length > 0) {
                        angular.forEach(updatedData, function (v, k) {
                            var obj = {
                                VALUE: v.MA_GIAODICH,
                                TEXT: v.MA_GIAODICH,
                                DESCRIPTION: v.MA_GIAODICH,
                                ID: v.ID
                            };
                            $scope.target.MA_GIAODICH.push(obj);
                        });
                    }
                }, function () {
                    $log.info('Modal dismissed at: ' + new Date());
                });
            }
            //end search by Mã giao dịch

            //search by warehouse
            $scope.selectedKhoHang = function () {
                var modalInstance = $uibModal.open({
                    backdrop: 'static',
                    templateUrl: configService.buildUrl('catalog/KhoHang', 'search'),
                    controller: 'khoHangSearch_Ctrl',
                    windowClass: 'search-window',
                    resolve: {
                        filterObject: function () {
                            return {
                                ISSELECT_POST: $scope.target.MAKHO,
                            };
                        }
                    }
                });
                modalInstance.result.then(function (updatedData) {
                    $scope.target.MAKHO = [];
                    if (updatedData && updatedData.length > 0) {
                        angular.forEach(updatedData, function (v, k) {
                            var obj = {
                                VALUE: v.MAKHO,
                                TEXT: v.MAKHO + ' | ' + v.TENKHO,
                                DESCRIPTION: v.TENKHO,
                                ID: v.ID
                            };
                            $scope.target.MAKHO.push(obj);
                        });
                    }
                }, function () {
                    $log.info('Modal dismissed at: ' + new Date());
                });
            }
            //end search by warehouse

            //search by type merchandise
            $scope.selectedLoaiHang = function () {
                var modalInstance = $uibModal.open({
                    backdrop: 'static',
                    templateUrl: configService.buildUrl('catalog/LoaiHang', 'search'),
                    controller: 'loaiHangSearch_Ctrl',
                    windowClass: 'search-window',
                    resolve: {
                        filterObject: function () {
                            return {
                                ISSELECT_POST: $scope.target.MALOAI,
                            };
                        }
                    }
                });
                modalInstance.result.then(function (updatedData) {
                    $scope.target.MALOAI = [];
                    if (updatedData && updatedData.length > 0) {
                        angular.forEach(updatedData, function (v, k) {
                            var obj = {
                                VALUE: v.MALOAI,
                                TEXT: v.MALOAI + ' | ' + v.TENLOAI,
                                DESCRIPTION: v.TENLOAI,
                                ID: v.ID
                            };
                            $scope.target.MALOAI.push(obj);
                        });
                    }
                }, function () {
                    $log.info('Modal dismissed at: ' + new Date());
                });
            }
            //end search by type merchandise


            //search by group merchandise
            $scope.selectedNhomHang = function () {
                var modalInstance = $uibModal.open({
                    backdrop: 'static',
                    templateUrl: configService.buildUrl('catalog/NhomHang', 'search'),
                    controller: 'nhomHangSearch_Ctrl',
                    windowClass: 'search-window',
                    resolve: {
                        filterObject: function () {
                            return {
                                ISSELECT_POST: $scope.target.MANHOM,
                            };
                        }
                    }
                });
                modalInstance.result.then(function (updatedData) {
                    $scope.target.MANHOM = [];
                    if (updatedData && updatedData.length > 0) {
                        angular.forEach(updatedData, function (v, k) {
                            var obj = {
                                VALUE: v.MANHOM,
                                TEXT: v.MANHOM + ' | ' + v.TENNHOM,
                                DESCRIPTION: v.TENNHOM,
                                ID: v.ID
                            };
                            $scope.target.MANHOM.push(obj);
                        });
                    }
                }, function () {
                    $log.info('Modal dismissed at: ' + new Date());
                });
            }
            //end search by group merchandise

            //search by supplier
            $scope.selectedNhaCungCap = function () {
                var modalInstance = $uibModal.open({
                    backdrop: 'static',
                    templateUrl: configService.buildUrl('catalog/NhaCungCap', 'search'),
                    controller: 'nhaCungCapSearch_Ctrl',
                    windowClass: 'search-window-nhacungcap',
                    resolve: {
                        filterObject: function () {
                            return {
                                ISSELECT_POST: $scope.target.MANHACUNGCAP,
                            };
                        }
                    }
                });
                modalInstance.result.then(function (updatedData) {
                    $scope.target.MANHACUNGCAP = [];
                    if (updatedData && updatedData.length > 0) {
                        angular.forEach(updatedData, function (v, k) {
                            var obj = {
                                VALUE: v.MANHACUNGCAP,
                                TEXT: v.MANHACUNGCAP + ' | ' + v.TENNHACUNGCAP,
                                DESCRIPTION: v.TENNHACUNGCAP,
                                ID: v.ID
                            };
                            $scope.target.MANHACUNGCAP.push(obj);
                        });
                    }
                }, function () {
                    $log.info('Modal dismissed at: ' + new Date());
                });
            }
            //end search by supplier

            //search by merchandise
            $scope.selectedMatHang = function () {
                var modalInstance = $uibModal.open({
                    backdrop: 'static',
                    templateUrl: configService.buildUrl('catalog/MatHang', 'search'),
                    controller: 'matHangSearch_Ctrl',
                    windowClass: 'search-window-mathang',
                    resolve: {
                        filterObject: function () {
                            return {
                                ISSELECT_POST: $scope.target.MAHANG,
                            };
                        }
                    }
                });
                modalInstance.result.then(function (updatedData) {
                    $scope.target.MAHANG = [];
                    if (updatedData && updatedData.length > 0) {
                        angular.forEach(updatedData, function (v, k) {
                            var obj = {
                                VALUE: v.MAHANG,
                                TEXT: v.MAHANG + ' | ' + v.TENHANG,
                                DESCRIPTION: v.TENHANG,
                                ID: v.ID
                            };
                            $scope.target.MAHANG.push(obj);
                        });
                    }
                }, function () {
                    $log.info('Modal dismissed at: ' + new Date());
                });
            }
            //end search by merchandise

            //tags input KhoHang
            $scope.loadKhoHangTags = function ($query) {
                var kho = $filter('filter')($scope.khoHang, { TEXT: $query.toUpperCase() }, false);
                if (kho && kho.length == 0) {
                    kho = $filter('filter')($scope.khoHang, { TEXT: $query }, false);
                }
                return kho;
            };
            //end 

            //tags input LoaiHang
            $scope.loadLoaiHangTags = function ($query) {
                var loai = $filter('filter')($scope.loaiHang, { TEXT: $query.toUpperCase() }, false);
                return loai;
            };
            //end 

            //tags input NhomHang
            $scope.loadNhomHangTags = function ($query) {
                var nhom = $filter('filter')($scope.nhomHang, { TEXT: $query.toUpperCase() }, false);
                return nhom;
            };
            //end 

            //tags input NhaCungCap
            $scope.loadNhaCungCapTags = function ($query) {
                var nhaCungCap = $filter('filter')($scope.nhaCungCap, { TEXT: $query.toUpperCase() }, false);
                return nhaCungCap;
            };
            //end 

            //tags input MatHang
            $scope.loadMatHangTags = function ($query) {
                var matHang = $filter('filter')($scope.matHang, { TEXT: $query.toUpperCase() }, false);
                return matHang;
            };
            //end 

            //tags input GiaoDich
            $scope.loadGiaoDichTags = function ($query) {
                var giaoDich = $filter('filter')($scope.giaoDich, { TEXT: $query.toUpperCase() }, false);
                return giaoDich;
            };
            //end 
            
            $scope.reportInDay = function () {
                if (!$scope.target.TEN_CUAHANG || $scope.target.TEN_CUAHANG == "") {
                    Lobibox.notify('error', {
                        title: 'Thiếu dữ liệu',
                        msg: 'Thiếu thông tin cửa hàng',
                        delay: 3000
                    });
                } else if (!$scope.target.TUNGAY || $scope.target.TUNGAY == "") {
                    Lobibox.notify('error', {
                        title: 'Thiếu dữ liệu',
                        msg: 'Thiếu dữ liệu từ ngày',
                        delay: 3000
                    });
                } else if (!$scope.target.DENNGAY || $scope.target.DENNGAY == "") {
                    Lobibox.notify('error', {
                        title: 'Thiếu dữ liệu',
                        msg: 'Thiếu dữ liệu đến ngày',
                        delay: 3000
                    });
                } else {
                    $scope.target.TUNGAY = $scope.config.moment($scope.target.TUNGAY).format();
                    $scope.target.DENNGAY = $scope.config.moment($scope.target.DENNGAY).format();
                    var modalInstance = $uibModal.open({
                        backdrop: 'static',
                        windowClass: 'reportTongHop-window',
                        keyboard: false,
                        templateUrl: configService.buildUrl('report/Template', 'reports.template'),
                        controller: 'XuatBanLeTrongNgayReport_Ctrl',
                        resolve: {
                            objParam: function () {
                                return $scope.target;
                            }
                        }
                    });
                    modalInstance.result.then(function (refundedData) {
                        if (refundedData) {
                            $scope.target.TUNGAY = new Date(refundedData.TUNGAY);
                            $scope.target.DENNGAY = new Date(refundedData.DENNGAY);
                        }
                    });
                }
            };

            $scope.report = function () {
                if (!$scope.target.TEN_CUAHANG || $scope.target.TEN_CUAHANG == "") {
                    Lobibox.notify('error', {
                        title: 'Thiếu dữ liệu',
                        msg: 'Thiếu thông tin cửa hàng',
                        delay: 3000
                    });
                } else if (!$scope.target.TUNGAY || $scope.target.TUNGAY == "") {
                    Lobibox.notify('error', {
                        title: 'Thiếu dữ liệu',
                        msg: 'Thiếu dữ liệu từ ngày',
                        delay: 3000
                    });
                } else if (!$scope.target.DENNGAY || $scope.target.DENNGAY == "") {
                    Lobibox.notify('error', {
                        title: 'Thiếu dữ liệu',
                        msg: 'Thiếu dữ liệu đến ngày',
                        delay: 3000
                    });
                } else {
                    $scope.target.TUNGAY = $scope.config.moment($scope.target.TUNGAY).format();
                    $scope.target.DENNGAY = $scope.config.moment($scope.target.DENNGAY).format();
                    var modalInstance = $uibModal.open({
                        backdrop: 'static',
                        windowClass: 'reportTongHop-window',
                        keyboard: false,
                        templateUrl: configService.buildUrl('report/Template', 'reports.template'),
                        controller: 'XuatBanLeReport_Ctrl',
                        resolve: {
                            objParam: function () {
                                return $scope.target;
                            }
                        }
                    });
                    modalInstance.result.then(function (refundedData) {
                        if (refundedData) {
                            $scope.target.TUNGAY = new Date(refundedData.TUNGAY);
                            $scope.target.DENNGAY = new Date(refundedData.DENNGAY);
                        }
                    });
                }
            };

            $scope.reportDetail = function () {
                if (!$scope.target.TEN_CUAHANG || $scope.target.TEN_CUAHANG == "") {
                    Lobibox.notify('error', {
                        title: 'Thiếu dữ liệu',
                        msg: 'Thiếu thông tin cửa hàng',
                        delay: 3000
                    });
                } else if (!$scope.target.TUNGAY || $scope.target.TUNGAY == "") {
                    Lobibox.notify('error', {
                        title: 'Thiếu dữ liệu',
                        msg: 'Thiếu dữ liệu từ ngày',
                        delay: 3000
                    });
                } else if (!$scope.target.DENNGAY || $scope.target.DENNGAY == "") {
                    Lobibox.notify('error', {
                        title: 'Thiếu dữ liệu',
                        msg: 'Thiếu dữ liệu đến ngày',
                        delay: 3000
                    });
                } else {
                    $scope.target.TUNGAY = $scope.config.moment($scope.target.TUNGAY).format();
                    $scope.target.DENNGAY = $scope.config.moment($scope.target.DENNGAY).format();
                    var modalInstance = $uibModal.open({
                        backdrop: 'static',
                        windowClass: 'reportChiTiet-window',
                        keyboard: false,
                        templateUrl: configService.buildUrl('report/Template', 'reports.template'),
                        controller: 'XuatBanLeReportDetail_Ctrl',
                        resolve: {
                            objParam: function () {
                                return $scope.target;
                            }
                        }
                    });
                    modalInstance.result.then(function (refundedData) {
                        if (refundedData) {
                            $scope.target.TUNGAY = new Date(refundedData.TUNGAY);
                            $scope.target.DENNGAY = new Date(refundedData.DENNGAY);
                        }
                    });
                }
            };
            $scope.cancel = function () {
                window.location.href = "#!/home";
            };
        }]);

    
    app.controller('XuatBanLeTrongNgayReport_Ctrl', ['$scope', '$http', 'configService', 'objParam', '$uibModalInstance', '$filter', 'kyKeToanService',
        function ($scope, $http, configService, objParam, $uibModalInstance, $filter, kyKeToanService) {
            $scope.config = angular.copy(configService);
            function convertArrayToString(arr) {
                var result = '';
                if (arr && arr.length > 0) {
                    angular.forEach(arr, function (v, k) {
                        result += v.VALUE + ',';
                    });
                    if (result && result.length > 0) result = result.substring(0, result.length - 1);
                } else {
                    result = '';
                }
                return result;
            };

            var paramsReport = angular.copy(objParam);
            paramsReport.TUNGAY = new Date(paramsReport.TUNGAY);
            paramsReport.DENNGAY = new Date(paramsReport.DENNGAY);
            paramsReport.TUNGAY = $scope.config.moment(paramsReport.TUNGAY).format();
            paramsReport.DENNGAY = $scope.config.moment(paramsReport.DENNGAY).format();
            if (paramsReport.MAKHO && paramsReport.MAKHO.length > 0)
                paramsReport.MAKHO = convertArrayToString(paramsReport.MAKHO);
            else paramsReport.MAKHO = '';

            if (paramsReport.MALOAI && paramsReport.MALOAI.length > 0)
                paramsReport.MALOAI = convertArrayToString(paramsReport.MALOAI);
            else paramsReport.MALOAI = '';

            if (paramsReport.MANHOM && paramsReport.MANHOM.length > 0)
                paramsReport.MANHOM = convertArrayToString(paramsReport.MANHOM);
            else paramsReport.MANHOM = '';

            if (paramsReport.MANHACUNGCAP && paramsReport.MANHACUNGCAP.length > 0)
                paramsReport.MANHACUNGCAP = convertArrayToString(paramsReport.MANHACUNGCAP);
            else paramsReport.MANHACUNGCAP = '';

            if (paramsReport.MAHANG && paramsReport.MAHANG.length > 0)
                paramsReport.MAHANG = convertArrayToString(paramsReport.MAHANG);
            else paramsReport.MAHANG = '';

            if (paramsReport.MA_GIAODICH && paramsReport.MA_GIAODICH.length > 0)
                paramsReport.MA_GIAODICH = convertArrayToString(paramsReport.MA_GIAODICH);
            else paramsReport.MA_GIAODICH = '';
            $scope.report = {
                name: "ERBus.Api.Reports.XuatBanLe.XUATBANLE_TRONGNGAY,ERBus.Api",
                title: "Báo cáo xuất bán lẻ tổng hợp",
                params: paramsReport
            };
            $scope.cancel = function () {
                $uibModalInstance.close(paramsReport);
            };
        }]);

    app.controller('XuatBanLeReport_Ctrl', ['$scope', '$http', 'configService', 'objParam','$uibModalInstance','$filter','kyKeToanService',
        function ($scope, $http, configService, objParam, $uibModalInstance, $filter, kyKeToanService) {
            $scope.config = angular.copy(configService);
            function convertArrayToString(arr) {
                var result = '';
                if (arr && arr.length > 0) {
                    angular.forEach(arr, function (v, k) {
                        result += v.VALUE + ',';
                    });
                    if (result && result.length > 0) result = result.substring(0, result.length - 1);
                } else {
                    result = '';
                }
                return result;
            };
           
            var paramsReport = angular.copy(objParam);
            paramsReport.TUNGAY = new Date(paramsReport.TUNGAY);
            paramsReport.DENNGAY = new Date(paramsReport.DENNGAY);
            paramsReport.TUNGAY = $scope.config.moment(paramsReport.TUNGAY).format();
            paramsReport.DENNGAY = $scope.config.moment(paramsReport.DENNGAY).format();
            if (paramsReport.MAKHO && paramsReport.MAKHO.length > 0)
                paramsReport.MAKHO = convertArrayToString(paramsReport.MAKHO);
            else paramsReport.MAKHO = '';

            if (paramsReport.MALOAI && paramsReport.MALOAI.length > 0)
                paramsReport.MALOAI = convertArrayToString(paramsReport.MALOAI);
            else paramsReport.MALOAI = '';

            if (paramsReport.MANHOM && paramsReport.MANHOM.length > 0)
                paramsReport.MANHOM = convertArrayToString(paramsReport.MANHOM);
            else paramsReport.MANHOM = '';

            if (paramsReport.MANHACUNGCAP && paramsReport.MANHACUNGCAP.length > 0)
                paramsReport.MANHACUNGCAP = convertArrayToString(paramsReport.MANHACUNGCAP);
            else paramsReport.MANHACUNGCAP = '';

            if (paramsReport.MAHANG && paramsReport.MAHANG.length > 0)
                paramsReport.MAHANG = convertArrayToString(paramsReport.MAHANG);
            else paramsReport.MAHANG = '';

            if (paramsReport.MA_GIAODICH && paramsReport.MA_GIAODICH.length > 0)
                paramsReport.MA_GIAODICH = convertArrayToString(paramsReport.MA_GIAODICH);
            else paramsReport.MA_GIAODICH = '';
            $scope.report = {
                name: "ERBus.Api.Reports.XuatBanLe.XBANLE_TONGHOP,ERBus.Api",
                title: "Báo cáo xuất bán lẻ tổng hợp",
                params: paramsReport
            };
            $scope.cancel = function () {
                $uibModalInstance.close(paramsReport);
            };
        }]);

    app.controller('XuatBanLeReportDetail_Ctrl', ['$scope', '$http', 'configService', 'objParam', '$uibModalInstance', '$filter','kyKeToanService',
        function ($scope, $http, configService, objParam, $uibModalInstance, $filter, kyKeToanService) {
            $scope.config = angular.copy(configService);
            function convertArrayToString(arr) {
                var result = '';
                if (arr && arr.length > 0) {
                    angular.forEach(arr, function (v, k) {
                        result += v.VALUE + ',';
                    });
                    if (result && result.length > 0) result = result.substring(0, result.length - 1);
                } else {
                    result = '';
                }
                return result;
            };
            
            var paramsReport = angular.copy(objParam);
            paramsReport.TUNGAY = new Date(paramsReport.TUNGAY);
            paramsReport.DENNGAY = new Date(paramsReport.DENNGAY);
            paramsReport.TUNGAY = $scope.config.moment(paramsReport.TUNGAY).format();
            paramsReport.DENNGAY = $scope.config.moment(paramsReport.DENNGAY).format();
            if (paramsReport.MAKHO && paramsReport.MAKHO.length > 0)
                paramsReport.MAKHO = convertArrayToString(paramsReport.MAKHO);
            else paramsReport.MAKHO = '';

            if (paramsReport.MALOAI && paramsReport.MALOAI.length > 0)
                paramsReport.MALOAI = convertArrayToString(paramsReport.MALOAI);
            else paramsReport.MALOAI = '';

            if (paramsReport.MANHOM && paramsReport.MANHOM.length > 0)
                paramsReport.MANHOM = convertArrayToString(paramsReport.MANHOM);
            else paramsReport.MANHOM = '';

            if (paramsReport.MANHACUNGCAP && paramsReport.MANHACUNGCAP.length > 0)
                paramsReport.MANHACUNGCAP = convertArrayToString(paramsReport.MANHACUNGCAP);
            else paramsReport.MANHACUNGCAP = '';

            if (paramsReport.MAHANG && paramsReport.MAHANG.length > 0)
                paramsReport.MAHANG = convertArrayToString(paramsReport.MAHANG);
            else paramsReport.MAHANG = '';

            if (paramsReport.MA_GIAODICH && paramsReport.MA_GIAODICH.length > 0)
                paramsReport.MA_GIAODICH = convertArrayToString(paramsReport.MA_GIAODICH);
            else paramsReport.MA_GIAODICH = '';
            $scope.report = {
                name: "ERBus.Api.Reports.XuatBanLe.XBANLE_CHITIET,ERBus.Api",
                title: "Báo cáo xuất bán lẻ chi tiết",
                params: paramsReport
            };
            $scope.cancel = function () {
                $uibModalInstance.close(paramsReport);
            };
        }]);
    return app;
});