require.config({
    base: '/',
    paths: {
        //'jquery': 'lib/jquery.min',
        'jquery': 'lib/jquery.min',
        'jquery-ui': 'lib/jquery-ui.min',
        'angular': 'lib/angular.min',
        'angular-animate': 'lib/angular-animate.min',
        'angular-resource': 'lib/angular-resource.min',
        'angular-filter': 'lib/angular-filter.min',
        'angular-sanitize': 'lib/angular-sanitize.min',
        'angular-cache': 'lib/angular-cache.min',
        'ocLazyLoad': 'lib/ocLazyLoad.require',
        'uiRouter': 'lib/angular-ui-router.min',
        'angularStorage': 'lib/angular-local-storage.min',
        'ui-bootstrap': 'lib/ui-bootstrap-tpls',
        'loading-bar': 'utils/loading-bar/loading-bar.min',
        'smartTable': 'utils/smart-table.min',
        'ngTable': 'utils/ng-table.min',
        'ui.tree': 'lib/angular-ui-tree.min',
        'dynamic-number': 'utils/dynamic-number.min',
        'telerikReportViewer': 'utils/telerik/js/telerikReportViewer-11.0.17.118.min',
        'telerikReportViewer_kendo': 'utils/telerik/js/telerikReportViewer.kendo-11.0.17.118.min',
        'ui-grid': 'utils/ui-grid/ui-grid.min',
        'fileUpload': 'lib/angular-file-upload.min',
        'ng-file-upload': 'utils/ng-file-upload-all.min',
        'ckeditor': 'utils/ckeditor/ckeditor',
        'ng-ckeditor': 'utils/ckeditor/ng-ckeditor',
        'ngMaterial': 'lib/angular-material.min',
        'ngAria': 'lib/angular-aria.min',
        'adapt-strap': 'utils/adapt-strap/adapt-strap',
        'chart-js': 'utils/angular-chart/Chart.min',
        'angular-chart': 'utils/angular-chart/angular-chart.min',
        'ng-tags-input': 'js/ngTagInput/ng-tags-input.min',
        'moment': 'js/moment/moment',
        'jp.ng-bs-animated-button': 'js/ng-bs-animated-button/ng-bs-animated-button',
        'kendo.all.min': 'js/kendo/kendo.all.min'
    },
    shim: {
        'jquery': {
            exports: '$'
        },
        'jquery-ui': ['jquery'],
        'angular': {
            deps: ['jquery'],
            exports: 'angular'
        },
        'ocLazyLoad': ['angular'],
        'uiRouter': ['angular'],
        'angular-animate': ['angular'],
        'angular-resource': ['angular'],
        'angular-filter': ['angular'],
        'angular-cache': ['angular'],
        'angular-sanitize': ['angular'],
        'angularStorage': ['angular'],
        'ui-bootstrap': ['angular'],
        'loading-bar': ['angular'],
        'smartTable': ['angular'],
        'ngTable': ['angular'],
        'ui.tree': ['angular'],
        'dynamic-number': ['angular'],
        'telerikReportViewer': ['jquery', 'angular'],
        'telerikReportViewer_kendo': ['jquery', 'angular'],
        'ui-grid': ['angular'],
        'fileUpload': ['angular'],
        'ng-file-upload': ['angular'],
        'ckeditor': ['angular'],
        'ng-ckeditor': ['angular'],
        'ngMaterial': ['angular'],
        'ngAria': ['angular'],
        'adapt-strap': ['angular'],
        'chart-js': ['angular'],
        'angular-chart': ['chart-js'],
        'ng-tags-input': ['angular'],
        'moment': ['angular'],
        'jp.ng-bs-animated-button': ['angular'],
        'kendo.all.min': {
            deps: ['angular']
        }
    },
    waitSeconds: 0,
    //urlArgs: 'bust=' + new Date().getTime()
});
// Start the main app logic.
require(['app'], function () {
    angular.bootstrap(document.body, ['myApp']);
});
