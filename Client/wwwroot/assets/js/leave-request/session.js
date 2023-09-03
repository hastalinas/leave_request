$(document).ready(function () {
    var path = window.location.pathname.split("/");
    var hris_path = path[1];
    $.ajax({
        url: '/'+ hris_path +'/getName',
        success: function (data) {
            var a = data.username;
            var b = data.nikUser;
            console.log(a);
            console.log(b);
            $('#username').text(a);
            $('#idUser').val(b);
        }
    })
});