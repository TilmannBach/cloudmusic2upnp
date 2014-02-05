cm2u.tabs.playlist = (new function()
{
    var module = {};
    
    cm2u.event.register("PlaylistNotification", "remote", function (eventName, data) {
        var html = $('<div id="playlist" class="list-group"></ul>');
        for (var i in data.Tracks) {
            var li = $("<a class=\"list-group-item\" trackid=\"" + data.Tracks[i].ID + "\"><span class=\"badge\"><span class=\"glyphicon glyphicon-minus\"></span></span>" + data.Tracks[i].Name + "</a>");
            html.append(li);
            li.click(function(){
                cm2u.socket.protocol.play_request($(this).attr("trackid"));
                return false;
            })
        }
        $("#tab-playlist").find("div#playlist").remove();
        $("#tab-playlist").append(html);
    });

    return module;
}());
