// taken from https://raw.githubusercontent.com/alexradulescu/FreezeUIand
// and modified
(function () {

    var freezeHtml = document.createElement('div');
    freezeHtml.classList.add('freeze-ui');

    var freezedItems = [];

    function getSelector(selector) {
        return selector ? selector : 'body';
    }

    function shouldFreezeItem(selector) {
        var itemSelector = getSelector(selector);
        return freezedItems.indexOf(itemSelector) >= 0;
    }

    function addFreezedItem(selector) {
        var itemSelector = getSelector(selector);
        freezedItems.push(itemSelector);
    }

    function removeFreezedItem(selector) {
        var itemSelector = getSelector(selector);
        for (var i = 0; i < freezedItems.length; i++) {
            if (freezedItems[i] === itemSelector) {
                freezedItems.splice(i, 1);
            }
        }
    }

    function normalizeFreezeDelay(delay) {
        return delay ? delay : 1000;
    }

    window.FreezeUI = function (options) {
        if (!options) {
            options = {};
        }

        addFreezedItem(options.selector);
        var freezeDelay = normalizeFreezeDelay(options.freezeDelay);

        setTimeout(function () {
            if (!shouldFreezeItem(options.selector)) {
                return;
            }

            var parent;

            if (options.element) {
                parent = options.element;
            } else {
                parent = document.querySelector(options.selector) || document.body;
            }

            freezeHtml.setAttribute('data-text', options.text || 'Loading');

            if (document.querySelector(options.selector) || options.element) {
                freezeHtml.style.position = 'absolute';
            }

            parent.appendChild(freezeHtml);
        }, freezeDelay);
    };

    window.UnFreezeUI = function (options) {
        if (!options) {
            options = {};
        }

        removeFreezedItem(options.selector);
        var freezeDelay = normalizeFreezeDelay(options.freezeDelay) + 250;
        
        setTimeout(function () {

            var freezeHtml;
            if (options.element) {
                freezeHtml = options.element.querySelector('.freeze-ui');
            } else {
                freezeHtml = document.querySelector('.freeze-ui');
            }

            if (freezeHtml) {
                freezeHtml.classList.remove('is-unfreezing');
                if (freezeHtml.parentElement) {
                    freezeHtml.parentElement.removeChild(freezeHtml);
                }
            }
        }, freezeDelay);
    };
})();
