define(['angular', 'controllers/catalog/loaiHangController', 'controllers/catalog/nhomHangController', 'controllers/catalog/nhaCungCapController', 'controllers/catalog/donViTinhController', 'controllers/catalog/keHangController', 'controllers/catalog/thueController'], function () {
    var app = angular.module('tempDataModule', ['loaiHangModule', 'nhomHangModule', 'nhaCungCapModule', 'donViTinhModule', 'keHangModule', 'thueModule']);
    app.factory('tempDataService', ['CacheFactory', 'loaiHangService', 'nhomHangService', 'nhaCungCapService', 'donViTinhService', 'keHangService', 'thueService', function (CacheFactory, loaiHangService, nhomHangService, nhaCungCapService, donViTinhService, keHangService, thueService) {
        var profileCache;
        if (!CacheFactory.get('profileCache')) {
            profileCache = CacheFactory('profileCache');
        }
        profileCache.put('status', [
            {
                TEXT: 'Sử dụng',
                VALUE: 10
            },
            {
                TEXT: 'Không sử dụng',
                VALUE: 0
            }
        ]);
        profileCache.put('statusClosingOut', [
            {
                TEXT: 'Đã khóa',
                VALUE: 10
            },
            {
                TEXT: 'Chưa khóa',
                VALUE: 0
            }
        ]);
        profileCache.put('trangThaiChungTu', [
            {
                TEXT: 'Đã duyệt',
                VALUE: 10
            },
            {
                TEXT: 'Lưu tạm',
                VALUE: 0
            }
        ]);
        var result = {
            dateFormat: 'dd/MM/yyyy',
            delegateEvent: function ($event) {
                $event.preventDefault();
                $event.stopPropagation();
            }
        };
        result.tempData = function (name) {
            return profileCache.get(name);
        }
        result.putTempData = function (module, data) {
            profileCache.put(module, data);
        }
        result.update = function (module, data) {
            profileCache.put(module, data);
        }
        result.remove = function (module) {
            profileCache.remove(module);
        }
        result.removeAll = function (module) {
            profileCache.removeAll();
        }
        result.dispose = function (module) {
            profileCache.destroy();
        }
        result.refreshData = function () {
            //Function load data catalog LoaiHang
            loaiHangService.getAllData().then(function (successRes) {
                if (successRes && successRes.status === 200 && successRes.data && successRes.data.Status && successRes.data.Data && successRes.data.Data.length > 0) {
                    profileCache.put('loaiHang', successRes.data.Data);
                }
            });
            //end
            //Function load data catalog NhomHang
            nhomHangService.getAllData().then(function (successRes) {
                if (successRes && successRes.status === 200 && successRes.data && successRes.data.Status && successRes.data.Data && successRes.data.Data.length > 0) {
                    profileCache.put('nhomHang', successRes.data.Data);
                }
            });
            //end

            //Function load data catalog NhaCungCap
            nhaCungCapService.getAllData().then(function (successRes) {
                if (successRes && successRes.status === 200 && successRes.data && successRes.data.Status && successRes.data.Data && successRes.data.Data.length > 0) {
                    profileCache.put('nhaCungCap', successRes.data.Data);
                }
            });
            //end
            //Function load data catalog DonViTinh
            donViTinhService.getAllData().then(function (successRes) {
                if (successRes && successRes.status === 200 && successRes.data && successRes.data.Status && successRes.data.Data && successRes.data.Data.length > 0) {
                    profileCache.put('donViTinh', successRes.data.Data);
                }
            });
            //end

            //Function load data catalog DonViTinh
            keHangService.getAllData().then(function (successRes) {
                if (successRes && successRes.status === 200 && successRes.data && successRes.data.Status && successRes.data.Data && successRes.data.Data.length > 0) {
                    profileCache.put('keHang', successRes.data.Data);
                }
            });
            //end
            //Function load data catalog DonViTinh
            thueService.getAllData().then(function (successRes) {
                if (successRes && successRes.status === 200 && successRes.data && successRes.data.Status && successRes.data.Data && successRes.data.Data.length > 0) {
                    profileCache.put('thue', successRes.data.Data);
                }
            });
            //end
            Lobibox.notify('success', {
                title: 'Thông báo',
                width: 400,
                msg: 'Làm mới dữ liệu thành công',
                delay: 1300
            });
        }
        return result;
    }
    ]);
    return app;
});
