// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

$(document).ready(function () {
    $('#departmentTable').DataTable({
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


$.ajax({
    url: "https://localhost:7293/api/departments"
}).done(function (result) {
    let getDepartment = ""
    $.each(result.data, (key, val) => {
        console.log(result)
        getDepartment += ` <option value="${val.guid}">${val.name} (${val.code})</option>`
    })
    $('.selectDepartment').html(getDepartment)
}).fail(function () {
    $(".selectDepartment").text("Failed to fetch data");
});

/*$(document).ready(function () {
    $('.datepicker').datepicker({
        format: 'yyyy-mm-dd',
        autoclose: true,
        todayHighlight: true
    });
});*/