/**
 * loads sub modules and wraps them up into the main module
 * this should be used for top-level module definitions only
 */
define([
	'jquery',
    'jquery-ui',
    'angular',
    'states/catalog',
    'states/authorize',
    'states/knowledge',
    'states/promotion',
    'states/report',
    'config/config',
	'ocLazyLoad',
	'uiRouter',
	'angularStorage',
    'angular-animate',
    'angular-resource',
    'angular-filter',
    'angular-sanitize',
    'angular-cache',
	'ui-bootstrap',
    'loading-bar',
    'smartTable',
    'ngTable',
    'ui.tree',
    'dynamic-number',
	'services/interceptorService',
    'services/configService',
    'services/tempDataService',
    'filters/common',
    'kendo.all.min',
    'telerikReportViewer',
    'ui-grid',
    'fileUpload',
    'ng-file-upload',
    'ng-ckeditor',
    'ngMaterial',
    'ngAria',
    'ng-tags-input',
    'moment',
    'jp.ng-bs-animated-button'
], function (jquery, jqueryui, angular, stateCatalog, stateAuthorize, stateKnowledge, statePromotion, stateReport) {
    'use strict';
    var app = angular.module('myApp', ['oc.lazyLoad', 'ui.router', 'InterceptorModule', 'LocalStorageModule', 'ui.bootstrap', 'configModule', 'tempDataModule', 'angular-loading-bar', 'ngAnimate', 'ngSanitize', 'common-filter', 'ngResource', 'smart-table', 'angular.filter', 'ngTable', 'angular-cache', 'ui.tree', 'dynamicNumber', 'ui.grid', 'angularFileUpload', 'ngFileUpload', 'ngCkeditor', 'ngMaterial', 'ngAria', 'ngTagsInput', 'jp.ng-bs-animated-button', 'kendo.directives']);
    var urlInternet = '';
    app.run(['ngTableDefaults', 'configService', function (ngTableDefaults, configService) {
        ngTableDefaults.params.count = 5;
        ngTableDefaults.settings.counts = [];
        urlInternet = configService.rootUrlWeb;
    }]);


    app.directive('enter', function () {
        return function (scope, element, attrs) {
            element.bind("keydown keypress", function (event) {
                if (event.which === 13) {
                    event.preventDefault();
                    var fields = $(this).parents('form:eq(0),body').find('input, textarea, select, button');
                    var index = fields.index(this);
                    if (index > -1 && (index + 1) < fields.length)
                        fields.eq(index + 1).focus();
                    fields.eq(index + 1).select();
                }
            });
        };
    });
    
    app.directive('dateCheck', [function () {
        return {
            require: 'ngModel',
            link: function (scope, elem, attrs, ctrl) {
                var firstDateElement = '#' + attrs.dateCheck;
                elem.add(firstDateElement).on('change', function () {
                    scope.$apply(function () {
                        var firstDate = $(firstDateElement).val().replace('-', '/');
                        var curDate = elem.val().replace('-', '/');
                        if (curDate >= firstDate) {
                            ctrl.$setValidity('dateok', true);
                        } else {
                            ctrl.$setValidity('dateok', false);
                        }
                    });
                });

            }
        }
    }]);

    app.directive('keyboard', function ($document, keyCodes) {
        return {
            link: function (scope, element, attrs) {

                var keysToHandle = scope.$eval(attrs.keyboard);
                var keyHandlers = {};
                // Registers key handlers
                angular.forEach(keysToHandle, function (callback, keyName) {
                    var keyCode = keyCodes[keyName];
                    keyHandlers[keyCode] = { callback: callback, name: keyName };
                });
                // Bind to document keydown event
                $document.on("keydown", function (event) {
                    var keyDown = keyHandlers[event.keyCode];
                    // Handler is registered
                    if (keyDown) {
                        event.preventDefault();
                        // Invoke the handler and digest
                        scope.$apply(function () {
                            keyDown.callback(keyDown.name, event.keyCode);
                        });
                    }
                });
            }
        };
    });
    app.directive('report', ['configService', function (configService) {
        return {
            restrict: 'EA',
            transclude: 'true',
            scope: {
                name: '@',
                params: '@'
            },
            template: "",
            link: function (scope, element, attrs) {
                //create the viewer object first, can be done in index.html as well
                var reportViewer = $("#reportViewer1").data("telerik_ReportViewer");
                if (!reportViewer) {
                    $("#reportViewer1").toggle();
                    var objpara = JSON.parse(scope.params);
                    $(document).ready(function () {
                        $("#reportViewer1").telerik_ReportViewer({
                            error: function (e, args) {
                                alert('Error from report directive:' + args);
                            },
                            reportSource: {
                                report: scope.name,
                                parameters: objpara,
                            },
                            serviceUrl: configService.rootUrlWebApi + "/reports",
                            scaleMode: 'SPECIFIC',
                            scale: 1.0,
                            viewMode: 'PRINT_PREVIEW',
                            ready: function () {
                            },
                            renderingBegin: function () {
                            },
                            renderingEnd: function () {
                            }
                        });
                    });
                }
                //on state change update the report source
                scope.$watch('name', function () {
                    var reportViewer = $("#reportViewer1").data("telerik_ReportViewer");
                    if (reportViewer) {
                        var rs = reportViewer.reportSource();
                        if (rs && rs.report) {
                            if (rs.report != scope.name &&
                                rs.parameters != scope.parameters) {
                                reportViewer.reportSource({
                                    report: scope.name,
                                    parameters: angular.toJson(scope.parameters),
                                });
                            }
                        }
                    }
                });
            }
        }
    }]);
    app.directive('preventDefault', function () {
        return function (scope, element, attrs) {
            angular.element(element).bind('click', function (event) {
                event.preventDefault();
                event.stopPropagation();
            });
        }
    });

    app.service('blockModalService', function () {
        var result = this;
        result.isBlocked = false;
        result.setValue = function (value) {
            if (result.isBlocked !== value) {
                result.isBlocked = value;
            }
        }
        return result;
    });
    app.directive('ngEnter', function () {
        return function (scope, element, attrs) {
            element.bind("keydown keypress", function (event) {
                if (event.which === 13) {
                    scope.$apply(function () {
                        scope.$eval(attrs.ngEnter);
                    });

                    event.preventDefault();
                }
            });
        };
    });

    /* Directive */
    app.directive('excelExport',
        function () {
            return {
                restrict: 'A',
                scope: {
                    fileName: "@",
                    data: "&exportData"
                },
                replace: true,
                template: '<button class="btn btn-primary btn-ef btn-ef-3 btn-ef-3c mb-10" ng-click="download()">Export to Excel <i class="fa fa-download"></i></button>',
                link: function (scope, element) {
                    scope.download = function () {
                        function datenum(v, date1904) {
                            if (date1904) v += 1462;
                            var epoch = Date.parse(v);
                            return (epoch - new Date(Date.UTC(1899, 11, 30))) / (24 * 60 * 60 * 1000);
                        };
                        function getSheet(data, opts) {
                            var ws = {};
                            var range = { s: { c: 10000000, r: 10000000 }, e: { c: 0, r: 0 } };
                            for (var R = 0; R != data.length; ++R) {
                                for (var C = 0; C != data[R].length; ++C) {
                                    if (range.s.r > R) range.s.r = R;
                                    if (range.s.c > C) range.s.c = C;
                                    if (range.e.r < R) range.e.r = R;
                                    if (range.e.c < C) range.e.c = C;
                                    var cell = { v: data[R][C] };
                                    if (cell.v == null) continue;
                                    var cell_ref = XLSX.utils.encode_cell({ c: C, r: R });

                                    if (typeof cell.v === 'number') cell.t = 'n';
                                    else if (typeof cell.v === 'boolean') cell.t = 'b';
                                    else if (cell.v instanceof Date) {
                                        cell.t = 'n'; cell.z = XLSX.SSF._table[14];
                                        cell.v = datenum(cell.v);
                                    }
                                    else cell.t = 's';

                                    ws[cell_ref] = cell;
                                }
                            }
                            if (range.s.c < 10000000) ws['!ref'] = XLSX.utils.encode_range(range);
                            return ws;
                        };
                        function Workbook() {
                            if (!(this instanceof Workbook)) return new Workbook();
                            this.SheetNames = [];
                            this.Sheets = {};
                        }
                        var wb = new Workbook(), ws = getSheet(scope.data());
                        /* add worksheet to workbook */
                        wb.SheetNames.push(scope.fileName);
                        wb.Sheets[scope.fileName] = ws;
                        var wbout = XLSX.write(wb, { bookType: 'xlsx', bookSST: true, type: 'binary' });
                        function s2ab(s) {
                            var buf = new ArrayBuffer(s.length);
                            var view = new Uint8Array(buf);
                            for (var i = 0; i != s.length; ++i) view[i] = s.charCodeAt(i) & 0xFF;
                            return buf;
                        }
                        saveAs(new Blob([s2ab(wbout)], { type: "application/octet-stream" }), scope.fileName + '.xlsx');
                    };
                }
            };
        }
     );

    app.config(['$httpProvider', 'CacheFactoryProvider', 'cfpLoadingBarProvider', 'localStorageServiceProvider', function ($httpProvider, CacheFactoryProvider, cfpLoadingBarProvider, localStorageServiceProvider) {
        $httpProvider.interceptors.push('interceptorService');

        cfpLoadingBarProvider.includeSpinner = true;
        cfpLoadingBarProvider.includeBar = true;

        localStorageServiceProvider.setStorageType('cookie').setPrefix('ss');

        angular.extend(CacheFactoryProvider.defaults, {
            maxAge: 3600000, //1 hour
            deleteOnExpire: 'aggressive',
            storageMode: 'memory',
            onExpire: function (key, value) {
                var _this = this; // "this" is the cache in which the item expired
                if (key.indexOf('/') !== -1) {
                    angular.injector(['ng']).get('$http').get(key).then(function (successRes) {
                        //console.log('successRes', successRes);
                        _this.put(key, successRes.data);
                    }, function (errorRes) {
                        //console.log('errorRes', errorRes);
                    });
                } else {
                    _this.put(key, value);
                    //console.log(key, angular.toJson(value));
                }
            }
        });
    }]);


    app.config(['$urlMatcherFactoryProvider', function ($urlMatcherFactory) {
        $urlMatcherFactory.type('configParams',
      {
          name: 'configParams',
          decode: function (val) { return typeof (val) === "string" ? JSON.parse(val) : val; },
          encode: function (val) { return JSON.stringify(val); },
          equals: function (a, b) { return this.is(a) && this.is(b) && a.ID === b.ID && a.TUNGAY == b.TUNGAY && a.DENNGAY == b.DENNGAY },
          is: function (val) { return angular.isObject(val) && "ID" in val && "TUNGAY" in val && "DENNGAY" in val },
      })
    }]);


    //validate number
    app.config(function (dynamicNumberStrategyProvider) {
        dynamicNumberStrategyProvider.addStrategy('number', {
            numInt: 18,
            numFract: 3,
            numSep: ',',
            numPos: true,
            numNeg: true,
            numRound: 'round',
            numThousand: true,
            numThousandSep: ' '
        });
        dynamicNumberStrategyProvider.addStrategy('number-utc', {
            numInt: 18,
            numFract: 2,
            numSep: '.',
            numPos: true,
            numNeg: true,
            numRound: 'round',
            numThousand: true
        });
        dynamicNumberStrategyProvider.addStrategy('number-int', {
            numInt: 18,
            numFract: 0,
            numSep: ',',
            numPos: true,
            numNeg: true,
            numRound: 'ceil',
            numThousand: true
        });
        dynamicNumberStrategyProvider.addStrategy('number-38', {
            numInt: 21,
            numFract: 2,
            numSep: ',',
            numPos: true,
            numNeg: true,
            numRound: 'round',
            numThousand: true
        });
        dynamicNumberStrategyProvider.addStrategy('number-int-38', {
            numInt: 21,
            numFract: 0,
            numSep: ',',
            numPos: true,
            numNeg: true,
            numRound: 'ceil',
            numThousand: true
        });
    });

    //Auth - check role
    app.service('securityService', ['$http', 'configService', function ($http, configService) {
        var result = {
            getAccessList: function (machucnang) {
                return $http.get(configService.rootUrlWebApi + '/Authorize/Access/GetAccesslist/' + machucnang);
            }
        };
        return result;
    }]);

    app.config(['$stateProvider', '$urlRouterProvider', '$ocLazyLoadProvider', '$locationProvider',
		function ($stateProvider, $urlRouterProvider, $ocLazyLoadProvider, $locationProvider) {
		    $ocLazyLoadProvider.config({
		        jsLoader: requirejs,
		        loadedModules: ['app'],
		        asyncLoader: require
		    });
		    var layoutUrl = urlInternet + "/ERBus/";
		    $urlRouterProvider.otherwise("/home");

		    $stateProvider.state('root', {
		        abstract: true,
		        views: {
		            'viewRoot': {
		                templateUrl: layoutUrl + "views/layouts/layout.html",
		                controller: "layout_Crtl as ctrl"
		            }
		        },
		        resolve: {
		            loadModule: ['$ocLazyLoad', '$q', function ($ocLazyLoad, $q) {
		                var deferred = $q.defer();
		                require([urlInternet + '/ERBus/controllers/layouts/layout-controller.js'],
                            function (layoutModule) {
                                deferred.resolve();
                                $ocLazyLoad.inject(layoutModule.name);
                            });
		                return deferred.promise;
		            }]
		        }
		    });

		    $stateProvider.state('layout', {
		        parent: 'root',
		        abstract: true,
		        views: {
		            'viewHeader': {
		                templateUrl: layoutUrl + "views/layouts/header.html",
		                controller: "Header_Ctrl as ctrl"
		            }
		        },
		        resolve: {
		            loadModule: [
                        '$ocLazyLoad', '$q', function ($ocLazyLoad, $q) {
                            var deferred = $q.defer();
                            require([urlInternet + '/ERBus/controllers/layouts/header-controller.js'],
                                function (headerModule) { //url c?a module
                                    deferred.resolve();
                                    $ocLazyLoad.inject(headerModule.name);
                                });
                            return deferred.promise;
                        }
		            ]

		        }
		    });
		    $stateProvider.state('login', {
		        url: "/login",
		        abstract: false,
		        views: {
		            'viewRoot': {
		                templateUrl: layoutUrl + "views/layouts/login.html",
		                controller: "login_Ctrl as ctrl"
		            }
		        },
		        resolve: {
		            loadModule: ['$ocLazyLoad', '$q', function ($ocLazyLoad, $q) {
		                var deferred = $q.defer();
		                require([urlInternet + '/ERBus/controllers/authorize/authController.js'],
                            function (layoutModule) {
                                deferred.resolve();
                                $ocLazyLoad.inject(layoutModule.name);
                            });
		                return deferred.promise;
		            }]
		        }
		    });
		    $stateProvider.state('home', {
		        url: "/home",
		        parent: 'layout',
		        abstract: false,
		        views: {
		            'viewMain@root': {
		                templateUrl: layoutUrl + "views/layouts/home.html",
		                controller: "home_Ctrl as ctrl"
		            }
		        },
		        resolve: {
		            loadModule: ['$ocLazyLoad', '$q', function ($ocLazyLoad, $q) {
		                var deferred = $q.defer();
		                require([urlInternet + '/ERBus/controllers/layouts/home-controller.js'],
                            function (homeModule) {
                                deferred.resolve();
                                $ocLazyLoad.inject(homeModule.name);
                            });
		                return deferred.promise;
		            }]
		        }
		    });
		    var lststate = [];
		    lststate = lststate.concat(stateCatalog).concat(stateAuthorize).concat(stateKnowledge).concat(statePromotion).concat(stateReport);
		    angular.forEach(lststate, function (state) {
		        $stateProvider.state(state.name, {
		            url: state.url,
		            parent: state.parent,
		            abstract: state.abstract,
		            views: state.views,
		            resolve: {
		                loadModule: ['$ocLazyLoad', '$q', function ($ocLazyLoad, $q) {
		                    var deferred = $q.defer();
		                    require([state.moduleUrl], function (module) {
		                        deferred.resolve();
		                        $ocLazyLoad.inject(module.name);
		                    });
		                    return deferred.promise;
		                }]
		            }
		        });
		    });
		}]);
    return app;
});
