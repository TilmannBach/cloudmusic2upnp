

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
	    $("#navbar-player").removeClass("active");
	    $("#navbar-playlist").removeClass("active");
	    $("#navbar-search").removeClass("active");
	    $("#navbar-settings").removeClass("active");
	    $("#navbar-about").removeClass("active");
	}

	function setNavbarClickEvents() {
	    $("#navbar-player").click(function() {
	        hideAllContent();
	        $("#tab-player").show();
	        $("#navbar-player").addClass("active");
	    });
	    $("#navbar-playlist").click(function() {
	        hideAllContent();
	        $("#tab-playlist").show();
	        $("#navbar-playlist").addClass("active");
	    });
	    $("#navbar-search").click(function() {
	        hideAllContent();
	        $("#tab-search").show();
	        $("#navbar-search").addClass("active");
	    });
	    $("#navbar-settings").click(function () {
	        hideAllContent();
	        $("#tab-settings").show();
	        $("#navbar-settings").addClass("active");
	    });
	    $("#navbar-about").click(function () {
	        hideAllContent();
	        $("#tab-about").show();
	        $("#navbar-about").addClass("active");
	    });
	}

	$(document).ready(function () {
	    setInitialContentVisibility();
	    setNavbarClickEvents();
	});
	
	return module;
}());