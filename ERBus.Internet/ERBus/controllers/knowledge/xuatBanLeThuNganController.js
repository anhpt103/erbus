define(['ui-bootstrap', 'controllers/catalog/khachHangController', 'controllers/catalog/donViTinhController', 'controllers/catalog/thueController', 'controllers/authorize/thamSoHeThongController', 'controllers/catalog/khoHangController', 'controllers/catalog/matHangController'], function () {
    'use strict';
    var app = angular.module('xuatBanLeThuNganModule', ['ui.bootstrap', 'khachHangModule', 'donViTinhModule', 'thueModule', 'thamSoHeThongModule', 'khoHangModule', 'matHangModule']);
    app.factory('xuatBanLeThuNganService', ['$http', 'configService', function ($http, configService) {
        var serviceUrl = configService.rootUrlWebApi + '/Knowledge/XuatBanLeThuNgan';
        var result = {
            postQuery: function (data) {
                return $http.post(serviceUrl + '/PostQuery', data);
            },
            update: function (params) {
                return $http.put(serviceUrl + '/' + params.ID, params);
            },
            getDetails: function (ID) {
                return $http.get(serviceUrl + '/GetDetails/' + ID);
            },
            getAllData: function () {
                return $http.get(serviceUrl + '/GetAllData');
            }
        }
        return result;
    }]);
    /* controller list */
    app.controller('XuatBanLeThuNgan_Ctrl', ['$scope', '$http', 'configService', 'xuatBanLeThuNganService', 'tempDataService', '$filter', '$uibModal', '$log', 'securityService', 'khachHangService', 'thueService', 'khoHangService','donViTinhService',
        function ($scope, $http, configService, service, tempDataService, $filter, $uibModal, $log, securityService, khachHangService, thueService, khoHangService, donViTinhService) {
            $scope.config = angular.copy(configService);
            $scope.paged = angular.copy(configService.pageDefault);
            $scope.filtered = angular.copy(configService.filterDefault);
            $scope.tempData = tempDataService.tempData;
            $scope.title = function () { return 'Xuất bán lẻ thu ngân' };
            $scope.data = [];
            $scope.setPage = function (pageNo) {
                $scope.paged.CurrentPage = pageNo;
                filterData();
            };
            $scope.sortType = 'MA_GIAODICH';
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
                securityService.getAccessList('XuatBanLeThuNgan').then(function (successRes) {
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
           
            /* Function details Item */
            $scope.detail = function (target) {
                var modalInstance = $uibModal.open({
                    backdrop: 'static',
                    animation: true,
                    windowClass: 'knowledge-window',
                    templateUrl: configService.buildUrl('knowledge/XuatBanLeThuNgan', 'detail'),
                    controller: 'xuatBanLeThuNganDetail_Ctrl',
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
                    templateUrl: configService.buildUrl('knowledge/XuatBanLeThuNgan', 'edit'),
                    controller: 'xuatBanLeThuNganEdit_Ctrl',
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

    app.controller('xuatBanLeThuNganDetail_Ctrl', ['$scope', '$uibModalInstance', '$http', 'configService', 'xuatBanLeThuNganService', 'targetData', 'tempDataService', '$filter', '$uibModal', '$log', 'thueService',
        function ($scope, $uibModalInstance, $http, configService, service, targetData, tempDataService, $filter, $uibModal, $log, thueService) {
            $scope.config = angular.copy(configService);
            $scope.paged = angular.copy(configService.pageDefault);
            $scope.tempData = tempDataService.tempData;
            $scope.target = {};
            $scope.target = angular.copy(targetData);
            $scope.title = function () { return 'Thông tin hóa đơn bán lẻ [' + targetData.MA_GIAODICH + ']'; };
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
           
            function listenerDataDetails() {
                $scope.$watch("target.DataDetails", function (newValue, oldValue) {
                    $scope.target.TONGTIEN = TinhTongTien($scope.target.DataDetails, 'THANHTIEN_CHUAGIAM');
                    $scope.target.TONGTIEN_SAUGIAM = TinhTongTien($scope.target.DataDetails, 'THANHTIEN');
                    $scope.target.TONG_TIEN_CHIETKHAU = TinhTongTien($scope.target.DataDetails, 'TIEN_CHIETKHAU');
                    $scope.target.TONG_TIENTHE_VIP = TinhTongTien($scope.target.DataDetails, 'TIENTHE_VIP');
                    $scope.target.TONG_TIEN_KHUYENMAI = TinhTongTien($scope.target.DataDetails, 'TIEN_KHUYENMAI');
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
                service.getDetails($scope.target.ID).then(function (sucessRes) {
                    if (sucessRes && sucessRes.status === 200 && sucessRes.data && sucessRes.data.Status && sucessRes.data.Data) {
                        $scope.target = sucessRes.data.Data;
                        $scope.target.NGAY_GIAODICH = new Date($scope.target.NGAY_GIAODICH);
                        if ($scope.target.DataDetails && $scope.target.DataDetails.length > 0) {
                            angular.forEach($scope.target.DataDetails, function (v, k) {
                                v.THANHTIEN_CHUAGIAM = v.SOLUONG * v.GIABANLE_VAT;
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

    app.controller('xuatBanLeThuNganEdit_Ctrl', ['$scope', '$uibModalInstance', '$http', 'configService', 'xuatBanLeThuNganService', 'targetData', 'tempDataService', '$filter', '$uibModal', '$log', 'thueService', 'matHangService', 'userService',
        function ($scope, $uibModalInstance, $http, configService, service, targetData, tempDataService, $filter, $uibModal, $log, thueService, matHangService, userService) {
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
                MATHUE_VAO: '',
                MATHUE_RA: '',
                TYLE_LAILE: 0,
                SOLUONG: 1,
                GIAMUA: 0,
                GIAMUA_VAT: 0,
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
            $scope.title = function () { return 'Chỉnh sửa hóa đơn bán lẻ'; };
         
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
            $scope.changedTienTheVip = function (item) {
                if (item) {
                    if (!item.TIENTHE_VIP) item.TIENTHE_VIP = 0;
                    if (!item.TIEN_CHIETKHAU) item.TIEN_CHIETKHAU = 0;
                    if (!item.TIEN_KHUYENMAI) item.TIEN_KHUYENMAI = 0;
                    if (item.TIENTHE_VIP <= 100) {
                        //giảm giá theo %
                        item.TIENTHE_VIP = Math.round(100 * (item.TIENTHE_VIP * (item.SOLUONG * item.GIABANLE_VAT) / 100)) / 100;
                        item.THANHTIEN = Math.round(100 * ((item.SOLUONG * item.GIABANLE_VAT) - item.TIENTHE_VIP / 100)) / 100 - item.TIEN_CHIETKHAU - item.TIEN_KHUYENMAI;
                    }
                    else {
                        //giảm giá theo số tiền
                        item.THANHTIEN = Math.round(100 * ((item.SOLUONG * item.GIABANLE_VAT) - item.TIENTHE_VIP)) / 100 - item.TIEN_CHIETKHAU - item.TIEN_KHUYENMAI;
                    }
                    $scope.$watch("target.DataDetails", function (newValue, oldValue) {
                        $scope.target.TONGTIEN = TinhTongTien($scope.target.DataDetails, 'THANHTIEN_CHUAGIAM');
                        $scope.target.TONGTIEN_SAUGIAM = TinhTongTien($scope.target.DataDetails, 'THANHTIEN');
                        $scope.target.TONG_TIEN_CHIETKHAU = TinhTongTien($scope.target.DataDetails, 'TIEN_CHIETKHAU');
                        $scope.target.TONG_TIENTHE_VIP = TinhTongTien($scope.target.DataDetails, 'TIENTHE_VIP');
                        $scope.target.TONG_TIEN_KHUYENMAI = TinhTongTien($scope.target.DataDetails, 'TIEN_KHUYENMAI');
                    }, true);
                }
            };

            $scope.changedTienChietKhau = function (item) {
                if (item) {
                    if (!item.TIENTHE_VIP) item.TIENTHE_VIP = 0;
                    if (!item.TIEN_CHIETKHAU) item.TIEN_CHIETKHAU = 0;
                    if (!item.TIEN_KHUYENMAI) item.TIEN_KHUYENMAI = 0;
                    if (item.TIENTHE_VIP <= 100) {
                        //giảm giá theo %
                        item.TIEN_CHIETKHAU = Math.round(100 * (item.TIEN_CHIETKHAU * (item.SOLUONG * item.GIABANLE_VAT) / 100)) / 100;
                        item.THANHTIEN = Math.round(100 * ((item.SOLUONG * item.GIABANLE_VAT) - item.TIEN_CHIETKHAU / 100)) / 100 - item.TIENTHE_VIP - item.TIEN_KHUYENMAI;
                    }
                    else {
                        //giảm giá theo số tiền
                        item.TYLE_CHIETKHAU = Math.round(100 * (item.TIEN_CHIETKHAU / (item.SOLUONG * item.GIABANLE_VAT))) / 100;
                        item.THANHTIEN = Math.round(100 * ((item.SOLUONG * item.GIABANLE_VAT) - item.TIEN_CHIETKHAU)) / 100 - item.TIENTHE_VIP - item.TIEN_KHUYENMAI;
                    }
                    $scope.$watch("target.DataDetails", function (newValue, oldValue) {
                        $scope.target.TONGTIEN = TinhTongTien($scope.target.DataDetails, 'THANHTIEN_CHUAGIAM');
                        $scope.target.TONGTIEN_SAUGIAM = TinhTongTien($scope.target.DataDetails, 'THANHTIEN');
                        $scope.target.TONG_TIEN_CHIETKHAU = TinhTongTien($scope.target.DataDetails, 'TIEN_CHIETKHAU');
                        $scope.target.TONG_TIENTHE_VIP = TinhTongTien($scope.target.DataDetails, 'TIENTHE_VIP');
                        $scope.target.TONG_TIEN_KHUYENMAI = TinhTongTien($scope.target.DataDetails, 'TIEN_KHUYENMAI');
                    }, true);
                }
            };

            $scope.changedTienKhuyenMai = function (item) {
                if (item) {
                    if (!item.TIENTHE_VIP) item.TIENTHE_VIP = 0;
                    if (!item.TIEN_CHIETKHAU) item.TIEN_CHIETKHAU = 0;
                    if (!item.TIEN_KHUYENMAI) item.TIEN_KHUYENMAI = 0;
                    if (item.TIENTHE_VIP <= 100) {
                        //giảm giá theo %
                        item.TIEN_KHUYENMAI = Math.round(100 * (item.TIEN_KHUYENMAI * (item.SOLUONG * item.GIABANLE_VAT) / 100)) / 100;
                        item.THANHTIEN = Math.round(100 * ((item.SOLUONG * item.GIABANLE_VAT) - item.TIEN_KHUYENMAI / 100)) / 100 - item.TIENTHE_VIP - item.TIEN_CHIETKHAU;
                    }
                    else {
                        //giảm giá theo số tiền
                        item.TYLE_KHUYENMAI = Math.round(100 * (item.TIEN_KHUYENMAI / (item.SOLUONG * item.GIABANLE_VAT))) / 100;
                        item.THANHTIEN = Math.round(100 * ((item.SOLUONG * item.GIABANLE_VAT) - item.TYLE_KHUYENMAI)) / 100 - item.TIENTHE_VIP - item.TIEN_CHIETKHAU;
                    }
                    $scope.$watch("target.DataDetails", function (newValue, oldValue) {
                        $scope.target.TONGTIEN = TinhTongTien($scope.target.DataDetails, 'THANHTIEN_CHUAGIAM');
                        $scope.target.TONGTIEN_SAUGIAM = TinhTongTien($scope.target.DataDetails, 'THANHTIEN');
                        $scope.target.TONG_TIEN_CHIETKHAU = TinhTongTien($scope.target.DataDetails, 'TIEN_CHIETKHAU');
                        $scope.target.TONG_TIENTHE_VIP = TinhTongTien($scope.target.DataDetails, 'TIENTHE_VIP');
                        $scope.target.TONG_TIEN_KHUYENMAI = TinhTongTien($scope.target.DataDetails, 'TIEN_KHUYENMAI');
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
                    $scope.target.TONGTIEN = TinhTongTien($scope.target.DataDetails, 'THANHTIEN_CHUAGIAM');
                    $scope.target.TONGTIEN_SAUGIAM = TinhTongTien($scope.target.DataDetails, 'THANHTIEN');
                    $scope.target.TONG_TIEN_CHIETKHAU = TinhTongTien($scope.target.DataDetails, 'TIEN_CHIETKHAU');
                    $scope.target.TONG_TIENTHE_VIP = TinhTongTien($scope.target.DataDetails, 'TIENTHE_VIP');
                    $scope.target.TONG_TIEN_KHUYENMAI = TinhTongTien($scope.target.DataDetails, 'TIEN_KHUYENMAI');
                }, true);
            };
            listenerDataDetails();

            function filterData() {
                service.getDetails($scope.target.ID).then(function (sucessRes) {
                    if (sucessRes && sucessRes.status === 200 && sucessRes.data && sucessRes.data.Status && sucessRes.data.Data) {
                        $scope.target = sucessRes.data.Data;
                        $scope.target.NGAY_GIAODICH = new Date($scope.target.NGAY_GIAODICH);
                        if ($scope.target.DataDetails && $scope.target.DataDetails.length > 0) {
                            angular.forEach($scope.target.DataDetails, function (v, k) {
                                v.THANHTIEN_CHUAGIAM = v.SOLUONG * v.GIABANLE_VAT;
                            });
                        }
                        $scope.pageChanged();
                    }
                });
            };
            filterData();

            $scope.save = function () {
                var isError = false;
                if ($scope.target.DataDetails && $scope.target.DataDetails.length > 0) {
                    if ($scope.target.MATHUE_TOANDON != $scope.target.DataDetails[0].MATHUE_VAO) isError = true;
                }
                if (!isError && (!$scope.target.MA_GIAODICH || !$scope.target.NGAY_GIAODICH || !$scope.target.MANHACUNGCAP || !$scope.target.MAKHO_NHAP || $scope.target.DataDetails.length <= 0)) {
                    Lobibox.notify('warning', {
                        title: 'Thiếu thông tin',
                        msg: 'Thông tin đầu vào không hợp lệ! Dữ liệu (*) không được để trống',
                        delay: 4000
                    });
                } else {
                    if (!isError && ($scope.addItem.MAHANG && $scope.addItem.MAHANG.length > 0 && $scope.addItem.TENHANG && $scope.addItem.TENHANG.length > 0 && $scope.addItem.THANHTIEN && $scope.addItem.THANHTIEN != 0)) {
                        Lobibox.notify('default', {
                            title: 'Nhắc nhở',
                            msg: 'Dòng hàng [' + $scope.addItem.MAHANG + '] chưa được thêm mới xuống danh sách, Bạn hãy thêm hoặc xóa trước khi lưu',
                            delay: 4000
                        });
                    }
                    else if (isError) {
                        Lobibox.notify('warning', {
                            title: 'Sai dữ liệu',
                            msg: 'Thuế toàn đơn và thuế từng mặt hàng khác nhau',
                            delay: 4000
                        });
                    }
                    else {
                        $scope.target.NGAY_GIAODICH = $scope.config.moment($scope.target.NGAY_GIAODICH).format();
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

    app.controller('xuatBanLeThuNganSearch_Ctrl', ['$scope', '$uibModalInstance', '$http', 'configService', 'xuatBanLeThuNganService', 'tempDataService', '$filter', '$uibModal', 'filterObject',
       function ($scope, $uibModalInstance, $http, configService, service, tempDataService, $filter, $uibModal, filterObject) {
           $scope.config = angular.copy(configService);;
           $scope.paged = angular.copy(configService.pageDefault);
           $scope.filtered = angular.copy(configService.filterDefault);
           $scope.title = function () { return 'Danh sách giao dịch bán lẻ'; };
           $scope.isLoading = false;
           $scope.sortType = 'MA_GIAODICH';
           $scope.sortReverse = false;
           $scope.listSelectedData = [];
           if (filterObject && filterObject.ISSELECT_POST && filterObject.ISSELECT_POST.length > 0) {
               angular.forEach(filterObject.ISSELECT_POST, function (v, k) {
                   var obj = {
                       MA_GIAODICH: v.VALUE,
                       LOAI_GIAODICH: v.DESCRIPTION,
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
                           var isSelected = $filter('filter')($scope.listSelectedData, { MA_GIAODICH: v.MA_GIAODICH }, true);
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
                   var checkExistList = $filter('filter')($scope.listSelectedData, { MA_GIAODICH: item.MA_GIAODICH }, true);
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