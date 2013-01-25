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
	
	var URI = quiz.c.WEBSOCKET;
	
	var socket;
	
	module.send = function(event, data)
	{
		var e = [ event, data ];
		socket.send(JSON.stringify(e));
	};
	

	
	var onopen = function () {
		cm2u.event.ready.done("socket");
	};
	

	var onerror = function (error) {
		cm2u.event.trigger("local:session.error", {
			title: "WebSocket error",
			text: "WebSocket connection closed",
		});
	};

	
	var onmessage = function (e) {
		var j = eval(e.data);
		var event = j[0];
		var data = j[1];
		
		cm2u.event.socket_event(event, data);
	};
	
	
	var onclose = function(e) {
		cm2u.event.trigger("local:session.error", {
			title: "WebSocket error",
			text: "WebSocket connection closed",
		});
	}

	
	var open = function()
	{
		socket = new WebSocket(URI);
		socket.onopen = onopen;
		socket.onerror = onerror;
		socket.onmessage = onmessage;
		socket.onclose = onclose;
	}
	
	open();
	
	
	
	return module;	
}());