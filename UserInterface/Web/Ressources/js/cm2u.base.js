/**
 * Base script
 * 
 * This script defines the only global var 'cm2u' and the app constants in
 * 'cm2u.c'.
 * 
 * 
 * @url: https://github.com/TilmannBach/cloudmusic2upnp
 * 
 */


var cm2u = {};


cm2u.c = {
	WEBSOCKET: 'ws://localhost:5009/',
}

console.log("froo");

/*
cm2u.utils = {
	get_form_data : function(form) {
		var $form = $(form);
		var data = {};

		$.each($form.find("*[name]"), function() {
			var elem = $(this);
			var name = elem.attr('name');
			var value = elem.val();

			if(name.slice(-2) == "[]"){
				name = name.slice(0,-2);
				if(data[name] == undefined)
					data[name] = [];
				data[name].push(value);
			} else {
				data[name] = value;
			}
		});

		return data;
	}
}*/