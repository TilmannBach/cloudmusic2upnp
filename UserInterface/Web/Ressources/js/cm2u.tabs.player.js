cm2u.tabs.player = (new function()
{
    var module = {};
    
    function initializePlayer() {
        var html = $('<a href="http://tumblr.com">'
            +'<div style="text-align: center;">'
            +'    <img style="width: 70%; height: 70%;" src="http://25.media.tumblr.com/tumblr_mbr6937auf1qgt4z0_1349999464_cover.jpg">'
            +'</div>'
        +'</a>'
        +'<h2>'
         +'   Nico Pusch - Live at M...'
        +'</h2>'
        +'<h5>'
         +'   next: Bloodhound Gang - ...'
    +'</h5>'
            + '<div class="progress"><div class="progress-bar" role="progressbar" style="width: 60%;"></div></div>'
            + '<div style="text-align: center;">'

            + '<div class="btn-toolbar" role="toolbar">'
            + '<div class="btn-group">'
            + '<a href="#" data-role="button" data-inline="true" data-iconpos="notext" data-icon="back">rev</a>'
            + '<a href="#" data-role="button" data-inline="true" data-iconpos="notext" data-icon="delete">stop</a>'
            + '<a href="#" data-role="button" data-inline="true" data-iconpos="notext" data-icon="arrow-r">play</a>'
            + '<a href="#" data-role="button" data-inline="true" data-iconpos="notext" data-icon="forward">ffwd</a>'
            + '</div>'
            + '<div class="btn-group">'
            + '<a href="#" data-role="button" data-inline="true" data-iconpos="notext" data-icon="grid">rand</a>'
            + '<a href="#" data-role="button" data-inline="true" data-iconpos="notext" data-icon="refresh">repeat</a>'
            + '<a href="#" data-role="button" data-inline="true" data-iconpos="notext" data-icon="minus">mute</a>'
            + '</div>'

            + '</div>'
            
            );

        $("#tab-player").empty().append(html).trigger("create");
    }


    $(document).ready(function () {
        initializePlayer();
    });

    return module;
}());
