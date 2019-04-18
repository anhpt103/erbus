define([
], function () {
    var hostname = window.location.hostname;
    var port = window.location.port;
    var rootUrl = 'http://' + hostname + ':' + port;
    var layoutUrl = rootUrl + "/ERBus/views/report/";
    var controlUrl = rootUrl + "/ERBus/controllers/report/";
    var states = [
        // Knowledge Tồn kho
        {
            name: 'BaoCaoTonKho',
            url: '/instock',
            parent: 'layout',
            abstract: false,
            views: {
                'viewMain@root': {
                    templateUrl: layoutUrl + "TonKho/index.html",
                    controller: "BaoCaoTonKho_Ctrl as ctrl"
                }
            },
            moduleUrl: controlUrl + "tonKhoReportController.js"
        },
        {
            name: 'BaoCaoXuatBanLe',
            url: '/retails',
            parent: 'layout',
            abstract: false,
            views: {
                'viewMain@root': {
                    templateUrl: layoutUrl + "XuatBanLe/index.html",
                    controller: "BaoCaoXuatBanLe_Ctrl as ctrl"
                }
            },
            moduleUrl: controlUrl + "xuatBanLeReportController.js"
        },
        {
            name: 'BaoCaoNhapMua',
            url: '/inputBuyReport',
            parent: 'layout',
            abstract: false,
            views: {
                'viewMain@root': {
                    templateUrl: layoutUrl + "NhapMua/index.html",
                    controller: "BaoCaoNhapMua_Ctrl as ctrl"
                }
            },
            moduleUrl: controlUrl + "nhapMuaReportController.js"
        },
        {
            name: 'BaoCaoXuatNhapTon',
            url: '/inputOutput',
            parent: 'layout',
            abstract: false,
            views: {
                'viewMain@root': {
                    templateUrl: layoutUrl + "XuatNhapTon/index.html",
                    controller: "BaoCaoXuatNhapTon_Ctrl as ctrl"
                }
            },
            moduleUrl: controlUrl + "xuatNhapTonReportController.js"
        }
    ];
    return states;
});