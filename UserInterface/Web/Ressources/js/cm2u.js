/**
 * Entry script.
 * 
 * This is the enty script for this application. It loads all required .js
 * files from the web server.
 * 
 * This script could be replaced with a minified version of the other script
 * files, later.
 * 
 * 
 * @url: https://github.com/TilmannBach/cloudmusic2upnp
 * 
 * @todo: Speed up, loading text (i.e. load css dynamicly)!
 * 
 */

var loading_queue = [
    "js/cm2u.base.js",
    "js/cm2u.event.js",
    "js/cm2u.socket.js",
    "js/cm2u.dialogs.js",
    "js/cm2u.tabs.js",
    "js/cm2u.tabs.search.js",
    "js/cm2u.tabs.settings.js",
    "js/cm2u.tabs.playlist.js",
    "js/cm2u.tabs.player.js"
];

function require(url, callback){
	/* from http://www.nczonline.net/blog/2009/07/28/the-best-way-to-load-external-javascript/ */
	var script = document.createElement("script")
	script.type = "text/javascript";

	if (script.readyState){  //IE
		script.onreadystatechange = function(){
			if (script.readyState == "loaded" || script.readyState == "complete"){
				script.onreadystatechange = null;
				callback();
			}
		};
	} else {  //Others
		script.onload = function(){
			callback();
		};
	}

	script.src = url + "?v=" + new Date().getTime();
	document.getElementsByTagName("head")[0].appendChild(script);
}

function load() {
	var url = loading_queue.shift();

	if(url == undefined) {
		cm2u.event.ready.done("scripts");
	} else {
		require(url, load);
	}
}

load();
