cm2u.tabs.playlist = (new function()
{
    var module = {};
    
    cm2u.event.register("PlaylistNotification", "remote", function (eventName, data) {
        var html = $('<div id="playlist" class="list-group"></ul>');
        for (var i in data.Tracks) {
            var li = $('<a class="list-group-item" playlistid="' + data.Tracks[i].PlayListID + '" trackid="' + data.Tracks[i].Track.ID + '"><span class="badge"><span class="glyphicon glyphicon-minus"></span></span>' + data.Tracks[i].Track.Name + '</a>');
            html.append(li);
            li.click(function () {
                cm2u.socket.protocol.playlistitem_remove_request($(this).attr("playlistid"));
                return false;
            })
        }
        $("#tab-playlist").find("div#playlist").remove();
        $("#tab-playlist").append(html);
    });

    return module;
}());
