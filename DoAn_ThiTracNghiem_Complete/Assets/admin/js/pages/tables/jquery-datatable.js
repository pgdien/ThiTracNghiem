$(function () {
    $('.js-basic-example').DataTable({
        responsive: true
    });

    //Exportable table
    $('.js-exportable').DataTable({
        dom: 'Bfrtip',
        responsive: true,
        buttons: [
            {
            extend: 'copy',
            text: 'Copy',
			exportOptions: {
            columns: ':not(.notexport)'
        }
        }, 
			{
            extend: 'excel',
            text: 'Excel',
			exportOptions: {
            columns: ':not(.notexport)'
        }
        }
			
        ]
    });
});
