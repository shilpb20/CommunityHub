window.filterByLocation = function () {
    console.log("filterByLocation() function executed!");
    var selectedLocation = document.getElementById('locationFilter').value;
    var rows = document.querySelectorAll('#requestTableBody .mainRow');
    var count = 1;

    var sortDropdown = document.getElementById('sortDropdown');
    if (selectedLocation === "" || selectedLocation === "all locations")
    {
        sortDropdown.innerHTML = `
            <option value="LocationA_Z">Sort by Location A-Z</option>
            <option value="LocationZ_A">Sort by Location Z-A</option>
            <option value="FullNameA_Z">Sort by Name A-Z</option>
            <option value="FullNameZ_A">Sort by Name Z-A</option>
        `;
    } else
    {
        sortDropdown.innerHTML = `
            <option value="FullNameA_Z">Sort by Name A-Z</option>
            <option value="FullNameZ_A">Sort by Name Z-A</option>
        `;
    }

    rows.forEach(function (row) {
        var locationCell = row.querySelector('.compact-location').textContent.trim();

        if (selectedLocation === "" || locationCell === selectedLocation) {
            row.style.display = "";
            row.cells[0].textContent = count;
            count++;
        } else {
            row.style.display = "none";
        }
    });

    sortUsers();
};

window.sortUsers = function () {
    console.log("sortUsers() function executed!");
    var sortDropdown = document.getElementById('sortDropdown');
    var selectedSort = sortDropdown.value;
    var tableBody = document.getElementById('requestTableBody');
    var rows = Array.from(tableBody.querySelectorAll('.mainRow')).filter(row => row.style.display !== "none");

    rows.sort((rowA, rowB) => {
        var cellA, cellB;
        if (selectedSort.includes("Location")) {
            cellA = rowA.querySelector('.compact-location').textContent.trim();
            cellB = rowB.querySelector('.compact-location').textContent.trim();
        } else if (selectedSort.includes("FullName")) {
            cellA = rowA.cells[1].textContent.trim();
            cellB = rowB.cells[1].textContent.trim();
        }

        return selectedSort.endsWith("A_Z") ? cellA.localeCompare(cellB) : cellB.localeCompare(cellA);
    });

    rows.forEach((row, index) => {
        row.cells[0].textContent = index + 1;
        tableBody.appendChild(row);
    });
};
