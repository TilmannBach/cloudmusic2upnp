cm2u.tabs.search = (new function()
{
    var module = {};
    
    var search_timeout = null;
    var SEARCH_SEND_TIMEOUT = 200;
    
    module.send_search_request = function(query)
    {
        cm2u.socket.send("SearchRequest", {
            Query: query
        })
    };
    
    $("#search-track").keyup(function () {
        clearTimeout(search_timeout);
        $("#tab-search").find("ul").remove();
        search_timeout = setTimeout(function(){
            var query = $("#search-track").val();
            module.send_search_request(query);
        }, SEARCH_SEND_TIMEOUT);
    });

    cm2u.event.register("SearchResponse", "remote", function (eventName, data) {
        var html = $("<ul id=\"search-results\" data-role=\"listview\" data-inset=\"true\"></ul>");
        for (var i in data.Tracks) {
            html.append("<li><a href=\""+data.Tracks[i].MediaUrl+"\">" + data.Tracks[i].Name + "</a></li>");
        }
        $("#tab-search").find("ul").remove();
        $("#tab-search").append(html).trigger("create");
    });

    return module;
}());
