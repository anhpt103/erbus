define(['angular'], function (angular) {
    var app = angular.module('common-filter', []);
    app.filter("statusClosingOut", ['$filter', function ($filter) {
        return function (input) {
            var mess = input;
            switch (input) {
                case 10:
                    mess = "<span class='badge badge-success customize'><i class=\"fa fa-check\"></i>&nbsp;Đã khóa</span>";
                    break;
                case 0:
                    mess = "<span class='badge badge-warning customize'><i class=\"fa fa-times\"></i>&nbsp;Chưa khóa</span>";
                    break;
                default:
                    mess = input;
            }
            return mess;
        };
    }]);

    app.filter('displayBool', function () {
        return function (input) {
            if (input === 1) {
                return "<i class='glyphicon glyphicon-ok text-success'></i>";
            }
        }
    });
    return app;
});