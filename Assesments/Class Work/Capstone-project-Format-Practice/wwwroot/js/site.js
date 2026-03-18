// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Full-featured AJAX Navigation for TheDetectives
$(document).ready(function () {
    // 1. Intercept Link Clicks (Single-Page Navigation)
    $(document).on("click", "a:not([target]):not([href^='#']):not([href^='javascript']):not(.no-ajax)", function (e) {
        const url = $(this).attr("href");
        if (url && url !== "#" && !url.includes("Identity")) {
            e.preventDefault();
            loadPage(url);
        }
    });

    // 2. Handle Browser Back/Forward Buttons
    window.onpopstate = function (event) {
        if (event.state && event.state.url) {
            loadPage(event.state.url, true);
        } else {
            loadPage(location.pathname, true);
        }
    };

    function loadPage(url, isPopState = false) {
        // Start showing loading state
        $("#main-content").css("opacity", "0.5");

        $.ajax({
            url: url,
            method: "GET",
            success: function (data) {
                // Parse the full HTML returned
                const newDoc = new DOMParser().parseFromString(data, "text/html");
                const newContent = $(newDoc).find("#main-content").html();
                const newTitle = newDoc.title;

                // Update the page
                $("#main-content").html(newContent);
                document.title = newTitle;
                $("#main-content").css("opacity", "1");

                // Update browser history (unless we're going back/forward)
                if (!isPopState) {
                    history.pushState({ url: url }, newTitle, url);
                }

                // Scroll to top
                window.scrollTo(0, 0);

                // Re-initialize specific components if needed (e.g. alerts)
                setupAlerts();
            },
            error: function () {
                $("#main-content").css("opacity", "1");
                window.location.href = url; // Fallback to full reload on error
            }
        });
    }

    function setupAlerts() {
        setTimeout(function () {
            $(".alert").alert('close');
        }, 3000);
    }

    // Initialize components on first load
    setupAlerts();
});
