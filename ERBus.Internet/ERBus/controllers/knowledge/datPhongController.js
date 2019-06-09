define(['ui-bootstrap', 'controllers/catalog/phongController'], function () {
    'use strict';
    var app = angular.module('datPhongModule', ['ui.bootstrap', 'phongModule']);
    app.factory('datPhongService', ['$http', 'configService', function ($http, configService) {
        var serviceUrl = configService.rootUrlWebApi + '/Knowledge/DatPhong';
        var selectedData = [];
        var result = {
            postQuery: function (data) {
                return $http.post(serviceUrl + '/PostQuery', data);
            }
        }
        return result;
    }]);
    /* controller list */
    app.controller('DatPhong_Ctrl', ['$scope', '$http', 'configService', 'datPhongService', 'tempDataService', '$filter', '$uibModal', '$log', 'securityService','phongService',
        function ($scope, $http, configService, service, tempDataService, $filter, $uibModal, $log, securityService, phongService) {
            $scope.config = angular.copy(configService);
            $scope.paged = angular.copy(configService.pageDefault);
            $scope.filtered = angular.copy(configService.filterDefault);
            $scope.tempData = tempDataService.tempData;
            $scope.title = function () { return 'Đặt phòng' };
            $scope.data = [];
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
            
            function filterData() {
                $scope.isLoading = true;
                if ($scope.accessList.VIEW) {
                    phongService.getAllData().then(function (successRes) {
                        if (successRes && successRes.status === 200 && successRes.data && successRes.data.Status) {
                            $scope.data = successRes.data.Data;
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
                        console.log(refundedData);
                    }, function () {
                        $log.info('Modal dismissed at: ' + new Date());
                    });
                }
            };
        }]);

    app.controller('bookingRoom_Ctrl', ['$scope', '$uibModalInstance', '$http', 'configService', 'datPhongService', 'targetData', 'tempDataService', '$filter', '$uibModal',
        function ($scope, $uibModalInstance, $http, configService, service, targetData, tempDataService, $filter, $uibModal) {
            $scope.config = angular.copy(configService);
            $scope.tempData = tempDataService.tempData;
            $scope.target = {};
            $scope.target.NGAY_DATPHONG = new Date();
            $scope.title = function () { return 'Đặt phòng [' + targetData.DESCRIPTION + ' - Tầng ' + targetData.PARENT + ']'; };
            var isSettimeout = true;
            $scope.isAddInfoCustomer = false;
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
            };

            $scope.save = function () {
                $scope.target.THOIGIAN_DATPHONG = getTime();
                console.log($scope.target);
            };
            $scope.cancel = function () {
                $uibModalInstance.close();
            };
        }]);
    return app;
});