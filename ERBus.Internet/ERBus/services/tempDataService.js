define(['angular'], function () {
    var app = angular.module('tempDataModule', []);
    app.factory('tempDataService', ['CacheFactory', function (CacheFactory) {
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
        return result;
    }
    ]);
    return app;
});
