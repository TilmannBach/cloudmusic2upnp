/**
 * Websocket handler.
 * 
 * Opens the socket to the server and handles all requests.
 * 
 * 
 * @url: https://github.com/TilmannBach/cloudmusic2upnp
 * 
 */


cm2u.socket = (new function(){
	var module = {};
	
	var socket;
	
	module.send = function(event, data)
	{
        var msg = JSON.stringify({
            Method: event,
            Body: data
        });
		socket.send(msg);
	};
	

	var onopen = function () {
		cm2u.event.ready.done("socket");
	};
	

	var onerror = function (error) {
		cm2u.event.trigger("remote.error", 'local');
	};

	
	var onmessage = function (e) {
		var j = JSON.parse(e.data);
		var event = j.Method;
		var data = j.Body;
		
		cm2u.event.trigger(event, 'remote', data);
	};
	
	
	var onclose = function(e) {
		cm2u.event.trigger("remote.error", 'local');
	}

	
	var open = function()
	{
		socket = new WebSocket(cm2u.c.WEBSOCKET);
		socket.onopen = onopen;
		socket.onerror = onerror;
		socket.onmessage = onmessage;
		socket.onclose = onclose;
	}
	
	cm2u.event.register('session.ready.scripts', 'local', function(eventname, data){
		open();
	});
	
	module.reconnect = function()
	{
		open();
	}
    
    module.protocol = {
    
        play_request : function(media_url) {
            module.send("PlayRequest", {
                MediaUrl: media_url,
            });
        },
        
        search_request : function(query) {
            module.send("SearchRequest", {
                Query: query
            });
        }
    }
	
	return module;	
}());