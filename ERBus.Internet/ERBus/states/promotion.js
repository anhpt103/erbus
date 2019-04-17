define([], function () {
    var hostname = window.location.hostname;
    var port = window.location.port;
    var rootUrl = 'http://' + hostname + ':' + port;
    var layoutUrl = rootUrl + "/ERBus/views/promotion/";
    var controlUrl = rootUrl + "/ERBus/controllers/promotion/";
    var states = [
        // Khuyến mãi tiền tỷ lệ
        {
            name: 'TienTyLe',
            url: '/discountPercent',
            parent: 'layout',
            abstract: false,
            views: {
                'viewMain@root': {
                    templateUrl: layoutUrl + "TienTyLe/index.html",
                    controller: "TienTyLe_Ctrl as ctrl"
                }
            },
            moduleUrl: controlUrl + "tienTyLeController.js"
        },
        // Khuyến mãi theo loại hàng
        {
            name: 'GiamGiaLoaiHang',
            url: '/discountType',
            parent: 'layout',
            abstract: false,
            views: {
                'viewMain@root': {
                    templateUrl: layoutUrl + "GiamGiaLoaiHang/index.html",
                    controller: "GiamGiaLoaiHang_Ctrl as ctrl"
                }
            },
            moduleUrl: controlUrl + "giamGiaLoaiHangController.js"
        },
        // Khuyến mãi theo nhóm hàng
        {
            name: 'GiamGiaNhomHang',
            url: '/discountGroup',
            parent: 'layout',
            abstract: false,
            views: {
                'viewMain@root': {
                    templateUrl: layoutUrl + "GiamGiaNhomHang/index.html",
                    controller: "GiamGiaNhomHang_Ctrl as ctrl"
                }
            },
            moduleUrl: controlUrl + "giamGiaNhomHangController.js"
        },
        // Khuyến mãi theo nhà cung cấp
        {
            name: 'GiamGiaNhaCungCap',
            url: '/discountSupplier',
            parent: 'layout',
            abstract: false,
            views: {
                'viewMain@root': {
                    templateUrl: layoutUrl + "GiamGiaNhaCungCap/index.html",
                    controller: "GiamGiaNhaCungCap_Ctrl as ctrl"
                }
            },
            moduleUrl: controlUrl + "giamGiaNhaCungCapController.js"
        }
    ];
    return states;
});