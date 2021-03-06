﻿(function () {
    'use strict';

    angular.module('teamify.app').service('peopleService', service);

    service.$inject = ['$http'];
    function service($http) {
        this.getPeople = getPeople;
        this.getPeopleFiltered = getPeopleFiltered;

        function getPeople() {
            return $http.get('/api/People/GetPeople');
        }

        function getPeopleFiltered(filter, filterOut) {
            return $http.post(`/api/People/Filter`, { Filter: filter, FilterOut: filterOut });
        }
    }

})(); 