async function getOrders() {
    // отправляет запрос и получаем ответ
    const response = await fetch("/api/Orders/all", {
        method: "GET",
        headers: { "Accept": "application/json" }
    });
    // если запрос прошел нормально
    if (response.ok === true) {
        // получаем данные
        const orders = await response.json();
        const rows = document.querySelector(".tbody_all_orders");
        // добавляем полученные элементы в таблицу
        orders.forEach((order, idx) => rows.append(row(order, idx+1)));
    }
}

async function getFilteredOrders(district, firstDeliveryTime, recordsQuantity) {

    const errorMark = document.querySelector(".error_mark");
    if (errorMark) errorMark.remove();

    // отправляет запрос и получаем ответ
    const response = await fetch("api/Orders/filter", {
        method: "POST",
        headers: { "Accept": "application/json", "Content-Type": "application/json" },
        body: JSON.stringify({
            _cityDistrict: district,
            _firstDeliveryDateTime: firstDeliveryTime,
            _recordsQuantity: recordsQuantity
        })
    });

   /* document.getElementById('.tbody_filtered_orders').innerHTML = '';*/
    if (response.ok === true) {
        const orders = await response.json();
        const rows = document.querySelector(".tbody_filtered_orders");
        // добавляем полученные элементы в таблицу
        orders.forEach((order, idx) => rows.append(row(order, idx + 1)));
    }
    else {
        const error = await response.json();
        const containerTable2 = document.querySelector(".container-table-2");
        const pError = document.createElement("p");
        pError.className = "center-aligh error_mark";
        let errMsg = "";
        if ("errors" in error) {
            let a = error["errors"]["_cityDistrict"];
            let b = error["errors"]["_firstDeliveryDateTime"];
            errMsg = a === undefined ? b : a;
        }
        else {
            errMsg = error.message
        }
        pError.textContent = errMsg;
        containerTable2.appendChild(pError);
    }

}

function clearFilteredData() {
    // document.getElementById('.tbody_filtered_orders').innerHTML = ''
    var tableHeaderRowCount = 1;
    var table = document.getElementById('filteredTable');
    var rowCount = table.rows.length;
    for (var i = tableHeaderRowCount; i < rowCount; i++) {
        table.deleteRow(tableHeaderRowCount);
    }
    console.log("Hello");
}

function row(order, index) {

    const tr = document.createElement("tr");
    tr.setAttribute("data-rowcounter", index);

    const iTd = document.createElement("td");
    iTd.append(index);
    tr.append(iTd);

    const weightTd = document.createElement("td");
    weightTd.append(order.weight);
    tr.append(weightTd);

    const districtTd = document.createElement("td");
    districtTd.append(order.district);
    tr.append(districtTd);

    const deliveryTimeTd = document.createElement("td");
    deliveryTimeTd.append(order.deliveryTime);
    tr.append(deliveryTimeTd);

    return tr;
}

document.getElementById("clearOrdersBtn").addEventListener("click", () => clearFilteredData());

// отправка формы
document.getElementById("applyBtn").addEventListener("click", async () => {

    const districtVal = document.getElementById("district").value;
    const deliveryTimeVal = document.getElementById("deliveryTime").value;
    const recordsQuantity = document.getElementById("recordsQuantity").value;
    await getFilteredOrders(districtVal, deliveryTimeVal, recordsQuantity);

});

getOrders();