﻿$(function () {
    window.ReportsUrl = window.ApiUrl + "reports";
    var reports = $.connection.reportsHub;

    window.update = reports.client.ReportDataChange = function () {
        $.ajax({
            dataType: "json",
            url: window.ReportsUrl,
            headers: {
                'Authorization': 'bearer ' + getCookie('token')
            },
            success: updateReports
        });
    };
    
    update();

    $.connection.hub.url = window.ApiUrl + "signalr";

    $.connection.hub.start();
});

function getCookie(cname) {
    var name = cname + "=";
    var ca = document.cookie.split(';');
    for (var i = 0; i < ca.length; i++) {
        var c = ca[i];
        while (c.charAt(0) == ' ') c = c.substring(1);
        if (c.indexOf(name) == 0) return c.substring(name.length, c.length);
    }
    return "";
}

function updateReports(data) {
    console.table(data);
    var source = $('#reportsTemplate').html();
    var template = Handlebars.compile(source);
    var html = template(data);
    $('body').html(html);
}

Handlebars.registerHelper('detailsSummary', function (category) {
    var result = '<span>De categorie is: ' + category + '</span>';

    return new Handlebars.SafeString(result);
});


Handlebars.registerHelper('translate', function (category, key) {
    if (typeof Translations === 'undefined') {
        $.ajax({
            dataType: "json",
            url: '/Resources/dutch',
            async: false,
            success: function (data) {
                window.Translations = data;
           }
        });
    }

    var categoryTranslations = Translations[category];
    if (typeof categoryTranslations === 'undefined') {
        console.warn('Category: "' + category + '" does not exists in the translations');
        return key;
    }

    var translation = categoryTranslations[key];
    if (typeof translation === 'undefined') {
        console.warn('Key: "' + key + '" does not exists in the category "' + category + '" of the translations')
        return key;
    }

    return translation;
});

Handlebars.registerHelper('prettyDate', function (date) {
    var today       = new Date(),
        todayDate   = today.getDate() + "-" + (today.getMonth() + 1) + "-" + today.getFullYear(),
        date        = new Date(date),
        dateDate    = date.getDate() + "-" + (date.getMonth() + 1) + "-" + date.getFullYear(),
        dateTime    = date.getHours() + ':' + date.getMinutes();

    return todayDate == dateDate ? dateTime : dateDate;
});