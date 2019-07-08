define(['ui-bootstrap', 'controllers/catalog/loaiHangController', 'controllers/catalog/nhomHangController', 'controllers/catalog/nhaCungCapController', 'controllers/catalog/donViTinhController', 'controllers/catalog/keHangController', 'controllers/catalog/thueController', 'controllers/authorize/thamSoHeThongController'], function () {
    'use strict';
    var app = angular.module('matHangModule', ['ui.bootstrap', 'loaiHangModule', 'nhomHangModule', 'nhaCungCapModule', 'donViTinhModule', 'keHangModule', 'thueModule', 'thamSoHeThongModule']);
    app.factory('matHangService', ['$http', 'configService', function ($http, configService) {
        var serviceUrl = configService.rootUrlWebApi + '/Catalog/MatHang';
        var selectedData = [];
        var result = {
            //service set ; get data
            getSelectData: function () {
                return selectedData;
            },
            setSelectData: function (array) {
                selectedData = array;
            },
            clearSelectData: function () {
                selectedData = [];
            },
            //end service
            getAllData: function () {
                return $http.get(serviceUrl + '/GetAllData');
            },
            getAllBarcode: function () {
                return $http.get(serviceUrl + '/GetAllBarcode');
            },
            postQuery: function (data) {
                return $http.post(serviceUrl + '/PostQuery', data);
            },
            postQueryInventory: function (data) {
                return $http.post(serviceUrl + '/PostQueryInventory', data);
            },
            buildNewCode: function (maLoaiSelected, unitCode) {
                return $http.get(serviceUrl + '/BuildNewCode/' + maLoaiSelected + '/' + unitCode);
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
            getMatHangNhapMuaTheoMaKho: function (data) {
                return $http.post(serviceUrl + '/GetMatHangNhapMuaTheoMaKho', data);
            },
            getMatHangXuatBanTheoMaKho: function (data) {
                return $http.post(serviceUrl + '/GetMatHangXuatBanTheoMaKho', data);
            },
            getMatHangTheoDieuKien: function (data) {
                return $http.post(serviceUrl + '/GetMatHangTheoDieuKien', data);
            },
            searchDataByKey: function (data) {
                return $http.post(serviceUrl + '/SearchDataByKey', data);
            },
            checkExistBarcode: function (barcodeText) {
                return $http.get(serviceUrl + '/CheckExistBarcode/' + barcodeText);
            },
            getPhysicalPathTemplate: function () {
                return $http.get(serviceUrl + '/GetPhysicalPathTemplate');
            },
            postExcelData: function (listData) {
                return $http.post(serviceUrl + '/PostExcelData', listData);
            }
        }
        return result;
    }]);
    /* controller list */
    app.controller('MatHang_Ctrl', ['$scope', '$http', 'configService', 'matHangService', 'tempDataService', '$filter', '$uibModal', '$log', 'securityService', 'loaiHangService', 'nhomHangService', 'nhaCungCapService', 'donViTinhService', 'keHangService', 'thueService', 'userService',
        function ($scope, $http, configService, service, tempDataService, $filter, $uibModal, $log, securityService, loaiHangService, nhomHangService, nhaCungCapService, donViTinhService, keHangService, thueService, userService) {
            var currentUser = userService.GetCurrentUser();
            var unitCode = currentUser.unitCode;
            $scope.config = angular.copy(configService);
            $scope.paged = angular.copy(configService.pageDefault);
            $scope.filtered = angular.copy(configService.filterDefault);
            $scope.tempData = tempDataService.tempData;
            $scope.title = function () { return 'Mặt hàng' };
            $scope.data = [];
            $scope.setPage = function (pageNo) {
                $scope.paged.CurrentPage = pageNo;
                filterData();
            };
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

            //Function load data catalog DonViTinh
            function loadDataKeHang() {
                $scope.keHang = [];
                if (!tempDataService.tempData('keHang')) {
                    keHangService.getAllData().then(function (successRes) {
                        if (successRes && successRes.status === 200 && successRes.data && successRes.data.Status && successRes.data.Data && successRes.data.Data.length > 0) {
                            tempDataService.putTempData('keHang', successRes.data.Data);
                            $scope.keHang = successRes.data.Data;
                        }
                    }, function (errorRes) {
                        console.log('errorRes', errorRes);
                    });
                } else {
                    $scope.keHang = tempDataService.tempData('keHang');
                }
            };
            loadDataKeHang();
            //end

            //Function load data catalog DonViTinh
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

            //Function load barcode danh muc mathang
            function loadBarcode() {
                $scope.barcode = [];
                if (!tempDataService.tempData('barcode')) {
                    service.getAllBarcode().then(function (successRes) {
                        if (successRes && successRes.status === 200 && successRes.data && successRes.data.Status && successRes.data.Data && successRes.data.Data.length > 0) {
                            tempDataService.putTempData('barcode', successRes.data.Data);
                            $scope.barcode = successRes.data.Data;
                        }
                    }, function (errorRes) {
                        console.log('errorRes', errorRes);
                    });
                } else {
                    $scope.barcode = tempDataService.tempData('barcode');
                }
            };
            loadBarcode();
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
                var userName = currentUser.userName;
                var unitCodeParam = !currentUser.parentUnitCode ? currentUser.unitCode : currentUser.parentUnitCode;
                securityService.getAccessList('MatHang', userName, unitCodeParam).then(function (successRes) {
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
                    templateUrl: configService.buildUrl('catalog/MatHang', 'create'),
                    controller: 'matHangCreate_Ctrl',
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
                    templateUrl: configService.buildUrl('catalog/MatHang', 'detail'),
                    controller: 'matHangDetail_Ctrl',
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
                    templateUrl: configService.buildUrl('catalog/MatHang', 'edit'),
                    controller: 'matHangEdit_Ctrl',
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
                    templateUrl: configService.buildUrl('catalog/MatHang', 'delete'),
                    controller: 'matHangDelete_Ctrl',
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
            //download Template Excel
            $scope.dowloadTemplateExcel = function () {
                var now = new Date();
                var month = now.getUTCMonth() + 1;
                var day = now.getUTCDate();
                var year = now.getUTCFullYear();
                service.getPhysicalPathTemplate().then(function (refundedRes) {
                    if (refundedRes && refundedRes.data) {
                        var urlTemplate = refundedRes.data + 'TemplateKhaiBaoHangHoa.xlsx';
                        window.open(urlTemplate);
                    }
                });
            };
            //

            //read data from Excel file
            $scope.JSON = null;
            function to_json(workbook) {
                var result = {};
                workbook.SheetNames.forEach(function (sheetName) {
                    var roa = XLS.utils.sheet_to_row_object_array(workbook.Sheets[sheetName]);
                    if (roa.length > 0) {
                        result[sheetName] = roa;
                    }
                });
                return result;
            };
            $scope.readExcelFile = function (e) {
                if (e && e.target) {
                    var file = event.target.files[0]
                    var reader = new FileReader();
                    reader.onload = function (e) {
                        var workbook = XLSX.read(e.target.result, { type: 'binary' });
                        $scope.JSON = JSON.stringify(to_json(workbook), 2, 2);
                        $scope.$apply(function () {
                            var refundData = JSON.parse($scope.JSON);
                            if (refundData && refundData.MATHANG && refundData.MATHANG.length > 0) {
                                var modalInstance = $uibModal.open({
                                    backdrop: 'static',
                                    animation: true,
                                    windowClass: 'modal-checkImport',
                                    templateUrl: configService.buildUrl('catalog/MatHang', 'checkDataImport'),
                                    controller: 'matHangCheckDataImport_Ctrl',
                                    resolve: {
                                        targetData: function () {
                                            return refundData.MATHANG;
                                        }
                                    }
                                });
                            }
                        });
                    };
                    reader.readAsBinaryString(file);
                }
            };
            //end read data from Excel file
        }]);

    app.controller('matHangCreate_Ctrl', ['$scope', '$uibModalInstance', '$http', 'configService', 'matHangService', 'tempDataService', '$filter', '$uibModal', '$log', 'nhomHangService', 'thueService', '$timeout', 'Upload', 'thamSoHeThongService','userService',
        function ($scope, $uibModalInstance, $http, configService, service, tempDataService, $filter, $uibModal, $log, nhomHangService, thueService, $timeout, upload, thamSoHeThongService, userService) {
            $scope.config = angular.copy(configService);
            $scope.tempData = tempDataService.tempData;
            var currentUser = userService.GetCurrentUser();
            $scope.title = function () { return 'Thêm mặt hàng'; };
            $scope.target = {};
            $scope.target.GIAMUA = 0;
            $scope.target.GIAMUA_VAT = 0;
            $scope.target.GIABANLE = 0;
            $scope.target.GIABANLE_VAT = 0;
            $scope.target.GIABANBUON = 0;
            $scope.target.GIABANBUON_VAT = 0;
            $scope.target.TYLE_LAILE = 0;
            $scope.target.TYLE_LAIBUON = 0;
            //Tham số cấu hình sử dụng chức năng thêm mới nhanh
            $scope.IS_QUICK_ADD_CATALOG = false;
            thamSoHeThongService.getDataByMaThamSo().then(function (successRes) {
                if (successRes && successRes.status === 200 && successRes.data && successRes.data.Status && successRes.data.Data) {
                    angular.forEach(successRes.data.Data, function (v, k) {
                        if (v.MA_THAMSO === 'QUICK_ADD_CATALOG' && v.GIATRI_SO === 10) {
                            $scope.IS_QUICK_ADD_CATALOG = true;
                        }
                        if (v.MA_THAMSO === 'DEFAULT_VAT' && v.GIATRI_SO === 10 && v.GIATRI_CHU.trim() != '') {
                            $scope.target.MATHUE_VAO = v.GIATRI_CHU.trim();
                            $scope.target.MATHUE_RA = v.GIATRI_CHU.trim();
                        }
                    });
                }
            });

            //end tham số
            $scope.convertCodeToName = function (paraValue, moduleName) {
                if (paraValue) {
                    var tempCache = $filter('filter')($scope.tempData(moduleName), { VALUE: paraValue }, true);
                    if (tempCache && tempCache.length === 1) {
                        return tempCache[0].TEXT;
                    } else {
                        return paraValue;
                    }
                }
            };
            //Change mã loại gender to MaHang
            $scope.changeMaLoai = function (maLoaiSelected) {
                $scope.listMaNhom = [];
                if (maLoaiSelected) {
                    var unitCodeParam = !currentUser.parentUnitCode ? currentUser.unitCode : currentUser.parentUnitCode;
                    service.buildNewCode(maLoaiSelected, unitCodeParam).then(function (successRes) {
                        if (successRes && successRes.status === 200 && successRes.data) {
                            $scope.target.MAHANG = successRes.data;
                        }
                    });
                    if ($scope.tempData('nhomHang') && $scope.tempData('nhomHang').length > 0) {
                        $scope.listMaNhom = $filter('filter')($scope.tempData('nhomHang'), { PARENT: maLoaiSelected }, true);
                    } else {
                        nhomHangService.getDataByMaLoai(maLoaiSelected).then(function (successRes) {
                            if (successRes && successRes.status === 200 && successRes.data && successRes.data.Status && successRes.data.Data && successRes.data.Data.length > 0) {
                                $scope.listMaNhom = successRes.data.Data;
                            }
                        }, function (errorRes) {
                            console.log('errorRes', errorRes);
                        });
                    }
                }
            };
            //end
            //Quick access create LoaiHang
            $scope.createLoai = function () {
                var modalInstance = $uibModal.open({
                    backdrop: 'static',
                    animation: true,
                    size: 'lg',
                    templateUrl: configService.buildUrl('catalog/LoaiHang', 'create'),
                    controller: 'loaiHangCreate_Ctrl',
                    resolve: {}
                });
                modalInstance.result.then(function (refundedData) {
                    if (refundedData) {
                        $scope.target.MALOAI = refundedData.MALOAI;
                        $scope.changeMaLoai($scope.target.MALOAI);
                        if ($scope.tempData('loaiHang')) {
                            var tempObj = {};
                            tempObj.VALUE = refundedData.MALOAI;
                            tempObj.TEXT = refundedData.MALOAI + " | " + refundedData.TENLOAI;
                            tempObj.DESCRIPTION = refundedData.TENLOAI;
                            $scope.tempData('loaiHang').push(tempObj);
                        }
                    }
                }, function () {
                    $log.info('Modal dismissed at: ' + new Date());
                });
            };
            //end 

            //Quick access create NhomHang
            $scope.createNhom = function () {
                var modalInstance = $uibModal.open({
                    backdrop: 'static',
                    animation: true,
                    size: 'lg',
                    templateUrl: configService.buildUrl('catalog/NhomHang', 'create'),
                    controller: 'nhomHangCreate_Ctrl',
                    resolve: {}
                });
                modalInstance.result.then(function (refundedData) {
                    if (refundedData) {
                        $scope.target.MANHOM = refundedData.MANHOM;
                        var tempObj = {};
                        tempObj.VALUE = refundedData.MANHOM;
                        tempObj.TEXT = refundedData.MANHOM + " | " + refundedData.TENNHOM;
                        tempObj.DESCRIPTION = refundedData.TENNHOM;
                        tempObj.PARENT = refundedData.MALOAI;
                        if ($scope.tempData('nhomHang')) {
                            $scope.tempData('nhomHang').push(tempObj);
                        }

                        if ($scope.listMaNhom && $scope.listMaNhom.length > 0) {
                            $scope.listMaNhom.push(tempObj);
                        }
                        else {
                            $scope.listMaNhom = [];
                            $scope.listMaNhom.push(tempObj);
                        }
                    }
                }, function () {
                    $log.info('Modal dismissed at: ' + new Date());
                });
            };
            //end

            //Quick access create NhaCungCap
            $scope.createNhaCungCap = function () {
                var modalInstance = $uibModal.open({
                    backdrop: 'static',
                    animation: true,
                    size: 'lg',
                    templateUrl: configService.buildUrl('catalog/NhaCungCap', 'create'),
                    controller: 'nhaCungCapCreate_Ctrl',
                    resolve: {}
                });
                modalInstance.result.then(function (refundedData) {
                    if (refundedData) {
                        $scope.target.MANHACUNGCAP = refundedData.MANHACUNGCAP;
                        var tempObj = {};
                        tempObj.VALUE = refundedData.MANHACUNGCAP;
                        tempObj.TEXT = refundedData.MANHACUNGCAP + " | " + refundedData.TENNHACUNGCAP;
                        tempObj.DESCRIPTION = refundedData.TENNHACUNGCAP;
                        if ($scope.tempData('nhaCungCap')) {
                            $scope.tempData('nhaCungCap').push(tempObj);
                        }
                    }
                }, function () {
                    $log.info('Modal dismissed at: ' + new Date());
                });
            };
            //end

            //Quick access create DonViTinh
            $scope.createDonViTinh = function () {
                var modalInstance = $uibModal.open({
                    backdrop: 'static',
                    animation: true,
                    size: 'lg',
                    templateUrl: configService.buildUrl('catalog/DonViTinh', 'create'),
                    controller: 'donViTinhCreate_Ctrl',
                    resolve: {}
                });
                modalInstance.result.then(function (refundedData) {
                    if (refundedData) {
                        $scope.target.MADONVITINH = refundedData.MADONVITINH;
                        var tempObj = {};
                        tempObj.VALUE = refundedData.MADONVITINH;
                        tempObj.TEXT = refundedData.MADONVITINH + " | " + refundedData.TENDONVITINH;
                        tempObj.DESCRIPTION = refundedData.TENDONVITINH;
                        if ($scope.tempData('donViTinh')) {
                            $scope.tempData('donViTinh').push(tempObj);
                        }
                    }
                }, function () {
                    $log.info('Modal dismissed at: ' + new Date());
                });
            };
            //end

            //Quick access create KeHang
            $scope.createKeHang = function () {
                var modalInstance = $uibModal.open({
                    backdrop: 'static',
                    animation: true,
                    size: 'lg',
                    templateUrl: configService.buildUrl('catalog/KeHang', 'create'),
                    controller: 'keHangCreate_Ctrl',
                    resolve: {}
                });
                modalInstance.result.then(function (refundedData) {
                    if (refundedData) {
                        $scope.target.MAKEHANG = refundedData.MAKEHANG;
                        var tempObj = {};
                        tempObj.VALUE = refundedData.MAKEHANG;
                        tempObj.TEXT = refundedData.MAKEHANG + " | " + refundedData.TENKEHANG;
                        tempObj.DESCRIPTION = refundedData.TENKEHANG;
                        if ($scope.tempData('keHang')) {
                            $scope.tempData('keHang').push(tempObj);
                        }
                    }
                }, function () {
                    $log.info('Modal dismissed at: ' + new Date());
                });
            };
            //end

            //Quick access create Thue Vao
            $scope.createThue_Vao = function () {
                var modalInstance = $uibModal.open({
                    backdrop: 'static',
                    animation: true,
                    size: 'lg',
                    templateUrl: configService.buildUrl('catalog/Thue', 'create'),
                    controller: 'thueCreate_Ctrl',
                    resolve: {}
                });
                modalInstance.result.then(function (refundedData) {
                    if (refundedData) {
                        $scope.target.MATHUE_VAO = refundedData.MATHUE;
                        var tempObj = {};
                        tempObj.VALUE = refundedData.MATHUE;
                        tempObj.TEXT = refundedData.MATHUE + " | " + refundedData.TENTHUE;
                        tempObj.DESCRIPTION = refundedData.TENTHUE;
                        tempObj.GIATRI = refundedData.GIATRI;
                        if ($scope.tempData('thue')) {
                            $scope.tempData('thue').push(tempObj);
                        }
                    }
                }, function () {
                    $log.info('Modal dismissed at: ' + new Date());
                });
            };
            //end

            //Quick access create Thue Ra
            $scope.createThue_Ra = function () {
                var modalInstance = $uibModal.open({
                    backdrop: 'static',
                    animation: true,
                    size: 'lg',
                    templateUrl: configService.buildUrl('catalog/Thue', 'create'),
                    controller: 'thueCreate_Ctrl',
                    resolve: {}
                });
                modalInstance.result.then(function (refundedData) {
                    if (refundedData) {
                        $scope.target.MATHUE_RA = refundedData.MATHUE;
                        var tempObj = {};
                        tempObj.VALUE = refundedData.MATHUE;
                        tempObj.TEXT = refundedData.MATHUE + " | " + refundedData.TENTHUE;
                        tempObj.DESCRIPTION = refundedData.TENTHUE;
                        tempObj.GIATRI = refundedData.GIATRI;
                        if ($scope.tempData('thue')) {
                            $scope.tempData('thue').push(tempObj);
                        }
                    }
                }, function () {
                    $log.info('Modal dismissed at: ' + new Date());
                });
            };
            //end

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

            $scope.selectedThueVao = function (maThueVao) {
                if (maThueVao && maThueVao != '') {
                    if ($scope.tempData('thue') && $scope.tempData('thue').length > 0) {
                        var dataTax = $filter('filter')($scope.tempData('thue'), { VALUE: maThueVao }, true);
                        if (dataTax && dataTax.length === 1) {
                            $scope.target.GIAMUA_VAT = $scope.target.GIAMUA * (dataTax[0].GIATRI / 100) + $scope.target.GIAMUA;
                        }
                    }
                    else {
                        thueService.getDataByMaThue(maThueVao).then(function (successRes) {
                            if (successRes && successRes.status === 200 && successRes.data && successRes.data.Status && successRes.data.Data) {
                                $scope.target.GIAMUA_VAT = $scope.target.GIAMUA * (successRes.data.Data.GIATRI / 100) + $scope.target.GIAMUA;
                            }
                        }, function (errorRes) {
                            console.log('errorRes', errorRes);
                        });
                    }
                }
            };

            $scope.selectedThueRa = function (maThueRa) {
                if (maThueRa && maThueRa != '') {
                    if ($scope.tempData('thue') && $scope.tempData('thue').length > 0) {
                        var dataTax = $filter('filter')($scope.tempData('thue'), { VALUE: maThueRa }, true);
                        if (dataTax && dataTax.length === 1) {
                            $scope.target.GIABANBUON_VAT = Math.round(100 * (($scope.target.GIABANBUON * (dataTax[0].GIATRI / 100)) + $scope.target.GIABANBUON)) / 100;
                            $scope.target.GIABANLE_VAT = Math.round(100 * (($scope.target.GIABANLE * (dataTax[0].GIATRI / 100)) + $scope.target.GIABANLE)) / 100;
                        }
                    }
                    else {
                        thueService.getDataByMaThue(maThueRa).then(function (successRes) {
                            if (successRes && successRes.status === 200 && successRes.data && successRes.data.Status && successRes.data.Data) {
                                $scope.target.GIABANBUON_VAT = Math.round(100 * (($scope.target.GIABANBUON * (successRes.data.Data.GIATRI / 100)) + $scope.target.GIABANBUON)) / 100;
                                $scope.target.GIABANLE_VAT = Math.round(100 * (($scope.target.GIABANLE * (successRes.data.Data.GIATRI / 100)) + $scope.target.GIABANLE)) / 100;
                            }
                        }, function (errorRes) {
                            console.log('errorRes', errorRes);
                        });
                    }
                }
            };

            $scope.changedGiaMua = function (giaMua) {
                if (giaMua) {
                    $scope.target.GIAMUA_VAT = Math.round(100 * (giaMua * (getGiaTriVatVao($scope.target.MATHUE_VAO) / 100) + giaMua)) / 100;
                    $scope.target.GIABANBUON = Math.round(100 * (giaMua * ($scope.target.TYLE_LAIBUON / 100) + giaMua)) / 100;
                    $scope.target.GIABANLE = Math.round(100 * (giaMua * ($scope.target.TYLE_LAILE / 100) + giaMua)) / 100;
                    $scope.target.GIABANLE_VAT = Math.round(100 * ($scope.target.GIABANLE * (getGiaTriVatVao($scope.target.MATHUE_RA) / 100) + $scope.target.GIABANLE)) / 100;
                    $scope.target.GIABANBUON_VAT = Math.round(100 * ($scope.target.GIABANBUON * (getGiaTriVatVao($scope.target.MATHUE_RA) / 100) + $scope.target.GIABANBUON)) / 100;
                }
            };

            $scope.changedGiaMuaVat = function (giaMuaVat) {
                if (giaMuaVat) {
                    $scope.target.GIAMUA = Math.round(100 * (giaMuaVat / ((getGiaTriVatVao($scope.target.MATHUE_VAO) / 100) + 1))) / 100;
                    $scope.target.GIABANLE = Math.round(100 * ($scope.target.GIAMUA * ($scope.target.TYLE_LAILE / 100) + $scope.target.GIAMUA)) / 100;
                    $scope.target.GIABANBUON = Math.round(100 * ($scope.target.GIAMUA * ($scope.target.TYLE_LAIBUON / 100) + $scope.target.GIAMUA)) / 100;
                    $scope.target.GIABANLE_VAT = Math.round(100 * ($scope.target.GIABANLE * (getGiaTriVatVao($scope.target.MATHUE_RA) / 100) + $scope.target.GIABANLE)) / 100;
                    $scope.target.GIABANBUON_VAT = Math.round(100 * ($scope.target.GIABANBUON * (getGiaTriVatVao($scope.target.MATHUE_RA) / 100) + $scope.target.GIABANBUON)) / 100;
                }
            };

            $scope.changedTyLeLaiLe = function (tyLeLaiLe) {
                if (tyLeLaiLe) {
                    $scope.target.GIABANLE = Math.round(100 * ($scope.target.GIAMUA * (tyLeLaiLe / 100) + $scope.target.GIAMUA)) / 100;
                    $scope.target.GIABANLE_VAT = Math.round(100 * ($scope.target.GIABANLE * (getGiaTriVatVao($scope.target.MATHUE_RA) / 100) + $scope.target.GIABANLE)) / 100;
                }
            };

            $scope.changedGiaBanLe = function (giaBanLe) {
                if (giaBanLe) {
                    $scope.target.TYLE_LAILE = Math.round(100 * ((100 * (giaBanLe - $scope.target.GIAMUA)) / $scope.target.GIAMUA)) / 100;
                    if (parseInt($scope.target.TYLE_LAILE) <= -100 || parseInt($scope.target.TYLE_LAILE) > 1000) {
                        Lobibox.notify('warning', {
                            title: 'Kiểm tra thông tin',
                            msg: 'Kiểm tra lại tỷ lệ lãi lẻ (quá lớn hoặc quá nhỏ)',
                            delay: 4000
                        });
                    }
                    $scope.target.GIABANLE_VAT = Math.round(100 * (giaBanLe * (getGiaTriVatVao($scope.target.MATHUE_RA) / 100) + giaBanLe)) / 100;
                }
            };

            $scope.changedTyLeLaiBuon = function (tyLeLaiBuon) {
                if (tyLeLaiBuon) {
                    $scope.target.GIABANBUON = Math.round(100 * (($scope.target.GIAMUA * (tyLeLaiBuon / 100)) + $scope.target.GIAMUA)) / 100;
                    $scope.target.GIABANBUON_VAT = Math.round(100 * (($scope.target.GIABANBUON * (getGiaTriVatVao($scope.target.MATHUE_RA) / 100)) + $scope.target.GIABANBUON)) / 100;
                }
            };

            $scope.changedGiaBanBuon = function (giaBanBuon) {
                if (giaBanBuon) {
                    $scope.target.TYLE_LAIBUON = Math.round(100 * ((100 * (giaBanBuon - $scope.target.GIAMUA)) / $scope.target.GIAMUA)) / 100;
                    if (parseInt($scope.target.TYLE_LAIBUON) <= -100 || parseInt($scope.target.TYLE_LAIBUON) > 1000) {
                        Lobibox.notify('warning', {
                            title: 'Kiểm tra thông tin',
                            msg: 'Kiểm tra lại tỷ lệ lãi buôn (quá lớn hoặc quá nhỏ)',
                            delay: 4000
                        });
                    }
                    $scope.target.GIABANBUON_VAT = Math.round(100 * (giaBanBuon * (getGiaTriVatVao($scope.target.MATHUE_RA) / 100) + giaBanBuon)) / 100;
                }
            };

            $scope.changedGiaBanLeVat = function (giaBanLeCoVat) {
                if (giaBanLeCoVat) {
                    $scope.target.GIABANLE = Math.round(100 * (giaBanLeCoVat / (1 + (getGiaTriVatVao($scope.target.MATHUE_RA) / 100)))) / 100;
                    $scope.target.TYLE_LAILE = Math.round(100 * ((100 * ($scope.target.GIABANLE - $scope.target.GIAMUA)) / $scope.target.GIAMUA)) / 100;
                    if (parseInt($scope.target.TYLE_LAILE) <= -100 || parseInt($scope.target.TYLE_LAILE) > 1000) {
                        Lobibox.notify('warning', {
                            title: 'Kiểm tra thông tin',
                            msg: 'Kiểm tra lại tỷ lệ lãi lẻ (quá lớn hoặc quá nhỏ)',
                            delay: 4000
                        });
                    }
                }
            };

            $scope.changedGiaBanBuonVat = function (giaBanBuonCoVat) {
                if (giaBanBuonCoVat) {
                    $scope.target.GIABANBUON = Math.round(100 * (giaBanBuonCoVat / (1 + (getGiaTriVatVao($scope.target.MATHUE_RA) / 100)))) / 100;
                    $scope.target.TYLE_LAIBUON = Math.round(100 * ((100 * ($scope.target.GIABANBUON - $scope.target.GIAMUA)) / $scope.target.GIAMUA)) / 100;
                    if (parseInt($scope.target.TYLE_LAIBUON) <= -100 || parseInt($scope.target.TYLE_LAIBUON) > 1000) {
                        Lobibox.notify('warning', {
                            title: 'Kiểm tra thông tin',
                            msg: 'Kiểm tra lại tỷ lệ lãi buôn (quá lớn hoặc quá nhỏ)',
                            delay: 4000
                        });
                    }
                }
            };
            //end tính toán giá

            //upload list image MatHang
            $scope.lstFile = [];
            $scope.lstImagesSrc = [];
            $scope.uploadFile = function (input) {
                if (input.files && input.files.length > 0) {
                    angular.forEach(input.files, function (file) {
                        $scope.lstFile.push(file);
                        $timeout(function () {
                            var fileReader = new FileReader();
                            fileReader.readAsDataURL(file);
                            fileReader.onload = function (e) {
                                $timeout(function () {
                                    $scope.lstImagesSrc.push(e.target.result);
                                });
                            };
                        });
                    });
                }
            };
            $scope.deleteImage = function (index) {
                $scope.lstImagesSrc.splice(index, 1);
                $scope.lstFile.splice(index, 1);
                if ($scope.lstFile.length < 1) {
                    angular.element("#file-input-upload").val(null);
                }
            };
            //end upload MatHang

            //upload list avatar MatHang
            $scope.fileAvatar = {};
            $scope.uploadAvatar = function (input) {
                $scope.inputAvatar = input;
                if (input.files && input.files.length > 0) {
                    $timeout(function () {
                        var fileReader = new FileReader();
                        fileReader.readAsDataURL(input.files[0]);
                        fileReader.onload = function (e) {
                            $timeout(function () {
                                $scope.fileAvatar.SRC = e.target.result;
                            });
                        };
                    });
                    $scope.fileAvatar.FILE = input.files[0];
                }
            };
            $scope.deleteAvatar = function () {
                if ($scope.target.AVATAR) {
                    $scope.target.AVATAR = null;
                }
                if ($scope.fileAvatar) {
                    $scope.fileAvatar = {};
                    angular.element("#file-input-avatar").val(null);
                }
            };
            //end upload avatar MatHang

            //function convertArray Barcodet to string
            function convertBarcodeToString(arrayBarcode) {
                var barcode = '';
                if (arrayBarcode && arrayBarcode.length > 0) {
                    angular.forEach(arrayBarcode, function (v, k) {
                        if (v.text && v.text != '') {
                            barcode += v.text + ';';
                        }
                    });
                }
                return barcode;
            };
            //end convert
            //check exist Barcode 
            $scope.validBarcode = function (tagAdd) {
                if (tagAdd && tagAdd.text.length > 0) {
                    service.checkExistBarcode(tagAdd.text).then(function (successRes) {
                        if (successRes && successRes.status === 200 && successRes.data && !successRes.data.Status && successRes.data.Data) {
                            $scope.target.BARCODE_ADD.splice(-1, 1);
                            Lobibox.notify('warning', {
                                title: 'Trùng mã BARCODE',
                                msg: successRes.data.Message,
                                delay: 3000
                            });
                        }
                    });
                }
            }
            //end check exist
            function saveMatHang() {
                //save MatHang
                $scope.target.BARCODE = convertBarcodeToString($scope.target.BARCODE_ADD);
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
                //end save MatHang
            };

            $scope.save = function () {
                if (!$scope.target.MAHANG || !$scope.target.TENHANG || !$scope.target.MANHACUNGCAP || !$scope.target.MALOAI || !$scope.target.MANHOM || !$scope.target.MATHUE_VAO || !$scope.target.MATHUE_RA) {
                    Lobibox.notify('warning', {
                        title: 'Thiếu thông tin',
                        msg: 'Thông tin đầu vào không hợp lệ! Dữ liệu (*) không được để trống',
                        delay: 4000
                    });
                } else {
                    //save image
                    if ($scope.fileAvatar && $scope.fileAvatar.SRC) {
                        $scope.fileAvatar.MAHANG = $scope.target.MAHANG;
                        upload.upload({
                            url: configService.rootUrlWebApi + '/Catalog/MatHang/UploadAvatar',
                            data: $scope.fileAvatar
                        }).then(function (successRes) {
                            if (successRes && successRes.status === 200 && successRes.data && successRes.data.Status && successRes.data.Data) {
                                $scope.target.AVATAR_NAME = successRes.data.Data.FILENAME;
                                $scope.target.DUONGDAN = successRes.data.Data.DUONGDAN;
                                //nếu avatar thành công thì mới upload các ảnh con
                                if ($scope.lstFile && $scope.lstFile.length > 0) {
                                    $scope.target.FILE = $scope.lstFile;
                                    upload.upload({
                                        url: configService.rootUrlWebApi + '/Catalog/MatHang/UploadImage',
                                        data: $scope.target
                                    }).then(function (successRes) {
                                        if (successRes && successRes.status === 200 && successRes.data && successRes.data.Status && successRes.data.Data) {
                                            $scope.target.IMAGE = successRes.data.Data.FILENAME;
                                            $scope.target.DUONGDAN = successRes.data.Data.DUONGDAN;
                                            //nếu có cả ảnh avatar và list ảnh
                                            saveMatHang();
                                        } else {
                                            Lobibox.notify('error', {
                                                title: 'Xảy ra lỗi',
                                                msg: 'Đã xảy ra lỗi! Thao tác upload ảnh không thành công',
                                                delay: 2500
                                            });
                                        }
                                    });
                                } else {
                                    //nếu chỉ có ảnh avatar
                                    saveMatHang();
                                }
                                //end
                            } else {
                                Lobibox.notify('error', {
                                    title: 'Xảy ra lỗi',
                                    msg: 'Đã xảy ra lỗi! Thao tác upload Avatar không thành công',
                                    delay: 2500
                                });
                            }
                        });
                    }
                    else {
                        //nếu không chọn ảnh avartar
                        saveMatHang();
                    }
                    //end save image
                }
            };
            $scope.cancel = function () {
                $uibModalInstance.close();
            };
        }]);
    app.controller('matHangDetail_Ctrl', ['$scope', '$uibModalInstance', '$http', 'configService', 'matHangService', 'targetData', 'tempDataService', '$filter', '$uibModal', '$log',
        function ($scope, $uibModalInstance, $http, configService, service, targetData, tempDataService, $filter, $uibModal, $log) {
            $scope.config = angular.copy(configService);
            $scope.tempData = tempDataService.tempData;
            $scope.target = {};
            $scope.target = angular.copy(targetData);
            $scope.qrcode = 'https://google.com/';
            $scope.convertCodeToName = function (paraValue, moduleName) {
                if (paraValue) {
                    var tempCache = $filter('filter')($scope.tempData(moduleName), { VALUE: paraValue }, true);
                    if (tempCache && tempCache.length === 1) {
                        return tempCache[0].TEXT;
                    } else {
                        return paraValue;
                    }
                }
            };
            service.getDetails($scope.target.ID).then(function (sucessRes) {
                if (sucessRes && sucessRes.status === 200 && sucessRes.data && sucessRes.data.Status && sucessRes.data.Data) {
                    $scope.target = sucessRes.data.Data;
                    if ($scope.target.IMAGE) {
                        var arrImage = $scope.target.IMAGE.split(',');
                        arrImage.splice(arrImage.length - 1, 1);
                        $scope.lstImagesSrcFromDb = arrImage;
                    }

                    if ($scope.target.BARCODE && $scope.target.BARCODE.length > 0) {
                        $scope.target.BARCODE_ADD = $scope.target.BARCODE.split(";");
                        if ($scope.target.BARCODE.substring($scope.target.BARCODE.length - 1) === ";") $scope.target.BARCODE_ADD.splice(-1, 1);
                    }
                }
            });
            $scope.title = function () { return 'Thông tin mặt hàng [' + targetData.MAHANG + ']'; };
            $scope.cancel = function () {
                $uibModalInstance.close();
            };
        }]);
    app.controller('matHangEdit_Ctrl', ['$scope', '$uibModalInstance', '$http', 'configService', 'matHangService', 'targetData', 'tempDataService', '$filter', '$uibModal', '$log', 'nhomHangService', 'thueService', '$timeout', 'Upload', 'thamSoHeThongService','userService',
        function ($scope, $uibModalInstance, $http, configService, service, targetData, tempDataService, $filter, $uibModal, $log, nhomHangService, thueService, $timeout, upload, thamSoHeThongService, userService) {
            $scope.config = angular.copy(configService);
            var currentUser = userService.GetCurrentUser();
            $scope.tempData = tempDataService.tempData;
            $scope.target = {};
            $scope.target = angular.copy(targetData);
            $scope.title = function () { return 'Chỉnh sửa mặt hàng'; };
            //Tham số cấu hình sử dụng chức năng thêm mới nhanh
            $scope.IS_QUICK_ADD_CATALOG = false;
            thamSoHeThongService.getDataByMaThamSo().then(function (successRes) {
                if (successRes && successRes.status === 200 && successRes.data && successRes.data.Status && successRes.data.Data) {
                    angular.forEach(successRes.data.Data, function (v, k) {
                        if (v.MA_THAMSO === 'QUICK_ADD_CATALOG' && v.GIATRI_SO === 10) {
                            $scope.IS_QUICK_ADD_CATALOG = true;
                        }
                    });
                }
            });
            //end tham số
            $scope.convertCodeToName = function (paraValue, moduleName) {
                if (paraValue) {
                    var tempCache = $filter('filter')($scope.tempData(moduleName), { VALUE: paraValue }, true);
                    if (tempCache && tempCache.length === 1) {
                        return tempCache[0].TEXT;
                    } else {
                        return paraValue;
                    }
                }
            };

            $scope.listMaNhom = [];
            if ($scope.target.MALOAI) {
                var unitCodeParam = !currentUser.parentUnitCode ? currentUser.unitCode : currentUser.parentUnitCode;
                service.buildNewCode($scope.target.MALOAI, unitCodeParam).then(function (successRes) {
                    if (successRes && successRes.status === 200 && successRes.data) {
                        $scope.target.MAHANG = successRes.data;
                    }
                });
                if ($scope.tempData('nhomHang') && $scope.tempData('nhomHang').length > 0) {
                    $scope.listMaNhom = $filter('filter')($scope.tempData('nhomHang'), { PARENT: $scope.target.MALOAI }, true);
                } else {
                    nhomHangService.getDataByMaLoai($scope.target.MALOAI).then(function (successRes) {
                        if (successRes && successRes.status === 200 && successRes.data && successRes.data.Status && successRes.data.Data && successRes.data.Data.length > 0) {
                            $scope.listMaNhom = successRes.data.Data;
                        }
                    }, function (errorRes) {
                        console.log('errorRes', errorRes);
                    });
                }
            }

            //Change mã loại gender to MaHang
            $scope.changeMaLoai = function (maLoaiSelected) {
                $scope.listMaNhom = [];
                if (maLoaiSelected) {
                    service.buildNewCode(maLoaiSelected).then(function (successRes) {
                        if (successRes && successRes.status === 200 && successRes.data) {
                            $scope.target.MAHANG = successRes.data;
                        }
                    });
                    if ($scope.tempData('nhomHang') && $scope.tempData('nhomHang').length > 0) {
                        $scope.listMaNhom = $filter('filter')($scope.tempData('nhomHang'), { PARENT: maLoaiSelected }, true);
                    } else {
                        nhomHangService.getDataByMaLoai(maLoaiSelected).then(function (successRes) {
                            if (successRes && successRes.status === 200 && successRes.data && successRes.data.Status && successRes.data.Data && successRes.data.Data.length > 0) {
                                $scope.listMaNhom = successRes.data.Data;
                            }
                        }, function (errorRes) {
                            console.log('errorRes', errorRes);
                        });
                    }
                }
            };
            //end
            //Quick access create LoaiHang
            $scope.createLoai = function () {
                var modalInstance = $uibModal.open({
                    backdrop: 'static',
                    animation: true,
                    size: 'lg',
                    templateUrl: configService.buildUrl('catalog/LoaiHang', 'create'),
                    controller: 'loaiHangCreate_Ctrl',
                    resolve: {}
                });
                modalInstance.result.then(function (refundedData) {
                    if (refundedData) {
                        $scope.target.MALOAI = refundedData.MALOAI;
                        $scope.changeMaLoai($scope.target.MALOAI);
                        if ($scope.tempData('loaiHang')) {
                            var tempObj = {};
                            tempObj.VALUE = refundedData.MALOAI;
                            tempObj.TEXT = refundedData.MALOAI + " | " + refundedData.TENLOAI;
                            tempObj.DESCRIPTION = refundedData.TENLOAI;
                            $scope.tempData('loaiHang').push(tempObj);
                        }
                    }
                }, function () {
                    $log.info('Modal dismissed at: ' + new Date());
                });
            };
            //end 

            //Quick access create NhomHang
            $scope.createNhom = function () {
                var modalInstance = $uibModal.open({
                    backdrop: 'static',
                    animation: true,
                    size: 'lg',
                    templateUrl: configService.buildUrl('catalog/NhomHang', 'create'),
                    controller: 'nhomHangCreate_Ctrl',
                    resolve: {}
                });
                modalInstance.result.then(function (refundedData) {
                    if (refundedData) {
                        $scope.target.MANHOM = refundedData.MANHOM;
                        var tempObj = {};
                        tempObj.VALUE = refundedData.MANHOM;
                        tempObj.TEXT = refundedData.MANHOM + " | " + refundedData.TENNHOM;
                        tempObj.DESCRIPTION = refundedData.TENNHOM;
                        tempObj.PARENT = refundedData.MALOAI;
                        if ($scope.tempData('nhomHang')) {
                            $scope.tempData('nhomHang').push(tempObj);
                        }

                        if ($scope.listMaNhom && $scope.listMaNhom.length > 0) {
                            $scope.listMaNhom.push(tempObj);
                        }
                        else {
                            $scope.listMaNhom = [];
                            $scope.listMaNhom.push(tempObj);
                        }
                    }
                }, function () {
                    $log.info('Modal dismissed at: ' + new Date());
                });
            };
            //end

            //Quick access create NhaCungCap
            $scope.createNhaCungCap = function () {
                var modalInstance = $uibModal.open({
                    backdrop: 'static',
                    animation: true,
                    size: 'lg',
                    templateUrl: configService.buildUrl('catalog/NhaCungCap', 'create'),
                    controller: 'nhaCungCapCreate_Ctrl',
                    resolve: {}
                });
                modalInstance.result.then(function (refundedData) {
                    if (refundedData) {
                        $scope.target.MANHACUNGCAP = refundedData.MANHACUNGCAP;
                        var tempObj = {};
                        tempObj.VALUE = refundedData.MANHACUNGCAP;
                        tempObj.TEXT = refundedData.MANHACUNGCAP + " | " + refundedData.TENNHACUNGCAP;
                        tempObj.DESCRIPTION = refundedData.TENNHACUNGCAP;
                        if ($scope.tempData('nhaCungCap')) {
                            $scope.tempData('nhaCungCap').push(tempObj);
                        }
                    }
                }, function () {
                    $log.info('Modal dismissed at: ' + new Date());
                });
            };
            //end

            //Quick access create DonViTinh
            $scope.createDonViTinh = function () {
                var modalInstance = $uibModal.open({
                    backdrop: 'static',
                    animation: true,
                    size: 'lg',
                    templateUrl: configService.buildUrl('catalog/DonViTinh', 'create'),
                    controller: 'donViTinhCreate_Ctrl',
                    resolve: {}
                });
                modalInstance.result.then(function (refundedData) {
                    if (refundedData) {
                        $scope.target.MADONVITINH = refundedData.MADONVITINH;
                        var tempObj = {};
                        tempObj.VALUE = refundedData.MADONVITINH;
                        tempObj.TEXT = refundedData.MADONVITINH + " | " + refundedData.TENDONVITINH;
                        tempObj.DESCRIPTION = refundedData.TENDONVITINH;
                        if ($scope.tempData('donViTinh')) {
                            $scope.tempData('donViTinh').push(tempObj);
                        }
                    }
                }, function () {
                    $log.info('Modal dismissed at: ' + new Date());
                });
            };
            //end

            //Quick access create KeHang
            $scope.createKeHang = function () {
                var modalInstance = $uibModal.open({
                    backdrop: 'static',
                    animation: true,
                    size: 'lg',
                    templateUrl: configService.buildUrl('catalog/KeHang', 'create'),
                    controller: 'keHangCreate_Ctrl',
                    resolve: {}
                });
                modalInstance.result.then(function (refundedData) {
                    if (refundedData) {
                        $scope.target.MAKEHANG = refundedData.MAKEHANG;
                        var tempObj = {};
                        tempObj.VALUE = refundedData.MAKEHANG;
                        tempObj.TEXT = refundedData.MAKEHANG + " | " + refundedData.TENKEHANG;
                        tempObj.DESCRIPTION = refundedData.TENKEHANG;
                        if ($scope.tempData('keHang')) {
                            $scope.tempData('keHang').push(tempObj);
                        }
                    }
                }, function () {
                    $log.info('Modal dismissed at: ' + new Date());
                });
            };
            //end

            //Quick access create Thue Vao
            $scope.createThue_Vao = function () {
                var modalInstance = $uibModal.open({
                    backdrop: 'static',
                    animation: true,
                    size: 'lg',
                    templateUrl: configService.buildUrl('catalog/Thue', 'create'),
                    controller: 'thueCreate_Ctrl',
                    resolve: {}
                });
                modalInstance.result.then(function (refundedData) {
                    if (refundedData) {
                        $scope.target.MATHUE_VAO = refundedData.MATHUE;
                        var tempObj = {};
                        tempObj.VALUE = refundedData.MATHUE;
                        tempObj.TEXT = refundedData.MATHUE + " | " + refundedData.TENTHUE;
                        tempObj.DESCRIPTION = refundedData.TENTHUE;
                        tempObj.GIATRI = refundedData.GIATRI;
                        if ($scope.tempData('thue')) {
                            $scope.tempData('thue').push(tempObj);
                        }
                    }
                }, function () {
                    $log.info('Modal dismissed at: ' + new Date());
                });
            };
            //end

            //Quick access create Thue Ra
            $scope.createThue_Ra = function () {
                var modalInstance = $uibModal.open({
                    backdrop: 'static',
                    animation: true,
                    size: 'lg',
                    templateUrl: configService.buildUrl('catalog/Thue', 'create'),
                    controller: 'thueCreate_Ctrl',
                    resolve: {}
                });
                modalInstance.result.then(function (refundedData) {
                    if (refundedData) {
                        $scope.target.MATHUE_RA = refundedData.MATHUE;
                        var tempObj = {};
                        tempObj.VALUE = refundedData.MATHUE;
                        tempObj.TEXT = refundedData.MATHUE + " | " + refundedData.TENTHUE;
                        tempObj.DESCRIPTION = refundedData.TENTHUE;
                        tempObj.GIATRI = refundedData.GIATRI;
                        if ($scope.tempData('thue')) {
                            $scope.tempData('thue').push(tempObj);
                        }
                    }
                }, function () {
                    $log.info('Modal dismissed at: ' + new Date());
                });
            };
            //end

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

            $scope.selectedThueVao = function (maThueVao) {
                if (maThueVao && maThueVao != '') {
                    if ($scope.tempData('thue') && $scope.tempData('thue').length > 0) {
                        var dataTax = $filter('filter')($scope.tempData('thue'), { VALUE: maThueVao }, true);
                        if (dataTax && dataTax.length === 1) {
                            $scope.target.GIAMUA_VAT = $scope.target.GIAMUA * (dataTax[0].GIATRI / 100) + $scope.target.GIAMUA;
                        }
                    }
                    else {
                        thueService.getDataByMaThue(maThueVao).then(function (successRes) {
                            if (successRes && successRes.status === 200 && successRes.data && successRes.data.Status && successRes.data.Data) {
                                $scope.target.GIAMUA_VAT = $scope.target.GIAMUA * (successRes.data.Data.GIATRI / 100) + $scope.target.GIAMUA;
                            }
                        }, function (errorRes) {
                            console.log('errorRes', errorRes);
                        });
                    }
                }
            };

            $scope.selectedThueRa = function (maThueRa) {
                if (maThueRa && maThueRa != '') {
                    if ($scope.tempData('thue') && $scope.tempData('thue').length > 0) {
                        var dataTax = $filter('filter')($scope.tempData('thue'), { VALUE: maThueRa }, true);
                        if (dataTax && dataTax.length === 1) {
                            $scope.target.GIABANBUON_VAT = Math.round(100 * (($scope.target.GIABANBUON * (dataTax[0].GIATRI / 100)) + $scope.target.GIABANBUON)) / 100;
                            $scope.target.GIABANLE_VAT = Math.round(100 * (($scope.target.GIABANLE * (dataTax[0].GIATRI / 100)) + $scope.target.GIABANLE)) / 100;
                        }
                    }
                    else {
                        thueService.getDataByMaThue(maThueRa).then(function (successRes) {
                            if (successRes && successRes.status === 200 && successRes.data && successRes.data.Status && successRes.data.Data) {
                                $scope.target.GIABANBUON_VAT = Math.round(100 * (($scope.target.GIABANBUON * (successRes.data.Data.GIATRI / 100)) + $scope.target.GIABANBUON)) / 100;
                                $scope.target.GIABANLE_VAT = Math.round(100 * (($scope.target.GIABANLE * (successRes.data.Data.GIATRI / 100)) + $scope.target.GIABANLE)) / 100;
                            }
                        }, function (errorRes) {
                            console.log('errorRes', errorRes);
                        });
                    }
                }
            };

            $scope.changedGiaMua = function (giaMua) {
                if (giaMua) {
                    $scope.target.GIAMUA_VAT = Math.round(100 * (giaMua * (getGiaTriVatVao($scope.target.MATHUE_VAO) / 100) + giaMua)) / 100;
                    $scope.target.GIABANBUON = Math.round(100 * (giaMua * ($scope.target.TYLE_LAIBUON / 100) + giaMua)) / 100;
                    $scope.target.GIABANLE = Math.round(100 * (giaMua * ($scope.target.TYLE_LAILE / 100) + giaMua)) / 100;
                    $scope.target.GIABANLE_VAT = Math.round(100 * ($scope.target.GIABANLE * (getGiaTriVatVao($scope.target.MATHUE_RA) / 100) + $scope.target.GIABANLE)) / 100;
                    $scope.target.GIABANBUON_VAT = Math.round(100 * ($scope.target.GIABANBUON * (getGiaTriVatVao($scope.target.MATHUE_RA) / 100) + $scope.target.GIABANBUON)) / 100;
                }
            };

            $scope.changedGiaMuaVat = function (giaMuaVat) {
                if (giaMuaVat) {
                    $scope.target.GIAMUA = Math.round(100 * (giaMuaVat / ((getGiaTriVatVao($scope.target.MATHUE_VAO) / 100) + 1))) / 100;
                    $scope.target.GIABANLE = Math.round(100 * ($scope.target.GIAMUA * ($scope.target.TYLE_LAILE / 100) + $scope.target.GIAMUA)) / 100;
                    $scope.target.GIABANBUON = Math.round(100 * ($scope.target.GIAMUA * ($scope.target.TYLE_LAIBUON / 100) + $scope.target.GIAMUA)) / 100;
                    $scope.target.GIABANLE_VAT = Math.round(100 * ($scope.target.GIABANLE * (getGiaTriVatVao($scope.target.MATHUE_RA) / 100) + $scope.target.GIABANLE)) / 100;
                    $scope.target.GIABANBUON_VAT = Math.round(100 * ($scope.target.GIABANBUON * (getGiaTriVatVao($scope.target.MATHUE_RA) / 100) + $scope.target.GIABANBUON)) / 100;
                }
            };

            $scope.changedTyLeLaiLe = function (tyLeLaiLe) {
                if (tyLeLaiLe) {
                    $scope.target.GIABANLE = Math.round(100 * ($scope.target.GIAMUA * (tyLeLaiLe / 100) + $scope.target.GIAMUA)) / 100;
                    $scope.target.GIABANLE_VAT = Math.round(100 * ($scope.target.GIABANLE * (getGiaTriVatVao($scope.target.MATHUE_RA) / 100) + $scope.target.GIABANLE)) / 100;
                }
            };

            $scope.changedGiaBanLe = function (giaBanLe) {
                if (giaBanLe) {
                    $scope.target.TYLE_LAILE = Math.round(100 * ((100 * (giaBanLe - $scope.target.GIAMUA)) / $scope.target.GIAMUA)) / 100;
                    if (parseInt($scope.target.TYLE_LAILE) <= -100 || parseInt($scope.target.TYLE_LAILE) > 1000) {
                        Lobibox.notify('warning', {
                            title: 'Kiểm tra thông tin',
                            msg: 'Kiểm tra lại tỷ lệ lãi lẻ (quá lớn hoặc quá nhỏ)',
                            delay: 4000
                        });
                    }
                    $scope.target.GIABANLE_VAT = Math.round(100 * (giaBanLe * (getGiaTriVatVao($scope.target.MATHUE_RA) / 100) + giaBanLe)) / 100;
                }
            };

            $scope.changedTyLeLaiBuon = function (tyLeLaiBuon) {
                if (tyLeLaiBuon) {
                    $scope.target.GIABANBUON = Math.round(100 * (($scope.target.GIAMUA * (tyLeLaiBuon / 100)) + $scope.target.GIAMUA)) / 100;
                    $scope.target.GIABANBUON_VAT = Math.round(100 * (($scope.target.GIABANBUON * (getGiaTriVatVao($scope.target.MATHUE_RA) / 100)) + $scope.target.GIABANBUON)) / 100;
                }
            };

            $scope.changedGiaBanBuon = function (giaBanBuon) {
                if (giaBanBuon) {
                    $scope.target.TYLE_LAIBUON = Math.round(100 * ((100 * (giaBanBuon - $scope.target.GIAMUA)) / $scope.target.GIAMUA)) / 100;
                    if (parseInt($scope.target.TYLE_LAIBUON) <= -100 || parseInt($scope.target.TYLE_LAIBUON) > 1000) {
                        Lobibox.notify('warning', {
                            title: 'Kiểm tra thông tin',
                            msg: 'Kiểm tra lại tỷ lệ lãi buôn (quá lớn hoặc quá nhỏ)',
                            delay: 4000
                        });
                    }
                    $scope.target.GIABANBUON_VAT = Math.round(100 * (giaBanBuon * (getGiaTriVatVao($scope.target.MATHUE_RA) / 100) + giaBanBuon)) / 100;
                }
            };

            $scope.changedGiaBanLeVat = function (giaBanLeCoVat) {
                if (giaBanLeCoVat) {
                    $scope.target.GIABANLE = Math.round(100 * (giaBanLeCoVat / (1 + (getGiaTriVatVao($scope.target.MATHUE_RA) / 100)))) / 100;
                    $scope.target.TYLE_LAILE = Math.round(100 * ((100 * ($scope.target.GIABANLE - $scope.target.GIAMUA)) / $scope.target.GIAMUA)) / 100;
                    if (parseInt($scope.target.TYLE_LAILE) <= -100 || parseInt($scope.target.TYLE_LAILE) > 1000) {
                        Lobibox.notify('warning', {
                            title: 'Kiểm tra thông tin',
                            msg: 'Kiểm tra lại tỷ lệ lãi lẻ (quá lớn hoặc quá nhỏ)',
                            delay: 4000
                        });
                    }
                }
            };

            $scope.changedGiaBanBuonVat = function (giaBanBuonCoVat) {
                if (giaBanBuonCoVat) {
                    $scope.target.GIABANBUON = Math.round(100 * (giaBanBuonCoVat / (1 + (getGiaTriVatVao($scope.target.MATHUE_RA) / 100)))) / 100;
                    $scope.target.TYLE_LAIBUON = Math.round(100 * ((100 * ($scope.target.GIABANBUON - $scope.target.GIAMUA)) / $scope.target.GIAMUA)) / 100;
                    if (parseInt($scope.target.TYLE_LAIBUON) <= -100 || parseInt($scope.target.TYLE_LAIBUON) > 1000) {
                        Lobibox.notify('warning', {
                            title: 'Kiểm tra thông tin',
                            msg: 'Kiểm tra lại tỷ lệ lãi buôn (quá lớn hoặc quá nhỏ)',
                            delay: 4000
                        });
                    }
                }
            };
            //end tính toán giá

            //upload list image MatHang
            $scope.lstFile = [];
            $scope.lstImagesSrc = [];
            $scope.uploadFile = function (input) {
                if (input.files && input.files.length > 0) {
                    angular.forEach(input.files, function (file) {
                        $scope.lstFile.push(file);
                        $timeout(function () {
                            var fileReader = new FileReader();
                            fileReader.readAsDataURL(file);
                            fileReader.onload = function (e) {
                                $timeout(function () {
                                    $scope.lstImagesSrc.push(e.target.result);
                                });
                            };
                        });
                    });
                }
            };
            $scope.deleteImage = function (index) {
                $scope.lstImagesSrc.splice(index, 1);
                $scope.lstFile.splice(index, 1);
                if ($scope.lstFile.length < 1) {
                    angular.element("#file-input-upload").val(null);
                }
            };
            //end upload MatHang

            //upload list avatar MatHang
            $scope.fileAvatar = {};
            $scope.uploadAvatar = function (input) {
                $scope.inputAvatar = input;
                if (input.files && input.files.length > 0) {
                    $timeout(function () {
                        var fileReader = new FileReader();
                        fileReader.readAsDataURL(input.files[0]);
                        fileReader.onload = function (e) {
                            $timeout(function () {
                                $scope.fileAvatar.SRC = e.target.result;
                            });
                        };
                    });
                    $scope.fileAvatar.FILE = input.files[0];
                }
            };
            $scope.deleteAvatar = function () {
                if ($scope.target.AVATAR) {
                    $scope.target.AVATAR = null;
                }
                if ($scope.fileAvatar) {
                    $scope.fileAvatar = {};
                    angular.element("#file-input-avatar").val(null);
                }
            };

            $scope.deleteImageFromDb = function (index) {
                $scope.lstImagesSrcFromDb.splice(index, 1);
                $scope.state = 'deleted';
            };
            //end upload avatar MatHang
            service.getDetails($scope.target.ID).then(function (sucessRes) {
                if (sucessRes && sucessRes.status === 200 && sucessRes.data && sucessRes.data.Status && sucessRes.data.Data) {
                    $scope.target = sucessRes.data.Data;
                    if ($scope.target.IMAGE) {
                        var arrImage = $scope.target.IMAGE.split(',');
                        arrImage.splice(arrImage.length - 1, 1);
                        $scope.lstImagesSrcFromDb = arrImage;
                        $scope.state = 'exits';
                    }

                    if ($scope.target.BARCODE && $scope.target.BARCODE.length > 0) {
                        $scope.target.BARCODE_ADD = $scope.target.BARCODE.split(";");
                        if ($scope.target.BARCODE.substring($scope.target.BARCODE.length - 1) === ";") $scope.target.BARCODE_ADD.splice(-1, 1);
                    }
                }
            });
            //function convertArray Barcodet to string
            function convertBarcodeToString(arrayBarcode) {
                var barcode = '';
                if (arrayBarcode && arrayBarcode.length > 0) {
                    angular.forEach(arrayBarcode, function (v, k) {
                        if (v.text && v.text != '') {
                            barcode += v.text + ';';
                        }
                    });
                }
                return barcode;
            };
            //end function convert
            //check exist Barcode 
            $scope.validBarcode = function (tagAdd) {
                if (tagAdd && tagAdd.text.length > 0) {
                    service.checkExistBarcode(tagAdd.text).then(function (successRes) {
                        if (successRes && successRes.status === 200 && successRes.data && !successRes.data.Status && successRes.data.Data) {
                            $scope.target.BARCODE_ADD.splice(-1, 1);
                            Lobibox.notify('warning', {
                                title: 'Trùng mã BARCODE',
                                msg: successRes.data.Message,
                                delay: 3000
                            });
                        }
                    });
                }
            }
            //end check exist
            function uploadAvartar() {
                if ($scope.fileAvatar && $scope.fileAvatar.SRC) {
                    $scope.fileAvatar.MAHANG = $scope.target.MAHANG;
                    upload.upload({
                        url: configService.rootUrlWebApi + '/Catalog/MatHang/UploadAvatar',
                        data: $scope.fileAvatar
                    }).then(function (successRes) {
                        if (successRes && successRes.status === 200 && successRes.data && successRes.data.Status && successRes.data.Data) {
                            $scope.target.AVATAR_NAME = successRes.data.Data.FILENAME;
                            $scope.target.DUONGDAN = successRes.data.Data.DUONGDAN;
                            //nếu avatar thành công thì mới upload các ảnh con
                            updateMatHang();
                            //end
                        } else {
                            Lobibox.notify('error', {
                                title: 'Xảy ra lỗi',
                                msg: 'Đã xảy ra lỗi! Thao tác upload Avatar không thành công',
                                delay: 2500
                            });
                        }
                    });
                }
                else {
                    //nếu không chọn ảnh avartar
                    updateMatHang();
                }
            };

            function updateMatHang() {
                //update MatHang
                $scope.target.BARCODE = convertBarcodeToString($scope.target.BARCODE_ADD);
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
                //end update MatHang
            };


            $scope.save = function () {
                if (!$scope.target.MAHANG || !$scope.target.TENHANG || !$scope.target.MANHACUNGCAP || !$scope.target.MALOAI || !$scope.target.MANHOM || !$scope.target.MATHUE_VAO || !$scope.target.MATHUE_RA) {
                    Lobibox.notify('warning', {
                        title: 'Thiếu thông tin',
                        msg: 'Thông tin đầu vào không hợp lệ! Dữ liệu (*) không được để trống',
                        delay: 4000
                    });
                } else {
                    //save image
                    if ($scope.lstFile && $scope.lstFile.length > 0) {
                        $scope.target.FILE = $scope.lstFile;
                        upload.upload({
                            url: configService.rootUrlWebApi + '/Catalog/MatHang/UploadImage',
                            data: $scope.target
                        }).then(function (successRes) {
                            if (successRes && successRes.status === 200 && successRes.data && successRes.data.Status && successRes.data.Data) {
                                $scope.target.IMAGE = successRes.data.Data.FILENAME;
                                $scope.target.DUONGDAN = successRes.data.Data.DUONGDAN;
                                if ($scope.lstImagesSrcFromDb && $scope.lstImagesSrcFromDb.length > 0) {
                                    angular.forEach($scope.lstImagesSrcFromDb, function (item) {
                                        $scope.target.IMAGE = $scope.target.IMAGE + item + ",";
                                    });
                                }
                                uploadAvartar()
                            } else {
                                Lobibox.notify('error', {
                                    title: 'Xảy ra lỗi',
                                    msg: 'Đã xảy ra lỗi! Thao tác upload ảnh không thành công',
                                    delay: 2500
                                });
                            }
                        });
                    } else if ($scope.target.AVATAR && $scope.target.AVATAR.length > 0) {
                        $scope.target.IMAGE = '';
                        if ($scope.lstImagesSrcFromDb && $scope.lstImagesSrcFromDb.length > 0 && $scope.state) {
                            angular.forEach($scope.lstImagesSrcFromDb, function (item) {
                                $scope.target.IMAGE = $scope.target.IMAGE + item + ",";
                            });
                        }
                        updateMatHang();
                    } else {
                        $scope.target.IMAGE = null;
                        uploadAvartar();
                    }
                    //end

                }
            };

            $scope.cancel = function () {
                $uibModalInstance.close();
            };
        }]);

    app.controller('matHangDelete_Ctrl', ['$scope', '$uibModalInstance', '$http', 'configService', 'matHangService', 'targetData', 'tempDataService', '$filter', '$uibModal', '$log',
       function ($scope, $uibModalInstance, $http, configService, service, targetData, tempDataService, $filter, $uibModal, $log) {
           $scope.config = angular.copy(configService);
           $scope.tempData = tempDataService.tempData;
           $scope.target = {};
           $scope.target = angular.copy(targetData);;
           $scope.title = function () { return 'Xóa mặt hàng [' + targetData.MAHANG + ']'; };
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

    app.controller('matHangInventorySearch_Ctrl', ['$scope', '$uibModalInstance', '$http', 'configService', 'matHangService', 'tempDataService', '$filter', '$uibModal', 'filterObject',
        function ($scope, $uibModalInstance, $http, configService, service, tempDataService, $filter, $uibModal, filterObject) {
            $scope.config = angular.copy(configService);;
            $scope.paged = angular.copy(configService.pageDefault);
            $scope.filtered = angular.copy(configService.filterDefault);
            $scope.title = function () { return 'Danh sách mặt hàng'; };
            $scope.isLoading = false;
            $scope.sortType = 'MAHANG';
            $scope.sortReverse = false;
            $scope.listSelectedData = [];
            if (filterObject && filterObject.ISSELECT_POST && filterObject.ISSELECT_POST.length > 0) {
                angular.forEach(filterObject.ISSELECT_POST, function (v, k) {
                    var obj = {
                        MAHANG: v.VALUE,
                        TENHANG: v.DESCRIPTION,
                        ISSELECT: true
                    };
                    $scope.listSelectedData.push(obj);
                });
            }
            function filterData() {
                $scope.isLoading = true;
                $scope.filtered.advanceData.TABLE_NAME = filterObject.tableName;
                $scope.filtered.advanceData.MAKHO = filterObject.maKho;
                var postdata = { paged: $scope.paged, filtered: $scope.filtered };
                service.postQueryInventory(postdata).then(function (successRes) {
                    if (successRes && successRes.status === 200 && successRes.data && successRes.data.Status && successRes.data.Data && successRes.data.Data.Data) {
                        $scope.isLoading = false;
                        $scope.data = successRes.data.Data.Data;
                        angular.forEach($scope.data, function (v, k) {
                            var isSelected = $filter('filter')($scope.listSelectedData, { MAHANG: v.MAHANG }, true);
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
                    var checkExistList = $filter('filter')($scope.listSelectedData, { MAHANG: item.MAHANG }, true);
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

    app.controller('matHangSearch_Ctrl', ['$scope', '$uibModalInstance', '$http', 'configService', 'matHangService', 'tempDataService', '$filter', '$uibModal', 'filterObject',
        function ($scope, $uibModalInstance, $http, configService, service, tempDataService, $filter, $uibModal, filterObject) {
            $scope.config = angular.copy(configService);;
            $scope.paged = angular.copy(configService.pageDefault);
            $scope.filtered = angular.copy(configService.filterDefault);
            $scope.title = function () { return 'Danh sách mặt hàng'; };
            $scope.isLoading = false;
            $scope.sortType = 'MAHANG';
            $scope.sortReverse = false;
            $scope.listSelectedData = [];
            if (filterObject && filterObject.ISSELECT_POST && filterObject.ISSELECT_POST.length > 0) {
                angular.forEach(filterObject.ISSELECT_POST, function (v, k) {
                    var obj = {
                        MAHANG: v.VALUE,
                        TENHANG: v.DESCRIPTION,
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
                            var isSelected = $filter('filter')($scope.listSelectedData, { MAHANG: v.MAHANG }, true);
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
                    var checkExistList = $filter('filter')($scope.listSelectedData, { MAHANG: item.MAHANG }, true);
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

    app.controller('matHangCheckDataImport_Ctrl', ['$scope', '$uibModalInstance', '$http', 'configService', 'matHangService', 'targetData', 'tempDataService', '$filter', '$uibModal', '$log', 'nhaCungCapService', 'thueService', 'loaiHangService', 'nhomHangService', 'donViTinhService',
        function ($scope, $uibModalInstance, $http, configService, service, targetData, tempDataService, $filter, $uibModal, $log, nhaCungCapService, thueService, loaiHangService, nhomHangService, donViTinhService) {
            $scope.config = angular.copy(configService);
            $scope.tempData = tempDataService.tempData;
            $scope.title = function () { return 'Kiểm tra mặt hàng tải lên từ Excel'; };
            $scope.target = {};
            $scope.target = angular.copy(targetData);
            $scope.data = [
            ];
            $scope.dataError = [];
            $scope.convertCodeToName = function (paraValue, moduleName) {
                if (paraValue) {
                    var tempCache = $filter('filter')($scope.tempData(moduleName), { VALUE: paraValue }, true);
                    if (tempCache && tempCache.length === 1) {
                        return tempCache[0].TEXT;
                    } else {
                        return paraValue;
                    }
                }
            };
            //Function load barcode danh muc mathang
            function loadBarcode() {
                $scope.barcode = [];
                if (!tempDataService.tempData('barcode')) {
                    service.getAllBarcode().then(function (successRes) {
                        if (successRes && successRes.status === 200 && successRes.data && successRes.data.Status && successRes.data.Data && successRes.data.Data.length > 0) {
                            tempDataService.putTempData('barcode', successRes.data.Data);
                            $scope.barcode = successRes.data.Data;
                        }
                    }, function (errorRes) {
                        console.log('errorRes', errorRes);
                    });
                } else {
                    $scope.barcode = tempDataService.tempData('barcode');
                }
            };
            loadBarcode();
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
            //bước kiểm tra dữ liệu có tồn tại trong danh mục chưa
            function kiemTraNhaCungCap(nhaCungCap) {
                var result = '';
                if (nhaCungCap && nhaCungCap != '') {
                    var tempCache = $filter('filter')($scope.nhaCungCap, { VALUE: nhaCungCap }, true);
                    if (tempCache && tempCache.length === 1) {
                        result = tempCache[0].VALUE;
                    }
                }
                return result;
            };

            function kiemTraLoaiHang(loaiHang) {
                var result = '';
                if (loaiHang && loaiHang != '') {
                    var tempCache = $filter('filter')($scope.loaiHang, { VALUE: loaiHang }, true);
                    if (tempCache && tempCache.length === 1) {
                        result = tempCache[0].VALUE;
                    }
                }
                return result;
            };

            function kiemTraNhomHang(nhomHang) {
                var result = '';
                if (nhomHang && nhomHang != '') {
                    var tempCache = $filter('filter')($scope.nhomHang, { VALUE: nhomHang }, true);
                    if (tempCache && tempCache.length === 1) {
                        result = tempCache[0].VALUE;
                    }
                }
                return result;
            };

            function kiemTraDonViTinh(donViTinh) {
                var result = '';
                if (donViTinh && donViTinh != '') {
                    var tempCache = $filter('filter')($scope.donViTinh, { VALUE: donViTinh }, true);
                    if (tempCache && tempCache.length === 1) {
                        result = tempCache[0].VALUE;
                    }
                }
                return result;
            };

            function kiemTraThue(thue) {
                var result = '';
                if (thue && thue != '') {
                    var tempCache = $filter('filter')($scope.thue, { VALUE: thue }, true);
                    if (tempCache && tempCache.length === 1) {
                        result = tempCache[0].VALUE;
                    }
                }
                return result;
            };

            //Change mã loại gender to MaHang
            $scope.changeMaLoai = function (item) {
                item.LIST_MANHOM = [];
                if (item.MALOAI) {
                    if ($scope.tempData('nhomHang') && $scope.tempData('nhomHang').length > 0) {
                        item.LIST_MANHOM = $filter('filter')($scope.tempData('nhomHang'), { PARENT: item.MALOAI }, true);
                    } else {
                        nhomHangService.getDataByMaLoai(item.MALOAI).then(function (successRes) {
                            if (successRes && successRes.status === 200 && successRes.data && successRes.data.Status && successRes.data.Data && successRes.data.Data.length > 0) {
                                item.LIST_MANHOM = successRes.data.Data;
                            }
                        }, function (errorRes) {
                            console.log('errorRes', errorRes);
                        });
                    }
                }
            };
            //end

            //Tính toán thuế
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

            $scope.selectedThueVao = function (item) {
                if (item.MATHUE_VAO && item.MATHUE_VAO != '') {
                    if ($scope.tempData('thue') && $scope.tempData('thue').length > 0) {
                        var dataTax = $filter('filter')($scope.tempData('thue'), { VALUE: item.MATHUE_VAO }, true);
                        if (dataTax && dataTax.length === 1) {
                            item.GIAMUA_VAT = item.GIAMUA * (dataTax[0].GIATRI / 100) + item.GIAMUA;
                        }
                    }
                    else {
                        thueService.getDataByMaThue(item.MATHUE_VAO).then(function (successRes) {
                            if (successRes && successRes.status === 200 && successRes.data && successRes.data.Status && successRes.data.Data) {
                                item.GIAMUA_VAT = item.GIAMUA * (successRes.data.Data.GIATRI / 100) + item.GIAMUA;
                            }
                        }, function (errorRes) {
                            console.log('errorRes', errorRes);
                        });
                    }
                }
            };

            $scope.selectedThueRa = function (item) {
                if (item.MATHUE_RA && item.MATHUE_RA != '') {
                    if ($scope.tempData('thue') && $scope.tempData('thue').length > 0) {
                        var dataTax = $filter('filter')($scope.tempData('thue'), { VALUE: item.MATHUE_RA }, true);
                        if (dataTax && dataTax.length === 1) {
                            item.GIABANBUON_VAT = Math.round(100 * ((item.GIABANBUON * (dataTax[0].GIATRI / 100)) + item.GIABANBUON)) / 100;
                            item.GIABANLE_VAT = Math.round(100 * ((item.GIABANLE * (dataTax[0].GIATRI / 100)) + item.GIABANLE)) / 100;
                        }
                    }
                    else {
                        thueService.getDataByMaThue(item.MATHUE_RA).then(function (successRes) {
                            if (successRes && successRes.status === 200 && successRes.data && successRes.data.Status && successRes.data.Data) {
                                item.GIABANBUON_VAT = Math.round(100 * ((item.GIABANBUON * (successRes.data.Data.GIATRI / 100)) + item.GIABANBUON)) / 100;
                                item.GIABANLE_VAT = Math.round(100 * ((item.GIABANLE * (successRes.data.Data.GIATRI / 100)) + item.GIABANLE)) / 100;
                            }
                        }, function (errorRes) {
                            console.log('errorRes', errorRes);
                        });
                    }
                }
            };

            $scope.changedGiaMua = function (item) {
                if (item.GIAMUA) {
                    item.GIAMUA_VAT = Math.round(100 * (item.GIAMUA * (getGiaTriVatVao(item.MATHUE_VAO) / 100) + item.GIAMUA)) / 100;
                    item.GIABANBUON = Math.round(100 * (item.GIAMUA * (item.TYLE_LAIBUON / 100) + item.GIAMUA)) / 100;
                    item.GIABANLE = Math.round(100 * (item.GIAMUA * (item.TYLE_LAILE / 100) + item.GIAMUA)) / 100;
                    item.GIABANLE_VAT = Math.round(100 * (item.GIABANLE * (getGiaTriVatVao(item.MATHUE_RA) / 100) + item.GIABANLE)) / 100;
                    item.GIABANBUON_VAT = Math.round(100 * (item.GIABANBUON * (getGiaTriVatVao(item.MATHUE_RA) / 100) + item.GIABANBUON)) / 100;
                }
            };

            $scope.changedGiaMuaVat = function (item) {
                if (item.GIAMUA_VAT) {
                    item.GIAMUA = Math.round(100 * (item.GIAMUA_VAT / ((getGiaTriVatVao(item.MATHUE_VAO) / 100) + 1))) / 100;
                    item.GIABANLE = Math.round(100 * (item.GIAMUA * (item.TYLE_LAILE / 100) + item.GIAMUA)) / 100;
                    item.GIABANBUON = Math.round(100 * (item.GIAMUA * (item.TYLE_LAIBUON / 100) + item.GIAMUA)) / 100;
                    item.GIABANLE_VAT = Math.round(100 * (item.GIABANLE * (getGiaTriVatVao(item.MATHUE_RA) / 100) + item.GIABANLE)) / 100;
                    item.GIABANBUON_VAT = Math.round(100 * (item.GIABANBUON * (getGiaTriVatVao(item.MATHUE_RA) / 100) + item.GIABANBUON)) / 100;
                }
            };

            $scope.changedTyLeLaiLe = function (item) {
                if (item.TYLE_LAILE) {
                    item.GIABANLE = Math.round(100 * (item.GIAMUA * (item.TYLE_LAILE / 100) + item.GIAMUA)) / 100;
                    item.GIABANLE_VAT = Math.round(100 * (item.GIABANLE * (getGiaTriVatVao(item.MATHUE_RA) / 100) + item.GIABANLE)) / 100;
                }
            };

            $scope.changedGiaBanLe = function (item) {
                if (item.GIABANLE) {
                    item.TYLE_LAILE = Math.round(100 * ((100 * (item.GIABANLE - item.GIAMUA)) / item.GIAMUA)) / 100;
                    if (parseInt(item.TYLE_LAILE) <= -100 || parseInt(item.TYLE_LAILE) > 1000) {
                        Lobibox.notify('warning', {
                            title: 'Kiểm tra thông tin',
                            msg: 'Kiểm tra lại tỷ lệ lãi lẻ (quá lớn hoặc quá nhỏ)',
                            delay: 4000
                        });
                    }
                    item.GIABANLE_VAT = Math.round(100 * (item.GIABANLE * (getGiaTriVatVao(item.MATHUE_RA) / 100) + item.GIABANLE)) / 100;
                }
            };

            $scope.changedTyLeLaiBuon = function (item) {
                if (item.TYLE_LAIBUON) {
                    item.GIABANBUON = Math.round(100 * ((item.GIAMUA * (item.TYLE_LAIBUON / 100)) + item.GIAMUA)) / 100;
                    item.GIABANBUON_VAT = Math.round(100 * ((item.GIABANBUON * (getGiaTriVatVao(item.MATHUE_RA) / 100)) + item.GIABANBUON)) / 100;
                }
            };

            $scope.changedGiaBanBuon = function (item) {
                if (item.GIABANBUON) {
                    item.TYLE_LAIBUON = Math.round(100 * ((100 * (item.GIABANBUON - item.GIAMUA)) / item.GIAMUA)) / 100;
                    if (parseInt(item.TYLE_LAIBUON) <= -100 || parseInt(item.TYLE_LAIBUON) > 1000) {
                        Lobibox.notify('warning', {
                            title: 'Kiểm tra thông tin',
                            msg: 'Kiểm tra lại tỷ lệ lãi buôn (quá lớn hoặc quá nhỏ)',
                            delay: 4000
                        });
                    }
                    item.GIABANBUON_VAT = Math.round(100 * (item.GIABANBUON * (getGiaTriVatVao(item.MATHUE_RA) / 100) + item.GIABANBUON)) / 100;
                }
            };

            $scope.changedGiaBanLeVat = function (item) {
                if (item.GIABANLE_VAT) {
                    item.GIABANLE = Math.round(100 * (item.GIABANLE_VAT / (1 + (getGiaTriVatVao(item.MATHUE_RA) / 100)))) / 100;
                    item.TYLE_LAILE = Math.round(100 * ((100 * (item.GIABANLE - item.GIAMUA)) / item.GIAMUA)) / 100;
                    if (parseInt(item.TYLE_LAILE) <= -100 || parseInt(item.TYLE_LAILE) > 1000) {
                        Lobibox.notify('warning', {
                            title: 'Kiểm tra thông tin',
                            msg: 'Kiểm tra lại tỷ lệ lãi lẻ (quá lớn hoặc quá nhỏ)',
                            delay: 4000
                        });
                    }
                }
            };

            $scope.changedGiaBanBuonVat = function (item) {
                if (item.GIABANBUON_VAT) {
                    item.GIABANBUON = Math.round(100 * (item.GIABANBUON_VAT / (1 + (getGiaTriVatVao(item.MATHUE_RA) / 100)))) / 100;
                    item.TYLE_LAIBUON = Math.round(100 * ((100 * (item.GIABANBUON - item.GIAMUA)) / item.GIAMUA)) / 100;
                    if (parseInt(item.TYLE_LAIBUON) <= -100 || parseInt(item.TYLE_LAIBUON) > 1000) {
                        Lobibox.notify('warning', {
                            title: 'Kiểm tra thông tin',
                            msg: 'Kiểm tra lại tỷ lệ lãi buôn (quá lớn hoặc quá nhỏ)',
                            delay: 4000
                        });
                    }
                }
            };

            function TinhToanGiaMatHang(obj) {
                obj.GIAMUA_VAT = Math.round(100 * (parseFloat(obj.GIAMUA) * (getGiaTriVatVao(obj.MATHUE_VAO) / 100) + parseFloat(obj.GIAMUA))) / 100;
                obj.GIABANLE_VAT = Math.round(100 * (parseFloat(obj.GIABANLE) * (getGiaTriVatVao(obj.MATHUE_RA) / 100) + parseFloat(obj.GIABANLE))) / 100;
                obj.GIABANBUON_VAT = Math.round(100 * (parseFloat(obj.GIABANBUON) * (getGiaTriVatVao(obj.MATHUE_RA) / 100) + parseFloat(obj.GIABANBUON))) / 100;
                obj.TYLE_LAILE = Math.round(100 * ((100 * (obj.GIABANLE - obj.GIAMUA)) / obj.GIAMUA)) / 100;
                obj.TYLE_LAIBUON = Math.round(100 * ((100 * (obj.GIABANBUON - obj.GIAMUA)) / obj.GIAMUA)) / 100;
            };

            function KiemTraTrungBarcode(barcode) {
                var existsSample = false;
                if ($scope.tempData('barcode') && $scope.tempData('barcode').length > 0) {
                    angular.forEach($scope.tempData('barcode'), function (v, k) {
                        if (v.TEXT.includes(';')) {
                            var arrayBarcode = v.TEXT.split(';');
                            if (arrayBarcode && arrayBarcode.length > 0) {
                                angular.forEach(arrayBarcode, function (value, index) {
                                    if (value.trim() === barcode.trim() && value.length === barcode.trim().length) {
                                        existsSample = true;
                                    }
                                });
                            }
                        } else if (v.TEXT.includes(',')) {
                            var arrayBarcode = v.TEXT.split(',');
                            if (arrayBarcode && arrayBarcode.length > 0) {
                                angular.forEach(arrayBarcode, function (value, index) {
                                    if (value.trim() === barcode.trim() && value.length === barcode.trim().length) {
                                        existsSample = true;
                                    }
                                });
                            }
                        } else {
                            if (v.TEXT.trim() === barcode.trim() && v.TEXT.length === barcode.trim().length) {
                                existsSample = true;
                            }
                        }
                    });
                }
                return existsSample;
            };
            var maTrungBarcodeMessage = '';
            maTrungBarcodeMessage = 'Trùng barcode giữa dòng ';
            function KiemTraTrungBarcodeTrongFileExcel(listObj) {
                var existsSample = false;
                if (listObj && listObj.length > 0) {
                    for (var i = 0; i < listObj.length; i++) {
                        for (var j = i + 1; j < listObj.length - 1; j++) {
                            if (listObj[i].BARCODE && listObj[i].BARCODE != ''
                                && listObj[j].BARCODE && listObj[j].BARCODE != ''
                                && listObj[i].BARCODE.trim() === listObj[j].BARCODE.trim()
                                && listObj[i].BARCODE.trim().length == listObj[j].BARCODE.trim().length) {
                                existsSample = true;
                                maTrungBarcodeMessage += (i+2) + ' và dòng ' + (j+2) + '; ';
                            }
                        }
                    }
                }
                return existsSample;
            };
            //end
            var message = '';
            if ($scope.target && $scope.target.length > 0) {
                angular.forEach($scope.target, function (v, k) {
                    var obj = {
                        MAHANG: '',
                        TENHANG: '',
                        MANHACUNGCAP: '',
                        MALOAI: '',
                        MANHOM: '',
                        MATHUE_VAO: '',
                        MATHUE_RA: '',
                        MADONVITINH: '',
                        BARCODE: '',
                        TRANGTHAI: 10,
                        TYLE_LAILE: 0,
                        TYLE_LAIBUON: 0,
                        GIAMUA: 0,
                        GIAMUA_VAT: 0,
                        GIABANLE: 0,
                        GIABANLE_VAT: 0,
                        GIABANBUON: 0,
                        GIABANBUON_VAT: 0,
                        IS_ERROR: false
                    };
                    if (!v['NHÀ CUNG CẤP']) obj.MANHACUNGCAP = '';
                    else obj.MANHACUNGCAP = v['NHÀ CUNG CẤP'].trim();
                    if (!v['TÊN SẢN PHẨM']) obj.TENHANG = '';
                    else obj.TENHANG = v['TÊN SẢN PHẨM'].trim();
                    if (!v['LOẠI']) obj.MALOAI = '';
                    else obj.MALOAI = v['LOẠI'].trim();
                    if (!v['NHÓM']) obj.MANHOM = '';
                    else obj.MANHOM = v['NHÓM'].trim();
                    if (!v['ĐƠN VỊ TÍNH']) obj.MADONVITINH = '';
                    else obj.MADONVITINH = v['ĐƠN VỊ TÍNH'].trim();
                    if (!v['THUẾ VÀO']) obj.MATHUE_VAO = '';
                    else obj.MATHUE_VAO = v['THUẾ VÀO'].trim();
                    if (!v['THUẾ RA']) obj.MATHUE_RA = '';
                    else obj.MATHUE_RA = v['THUẾ RA'].trim();
                    if (!v['BARCODE']) obj.BARCODE = '';
                    else obj.BARCODE = v['BARCODE'].trim();
                    if (!v['GIÁ MUA']) obj.GIAMUA = 0;
                    else {
                        var giaMua = v['GIÁ MUA'].trim();
                        if (giaMua.includes(',')) giaMua = giaMua.replace(',', '');
                        obj.GIAMUA = Math.round(100 * (parseFloat(giaMua))) / 100;
                    }
                    if (!v['GIÁ BÁN LẺ']) obj.GIABANLE = 0;
                    else {
                        var giaBanLe = v['GIÁ BÁN LẺ'].trim();
                        if (giaBanLe.includes(',')) giaBanLe = giaBanLe.replace(',', '');
                        obj.GIABANLE = Math.round(100 * (parseFloat(giaBanLe))) / 100;
                    }
                    if (!v['GIÁ BÁN BUÔN']) obj.GIABANBUON = 0;
                    else {
                        var giaBanBuon = v['GIÁ BÁN BUÔN'].trim();
                        if (giaBanBuon.includes(',')) giaBanBuon = giaBanBuon.replace(',', '');
                        obj.GIABANBUON = Math.round(100 * (parseFloat(giaBanBuon))) / 100;
                    }
                    obj.LIST_MANHOM = [];
                    if (obj.MALOAI) {
                        if ($scope.tempData('nhomHang') && $scope.tempData('nhomHang').length > 0) {
                            obj.LIST_MANHOM = $filter('filter')($scope.tempData('nhomHang'), { PARENT: obj.MALOAI }, true);
                        }
                    }

                    var checkNhaCungCap = kiemTraNhaCungCap(obj.MANHACUNGCAP);
                    if (checkNhaCungCap === '') {
                        obj.IS_ERROR = true;
                        message = 'Lỗi: Nhà cung cấp,';
                    }

                    var checkLoaiHang = kiemTraLoaiHang(obj.MALOAI);
                    if (checkLoaiHang === '') {
                        obj.IS_ERROR = true;
                        message += 'Loại hàng,';
                    }

                    var checkNhomHang = kiemTraNhomHang(obj.MANHOM);
                    if (checkNhomHang === '') {
                        obj.IS_ERROR = true;
                        message += 'Nhóm hàng,';
                    }

                    var checkThue_Vao = kiemTraThue(obj.MATHUE_VAO);
                    if (checkThue_Vao === '') {
                        obj.IS_ERROR = true;
                        message += 'Thuế vào,';
                    }

                    var checkThue_Ra = kiemTraThue(obj.MATHUE_RA);
                    if (checkThue_Ra === '') {
                        obj.IS_ERROR = true;
                        message += 'Thuế ra,';
                    }

                    if (obj.BARCODE && obj.BARCODE != '') {
                        var isBarcodeExists = KiemTraTrungBarcode(obj.BARCODE);
                        if (isBarcodeExists) obj.IS_ERROR = true;
                    }

                    if (!obj.IS_ERROR) {
                        TinhToanGiaMatHang(obj);
                        $scope.data.push(obj);
                    } else {
                        $scope.dataError.push(obj);
                    }
                });

                if (message && message !== '') {
                    Lobibox.notify('error', {
                        title: 'Lỗi thông tin đầu vào',
                        msg: message,
                        delay: 5000
                    });
                }
                $scope.insertExcelToDb = function () {
                    if ($scope.data.length > 0) {
                        if (KiemTraTrungBarcodeTrongFileExcel($scope.data)) {
                            Lobibox.notify('error', {
                                title: 'Trùng barcode',
                                msg: maTrungBarcodeMessage + '! Kiểm tra file Excel đầu vào',
                                delay: 5000
                            });
                        } else {
                            service.postExcelData($scope.data).then(function (successRes) {
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
                    } else {
                        Lobibox.notify('warning', {
                            title: 'Thông tin đầu vào không đúng',
                            msg: 'Không tìm thấy danh sách mặt hàng từ Excel',
                            delay: 4000
                        });
                    }
                };
            };
            $scope.cancel = function () {
                $uibModalInstance.close();
            };
        }]);
    return app;
});