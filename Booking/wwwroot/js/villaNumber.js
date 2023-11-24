var dataTable;



$(document).ready(function () {
	loadDataTable();
})


function loadDataTable() {
	dataTable = $('#villaNumberTable').DataTable({
		"ajax": { url: 'villanumber/getall' },
		"columns": [
			{ "data": 'villa_number', "width": '10%' },
			{ "data": 'specialDetails', "width": '10%' },
			{
				"data": 'id',
				"render": function (data) {
					return `<div class="w-75 btn-group" role="group">
                     <a href="/villa/upsert?id=${data}" class="btn btn-primary mx-2"> <i class="bi bi-pencil-square"></i> Edit</a>               
                     <a onClick=Delete('/villa/delete/${data}') class="btn btn-danger mx-2"> <i class="bi bi-trash-fill"></i> Delete</a>
                    </div>`
				},
				"width": "25%"
			}


		]

	});
	
}