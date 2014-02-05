cm2u.tabs.search = (new function()
{
    var module = {};
    
    var search_timeout = null;
    var SEARCH_SEND_TIMEOUT = 200;
    

    $("#search-track").keyup(function () {
        clearTimeout(search_timeout);
        $("#tab-search").find("div#search-results").remove();
        search_timeout = setTimeout(function(){
            var query = $("#search-track").val();
            cm2u.socket.protocol.search_request(query);
        }, SEARCH_SEND_TIMEOUT);
    });

    cm2u.event.register("SearchResponse", "remote", function (eventName, data) {
        var html = $('<div id="search-results" class="list-group"></div>');
        for (var i in data.Tracks) {
            var li = $("<a class=\"list-group-item\" trackid=\"" + data.Tracks[i].ID + "\"><span class=\"badge\"><span class=\"glyphicon glyphicon-plus\"></span></span>" + data.Tracks[i].Name + "</a>");
            html.append(li);
            li.click(function(){
                cm2u.socket.protocol.play_request($(this).attr("trackid"));
                return false;
            })
        }
        $("#tab-search").find("div#search-results").remove();
        $("#tab-search").append(html);
    });

    return module;
}());
