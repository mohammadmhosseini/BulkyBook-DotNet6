var dataTable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    $('#tblData').DataTable({
        "ajax": {
            "url": "/Admin/Company/GetAll"
        },
        "columns": [
            { "data": "name", "width": "15%" },
            { "data": "streetAddress", "width": "15%" },
            { "data": "province", "width": "15%" },
            { "data": "city", "width": "15%" },
            { "data": "postalCode", "width": "15%" },
            { "data": "phoneNumber", "width": "15%" },
            {
                "data": "id",
                "render": function (data) {
                    return `
                        <div class="w-90 btn-group" role="group">
                            <a href="/Admin/Company/Upsert?id=${data}"
                            class="btn btn-outline-primary mx-2"> <i class="bi bi-pencil-square"></i> ویرایش </a>
                            <a onClick=Delete('/Admin/Company/Delete/${data}')
                            class="btn btn-outline-danger mx-2"> <i class="bi bi-trash-fill"></i> حذف </a>
                        </div>
                    `
                },
                "width": "15%"
            },
        ]
    });
}

function Delete(url) {
    Swal.fire({
        title: 'مطمعن هستید?',
        text: "قادر به بازگشت عملیات نخواهید بود!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'بله, پاکش کن!'
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: url,
                type: 'DELETE',
                success: function (data) {
                    if (data.success) {
                        /*dataTable.reload();*/
                        toastr.success(data.message);
                    } else {
                        toastr.error(data.message);
                    }
                }
            })
        }
    })
}