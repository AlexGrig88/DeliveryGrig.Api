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
        const rows = document.querySelector("tbody");
        // добавляем полученные элементы в таблицу
        orders.forEach(order => rows.append(row(order)));
    }
}

function row(order) {

    const tr = document.createElement("tr");
    tr.setAttribute("data-rowid", order.id);

    const idTd = document.createElement("td");
    idTd.append(order.id);
    tr.append(idTd);

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
getOrders();