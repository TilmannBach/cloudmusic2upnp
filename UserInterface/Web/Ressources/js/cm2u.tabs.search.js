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
        search_timeout = setTimeout(function(){
            var query = $("#search-track").val();
            module.send_search_request(query);
        }, SEARCH_SEND_TIMEOUT);
    });

    cm2u.event.register("SearchResponse", "remote", function (eventName, data) {
        var html = $("");
        for (var i in data.Tracks) {
            data.Tracks[i].Name
            html.append("<li><a href=\"#\">Track 3</a></li>");
        }
        $("#search-results").append(html);
    });

    return module;
}());
