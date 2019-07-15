define(['ui-bootstrap', 'controllers/catalog/loaiPhongController', 'controllers/catalog/phongController', 'controllers/knowledge/datPhongController', 'controllers/authorize/thamSoHeThongController', 'controllers/catalog/matHangController'], function () {
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
            },
            senderGmail: function (body) {
                return $http.post(serviceUrl + '/SenderGmail', body);
            },
            getHistoryPay: function () {
                return $http.get(serviceUrl + '/GetHistoryPay');
            },
            getDetails: function (ID) {
                return $http.get(serviceUrl + '/GetDetails/' + ID);
            }
        }
        return result;
    }]);
    /* controller list */
    app.controller('ThanhToanDatPhong_Ctrl', ['$scope', '$http', 'configService', 'thanhToanDatPhongService', 'tempDataService', '$filter', '$uibModal', '$log', 'securityService','phongService','datPhongService','thamSoHeThongService','keyCodes','$sce','loaiPhongService','userService','$timeout','closingService','getsetDataService','matHangService','$rootScope',
        function ($scope, $http, configService, service, tempDataService, $filter, $uibModal, $log, securityService, phongService, datPhongService, thamSoHeThongService, keyCodes, $sce, loaiPhongService, userService, $timeout, closingService, getsetDataService, matHangService, $rootScope) {
            $scope.keys = keyCodes;
            var currentUser = userService.GetCurrentUser();
            $scope.modalOpen = false;
            $scope.config = angular.copy(configService);
            $scope.paged = angular.copy(configService.pageDefault);
            $scope.filtered = angular.copy(configService.filterDefault);
            $scope.tempData = tempDataService.tempData;
            $scope.title = function () { return 'Thanh toán đặt phòng' };
            $scope.search = {};
            $scope.data = {
                TONGSOLUONG: 0,
                TONGTIEN_THANHTOAN: 0,
                TONGTIEN_BANGCHU: '',
                USERNAME: currentUser.userName,
                FULLNAME: currentUser.fullName,
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
                var userName = currentUser.userName;
                var unitCodeParam = !currentUser.parentUnitCode ? currentUser.unitCode : currentUser.parentUnitCode;
                securityService.getAccessList('ThanhToanDatPhong', userName, unitCodeParam).then(function (successRes) {
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

            function SumQuanlity(list, field) {
                var sum = 0;
                if (list && list.length > 0) {
                    angular.forEach(list, function (v, k) {
                        sum += parseFloat(v[field]);
                    });
                    //if (field === 'THANHTIEN' && $scope.data.TIEN_GIOHAT > 0) sum = sum + $scope.data.TIEN_GIOHAT;
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

            function commafy(num) {
                var str = num.toString().split('.');
                if (str[0].length >= 5) {
                    str[0] = str[0].replace(/(\d)(?=(\d{3})+$)/g, '$1,');
                }
                if (str[1] && str[1].length >= 5) {
                    str[1] = str[1].replace(/(\d{3})/g, '$1 ');
                }
                return str.join('.');
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
                            hourSingle[0].THANHTIEN = $scope.data.TIEN_GIOHAT;
                            $("#" + hourSingle[0].MAHANG + '_' + hourSingle[0].SAPXEP).val(commafy($scope.data.TIEN_GIOHAT));
                        }

                        $scope.trustSOGIO = $sce.trustAsHtml('<input type="text" id="SOGIO" class="form-control" readonly style="height: 45px;border-bottom: 1px solid #f5f5f5;background: #fff;font-size: 14px;color: #000000;padding-right: 10px;" />');
                        if (document.getElementById('SOGIO') !== null) {
                            $("#SOGIO").val('Số giờ vào: ' + toHHMMSS(timeRuning));
                        }

                        $scope.trustTONGTIEN_THANHTOAN = $sce.trustAsHtml('<input type="text" awnum="number" id="TONGTIEN_THANHTOAN" class="form-control" readonly style="height: 45px;border-bottom: 1px solid #f5f5f5;background: #fff;font-size: 18px;color: #ff0000;font-weight: bold;padding-right: 10px;" />');
                        var tongTienThanhToan = SumQuanlity($scope.data.DtoDetails, 'THANHTIEN');
                        $scope.data.TONGTIEN_THANHTOAN = tongTienThanhToan;
                        if (document.getElementById('TONGTIEN_THANHTOAN') !== null) {
                            $("#TONGTIEN_THANHTOAN").val('Tiền thanh toán: ' + commafy(tongTienThanhToan));
                        }
                    }
                }
                if (document.getElementById("_thoiGianHienTai") !== null) {
                    $("#_thoiGianHienTai").val('Thời gian hiện tại: ' + getTime());
                    $scope.data.THOIGIAN_TINHTIEN = getTime();
                }
                action = setTimeout(caculateCountHour, 1000);
            };

            $scope.dischargeRoom = function (item) {
                if (item) {
                    $scope.selectedMaDatPhong = item.MA_DATPHONG;
                    service.getMerchandiseInBundleGoods(item).then(function (response) {
                        if (response && response.status === 200 && response.data && response.data.Status && response.data.Data) {
                            $scope.data = response.data.Data;
                            $scope.data.MALOAIPHONG = item.MALOAIPHONG;
                            $scope.data.TANG = item.TANG;
                            $scope.data.TENLOAIPHONG = item.TENLOAIPHONG;
                            $scope.data.TENPHONG = item.TENPHONG;
                            $scope.data.TIEN_GIOHAT = 0;
                            $scope.data.TONGTIEN_THANHTOAN = 0;
                            if ($scope.data && $scope.data.DtoDetails.length > 0) {
                                angular.forEach($scope.data.DtoDetails, function (v, k) {
                                    //mặc định số lượng ban đầu là 0 để người sử dụng đánh
                                    v.SOLUONG = 0;
                                    v.THANHTIEN = v.SOLUONG * v.GIABANLE_VAT;
                                    v.SAPXEP = k;
                                    if (v.MAHANG === $scope.data.MAHANG_DICHVU) v.IS_EDIT_VALUE = true
                                    if (v.MAHANG === $scope.data.MAHANG) {
                                        v.IS_HOUR_SINGLE_MERCHANDISE = true;
                                        caculateCountHour();
                                    }
                                });
                                //$scope.data.TONGSOLUONG = SumQuanlity($scope.data.DtoDetails, 'SOLUONG');
                                //$scope.data.TONGTIEN_THANHTOAN = SumQuanlity($scope.data.DtoDetails, 'THANHTIEN');
                            }
                            $('.infoPay').toggle("slide");
                        }
                    });
                }
            };

            //chọn thanh toán từ tab đặt phòng
            $scope.selectedMaDatPhong = null;
            var returnedData = getsetDataService.getJson();
            if (returnedData && returnedData.MALOAIPHONG && returnedData.MA_DATPHONG) {
                datPhongService.getBookingRoomByRoom(returnedData.MA_DATPHONG).then(function (successRes) {
                    if (successRes && successRes.status === 200 && successRes.data && successRes.data.Status && successRes.data.Data && successRes.data.Data.length === 1) {
                        $scope.dischargeRoom(successRes.data.Data[0]);
                    }
                });
            }
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
                            console.log(refundedData);
                        }
                    });
                }
            };
            //end function search

            //CHANGED MAHANG ADD ITEM
            $scope.changedMaHang = function (maHang) {
                if (maHang) {
                    var obj = {
                        MAHANG: maHang,
                        UNITCODE: currentUser.unitCode
                    }
                    matHangService.getMatHangTheoDieuKien(obj).then(function (successRes) {
                        if (successRes && successRes.status === 200 && successRes.data && successRes.data.Status && successRes.data.Data) {
                            
                        }
                        else {
                            //bật lên modal tìm kiếm mặt hàng
                            $scope.searchMatHang(maHang);
                        }
                    });
                }
            };
            //lịch sử thanh toán đặt phòng
            $scope.historyPay = function () {
                var modalInstance = $uibModal.open({
                    backdrop: 'static',
                    animation: true,
                    windowClass: 'knowledge-historyPay',
                    templateUrl: configService.buildUrl('knowledge/ThanhToanDatPhong', 'history'),
                    controller: 'historyPay_Ctrl',
                    resolve: {}
                });
                modalInstance.result.then(function (refundedData) {
                    $scope.refresh();
                }, function () {
                    $log.info('Modal dismissed at: ' + new Date());
                });
            };
            //end lịch sử thanh toán

            $scope.payRoom = function () {
                clearTimeout(action);
                if ($scope.data && $scope.data.MAPHONG && !$scope.modalOpen) {
                    if ($scope.data.DtoDetails.length === 0) {
                        Lobibox.notify('warning', {
                            position: 'bottom left',
                            msg: 'Không có dòng hàng! Không thể thanh toán'
                        });
                        return;
                    }
                    var dataPost = angular.copy($scope.data);
                    dataPost.DtoDetails = [];
                    angular.forEach($scope.data.DtoDetails, function (v, k) {
                        if (v.THANHTIEN && v.THANHTIEN !== 0) {
                            dataPost.DtoDetails.push(v);
                        }
                    });
                    var modalInstance = $uibModal.open({
                        backdrop: 'static',
                        keyboard  : false,
                        animation: true,
                        windowClass: 'modal-delete',
                        templateUrl: configService.buildUrl('knowledge/ThanhToanDatPhong', 'pay'),
                        controller: 'pay_Ctrl',
                        resolve: {
                            targetData: function () {
                                return dataPost;
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
                    $timeout.cancel();
                    $scope.payRoom();
                }
            };

            $scope.$on('$destroy', function () {
                clearTimeout(action);
                $timeout.cancel();
                getsetDataService.setJson({});
                $rootScope.$emit("loadDataAfterPaySuccess", {});
            });
        }]);
    
    app.controller('historyPay_Ctrl', ['$scope', '$uibModalInstance', '$http', 'configService', 'thanhToanDatPhongService', 'tempDataService', '$filter', '$uibModal', '$log','securityService','userService',
        function ($scope, $uibModalInstance, $http, configService, service, tempDataService, $filter, $uibModal, $log, securityService, userService) {
            $scope.config = angular.copy(configService);
            $scope.tempData = tempDataService.tempData;
            $scope.title = function () { return 'Lịch sử thanh toán' };
            $scope.sortType = 'NGAY_THANHTOAN';
            $scope.sortReverse = true;
            $scope.listThanhToan = [];
            $scope.target = {};
            var currentUser = userService.GetCurrentUser();
            //check authorize
            function loadAccessList() {
                var userName = currentUser.userName;
                var unitCodeParam = !currentUser.parentUnitCode ? currentUser.unitCode : currentUser.parentUnitCode;
                securityService.getAccessList('ThanhToanDatPhong', userName, unitCodeParam).then(function (successRes) {
                    if (successRes && successRes.status == 200 && successRes.data) {
                        $scope.accessList = successRes.data;
                        if (!$scope.accessList.XEM) {
                            Lobibox.notify('error', {
                                position: 'bottom left',
                                msg: 'Không có quyền truy cập !'
                            });
                        } else {
                            getHistory();
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
            function getHistory() {
                service.getHistoryPay().then(function (sucessRes) {
                    if (sucessRes && sucessRes.status === 200 && sucessRes.data && sucessRes.data.Status && sucessRes.data.Data && sucessRes.data.Data.length > 0) {
                        $scope.listThanhToan = sucessRes.data.Data;
                    }
                });
            };

            function commafy(num) {
                var str = num.toString().split('.');
                if (str[0].length >= 5) {
                    str[0] = str[0].replace(/(\d)(?=(\d{3})+$)/g, '$1,');
                }
                if (str[1] && str[1].length >= 5) {
                    str[1] = str[1].replace(/(\d{3})/g, '$1 ');
                }
                return str.join('.');
            };


            var default_numbers = ' hai ba bốn năm sáu bảy tám chín';
            var units = ('1 một' + default_numbers).split(' ');
            var ch = 'lẻ mười' + default_numbers;
            var tr = 'không một' + default_numbers;
            var tram = tr.split(' ');
            var u = '2 nghìn triệu tỉ'.split(' ');
            var chuc = ch.split(' ');
            /**
             * additional words 
             * @param  {[type]} a [description]
             * @return {[type]}   [description]
             */

            function tenth(a) {
                var sl1 = units[a[1]];
                var sl2 = chuc[a[0]];
                var append = '';
                if (a[0] > 0 && a[1] == 5)
                    sl1 = 'lăm';
                if (a[0] > 1) {
                    append = ' mươi';
                    if (a[1] == 1)
                        sl1 = ' mốt';
                }
                var str = sl2 + '' + append + ' ' + sl1;
                return str;
            };

            /**
             * convert number in blocks of 3 
             * @param  {[type]} d [description]
             * @return {[type]}   [description]
             */
            function block_of_three(d) {
                var _a = d + '';
                if (d == '000') return '';
                switch (_a.length) {
                    case 0:
                        return '';

                    case 1:
                        return units[_a];

                    case 2:
                        return tenth(_a);

                    case 3:
                        var sl12 = '';
                        if (_a.slice(1, 3) != '00')
                            sl12 = tenth(_a.slice(1, 3));
                        var sl3 = tram[_a[0]] + ' trăm';
                        return sl3 + ' ' + sl12;
                }
            };
            /**
             * Get number from unit, to string
             * @param  {mixed} nStr input money
             * @return {String}  money string, removed digits
             */
            function formatnumber(nStr) {
                nStr += '';
                var x = nStr.split('.');
                var x1 = x[0];
                var x2 = x.length > 1 ? '.' + x[1] : '';
                var rgx = /(\d+)(\d{3})/;
                while (rgx.test(x1)) {
                    x1 = x1.replace(rgx, '$1' + ',' + '$2');
                }
                return x1 + x2;
            };

            function to_vietnamese(str, currency) {
                var str = parseInt(str) + '';
                //str=fixCurrency(a,1000);
                var i = 0;
                var arr = [];
                var index = str.length;
                var result = []
                if (index == 0 || str == 'NaN')
                    return '';
                var string = '';

                //explode number string into blocks of 3numbers and push to queue
                while (index >= 0) {
                    arr.push(str.substring(index, Math.max(index - 3, 0)));
                    index -= 3;
                }

                //loop though queue and convert each block 
                for (i = arr.length - 1; i >= 0; i--) {
                    if (arr[i] != '' && arr[i] != '000') {
                        result.push(block_of_three(arr[i]))
                        if (u[i])
                            result.push(u[i]);
                    }
                }
                if (currency)
                    result.push(currency)
                string = result.join(' ')
                //remove unwanted white space
                return string.replace(/[0-9]/g, '').replace(/  /g, ' ').replace(/ $/, '');
            };

            function SumQuanlity(list, field) {
                var sum = 0;
                if (list && list.length > 0) {
                    angular.forEach(list, function (v, k) {
                        sum += parseFloat(v[field]);
                    });
                    //if (field === 'THANHTIEN' && $scope.data.TIEN_GIOHAT > 0) sum = sum + $scope.data.TIEN_GIOHAT;
                }
                return sum;
            };

            function InHoaDon(data) {
                var inVoice = '<html>';
                //header
                inVoice += '<head>';
                inVoice += '<style>';
                inVoice += '@media print {';
                inVoice += '.page-break { display: block; page-break-before: always; }';
                inVoice += '}';
                // css invoice-POS
                inVoice += '#invoice-POS {';
                inVoice += 'box-shadow: 0 0 1in -0.25in rgba(0, 0, 0, 0.5);';
                inVoice += 'padding: 2mm;';
                inVoice += 'margin: 0 auto;';
                inVoice += 'width: 90mm;';
                inVoice += 'background: #FFF;';
                inVoice += ' }';

                inVoice += '#invoice-POS ::moz-selection {';
                inVoice += 'background: #f31544;';
                inVoice += 'color: #FFF;';
                inVoice += ' }';

                inVoice += '#invoice-POS ::selection {';
                inVoice += 'background: #f31544;';
                inVoice += 'color: #FFF;';
                inVoice += ' }';
                // end css invoice-POS
                inVoice += 'table {';
                inVoice += 'border-collapse: collapse;';
                inVoice += ' }';
                inVoice += '@media only screen and (max-width: 350px) {';
                inVoice += 'body { font-family: "Times New Roman", Times, serif; }';
                inVoice += '}';
                inVoice += '</style>';
                inVoice += '</head>';
                //end header
                //body
                inVoice += '<body translate="no">';
                //table
                inVoice += '<div id="invoice-POS">';
                inVoice += '<table>';
                inVoice += '<tr>';
                inVoice += '<td colspan="6" style="text-align: center;font-weight: bold; font-style: normal; font-size: 14px;">HÓA ĐƠN THANH TOÁN</td>';
                inVoice += '</tr>';
                inVoice += '<tr>';
                inVoice += '<td colspan="6"><hr></td>';
                inVoice += '</tr>';
                inVoice += '<tr>';
                inVoice += '<td colspan="3" style="text-align: left;font-weight: bold; font-style: normal; font-size: 12px;">Phục vụ:' + data.PHUCVU + '</td>';
                inVoice += '<td colspan="3" style="text-align: left;font-weight: bold; font-style: normal; font-size: 12px;">HĐ:' + data.MA_DATPHONG + '</td>';
                inVoice += '</tr>';
                inVoice += '<tr>';
                inVoice += '<td colspan="3" style="text-align: left;font-weight: bold; font-style: normal; font-size: 12px;">Số bàn: ' + data.TENPHONG + '</td>';
                inVoice += '<td colspan="3" style="text-align: left;font-weight: bold; font-style: normal; font-size: 12px;">Ngày: ' + $scope.config.moment(new Date(data.NGAY_THANHTOAN)).format('DD-MM-YYYY') + '</td>';
                inVoice += '</tr>';
                inVoice += '<tr>';
                inVoice += '<td colspan="3" style="text-align: left;font-weight: bold; font-style: normal; font-size: 12px;">Giờ vào:' + data.THOIGIAN_DATPHONG + '</td>';
                inVoice += '<td colspan="3" style="text-align: left;font-weight: bold; font-style: normal; font-size: 12px;">Giờ ra: ' + data.THOIGIAN_THANHTOAN + '</td>';
                inVoice += '</tr>';
                inVoice += '<tr>';
                inVoice += '<td colspan="6"><br/></td>';
                inVoice += '</tr>';
                inVoice += '<tr>';
                inVoice += '<th style="text-align: center; font-style: normal; font-size: 13px; border: 1px solid black;">STT</th>';
                inVoice += '<th style="text-align: center; font-style: normal; font-size: 13px; border: 1px solid black;">Thực đơn</th>';
                inVoice += '<th style="text-align: center; font-style: normal; font-size: 13px; border: 1px solid black;">SLg</th>';
                inVoice += '<th style="text-align: center; font-style: normal; font-size: 13px; border: 1px solid black;">ĐVT</th>';
                inVoice += '<th style="text-align: center; font-style: normal; font-size: 13px; border: 1px solid black;">Đơn giá</th>';
                inVoice += '<th style="text-align: center; font-style: normal; font-size: 13px; border: 1px solid black;">Thành tiền</th>';
                inVoice += '</tr>';
                //binding data
                if (data.DtoDetails && data.DtoDetails.length > 0) {
                    angular.forEach(data.DtoDetails, function (v, k) {
                        inVoice += '<tr>';
                        inVoice += '<td style="text-align: center;font-weight: bold; font-style: normal; font-size: 13px; border: 1px solid black;">' + (k + 1) + '</td>';
                        inVoice += '<td style="text-align: left; font-style: normal; font-size: 13px; border: 1px solid black;">' + v.TENHANG + '</td>';
                        if (v.MAHANG === data.MAHANG) {
                            inVoice += '<td style="text-align: right; font-style: normal; font-size: 13px; border: 1px solid black;">' + data.THOIGIAN_SUDUNG + '</td>';
                        } else {
                            inVoice += '<td style="text-align: right; font-style: normal; font-size: 13px; border: 1px solid black;">' + commafy(v.SOLUONG) + '</td>';
                        }
                        if (v.MAHANG === data.MAHANG) {
                            inVoice += '<td style="text-align: right; font-style: normal; font-size: 13px; border: 1px solid black;"> phút</td>';
                        } else {
                            inVoice += '<td style="text-align: right; font-style: normal; font-size: 13px; border: 1px solid black;">' + v.DONVITINH + '</td>';
                        }
                        inVoice += '<td style="text-align: right; font-style: normal; font-size: 13px; border: 1px solid black;">' + commafy(v.GIABANLE_VAT) + '</td>';
                        inVoice += '<td style="text-align: right; font-style: normal; font-size: 13px; border: 1px solid black;">' + commafy(v.THANHTIEN) + '</td>';
                        inVoice += '</tr>';
                    });
                }

                var sumTienMatHang = 0;
                if (data.MAHANG && data.DtoDetails.length > 0) {
                    angular.forEach(data.DtoDetails, function (v, k) {
                        if (v.MAHANG !== data.MAHANG && v.MAHANG !== data.MAHANG_DICHVU) sumTienMatHang += parseFloat(v.THANHTIEN);
                    });
                }

                inVoice += '<tr>';
                inVoice += '<td colspan="3">Bằng chữ: </td>';
                inVoice += '<td colspan="2" style="text-align: left;font-weight: bold; font-style: normal; font-size: 12px;">Tiền hàng</td>';
                inVoice += '<td colspan="1" style="text-align: right;">' + commafy(sumTienMatHang) + '</td>';
                inVoice += '</tr>';
                inVoice += '<tr>';
                var tongTienThanhToan = SumQuanlity(data.DtoDetails, 'THANHTIEN');
                inVoice += '<td colspan="3"><span style="font-style: italic;">' + to_vietnamese(tongTienThanhToan) + '</span></td>';
                inVoice += '<td colspan="2" style="text-align: left;font-weight: bold; font-style: normal; font-size: 12px;">Tiền hát</td>';
                inVoice += '<td colspan="1" style="text-align: right;">' + commafy(data.TIEN_GIOHAT) + '</td>';
                inVoice += '</tr>';
                inVoice += '<tr>';
                inVoice += '<td colspan="3"></td>';
                inVoice += '<td colspan="2" style="text-align: left;font-weight: bold; font-style: normal; font-size: 12px;">Giảm giá</td>';
                inVoice += '<td colspan="1" style="text-align: right;">0</td>';
                inVoice += '</tr>';
                inVoice += '<tr>';
                inVoice += '<td colspan="3"></td>';
                inVoice += '<td colspan="2" style="text-align: left;font-weight: bold; font-style: normal; font-size: 12px;">Dịch vụ</td>';
                var sumTienDichVu = 0;
                if (data.MAHANG_DICHVU && data.DtoDetails.length > 0) {
                    var tienDichVu = $filter('filter')(data.DtoDetails, { MAHANG: data.MAHANG_DICHVU }, true)
                    if (tienDichVu && tienDichVu.length === 1) {
                        sumTienDichVu = parseFloat(tienDichVu[0].THANHTIEN);
                    }
                }
                inVoice += '<td colspan="1" style="text-align: right;">' + commafy(sumTienDichVu) + '</td>';
                inVoice += '</tr>';
                inVoice += '<tr>';
                inVoice += '<td colspan="3"></td>';
                inVoice += '<td colspan="3" style="text-align: right;"><hr></td>';
                inVoice += '</tr>';
                inVoice += '<tr>';
                inVoice += '<td colspan="3"></td>';
                inVoice += '<td colspan="2" style="text-align: left;font-weight: bold; font-style: normal; font-size: 12px;">Tổng tiền</td>';
                inVoice += '<td colspan="1" style="text-align: right;">' + commafy(tongTienThanhToan) + '</td>';
                inVoice += '</tr>';
                inVoice += '<tr>';
                inVoice += '<td colspan="3"></td>';
                inVoice += '<td colspan="2" style="text-align: left;font-weight: bold; font-style: normal; font-size: 12px;">Khách đưa</td>';
                inVoice += '<td colspan="1" style="text-align: right;">' + commafy(data.TIENKHACH_TRA) + '</td>';
                inVoice += '</tr>';
                inVoice += '<tr>';
                inVoice += '<td colspan="3"></td>';
                inVoice += '<td colspan="2" style="text-align: left;font-weight: bold; font-style: normal; font-size: 12px;">Trả lại</td>';
                inVoice += '<td colspan="1" style="text-align: right;">' + commafy(data.TIEN_TRALAI_KHACH) + '</td>';
                inVoice += '</tr>';
                inVoice += '<tr>';
                inVoice += '<td colspan="6" style="text-align: center; font-style: italic; font-weight: bold;">Cảm ơn quý khách và hẹn gặp lại!</td>';
                inVoice += '</tr>';
                //end binding data
                inVoice += '</table>';
                inVoice += '</div>';
                //end table
                inVoice += '</body>';
                //end body
                inVoice += '</html>';
                return inVoice;
            };
            //chi tiết hóa đơn
            $scope.detail = function (item) {
                if (item) {
                    service.getDetails(item.ID).then(function (sucessRes) {
                        if (sucessRes && sucessRes.status === 200 && sucessRes.data && sucessRes.data.Status && sucessRes.data.Data) {
                            $scope.target = sucessRes.data.Data;
                            console.log($scope.target);
                            console.log(InHoaDon($scope.target));
                        }
                    });
                }
            };
            //end
            $scope.cancel = function () {
                $uibModalInstance.close();
            };
        }]);


    app.controller('pay_Ctrl', ['$scope', '$http', 'configService', 'thanhToanDatPhongService', 'tempDataService', '$filter', '$uibModal', '$uibModalInstance' , '$log', 'securityService', 'phongService', 'loaiPhongService', 'datPhongService', 'thamSoHeThongService', 'keyCodes', 'targetData','$sce','userService',
       function ($scope, $http, configService, service, tempDataService, $filter, $uibModal, $uibModalInstance, $log, securityService, phongService, loaiPhongService, datPhongService, thamSoHeThongService, keyCodes, targetData, $sce, userService) {
           $scope.keys = keyCodes;
           var currentUser = userService.GetCurrentUser();
           $scope.isValid = false;
           $scope.title = function () { return 'Xác nhận thanh toán phòng [' + targetData.TENPHONG + ' - Tầng ' + targetData.TANG + ']' };
           $scope.config = angular.copy(configService);
           $scope.tempData = tempDataService.tempData;
           $scope.target = angular.copy(targetData);
           $scope.target.TIENKHACH_TRA = 0;
           $scope.target.TIEN_TRALAI_KHACH = 0;
           var isPayed = false;
           $scope.keys = {
               ENTER: function (name, code) {
                   $scope.save();
               },
               ESC: function (name, code) {
                   $uibModalInstance.close(false);
               }
           };
           var sumTienMatHang = 0;
           if ($scope.target.MAHANG && $scope.target.DtoDetails.length > 0) {
               angular.forEach($scope.target.DtoDetails, function (v, k) {
                   if (v.MAHANG !== $scope.target.MAHANG && v.MAHANG !== $scope.target.MAHANG_DICHVU) sumTienMatHang += parseFloat(v.THANHTIEN);
               });
           }
           if (document.getElementById("TIENKHACH_TRA") != null) {
               focus('TIENKHACH_TRA');
               document.getElementById('TIENKHACH_TRA').focus();
               document.getElementById('TIENKHACH_TRA').select();
           }
           
           var sumTienDichVu = 0;
           if ($scope.target.MAHANG_DICHVU && $scope.target.DtoDetails.length > 0) {
               var tienDichVu = $filter('filter')($scope.target.DtoDetails, { MAHANG: $scope.target.MAHANG_DICHVU }, true)
               if (tienDichVu && tienDichVu.length === 1) {
                   sumTienDichVu = parseFloat(tienDichVu[0].THANHTIEN);
               }
           }

           var default_numbers = ' hai ba bốn năm sáu bảy tám chín';
           var units = ('1 một' + default_numbers).split(' ');
           var ch = 'lẻ mười' + default_numbers;
           var tr = 'không một' + default_numbers;
           var tram = tr.split(' ');
           var u = '2 nghìn triệu tỉ'.split(' ');
           var chuc = ch.split(' ');
           /**
            * additional words 
            * @param  {[type]} a [description]
            * @return {[type]}   [description]
            */

           function tenth(a) {
               var sl1 = units[a[1]];
               var sl2 = chuc[a[0]];
               var append = '';
               if (a[0] > 0 && a[1] == 5)
                   sl1 = 'lăm';
               if (a[0] > 1) {
                   append = ' mươi';
                   if (a[1] == 1)
                       sl1 = ' mốt';
               }
               var str = sl2 + '' + append + ' ' + sl1;
               return str;
           };

           /**
            * convert number in blocks of 3 
            * @param  {[type]} d [description]
            * @return {[type]}   [description]
            */
           function block_of_three(d) {
               var _a = d + '';
               if (d == '000') return '';
               switch (_a.length) {
                   case 0:
                       return '';

                   case 1:
                       return units[_a];

                   case 2:
                       return tenth(_a);

                   case 3:
                       var sl12 = '';
                       if (_a.slice(1, 3) != '00')
                           sl12 = tenth(_a.slice(1, 3));
                       var sl3 = tram[_a[0]] + ' trăm';
                       return sl3 + ' ' + sl12;
               }
           };
           /**
            * Get number from unit, to string
            * @param  {mixed} nStr input money
            * @return {String}  money string, removed digits
            */
           function formatnumber(nStr) {
               nStr += '';
               var x = nStr.split('.');
               var x1 = x[0];
               var x2 = x.length > 1 ? '.' + x[1] : '';
               var rgx = /(\d+)(\d{3})/;
               while (rgx.test(x1)) {
                   x1 = x1.replace(rgx, '$1' + ',' + '$2');
               }
               return x1 + x2;
           };

           function to_vietnamese(str, currency) {
               var str = parseInt(str) + '';
               //str=fixCurrency(a,1000);
               var i = 0;
               var arr = [];
               var index = str.length;
               var result = []
               if (index == 0 || str == 'NaN')
                   return '';
               var string = '';

               //explode number string into blocks of 3numbers and push to queue
               while (index >= 0) {
                   arr.push(str.substring(index, Math.max(index - 3, 0)));
                   index -= 3;
               }

               //loop though queue and convert each block 
               for (i = arr.length - 1; i >= 0; i--) {
                   if (arr[i] != '' && arr[i] != '000') {
                       result.push(block_of_three(arr[i]))
                       if (u[i])
                           result.push(u[i]);
                   }
               }
               if (currency)
                   result.push(currency)
               string = result.join(' ')
               //remove unwanted white space
               return string.replace(/[0-9]/g, '').replace(/  /g, ' ').replace(/ $/, '');
           };

           function commafy(num) {
               var str = num.toString().split('.');
               if (str[0].length >= 5) {
                   str[0] = str[0].replace(/(\d)(?=(\d{3})+$)/g, '$1,');
               }
               if (str[1] && str[1].length >= 5) {
                   str[1] = str[1].replace(/(\d{3})/g, '$1 ');
               }
               return str.join('.');
           };

           var inVoice = '<html>';
           //header
           inVoice += '<head>';
           inVoice += '<style>';
           inVoice += '@media print {';
           inVoice += '.page-break { display: block; page-break-before: always; }';
           inVoice += '}';
           // css invoice-POS
           inVoice += '#invoice-POS {';
           inVoice += 'box-shadow: 0 0 1in -0.25in rgba(0, 0, 0, 0.5);';
           inVoice += 'padding: 2mm;';
           inVoice += 'margin: 0 auto;';
           inVoice += 'width: 90mm;';
           inVoice += 'background: #FFF;';
           inVoice += ' }';

           inVoice += '#invoice-POS ::moz-selection {';
           inVoice += 'background: #f31544;';
           inVoice += 'color: #FFF;';
           inVoice += ' }';

           inVoice += '#invoice-POS ::selection {';
           inVoice += 'background: #f31544;';
           inVoice += 'color: #FFF;';
           inVoice += ' }';
           // end css invoice-POS
           inVoice += 'table {';
           inVoice += 'border-collapse: collapse;';
           inVoice += ' }';
           inVoice += '@media only screen and (max-width: 350px) {';
           inVoice += 'body { font-family: "Times New Roman", Times, serif; }';
           inVoice += '}';
           inVoice += '</style>';
           inVoice += '</head>';
           //end header
           //body
           inVoice += '<body translate="no">';
           //table
           inVoice += '<div id="invoice-POS">';
           inVoice += '<table>';
           inVoice += '<tr>';
           inVoice += '<td colspan="6" style="text-align: center;font-weight: bold; font-style: normal; font-size: 14px;">HÓA ĐƠN THANH TOÁN</td>';
           inVoice += '</tr>';
           inVoice += '<tr>';
           inVoice += '<td colspan="6"><hr></td>';
           inVoice += '</tr>';
           inVoice += '<tr>';
           inVoice += '<td colspan="3" style="text-align: left;font-weight: bold; font-style: normal; font-size: 12px;">Phục vụ:' + currentUser.fullName + '</td>';
           inVoice += '<td colspan="3" style="text-align: left;font-weight: bold; font-style: normal; font-size: 12px;">HĐ:' + $scope.target.MA_DATPHONG + '</td>';
           inVoice += '</tr>';
           inVoice += '<tr>';
           inVoice += '<td colspan="3" style="text-align: left;font-weight: bold; font-style: normal; font-size: 12px;">Số bàn: ' + $scope.target.TENPHONG + '</td>';
           inVoice += '<td colspan="3" style="text-align: left;font-weight: bold; font-style: normal; font-size: 12px;">Ngày: ' + $scope.config.moment(new Date()).format('DD-MM-YYYY') + '</td>';
           inVoice += '</tr>';
           inVoice += '<tr>';
           inVoice += '<td colspan="3" style="text-align: left;font-weight: bold; font-style: normal; font-size: 12px;">Giờ vào:' + $scope.target.THOIGIAN_DATPHONG + '</td>';
           inVoice += '<td colspan="3" style="text-align: left;font-weight: bold; font-style: normal; font-size: 12px;">Giờ ra: ' + $scope.target.THOIGIAN_TINHTIEN + '</td>';
           inVoice += '</tr>';
           inVoice += '<tr>';
           inVoice += '<td colspan="6"><br/></td>';
           inVoice += '</tr>';
           inVoice += '<tr>';
           inVoice += '<th style="text-align: center; font-style: normal; font-size: 13px; border: 1px solid black;">STT</th>';
           inVoice += '<th style="text-align: center; font-style: normal; font-size: 13px; border: 1px solid black;">Thực đơn</th>';
           inVoice += '<th style="text-align: center; font-style: normal; font-size: 13px; border: 1px solid black;">SLg</th>';
           inVoice += '<th style="text-align: center; font-style: normal; font-size: 13px; border: 1px solid black;">ĐVT</th>';
           inVoice += '<th style="text-align: center; font-style: normal; font-size: 13px; border: 1px solid black;">Đơn giá</th>';
           inVoice += '<th style="text-align: center; font-style: normal; font-size: 13px; border: 1px solid black;">Thành tiền</th>';
           inVoice += '</tr>';
           //binding data
           if ($scope.target.DtoDetails && $scope.target.DtoDetails.length > 0) {
               angular.forEach($scope.target.DtoDetails, function(v,k){
                   inVoice += '<tr>';
                   inVoice += '<td style="text-align: center;font-weight: bold; font-style: normal; font-size: 13px; border: 1px solid black;">' + (k + 1) + '</td>';
                   inVoice += '<td style="text-align: left; font-style: normal; font-size: 13px; border: 1px solid black;">' + v.TENHANG + '</td>';
                   if (v.MAHANG === $scope.target.MAHANG) {
                       inVoice += '<td style="text-align: right; font-style: normal; font-size: 13px; border: 1px solid black;">' + $scope.target.THOIGIAN_SUDUNG + '</td>';
                   } else {
                       inVoice += '<td style="text-align: right; font-style: normal; font-size: 13px; border: 1px solid black;">' + commafy(v.SOLUONG) + '</td>';
                   }
                   if (v.MAHANG === $scope.target.MAHANG) {
                       inVoice += '<td style="text-align: right; font-style: normal; font-size: 13px; border: 1px solid black;"> phút</td>';
                   } else {
                       inVoice += '<td style="text-align: right; font-style: normal; font-size: 13px; border: 1px solid black;">' + v.DONVITINH + '</td>';
                   }
                   inVoice += '<td style="text-align: right; font-style: normal; font-size: 13px; border: 1px solid black;">' + commafy(v.GIABANLE_VAT) + '</td>';
                   inVoice += '<td style="text-align: right; font-style: normal; font-size: 13px; border: 1px solid black;">' + commafy(v.THANHTIEN) + '</td>';
                   inVoice += '</tr>';
               });
           }
           inVoice += '<tr>';
           inVoice += '<td colspan="3">Bằng chữ: </td>';
           inVoice += '<td colspan="2" style="text-align: left;font-weight: bold; font-style: normal; font-size: 12px;">Tiền hàng</td>';
           inVoice += '<td colspan="1" style="text-align: right;">' + commafy(sumTienMatHang) + '</td>';
           inVoice += '</tr>';
           inVoice += '<tr>';
           inVoice += '<td colspan="3"><span style="font-style: italic;">' + to_vietnamese($scope.target.TONGTIEN_THANHTOAN) + '</span></td>';
           inVoice += '<td colspan="2" style="text-align: left;font-weight: bold; font-style: normal; font-size: 12px;">Tiền hát</td>';
           inVoice += '<td colspan="1" style="text-align: right;">' + commafy($scope.target.TIEN_GIOHAT) + '</td>';
           inVoice += '</tr>';
           inVoice += '<tr>';
           inVoice += '<td colspan="3"></td>';
           inVoice += '<td colspan="2" style="text-align: left;font-weight: bold; font-style: normal; font-size: 12px;">Giảm giá</td>';
           inVoice += '<td colspan="1" style="text-align: right;">0</td>';
           inVoice += '</tr>';
           inVoice += '<tr>';
           inVoice += '<td colspan="3"></td>';
           inVoice += '<td colspan="2" style="text-align: left;font-weight: bold; font-style: normal; font-size: 12px;">Dịch vụ</td>';
           inVoice += '<td colspan="1" style="text-align: right;">' + commafy(sumTienDichVu) + '</td>';
           inVoice += '</tr>';
           inVoice += '<tr>';
           inVoice += '<td colspan="3"></td>';
           inVoice += '<td colspan="3" style="text-align: right;"><hr></td>';
           inVoice += '</tr>';
           inVoice += '<tr>';
           inVoice += '<td colspan="3"></td>';
           inVoice += '<td colspan="2" style="text-align: left;font-weight: bold; font-style: normal; font-size: 12px;">Tổng tiền</td>';
           inVoice += '<td colspan="1" style="text-align: right;">' + commafy($scope.target.TONGTIEN_THANHTOAN) + '</td>';
           inVoice += '</tr>';
           inVoice += '<tr>';
           inVoice += '<td colspan="3"></td>';
           inVoice += '<td colspan="2" style="text-align: left;font-weight: bold; font-style: normal; font-size: 12px;">Khách đưa</td>';
           inVoice += '<td colspan="1" style="text-align: right;">TIENKHACH_TRA_BINDING</td>';
           inVoice += '</tr>';
           inVoice += '<tr>';
           inVoice += '<td colspan="3"></td>';
           inVoice += '<td colspan="2" style="text-align: left;font-weight: bold; font-style: normal; font-size: 12px;">Trả lại</td>';
           inVoice += '<td colspan="1" style="text-align: right;">TIEN_TRALAI_KHACH_BINDING</td>';
           inVoice += '</tr>';
           inVoice += '<tr>';
           inVoice += '<td colspan="6" style="text-align: center; font-style: italic; font-weight: bold;">Cảm ơn quý khách và hẹn gặp lại!</td>';
           inVoice += '</tr>';
           //end binding data
           inVoice += '</table>';
           inVoice += '</div>';
           //end table
           inVoice += '</body>';
           //end body
           inVoice += '</html>';

           $scope.changeTienKhachTra = function (tienKhachTra) {
               if (tienKhachTra < 0) {
                   Lobibox.notify('warning', {
                       title: 'Cảnh báo',
                       msg: 'Số tiền không đúng',
                       delay: 500
                   });
                   $scope.target.TIENKHACH_TRA = 0;
               } else {
                   $scope.target.TIEN_TRALAI_KHACH = parseFloat($scope.target.TIENKHACH_TRA) - parseFloat($scope.target.TONGTIEN_THANHTOAN);
               }
           };

           function printInvoice() {
               var frame = document.createElement('iframe');
               frame.name = "frameInvoice";
               frame.style.position = "absolute";
               //frame.style.top = "-1000000px";
               document.body.appendChild(frame);
               var frameDoc = frame.contentWindow ? frame.contentWindow : frame.contentDocument.document ? frame.contentDocument.document : frame.contentDocument;
               frameDoc.document.open();
               frameDoc.document.write(inVoice);
               frameDoc.document.close();
               setTimeout(function () {
                   window.frames["frameInvoice"].focus();
                   window.frames["frameInvoice"].print();
                   document.body.removeChild(frame);
               }, 100);
               return false;
               window.print();
           };
          
           function sendGmail() {
               if (inVoice && inVoice !== '') {
                   var obj = {
                       MA_DATPHONG: $scope.target.MA_DATPHONG,
                       MAPHONG: $scope.target.MAPHONG,
                       UNITCODE: currentUser.unitCode,
                       BODY: inVoice
                   };
                   service.senderGmail(obj).then(function (successRes) {
                   });
               }
           };

           $scope.save = function () {
               $scope.target.NGAY_DATPHONG = $scope.config.moment($scope.target.NGAY_DATPHONG).format();
               inVoice = inVoice.replace("TIENKHACH_TRA_BINDING", commafy($scope.target.TIENKHACH_TRA));
               inVoice = inVoice.replace("TIEN_TRALAI_KHACH_BINDING", commafy($scope.target.TIEN_TRALAI_KHACH));
               sendGmail();
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
               printInvoice();
           };
           $scope.cancel = function () {
               $uibModalInstance.close(isPayed);
           };
       }]);
    return app;
});