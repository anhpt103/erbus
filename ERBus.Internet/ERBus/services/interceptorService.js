define(['angular', 'controllers/authorize/authController'], function () {
    var app = angular.module('InterceptorModule', ['authModule']);
    app.factory('interceptorService', ['$q', '$injector', '$location', '$log', 'userService', '$state', function ($q, $injector, $location, $log, userService, $state) {
        var interceptorServiceFactory = {};
        var _request = function (request) {
            request.headers = request.headers || {};
            var currentUser = userService.GetCurrentUser();
            if (currentUser != null) {
                request.headers.Authorization = 'Bearer ' + currentUser.access_token;
            }
            return request;
        };
        var _response = function (res) {
            if (res.data && res.data.Data && res.data.Status) {
                var object;
                try {
                    object = JSON.parse(JSON.stringify(res.data.Data));
                } catch (e) {
                    object = res.data.Data;
                }
            }
            return res;
        };
        var _requestError = function (request) {
            return request
        };
        var _responseError = function (rejection) {
            if (rejection.status === 401) {
                $state.go('login');
            } else {
            }
            return $q.reject(rejection);
        };
        interceptorServiceFactory.request = _request;
        interceptorServiceFactory.response = _response;
        interceptorServiceFactory.requestError = _requestError;
        interceptorServiceFactory.responseError = _responseError;
        return interceptorServiceFactory;
    }]);
    return app;
});