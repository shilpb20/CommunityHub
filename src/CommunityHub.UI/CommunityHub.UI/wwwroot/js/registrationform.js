document.addEventListener("DOMContentLoaded", function () {

    //Spouse section
    var maritalStatusSelector = document.getElementById("MaritalStatus");
    var spouseSection = document.getElementById("spouseSection");

    maritalStatusSelector.addEventListener("change", function () {
        if (maritalStatusSelector.value !== "Married") {
            spouseSection.style.display = "none"; // Hide the spouse section

            // Clear all spouse-related form data
            var spouseInputs = document.querySelectorAll("#spouseSection input, #spouseSection select");
            spouseInputs.forEach(function (input) {
                input.value = ""; // Reset input values
            });

            // Optionally, disable the spouse-related inputs to prevent form submission
            spouseInputs.forEach(function (input) {
                input.disabled = true; // Disable fields for unmarrieds
            });
        } else {
            spouseSection.style.display = "block"; // Show the spouse section

            // Enable spouse-related fields if marital status is "Married"
            var spouseInputs = document.querySelectorAll("#spouseSection input, #spouseSection select");
            spouseInputs.forEach(function (input) {
                input.disabled = false; // Enable fields when married
            });
        }
    });

    // Initialize visibility on page load
    if (maritalStatusSelector.value !== "Married")
    {
        if (spouseSection != null)
        {
            spouseSection.style.display = "none"; // Hide the spouse section initially if not married
            var spouseInputs = document.querySelectorAll("#spouseSection input, #spouseSection select");
            spouseInputs.forEach(function (input) {
                input.value = ""; // Clear values
                input.disabled = true; // Disable fields if not married
            });
        } 
    }

    var btnAddChild = document.getElementById("btnAddChild");
    var childInputSection = document.getElementById("childInputSection");
    var childNameInput = document.getElementById("childNameInput");
    var btnSaveChild = document.getElementById("btnSaveChild");
    var btnCancelChild = document.getElementById("btnCancelChild");
    var childrenList = document.getElementById("childrenList");

    // Show the input section when Add Child is clicked
    if (btnAddChild) {
        btnAddChild.addEventListener("click", function () {
            childInputSection.style.display = "block";
            btnAddChild.disabled = true; // Disable the "Add Child" button
        });
    }

    // Handle Save button click
    if (btnSaveChild) {
        btnSaveChild.addEventListener("click", function () {
            var childName = childNameInput.value.trim();

            // Check if the child name is not empty
            if (childName !== "") {
                // Check if the child name already exists in the list (case-insensitive check)
                var existingChildren = document.querySelectorAll('input[name="Children"]');
                var isDuplicate = false;

                // Loop through existing children to see if the name exists (case insensitive)
                existingChildren.forEach(function (input) {
                    if (input.value.trim().toLowerCase() === childName.toLowerCase()) {
                        isDuplicate = true;
                    }
                });

                if (isDuplicate) {
                    // Display error message if name is a duplicate
                    var errorMessage = document.getElementById("duplicateErrorMessage");
                    if (errorMessage) {
                        errorMessage.style.display = "block"; // Show error
                    }
                } else {
                    // Hide the error message if name is not a duplicate
                    var errorMessage = document.getElementById("duplicateErrorMessage");
                    if (errorMessage) {
                        errorMessage.style.display = "none"; // Hide error
                    }

                    // Create a new list item for the child
                    var listItem = document.createElement("li");
                    listItem.classList.add("list-group-item", "d-flex", "justify-content-between", "align-items-center");
                    listItem.innerHTML = childName +
                        '<input type="hidden" name="Children" value="' + childName + '" />' +
                        '<button type="button" class="btn btn-danger btn-sm btnDeleteChild">Delete</button>';

                    // Append the new child to the list
                    childrenList.appendChild(listItem);

                    // Clear input and ensure the list is visible
                    childNameInput.value = "";

                    // Ensure the child input section remains visible
                    childInputSection.style.display = "block"; // Make sure it's always visible

                    // Enable the Add Child button
                    btnAddChild.disabled = false;
                }
            }
        });
    }


    // Handle Cancel button click
    if (btnCancelChild) {
        btnCancelChild.addEventListener("click", function () {
            // Hide the input section without saving
            childInputSection.style.display = "none";
            btnAddChild.disabled = false;
        });
    }

    // Handle Delete button click (optional, if you want to delete the child)
    childrenList.addEventListener("click", function (event) {
        if (event.target && event.target.matches("button.btnDeleteChild")) {
            var listItem = event.target.closest("li");
            childrenList.removeChild(listItem);
        }
    });
});
