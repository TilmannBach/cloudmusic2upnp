
cm2u.dialogs = (new function()
{
    var module = {};

	cm2u.event.register('remote.error', 'local', function(eventname, data){
	
		$("#dialog-remote-error").popup( "open", {});

	});
	
	$( "#dialog-remote-error" ).bind({
   		popupafterclose: function(event, ui) {
   			cm2u.socket.reconnect();
   		}
	});

    return module;
}());
