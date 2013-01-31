/**
 * Event dispatcher.
 *
 * This module 'quiz.event' provides the event dispatcher for this application.
 * All communication within this app are handled through events, which are
 * handled here. There are three important functions:
 *
 *  - cm2u.event.register(event, func);
 *      Registers a new callback for a certain 'event' (string). Every time an
 *      event is triggered, all registered callbacks will be called with an
 *      event object as argument.
 *
 *  - cm2u.event.unregister(event, ident);
 *      TODO
 *
 *  - cm2u.event.trigger(event);
 *      Triggers a certain event (string). Everybody can trigger every event
 *      and then all registered callbacks will be called.
 *
 * The module also take care about the conditions for the 'ready' event.
 *
 *
 * @url: https://github.com/TilmannBach/cloudmusic2upnp
 *
 */


cm2u.event = (new function()
{
    var module = {};
    var list_local = {};
    var list_remote = {};


    module.register = function(eventname, target, cb)
    {
        if (target == 'local') {
            if (!(eventname in list_local))
                list_local[eventname] = new Array();

            list_local[eventname].push(cb);
        }

        if (target == 'remote') {
            if (!(eventname in list_remote))
                list_remote[eventname] = new Array();

            list_remote[eventname].push(cb);
        }
        
    };

    module.trigger = function(eventname, target, data)
    {
        console.log('Trigger ('+target+'): ' + eventname, data);

        if (target == 'remote') {
            if (eventname in list_remote) {
                list = list_remote[eventname];
                for (var i = 0; i < list.length; i++) {
                    cb = list[i];
                    cb(eventname, data);
                }
            }
        }

        if (target == 'local') {
            if (eventname in list_local) {
                list = list_local[eventname];
                for (var i = 0; i < list.length; i++) {
                    cb = list[i];
                    cb(eventname, data);
                }
            }
        }
    };


    return module;
}());


cm2u.event.ready = (new function(){
    var module = {};

    var required = {
        dom: false,
        socket: false,
        scripts: false,
    };

    module.done = function(name) {
        required[name] = true
        cm2u.event.trigger("session.ready." + name, "local");

        if(required.socket && required.dom && required.scripts) {
            cm2u.event.trigger("session.ready", "local");
        };
    }

    $(document).ready(function() {
        module.done("dom");
    });



    return module;
}());