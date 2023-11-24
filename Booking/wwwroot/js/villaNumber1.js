var dataTable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#villaNumberTable').DataTable({
        "ajax": { url: '/villaNumber/getall' },
        "columns": [
            { "data": "villa_Number", "width": "15%" },
            { "data": "villa.name", "width": "15%" },
            { "data": "specialDetails", "width": "15%" },
            {
                data: 'villa_Number',
                "render": function (data) {
                    return `<div class="w-75 btn-group" role="group">
                     <a href="/villanumber/edit?villa_Number=${data}" class="btn btn-primary mx-2"> <i class="bi bi-pencil-square"></i> Edit</a>
                     <a onClick=Delete('/villanumber/delete?villa_Number=${data}') class="btn btn-danger mx-2"> <i class="bi bi-trash-fill"></i> Delete</a>
                    </div>`
                },
                "width": "25%"
            }
        ]
    });
}

function Delete(url) {
    Swal.fire({
        title: 'Are you sure?',
        text: "You won't be able to revert this!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes, delete it!'
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: url,
                type: 'DELETE',
                success: function (data) {
                    dataTable.ajax.reload();
                    toastr.success(data.message);
                }
            })
        }
    })
}