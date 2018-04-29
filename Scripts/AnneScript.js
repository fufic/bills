var app = angular.module('App1', []);
app.filter('ctime', function () {
    return function (jsonDate) {
        var date = new Date(parseInt(jsonDate.substr(6)));
        return date;
    };

});

app.filter('visib', function() {
    return function(input, isVisib) {
        if (input) {
            var finish = 0; //parse to int
            for (var i = 0; i < input.length;i++) {
                finish++;
              //  alert("h"+input[i].id);
                if (input[i].isPaid) 
                    break;
                
            }
           // alert(finish);
            if (isVisib)
                return input.slice(0, finish);
            else
                return input.slice(finish);
        }
        return [];
    }
});

app.filter('showOne', function () {
    return function (input, showOne) {
        if (input) {
            if ((showOne) && (input.length > 0))
                return input.slice(0, 1);
            else
                return input;
        }
        return [];
    }
});


app.controller('waterBillsCtrl', function ($scope, $http) {
    $scope.showOnePay = true;
    $scope.showOneMeet = true;
    $scope.showWater = false;
    $scope.showElect = false;
    $scope.showRent = false;
    $scope.newElect = {};
  /*  $scope.newMeet = {
        data: new Date()
    }*/
    $http.get('Home/SendData').success(function (response) {
        $scope.bills = response.data;
    });
    $scope.rentPaid = function (id) {
        $http({
            method: 'POST',
            url: "Home/RentBillPaid",
            data: { id: id },
        }).success(function (response) {
            $scope.bills.rent = response.data;
        });
    };
    $scope.electPaid = function (id) {
        $http({
            method: 'POST',
            url: "Home/ElectricBillPaid",
            data: { id: id },
        }).success(function (response) {
            $scope.bills.elect = response.data;
        });
    };
    $scope.waterPaid = function (id) {
        $http({
            method: 'POST',
            url: "Home/WaterBillPaid",
            data: { id: id },

        }).success(function (response) {
            $scope.bills.water = response.data;
        });
    };

    $scope.newMeetCreate = function () {
        $http({
            method: 'POST',
            url: "Home/NewMeet",
            data: $scope.newMeet
        }).success(function (response) {
            $scope.bills.meet = response.data;
        });
    };
    $scope.rBillCreate = function () {
        $http({
            method: 'POST',
            url: "Home/NewRBill",
            data: $scope.newRBill
        }).success(function (response) {
            $scope.bills.rent = response.data;
        });
    };
    $scope.wBillCreate = function () {
        $http({
            method: 'POST',
            url: "Home/NewWBill",
            data: $scope.newWBill
        }).success(function (response) {
            $scope.bills.water = response.data;
        });
    };
    $scope.eBillCreate = function () {
        $http({
            method: 'POST',
            url: "Home/NewEBill",
            data: $scope.newEBill
        }).success(function (response) {
            $scope.bills.elect = response.data;
        });
    };
    $scope.paymentCreate = function () {
        $http({
            method: 'POST',
            url: "Home/NewPay",
            data: $scope.newPay
        }).success(function (response) {
            $scope.bills.pay = response.data;
        });
    };
    $scope.calcResSumm = function () {
           var summ = 0.0;
           for (var i = 0; i < $scope.bills.water.length; i++) {
               if ($scope.bills.water[i].isPaid)
                   break;
               summ += $scope.bills.water[i].summRub;   
           }
           for (var i = 0; i < $scope.bills.elect.length; i++) {
               if ($scope.bills.elect[i].isPaid)
                   break;
               summ += $scope.bills.elect[i].summRub;
           }
        if (($scope.bills.pay.length > 0) && ($scope.bills.pay[0].debt != 0))
            summ-=$scope.bills.pay[0].debt;
           return summ;
    };
});