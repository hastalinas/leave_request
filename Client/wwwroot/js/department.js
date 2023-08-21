﻿// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
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

$(document).ready(function () {
    $.ajax({
        url: "https://localhost:7293/api/departments"
    }).done(function (result) {
        // Assuming the API response contains a property named "totalEmployees"
        var totalDepartment = result.data.length;
        $("#total-departments").text(totalDepartment);
    }).fail(function () {
        $("#total-departments").text("Failed to fetch data");
    });
});

$.ajax({
    url: "https://localhost:7293/api/employees"
}).done((result) => {
    let selectEmployee = ""
    $.each(result.data, (key, val) => {
        console.log(val)
        selectEmployee += ` <option value="${val.guid}">${val.firstName} ${val.lastName}</option>`
    })
    $('.employeeSelect').html(selectEmployee)
})

$(document).ready(function () {
    $('.datepicker').datepicker({
        format: 'yyyy-mm-dd',
        autoclose: true,
        todayHighlight: true
    });
});