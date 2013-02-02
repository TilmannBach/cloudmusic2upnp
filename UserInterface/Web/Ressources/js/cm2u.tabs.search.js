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
    
    $("#search-basic").keyup(function(){
        clearTimeout(search_timeout);
        search_timeout = setTimeout(function(){
            var query = $("#search-basic").val();
            module.send_search_request(query);
        }, SEARCH_SEND_TIMEOUT);
    });

    return module;
}());
