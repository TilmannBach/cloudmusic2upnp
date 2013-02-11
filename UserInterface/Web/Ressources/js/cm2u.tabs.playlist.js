cm2u.tabs.playlist = (new function()
{
    var module = {};
    
    cm2u.event.register("PlaylistNotification", "remote", function (eventName, data) {
        var html = $("<ul id=\"playlist\" data-role=\"listview\" data-inset=\"true\"></ul>");
        for (var i in data.Tracks) {
            var li = $("<li data-icon=\"minus\"><a href=\""+data.Tracks[i].ID+"\">" + data.Tracks[i].Name + "</a></li>");
            html.append(li);
            li.find("a").click(function(){
                cm2u.socket.protocol.play_request($(this).attr("href"));
                return false;
            })
        }
        $("#tab-playlist").find("ul").remove();
        $("#tab-playlist").append(html).trigger("create");
    });

    return module;
}());
