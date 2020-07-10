define(['angular', 'controllers/authorize/authController', 'controllers/authorize/menuController', 'controllers/authorize/nguoiDungController'], function (angular) {
    var app = angular.module('headerModule', ['authModule', 'configModule', 'menuModule', 'nguoiDungModule']);
    var layoutUrl = "/ERBus/";

    app.directive('tree', function () {
        return {
            restrict: "E",
            replace: true,
            scope: {
                tree: '='
            },
            templateUrl: layoutUrl + 'utils/tree/template-ul.html'
        };
    });
    app.directive('leaf', function ($compile) {
        return {
            restrict: "E",
            replace: true,
            scope: {
                leaf: "="
            },
            templateUrl: layoutUrl + 'utils/tree/template-li.html',
            link: function (scope, element, attrs) {
                if (angular.isArray(scope.leaf.CHILDREN) && scope.leaf.CHILDREN.length > 0) {
                    element.append("<tree tree='leaf.CHILDREN'></tree>");
                    element.addClass('list-unstyled navbar__sub-list js-sub-list');
                    $compile(element.contents())(scope);
                }
            }
        };
    });
    app.controller('Header_Ctrl', ['$scope', 'configService', '$state', 'accountService', '$filter', 'userService', 'nguoiDungService',
        function ($scope, configService, $state, accountService, $filter, userService, nguoiDungService) {
            $scope.rootUrlHome = configService.rootUrl;
            $scope.currentUser = userService.GetCurrentUser();
            //khởi tạo configService UNITCODE; PARENT_UNITCODE
            if ($scope.currentUser) {
                configService.filterDefault.UNITCODE = $scope.currentUser.unitCode;
                configService.filterDefault.PARENT_UNITCODE = $scope.currentUser.parentUnitCode;
            }
            //end
            function treeify(list, idAttr, parentAttr, childrenAttr) {
                if (!idAttr) idAttr = 'VALUE';
                if (!parentAttr) parentAttr = 'PARENT';
                if (!childrenAttr) childrenAttr = 'CHILDREN';
                var lookup = {};
                var result = {};
                result[childrenAttr] = [];
                list.forEach(function (obj) {
                    lookup[obj[idAttr]] = obj;
                    obj[childrenAttr] = [];
                });
                list.forEach(function (obj) {
                    if (obj[parentAttr] != null) {
                        try { lookup[obj[parentAttr]][childrenAttr].push(obj); }
                        catch (err) {
                            result[childrenAttr].push(obj);
                        }

                    } else {
                        result[childrenAttr].push(obj);
                    }
                });
                return result;
            };
            $scope.linkHref = function () {
                $state.go('home');
            };
            function loadUser() {
                if (!$scope.currentUser) {
                    $state.go('login');
                } else {
                    let listMenu = [
                        {
                            "PARENT": null,
                            "DESCRIPTION": null,
                            "OLD_SELECTED": false,
                            "REFERENCE_DATA_ID": null,
                            "ID": "ce80e206-f391-4564-977f-589adfa87f0e",
                            "VALUE": "HeThong",
                            "GIATRI": 0,
                            "EXTEND_VALUE": "fas fa-tachometer-alt",
                            "TEXT": "1. Hệ thống",
                            "SELECTED": false,
                            "INFOMATION": null,
                            "ISVISIBLE": true
                        },
                        {
                            "PARENT": "HeThong",
                            "DESCRIPTION": null,
                            "OLD_SELECTED": false,
                            "REFERENCE_DATA_ID": null,
                            "ID": "8117d8ec-b3ee-4abd-8621-806617bdee5e",
                            "VALUE": "ThamSoHeThong",
                            "GIATRI": 0,
                            "EXTEND_VALUE": null,
                            "TEXT": "1.1 Tham số hệ thống",
                            "SELECTED": false,
                            "INFOMATION": null,
                            "ISVISIBLE": false
                        },
                        {
                            "PARENT": "HeThong",
                            "DESCRIPTION": null,
                            "OLD_SELECTED": false,
                            "REFERENCE_DATA_ID": null,
                            "ID": "843b8e77-082f-42b4-865d-f1016a45dea7",
                            "VALUE": "KyKeToan",
                            "GIATRI": 0,
                            "EXTEND_VALUE": null,
                            "TEXT": "1.2 Kỳ kế toán",
                            "SELECTED": false,
                            "INFOMATION": null,
                            "ISVISIBLE": false
                        },
                        {
                            "PARENT": "HeThong",
                            "DESCRIPTION": null,
                            "OLD_SELECTED": false,
                            "REFERENCE_DATA_ID": null,
                            "ID": "5c703d1e-1303-4777-8c05-4142b3382f82",
                            "VALUE": "Menu",
                            "GIATRI": 0,
                            "EXTEND_VALUE": null,
                            "TEXT": "1.3 Menu",
                            "SELECTED": false,
                            "INFOMATION": null,
                            "ISVISIBLE": false
                        },
                        {
                            "PARENT": "HeThong",
                            "DESCRIPTION": null,
                            "OLD_SELECTED": false,
                            "REFERENCE_DATA_ID": null,
                            "ID": "744c688e-8a4f-441d-9905-8a9eb610e187",
                            "VALUE": "NguoiDung",
                            "GIATRI": 0,
                            "EXTEND_VALUE": null,
                            "TEXT": "1.4 Tài khoản người dùng",
                            "SELECTED": false,
                            "INFOMATION": null,
                            "ISVISIBLE": true
                        },
                        {
                            "PARENT": "HeThong",
                            "DESCRIPTION": null,
                            "OLD_SELECTED": false,
                            "REFERENCE_DATA_ID": null,
                            "ID": "0c5ac021-1bef-4885-8906-85f01ee966c9",
                            "VALUE": "NhomQuyen",
                            "GIATRI": 0,
                            "EXTEND_VALUE": null,
                            "TEXT": "1.5 Nhóm quyền",
                            "SELECTED": false,
                            "INFOMATION": null,
                            "ISVISIBLE": true
                        },
                        {
                            "PARENT": "HeThong",
                            "DESCRIPTION": null,
                            "OLD_SELECTED": false,
                            "REFERENCE_DATA_ID": null,
                            "ID": "096240dd-6qqe-4051-82e3-c3a54273089a",
                            "VALUE": "CuaHang",
                            "GIATRI": 0,
                            "EXTEND_VALUE": null,
                            "TEXT": "1.6 Thông tin cửa hàng",
                            "SELECTED": false,
                            "INFOMATION": null,
                            "ISVISIBLE": true
                        },
                        {
                            "PARENT": null,
                            "DESCRIPTION": null,
                            "OLD_SELECTED": false,
                            "REFERENCE_DATA_ID": null,
                            "ID": "a2f4b867-e879-4678-ba7a-41fcbc6eee99",
                            "VALUE": "DanhMuc",
                            "GIATRI": 0,
                            "EXTEND_VALUE": "fas fa-shopping-basket",
                            "TEXT": "2. Danh mục",
                            "SELECTED": false,
                            "INFOMATION": null,
                            "ISVISIBLE": true
                        },
                        {
                            "PARENT": "DanhMuc",
                            "DESCRIPTION": null,
                            "OLD_SELECTED": false,
                            "REFERENCE_DATA_ID": null,
                            "ID": "f1e14d02-0fa6-4181-8393-dffb5b107323",
                            "VALUE": "LoaiHang",
                            "GIATRI": 0,
                            "EXTEND_VALUE": null,
                            "TEXT": "2.1 Loại hàng hóa",
                            "SELECTED": false,
                            "INFOMATION": null,
                            "ISVISIBLE": false
                        },
                        {
                            "PARENT": "DanhMuc",
                            "DESCRIPTION": null,
                            "OLD_SELECTED": false,
                            "REFERENCE_DATA_ID": null,
                            "ID": "3be49267-5a86-4ea8-8abc-632d9d4f312e",
                            "VALUE": "NhomHang",
                            "GIATRI": 0,
                            "EXTEND_VALUE": null,
                            "TEXT": "2.2 Nhóm hàng hóa",
                            "SELECTED": false,
                            "INFOMATION": null,
                            "ISVISIBLE": false
                        },
                        {
                            "PARENT": "DanhMuc",
                            "DESCRIPTION": null,
                            "OLD_SELECTED": false,
                            "REFERENCE_DATA_ID": null,
                            "ID": "04d82a03-a376-4dbc-a867-c9eebd261d52",
                            "VALUE": "MatHang",
                            "GIATRI": 0,
                            "EXTEND_VALUE": null,
                            "TEXT": "2.3 Hàng hóa",
                            "SELECTED": false,
                            "INFOMATION": null,
                            "ISVISIBLE": true
                        },
                        {
                            "PARENT": "DanhMuc",
                            "DESCRIPTION": null,
                            "OLD_SELECTED": false,
                            "REFERENCE_DATA_ID": null,
                            "ID": "78a97e8d-ec45-4982-96e8-b3d8df564dd7",
                            "VALUE": "BaoBi",
                            "GIATRI": 0,
                            "EXTEND_VALUE": null,
                            "TEXT": "2.4 Bao bì",
                            "SELECTED": false,
                            "INFOMATION": null,
                            "ISVISIBLE": false
                        },
                        {
                            "PARENT": "DanhMuc",
                            "DESCRIPTION": null,
                            "OLD_SELECTED": false,
                            "REFERENCE_DATA_ID": null,
                            "ID": "f639de7d-f01f-4701-8c7b-b985b6536e3e",
                            "VALUE": "KhoHang",
                            "GIATRI": 0,
                            "EXTEND_VALUE": null,
                            "TEXT": "2.5 Kho hàng",
                            "SELECTED": false,
                            "INFOMATION": null,
                            "ISVISIBLE": false
                        },
                        {
                            "PARENT": "DanhMuc",
                            "DESCRIPTION": null,
                            "OLD_SELECTED": false,
                            "REFERENCE_DATA_ID": null,
                            "ID": "fb01d80a-9cfb-42b1-b947-64d941b844cd",
                            "VALUE": "BoHang",
                            "GIATRI": 0,
                            "EXTEND_VALUE": null,
                            "TEXT": "2.6 Bó hàng",
                            "SELECTED": false,
                            "INFOMATION": null,
                            "ISVISIBLE": false
                        },
                        {
                            "PARENT": "DanhMuc",
                            "DESCRIPTION": null,
                            "OLD_SELECTED": false,
                            "REFERENCE_DATA_ID": null,
                            "ID": "da123785-eb2b-4191-93f4-582d0272a53e",
                            "VALUE": "NhaCungCap",
                            "GIATRI": 0,
                            "EXTEND_VALUE": null,
                            "TEXT": "2.7 Nhà cung cấp",
                            "SELECTED": false,
                            "INFOMATION": null,
                            "ISVISIBLE": true
                        },
                        {
                            "PARENT": "DanhMuc",
                            "DESCRIPTION": null,
                            "OLD_SELECTED": false,
                            "REFERENCE_DATA_ID": null,
                            "ID": "5ba6ec62-d50e-4120-bcd8-5bbc83b20afa",
                            "VALUE": "KhachHang",
                            "GIATRI": 0,
                            "EXTEND_VALUE": null,
                            "TEXT": "2.8 Khách hàng",
                            "SELECTED": false,
                            "INFOMATION": null,
                            "ISVISIBLE": false
                        },
                        {
                            "PARENT": "DanhMuc",
                            "DESCRIPTION": null,
                            "OLD_SELECTED": false,
                            "REFERENCE_DATA_ID": null,
                            "ID": "97ytd785-eb2b-4191-93f4-582d0272a53e",
                            "VALUE": "DonViTinh",
                            "GIATRI": 0,
                            "EXTEND_VALUE": null,
                            "TEXT": "2.9 Đơn vị tính",
                            "SELECTED": false,
                            "INFOMATION": null,
                            "ISVISIBLE": true
                        },
                        {
                            "PARENT": "DanhMuc",
                            "DESCRIPTION": null,
                            "OLD_SELECTED": false,
                            "REFERENCE_DATA_ID": null,
                            "ID": "fa198715-eb2b-4191-93f4-582d0272a53e",
                            "VALUE": "HangKhachHang",
                            "GIATRI": 0,
                            "EXTEND_VALUE": null,
                            "TEXT": "2.10 Hạng khách hàng",
                            "SELECTED": false,
                            "INFOMATION": null,
                            "ISVISIBLE": false
                        },
                        {
                            "PARENT": "DanhMuc",
                            "DESCRIPTION": null,
                            "OLD_SELECTED": false,
                            "REFERENCE_DATA_ID": null,
                            "ID": "fa5cdb82-c911-4545-93d8-1d9014fdcf06",
                            "VALUE": "KeHang",
                            "GIATRI": 0,
                            "EXTEND_VALUE": null,
                            "TEXT": "2.11 Kệ hàng",
                            "SELECTED": false,
                            "INFOMATION": null,
                            "ISVISIBLE": false
                        },
                        {
                            "PARENT": "DanhMuc",
                            "DESCRIPTION": null,
                            "OLD_SELECTED": false,
                            "REFERENCE_DATA_ID": null,
                            "ID": "59c6339c-e493-4325-8a57-c6ccb5d56c07",
                            "VALUE": "Thue",
                            "GIATRI": 0,
                            "EXTEND_VALUE": null,
                            "TEXT": "2.12 Loại thuế",
                            "SELECTED": false,
                            "INFOMATION": null,
                            "ISVISIBLE": false
                        },
                        {
                            "PARENT": "DanhMuc",
                            "DESCRIPTION": null,
                            "OLD_SELECTED": false,
                            "REFERENCE_DATA_ID": null,
                            "ID": "4579339c-e493-4325-8a57-c6ccb5d56c31",
                            "VALUE": "LoaiPhong",
                            "GIATRI": 0,
                            "EXTEND_VALUE": null,
                            "TEXT": "2.13 Loại phòng",
                            "SELECTED": false,
                            "INFOMATION": null,
                            "ISVISIBLE": true
                        },
                        {
                            "PARENT": "DanhMuc",
                            "DESCRIPTION": null,
                            "OLD_SELECTED": false,
                            "REFERENCE_DATA_ID": null,
                            "ID": "19c6339c-e493-4325-8a57-c6ccb5d56f36",
                            "VALUE": "Phong",
                            "GIATRI": 0,
                            "EXTEND_VALUE": null,
                            "TEXT": "2.14 Phòng",
                            "SELECTED": false,
                            "INFOMATION": null,
                            "ISVISIBLE": true
                        },
                        {
                            "PARENT": null,
                            "DESCRIPTION": null,
                            "OLD_SELECTED": false,
                            "REFERENCE_DATA_ID": null,
                            "ID": "11fd38ac-7ea9-4a4c-8fad-af41f0ec1cfd",
                            "VALUE": "NghiepVu",
                            "GIATRI": 0,
                            "EXTEND_VALUE": "fas fa-trophy",
                            "TEXT": "3. Nghiệp vụ",
                            "SELECTED": false,
                            "INFOMATION": null,
                            "ISVISIBLE": true
                        },
                        {
                            "PARENT": "NghiepVu",
                            "DESCRIPTION": null,
                            "OLD_SELECTED": false,
                            "REFERENCE_DATA_ID": null,
                            "ID": "c69162f4-da9a-4d9a-9aee-67331d9a6684",
                            "VALUE": "NhapMua",
                            "GIATRI": 0,
                            "EXTEND_VALUE": null,
                            "TEXT": "3.1 Nhập hàng mua",
                            "SELECTED": false,
                            "INFOMATION": null,
                            "ISVISIBLE": true
                        },
                        {
                            "PARENT": "NghiepVu",
                            "DESCRIPTION": null,
                            "OLD_SELECTED": false,
                            "REFERENCE_DATA_ID": null,
                            "ID": "c69161f4-da9a-4d9a-9aee-67331d9a6684",
                            "VALUE": "XuatBan",
                            "GIATRI": 0,
                            "EXTEND_VALUE": null,
                            "TEXT": "3.2 Xuất bán buôn",
                            "SELECTED": false,
                            "INFOMATION": null,
                            "ISVISIBLE": false
                        },
                        {
                            "PARENT": "NghiepVu",
                            "DESCRIPTION": null,
                            "OLD_SELECTED": false,
                            "REFERENCE_DATA_ID": null,
                            "ID": "c69161f4-da9a-4d9a-9aee-67331d9ass84",
                            "VALUE": "XuatBanLeThuNgan",
                            "GIATRI": 0,
                            "EXTEND_VALUE": null,
                            "TEXT": "3.3 Xuất bán lẻ thu ngân",
                            "SELECTED": false,
                            "INFOMATION": null,
                            "ISVISIBLE": false
                        },
                        {
                            "PARENT": "NghiepVu",
                            "DESCRIPTION": null,
                            "OLD_SELECTED": false,
                            "REFERENCE_DATA_ID": null,
                            "ID": "c69161f4-da9a-4d9a-9aee-67331d9ass82",
                            "VALUE": "KiemKeHangHoa",
                            "GIATRI": 0,
                            "EXTEND_VALUE": null,
                            "TEXT": "3.4 Kiểm kê hàng hóa",
                            "SELECTED": false,
                            "INFOMATION": null,
                            "ISVISIBLE": false
                        },
                        {
                            "PARENT": "NghiepVu",
                            "DESCRIPTION": null,
                            "OLD_SELECTED": false,
                            "REFERENCE_DATA_ID": null,
                            "ID": "d49161f4-da9a-4d9a-9aee-67331d9ass12",
                            "VALUE": "DatPhong",
                            "GIATRI": 0,
                            "EXTEND_VALUE": null,
                            "TEXT": "3.5 Đặt phòng",
                            "SELECTED": false,
                            "INFOMATION": null,
                            "ISVISIBLE": true
                        },
                        {
                            "PARENT": "NghiepVu",
                            "DESCRIPTION": null,
                            "OLD_SELECTED": false,
                            "REFERENCE_DATA_ID": null,
                            "ID": "g59161f4-da9a-4d9a-9aee-67331d9ass30",
                            "VALUE": "ThanhToanDatPhong",
                            "GIATRI": 0,
                            "EXTEND_VALUE": null,
                            "TEXT": "3.6 Thanh toán đặt phòng",
                            "SELECTED": false,
                            "INFOMATION": null,
                            "ISVISIBLE": true
                        },
                        {
                            "PARENT": null,
                            "DESCRIPTION": null,
                            "OLD_SELECTED": false,
                            "REFERENCE_DATA_ID": null,
                            "ID": "780551dd-f54a-400f-aa21-a33e5387a4ba",
                            "VALUE": "KhuyenMai",
                            "GIATRI": 0,
                            "EXTEND_VALUE": "fas fa-copy",
                            "TEXT": "4. Khuyến mại",
                            "SELECTED": false,
                            "INFOMATION": null,
                            "ISVISIBLE": false
                        },
                        {
                            "PARENT": "KhuyenMai",
                            "DESCRIPTION": null,
                            "OLD_SELECTED": false,
                            "REFERENCE_DATA_ID": null,
                            "ID": "540551dd-f54a-400f-aa21-a33e5387a4ba",
                            "VALUE": "TienTyLe",
                            "GIATRI": 0,
                            "EXTEND_VALUE": null,
                            "TEXT": "4.1. Khuyến mại tiền, tỷ lệ",
                            "SELECTED": false,
                            "INFOMATION": null,
                            "ISVISIBLE": false
                        },
                        {
                            "PARENT": "KhuyenMai",
                            "DESCRIPTION": null,
                            "OLD_SELECTED": false,
                            "REFERENCE_DATA_ID": null,
                            "ID": "540551dd-f54a-400f-aa21-a33e5387a4bg",
                            "VALUE": "GiamGiaLoaiHang",
                            "GIATRI": 0,
                            "EXTEND_VALUE": null,
                            "TEXT": "4.2. Khuyến mại loại hàng",
                            "SELECTED": false,
                            "INFOMATION": null,
                            "ISVISIBLE": false
                        },
                        {
                            "PARENT": "KhuyenMai",
                            "DESCRIPTION": null,
                            "OLD_SELECTED": false,
                            "REFERENCE_DATA_ID": null,
                            "ID": "540551dd-f54a-400f-aa21-a33e5387a4bk",
                            "VALUE": "GiamGiaNhomHang",
                            "GIATRI": 0,
                            "EXTEND_VALUE": null,
                            "TEXT": "4.3. Khuyến mại nhóm hàng",
                            "SELECTED": false,
                            "INFOMATION": null,
                            "ISVISIBLE": false
                        },
                        {
                            "PARENT": "KhuyenMai",
                            "DESCRIPTION": null,
                            "OLD_SELECTED": false,
                            "REFERENCE_DATA_ID": null,
                            "ID": "540551dd-f54a-400f-aa21-a33e5387a4js",
                            "VALUE": "GiamGiaNhaCungCap",
                            "GIATRI": 0,
                            "EXTEND_VALUE": null,
                            "TEXT": "4.4. Khuyến mại nhà cung cấp",
                            "SELECTED": false,
                            "INFOMATION": null,
                            "ISVISIBLE": false
                        },
                        {
                            "PARENT": "BaoCao",
                            "DESCRIPTION": null,
                            "OLD_SELECTED": false,
                            "REFERENCE_DATA_ID": null,
                            "ID": "1758ab70-3da0-4fcf-a2e2-f733e2c3b6fc",
                            "VALUE": "BaoCaoTonKho",
                            "GIATRI": 0,
                            "EXTEND_VALUE": null,
                            "TEXT": "5.1 Báo cáo tồn kho",
                            "SELECTED": false,
                            "INFOMATION": null,
                            "ISVISIBLE": true
                        },
                        {
                            "PARENT": "BaoCao",
                            "DESCRIPTION": null,
                            "OLD_SELECTED": false,
                            "REFERENCE_DATA_ID": null,
                            "ID": "1758ab70-1rd0-4fcf-a2e2-f733e2c3b6fc",
                            "VALUE": "BaoCaoXuatNhapTon",
                            "GIATRI": 0,
                            "EXTEND_VALUE": null,
                            "TEXT": "5.2 Báo cáo xuất nhập tồn",
                            "SELECTED": false,
                            "INFOMATION": null,
                            "ISVISIBLE": true
                        },
                        {
                            "PARENT": "BaoCao",
                            "DESCRIPTION": null,
                            "OLD_SELECTED": false,
                            "REFERENCE_DATA_ID": null,
                            "ID": "0c96a843-959e-4284-bee9-0540a91a8aab",
                            "VALUE": "BaoCaoXuatBanLe",
                            "GIATRI": 0,
                            "EXTEND_VALUE": null,
                            "TEXT": "5.3 Báo cáo bán lẻ",
                            "SELECTED": false,
                            "INFOMATION": null,
                            "ISVISIBLE": true
                        },
                        {
                            "PARENT": "BaoCao",
                            "DESCRIPTION": null,
                            "OLD_SELECTED": false,
                            "REFERENCE_DATA_ID": null,
                            "ID": "1c96a843-959e-4284-bee9-0540a91a8aab",
                            "VALUE": "BaoCaoNhapMua",
                            "GIATRI": 0,
                            "EXTEND_VALUE": null,
                            "TEXT": "5.4 Báo cáo nhập",
                            "SELECTED": false,
                            "INFOMATION": null,
                            "ISVISIBLE": true
                        },
                        {
                            "PARENT": "BaoCao",
                            "DESCRIPTION": null,
                            "OLD_SELECTED": false,
                            "REFERENCE_DATA_ID": null,
                            "ID": "2c96a843-959e-4284-bee9-0540a91a8aab",
                            "VALUE": "BaoCaoXuatBanBuon",
                            "GIATRI": 0,
                            "EXTEND_VALUE": null,
                            "TEXT": "5.5 Báo cáo xuất bán buôn",
                            "SELECTED": false,
                            "INFOMATION": null,
                            "ISVISIBLE": false
                        },
                        {
                            "PARENT": null,
                            "DESCRIPTION": null,
                            "OLD_SELECTED": false,
                            "REFERENCE_DATA_ID": null,
                            "ID": "abdbd6ef-0ec4-48e4-b978-6453adcc58e1",
                            "VALUE": "BaoCao",
                            "GIATRI": 0,
                            "EXTEND_VALUE": "fas fa-chart-bar",
                            "TEXT": "5. Báo cáo",
                            "SELECTED": false,
                            "INFOMATION": null,
                            "ISVISIBLE": true
                        }
                    ];
                    $scope.treeMenu = treeify($filter('filter')(listMenu, { ISVISIBLE: true }, true));
                }
            };
            loadUser();

            $scope.showInfoUser = false;
            $scope.detailInfoUser = function () {
                if ($scope.showInfoUser) $scope.showInfoUser = false;
                else $scope.showInfoUser = true;
            }

            $scope.detailAccount = function (currentUser) {
                nguoiDungService.getDetails(currentUser.id).then(function (response) {
                    if (response && response.status === 200 && response.data) {
                        $scope.detailCurrentUser = response.data;
                    }
                });
            };

            $scope.logOut = function () {
                accountService.logout();
            };

        }]);
    return app;
});