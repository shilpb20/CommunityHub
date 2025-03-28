document.addEventListener("DOMContentLoaded", function () {

    var successMessage = document.getElementById("successMessage");
    if (successMessage) {
        setTimeout(function () {
            $(successMessage).fadeOut();
        }, 5000);
    }
});
