cm2u.tabs.provider = (new function()
{
    var module = {};
    
    cm2u.event.register("ProviderNotification", "remote", function(event, data){
        var tpl = $("#provider-template");

        for (var i = 0; i < data.Providers.length; i++) {
            var provider = data.Providers[i];
            var html = $(tpl.html());
            var label = html.find(".provider-label");
            var input = html.find(".provider-input");
            
            input.attr("id", "provider-"+provider.ID);
            label.text(provider.Name);
            tpl.after(html);
        }

    });
    
    return module;
}());
