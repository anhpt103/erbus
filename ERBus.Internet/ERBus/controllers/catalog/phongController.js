define(['ui-bootstrap', 'controllers/catalog/loaiPhongController', 'controllers/catalog/khoHangController'], function () {
    'use strict';
    var app = angular.module('phongModule', ['ui.bootstrap', 'loaiPhongModule', 'khoHangModule']);
    app.factory('phongService', ['$http', 'configService', function ($http, configService) {
        var serviceUrl = configService.rootUrlWebApi + '/Catalog/Phong';
        var selectedData = [];
        var result = {
            getAllData: function () {
                return $http.get(serviceUrl + '/GetAllData');
            },
            getDataByMaPhong: function (maPhongSelected) {
                return $http.get(serviceUrl + '/GetDataByMaPhong/' + maPhongSelected);
            },
            postQuery: function (data) {
                return $http.post(serviceUrl + '/PostQuery', data);
            },
            buildNewCode: function (maTang) {
                return $http.get(serviceUrl + '/BuildNewCode/' + maTang);
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
            getStatusAllRoom: function () {
                return $http.get(serviceUrl + '/GetStatusAllRoom');
            }
        }
        return result;
    }]);
    /* controller list */
    app.controller('Phong_Ctrl', ['$scope', '$http', 'configService', 'phongService', 'tempDataService', '$filter', '$uibModal', '$log', 'securityService','loaiPhongService','userService','khoHangService',
        function ($scope, $http, configService, service, tempDataService, $filter, $uibModal, $log, securityService, loaiPhongService, userService, khoHangService) {
            $scope.config = angular.copy(configService);
            $scope.paged = angular.copy(configService.pageDefault);
            $scope.filtered = angular.copy(configService.filterDefault);
            $scope.tempData = tempDataService.tempData;
            $scope.title = function () { return 'Phòng' };
            $scope.data = [];
            $scope.treeFloor = [];
            $scope.setPage = function (pageNo) {
                $scope.paged.CurrentPage = pageNo;
                filterData();
            };
            $scope.sortType = 'MAPHONG';
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
            var listFloor = [
                { VALUE: 1, TEXT: 'Tầng 1' },
                { VALUE: 2, TEXT: 'Tầng 2' },
                { VALUE: 3, TEXT: 'Tầng 3' },
                { VALUE: 4, TEXT: 'Tầng 4' },
                { VALUE: 5, TEXT: 'Tầng 5' },
                { VALUE: 6, TEXT: 'Tầng 6' },
                { VALUE: 7, TEXT: 'Tầng 7' },
                { VALUE: 8, TEXT: 'Tầng 8' },
                { VALUE: 9, TEXT: 'Tầng 9' },
                { VALUE: 10, TEXT: 'Tầng 10' },
            ];
            if (!tempDataService.tempData('floor')) {
                tempDataService.putTempData('floor', listFloor);
            } else {
                $scope.listFloor = tempDataService.tempData('floor');
            }
            //

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

            function treeify(list, idAttr, parentAttr, childrenAttr) {
                if (!idAttr) idAttr = 'MAPHONG';
                if (!parentAttr) parentAttr = 'TANG';
                if (!childrenAttr) childrenAttr = 'CHILDREN';
                var lookup = {};
                var result = {};
                result[childrenAttr] = [];
                list.forEach(function (obj) {
                    lookup[obj[idAttr]] = obj;
                    obj[childrenAttr] = [];
                });
                list.forEach(function (obj) {
                    if (obj[parentAttr] != null) {
                        try { lookup[obj[parentAttr]][childrenAttr].push(obj); }
                        catch (err) {
                            result[childrenAttr].push(obj);
                        }

                    } else {
                        result[childrenAttr].push(obj);
                    }
                });
                return result;
            };
            function filterData() {
                $scope.isLoading = true;
                if ($scope.accessList.XEM) {
                    var postdata = { paged: $scope.paged, filtered: $scope.filtered };
                    service.postQuery(postdata).then(function (successRes) {
                        if (successRes && successRes.status === 200 && successRes.data && successRes.data.Status && successRes.data.Data && successRes.data.Data.Data) {
                            $scope.isLoading = false;
                            $scope.data = [];
                            $scope.treeFloor = [];
                            $scope.data = successRes.data.Data.Data;
                            var groups = {};
                            for (var i = 0; i < $scope.data.length; i++) {
                                var groupName = $scope.data[i].TANG;
                                if (!groups[groupName]) {
                                    groups[groupName] = [];
                                }
                                groups[groupName].push($scope.data[i].TANG);
                            }
                            $scope.groupValue = [];
                            for (var groupName in groups) {
                                $scope.groupValue.push({ TANG: groupName });
                            }
                            if ($scope.groupValue && $scope.groupValue.length > 0) {
                                angular.forEach($scope.groupValue, function (v, k) {
                                    var obj = {
                                        TANG: null,
                                        MAPHONG: v.TANG,
                                        TENPHONG: 'Tầng ' + v.TANG,
                                        VITRI: 'Tầng ' + v.TANG
                                    }
                                    $scope.data.push(obj)
                                });
                            }
                            $scope.treeFloor = treeify($scope.data).CHILDREN;
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
                var currentUser = userService.GetCurrentUser();
                var userName = currentUser.userName;
                var unitCodeParam = !currentUser.parentUnitCode ? currentUser.unitCode : currentUser.parentUnitCode;
                securityService.getAccessList('Phong', userName, unitCodeParam).then(function (successRes) {
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
                    //windowClass: 'catalog-window',
                    size: 'lg',
                    templateUrl: configService.buildUrl('catalog/Phong', 'create'),
                    controller: 'phongCreate_Ctrl',
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
                    templateUrl: configService.buildUrl('catalog/Phong', 'detail'),
                    controller: 'phongDetail_Ctrl',
                    resolve: {
                        targetData: function () {
                            return target;
                        }
                    }
                });
                modalInstance.result.then(function (refundedData) {
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
                    templateUrl: configService.buildUrl('catalog/Phong', 'edit'),
                    controller: 'phongEdit_Ctrl',
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

    app.controller('phongCreate_Ctrl', ['$scope', '$uibModalInstance', '$http', 'configService', 'phongService', 'tempDataService', '$filter', '$uibModal', '$log','khoHangService',
        function ($scope, $uibModalInstance, $http, configService, service, tempDataService, $filter, $uibModal, $log, khoHangService) {
            $scope.config = angular.copy(configService);
            $scope.tempData = tempDataService.tempData;
            $scope.title = function () { return 'Thêm phòng'; };
            $scope.target = { TANG: 1 };

            service.buildNewCode($scope.target.TANG).then(function (successRes) {
                if (successRes && successRes.status == 200 && successRes.data) {
                    $scope.target.MAPHONG = successRes.data;
                }
            });

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

            //Tạo mới mã phòng
            $scope.changeFloor = function (maTang) {
                if (maTang) {
                    service.buildNewCode(maTang).then(function (successRes) {
                        if (successRes && successRes.status == 200 && successRes.data) {
                            $scope.target.MAPHONG = successRes.data;
                        }
                    });
                }
            };
            //end
            $scope.save = function () {
                if (!$scope.target.MAPHONG || !$scope.target.TENPHONG ) {
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
    app.controller('phongDetail_Ctrl', ['$scope', '$uibModalInstance', '$http', 'configService', 'phongService', 'targetData', 'tempDataService', '$filter', '$uibModal', '$log',
        function ($scope, $uibModalInstance, $http, configService, service, targetData, tempDataService, $filter, $uibModal, $log) {
            $scope.config = angular.copy(configService);
            $scope.tempData = tempDataService.tempData;
            $scope.target = {};
            $scope.target = angular.copy(targetData);
            $scope.title = function () { return 'Thông tin phòng [' + targetData.MAPHONG + ']'; };
            $scope.cancel = function () {
                $uibModalInstance.close();
            };
        }]);
    app.controller('phongEdit_Ctrl', ['$scope', '$uibModalInstance', '$http', 'configService', 'phongService', 'targetData', 'tempDataService', '$filter', '$uibModal', '$log',
        function ($scope, $uibModalInstance, $http, configService, service, targetData, tempDataService, $filter, $uibModal, $log) {
            $scope.config = angular.copy(configService);
            $scope.tempData = tempDataService.tempData;
            $scope.target = {};
            $scope.target = angular.copy(targetData);
            $scope.title = function () { return 'Chỉnh sửa phòng [' + targetData.MAPHONG + ']'; };
            $scope.save = function () {
                if (!$scope.target.MAPHONG || !$scope.target.TENPHONG ) {
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

            /* Function delete item */
            $scope.delete = function (event, target) {
                var modalInstance = $uibModal.open({
                    backdrop: 'static',
                    animation: true,
                    windowClass: 'modal-delete',
                    templateUrl: configService.buildUrl('catalog/Phong', 'delete'),
                    controller: 'phongDelete_Ctrl',
                    resolve: {
                        targetData: function () {
                            return target;
                        }
                    }
                });
                modalInstance.result.then(function (refundedData) {
                    $uibModalInstance.close('delete');
                }, function () {
                    $log.info('Modal dismissed at: ' + new Date());
                });
            };

            $scope.cancel = function () {
                $uibModalInstance.close();
            };
        }]);

    app.controller('phongDelete_Ctrl', ['$scope', '$uibModalInstance', '$http', 'configService', 'phongService', 'targetData', 'tempDataService', '$filter', '$uibModal', '$log',
       function ($scope, $uibModalInstance, $http, configService, service, targetData, tempDataService, $filter, $uibModal, $log) {
           $scope.config = angular.copy(configService);
           $scope.tempData = tempDataService.tempData;
           $scope.target = {};
           $scope.target = angular.copy(targetData);;
           $scope.title = function () { return 'Xóa phòng [' + targetData.MAPHONG + ']'; };
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