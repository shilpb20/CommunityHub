document.addEventListener("DOMContentLoaded", function () {

    var successMessage = document.getElementById("successGrowl");
    if (successMessage) {
        setTimeout(function () {
            $(successMessage).fadeOut();
        }, 5000);
    }
});
