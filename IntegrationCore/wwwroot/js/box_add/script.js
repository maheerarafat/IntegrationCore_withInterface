var type_select = 0;
var type_owner = -1;
var c = function () {
    return ({
        log: function (msg) {
            consoleDiv = document.getElementById('console');
            para = document.createElement('p');
            text = document.createTextNode(msg);
            para.appendChild(text);
            consoleDiv.appendChild(para);
        }
    });
}();

var owl = $("#clients-container");
owl.owlCarousel({
    singleItem: true,
    afterAction: syncPosition,
});

function syncPosition(el) {
    var current = this.currentItem;
    $("#clients-container")
        .find(".owl-item")
        .removeClass("synced")
        .eq(current)
        .addClass("synced")
    if ($("#sync2").data("owlCarousel") !== undefined) {
        center(current)
    }
}

$.fn.editable.defaults.mode = 'inline';
var cr_data = [
    { value: 1, text: 'إضافة' },
    { value: 2, text: 'تحديث بيانات' },
    { value: 3, text: 'ربط بيانات موظف قديم' },
];


var type_data = [
    { value: 0, text: 'ملاك القيادة العامة للشرطة' },
    { value: 3, text: 'الملاك الاتحادى' },
    { value: 29, text: 'ملاك محلي حكومة الشارقة' },
    { value: 33, text: 'تحالف الإمارات للحلول التقنية' },
    { value: 26, text: 'ملاك شركة الرواد لأنظمة تقنية المعلومات' },
    { value: 30, text: 'ملاك محلي حكومة الفجيرة' },
    { value: 23, text: 'ملاك شؤون الجنسية والاقامة والمنافذ / المحلي' },
    { value: 24, text: 'ملاك شؤون الجنسية والاقامة والمنافذ / الخدمة المدنية لإمارة أبوظبي' },
    { value: 32, text: 'ملاك الهوية' },
    { value: 25, text: 'ملاك شركة خدماتي' },
    { value: 31, text: 'ملاك القيادة العامة لشرطة الشارقة' },

];

//$('.callresult').each(function () {

//    $(this).editable({
//        type: 'select',
//        title: 'نوع الإجراء',
//        mode: 'popup',
//        source: cr_data,
//        success: function (response, newValue) {
//            $(this).text(newValue);
//            type_select = newValue;
//        }
//    });
//});


//$('.owner').each(function () {

//    $(this).editable({
//        type: 'select',
//        title: 'ملاك',
//        mode: 'popup',
//        source: type_data,
//        success: function (response, newValue) {
//            $(this).text(newValue);
//            type_owner = newValue;
//        }
//    });
//});

$(".callresult-edit").each(function () {

});


$(".old-ecs").each(function () {
    $(this).editable({
        type: 'text',
        title: 'رقم حساب نظام المراسلات القديم (الرواد)',
        mode: 'popup',
        success: function (res, nv) {
            $(this).text(nv);
        }
    });
});