var dataTable;
$(document).ready(function () {
    loadDataTable()
})

function loadDataTable() {
    dataTable = $('#tblData').dataTable({
        "ajax": {
            "url": "/Admin/coverType/GetAll"
        },
        "columns": [
            { "data": "name", "width": "60%" },
            {
                "data": "id",
                "render": function (data) {
                    return `
                        <div class="text-center">
                        <a href="/Admin/coverType/upsert/${data}" class="btn btn-info">
                            <i class="fas fa-edit"></i>
                        </a>
                        <a class="btn btn-danger" onclick=Delete("/Admin/coverType/Delete/${data}")>
                        <i class="fas fa-trash-alt"></i>
                        </a >
                    </div>
`;
                }
            }
        ]
    })
}
function Delete(url) {
    //  alert(url);
    swal({
        title: "Want To Delete Data?",
        text: "Delete information!!",
        icon: "warning",
        buttons: true,
        dangermodel: true
    }).then((willDelete) => {
        if (willDelete) {
            $.ajax({
                url: url,
                type: "DELETE",
                success: function (data) {
                    if (data.success) {
                        toastr.success(data.message);
                        dataTable.ajax.reload();
                    }
                    else {
                        toastr.error(data.message);
                    }
                }
            })
        }
    })
}