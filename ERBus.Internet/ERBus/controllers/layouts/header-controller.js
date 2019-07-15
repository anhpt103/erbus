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
    app.controller('Header_Ctrl', ['$scope', '$uibModal', 'configService', '$state', 'accountService', '$log', 'userService', 'menuService', 'nguoiDungService',
    function ($scope, $uibModal, configService, $state, accountService, $log, userService, menuService, nguoiDungService) {
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
                var unitCodeParam = !$scope.currentUser.parentUnitCode ? $scope.currentUser.unitCode : $scope.currentUser.parentUnitCode;
                menuService.getMenu($scope.currentUser.userName, unitCodeParam).then(function (response) {
                    if (response && response.status === 200 && response.data && response.data.Data && response.data.Data.length > 0) {
                        $scope.treeMenu = treeify(response.data.Data);
                    }
                });
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