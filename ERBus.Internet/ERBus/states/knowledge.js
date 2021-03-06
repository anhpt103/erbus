﻿define([], function () {
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
        },
        // in phiếu chứng từ
        {
            name: 'printPhieuXuatBan',
            url: '/printPhieuXuatBan?{postParam:configParams}',
            parent: 'layout',
            abstract: false,
            views: {
                'viewMain@root': {
                    templateUrl: layoutUrl + "XuatBan/print.html",
                    controller: "XuatBanPrint_Ctrl as ctrl"
                }
            },
            moduleUrl: controlUrl + "xuatBanController.js"
        },
         // Knowledge Đặt phòng
        {
            name: 'DatPhong',
            url: '/booking',
            parent: 'layout',
            abstract: false,
            views: {
                'viewMain@root': {
                    templateUrl: layoutUrl + "DatPhong/index.html",
                    controller: "DatPhong_Ctrl as ctrl"
                }
            },
            moduleUrl: controlUrl + "datPhongController.js"
        },
         // Knowledge Thanh toán đặt phòng
        {
            name: 'ThanhToanDatPhong',
            url: '/dischargeRoom',
            parent: 'layout',
            abstract: false,
            views: {
                'viewMain@root': {
                    templateUrl: layoutUrl + "ThanhToanDatPhong/index.html",
                    controller: "ThanhToanDatPhong_Ctrl as ctrl"
                }
            },
            moduleUrl: controlUrl + "thanhToanDatPhongController.js"
        }
        ,
         /*Knowledge sắp xếp mặt hàng thanh toán*/
        {
            name: 'SapXepMatHang',
            url: '/orderMerchandise',
            parent: 'layout',
            abstract: false,
            views: {
                'viewMain@root': {
                    templateUrl: layoutUrl + "SapXepMatHang/index.html",
                    controller: "SapXepMatHang_Ctrl as ctrl"
                }
            },
            moduleUrl: controlUrl + "sapXepMatHangController.js"
        }
    ];
    return states;
});