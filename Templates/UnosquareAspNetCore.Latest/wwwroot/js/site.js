(function (angular) {
    'use strict';

    angular.module('app.routes', ['ngRoute'])
        .config([
            '$routeProvider', '$locationProvider', function ($routeProvider, $locationProvider) {
                $routeProvider.
                    when('/', {
                        templateUrl: '/views/grid.html',
                        title: 'Grid'
                    }).when('/Login',
                    {
                        templateUrl: '/views/login.html',
                        title: 'Login'
                    }).otherwise({
                        redirectTo: '/'
                    });

                $locationProvider.html5Mode(true);
            }
        ]);

    angular.module('app.controllers', ['tubular.services'])
        .controller('titleController', [
            '$scope', '$route', 'tubularHttp', '$location', function ($scope, $route, tubularHttp, $location) {
                var me = this;
                me.content = "Home";

                $scope.$on('$routeChangeSuccess', function () {
                    me.content = $route.current.title;
                    me.isAnonymousView = me.content === 'Login';

                    if (!me.isAnonymousView && !tubularHttp.isAuthenticated()) {
                        $location.path("/Login");
                    }
                });
            }
        ])
        .controller('gridCtrl', [
            '$scope', '$location', 'toastr', 'tubularHttp', function ($scope, $location, toastr, tubularHttp) {
                var me = this;

                // Grid Events
                $scope.$on('tbGrid_OnBeforeRequest', function (event, eventData) {
                    console.log(eventData);
                });

                $scope.$on('tbGrid_OnRemove', function () {
                    toastr.success("Record removed");
                });

                $scope.$on('tbGrid_OnConnectionError', function (error) {
                    toastr.error(error.statusText || "Connection error");
                });

                $scope.$on('tbGrid_OnSuccessfulSave', function (event, data, gridScope) {
                    toastr.success("Record updated");
                });

                // Form Events
                $scope.$on('tbForm_OnConnectionError', function (error) { toastr.error(error.statusText || "Connection error"); });

                $scope.$on('tbForm_OnSuccessfulSave', function (event, data, formScope) {
                    toastr.success("Record updated");
                    if (formScope) formScope.clear();
                });

                $scope.$on('tbForm_OnSavingNoChanges', function (event) {
                    toastr.warning("Nothing to save");
                    $location.path('/');
                });

                $scope.$on('tbForm_OnCancel', function () {
                    $location.path('/');
                });
            }
        ]).controller('loginCtrl', ['$scope', '$location', 'tubularHttp', 'toastr',
            function ($scope, $location, tubularHttp, toastr) {
                $scope.loading = false;

                $scope.submitForm = function () {
                    if (!$scope.username ||
                        !$scope.password ||
                        $scope.username.trim() === '' ||
                        $scope.password.trim() === '') {
                        toastr.error("", "You need to fill in a username and password");
                        return;
                    }

                    $scope.loading = true;

                    tubularHttp
                        .authenticate($scope.username, $scope.password)
                        .then(() => {
                            $location.path("/");
                        },
                        (error) => {
                            $scope.loading = false;
                            toastr.error(error);
                        });
                };
            }]);

    angular.module('app', [
        'ngAnimate',
        'tubular',
        'toastr',
        'app.routes',
        'app.controllers'
    ]);
})(angular);