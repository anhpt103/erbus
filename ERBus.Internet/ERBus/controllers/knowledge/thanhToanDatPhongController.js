define(['ui-bootstrap', 'controllers/catalog/loaiPhongController', 'controllers/catalog/phongController', 'controllers/knowledge/datPhongController', 'controllers/authorize/thamSoHeThongController'], function () {
    'use strict';
    var app = angular.module('thanhToanDatPhongModule', ['ui.bootstrap', 'loaiPhongModule', 'phongModule', 'datPhongModule', 'thamSoHeThongModule']);
    app.factory('thanhToanDatPhongService', ['$http', 'configService', function ($http, configService) {
        var serviceUrl = configService.rootUrlWebApi + '/Knowledge/ThanhToanDatPhong';
        var selectedData = [];
        var result = {
            postQuery: function (data) {
                return $http.post(serviceUrl + '/PostQuery', data);
            },
            getMerchandiseInBundleGoods: function (data) {
                return $http.post(serviceUrl + '/GetMerchandiseInBundleGoods', data);
            },
            post: function (data) {
                return $http.post(serviceUrl + '/Post', data);
            }
        }
        return result;
    }]);
    /* controller list */
    app.controller('ThanhToanDatPhong_Ctrl', ['$scope', '$http', 'configService', 'thanhToanDatPhongService', 'tempDataService', '$filter', '$uibModal', '$log', 'securityService','phongService','datPhongService','thamSoHeThongService','keyCodes','$sce','loaiPhongService',
        function ($scope, $http, configService, service, tempDataService, $filter, $uibModal, $log, securityService, phongService, datPhongService, thamSoHeThongService, keyCodes, $sce, loaiPhongService) {
            $scope.keys = keyCodes;
            $scope.modalOpen = false;
            $scope.config = angular.copy(configService);
            $scope.paged = angular.copy(configService.pageDefault);
            $scope.filtered = angular.copy(configService.filterDefault);
            $scope.tempData = tempDataService.tempData;
            $scope.title = function () { return 'Thanh toán đặt phòng' };
            $scope.data = {
                TONGSOLUONG: 0,
                TONGTIEN_THANHTOAN: 0,
                TONGTIEN_BANGCHU: '',
                DtoDetails: []
            };
            $scope.setPage = function (pageNo) {
                $scope.paged.CurrentPage = pageNo;
                filterData();
            };
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

            //Function load data catalog LoaiHang
            function loadDataLoaiPhong() {
                $scope.loaiPhong = [];
                if (!tempDataService.tempData('loaiPhong')) {
                    loaiPhongService.getAllData().then(function (successRes) {
                        if (successRes && successRes.status === 200 && successRes.data && successRes.data.Status && successRes.data.Data && successRes.data.Data.length > 0) {
                            tempDataService.putTempData('loaiPhong', successRes.data.Data);
                            $scope.loaiPhong = successRes.data.Data;
                        }
                    }, function (errorRes) {
                        console.log('errorRes', errorRes);
                    });
                } else {
                    $scope.loaiPhong = tempDataService.tempData('loaiPhong');
                }
            };
            loadDataLoaiPhong();
            //end

            //load tham số các mã được phép cài đặt giá từ tham số hệ thống
            var SPECIAL_MERCHANDISE = '';
            thamSoHeThongService.getDataByMaThamSo().then(function (successRes) {
                if (successRes && successRes.status === 200 && successRes.data && successRes.data.Status && successRes.data.Data) {
                    angular.forEach(successRes.data.Data, function (v, k) {
                        if (v.MA_THAMSO === 'SPECIAL_MER' && v.GIATRI_SO === 10) {
                            SPECIAL_MERCHANDISE = v.GIATRI_CHU;
                        }
                    });
                }
            });

            //Function load data catalog Phong
            function loadDataPhong() {
                $scope.phong = [];
                if (!tempDataService.tempData('phong')) {
                    phongService.getAllData().then(function (successRes) {
                        if (successRes && successRes.status === 200 && successRes.data && successRes.data.Status && successRes.data.Data && successRes.data.Data.length > 0) {
                            tempDataService.putTempData('phong', successRes.data.Data);
                            $scope.phong = successRes.data.Data;
                        }
                    }, function (errorRes) {
                        console.log('errorRes', errorRes);
                    });
                } else {
                    $scope.phong = tempDataService.tempData('phong');
                }
            };
            loadDataPhong();
            //end

            function filterData() {
                datPhongService.getListBookingRoom().then(function (successRes) {
                    if (successRes && successRes.status === 200 && successRes.data && successRes.data.Status && successRes.data.Data && successRes.data.Data.length > 0) {
                        $scope.listBookingRoom = successRes.data.Data;
                    }
                    else {
                        $scope.listBookingRoom = [];
                    }
                });
            };
            //check authorize
            function loadAccessList() {
                securityService.getAccessList('ThanhToanDatPhong').then(function (successRes) {
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

            function SumQuanlity(list, field) {
                var sum = 0;
                if (list && list.length > 0) {
                    angular.forEach(list, function (v, k) {
                        sum += parseFloat(v[field]);
                    });
                    if (field === 'THANHTIEN' && $scope.data.TIEN_GIOHAT > 0) sum = sum + $scope.data.TIEN_GIOHAT;
                }
                return sum;
            };

            $scope.plusQuanlity = function (item) {
                if (!item.SOLUONG) item.SOLUONG = 0;
                item.SOLUONG = parseFloat(item.SOLUONG) + 1;
                item.THANHTIEN = item.SOLUONG * item.GIABANLE_VAT;
                $scope.data.TONGSOLUONG = SumQuanlity($scope.data.DtoDetails, 'SOLUONG');
                $scope.data.TONGTIEN_THANHTOAN = SumQuanlity($scope.data.DtoDetails, 'THANHTIEN');
            };

            $scope.minusQuanlity = function (item) {
                if (!item.SOLUONG) item.SOLUONG = 0;
                item.SOLUONG = parseFloat(item.SOLUONG) - 1;
                if (item.SOLUONG < 0) item.SOLUONG = 0;
                item.THANHTIEN = parseFloat(item.SOLUONG) * item.GIABANLE_VAT;
                $scope.data.TONGSOLUONG = SumQuanlity($scope.data.DtoDetails, 'SOLUONG');
                $scope.data.TONGTIEN_THANHTOAN = SumQuanlity($scope.data.DtoDetails, 'THANHTIEN');
            };

            $scope.changeSoLuong = function (item) {
                if (!item.SOLUONG) item.SOLUONG = 0;
                if (item.SOLUONG < 0) item.SOLUONG = 0;
                item.SOLUONG = parseFloat(item.SOLUONG);
                item.THANHTIEN = item.SOLUONG * item.GIABANLE_VAT;
                $scope.data.TONGSOLUONG = SumQuanlity($scope.data.DtoDetails, 'SOLUONG');
                $scope.data.TONGTIEN_THANHTOAN = SumQuanlity($scope.data.DtoDetails, 'THANHTIEN');
            };

            $scope.changeGiaBanLeVat = function (item) {
                if (!item.SOLUONG) item.SOLUONG = 0;
                if (!item.GIABANLE_VAT) item.GIABANLE_VAT = 0;
                item.SOLUONG = parseFloat(item.SOLUONG);
                item.GIABANLE_VAT = parseFloat(item.GIABANLE_VAT);
                item.THANHTIEN = item.SOLUONG * item.GIABANLE_VAT;
                $scope.data.TONGSOLUONG = SumQuanlity($scope.data.DtoDetails, 'SOLUONG');
                $scope.data.TONGTIEN_THANHTOAN = SumQuanlity($scope.data.DtoDetails, 'THANHTIEN');
            };

            function getTime() {
                var date = new Date();
                var h = date.getHours(); // 0 - 23
                var m = date.getMinutes(); // 0 - 59
                var s = date.getSeconds(); // 0 - 59
                var session = "AM";
                if (h == 0) {
                    h = 12;
                }
                if (h > 12) {
                    h = h - 12;
                    session = "PM";
                }
                h = (h < 10) ? "0" + h : h;
                m = (m < 10) ? "0" + m : m;
                s = (s < 10) ? "0" + s : s;
                var time = h + ":" + m + ":" + s + " " + session;
                return time;
            };

            const monthNames = ["January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"];
            function toHHMMSS(time) {
                var sec_num = parseInt(time, 10); // don't forget the second param
                var hours = Math.floor(sec_num / 3600);
                var minutes = Math.floor((sec_num - (hours * 3600)) / 60);
                var seconds = sec_num - (hours * 3600) - (minutes * 60);
                if (hours < 10) { hours = "0" + hours; }
                if (minutes < 10) { minutes = "0" + minutes; }
                if (seconds < 10) { seconds = "0" + seconds; }
                return hours + ':' + minutes + ':' + seconds;
            };

            //convert time to minus
            function toMinus(time) {
                var minutes = Math.floor(time / 60);
                $scope.data.THOIGIAN_SUDUNG = minutes;
                return minutes;
            };
            var action;
            function caculateCountHour() {
                var hourSingle = $filter('filter')($scope.data.DtoDetails, { IS_HOUR_SINGLE_MERCHANDISE: true }, true);
                if (hourSingle && hourSingle.length === 1) {
                    $scope.data.NGAY_DATPHONG = new Date($scope.data.NGAY_DATPHONG);
                    if ($scope.data.THOIGIAN_DATPHONG && $scope.data.NGAY_DATPHONG) {
                        var time = monthNames[$scope.data.NGAY_DATPHONG.getMonth()] + ' ' + $scope.data.NGAY_DATPHONG.getDate() + ',' + ' ' + $scope.data.NGAY_DATPHONG.getFullYear() + ' ' + $scope.data.THOIGIAN_DATPHONG;
                        var dateTimeBooking = new Date(time);
                        hourSingle[0].textThoiGian = $sce.trustAsHtml('<input type="text" id = "' + hourSingle[0].MAHANG + '"' + ' readonly style="height: 30.5px;text-align: right;" class="form-control input-number">');
                        if (document.getElementById(hourSingle[0].MAHANG) !== null) {
                            var timeRuning = (new Date().getTime() - dateTimeBooking.getTime()) / 1000;
                            $("#" + hourSingle[0].MAHANG).val(toHHMMSS(timeRuning));
                        }

                        hourSingle[0].textThanhTien = $sce.trustAsHtml('<input type="text" awnum="number" id = "' + hourSingle[0].MAHANG + '_' + hourSingle[0].SAPXEP + '"' + ' style="height: 30.5px;text-align: right;width: 100%;" />');
                        if (document.getElementById(hourSingle[0].MAHANG + '_' + hourSingle[0].SAPXEP) !== null) {
                            $scope.data.TIEN_GIOHAT = (Math.floor(hourSingle[0].GIABANLE_VAT / $scope.data.DONVI_THOIGIAN_TINHTIEN) * toMinus(timeRuning));
                            $("#" + hourSingle[0].MAHANG + '_' + hourSingle[0].SAPXEP).val($scope.data.TIEN_GIOHAT);

                        }

                        $scope.trustSOGIO = $sce.trustAsHtml('<input type="text" id="SOGIO" class="form-control" readonly style="height: 45px;border-bottom: 1px solid #f5f5f5;background: #fff;font-size: 14px;color: #808080;padding: 12px 40px;padding-right: 10px;" />');
                        if (document.getElementById('SOGIO') !== null) {
                            $("#SOGIO").val('Số giờ vào: ' + toHHMMSS(timeRuning));
                        }

                        $scope.trustTONGTIEN_THANHTOAN = $sce.trustAsHtml('<input type="text" awnum="number" id="TONGTIEN_THANHTOAN" class="form-control" readonly style="height: 45px;border-bottom: 1px solid #f5f5f5;background: #fff;font-size: 14px;color: #808080;padding: 12px 40px;padding-right: 10px;" />');
                        if (document.getElementById('TONGTIEN_THANHTOAN') !== null) {
                            var tongTienThanhToan = SumQuanlity($scope.data.DtoDetails, 'THANHTIEN');
                            $scope.data.TONGTIEN_THANHTOAN = tongTienThanhToan;
                            $("#TONGTIEN_THANHTOAN").val('Tổng tiền thanh toán: ' + tongTienThanhToan);
                        }
                    }
                }
                if (document.getElementById("_thoiGianHienTai") !== null) {
                    $("#_thoiGianHienTai").val('Thời gian hiện tại: ' + getTime());
                }
                action = setTimeout(caculateCountHour, 1000);
            };

            $scope.dischargeRoom = function (item) {
                if (item) {
                    service.getMerchandiseInBundleGoods(item).then(function (response) {
                        if (response && response.status === 200 && response.data && response.data.Status && response.data.Data) {
                            $scope.data = response.data.Data;
                            $scope.data.MALOAIPHONG = item.MALOAIPHONG;
                            $scope.data.TANG = item.TANG;
                            $scope.data.TENLOAIPHONG = item.TENLOAIPHONG;
                            $scope.data.TENPHONG = item.TENPHONG;
                            if ($scope.data && $scope.data.DtoDetails.length > 0) {
                                angular.forEach($scope.data.DtoDetails, function (v, k) {
                                    //mặc định số lượng ban đầu là 0 để người sử dụng đánh
                                    v.SOLUONG = 0;
                                    v.THANHTIEN = v.SOLUONG * v.GIABANLE_VAT;
                                    v.SAPXEP = k;
                                    if (SPECIAL_MERCHANDISE.includes(v.MAHANG)) v.IS_EDIT_VALUE = true
                                    if (v.MAHANG === $scope.data.MAHANG) {
                                        v.IS_HOUR_SINGLE_MERCHANDISE = true;
                                        caculateCountHour();
                                    }
                                });
                                $scope.data.TONGSOLUONG = SumQuanlity($scope.data.DtoDetails, 'SOLUONG');
                                $scope.data.TONGTIEN_THANHTOAN = SumQuanlity($scope.data.DtoDetails, 'THANHTIEN');
                            }
                        }
                    });
                }
            };

            $scope.payRoom = function () {
                if ($scope.data && $scope.data.MAPHONG && !$scope.modalOpen) {
                    var modalInstance = $uibModal.open({
                        backdrop: 'static',
                        keyboard  : false,
                        animation: true,
                        windowClass: 'modal-delete',
                        templateUrl: configService.buildUrl('knowledge/ThanhToanDatPhong', 'pay'),
                        controller: 'pay_Ctrl',
                        resolve: {
                            targetData: function () {
                                return $scope.data;
                            }
                        }
                    });
                    modalInstance.opened.then(function () {
                        $scope.modalOpen = true;
                    });
                    modalInstance.result.then(function (refundData) {
                        $scope.modalOpen = false;
                        if (refundData === false) {
                            //trả về false thì tính thời gian tiếp
                            caculateCountHour();
                        } else {
                            //đã thanh toán xong, load lại list phòng cần thanh toán
                            filterData();
                            $scope.data = {
                                DtoDetails: []
                            }; 
                        }
                    }, function () {
                        $log.info('Modal dismissed at: ' + new Date());
                    });
                } else {
                    if (!$scope.modalOpen) {
                        Lobibox.notify('warning', {
                            position: 'bottom left',
                            msg: 'Chưa chọn phòng cần thanh toán !'
                        });
                    }
                    else {
                        Lobibox.notify('warning', {
                            position: 'bottom left',
                            msg: 'Chưa thanh toán xong !'
                        });
                    }
                }
            }

            $scope.keys = {
                F9: function (name, code) {
                    clearTimeout(action);
                    $scope.payRoom();
                }
            };
        }]);

    app.controller('pay_Ctrl', ['$scope', '$http', 'configService', 'thanhToanDatPhongService', 'tempDataService', '$filter', '$uibModal', '$uibModalInstance' , '$log', 'securityService', 'phongService', 'loaiPhongService', 'datPhongService', 'thamSoHeThongService', 'keyCodes', 'targetData','$sce',
       function ($scope, $http, configService, service, tempDataService, $filter, $uibModal, $uibModalInstance, $log, securityService, phongService, loaiPhongService, datPhongService, thamSoHeThongService, keyCodes, targetData, $sce) {
           $scope.keys = keyCodes;
           $scope.isValid = false;
           $scope.title = function () { return 'Xác nhận thanh toán phòng [' + targetData.TENPHONG + ' - Tầng ' + targetData.TANG + ']' };
           $scope.config = angular.copy(configService);
           $scope.tempData = tempDataService.tempData;
           $scope.target = angular.copy(targetData);
           var isPayed = false;
           $scope.keys = {
               ENTER: function (name, code) {
                   $scope.save();
               },
               ESC: function (name, code) {
                   $uibModalInstance.close(false);
               }
           };
           $scope.save = function() {
               service.post($scope.target).then(function (successRes) {
                   $scope.isValid = true;
                   if (successRes && successRes.status === 200 && successRes.data && successRes.data.Status && successRes.data.Data) {
                       Lobibox.notify('success', {
                           title: 'Thông báo',
                           width: 400,
                           msg: successRes.data.Message,
                           delay: 1500
                       });
                       isPayed = true;
                       $scope.isValid = false;
                       $uibModalInstance.close(isPayed);
                   } else {
                       Lobibox.notify('error', {
                           title: 'Xảy ra lỗi',
                           msg: 'Đã xảy ra lỗi! Thao tác không thành công',
                           delay: 3000
                       });
                       $scope.isValid = false;
                   }
               },
                function (errorRes) {
                    console.log('errorRes', errorRes);
                    $scope.isValid = false;
                });
           };
           $scope.cancel = function () {
               $uibModalInstance.close(isPayed);
           };
       }]);
    return app;
});