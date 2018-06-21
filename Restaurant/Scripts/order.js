function chooseTable(id) {
    var oldButton = document.getElementById(sessionStorage["btnId"]);
    var newButton = document.getElementById(id);
    newButton.className = "btn btn-success";

    if (sessionStorage["btnId"] != null) {
        oldButton.className = "btn btn-default";
    }

    if (oldButton == newButton) {
        sessionStorage.removeItem("btnId")
    }
    else {
        sessionStorage["btnId"] = id;
    }
    
}

function calculateChange() {
    var input1 = document.getElementById('order-summary').innerHTML;
    var input2 = document.getElementById('order-money').value;
    var output = document.getElementById('order-change');

    if (+input2 > +input1) {
        output.innerHTML = 'Сдача: ' + (+input2 - +input1) + ' ₽';
        output.className = "text-success";
    }
    else {
        output.innerHTML = 'Клиенту не хватает ' + (+input1 - +input2) + ' ₽';
        output.className = "text-danger";
    }
}

function addDish(id, orderId) {
    
    $.getJSON('Order/GetOrderInfo?orderId=' + orderId + '&dishId=' + id, viewDish)
}

function viewDish(data) {
    var dishPad = document.getElementById("dish-pad");
    var p = "<p class='dish-position'>" + data.DishName + ' -- ' + data.DishCost + " ₽</p>";
    var cost = document.getElementById('dish-pad-cost');
    cost.innerHTML = 'Итого: ' + data.Summary + ' ₽';

    cost.insertAdjacentHTML('beforebegin', p);

}


function getOrderInfo(orderId) {
    $.ajax({
        url: 'GetOrderInfo?orderId=' + orderId,
        type: 'GET',
        data: JSON.stringify(),
        contentType: "application/json;charset=utf-8"
    });
}

function addOrder() {
    var clientName = document.getElementById("clientName").value;
    var tableError = false;
    var nameError = false;

    document.getElementById("sucessAlert").innerHTML = ""

    if (sessionStorage["btnId"] == null) {
        document.getElementById("tableAlert").innerHTML = "Выберите столик";
        tableError = true;
    }
    else {
        document.getElementById("tableAlert").innerHTML = "";
        tableError = false;
    }

    if (clientName == "") {
        document.getElementById("nameAlert").innerHTML = "Введите имя клиента";
        nameError = true;
    }
    else {
        document.getElementById("nameAlert").innerHTML = "";
        nameError = false;
    }

    if (!nameError && !tableError) {

        document.getElementById("sucessAlert").innerHTML = "Клиент успешно зарегестрирован"

        $.ajax({
            url: 'Create?name=' + clientName + '&table=' + sessionStorage["btnId"],
            type: 'POST',
            data: JSON.stringify(),
            contentType: "application/json;charset=utf-8"
        });

        var btn = document.getElementById(sessionStorage["btnId"]);
        btn.className = "btn btn";
        btn.disabled = 'disabled';
        document.getElementById("clientName").value = "";

        sessionStorage.removeItem("btnId")

    }
        
}

function pickDate(td) {
    var reserveDate = new Date(document.getElementById("reserveDate").value);
    var a = td.substring(0, 10);
    var day = a.substring(0, a.indexOf('.'));
    a = a.substring(a.indexOf('.') + 1);
    var month = a.substring(0, a.indexOf('.'));
    a = a.substring(a.indexOf('.') + 1);
    var year = a.substring(a.indexOf('.') + 1, 4);
    var today = new Date(year + '-' + month + '-' + day);

    document.getElementById("dateSuccess").innerHTML = "";


    if (isNaN(reserveDate) ) {
        document.getElementById("dateAlert").innerHTML = "Введите дату резервации";
        dateError = true;
    }
    else {
        if (!reserveDate.getDate() > 0 || !reserveDate.getMonth() > 0 || !reserveDate.getYear() > 101) {
            document.getElementById("dateAlert").innerHTML = "Дата введена некорректно";
        }
        else {
            if (today > reserveDate) {
                document.getElementById("dateAlert").innerHTML = "Введенная дата уже прошла";
            }
            else {

                var url = 'Reserve?date=' + reserveDate.toJSON();

                window.open(url,"_self");

            }


        }

        dateError = false;
    }
}


function reserveOrder(reserveDate) {
    var clientName = document.getElementById("clientName").value;
    var tableError = false;
    var nameError = false;

    document.getElementById("sucessAlert").innerHTML = ""

    if (sessionStorage["btnId"] == null) {
        document.getElementById("tableAlert").innerHTML = "Выберите столик";
        tableError = true;
    }
    else {
        document.getElementById("tableAlert").innerHTML = "";
        tableError = false;
    }

    if (clientName == "") {
        document.getElementById("nameAlert").innerHTML = "Введите имя клиента";
        nameError = true;
    }
    else {
        document.getElementById("nameAlert").innerHTML = "";
        nameError = false;
    }
          
    if (!nameError && !tableError) {

        document.getElementById("sucessAlert").innerHTML = "Столик успешно зарезервирован"
        
        var a = reserveDate.substring(0, 10);
        
        var day = a.substring(0, a.indexOf('.'));
        a = a.substring(a.indexOf('.') + 1);
        var month = a.substring(0, a.indexOf('.'));
        a = a.substring(a.indexOf('.') + 1);
        var year = a.substring(0, 4);

        var date = new Date( year + '-' + month + '-' + day);

        

        $.ajax({
            url: 'Reserve?table=' + sessionStorage["btnId"] + '&date=' + date.toJSON() + '&clientName=' + clientName,
            type: 'POST',
            data: JSON.stringify(),
            contentType: "application/json;charset=utf-8"
        });

        var btn = document.getElementById(sessionStorage["btnId"]);
        btn.className = "btn btn";
        btn.disabled = 'disabled';
        document.getElementById("clientName").value = "";

        sessionStorage.removeItem("btnId")

    }

}


$(document).ready(sessionStorage.removeItem("btnId"))
