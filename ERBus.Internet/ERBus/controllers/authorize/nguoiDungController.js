define(['ui-bootstrap', 'controllers/authorize/nguoiDungNhomQuyenController', 'controllers/authorize/nguoiDungQuyenController'], function () {
    'use strict';
    var app = angular.module('nguoiDungModule', ['ui.bootstrap', 'nguoiDungNhomQuyenModule', 'nguoiDungQuyenModule']);
    app.factory('nguoiDungService', ['$http', 'configService', function ($http, configService) {
        var serviceUrl = configService.rootUrlWebApi + '/Authorize/NguoiDung';
        var selectedData = [];
        var result = {
            getAllData: function () {
                return $http.get(serviceUrl + '/GetAllData');
            },
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
            checkExistsUsername: function (user) {
                return $http.get(serviceUrl + '/CheckExistsUsername/' + user);
            }
        }
        return result;
    }]);
    /* controller list */
    app.controller('NguoiDung_Ctrl', ['$scope', '$http', 'configService', 'nguoiDungService', 'tempDataService', '$filter', '$uibModal', '$log', 'securityService', 'nguoiDungNhomQuyenService','nguoiDungQuyenService',
        function ($scope, $http, configService, service, tempDataService, $filter, $uibModal, $log, securityService, nguoiDungNhomQuyenService, nguoiDungQuyenService) {
            $scope.config = angular.copy(configService);
            $scope.paged = angular.copy(configService.pageDefault);
            $scope.filtered = angular.copy(configService.filterDefault);
            $scope.tempData = tempDataService.tempData;
            $scope.title = function () { return 'Tài khoản người dùng' };
            $scope.data = [];
            $scope.setPage = function (pageNo) {
                $scope.paged.CurrentPage = pageNo;
                filterData();
            };
            $scope.sortType = 'USERNAME';
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
                securityService.getAccessList('NguoiDung').then(function (successRes) {
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
                    size: 'lg',
                    templateUrl: configService.buildUrl('authorize/NguoiDung', 'create'),
                    controller: 'nguoiDungCreate_Ctrl',
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
                    size: 'lg',
                    templateUrl: configService.buildUrl('authorize/NguoiDung', 'detail'),
                    controller: 'nguoiDungDetail_Ctrl',
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
                    size: 'lg',
                    templateUrl: configService.buildUrl('authorize/NguoiDung', 'edit'),
                    controller: 'nguoiDungEdit_Ctrl',
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

            /*Function phân nhóm quyền*/
            $scope.settingGroup = function (item) {
                $uibModal.open({
                    backdrop: 'static',
                    animation: true,
                    size: 'lg',
                    templateUrl: configService.buildUrl('authorize/NguoiDungNhomQuyen', 'add'),
                    controller: 'AddNguoiDungNhomQuyen_ctrl',
                    resolve: {
                        targetData: function () {
                            return item;
                        }
                    }
                });
            };
            /*end*/

            /*Function phân quyền người dùng*/
            $scope.settingGrant = function (item) {
                $uibModal.open({
                    backdrop: 'static',
                    animation: true,
                    windowClass: 'modal-grant',
                    templateUrl: configService.buildUrl('authorize/NguoiDungQuyen', 'add'),
                    controller: 'AddNguoiDungQuyen_ctrl',
                    resolve: {
                        targetData: function () {
                            return item;
                        }
                    }
                });
            };
            /*end*/

            /* Function delete item */
            $scope.delete = function (event, target) {
                var modalInstance = $uibModal.open({
                    backdrop: 'static',
                    animation: true,
                    windowClass: 'modal-delete',
                    templateUrl: configService.buildUrl('authorize/NguoiDung', 'delete'),
                    controller: 'nguoiDungDelete_Ctrl',
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

    app.controller('nguoiDungCreate_Ctrl', ['$scope', '$uibModalInstance', '$http', 'configService', 'nguoiDungService', 'tempDataService', '$filter', '$uibModal', '$log',
        function ($scope, $uibModalInstance, $http, configService, service, tempDataService, $filter, $uibModal, $log) {
            $scope.config = angular.copy(configService);
            $scope.tempData = tempDataService.tempData;
            $scope.title = function () { return 'Thêm tài khoản người dùng'; };
            $scope.target = {};
            //Tạo mới mã loại
            service.buildNewCode().then(function (successRes) {
                if (successRes && successRes.status == 200 && successRes.data) {
                    $scope.target.MANHANVIEN = successRes.data;
                }
            });
            //end
            //check exists username
            $scope.checkExistsUsername = function (userName) {
                if (userName) {
                    service.checkExistsUsername(userName).then(function (response) {
                        if (response && response.status === 200 && response.data) {
                            Lobibox.notify('warning', {
                                title: 'Cảnh báo',
                                msg: 'Username này đã tồn tại! Xin nhập Username khác',
                                delay: 3000
                            });
                            $scope.target.USERNAME = '';
                            document.getElementById('_username').focus();
                        } else {
                            Lobibox.notify('success', {
                                title: 'Thông báo',
                                width: 400,
                                msg: 'Bạn có thể sử dụng Username này',
                                delay: 2000
                            });
                        }
                    });
                }
            };
            //end check
            $scope.save = function () {
                if (!$scope.target.USERNAME || !$scope.target.MANHANVIEN) {
                    Lobibox.notify('warning', {
                        title: 'Thiếu thông tin',
                        msg: 'Thông tin đầu vào không hợp lệ! Dữ liệu (*) không được để trống',
                        delay: 4000
                    });
                } else {
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
            };
            $scope.cancel = function () {
                $uibModalInstance.close();
            };
        }]);
    app.controller('nguoiDungDetail_Ctrl', ['$scope', '$uibModalInstance', '$http', 'configService', 'nguoiDungService', 'targetData', 'tempDataService', '$filter', '$uibModal', '$log',
        function ($scope, $uibModalInstance, $http, configService, service, targetData, tempDataService, $filter, $uibModal, $log) {
            $scope.config = angular.copy(configService);
            $scope.tempData = tempDataService.tempData;
            $scope.target = {};
            $scope.target = angular.copy(targetData);
            $scope.title = function () { return 'Thông tin tài khoản người dùng [' + targetData.USERNAME + ']'; };
            $scope.cancel = function () {
                $uibModalInstance.close();
            };
        }]);
    app.controller('nguoiDungEdit_Ctrl', ['$scope', '$uibModalInstance', '$http', 'configService', 'nguoiDungService', 'targetData', 'tempDataService', '$filter', '$uibModal', '$log',
        function ($scope, $uibModalInstance, $http, configService, service, targetData, tempDataService, $filter, $uibModal, $log) {
            $scope.config = angular.copy(configService);
            $scope.tempData = tempDataService.tempData;
            $scope.target = {};
            $scope.target = angular.copy(targetData);
            console.log($scope.target);
            $scope.title = function () { return 'Chỉnh sửa tài khoản người dùng'; };
            $scope.save = function () {
                if (!$scope.target.USERNAME || !$scope.target.MANHANVIEN) {
                    Lobibox.notify('warning', {
                        title: 'Thiếu thông tin',
                        msg: 'Thông tin đầu vào không hợp lệ! Dữ liệu (*) không được để trống',
                        delay: 4000
                    });
                } else {
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
            };
            $scope.cancel = function () {
                $uibModalInstance.close();
            };
        }]);

    app.controller('nguoiDungDelete_Ctrl', ['$scope', '$uibModalInstance', '$http', 'configService', 'nguoiDungService', 'targetData', 'tempDataService', '$filter', '$uibModal', '$log',
       function ($scope, $uibModalInstance, $http, configService, service, targetData, tempDataService, $filter, $uibModal, $log) {
           $scope.config = angular.copy(configService);
           $scope.tempData = tempDataService.tempData;
           $scope.target = {};
           $scope.target = angular.copy(targetData);;
           $scope.title = function () { return 'Xóa tài khoản người dùng [' + targetData.USERNAME + ']'; };
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