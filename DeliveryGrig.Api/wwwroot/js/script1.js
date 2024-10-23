async function getOrders() {
    // ���������� ������ � �������� �����
    const response = await fetch("/api/Orders/all", {
        method: "GET",
        headers: { "Accept": "application/json" }
    });
    // ���� ������ ������ ���������
    if (response.ok === true) {
        // �������� ������
        const orders = await response.json();
        const rows = document.querySelector("tbody");
        // ��������� ���������� �������� � �������
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