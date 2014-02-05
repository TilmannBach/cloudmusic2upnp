
cm2u.dialogs = (new function()
{
    var module = {};

	cm2u.event.register('remote.error', 'local', function(eventname, data){
	
	    $("#dialog-remote-error").modal('show');

	});
	
	$( "#dialog-remote-error" ).find("button").click(function(){
	    cm2u.socket.reconnect();
	    $("#dialog-remote-error").modal('hide');
	    return false;
	})

    return module;
}());
