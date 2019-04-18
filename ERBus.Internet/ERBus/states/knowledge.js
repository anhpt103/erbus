define([], function () {
    var hostname = window.location.hostname;
    var port = window.location.port;
    var rootUrl = 'http://' + hostname + ':' + port;
    var layoutUrl = rootUrl + "/ERBus/views/knowledge/";
    var controlUrl = rootUrl + "/ERBus/controllers/knowledge/";
    var states = [
        // Knowledge Nhập mua
        {
            name: 'NhapMua',
            url: '/inputBuy',
            parent: 'layout',
            abstract: false,
            views: {
                'viewMain@root': {
                    templateUrl: layoutUrl + "NhapMua/index.html",
                    controller: "NhapMua_Ctrl as ctrl"
                }
            },
            moduleUrl: controlUrl + "nhapMuaController.js"
        },
        // Knowledge Xuất bán
        {
            name: 'XuatBan',
            url: '/outputBuy',
            parent: 'layout',
            abstract: false,
            views: {
                'viewMain@root': {
                    templateUrl: layoutUrl + "XuatBan/index.html",
                    controller: "XuatBan_Ctrl as ctrl"
                }
            },
            moduleUrl: controlUrl + "xuatBanController.js"
        },
        // Knowledge Xuất bán lẻ thu ngân
        {
            name: 'XuatBanLeThuNgan',
            url: '/cashier',
            parent: 'layout',
            abstract: false,
            views: {
                'viewMain@root': {
                    templateUrl: layoutUrl + "XuatBanLeThuNgan/index.html",
                    controller: "XuatBanLeThuNgan_Ctrl as ctrl"
                }
            },
            moduleUrl: controlUrl + "xuatBanLeThuNganController.js"
        }
    ];
    return states;
});