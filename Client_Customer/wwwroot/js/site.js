// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your Javascript code.


/*$(".form_datetime").datetimepicker({ format: 'yyyy-mm-dd hh:ii' }*/


function openNav() {
    document.getElementById("mySidenav").style.width = "250px";
    document.getElementById("main").style.marginLeft = "250px";
    document.body.style.backgroundColor = "rgba(0,0,0,0.4)";
}

function closeNav() {
    document.getElementById("mySidenav").style.width = "0";
    document.getElementById("main").style.marginLeft = "0";
    document.body.style.backgroundColor = "white";
}


/*login*/
$(document).ready(function () {
    var signUp = $('.signup-but');
    var logIn = $('.login-but');

    signUp.on('click', function () {
        $('.login').fadeOut('slow').css('display', 'none');
        $('.sign-up').fadeIn('slow');

        $('.form-container').animate({ left: '10px' }, 'slow');
    });

    logIn.on('click', function () {
        $('.login').removeClass("hide");
        $('.sign-up').fadeOut('slow').css('display', 'none');
        $('.login').fadeIn('slow');
        

        $('.form-container').animate({ left: '430px' }, 'slow');
    });
});



$(".form_datetime").datetimepicker({
    format: "dd-mm-yyyy hh:ii:ss",
    autoclose: true,
    todayBtn: true,
    pickerPosition: "bottom-left"
});