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


    var parse_eventname = function(eventname) {
        var local;
        var remote;

        if (eventname.substr(0, 6) == 'local:') {
            local = true;
            remote = false;
            ident = eventname.substr(6);

        } else if (eventname.substr(0, 7) == 'remote:') {
            local = false;
            remote = true;
            ident = eventname.substr(7);

        } else {
            local = true;
            remote = true;
            ident = eventname;
        }

        return {
            'ident': ident,
            'local': local,
            'remote': remote
        };
    };

    module.register = function(eventname, cb)
    {
        e = parse_eventname(eventname);

        if (e.local == true) {
            if (!(e.ident in list_local))
                list_local[e.ident] = new Array();

            list_local[e.ident].push(cb);
        }

        if (e.remote == true) {
            if (!(e.ident in list_remote))
                list_remote[e.ident] = new Array();

            list_remote[e.ident].push(cb);
        }
    };

    module.trigger = function(eventname, data)
    {
        console.log('Trigger: ' + eventname, data);
        e = parse_eventname(eventname);

        if (e.remote == true) {
            quiz.socket.send(e.ident, data);
        }

        if (e.local == true) {
            if (e.ident in list_local) {
                list = list_local[e.ident];
                for (var i = 0; i < list.length; i++) {
                    cb = list[i];
                    cb(e.ident, data);
                }
            }

        }
    };

    module.socket_event = function(eventname, data)
    {
        console.log('Socket Event: ' + eventname, data);
        e = parse_eventname(eventname);

        if (e.ident in list_remote) {
            list = list_local[e.ident];
            for (var i = 0; i < list.length; i++) {
                cb = list[i];
                cb(e.ident, data);
            }
        }
    };

    return module;
}());


cm2u.event.ready = (new function(){
    var module = {};

    var required = {
        tabs: false,
        dom: false,
        socket: false,
        scripts: false,
    };

    module.done = function(name) {
        required[name] = true
        
        console.log(name);

        /*if(required.tabs && required.dom && required.socket && required.scripts) {*/
        if(required.socket) {
            cm2u.event.trigger("session.ready");
        };
    }

    $(document).ready(function() {
        module.done("dom");
    });



    return module;
}());