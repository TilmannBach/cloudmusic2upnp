cm2u.tabs.provider = (new function()
{
    var module = {};
    
    cm2u.event.register("ProviderNotification", "remote", function (event, data) {
        var tpl = $("#provider-template");

        if (data.Providers.length == 0) {
            tpl.nextAll().remove();
        }

        for (var i = 0; i < data.Providers.length; i++) {
            var provider = data.Providers[i];
            var html = $(tpl.html());
            var label = html.find(".provider-label");
            var input = html.find(".provider-input");

            html.attr("id", "provider-" + provider.ID);
            input.attr("id", "provider-" + provider.ID);
            label.text(provider.Name);

            $("#tab-settings").find("#provider-" + provider.ID).remove();

            tpl.after(html);
        }

    });

    cm2u.event.register("DeviceNotification", "remote", function (event, data) {
        var tpl = $("#renderers-template");

        if (data.Devices.length == 0)
        {
            tpl.nextAll().remove();
        }

        for (var i = 0; i < data.Devices.length; i++) {
            var device = data.Devices[i];
            var html = $(tpl.html());
            var label = html.find(".renderer-label");
            var input = html.find(".renderer-input");

            html.attr("id", "renderers-" + device.Udn);
            input.attr("id", "renderer-" + device.Udn);
            label.text(device.Name);
            
            $("#tab-settings").find("#renderers-" + device.Udn).remove();

            tpl.after(html);
        }

    });
    
    return module;
}());
