define([
], function () {
    var hostname = window.location.hostname;
    var port = window.location.port;
    var rootUrl = 'http://' + hostname + ':' + port;
    var layoutUrl = rootUrl + "/ERBus/views/authorize/";
    var controlUrl = rootUrl + "/ERBus/controllers/authorize/";
    var states = [
        {
            name: 'NguoiDung',
            url: '/user',
            parent: 'layout',
            abstract: false,
            views: {
                'viewMain@root': {
                    templateUrl: layoutUrl + "/NguoiDung/index.html",
                    controller: "NguoiDung_Ctrl as ctrl"
                }
            },
            moduleUrl: controlUrl + "nguoiDungController.js"
        },
        {
            name: 'ThamSoHeThong',
            url: '/params',
            parent: 'layout',
            abstract: false,
            views: {
                'viewMain@root': {
                    templateUrl: layoutUrl + "/ThamSoHeThong/index.html",
                    controller: "ThamSoHeThong_Ctrl as ctrl"
                }
            },
            moduleUrl: controlUrl + "thamSoHeThongController.js"
        },
        {
            name: 'KyKeToan',
            url: '/period',
            parent: 'layout',
            abstract: false,
            views: {
                'viewMain@root': {
                    templateUrl: layoutUrl + "/KyKeToan/index.html",
                    controller: "KyKeToan_Ctrl as ctrl"
                }
            },
            moduleUrl: controlUrl + "kyKeToanController.js"
        },
        {
            name: 'CuaHang',
            url: '/store',
            parent: 'layout',
            abstract: false,
            views: {
                'viewMain@root': {
                    templateUrl: layoutUrl + "/CuaHang/index.html",
                    controller: "CuaHang_Ctrl as ctrl"
                }
            },
            moduleUrl: controlUrl + "cuaHangController.js"
        },
        {
            name: 'NhomQuyen',
            url: '/groupAu',
            parent: 'layout',
            abstract: false,
            views: {
                'viewMain@root': {
                    templateUrl: layoutUrl + "/NhomQuyen/index.html",
                    controller: "NhomQuyen_Ctrl as ctrl"
                }
            },
            moduleUrl: controlUrl + "nhomQuyenController.js"
        }
    ];
    return states;
});