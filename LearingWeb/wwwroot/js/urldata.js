/*
 * Learing Web - urldata.js
 * Parses URL query parameters and populates the watch page elements.
 */
(function () {
    'use strict';

    var params = new URLSearchParams(location.search);
    var vLink = params.get('vlink') || params.get('v');
    var vName = params.get('vname') || params.get('h');
    var vDes  = params.get('vdes') || params.get('p');

    var videoFrame = document.getElementById('video');
    var headNameEl = document.getElementById('headname');
    var descEl = document.getElementById('p');

    if (vLink && videoFrame) {
        videoFrame.src = vLink;
    }

    if (vName && headNameEl) {
        headNameEl.textContent = vName;
    }

    if (vDes && descEl) {
        descEl.textContent = vDes;
    }

    if (!vLink) {
        if (headNameEl) headNameEl.textContent = 'No video selected';
        if (descEl) descEl.textContent = 'Please select a course from the home page.';
    }
})();
