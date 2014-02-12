cm2u.tabs.player = (new function()
{
    var module = {};
    
    cm2u.event.register("DeviceStateNotification", "remote", function (event, data) {
        
        if (data.MuteActive != null) {
            if (data.MuteActive == "true") {
                $("#button_player_mute").addClass("active");
            }
            else {
                $("#button_player_mute").removeClass("active");
            }
        }

    });

    cm2u.event.register("DeviceNotification", "remote", function (event, data) {
        if (data.Devices.length == 0) {
            initializePlayerPage();
        } else {
            initializePlayer();
        }

    });

    function initializePlayer() {
        var html = $('<div id="player_activated" class="container">'
            +'  <div class="row">'
            +'      <div class="col-sm-2 hidden-xs"><img style="width: 100%" src="http://25.media.tumblr.com/tumblr_mbr6937auf1qgt4z0_1349999464_cover.jpg"></div>'
            +'      <div class="col-xs-12 col-sm-10"><h2>Nico Pusch - Live at M...</h2></div>'
            +'  </div>'
            +'</div>'
            
            + '<div class="progress"><div class="progress-bar" role="progressbar" style="width: 60%;"></div></div>'
            + '<div style="text-align: center;">'

            + '<div class="btn-toolbar" role="toolbar">'
            + '<div class="btn-group">'
            + '<button id="button_player_stop" type="button" class="btn btn-default"><i class="glyphicon glyphicon-stop"></i></button>'
            + '<button id="button_player_togglePause" type="button" class="btn btn-default"><i class="glyphicon glyphicon-pause"></i></button>'
            + '<button id="button_player_forward" type="button" class="btn btn-default"><i class="glyphicon glyphicon-step-forward"></i></button>'
            + '</div>'
            + '<div class="btn-group">'
            + '<button id="button_player_random" type="button" class="btn btn-default"><i class="glyphicon glyphicon-random"></i></button>'
            + '<button id="button_player_repeat" type="button" class="btn btn-default"><i class="glyphicon glyphicon-retweet"></i></button>'
            + '<button id="button_player_mute" type="button" class="btn btn-default"><i class="glyphicon glyphicon-volume-off"></i></button>'
            + '</div>'

            + '</div>'
            
            );

        $("#tab-player").empty().append(html);

        $("#button_player_mute").click(function () {
            var currentMuteState = false;
            if ($("#button_player_mute").hasClass("active"))
                currentMuteState = true;
            cm2u.socket.protocol.set_mute(!currentMuteState);
            this.blur();
            return false;
        })
    }

    function initializePlayerPage() {
        var html = $('<div id="player_deactivated_warning" class="container">'
            + '  <div class="alert alert-warning"><strong>Warning!</strong> No rendering device found. Player is deactivated!</div>'
            + '</div>'
            );
        $("#tab-player").empty().append(html);
    }


    $(document).ready(function () {
        initializePlayerPage();
    });

    return module;
}());
