define(['angular'], function () {
    'use strict';
    var app = angular.module('configModule', []);
    app.factory('configService', function () {
        var hostname = window.location.hostname;
        var port = window.location.port;
        var rootUrl = 'http://' + hostname + ':' + port;
        var rootUrlApi = 'http://tuananh:6868';
        if (!port) {
            rootUrl = 'http://' + hostname;
        }
        var result = {
            rootUrlWeb: rootUrl,
            rootUrlWebApi: rootUrlApi + '/api',
            apiServiceBaseUri: rootUrlApi,
            dateFormat: 'dd/MM/yyyy',
            delegateEvent: function ($event) {
                $event.preventDefault();
                $event.stopPropagation();
            },
            moment: require('moment')
        };
        result.buildUrl = function (folder, file) {
            return this.rootUrlWeb + "/ERBus/views/" + folder + "/" + file + ".html";
        };
        result.pageDefault = {
            TotalItems: 0,
            ItemsPerPage: 10,
            CurrentPage: 1,
            PageSize: 5,
            TotalPages: 5,
            MaxSize: 5
        };
        result.filterDefault = {
            summary: '',
            isAdvance: false,
            advanceData: {},
            orderBy: '',
            orderType: 'ASC',
            UNITCODE: '',
            PARENT_UNITCODE: ''
        };
        result.saveExcel = function (data, fileName) {
            var fileName = fileName + ".xls"
            var filetype = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet;charset=utf-8";
            var ieEDGE = navigator.userAgent.match(/Edge/g);
            var ie = navigator.userAgent.match(/.NET/g); // IE 11+
            var oldIE = navigator.userAgent.match(/MSIE/g);
            if (ie || oldIE || ieEDGE) {
                var blob = new window.Blob(data, { type: filetype });
                window.navigator.msSaveBlob(blob, fileName);
            }
            else {
                var a = $("<a style='display: none;'/>");
                var url = window.url.createObjectURL(new Blob(data, { type: filetype }));
                a.attr("href", url);
                a.attr("download", fileName);
                $("body").append(a);
                a[0].click();
                window.url.revokeObjectURL(url);
                a.remove();
            }
        }
        var label = {
            lblMessage: 'Thông báo',
            lblNotifications: 'Thông báo',
            lblindex: '',
            lblDetails: 'Thông tin',
            lblEdit: 'Cập nhập',
            lblCreate: 'Thêm',
            lbl: '',
            btnCreate: 'Thêm mới',
            btnImport: 'Cập nhật từ tệp excel',
            btnDetail: 'Thông tin',
            btnEdit: 'Cập nhật',
            btnDelete: 'Xóa',
            btnActive: 'Active',
            btnToggle: 'Toggle',
            btnClosing: 'Khóa sổ',
            btnSaveAndKeep: 'Lưu và giữ lại',
            btnSaveAndPrint: 'Lưu và in phiếu',
            btnReport: 'Báo cáo tổng',
            btnDetailReport: 'Báo cáo chi tiết',
            btnChanged: 'Chọn',
            btnSettingGroup: 'Phân nhóm',
            btnSettingGrant: 'Phân quyền',
            btnPrintItem: 'In tem',
            btnPrintItemShelves: 'In tem kệ',
            btnActionPrint: 'In',
            btnPrint: 'In phiếu',
            btnSearch: 'Tìm kiếm',
            btnRefresh: 'Làm mới',
            btnBack: 'Quay lại',
            btnClear: 'Xóa tất cả',
            btnCancel: 'Thoát',

            btnSave: 'Lưu lại',
            btnSubmit: 'Lưu',

            btnLogOn: 'Đăng nhập',
            btnLogOff: 'Đăng xuất',
            btnChangePassword: 'Đổi mật khẩu',

            btnSendMessage: 'Gửi tin nhắn',
            btnSendNotification: 'Gửi thông báo',
            btnNotifications: 'Thông báo',

            btnImportExcel: 'Import Excel',
            btnExportExcel: 'Xuất Excel',

            btnCall: 'Call',
            btnChart: 'Biểu đồ',
            btnData: 'Số liệu',
           
            btnExit: 'Thoát',
            btnExportPDF: 'Kết xuất file PDF',
            btnExport: 'Kết xuất',

           
            btnPrintDetailList: 'In DS chi tiết',
            btnSend: 'DS duyệt',
            btnApproval: 'Duyệt',
            btnUnApproval: 'Bỏ duyệt',
            btnComplete: 'Hoàn thành',
            btnAddInfo: 'Bổ sung',
            btnOrder: 'Sắp xếp'
        };
        result.label = label;
        return result;
    }
    );
    return app;
});
