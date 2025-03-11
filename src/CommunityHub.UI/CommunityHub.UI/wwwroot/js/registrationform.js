document.addEventListener("DOMContentLoaded", function () {
    var maritalStatusSelector = document.getElementById("MaritalStatus");
    var spouseButton = document.getElementById("addSpouseButton");
    var spouseSection = document.getElementById("spouseSection");
    var removeSpouseButton = document.getElementById("removeSpouseButton");

    function toggleSpouseButton() {
        if (!spouseButton || !maritalStatusSelector || !spouseSection || !removeSpouseButton) return;

        // If marital status is "Married"
        if (maritalStatusSelector.value === "Married") {
            // If spouse section is not visible, enable the "Add Spouse" button
            if (spouseSection.style.display === "none" || spouseSection.style.display === "") {
                spouseButton.removeAttribute("disabled");
            }
            // If spouse section is visible, disable the "Add Spouse" button
            else {
                spouseButton.setAttribute("disabled", "true");
            }
        } else {
            spouseButton.setAttribute("disabled", "true"); // Disable button if not married
            spouseSection.style.display = "none"; // Hide spouse section
        }
    }

    // Show Spouse Section on Button Click
    spouseButton?.addEventListener("click", function () {
        if (spouseSection) {
            spouseSection.style.display = "block"; // Show spouse section
            spouseButton.setAttribute("disabled", "true"); // Disable button after adding spouse
            removeSpouseButton.style.display = "inline-block"; // Show remove spouse button
        }
    });

    // Remove Spouse Button functionality
    removeSpouseButton?.addEventListener("click", function () {
        if (spouseSection) {
            spouseSection.style.display = "none"; // Hide spouse section
            spouseButton.removeAttribute("disabled"); // Re-enable "Add Spouse" button
            removeSpouseButton.style.display = "none"; // Hide remove spouse button
        }
    });

    maritalStatusSelector?.addEventListener("change", toggleSpouseButton);

    toggleSpouseButton(); // Initial call to set the correct state when page loads
});
