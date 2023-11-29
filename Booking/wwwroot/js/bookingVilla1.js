var dataTable;


$(document).ready(function () {
	const urlParams = new URLSearchParams(window.location.search);
	const status = urlParams.get('status')

	loadDataTable(status);
})


function loadDataTable(status) {

	dataTable = $('#bookingTable').DataTable({
		"ajax": { url: '/bookingvilla/getall?status=' + status },
		"columns": [
			{ "data": "id", "width": "5%" },
			{ "data": "email", "width": "10%" },
			{ "data": "name", "width": "5%" },
			{ "data": "villa.name", "width": "10%" },
			{ "data": "nights", "width": "5%" },
			{ "data": "checkInDate", "width": "5%" },
			{ "data": "checkOutDate", "width": "5%" },
			{ "data": "status", "width": "5%" },
			{ data: 'price', render: $.fn.dataTable.render.number(',', '.', 2), "width": "5%" },
			{
				data: 'id',
				"render": function (data) {
					return `<div class="w-75 btn-group" role="group">
                     <a href="/bookingvilla/detail?id=${data}" class="btn btn-outline-warning mx-2"> <i class="bi bi-pencil-square"></i>Detail</a>                
                    </div>`
				},
				"width": "15%"
			}
		]
	})

}



