define(['ui-bootstrap', 'controllers/catalog/nhaCungCapController', 'controllers/catalog/donViTinhController', 'controllers/catalog/thueController', 'controllers/authorize/thamSoHeThongController', 'controllers/catalog/khoHangController', 'controllers/authorize/kyKeToanController', 'controllers/catalog/matHangController', 'controllers/authorize/cuaHangController', 'controllers/authorize/thamSoHeThongController'], function () {
    'use strict';
    var app = angular.module('kiemKeModule', ['ui.bootstrap', 'nhaCungCapModule', 'donViTinhModule', 'thueModule', 'thamSoHeThongModule', 'khoHangModule', 'kyKeToanModule', 'matHangModule', 'cuaHangModule', 'thamSoHeThongModule']);
    app.factory('kiemKeService', ['$http', 'configService', function ($http, configService) {
        var serviceUrl = configService.rootUrlWebApi + '/Knowledge/KiemKe';
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
            getPrintItemDetails: function (ID) {
                return $http.get(serviceUrl + '/GetPrintItemDetails/' + ID);
            },
            writeDataToExcel: function (data) {
                return $http.post(serviceUrl + '/WriteDataToExcel', data);
            }
        }
        return result;
    }]);
    /* controller list */
    app.controller('KiemKe_Ctrl', ['$scope', '$http', 'configService', 'kiemKeService', 'tempDataService', '$filter', '$uibModal', '$log', 'securityService', 'nhaCungCapService', 'thueService', 'khoHangService',
        function ($scope, $http, configService, service, tempDataService, $filter, $uibModal, $log, securityService, nhaCungCapService, thueService, khoHangService) {
            $scope.config = angular.copy(configService);
            $scope.paged = angular.copy(configService.pageDefault);
            $scope.filtered = angular.copy(configService.filterDefault);
            $scope.tempData = tempDataService.tempData;
            $scope.title = function () { return 'Kiểm kê hàng hóa' };
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
                securityService.getAccessList('KiemKe').then(function (successRes) {
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


            $scope.printItem = function (target) {
                var modalInstance = $uibModal.open({
                    backdrop: 'static',
                    animation: true,
                    windowClass: 'knowledge-window',
                    templateUrl: configService.buildUrl('knowledge/KiemKe', 'printItem'),
                    controller: 'kiemKePrintItemController',
                    resolve: {
                        targetData: function () {
                            return target;
                        }
                    }
                });
                modalInstance.result.then(function (updatedData) {
                    $scope.refresh();
                }, function () {
                    $log.info('Modal dismissed at: ' + new Date());
                });
            };

            $scope.printItemShelves = function (target) {
                var modalInstance = $uibModal.open({
                    backdrop: 'static',
                    animation: true,
                    windowClass: 'knowledge-window',
                    templateUrl: configService.buildUrl('knowledge/KiemKe', 'printItemShelves'),
                    controller: 'kiemKePrintItemShelvesController',
                    resolve: {
                        targetData: function () {
                            return target;
                        }
                    }
                });
                modalInstance.result.then(function (updatedData) {
                    $scope.refresh();
                }, function () {
                    $log.info('Modal dismissed at: ' + new Date());
                });
            };

            /* Function create new item */
            $scope.create = function () {
                var modalInstance = $uibModal.open({
                    backdrop: 'static',
                    animation: true,
                    windowClass: 'knowledge-window',
                    templateUrl: configService.buildUrl('knowledge/KiemKe', 'create'),
                    controller: 'kiemKeCreate_Ctrl',
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
                    templateUrl: configService.buildUrl('knowledge/KiemKe', 'detail'),
                    controller: 'kiemKeDetail_Ctrl',
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
                    templateUrl: configService.buildUrl('knowledge/KiemKe', 'approval'),
                    controller: 'kiemKeApproval_Ctrl',
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
                    templateUrl: configService.buildUrl('knowledge/KiemKe', 'edit'),
                    controller: 'kiemKeEdit_Ctrl',
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
                    templateUrl: configService.buildUrl('knowledge/KiemKe', 'delete'),
                    controller: 'kiemKeDelete_Ctrl',
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

    app.controller('kiemKePrintItemController', ['$scope', '$uibModalInstance', '$http', 'configService', 'kiemKeService', 'tempDataService', '$filter', '$uibModal', '$log', 'targetData', '$window',
        function ($scope, $uibModalInstance, $http, configService, service, tempDataService, $filter, $uibModal, $log, targetData, $window) {
            $scope.config = angular.copy(configService);
            $scope.title = function () { return 'In tem hàng hóa'; };
            $scope.data = [];
            service.getPrintItemDetails(targetData.ID).then(function (response) {
                if (response && response.status === 200 && response.data.Status && response.data.Data && response.data.Data.length > 0) {
                    $scope.data = response.data.Data;
                }
            });
            $scope.hrefTem = configService.apiServiceBaseUri + "/Upload/BARCODE/";
            $scope.exportToExcel = function () {
                service.writeDataToExcel($scope.data).then(function (response) {
                    if (response && response.status === 200 && response.data.Status && response.data.Data) {
                        Lobibox.notify('success', {
                            title: 'Thông báo',
                            width: 400,
                            msg: "Kết xuất file thành công",
                            delay: 1500
                        });
                        $window.location.href = $scope.hrefTem + "" + response.data.Message;
                        $uibModalInstance.close($scope.data);
                    }
                    else {
                        Lobibox.notify('error', {
                            position: 'bottom left',
                            msg: 'Xảy ra lỗi không thể kết xuất tệp Excel !'
                        });
                    }
                });
            };
            $scope.cancel = function () {
                $uibModalInstance.close();
            };
        }]);
    app.controller('kiemKePrintItemShelvesController', ['$scope', '$uibModalInstance', '$http', 'configService', 'kiemKeService', 'tempDataService', '$filter', '$uibModal', '$log', 'targetData', '$window',
       function ($scope, $uibModalInstance, $http, configService, service, tempDataService, $filter, $uibModal, $log, targetData, $window) {
           $scope.config = angular.copy(configService);
           $scope.title = function () { return 'In tem kệ hàng'; };
           $scope.data = [];
           service.getPrintItemDetails(targetData.ID).then(function (response) {
               if (response && response.status === 200 && response.data.Status && response.data.Data && response.data.Data.length > 0) {
                   $scope.data = response.data.Data;
                   if ($scope.data.length > 0) {
                       angular.forEach($scope.data, function (v, k) {
                           v.SOLUONG = 1;
                       });
                   }
               }
           });
           $scope.hrefTem = configService.apiServiceBaseUri + "/Upload/BARCODE/";
           $scope.exportToExcel = function () {
               service.writeDataToExcel($scope.data).then(function (response) {
                   if (response && response.status === 200 && response.data.Status && response.data.Data) {
                       Lobibox.notify('success', {
                           title: 'Thông báo',
                           width: 400,
                           msg: "Kết xuất file thành công",
                           delay: 1500
                       });
                       $window.location.href = $scope.hrefTem + "" + response.data.Message;
                       $uibModalInstance.close($scope.data);
                   }
                   else {
                       Lobibox.notify('error', {
                           position: 'bottom left',
                           msg: 'Xảy ra lỗi không thể kết xuất tệp Excel !'
                       });
                   }
               });
           };
           $scope.cancel = function () {
               $uibModalInstance.close();
           };
       }]);
    
    app.controller('kiemKeCreate_Ctrl', ['$scope', '$uibModalInstance', '$http', 'configService', 'kiemKeService', 'tempDataService', '$filter', '$uibModal', '$log', 'nhaCungCapService', 'donViTinhService', 'thueService', 'khoHangService', 'kyKeToanService', 'matHangService', 'userService','thamSoHeThongService',
    function ($scope, $uibModalInstance, $http, configService, service, tempDataService, $filter, $uibModal, $log, nhaCungCapService, donViTinhService, thueService, khoHangService, kyKeToanService, matHangService, userService, thamSoHeThongService) {
        $scope.config = angular.copy(configService);
        $scope.tempData = tempDataService.tempData;
        $scope.paged = angular.copy(configService.pageDefault);
        var currentUser = userService.GetCurrentUser();
        var unitCode = currentUser.unitCode;
        $scope.title = function () { return 'Thêm phiếu nhập hàng'; };
        $scope.target = {
            TONGTIEN_TRUOCTHUE: 0,
            TIEN_CHIETKHAU: 0,
            TONG_TIENTHUE: 0,
            TONGTIEN_SAUTHUE: 0,
            TRANGTHAI: 0,
            DataDetails: [],
            //DataChangeCost: []
        };
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
        //Tham số cấu hình sử dụng chức năng
        thamSoHeThongService.getDataByMaThamSo().then(function (successRes) {
            if (successRes && successRes.status === 200 && successRes.data && successRes.data.Status && successRes.data.Data) {
                angular.forEach(successRes.data.Data, function (v, k) {
                    if (v.MA_THAMSO === 'DEFAULT_KHONHAP' && v.GIATRI_SO === 10 && v.GIATRI_CHU.trim() != '') {
                        $scope.target.MAKHO_NHAP = v.GIATRI_CHU.trim();
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
            }
        });
        //END


        //Tính toán giá
        function getGiaTriVatVao(maThue) {
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
                addItem.THANHTIEN = Math.round(100 * (addItem.SOLUONG * addItem.GIAMUA)) / 100;
                addItem.THANHTIEN_VAT = Math.round(100 * (addItem.THANHTIEN * (1 + (getGiaTriVatVao(addItem.MATHUE_VAO)) / 100))) / 100;
                addItem.INDEX = $scope.target.DataDetails.length + 1;
            }
        };
        function kiemTraThueToanDon(thueToanDon, addItem) {
            var result = false;
            if (thueToanDon && thueToanDon !== '') {
                if (thueToanDon !== addItem.MATHUE_VAO) {
                    Lobibox.notify('warning', {
                        title: 'Dữ liệu không hợp lệ',
                        msg: 'Các mặt hàng phải cùng THUẾ',
                        delay: 3000
                    });
                    result = true;
                }
            } else {
                //nếu chưa chọn thuế tổng thì gán thuế tổng bằng thuế mặt hàng
                $scope.target.MATHUE_TOANDON = addItem.MATHUE_VAO;
            }
            return result;
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
                        if (!kiemTraThueToanDon($scope.target.MATHUE_TOANDON, refundedData)) {
                            $scope.addItem = {
                                MAHANG: refundedData.MAHANG,
                                TENHANG: refundedData.TENHANG,
                                MANHACUNGCAP: refundedData.MANHACUNGCAP,
                                MADONVITINH: refundedData.MADONVITINH,
                                MATHUE_VAO: refundedData.MATHUE_VAO,
                                MATHUE_RA: refundedData.MATHUE_RA,
                                TYLE_LAILE: refundedData.TYLE_LAILE,
                                SOLUONG: 1,
                                GIAMUA: refundedData.GIAMUA,
                                GIAMUA_VAT: refundedData.GIAMUA_VAT,
                                TIEN_GIAMGIA: 0,
                                INDEX: 0
                            };
                            caculatorThanhTien($scope.addItem);
                            $scope.pageChanged();
                        } else { errorFocusMaHang(); }
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
                    MAKHO_NHAP: $scope.target.MAKHO_NHAP,
                    UNITCODE: unitCode
                }
                matHangService.getMatHangKiemKeTheoMaKho(obj).then(function (successRes) {
                    if (successRes && successRes.status === 200 && successRes.data && successRes.data.Status && successRes.data.Data) {
                        if (!kiemTraThueToanDon($scope.target.MATHUE_TOANDON, successRes.data.Data)) {
                            $scope.addItem.BARCODE = successRes.data.Data.BARCODE;
                            $scope.addItem.MAHANG = successRes.data.Data.MAHANG;
                            $scope.addItem.TENHANG = successRes.data.Data.TENHANG;
                            $scope.addItem.MANHACUNGCAP = successRes.data.Data.MANHACUNGCAP;
                            $scope.addItem.MADONVITINH = successRes.data.Data.MADONVITINH;
                            $scope.addItem.SOLUONG = 1;
                            $scope.addItem.GIAMUA = successRes.data.Data.GIAMUA;
                            $scope.addItem.GIAMUA_VAT = successRes.data.Data.GIAMUA_VAT;
                            $scope.addItem.MATHUE_VAO = successRes.data.Data.MATHUE_VAO;
                            $scope.addItem.MATHUE_RA = successRes.data.Data.MATHUE_RA;
                            $scope.addItem.TIEN_GIAMGIA = 0;
                            $scope.addItem.TYLE_LAILE = successRes.data.Data.TYLE_LAILE;
                            $scope.addItem.THANHTIEN = Math.round(100 * ($scope.addItem.SOLUONG * $scope.addItem.GIAMUA)) / 100;
                            $scope.addItem.THANHTIEN_VAT = Math.round(100 * ($scope.addItem.THANHTIEN * (1 + (getGiaTriVatVao($scope.addItem.MATHUE_VAO)) / 100))) / 100;
                            $scope.addItem.INDEX = $scope.target.DataDetails.length + 1;
                            document.getElementById('_soLuongAddItem').focus();
                            document.getElementById('_soLuongAddItem').select();
                        } else { errorFocusMaHang(); }

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
                addItem.THANHTIEN = Math.round(100 * (addItem.SOLUONG * addItem.GIAMUA)) / 100;
                addItem.THANHTIEN_VAT = Math.round(100 * (addItem.THANHTIEN * (1 + (getGiaTriVatVao(addItem.MATHUE_VAO)) / 100))) / 100;
            }
        };

        //function UpdateListChangeCost(updateItem) {
        //    if (updateItem) {
        //        var checkExistsInList = $filter('filter')($scope.target.DataChangeCost, { MAHANG: updateItem.MAHANG }, true);
        //        if (checkExistsInList && checkExistsInList.length === 1) {
        //            //exists
        //            checkExistsInList[0].MAHANG = updateItem.MAHANG;
        //            checkExistsInList[0].GIAMUA = updateItem.GIAMUA;
        //            checkExistsInList[0].GIAMUA_VAT = updateItem.GIAMUA_VAT;
        //        }
        //        else {
        //            $scope.target.DataChangeCost.push(updateItem);
        //        }
        //    }
        //};

        $scope.changedDonGia = function (addItem) {
            if (addItem) {
                if (!addItem.SOLUONG) addItem.SOLUONG = 1;
                addItem.THANHTIEN = Math.round(100 * (addItem.SOLUONG * addItem.GIAMUA)) / 100;
                addItem.GIAMUA_VAT = Math.round(100 * (addItem.GIAMUA * (1 + (getGiaTriVatVao(addItem.MATHUE_VAO) / 100)))) / 100;
                addItem.THANHTIEN_VAT = Math.round(100 * (addItem.THANHTIEN * (1 + (getGiaTriVatVao(addItem.MATHUE_VAO)) / 100))) / 100;
                //UpdateListChangeCost(addItem);
            }
        };

        $scope.changedDonGiaVat = function (addItem) {
            if (addItem) {
                if (!addItem.SOLUONG) addItem.SOLUONG = 1;
                addItem.GIAMUA = Math.round(100 * (addItem.GIAMUA_VAT / (1 + (getGiaTriVatVao(addItem.MATHUE_VAO) / 100)))) / 100;
                addItem.THANHTIEN = Math.round(100 * (addItem.SOLUONG * addItem.GIAMUA)) / 100;
                addItem.THANHTIEN_VAT = Math.round(100 * (addItem.THANHTIEN * (1 + (getGiaTriVatVao(addItem.MATHUE_VAO)) / 100))) / 100;
                //UpdateListChangeCost(addItem);
            }
        };

        $scope.changedGiamGia = function (addItem) {
            if (addItem) {
                if (!addItem.SOLUONG) addItem.SOLUONG = 1;
                if (!addItem.TIEN_GIAMGIA) addItem.TIEN_GIAMGIA = 0;
                if (addItem.TIEN_GIAMGIA <= 100) {
                    //giảm giá theo %
                    addItem.THANHTIEN = Math.round(100 * ((addItem.SOLUONG * addItem.GIAMUA) - (addItem.TIEN_GIAMGIA * (addItem.SOLUONG * addItem.GIAMUA)) / 100)) / 100;
                }
                else {
                    //giảm giá theo số tiền
                    addItem.THANHTIEN = Math.round(100 * ((addItem.SOLUONG * addItem.GIAMUA) - addItem.TIEN_GIAMGIA)) / 100;
                }
                addItem.THANHTIEN_VAT = Math.round(100 * (addItem.THANHTIEN * (1 + (getGiaTriVatVao(addItem.MATHUE_VAO)) / 100))) / 100;
                $scope.$watch("target.DataDetails", function (newValue, oldValue) {
                    $scope.target.TIEN_CHIETKHAU = TinhTongTien($scope.target.DataDetails, 'TIEN_GIAMGIA');
                }, true);
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
                        value.TIEN_GIAMGIA = Math.round(100 * ((chietKhauTong * value.SOLUONG * value.GIAMUA) / 100)) / 100;
                        value.THANHTIEN = Math.round(100 * ((value.SOLUONG * value.GIAMUA) - value.TIEN_GIAMGIA)) / 100;
                        value.THANHTIEN_VAT = Math.round(100 * (value.THANHTIEN * (1 + (getGiaTriVatVao(value.MATHUE_VAO)) / 100))) / 100;
                    });
                }
            } else {
                //theo tiền
                if ($scope.target.DataDetails && $scope.target.DataDetails.length > 0) {
                    var TIEN_GIAMGIA_TUNGMATHANG = Math.round(100 * (chietKhauTong / $scope.target.DataDetails.length)) / 100;
                    angular.forEach($scope.target.DataDetails, function (value, key) {
                        value.TIEN_GIAMGIA = TIEN_GIAMGIA_TUNGMATHANG;
                        value.THANHTIEN = Math.round(100 * ((value.SOLUONG * value.GIAMUA) - value.TIEN_GIAMGIA)) / 100;
                        value.THANHTIEN_VAT = Math.round(100 * (value.THANHTIEN * (1 + (getGiaTriVatVao(value.MATHUE_VAO)) / 100))) / 100;
                    });
                }
            }
        };

        $scope.changedThueToanDon = function (maThueToanDon) {
            if (maThueToanDon) {
                if ($scope.target.DataDetails && $scope.target.DataDetails.length > 0) {
                    if (maThueToanDon !== $scope.target.DataDetails[0].MATHUE_VAO) $scope.target.MATHUE_TOANDON = $scope.target.DataDetails[0].MATHUE_VAO;
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

        //add Row
        $scope.addRow = function () {
            if ($scope.addItem && $scope.addItem.MAHANG !== '') {
                var exist = $filter('filter')($scope.target.DataDetails, { MAHANG: $scope.addItem.MAHANG }, true);
                if (exist && exist.length == 1) {
                    if (!kiemTraThueToanDon($scope.target.MATHUE_TOANDON, exist[0])) {
                        exist[0].SOLUONG = exist[0].SOLUONG + $scope.addItem.SOLUONG;
                        exist[0].GIAMUA = $scope.addItem.GIAMUA;
                        exist[0].GIAMUA_VAT = $scope.addItem.GIAMUA_VAT;
                        exist[0].MATHUE_VAO = $scope.addItem.MATHUE_VAO;
                        exist[0].THANHTIEN = Math.round(100 * (exist[0].SOLUONG * exist[0].GIAMUA)) / 100;
                        exist[0].THANHTIEN_VAT = Math.round(100 * (exist[0].THANHTIEN * (1 + (getGiaTriVatVao(exist[0].MATHUE_VAO)) / 100))) / 100;
                        Lobibox.notify('success', {
                            title: 'Thông báo',
                            width: 400,
                            msg: "Đã tồn tại mã hàng " + exist[0].MAHANG + ". Cộng gộp!",
                            delay: 1500
                        });
                    } else { errorFocusMaHang(); }
                }
                else {
                    if (!kiemTraThueToanDon($scope.target.MATHUE_TOANDON, $scope.addItem)) {
                        $scope.target.DataDetails.push($scope.addItem);
                    } else { errorFocusMaHang(); }
                }
                $scope.pageChanged();
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
                    THANHTIEN_VAT: 0
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
        $scope.fileName = "KiemKeExport_" + $scope.target.MA_CHUNGTU;
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
                    $scope.exportData.push(["STT", "MAHANG", "TENHANG", "MADONVITINH", "MATHUE_VAO", "TYLE_LAILE", "SOLUONG", "GIAMUA", "GIAMUA_VAT", "TIEN_GIAMGIA", "THANHTIEN", "THANHTIEN_VAT"]);
                    angular.forEach(newValue, function (value, idx) {
                        $scope.exportData.push([value.INDEX, value.MAHANG, value.TENHANG, value.MADONVITINH, value.MATHUE_VAO, value.TYLE_LAILE, value.SOLUONG, value.GIAMUA, value.GIAMUA_VAT, value.TIEN_GIAMGIA, value.THANHTIEN, value.THANHTIEN_VAT]);
                    });
                }
            }, true);
        };
        listenerDataDetails();

        $scope.save = function () {
            if (!$scope.target.MA_CHUNGTU || !$scope.target.NGAY_CHUNGTU || !$scope.target.MANHACUNGCAP || !$scope.target.MAKHO_NHAP || $scope.target.DataDetails.length <= 0) {
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


    app.controller('kiemKeDetail_Ctrl', ['$scope', '$uibModalInstance', '$http', 'configService', 'kiemKeService', 'targetData', 'tempDataService', '$filter', '$uibModal', '$log', 'thueService',
        function ($scope, $uibModalInstance, $http, configService, service, targetData, tempDataService, $filter, $uibModal, $log, thueService) {
            $scope.config = angular.copy(configService);
            $scope.paged = angular.copy(configService.pageDefault);
            $scope.tempData = tempDataService.tempData;
            $scope.target = {};
            $scope.target = angular.copy(targetData);
            //export excel
            $scope.exportData = [];
            // Prepare Excel data:
            $scope.fileName = "KiemKeExport_" + targetData.MA_CHUNGTU;
            $scope.exportData.push(["STT", "MAHANG", "TENHANG", "MADONVITINH", "MATHUE_VAO", "TYLE_LAILE", "SOLUONG", "GIAMUA", "GIAMUA_VAT", "TIEN_GIAMGIA", "THANHTIEN", "THANHTIEN_VAT"]);
            //end
            $scope.title = function () { return 'Thông tin phiếu nhập hàng [' + targetData.MA_CHUNGTU + ']'; };
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
            function getGiaTriVatVao(maThue) {
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
                service.getDetails($scope.target.ID).then(function (sucessRes) {
                    if (sucessRes && sucessRes.status === 200 && sucessRes.data && sucessRes.data.Status && sucessRes.data.Data) {
                        $scope.target = sucessRes.data.Data;
                        $scope.target.NGAY_CHUNGTU = new Date($scope.target.NGAY_CHUNGTU);
                        if ($scope.target.DataDetails && $scope.target.DataDetails.length > 0) {
                            angular.forEach($scope.target.DataDetails, function (value, idx) {
                                value.THANHTIEN_VAT = Math.round(100 * (value.THANHTIEN * (1 + (getGiaTriVatVao(value.MATHUE_VAO)) / 100))) / 100;
                                $scope.exportData.push([value.INDEX, value.MAHANG, value.TENHANG, value.MADONVITINH, value.MATHUE_VAO, value.TYLE_LAILE, value.SOLUONG, value.GIAMUA, value.GIAMUA_VAT, value.TIEN_GIAMGIA, value.THANHTIEN, value.THANHTIEN_VAT]);
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

    app.controller('kiemKeApproval_Ctrl', ['$scope', '$uibModalInstance', '$http', 'configService', 'kiemKeService', 'targetData', 'tempDataService', '$filter', '$uibModal', '$log', 'thueService', 'donViTinhService', 'userService', 'nhaCungCapService', 'cuaHangService', 'kyKeToanService', '$mdDialog',
        function ($scope, $uibModalInstance, $http, configService, service, targetData, tempDataService, $filter, $uibModal, $log, thueService, donViTinhService, userService, nhaCungCapService, cuaHangService, kyKeToanService, $mdDialog) {
            $scope.config = angular.copy(configService);
            $scope.tempData = tempDataService.tempData;
            $scope.currentUser = userService.GetCurrentUser();
            $scope.createDay = new Date();
            $scope.target = {};
            $scope.target = angular.copy(targetData);
            $scope.title = function () { return 'Duyệt phiếu nhập hàng [' + targetData.MA_CHUNGTU + ']'; };
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
            function getGiaTriVatVao(maThue) {
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
                service.getDetails($scope.target.ID).then(function (sucessRes) {
                    if (sucessRes && sucessRes.status === 200 && sucessRes.data && sucessRes.data.Status && sucessRes.data.Data) {
                        $scope.target = sucessRes.data.Data;
                        $scope.target.NGAY_CHUNGTU = new Date($scope.target.NGAY_CHUNGTU);
                        if ($scope.target.DataDetails && $scope.target.DataDetails.length > 0) {
                            angular.forEach($scope.target.DataDetails, function (value, idx) {
                                value.THANHTIEN_VAT = Math.round(100 * (value.THANHTIEN * (1 + (getGiaTriVatVao(value.MATHUE_VAO)) / 100))) / 100;
                            });
                        }
                        //get info NHACUNGCAP BY CHUNGTY
                        $scope.thongTinNhaCungCap = {};
                        var nhaCungCap = $filter('filter')($scope.tempData('nhaCungCap'), { VALUE: $scope.target.MANHACUNGCAP }, true);
                        if (nhaCungCap && nhaCungCap.length === 1) {
                            $scope.thongTinNhaCungCap = nhaCungCap[0];
                        }
                        //end
                    }
                });
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

    app.controller('kiemKeEdit_Ctrl', ['$scope', '$uibModalInstance', '$http', 'configService', 'kiemKeService', 'targetData', 'tempDataService', '$filter', '$uibModal', '$log', 'thueService', 'matHangService', 'userService',
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
            $scope.title = function () { return 'Chỉnh sửa phiếu nhập hàng'; };
            //Tính toán giá
            function getGiaTriVatVao(maThue) {
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
                    addItem.THANHTIEN = Math.round(100 * (addItem.SOLUONG * addItem.GIAMUA)) / 100;
                    addItem.THANHTIEN_VAT = Math.round(100 * (addItem.THANHTIEN * (1 + (getGiaTriVatVao(addItem.MATHUE_VAO)) / 100))) / 100;
                    addItem.INDEX = $scope.target.DataDetails.length + 1;
                }
            };
            function kiemTraThueToanDon(thueToanDon, addItem) {
                var result = false;
                if (thueToanDon && thueToanDon !== '') {
                    if (thueToanDon !== addItem.MATHUE_VAO) {
                        Lobibox.notify('warning', {
                            title: 'Dữ liệu không hợp lệ',
                            msg: 'Các mặt hàng phải cùng THUẾ',
                            delay: 3000
                        });
                        result = true;
                    }
                } else {
                    //nếu chưa chọn thuế tổng thì gán thuế tổng bằng thuế mặt hàng
                    $scope.target.MATHUE_TOANDON = addItem.MATHUE_VAO;
                }
                return result;
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
                            if (!kiemTraThueToanDon($scope.target.MATHUE_TOANDON, refundedData)) {
                                $scope.addItem = {
                                    MAHANG: refundedData.MAHANG,
                                    TENHANG: refundedData.TENHANG,
                                    MANHACUNGCAP: refundedData.MANHACUNGCAP,
                                    MADONVITINH: refundedData.MADONVITINH,
                                    MATHUE_VAO: refundedData.MATHUE_VAO,
                                    MATHUE_RA: refundedData.MATHUE_RA,
                                    TYLE_LAILE: refundedData.TYLE_LAILE,
                                    SOLUONG: 1,
                                    GIAMUA: refundedData.GIAMUA,
                                    GIAMUA_VAT: refundedData.GIAMUA_VAT,
                                    TIEN_GIAMGIA: 0,
                                    INDEX: 0
                                };
                                caculatorThanhTien($scope.addItem);
                                $scope.pageChanged();
                            } else { errorFocusMaHang(); }
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
                        MAKHO_NHAP: $scope.target.MAKHO_NHAP,
                        UNITCODE: unitCode
                    }
                    matHangService.getMatHangKiemKeTheoMaKho(obj).then(function (successRes) {
                        if (successRes && successRes.status === 200 && successRes.data && successRes.data.Status && successRes.data.Data) {
                            if (!kiemTraThueToanDon($scope.target.MATHUE_TOANDON, successRes.data.Data)) {
                                $scope.addItem.BARCODE = successRes.data.Data.BARCODE;
                                $scope.addItem.MAHANG = successRes.data.Data.MAHANG;
                                $scope.addItem.TENHANG = successRes.data.Data.TENHANG;
                                $scope.addItem.MANHACUNGCAP = successRes.data.Data.MANHACUNGCAP;
                                $scope.addItem.MADONVITINH = successRes.data.Data.MADONVITINH;
                                $scope.addItem.SOLUONG = 1;
                                $scope.addItem.GIAMUA = successRes.data.Data.GIAMUA;
                                $scope.addItem.GIAMUA_VAT = successRes.data.Data.GIAMUA_VAT;
                                $scope.addItem.MATHUE_VAO = successRes.data.Data.MATHUE_VAO;
                                $scope.addItem.MATHUE_RA = successRes.data.Data.MATHUE_RA;
                                $scope.addItem.TIEN_GIAMGIA = 0;
                                $scope.addItem.TYLE_LAILE = successRes.data.Data.TYLE_LAILE;
                                $scope.addItem.THANHTIEN = Math.round(100 * ($scope.addItem.SOLUONG * $scope.addItem.GIAMUA)) / 100;
                                $scope.addItem.THANHTIEN_VAT = Math.round(100 * ($scope.addItem.THANHTIEN * (1 + (getGiaTriVatVao($scope.addItem.MATHUE_VAO)) / 100))) / 100;
                                $scope.addItem.INDEX = $scope.target.DataDetails.length + 1;
                                document.getElementById('_soLuongAddItem').focus();
                                document.getElementById('_soLuongAddItem').select();
                            } else { errorFocusMaHang(); }

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
                    addItem.THANHTIEN = Math.round(100 * (addItem.SOLUONG * addItem.GIAMUA)) / 100;
                    addItem.THANHTIEN_VAT = Math.round(100 * (addItem.THANHTIEN * (1 + (getGiaTriVatVao(addItem.MATHUE_VAO)) / 100))) / 100;
                }
            };

            $scope.changedDonGia = function (addItem) {
                if (addItem) {
                    if (!addItem.SOLUONG) addItem.SOLUONG = 1;
                    addItem.THANHTIEN = Math.round(100 * (addItem.SOLUONG * addItem.GIAMUA)) / 100;
                    addItem.GIAMUA_VAT = Math.round(100 * (addItem.GIAMUA * (1 + (getGiaTriVatVao(addItem.MATHUE_VAO) / 100)))) / 100;
                    addItem.THANHTIEN_VAT = Math.round(100 * (addItem.THANHTIEN * (1 + (getGiaTriVatVao(addItem.MATHUE_VAO)) / 100))) / 100;
                }
            };

            $scope.changedDonGiaVat = function (addItem) {
                if (addItem) {
                    if (!addItem.SOLUONG) addItem.SOLUONG = 1;
                    addItem.GIAMUA = Math.round(100 * (addItem.GIAMUA_VAT / (1 + (getGiaTriVatVao(addItem.MATHUE_VAO) / 100)))) / 100;
                    addItem.THANHTIEN = Math.round(100 * (addItem.SOLUONG * addItem.GIAMUA)) / 100;
                    addItem.THANHTIEN_VAT = Math.round(100 * (addItem.THANHTIEN * (1 + (getGiaTriVatVao(addItem.MATHUE_VAO)) / 100))) / 100;
                }
            };

            $scope.changedGiamGia = function (addItem) {
                if (addItem) {
                    if (!addItem.SOLUONG) addItem.SOLUONG = 1;
                    if (!addItem.TIEN_GIAMGIA) addItem.TIEN_GIAMGIA = 0;
                    if (addItem.TIEN_GIAMGIA <= 100) {
                        //giảm giá theo %
                        addItem.THANHTIEN = Math.round(100 * ((addItem.SOLUONG * addItem.GIAMUA) - (addItem.TIEN_GIAMGIA * (addItem.SOLUONG * addItem.GIAMUA)) / 100)) / 100;
                    }
                    else {
                        //giảm giá theo số tiền
                        addItem.THANHTIEN = Math.round(100 * ((addItem.SOLUONG * addItem.GIAMUA) - addItem.TIEN_GIAMGIA)) / 100;
                    }
                    addItem.THANHTIEN_VAT = Math.round(100 * (addItem.THANHTIEN * (1 + (getGiaTriVatVao(addItem.MATHUE_VAO)) / 100))) / 100;
                    $scope.$watch("target.DataDetails", function (newValue, oldValue) {
                        $scope.target.TIEN_CHIETKHAU = TinhTongTien($scope.target.DataDetails, 'TIEN_GIAMGIA');
                    }, true);
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
                            value.TIEN_GIAMGIA = Math.round(100 * ((chietKhauTong * value.SOLUONG * value.GIAMUA) / 100)) / 100;
                            value.THANHTIEN = Math.round(100 * ((value.SOLUONG * value.GIAMUA) - value.TIEN_GIAMGIA)) / 100;
                            value.THANHTIEN_VAT = Math.round(100 * (value.THANHTIEN * (1 + (getGiaTriVatVao(value.MATHUE_VAO)) / 100))) / 100;
                        });
                    }
                } else {
                    //theo tiền
                    if ($scope.target.DataDetails && $scope.target.DataDetails.length > 0) {
                        var TIEN_GIAMGIA_TUNGMATHANG = Math.round(100 * (chietKhauTong / $scope.target.DataDetails.length)) / 100;
                        angular.forEach($scope.target.DataDetails, function (value, key) {
                            value.TIEN_GIAMGIA = TIEN_GIAMGIA_TUNGMATHANG;
                            value.THANHTIEN = Math.round(100 * ((value.SOLUONG * value.GIAMUA) - value.TIEN_GIAMGIA)) / 100;
                            value.THANHTIEN_VAT = Math.round(100 * (value.THANHTIEN * (1 + (getGiaTriVatVao(value.MATHUE_VAO)) / 100))) / 100;
                        });
                    }
                }
            };

            $scope.changedThueToanDon = function (maThueToanDon) {
                if (maThueToanDon) {
                    if ($scope.target.DataDetails && $scope.target.DataDetails.length > 0) {
                        if (maThueToanDon !== $scope.target.DataDetails[0].MATHUE_VAO) $scope.target.MATHUE_TOANDON = $scope.target.DataDetails[0].MATHUE_VAO;
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

            //add Row
            $scope.addRow = function () {
                if ($scope.addItem && $scope.addItem.MAHANG !== '') {
                    var exist = $filter('filter')($scope.target.DataDetails, { MAHANG: $scope.addItem.MAHANG }, true);
                    if (exist && exist.length == 1) {
                        if (!kiemTraThueToanDon($scope.target.MATHUE_TOANDON, exist[0])) {
                            exist[0].SOLUONG = exist[0].SOLUONG + $scope.addItem.SOLUONG;
                            exist[0].GIAMUA = $scope.addItem.GIAMUA;
                            exist[0].GIAMUA_VAT = $scope.addItem.GIAMUA_VAT;
                            exist[0].MATHUE_VAO = $scope.addItem.MATHUE_VAO;
                            exist[0].THANHTIEN = Math.round(100 * (exist[0].SOLUONG * exist[0].GIAMUA)) / 100;
                            exist[0].THANHTIEN_VAT = Math.round(100 * (exist[0].THANHTIEN * (1 + (getGiaTriVatVao(exist[0].MATHUE_VAO)) / 100))) / 100;
                            Lobibox.notify('success', {
                                title: 'Thông báo',
                                width: 400,
                                msg: "Đã tồn tại mã hàng " + exist[0].MAHANG + ". Cộng gộp!",
                                delay: 1500
                            });
                        } else { errorFocusMaHang(); }
                    }
                    else {
                        if (!kiemTraThueToanDon($scope.target.MATHUE_TOANDON, $scope.addItem)) {
                            $scope.addItem.INDEX = $scope.target.DataDetails.length + 2;
                            $scope.target.DataDetails.push($scope.addItem);
                        } else { errorFocusMaHang(); }
                    }
                    $scope.pageChanged();
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
                        THANHTIEN_VAT: 0
                    };
                    errorFocusMaHang();
                }
                else {
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
            $scope.fileName = "KiemKeExport_" + $scope.target.MA_CHUNGTU;
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
                        $scope.exportData.push(["STT", "MAHANG", "TENHANG", "MADONVITINH", "MATHUE_VAO", "TYLE_LAILE", "SOLUONG", "GIAMUA", "GIAMUA_VAT", "TIEN_GIAMGIA", "THANHTIEN", "THANHTIEN_VAT"]);
                        angular.forEach(newValue, function (value, idx) {
                            $scope.exportData.push([value.INDEX, value.MAHANG, value.TENHANG, value.MADONVITINH, value.MATHUE_VAO, value.TYLE_LAILE, value.SOLUONG, value.GIAMUA, value.GIAMUA_VAT, value.TIEN_GIAMGIA, value.THANHTIEN, value.THANHTIEN_VAT]);
                        });
                    }
                }, true);
            };
            listenerDataDetails();

            function filterData() {
                service.getDetails($scope.target.ID).then(function (sucessRes) {
                    if (sucessRes && sucessRes.status === 200 && sucessRes.data && sucessRes.data.Status && sucessRes.data.Data) {
                        $scope.target = sucessRes.data.Data;
                        $scope.target.NGAY_CHUNGTU = new Date($scope.target.NGAY_CHUNGTU);
                        if ($scope.target.DataDetails && $scope.target.DataDetails.length > 0) {
                            angular.forEach($scope.target.DataDetails, function (value, idx) {
                                value.THANHTIEN_VAT = Math.round(100 * (value.THANHTIEN * (1 + (getGiaTriVatVao(value.MATHUE_VAO)) / 100))) / 100;
                                $scope.exportData.push([value.INDEX, value.MAHANG, value.TENHANG, value.MADONVITINH, value.MATHUE_VAO, value.TYLE_LAILE, value.SOLUONG, value.GIAMUA, value.GIAMUA_VAT, value.TIEN_GIAMGIA, value.THANHTIEN, value.THANHTIEN_VAT]);
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
                if (!isError && (!$scope.target.MA_CHUNGTU || !$scope.target.NGAY_CHUNGTU || !$scope.target.MANHACUNGCAP || !$scope.target.MAKHO_NHAP || $scope.target.DataDetails.length <= 0)) {
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

    app.controller('kiemKeDelete_Ctrl', ['$scope', '$uibModalInstance', '$http', 'configService', 'kiemKeService', 'targetData', 'tempDataService', '$filter', '$uibModal', '$log',
       function ($scope, $uibModalInstance, $http, configService, service, targetData, tempDataService, $filter, $uibModal, $log) {
           $scope.config = angular.copy(configService);
           $scope.tempData = tempDataService.tempData;
           $scope.target = {};
           $scope.target = angular.copy(targetData);;
           $scope.title = function () { return 'Xóa phiếu nhập hàng [' + targetData.MA_CHUNGTU + ']'; };
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