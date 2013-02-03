

cm2u.tabs = (new function()
{
	var module = {};
	
	function setInitialContentVisibility() {
	    $("#tab-player").show();
	    $("#tab-settings").hide();
	    $("#tab-search").hide();
	    $("#tab-playlist").hide();
	    $("#tab-about").hide();
	}

	function hideAllContent() {
	    $("#tab-player").hide();
	    $("#tab-settings").hide();
	    $("#tab-search").hide();
	    $("#tab-playlist").hide();
	    $("#tab-about").hide();
	}

	function setNavbarClickEvents() {
	    $("#navbar-player").click(function() {
	        hideAllContent();
	        $("#navbar-about").removeClass("ui-btn-active");
	        $("#tab-player").show();
	    });
	    $("#navbar-playlist").click(function() {
	        hideAllContent();
	        $("#navbar-about").removeClass("ui-btn-active");
	        $("#tab-playlist").show();
	    });
	    $("#navbar-search").click(function() {
	        hideAllContent();
	        $("#navbar-about").removeClass("ui-btn-active");
	        $("#tab-search").show();
	    });
	    $("#navbar-settings").click(function () {
	        hideAllContent();
	        $("#navbar-about").removeClass("ui-btn-active");
	        $("#tab-settings").show();
	    });
	    $("#navbar-about").click(function () {
	        hideAllContent();
	        $("#tab-about").show();
	        $("#navbar a").removeClass("ui-btn-active");
	        $("#navbar-about").addClass("ui-btn-active");
	    });
	}

	$(document).ready(function () {
	    setInitialContentVisibility();
	    setNavbarClickEvents();
	});
	
	return module;
}());