// urldata.js — Extracts video data from URL query parameters and populates the watch page.
// Expected URL format: watch.aspx?v=VIDEO_ID&vname=TITLE&vlink=URL&vdes=DESCRIPTION

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

    // If no video loaded, show a friendly message
    if (!vLink) {
        if (headNameEl) headNameEl.textContent = 'No video selected';
        if (descEl) descEl.textContent = 'Please select a course from the home page.';
    }
})();
