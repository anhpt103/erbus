define(['ui-bootstrap', 'controllers/catalog/boHangController', 'controllers/catalog/cauHinhLoaiPhongController'], function () {
    'use strict';
    var app = angular.module('loaiPhongModule', ['ui.bootstrap', 'boHangModule', 'cauHinhLoaiPhongModule']);
    app.factory('loaiPhongService', ['$http', 'configService', function ($http, configService) {
        var serviceUrl = configService.rootUrlWebApi + '/Catalog/LoaiPhong';
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
            getDetails: function (maLoaiPhong) {
                return $http.get(serviceUrl + '/GetDetails/' + maLoaiPhong);
            }
        }
        return result;
    }]);
    /* controller list */
    app.controller('LoaiPhong_Ctrl', ['$scope', '$http', 'configService', 'loaiPhongService', 'tempDataService', '$filter', '$uibModal', '$log', 'securityService', 'boHangService','userService',
        function ($scope, $http, configService, service, tempDataService, $filter, $uibModal, $log, securityService, boHangService, userService) {
            $scope.config = angular.copy(configService);
            $scope.paged = angular.copy(configService.pageDefault);
            $scope.filtered = angular.copy(configService.filterDefault);
            $scope.tempData = tempDataService.tempData;
            $scope.title = function () { return 'Loại phòng' };
            $scope.data = [];
            $scope.treeFloor = [];
            $scope.setPage = function (pageNo) {
                $scope.paged.CurrentPage = pageNo;
                filterData();
            };
            $scope.sortType = 'MALOAIPHONG';
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
            //Function load data catalog LoaiHang
            function loadDataBoHang() {
                $scope.boHang = [];
                if (!tempDataService.tempData('boHang')) {
                    boHangService.getAllData().then(function (successRes) {
                        if (successRes && successRes.status === 200 && successRes.data && successRes.data.Status && successRes.data.Data && successRes.data.Data.length > 0) {
                            tempDataService.putTempData('boHang', successRes.data.Data);
                            $scope.boHang = successRes.data.Data;
                        }
                    }, function (errorRes) {
                        console.log('errorRes', errorRes);
                    });
                } else {
                    $scope.boHang = tempDataService.tempData('boHang');
                }
            };
            loadDataBoHang();
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
                var currentUser = userService.GetCurrentUser();
                var userName = currentUser.userName;
                var unitCodeParam = !currentUser.parentUnitCode ? currentUser.unitCode : currentUser.parentUnitCode;
                securityService.getAccessList('LoaiPhong', userName, unitCodeParam).then(function (successRes) {
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
                    templateUrl: configService.buildUrl('catalog/LoaiPhong', 'create'),
                    controller: 'loaiPhongCreate_Ctrl',
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
                    templateUrl: configService.buildUrl('catalog/LoaiPhong', 'detail'),
                    controller: 'loaiPhongDetail_Ctrl',
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
                    templateUrl: configService.buildUrl('catalog/LoaiPhong', 'edit'),
                    controller: 'loaiPhongEdit_Ctrl',
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

            $scope.setting = function (target) {
                var modalInstance = $uibModal.open({
                    backdrop: 'static',
                    animation: true,
                    size: 'lg',
                    templateUrl: configService.buildUrl('catalog/LoaiPhong', 'setting'),
                    controller: 'loaiPhongSetting_Ctrl',
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
                    templateUrl: configService.buildUrl('catalog/LoaiPhong', 'delete'),
                    controller: 'loaiPhongDelete_Ctrl',
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

    app.controller('loaiPhongCreate_Ctrl', ['$scope', '$uibModalInstance', '$http', 'configService', 'loaiPhongService', 'tempDataService', '$filter', '$uibModal', '$log', '$timeout', 'Upload','userService',
        function ($scope, $uibModalInstance, $http, configService, service, tempDataService, $filter, $uibModal, $log, $timeout, upload, userService) {
            $scope.config = angular.copy(configService);
            var currentUser = userService.GetCurrentUser();
            $scope.tempData = tempDataService.tempData;
            $scope.title = function () { return 'Thêm loại phòng'; };

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

            service.buildNewCode().then(function (successRes) {
                if (successRes && successRes.status == 200 && successRes.data) {
                    $scope.target.MALOAIPHONG = successRes.data;
                }
            });

            $scope.fileBackground = {};
            $scope.uploadBackground = function (input) {
                $scope.inputBackground = input;
                if (input.files && input.files.length > 0) {
                    $timeout(function () {
                        var fileReader = new FileReader();
                        fileReader.readAsDataURL(input.files[0]);
                        fileReader.onload = function (e) {
                            $timeout(function () {
                                $scope.fileBackground.SRC = e.target.result;
                            });
                        };
                    });
                    $scope.fileBackground.FILE = input.files[0];
                }
            };
            $scope.deleteBackground = function () {
                if ($scope.target.BACKGROUND) {
                    $scope.target.BACKGROUND = null;
                }
                if ($scope.fileBackground) {
                    $scope.fileBackground = {};
                    angular.element("#file-input-background").val(null);
                }
            };

            $scope.fileIcon = {};
            $scope.uploadIcon = function (input) {
                $scope.inputIcon = input;
                if (input.files && input.files.length > 0) {
                    $timeout(function () {
                        var fileReader = new FileReader();
                        fileReader.readAsDataURL(input.files[0]);
                        fileReader.onload = function (e) {
                            $timeout(function () {
                                $scope.fileIcon.SRC = e.target.result;
                            });
                        };
                    });
                    $scope.fileIcon.FILE = input.files[0];
                }
            };
            $scope.deleteIcon = function () {
                if ($scope.target.ICON) {
                    $scope.target.ICON = null;
                }
                if ($scope.fileIcon) {
                    $scope.fileIcon = {};
                    angular.element("#file-input-icon").val(null);
                }
            };

            function saveLoaiPhong() {
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
            };


            function UploadIcon() {
                if ($scope.fileIcon && $scope.fileIcon.SRC) {
                    $scope.fileIcon.MALOAIPHONG = $scope.target.MALOAIPHONG;
                    upload.upload({
                        url: configService.rootUrlWebApi + '/Catalog/LoaiPhong/UploadIcon',
                        headers: { 'Authorization': 'Bearer ' + currentUser.access_token },
                        contentType: "application/json; charset=utf-8",
                        dataType: 'json',
                        data: $scope.fileIcon
                    }).then(function (successRes) {
                        if (successRes && successRes.status === 200 && successRes.data && successRes.data.Status && successRes.data.Data) {
                            $scope.target.ICON_NAME = successRes.data.Data.FILENAME;
                            saveLoaiPhong();
                        } else {
                            Lobibox.notify('error', {
                                title: 'Xảy ra lỗi',
                                msg: 'Đã xảy ra lỗi! Thao tác upload Icon không thành công',
                                delay: 2500
                            });
                        }
                    });
                } else {
                    saveLoaiPhong();
                }
            };


            $scope.save = function () {
                if (!$scope.target.MALOAIPHONG || !$scope.target.TENLOAIPHONG) {
                    Lobibox.notify('warning', {
                        title: 'Thiếu thông tin',
                        msg: 'Thông tin đầu vào không hợp lệ! Dữ liệu (*) không được để trống',
                        delay: 4000
                    });
                } else {
                    if ($scope.fileBackground && $scope.fileBackground.SRC) {
                        $scope.fileBackground.MALOAIPHONG = $scope.target.MALOAIPHONG;
                        upload.upload({
                            url: configService.rootUrlWebApi + '/Catalog/LoaiPhong/UploadBackground',
                            headers: { 'Authorization': 'Bearer ' + currentUser.access_token },
                            contentType: "application/json; charset=utf-8",
                            dataType: 'json',
                            data: $scope.fileBackground
                        }).then(function (successRes) {
                            if (successRes && successRes.status === 200 && successRes.data && successRes.data.Status && successRes.data.Data) {
                                $scope.target.BACKGROUND_NAME = successRes.data.Data.FILENAME;
                                UploadIcon();
                            } else {
                                Lobibox.notify('error', {
                                    title: 'Xảy ra lỗi',
                                    msg: 'Đã xảy ra lỗi! Thao tác upload Background không thành công',
                                    delay: 2500
                                });
                            }
                        });
                    } else {
                        UploadIcon();
                    }
                }
            };
            $scope.cancel = function () {
                $uibModalInstance.close();
            };
        }]);
    app.controller('loaiPhongDetail_Ctrl', ['$scope', '$uibModalInstance', '$http', 'configService', 'loaiPhongService', 'targetData', 'tempDataService', '$filter', '$uibModal', '$log',
        function ($scope, $uibModalInstance, $http, configService, service, targetData, tempDataService, $filter, $uibModal, $log) {
            $scope.config = angular.copy(configService);
            $scope.tempData = tempDataService.tempData;
            $scope.target = {};
            $scope.target = angular.copy(targetData);
            $scope.title = function () { return 'Thông tin loại phòng [' + targetData.MALOAIPHONG + ']'; };
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
            $scope.cancel = function () {
                $uibModalInstance.close();
            };
        }]);

    app.controller('loaiPhongEdit_Ctrl', ['$scope', '$uibModalInstance', '$http', 'configService', 'loaiPhongService', 'targetData', 'tempDataService', '$filter', '$uibModal', '$log', '$timeout', 'Upload','userService',
        function ($scope, $uibModalInstance, $http, configService, service, targetData, tempDataService, $filter, $uibModal, $log, $timeout, upload, userService) {
            var currentUser = userService.GetCurrentUser();
            $scope.config = angular.copy(configService);
            $scope.tempData = tempDataService.tempData;
            $scope.target = {};
            $scope.target = angular.copy(targetData);
            $scope.title = function () { return 'Chỉnh sửa loại phòng [' + targetData.MALOAIPHONG + ']'; };
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
            $scope.fileBackground = {};
            $scope.uploadBackground = function (input) {
                $scope.inputBackground = input;
                if (input.files && input.files.length > 0) {
                    $timeout(function () {
                        var fileReader = new FileReader();
                        fileReader.readAsDataURL(input.files[0]);
                        fileReader.onload = function (e) {
                            $timeout(function () {
                                $scope.fileBackground.SRC = e.target.result;
                            });
                        };
                    });
                    $scope.fileBackground.FILE = input.files[0];
                }
            };
            $scope.deleteBackground = function () {
                if ($scope.target.BACKGROUND) {
                    $scope.target.BACKGROUND = null;
                }
                if ($scope.fileBackground) {
                    $scope.fileBackground = {};
                    angular.element("#file-input-background").val(null);
                }
            };
            function saveLoaiPhong() {
                $scope.save = function () {
                    if (!$scope.target.MALOAIPHONG || !$scope.target.TENLOAIPHONG) {
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
            };

            function UploadIcon() {
                if ($scope.fileIcon && $scope.fileIcon.SRC) {
                    $scope.fileIcon.MALOAIPHONG = $scope.target.MALOAIPHONG;
                    upload.upload({
                        url: configService.rootUrlWebApi + '/Catalog/LoaiPhong/UploadIcon',
                        headers: { 'Authorization': 'Bearer ' + currentUser.access_token },
                        contentType: "application/json; charset=utf-8",
                        dataType: 'json',
                        data: $scope.fileIcon
                    }).then(function (successRes) {
                        if (successRes && successRes.status === 200 && successRes.data && successRes.data.Status && successRes.data.Data) {
                            $scope.target.ICON_NAME = successRes.data.Data.FILENAME;
                            saveLoaiPhong();
                        } else {
                            Lobibox.notify('error', {
                                title: 'Xảy ra lỗi',
                                msg: 'Đã xảy ra lỗi! Thao tác upload Icon không thành công',
                                delay: 2500
                            });
                        }
                    });
                } else {
                    saveLoaiPhong();
                }
            };

            $scope.deleteIcon = function () {
                if ($scope.target.ICON) {
                    $scope.target.ICON = null;
                }
                if ($scope.fileIcon) {
                    $scope.fileIcon = {};
                    angular.element("#file-input-icon").val(null);
                }
            };

            $scope.save = function () {
                if ($scope.fileBackground && $scope.fileBackground.SRC) {
                    $scope.fileBackground.MALOAIPHONG = $scope.target.MALOAIPHONG;
                    upload.upload({
                        url: configService.rootUrlWebApi + '/Catalog/LoaiPhong/UploadBackground',
                        headers: { 'Authorization': 'Bearer ' + currentUser.access_token },
                        contentType: "application/json; charset=utf-8",
                        dataType: 'json',
                        data: $scope.fileBackground
                    }).then(function (successRes) {
                        if (successRes && successRes.status === 200 && successRes.data && successRes.data.Status && successRes.data.Data) {
                            $scope.target.BACKGROUND_NAME = successRes.data.Data.FILENAME;
                            UploadIcon();
                        } else {
                            Lobibox.notify('error', {
                                title: 'Xảy ra lỗi',
                                msg: 'Đã xảy ra lỗi! Thao tác upload Background không thành công',
                                delay: 2500
                            });
                        }
                    });
                } else {
                    UploadIcon();
                }
            };


            $scope.cancel = function () {
                $uibModalInstance.close();
            };
        }]);

    app.controller('loaiPhongSetting_Ctrl', ['$scope', '$uibModalInstance', '$http', 'configService', 'loaiPhongService', 'targetData', 'tempDataService', '$filter', '$uibModal', '$log', 'boHangService', 'cauHinhLoaiPhongService',
       function ($scope, $uibModalInstance, $http, configService, service, targetData, tempDataService, $filter, $uibModal, $log, boHangService, cauHinhLoaiPhongService) {
           $scope.config = angular.copy(configService);
           $scope.tempData = tempDataService.tempData;
           $scope.target = {};
           $scope.target = angular.copy(targetData);
           $scope.title = function () { return 'Cài đặt loại phòng [' + targetData.MALOAIPHONG + ']'; };

           $scope.convertCodeToNameBoHang = function (paraValue, moduleName) {
               if (paraValue) {
                   var tempCache = $filter('filter')($scope.tempData(moduleName), { VALUE: paraValue }, true);
                   if (tempCache && tempCache.length === 1) {
                       return tempCache[0].TEXT;
                   } else {
                       return paraValue;
                   }
               }
           };

           $scope.convertCodeToName = function (paraValue, moduleName) {
               if (paraValue) {
                   var tempCache = $filter('filter')($scope.tempData(moduleName), { VALUE: paraValue }, true);
                   if (tempCache && tempCache.length === 1) {
                       return tempCache[0].VALUE + ' | ' + tempCache[0].TEXT;
                   } else {
                       return paraValue;
                   }
               }
           };
           //Function load data catalog LoaiHang
           function loadDataMatHangTrongBoHang() {
               $scope.matHangTrongBo = [];
               if (!tempDataService.tempData('matHangTrongBo')) {
                   boHangService.getMatHangTrongBo($scope.target.MABOHANG).then(function (successRes) {
                       if (successRes && successRes.status === 200 && successRes.data && successRes.data.Status && successRes.data.Data && successRes.data.Data.length > 0) {
                           tempDataService.putTempData('matHangTrongBo', successRes.data.Data);
                           $scope.matHangTrongBo = successRes.data.Data;
                       }
                   }, function (errorRes) {
                       console.log('errorRes', errorRes);
                   });
               } else {
                   $scope.matHangTrongBo = tempDataService.tempData('matHangTrongBo');
               }
           };
           loadDataMatHangTrongBoHang();
           //end

           function filterData() {
               service.getDetails($scope.target.MALOAIPHONG).then(function (sucessRes) {
                   if (sucessRes && sucessRes.status === 200 && sucessRes.data && sucessRes.data.Status && sucessRes.data.Data) {
                       $scope.target = sucessRes.data.Data;
                   }
               });
           };
           filterData();

           $scope.save = function () {
               cauHinhLoaiPhongService.post($scope.target).then(function (successRes) {
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
           };

           $scope.cancel = function () {
               $uibModalInstance.close();
           };
       }]);

    app.controller('loaiPhongDelete_Ctrl', ['$scope', '$uibModalInstance', '$http', 'configService', 'loaiPhongService', 'targetData', 'tempDataService', '$filter', '$uibModal', '$log',
       function ($scope, $uibModalInstance, $http, configService, service, targetData, tempDataService, $filter, $uibModal, $log) {
           $scope.config = angular.copy(configService);
           $scope.tempData = tempDataService.tempData;
           $scope.target = {};
           $scope.target = angular.copy(targetData);;
           $scope.title = function () { return 'Xóa loại phòng [' + targetData.MALOAIPHONG + ']'; };
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