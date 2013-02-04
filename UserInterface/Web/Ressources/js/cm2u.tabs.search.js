cm2u.tabs.search = (new function()
{
    var module = {};
    
    var search_timeout = null;
    var SEARCH_SEND_TIMEOUT = 200;
    

    $("#search-track").keyup(function () {
        clearTimeout(search_timeout);
        $("#tab-search").find("ul").remove();
        search_timeout = setTimeout(function(){
            var query = $("#search-track").val();
            cm2u.socket.protocol.search_request(query);
        }, SEARCH_SEND_TIMEOUT);
    });

    cm2u.event.register("SearchResponse", "remote", function (eventName, data) {
        var html = $("<ul id=\"search-results\" data-role=\"listview\" data-inset=\"true\"></ul>");
        for (var i in data.Tracks) {
            var li = $("<li><a href=\""+data.Tracks[i].MediaUrl+"\">" + data.Tracks[i].Name + "</a></li>");
            html.append(li);
            li.find("a").click(function(){
                cm2u.socket.protocol.play_request($(this).attr("href"));
                return false;
            })
        }
        $("#tab-search").find("ul").remove();
        $("#tab-search").append(html).trigger("create");
    });

    return module;
}());
