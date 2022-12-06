// Write your JavaScript code.


function convertString2Date(date_str) {

    var date_formate = new Date(date_str);
    var date_str = date_formate.getDate() + "-" + (date_formate.getMonth() + 1) + "-" + date_formate.getFullYear() + " " +
        date_formate.getHours() + ":" + date_formate.getMinutes()
    return date_str;
}

function getBaseURL() {
    var arr = window.location.href.split("/");
    delete arr[arr.length - 1];
    return arr.join("/");
}

// render dataTable
function render_dataTable(url, id, columns, id_spinner, isFilter, isOrder) {

    isOrder = isOrder || false;
    $('#' + id_spinner).show(0);
    $("#" + id).DataTable({
        "processing": true, // for show progress bar
        "serverSide": true, // for process server side
        "filter": true, // this is for disable filter (search box)
        "orderMulti": false, // for disable multiple column at once
        "bDestroy": true, // destory table when refresh
        "ordering": isOrder, // disable order
        "ajax": {
            "url": url,
            "type": "POST",
            "datatype": "json",
            "beforeSend": function (xhdr) {
                let token = sessionStorage['token'];
                xhdr.setRequestHeader(
                    "Authorization", "Bearer " + token);
            },
            "error": function (xhr, error, code) {
                $('.alert').css('display', 'block');
                $('#' + id_spinner).css('display', 'none');
                if (xhr.responseText != "") {
                    $('#resMessage').text(xhr.responseJSON.value.eMessage);
                } else if (xhr.statusText == "Unauthorized") {
                    $('#resMessage').text(xhr.statusText);
                    CallBackUnauthorized();
                }
                else {
                    $('#resMessage').text(xhr.statusText);
                }
            }
        },
        "initComplete": function (settings, json) {
            $('#table_before_render').css('display', 'none');
            $('#tables_render').css('display', 'block');
            $('#' + id_spinner).css('display', 'none');
        },
        "columnDefs":
            [{
                "targets": [0],
                "visible": false,
                "searchable": false
            }],
        "columns": columns
    });
}

// logout button
$("#logout").click(function () {

    sessionStorage['token'] = '';
    window.location.href = getBaseURL();
});



// Shared function to call when Unauthorized happen
function CallBackUnauthorized() {
    sessionStorage['token'] = '';
    window.location.href = getBaseURL();
}