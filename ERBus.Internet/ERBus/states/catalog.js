define([], function () {
    var hostname = window.location.hostname;
    var port = window.location.port;
    var rootUrl = 'http://' + hostname + ':' + port;
    var layoutUrl = rootUrl+ "/ERBus/views/catalog/";
    var controlUrl = rootUrl + "/ERBus/controllers/catalog/";
    var states = [
        // Catalog Loại Hàng
        {
            name: 'LoaiHang',
            url: '/type',
            parent: 'layout',
            abstract: false,
            views: {
                'viewMain@root': {
                    templateUrl: layoutUrl + "LoaiHang/index.html",
                    controller: "LoaiHang_Ctrl as ctrl"
                }
            },
            moduleUrl: controlUrl + "loaiHangController.js"
        },
        // Catalog Bao Bì
        {
            name: 'BaoBi',
            url: '/pakage',
            parent: 'layout',
            abstract: false,
            views: {
                'viewMain@root': {
                    templateUrl: layoutUrl + "BaoBi/index.html",
                    controller: "BaoBi_Ctrl as ctrl"
                }
            },
            moduleUrl: controlUrl + "baoBiController.js"
        },
        // Catalog Nhóm Hàng
        {
            name: 'NhomHang',
            url: '/commodity',
            parent: 'layout',
            abstract: false,
            views: {
                'viewMain@root': {
                    templateUrl: layoutUrl + "NhomHang/index.html",
                    controller: "NhomHang_Ctrl as ctrl"
                }
            },
            moduleUrl: controlUrl + "nhomHangController.js"
        },
        // Catalog Bó Hàng
        {
            name: 'BoHang',
            url: '/pakage',
            parent: 'layout',
            abstract: false,
            views: {
                'viewMain@root': {
                    templateUrl: layoutUrl + "BoHang/index.html",
                    controller: "BoHang_Ctrl as ctrl"
                }
            },
            moduleUrl: controlUrl + "boHangController.js"
        },
        // Catalog Đơn Vị Tính
        {
            name: 'DonViTinh',
            url: '/unit',
            parent: 'layout',
            abstract: false,
            views: {
                'viewMain@root': {
                    templateUrl: layoutUrl + "DonViTinh/index.html",
                    controller: "DonViTinh_Ctrl as ctrl"
                }
            },
            moduleUrl: controlUrl + "donViTinhController.js"
        },
        // Catalog Hạng Khách Hàng
        {
            name: 'HangKhachHang',
            url: '/rating',
            parent: 'layout',
            abstract: false,
            views: {
                'viewMain@root': {
                    templateUrl: layoutUrl + "HangKhachHang/index.html",
                    controller: "HangKhachHang_Ctrl as ctrl"
                }
            },
            moduleUrl: controlUrl + "hangKhachHangController.js"
        },
        // Catalog Kệ Hàng
        {
            name: 'KeHang',
            url: '/shelf',
            parent: 'layout',
            abstract: false,
            views: {
                'viewMain@root': {
                    templateUrl: layoutUrl + "KeHang/index.html",
                    controller: "KeHang_Ctrl as ctrl"
                }
            },
            moduleUrl: controlUrl + "keHangController.js"
        },
        // Catalog Thuế
        {
            name: 'Thue',
            url: '/tax',
            parent: 'layout',
            abstract: false,
            views: {
                'viewMain@root': {
                    templateUrl: layoutUrl + "Thue/index.html",
                    controller: "Thue_Ctrl as ctrl"
                }
            },
            moduleUrl: controlUrl + "thueController.js"
        },
        // Catalog Kho Hàng
        {
            name: 'KhoHang',
            url: '/warehouse',
            parent: 'layout',
            abstract: false,
            views: {
                'viewMain@root': {
                    templateUrl: layoutUrl + "KhoHang/index.html",
                    controller: "KhoHang_Ctrl as ctrl"
                }
            },
            moduleUrl: controlUrl + "khoHangController.js"
        },
        // Catalog Khách Hàng
        {
            name: 'KhachHang',
            url: '/customer',
            parent: 'layout',
            abstract: false,
            views: {
                'viewMain@root': {
                    templateUrl: layoutUrl + "KhachHang/index.html",
                    controller: "KhachHang_Ctrl as ctrl"
                }
            },
            moduleUrl: controlUrl + "khachHangController.js"
        },
        // Catalog Nhà Cung Cấp
        {
            name: 'NhaCungCap',
            url: '/supplier',
            parent: 'layout',
            abstract: false,
            views: {
                'viewMain@root': {
                    templateUrl: layoutUrl + "NhaCungCap/index.html",
                    controller: "NhaCungCap_Ctrl as ctrl"
                }
            },
            moduleUrl: controlUrl + "nhaCungCapController.js"
        },
        // Catalog Mặt Hàng
        {
            name: 'MatHang',
            url: '/product',
            parent: 'layout',
            abstract: false,
            views: {
                'viewMain@root': {
                    templateUrl: layoutUrl + "MatHang/index.html",
                    controller: "MatHang_Ctrl as ctrl"
                }
            },
            moduleUrl: controlUrl + "matHangController.js"
        },
        // Catalog Phòng
        {
            name: 'Phong',
            url: '/room',
            parent: 'layout',
            abstract: false,
            views: {
                'viewMain@root': {
                    templateUrl: layoutUrl + "Phong/index.html",
                    controller: "Phong_Ctrl as ctrl"
                }
            },
            moduleUrl: controlUrl + "phongController.js"
        },
        // Catalog Loại Phòng
        {
            name: 'LoaiPhong',
            url: '/typeRoom',
            parent: 'layout',
            abstract: false,
            views: {
                'viewMain@root': {
                    templateUrl: layoutUrl + "LoaiPhong/index.html",
                    controller: "LoaiPhong_Ctrl as ctrl"
                }
            },
            moduleUrl: controlUrl + "loaiPhongController.js"
        }
    ];
    return states;
});