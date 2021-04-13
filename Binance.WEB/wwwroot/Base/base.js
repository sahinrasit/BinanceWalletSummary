"use strict";
var _baseUrl = window.location.origin;

var datatablesLangUrl = "//cdn.datatables.net/plug-ins/1.10.22/i18n/Turkish.json";

var HttpCodes = {
    success: 200,
    notFound: 404
}

function getBaseUrl() {
    return _baseUrl;
};
function setCookie(cname, cvalue, exdays) {
    var d = new Date();
    d.setTime(d.getTime() + (exdays * 24 * 60 * 60 * 1000));
    var expires = "expires=" + d.toUTCString();
    document.cookie = cname + "=" + cvalue + ";" + expires + ";path=/";
}
function getCookie(cname) {
    var name = cname + "=";
    var ca = document.cookie.split(';');
    for (var i = 0; i < ca.length; i++) {
        var c = ca[i];
        while (c.charAt(0) == ' ') {
            c = c.substring(1);
        }
        if (c.indexOf(name) == 0) {
            return c.substring(name.length, c.length);
        }
    }
    return "";
}
function checkCookie(cname) {
    var cvalue = getCookie(cname);
    if (cvalue != "") {
        return cvalue;
    } else {
        if (window.location.pathname != "/Account/Login") {
            window.location.href = "../Account/Login";
        }
    }
}

$(document).ready(function () {
    if (window.location.hostname != "localhost") {
        _baseUrl = "https://binanceapi.abusogullari.com/";
    }
    else {
        _baseUrl = "https://localhost:44335/";
    }  
});
