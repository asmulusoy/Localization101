// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
var cultureInfo = $.cookie(".AspNetCore.Culture");

if (cultureInfo && cultureInfo.indexOf("uic=") > 0) {
    var currentCulture = cultureInfo.substring(cultureInfo.indexOf("uic=") + 4).split("-")[0];
    DataTableLocalize(currentCulture);
} else {
    var language = window.navigator.userLanguage || window.navigator.language;
    DataTableLocalize(language);
}


function DataTableLocalize(currentCulture) {

    var dataTableLanguageUrl;
    switch (currentCulture) {
        case "tr": dataTableLanguageUrl = "//cdn.datatables.net/plug-ins/1.10.21/i18n/Turkish.json"; break;
        case "fr": dataTableLanguageUrl = "//cdn.datatables.net/plug-ins/1.10.21/i18n/French.json"; break;
        default: "";
    }

    if (dataTableLanguageUrl) {
        $.extend(true, $.fn.dataTable.defaults, {
            "language": {
                "url": dataTableLanguageUrl
            }
        });
    }
   

}

