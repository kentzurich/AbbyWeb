var dataTable;

$(document).ready(function () {
    var url = window.location.search;

    if (url.includes("inprocess"))
        loadDataTable("inprocess");
    else if (url.includes("completed"))
        loadDataTable("completed");
    else if (url.includes("paymentpending"))
        loadDataTable("paymentpending");
    else if (url.includes("approved"))
        loadDataTable("approved");
    else
        loadDataTable("all");
});

function loadDataTable(status) {
    dataTable = $('#tblOrder').DataTable({
        "ajax": {
            "url":"/Admin/Order/GetAll?status=" + status
        },
        "columns": [
            {"data": "id", "width": "5%"},
            {"data": "name", "width": "20%"},
            {"data": "phoneNumber", "width": "15%"},
            {"data": "applicationUser.email", "width": "15%"},
            {"data": "orderStatus", "width": "15%"},
            {"data": "orderTotal", "width": "15%"},
            {
                "data": "id",
                "render": function (data) {
                    return `
                        <div class="btn-group" role="group">
                            <a href="/Admin/Order/Details?orderId=${data}" class="btn btn-primary">
                                <i class="bi bi-search"></i> &nbsp;Details
                            </a>
                        </div>
                    `
                },
                "width": "15%"
            }
        ]
    });
}