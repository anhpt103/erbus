define(['ui-bootstrap','controllers/authorize/authController'], function () {
    'use strict';
    var app = angular.module('thamSoHeThongModule', ['ui.bootstrap','authModule']);
    app.factory('thamSoHeThongService', ['$http', 'configService', function ($http, configService) {
        var serviceUrl = configService.rootUrlWebApi + '/Authorize/ThamSoHeThong';
        var selectedData = [];
        var result = {
            postQuery: function (data) {
                return $http.post(serviceUrl + '/PostQuery', data);
            },
            getDataByMaThamSo: function (maThamSo) {
                return $http.get(serviceUrl + '/GetDataByMaThamSo');
            },
            buildNewCode: function () {
                return $http.get(serviceUrl + '/BuildNewCode');
            },
            post: function (data) {
                return $http.post(serviceUrl + '/Post', data);
            },
            settingParam: function (data) {
                return $http.post(serviceUrl + '/SettingParam', data);
            },
            update: function (params) {
                return $http.put(serviceUrl + '/' + params.ID, params);
            }
        }
        return result;
    }]);
    /* controller list */
    app.controller('ThamSoHeThong_Ctrl', ['$scope', '$http', 'configService', 'thamSoHeThongService', 'tempDataService', '$filter', '$uibModal', '$log', 'securityService','userService',
        function ($scope, $http, configService, service, tempDataService, $filter, $uibModal, $log, securityService,userService) {
            $scope.config = angular.copy(configService);
            $scope.paged = angular.copy(configService.pageDefault);
            $scope.filtered = angular.copy(configService.filterDefault);
            $scope.tempData = tempDataService.tempData;
            $scope.title = function () { return 'Tham số hệ thống' };
            $scope.listThamSo = [
                 {
                     MA_THAMSO: 'SUDUNG_LCD',
                     TEN_THAMSO: 'SỬ DỤNG MÀN HÌNH HIỂN THỊ LCD CHỨC NĂNG BÁN LẺ',
                     GIATRI_SO: 0,
                     GIATRI_CHU: null,
                     I_STATE: 'C', //NOT
                     UNITCODE: userService.CurrentUser.unitCode,
                     TRANGTHAI: 10,
                     ISDIABLED_GIATRI_CHU: true,
                     PLACEHOLDER: 'Để trống'
                 },
                {
                    MA_THAMSO: 'KHOA_BANAM',
                    TEN_THAMSO: 'KHÔNG CHO PHÉP BÁN ÂM',
                    GIATRI_SO: 0,
                    GIATRI_CHU: null,
                    I_STATE: 'C', //NOT
                    UNITCODE: userService.CurrentUser.unitCode,
                    TRANGTHAI: 10,
                    ISDIABLED_GIATRI_CHU: true,
                    PLACEHOLDER: 'Để trống'
                },
                {
                    MA_THAMSO: 'QUICK_ADD_CATALOG',
                    TEN_THAMSO: 'SỬ DỤNG CHỨC NĂNG THÊM NHANH TRONG DANH MỤC',
                    GIATRI_SO: 0,
                    GIATRI_CHU: null,
                    I_STATE: 'C', //NOT
                    UNITCODE: userService.CurrentUser.unitCode,
                    TRANGTHAI: 10,
                    ISDIABLED_GIATRI_CHU: true,
                    PLACEHOLDER: 'Để trống'
                },
                {
                    MA_THAMSO: 'QUICK_ADD_KNOWLEDGE',
                    TEN_THAMSO: 'SỬ DỤNG CHỨC NĂNG THÊM NHANH TRONG NGHIỆP VỤ',
                    GIATRI_SO: 0,
                    GIATRI_CHU: null,
                    I_STATE: 'C', //NOT
                    UNITCODE: userService.CurrentUser.unitCode,
                    TRANGTHAI: 10,
                    ISDIABLED_GIATRI_CHU: true,
                    PLACEHOLDER: 'Để trống'
                },
                {
                    MA_THAMSO: 'QUICK_ADD',
                    TEN_THAMSO: 'SỬ DỤNG CHỨC NĂNG KHÁC',
                    GIATRI_SO: 0,
                    GIATRI_CHU: null,
                    I_STATE: 'C', //NOT
                    UNITCODE: userService.CurrentUser.unitCode,
                    TRANGTHAI: 10,
                    ISDIABLED_GIATRI_CHU: true,
                    PLACEHOLDER: 'Để trống'
                },
                {
                    MA_THAMSO: 'DEFAULT_VAT',
                    TEN_THAMSO: 'GIÁ TRỊ THUẾ MẶC ĐỊNH',
                    GIATRI_SO: 0,
                    GIATRI_CHU: null,
                    I_STATE: 'C', //NOT
                    UNITCODE: userService.CurrentUser.unitCode,
                    TRANGTHAI: 10,
                    ISDIABLED_GIATRI_CHU: false,
                    PLACEHOLDER: 'Nhập giá trị theo mã thuế trong danh mục thuế'
                },
                {
                    MA_THAMSO: 'DEFAULT_KHOBANLE',
                    TEN_THAMSO: 'MẶC ĐỊNH KHO BÁN LẺ',
                    GIATRI_SO: 0,
                    GIATRI_CHU: null,
                    I_STATE: 'C', //NOT
                    UNITCODE: userService.CurrentUser.unitCode,
                    TRANGTHAI: 10,
                    ISDIABLED_GIATRI_CHU: false,
                    PLACEHOLDER: 'Nhập giá trị theo mã kho trong danh mục kho'
                }
            ];
           
            $scope.setPage = function (pageNo) {
                $scope.paged.CurrentPage = pageNo;
                filterData();
            };
            $scope.sortType = 'MA_THAMSO';
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
            function filterData() {
                $scope.data = [];
                $scope.isLoading = true;
                if ($scope.accessList.VIEW) {
                    var postdata = { paged: $scope.paged, filtered: $scope.filtered };
                    service.postQuery(postdata).then(function (successRes) {
                        if (successRes && successRes.status === 200 && successRes.data && successRes.data.Status && successRes.data.Data && successRes.data.Data.Data) {
                            $scope.isLoading = false;
                            $scope.data = successRes.data.Data.Data;
                            if ($scope.listThamSo && $scope.listThamSo.length > 0) {
                                angular.forEach($scope.listThamSo, function (v, k) {
                                    if (successRes.data.Data.Data.length > 0) {
                                        var checkExist = $filter('filter')(successRes.data.Data.Data, { MA_THAMSO: v.MA_THAMSO }, true)
                                        if (checkExist.length === 0) {
                                            $scope.data.push(v);
                                        }
                                    }
                                    else {
                                        $scope.data.push(v);
                                    }
                                });
                            }
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
                securityService.getAccessList('ThamSoHeThong').then(function (successRes) {
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

            //change
            $scope.changeGiaTriThamSo = function (item) {
                if (item) {
                    if (!item.MA_THAMSO || !item.TEN_THAMSO) {
                        item.GIATRI_SO = 0;
                        Lobibox.notify('warning', {
                            title: 'Thiếu thông tin',
                            msg: 'Thông tin đầu vào không hợp lệ! Dữ liệu (*) không được để trống',
                            delay: 4000
                        });
                    }
                    else if (!item.ISDIABLED_GIATRI_CHU && !item.GIATRI_CHU) {
                        item.GIATRI_SO = 0;
                        Lobibox.notify('warning', {
                            title: 'Thiếu thông tin',
                            msg: 'Phải nhập giá trị cài đặt (Theo Mã)',
                            delay: 4000
                        });
                    }
                    else {
                        service.settingParam(item).then(function (successRes) {
                            if (successRes && successRes.status === 200 && successRes.data && successRes.data.Status && successRes.data.Data) {
                                Lobibox.notify('success', {
                                    title: 'Thông báo',
                                    width: 400,
                                    msg: successRes.data.Message,
                                    delay: 1500
                                });
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
        }]);
    return app;
});