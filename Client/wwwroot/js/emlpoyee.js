// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

$(document).ready(function () {
    $('#table').DataTable({
        dom: 'Blfrtip',
        buttons: [
            {
                extend: 'colvis',
                postfixButtons: ['colvisRestore'],
                collectionLayout: 'fixed two-column',
                className: 'btn btn-primary'
            }
            , 'copy', 'csv', 'excel', 'pdf', 'print'
        ]
    });
});

$(document).ready(function () {
    $.ajax({
        url: "https://localhost:7237/api/employees"
    }).done(function (result) {
        // Assuming the API response contains a property named "totalEmployees"
        var totalEmployees = result.data.length;
        $("#total-employees").text(totalEmployees);
    }).fail(function () {
        $("#total-employees").text("Failed to fetch data");
    });
});

$(document).ready(function () {
    $('.datepicker').datepicker({
        format: 'yyyy-mm-dd',
        autoclose: true,
        todayHighlight: true
    });
});