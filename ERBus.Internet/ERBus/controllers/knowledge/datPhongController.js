define(['ui-bootstrap', 'controllers/catalog/phongController'], function () {
    'use strict';
    var app = angular.module('datPhongModule', ['ui.bootstrap', 'phongModule']);
    app.factory('datPhongService', ['$http', 'configService', function ($http, configService) {
        var serviceUrl = configService.rootUrlWebApi + '/Knowledge/DatPhong';
        var selectedData = [];
        var result = {
            postQuery: function (data) {
                return $http.post(serviceUrl + '/PostQuery', data);
            },
            post: function (data) {
                return $http.post(serviceUrl + '/Post', data);
            },
            buildNewCode: function () {
                return $http.get(serviceUrl + '/BuildNewCode');
            },
            removeBooking: function (params) {
                return $http.delete(serviceUrl + '/' + params.ID, params);
            },
            getListBookingRoom: function () {
                return $http.get(serviceUrl + '/GetListBookingRoom');
            },
            updateBooking: function (params) {
                return $http.put(serviceUrl + '/' + params.ID, params);
            }
        }
        return result;
    }]);
    /* controller list */
    app.controller('DatPhong_Ctrl', ['$scope', '$http', 'configService', 'datPhongService', 'tempDataService', '$filter', '$uibModal', '$log', 'securityService','phongService','$sce','$timeout',
        function ($scope, $http, configService, service, tempDataService, $filter, $uibModal, $log, securityService, phongService, $sce, $timeout) {
            $scope.config = angular.copy(configService);
            $scope.paged = angular.copy(configService.pageDefault);
            $scope.filtered = angular.copy(configService.filterDefault);
            $scope.tempData = tempDataService.tempData;
            $scope.title = function () { return 'Đặt phòng' };
            var dateTimeBooking;
            $scope.setPage = function (pageNo) {
                $scope.paged.CurrentPage = pageNo;
                filterData();
            };
            $scope.sortType = 'MA_DATPHONG';
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
            function caculateCountHour() {
                if ($scope.data && $scope.data.length > 0) {
                    var result = '';
                    angular.forEach($scope.data, function (v, k) {
                        v.NGAY_DATPHONG = new Date(v.NGAY_DATPHONG);
                        if (v.THOIGIAN_DATPHONG && v.NGAY_DATPHONG && v.TRANGTHAI_DATPHONG === 10) {
                            var time = monthNames[v.NGAY_DATPHONG.getMonth()] + ' ' + v.NGAY_DATPHONG.getDate() + ',' + ' ' + v.NGAY_DATPHONG.getFullYear() + ' ' + v.THOIGIAN_DATPHONG;
                            dateTimeBooking = new Date(time);
                            result = result + ' | ' + toHHMMSS((new Date().getTime() - dateTimeBooking.getTime()) / 1000)
                            $scope.decriptionBooking = $sce.trustAsHtml('<a id = "noteBooking" class="form-control">' + result + '</a>');
                        }
                    });
                }
                setTimeout(caculateCountHour, 1000);
            };
            
            function filterData() {
                if ($scope.accessList.VIEW) {
                    phongService.getStatusAllRoom().then(function (successRes) {
                        if (successRes && successRes.status === 200 && successRes.data && successRes.data.Status) {
                            $scope.data = successRes.data.Data;
                            angular.forEach($scope.data, function (v, k) {
                                v.dataHtml = '<hr/>';
                                v.dataHtml += '<button class="btn btn-success" prevent-default ng-click=\"bookingRoom(item)\"><img style="width: 11px; height: 11px;" ng-src="data:image/svg+xml;base64,' + v.ICON + '">&nbsp;&nbsp;Đặt phòng</button>';
                                if (v.TRANGTHAI_DATPHONG === 10) 
                                    v.dataHtml += '<button class="btn btn-success" style="margin: 0 10px;" prevent-default ng-click=\"dischargeRoom(item)\"><i class="zmdi zmdi-paypal"></i>&nbsp;&nbsp;Thanh toán</button>';
                            });
                        }
                    });
                }
            };
            //check authorize
            function loadAccessList() {
                securityService.getAccessList('DatPhong').then(function (successRes) {
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

            $scope.bookingRoom = function (item) {
                if (item && item.ID) {
                    var modalInstance = $uibModal.open({
                        backdrop: 'static',
                        animation: true,
                        windowClass: 'modal-booking',
                        templateUrl: configService.buildUrl('knowledge/DatPhong', 'booking'),
                        controller: 'bookingRoom_Ctrl',
                        resolve: {
                            targetData: function () {
                                return item;
                            }
                        }
                    });
                    modalInstance.result.then(function (refundedData) {
                        filterData();
                    }, function () {
                        $log.info('Modal dismissed at: ' + new Date());
                    });
                }
            };

            $scope.dischargeRoom = function (item) {
                if (item && item.ID) {
                    var modalInstance = $uibModal.open({
                        backdrop: 'static',
                        animation: true,
                        windowClass: 'modal-booking',
                        templateUrl: configService.buildUrl('knowledge/DatPhong', 'discharge'),
                        controller: 'dischargeRoom_Ctrl',
                        resolve: {
                            targetData: function () {
                                return item;
                            }
                        }
                    });
                    modalInstance.result.then(function (refundedData) {
                    }, function () {
                        $log.info('Modal dismissed at: ' + new Date());
                    });
                }
            };
            $scope.$on('$destroy', function () {
                $timeout.cancel();
            });
        }]);

    app.controller('bookingRoom_Ctrl', ['$scope', '$uibModalInstance', '$http', 'configService', 'datPhongService', 'targetData', 'tempDataService', '$filter', '$uibModal','$sce','$timeout',
        function ($scope, $uibModalInstance, $http, configService, service, targetData, tempDataService, $filter, $uibModal, $sce, $timeout) {
            $scope.config = angular.copy(configService);
            $scope.tempData = tempDataService.tempData;
            $scope.target = {};
            $scope.target.NGAY_DATPHONG = new Date();
            $scope.paged = angular.copy(configService.pageDefault);
            $scope.filtered = angular.copy(configService.filterDefault);
            $scope.title = function () { return 'Đặt phòng [' + targetData.TENPHONG + ' - Tầng ' + targetData.TANG + ']'; };
            $scope.listBooking = [];
            var dateTimeBooking;
            var isSettimeout = true;
            $scope.isAddInfoCustomer = false;
            $scope.isShow = true;
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
            function showTime() {
                if (document.getElementById("_thoiGianDatPhong") !== null) {
                    $("#_thoiGianDatPhong").val(getTime());
                }
                if (isSettimeout) setTimeout(showTime, 1000);
            };
            showTime();
            $scope.settingTime = function () {
                if (isSettimeout) {
                    isSettimeout = false;
                }else {
                    isSettimeout = true;
                    showTime();
                }
            };
            
            $scope.addInfoCus = function () {
                if (!$scope.isAddInfoCustomer) {
                    $scope.isAddInfoCustomer = true;
                } else {
                    $scope.isAddInfoCustomer = false;
                }
                $scope.isShow = false
            };

            $scope.minusInfoCus = function () {
                if ($scope.isAddInfoCustomer) {
                    $scope.isAddInfoCustomer = false;
                } else {
                    $scope.isAddInfoCustomer = true;
                }
                $scope.isShow = true;
            };

            function buildCode() {
                service.buildNewCode().then(function (successRes) {
                    if (successRes && successRes.status == 200 && successRes.data) {
                        $scope.target.MA_DATPHONG = successRes.data;
                    }
                });
            };
            buildCode();

            $scope.convertCodeToName = function (paraValue, moduleName) {
                if (paraValue) {
                    var tempCache = $filter('filter')($scope.tempData(moduleName), { VALUE: paraValue }, true);
                    if (tempCache && tempCache.length === 1) {
                        return tempCache[0].DESCRIPTION + ' - Tầng ' + tempCache[0].PARENT;
                    } else {
                        return paraValue;
                    }
                }
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
            var action;
            function caculateCountHour() {
                if ($scope.listBooking && $scope.listBooking.length > 0) {
                    angular.forEach($scope.listBooking, function (v, k) {
                        v.NGAY_DATPHONG = new Date(v.NGAY_DATPHONG);
                        if (v.THOIGIAN_DATPHONG && v.NGAY_DATPHONG && v.TRANGTHAI === 10) {
                            var time = monthNames[v.NGAY_DATPHONG.getMonth()] + ' ' + v.NGAY_DATPHONG.getDate() + ',' + ' ' + v.NGAY_DATPHONG.getFullYear() + ' ' + v.THOIGIAN_DATPHONG;
                            dateTimeBooking = new Date(time);
                            v.inputTextThoiGianDat = $sce.trustAsHtml('<input type="text" id = "' + v.MA_DATPHONG + '"' + 'class="form-control" readonly>');
                            if (document.getElementById(v.MA_DATPHONG) !== null) {
                                $("#" + v.MA_DATPHONG).val(toHHMMSS((new Date().getTime() - dateTimeBooking.getTime()) / 1000));
                            }
                        }
                    });
                }
                action = setTimeout(caculateCountHour, 1000);
            };

            function filterData() {
                $scope.isLoading = true;
                $scope.filtered.advanceData.MAPHONG = targetData.MAPHONG;
                $scope.filtered.advanceData.NGAY_DATPHONG = $scope.config.moment(new Date()).format();
                $scope.filtered.isAdvance = true;
                var postdata = { paged: $scope.paged, filtered: $scope.filtered };
                service.postQuery(postdata).then(function (successRes) {
                    if (successRes && successRes.status === 200 && successRes.data && successRes.data.Status && successRes.data.Data && successRes.data.Data.Data) {
                        $scope.isLoading = false;
                        $scope.listBooking = successRes.data.Data.Data;
                        caculateCountHour();
                        angular.extend($scope.paged, successRes.data.Data);
                    }
                }, function (errorRes) {
                    $scope.isLoading = false;
                    console.log(errorRes);
                });
            };

            filterData();

            $scope.isValid = false;
            $scope.save = function () {
                $scope.target.THOIGIAN_DATPHONG = getTime();
                $scope.target.MAPHONG = targetData.MAPHONG;
                $scope.target.NGAY_DATPHONG = $scope.config.moment($scope.target.NGAY_DATPHONG).format();
                service.post($scope.target).then(function (successRes) {
                    $scope.isValid = true;
                    if (successRes && successRes.status === 200 && successRes.data && successRes.data.Status && successRes.data.Data) {
                        Lobibox.notify('success', {
                            title: 'Thông báo',
                            width: 400,
                            msg: successRes.data.Message,
                            delay: 1500
                        });
                        $scope.isValid = false;
                        filterData();
                        $scope.target.NGAY_DATPHONG = new Date();
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
            $scope.removeBooking = function (event, item) {
                var modalInstance = $uibModal.open({
                    backdrop: 'static',
                    animation: true,
                    windowClass: 'modal-delete',
                    templateUrl: configService.buildUrl('knowledge/DatPhong', 'remove'),
                    controller: 'datPhongRemoveBooking_Ctrl',
                    resolve: {
                        targetData: function () {
                            return item;
                        }
                    }
                });
                modalInstance.result.then(function (refundedData) {
                    filterData();
                }, function () {
                    $log.info('Modal dismissed at: ' + new Date());
                });
            };

            $scope.updateBooking = function (event, item) {
                var modalInstance = $uibModal.open({
                    backdrop: 'static',
                    animation: true,
                    windowClass: 'modal-delete',
                    templateUrl: configService.buildUrl('knowledge/DatPhong', 'update'),
                    controller: 'datPhongUpdateBooking_Ctrl',
                    resolve: {
                        targetData: function () {
                            return item;
                        }
                    }
                });
                modalInstance.result.then(function (refundedData) {
                    filterData();
                }, function () {
                    $log.info('Modal dismissed at: ' + new Date());
                });
            };

            $scope.$on('$destroy', function () {
                clearTimeout(action);
                $timeout.cancel();
            });

            $scope.cancel = function () {
                $uibModalInstance.close();
            };
        }]);

    app.controller('datPhongUpdateBooking_Ctrl', ['$scope', '$uibModalInstance', '$http', 'configService', 'datPhongService', 'targetData', 'tempDataService', '$filter', '$uibModal', '$log',
       function ($scope, $uibModalInstance, $http, configService, service, targetData, tempDataService, $filter, $uibModal, $log) {
           $scope.config = angular.copy(configService);
           $scope.tempData = tempDataService.tempData;
           $scope.target = {};
           $scope.target = angular.copy(targetData);;
           $scope.title = function () { return 'Cập nhật đặt phòng [' + targetData.MAPHONG + ']'; };
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
           $scope.updateBooking = function () {
               $scope.target.THOIGIAN_DATPHONG = getTime();
               $scope.target.NGAY_DATPHONG = $scope.config.moment(new Date()).format();
               service.updateBooking($scope.target).then(function (successRes) {
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

    app.controller('datPhongRemoveBooking_Ctrl', ['$scope', '$uibModalInstance', '$http', 'configService', 'datPhongService', 'targetData', 'tempDataService', '$filter', '$uibModal', '$log',
       function ($scope, $uibModalInstance, $http, configService, service, targetData, tempDataService, $filter, $uibModal, $log) {
           $scope.config = angular.copy(configService);
           $scope.tempData = tempDataService.tempData;
           $scope.target = {};
           $scope.target = angular.copy(targetData);;
           $scope.title = function () { return 'Hủy đặt phòng [' + targetData.MAPHONG + ']'; };
           $scope.removeBooking = function () {
               service.removeBooking($scope.target).then(function (successRes) {
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
