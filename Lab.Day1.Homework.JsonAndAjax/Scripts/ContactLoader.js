var $ = $ || {};

$.LoadContacts = function (url, callback) {
    var ajax = new XMLHttpRequest();
    ajax.open("GET", url, true);
    ajax.onreadystatechange = function() {
        if (ajax.readyState === 4) {
            if (ajax.status === 200) {
                if (callback != undefined && callback != null)
                    if (callback instanceof Function) callback(ajax.responseText);
            } else {
                alert("Ajax call unsuccessful. Error code: " + ajax.readyState);
            }
        }
    };
    ajax.send(null);
};