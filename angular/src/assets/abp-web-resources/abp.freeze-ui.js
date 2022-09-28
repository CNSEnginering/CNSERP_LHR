var abp = abp || {};
(function () {

    abp.ui.setBusy = function (element, text, freezeDelay) {
        FreezeUI({ element: element, text: text ? text : ' ', freezeDelay: freezeDelay });
    };

    abp.ui.clearBusy = function (element, freezeDelay) {
        UnFreezeUI({ element: element,freezeDelay: freezeDelay });
    };

})();
