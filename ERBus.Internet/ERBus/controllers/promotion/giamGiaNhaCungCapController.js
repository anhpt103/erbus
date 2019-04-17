﻿define(['ui-bootstrap', 'controllers/catalog/khoHangController', 'controllers/catalog/nhaCungCapController'], function () {
    'use strict';
    var app = angular.module('giamGiaNhaCungCapModule', ['ui.bootstrap', 'khoHangModule', 'nhaCungCapModule']);
    app.factory('giamGiaNhaCungCapService', ['$http', 'configService', function ($http, configService) {
        var serviceUrl = configService.rootUrlWebApi + '/Promotion/GiamGiaNhaCungCap';
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
    app.controller('GiamGiaNhaCungCap_Ctrl', ['$scope', '$http', 'configService', 'giamGiaNhaCungCapService', 'tempDataService', '$filter', '$uibModal', '$log', 'securityService', 'khoHangService','nhaCungCapService',
        function ($scope, $http, configService, service, tempDataService, $filter, $uibModal, $log, securityService, khoHangService, nhaCungCapService) {
            $scope.config = angular.copy(configService);
            $scope.paged = angular.copy(configService.pageDefault);
            $scope.filtered = angular.copy(configService.filterDefault);
            $scope.tempData = tempDataService.tempData;
            $scope.title = function () { return 'Khuyến mãi giảm giá theo nhà cung cấp' };
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
            //Function load data catalog NhomHang
            function loadDataNhomHang() {
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
            loadDataNhomHang();
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
                securityService.getAccessList('GiamGiaNhaCungCap').then(function (successRes) {
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
                    windowClass: 'promotion-window',
                    templateUrl: configService.buildUrl('promotion/GiamGiaNhaCungCap', 'create'),
                    controller: 'giamGiaNhaCungCapCreate_Ctrl',
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
                    templateUrl: configService.buildUrl('promotion/GiamGiaNhaCungCap', 'detail'),
                    controller: 'giamGiaNhaCungCapDetail_Ctrl',
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
                    templateUrl: configService.buildUrl('promotion/GiamGiaNhaCungCap', 'edit'),
                    controller: 'giamGiaNhaCungCapEdit_Ctrl',
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
                    templateUrl: configService.buildUrl('promotion/GiamGiaNhaCungCap', 'delete'),
                    controller: 'giamGiaNhaCungCapDelete_Ctrl',
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

    app.controller('giamGiaNhaCungCapCreate_Ctrl', ['$scope', '$uibModalInstance', '$http', 'configService', 'giamGiaNhaCungCapService', 'tempDataService', '$filter', '$uibModal', '$log', 'khoHangService','userService','nhaCungCapService',
    function ($scope, $uibModalInstance, $http, configService, service, tempDataService, $filter, $uibModal, $log, khoHangService, userService, nhaCungCapService) {
        $scope.config = angular.copy(configService);
        $scope.tempData = tempDataService.tempData;
        $scope.paged = angular.copy(configService.pageDefault);
        var currentUser = userService.GetCurrentUser();
        var unitCode = currentUser.unitCode;
        $scope.title = function () { return 'Thêm chương trình khuyến mãi giảm giá theo nhà cung cấp'; };
        $scope.target = {
            TUNGAY: new Date(),
            DENNGAY: new Date(),
            TRANGTHAI: 0,
            TUGIO: '00:00',
            DENGIO: '23:59',
            DataDetails: [],
        };
        $scope.addItem = {
            MANHACUNGCAP: '',
            TENNHACUNGCAP: '',
            SOLUONG: 1,
            GIATRI_KHUYENMAI: 0,
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
      
        //Function load data catalog NhomHang
        function loadDataNhomHang() {
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
        loadDataNhomHang();
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
            focus('_maNhaCungCapAddItem');
            document.getElementById('_maNhaCungCapAddItem').focus();
            document.getElementById('_maNhaCungCapAddItem').select();
        };
        //sự kiện click ESC exit modal
        document.addEventListener('keyup', function (e) {
            if (e.keyCode == 27) {
                errorFocusMaHang();
            }
        });
        //end 
        $scope.selectMaNhaCungCap = function (maLoai) {
            if (maLoai) {
                var tempCache = $filter('filter')($scope.nhaCungCap, { VALUE: maLoai }, true);
                if (tempCache && tempCache.length === 1) {
                    $scope.addItem.TENNHACUNGCAP = tempCache[0].DESCRIPTION;
                }
            }
            document.getElementById('_giaTriKhuyenMaiAddItem').focus();
            document.getElementById('_giaTriKhuyenMaiAddItem').select();
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
            if ($scope.addItem && $scope.addItem.MANHACUNGCAP !== '') {
                var exist = $filter('filter')($scope.target.DataDetails, { MANHACUNGCAP: $scope.addItem.MANHACUNGCAP }, true);
                if (exist && exist.length == 1) {
                    exist[0].SOLUONG = 1;
                    exist[0].GIATRI_KHUYENMAI = $scope.addItem.GIATRI_KHUYENMAI;
                    Lobibox.notify('success', {
                        title: 'Thông báo',
                        width: 400,
                        msg: "Đã tồn tại nhà cung cấp " + exist[0].MANHACUNGCAP + ".",
                        delay: 1500
                    });
                }
                else {
                    $scope.target.DataDetails.push($scope.addItem);
                }
                $scope.pageChanged();
                $scope.addItem = {
                    MANHACUNGCAP: '',
                    TENNHACUNGCAP: '',
                    SOLUONG: 1,
                    GIATRI_KHUYENMAI: 0,
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
                if ($scope.addItem.MANHACUNGCAP && $scope.addItem.MANHACUNGCAP.length > 0 && $scope.addItem.TENNHACUNGCAP && $scope.addItem.TENNHACUNGCAP.length > 0 && $scope.addItem.SOLUONG && $scope.addItem.SOLUONG != 0) {
                    Lobibox.notify('default', {
                        title: 'Nhắc nhở',
                        msg: 'Dòng nhà cung cấp [' + $scope.addItem.MANHACUNGCAP + '] chưa được thêm mới xuống danh sách, Bạn hãy thêm hoặc xóa trước khi lưu',
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


    app.controller('giamGiaNhaCungCapDetail_Ctrl', ['$scope', '$uibModalInstance', '$http', 'configService', 'giamGiaNhaCungCapService', 'targetData', 'tempDataService', '$filter', '$uibModal', '$log',
        function ($scope, $uibModalInstance, $http, configService, service, targetData, tempDataService, $filter, $uibModal, $log) {
            $scope.config = angular.copy(configService);
            $scope.paged = angular.copy(configService.pageDefault);
            $scope.tempData = tempDataService.tempData;
            $scope.target = {};
            $scope.target = angular.copy(targetData);
            $scope.title = function () { return 'Thông tin chương trình khuyến mãi giảm giá theo nhà cung cấp [' + targetData.MA_KHUYENMAI + ']'; };
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
                focus('_maNhaCungCapAddItem');
                document.getElementById('_maNhaCungCapAddItem').focus();
                document.getElementById('_maNhaCungCapAddItem').select();
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
                }, true);
            };
            listenerDataDetails();
            function filterData() {
                service.getDetails($scope.target.ID).then(function (sucessRes) {
                    if (sucessRes && sucessRes.status === 200 && sucessRes.data && sucessRes.data.Status && sucessRes.data.Data) {
                        $scope.target = sucessRes.data.Data;
                        $scope.target.TUNGAY = new Date($scope.target.TUNGAY);
                        $scope.target.DENNGAY = new Date($scope.target.DENNGAY);
                        $scope.pageChanged();
                    }
                });
            };
            filterData();
            $scope.cancel = function () {
                $uibModalInstance.close();
            };
        }]);

    app.controller('giamGiaNhaCungCapEdit_Ctrl', ['$scope', '$uibModalInstance', '$http', 'configService', 'giamGiaNhaCungCapService', 'targetData', 'tempDataService', '$filter', '$uibModal', '$log','userService','nhaCungCapService','khoHangService',
        function ($scope, $uibModalInstance, $http, configService, service, targetData, tempDataService, $filter, $uibModal, $log, userService, nhaCungCapService, khoHangService) {
            $scope.config = angular.copy(configService);
            $scope.paged = angular.copy(configService.pageDefault);
            $scope.tempData = tempDataService.tempData;
            var currentUser = userService.GetCurrentUser();
            var unitCode = currentUser.unitCode;
            $scope.target = {};
            $scope.target = angular.copy(targetData);
            $scope.addItem = {
                MANHACUNGCAP: '',
                TENNHACUNGCAP: '',
                SOLUONG: 1,
                GIATRI_KHUYENMAI: 0,
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

            //Function load data catalog NhomHang
            function loadDataNhomHang() {
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
            loadDataNhomHang();
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
            function caculatorThanhTien(addItem) {
                if (addItem && addItem.MAHANG) {
                    addItem.THANHTIEN = Math.round(100 * ((addItem.SOLUONG * addItem.GIABANLE_VAT) - (addItem.GIATRI_KHUYENMAI * (addItem.SOLUONG * addItem.GIABANLE_VAT)) / 100)) / 100;
                    addItem.INDEX = $scope.target.DataDetails.length + 1;
                }
            };

            function errorFocusMaHang() {
                focus('_maNhaCungCapAddItem');
                document.getElementById('_maNhaCungCapAddItem').focus();
                document.getElementById('_maNhaCungCapAddItem').select();
            };
            //sự kiện click ESC exit modal
            document.addEventListener('keyup', function (e) {
                if (e.keyCode == 27) {
                    errorFocusMaHang();
                }
            });
            //end 
            $scope.selectMaNhaCungCap = function (maLoai) {
                if (maLoai) {
                    var tempCache = $filter('filter')($scope.nhaCungCap, { VALUE: maLoai }, true);
                    if (tempCache && tempCache.length === 1) {
                        $scope.addItem.TENNHACUNGCAP = tempCache[0].DESCRIPTION;
                    }
                }
                document.getElementById('_giaTriKhuyenMaiAddItem').focus();
                document.getElementById('_giaTriKhuyenMaiAddItem').select();
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
                if ($scope.addItem && $scope.addItem.MANHACUNGCAP !== '') {
                    var exist = $filter('filter')($scope.target.DataDetails, { MANHACUNGCAP: $scope.addItem.MANHACUNGCAP }, true);
                    if (exist && exist.length == 1) {
                        exist[0].SOLUONG = 1;
                        exist[0].GIATRI_KHUYENMAI = $scope.addItem.GIATRI_KHUYENMAI;
                        Lobibox.notify('success', {
                            title: 'Thông báo',
                            width: 400,
                            msg: "Đã tồn tại nhà cung cấp " + exist[0].MANHACUNGCAP + ".",
                            delay: 1500
                        });
                    }
                    else {
                        $scope.target.DataDetails.push($scope.addItem);
                    }
                    $scope.pageChanged();
                    $scope.addItem = {
                        MANHACUNGCAP: '',
                        TENNHACUNGCAP: '',
                        SOLUONG: 1,
                        GIATRI_KHUYENMAI: 0,
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
                }, true);
            };
            listenerDataDetails();

            function filterData() {
                service.getDetails($scope.target.ID).then(function (sucessRes) {
                    if (sucessRes && sucessRes.status === 200 && sucessRes.data && sucessRes.data.Status && sucessRes.data.Data) {
                        $scope.target = sucessRes.data.Data;
                        $scope.target.TUNGAY = new Date($scope.target.TUNGAY);
                        $scope.target.DENNGAY = new Date($scope.target.DENNGAY);
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
                    if ($scope.addItem.MANHACUNGCAP && $scope.addItem.MANHACUNGCAP.length > 0 && $scope.addItem.TENNHACUNGCAP && $scope.addItem.TENNHACUNGCAP.length > 0 && $scope.addItem.SOLUONG && $scope.addItem.SOLUONG != 0) {
                        Lobibox.notify('default', {
                            title: 'Nhắc nhở',
                            msg: 'Dòng nhà cung cấp [' + $scope.addItem.MANHACUNGCAP + '] chưa được thêm mới xuống danh sách, Bạn hãy thêm hoặc xóa trước khi lưu',
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

    app.controller('giamGiaNhaCungCapDelete_Ctrl', ['$scope', '$uibModalInstance', '$http', 'configService', 'giamGiaNhaCungCapService', 'targetData', 'tempDataService', '$filter', '$uibModal', '$log',
       function ($scope, $uibModalInstance, $http, configService, service, targetData, tempDataService, $filter, $uibModal, $log) {
           $scope.config = angular.copy(configService);
           $scope.tempData = tempDataService.tempData;
           $scope.target = {};
           $scope.target = angular.copy(targetData);;
           $scope.title = function () { return 'Xóa chương trình khuyến mãi giảm giá theo nhà cung cấp [' + targetData.MA_KHUYENMAI + ']'; };
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