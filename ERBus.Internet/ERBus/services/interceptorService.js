define(['angular', 'controllers/authorize/authController'], function () {
    var app = angular.module('InterceptorModule', ['authModule']);
    app.factory('interceptorService', ['$q', '$injector', '$location', '$log', 'userService', '$state', '$rootScope', function ($q, $injector, $location, $log, userService, $state, $rootScope) {
        var interceptorServiceFactory = {};
        var _request = function (request) {
            var deferred = $q.defer();

            request.headers = request.headers || {};
            var currentUser = userService.GetCurrentUser();
            if (currentUser != null) {
                request.headers.Authorization = 'Bearer ' + currentUser.access_token;
            }

            deferred.resolve(request);
            return deferred.promise;
        };

        var _response = function (response) {
            if (response.data && response.data.Data && response.data.Status) {
                try {
                    JSON.parse(JSON.stringify(response.data.Data));
                } catch (e) {
                    console.log(e);
                }
            } else if (response.data && response.data.Status == false && response.data.Message) {
                if (response.data.Message.indexOf('database'))
                    Lobibox.notify(type, {
                        size: 'mini',
                        rounded: true,
                        delay: false,
                        position: {
                            left: number, top: number
                        },
                        msg: 'Không thể kết nối tới Cơ sở dữ liệu! Kiểm tra kết nối Internet hoặc liên hệ Quản trị'
                    });
                else console.log(response);
            }
            return response || $q.when(response);;
        };

        var _requestError = function (rejection) {
            return $q.reject(rejection);
        };

        var _responseError = function (rejection) {
            if (rejection.status === 401) {
                $state.go('login');
                $rootScope.$broadcast('error')
            }

            if (rejection.status === 500) {
                $rootScope.ErrorMsg = "An Unexpected error occured";
                $location.path('/Error')
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