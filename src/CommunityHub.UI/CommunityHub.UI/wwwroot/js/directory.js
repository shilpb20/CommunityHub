let usersData = [];
let filteredData = [];
let currentPage = 1;
let rowsPerPage = 20;

document.addEventListener("DOMContentLoaded", function () {
    // Ensure that the data is available and assign to usersData
    if (window.usersData && Array.isArray(window.usersData)) {
        usersData = window.usersData;
        filteredData = [...usersData]; // Clone the data for filtering
        renderTable(); // Initial render with all users
    } else {
        console.error("Error: users data is not available or is not in the correct format.");
    }

    // Set default sorting if no filter is applied
    if (!document.getElementById('locationFilter').value) {
        document.getElementById('sortDropdown').value = "FullName_A_Z";
    }
});

// Function to filter by location
function filterByLocation() {
    const selectedLocation = document.getElementById('locationFilter').value;

    if (selectedLocation === "") {
        filteredData = [...usersData]; // No filter
    } else {
        filteredData = usersData.filter(user =>
            user.Location === selectedLocation ||
            (user.SpouseInfo && user.SpouseInfo.Location === selectedLocation)
        );
    }

    // Reset to default sorting (Name A-Z) when location is selected
    document.getElementById('sortDropdown').value = "FullName_A_Z";
    sortUsers();  // Apply the default sorting
    renderTable();
}

// Function to handle sorting
function sortUsers() {
    const sortOption = document.getElementById('sortDropdown').value;
    if (sortOption === 'FullName_A_Z') {
        filteredData.sort((a, b) => a.FullName.localeCompare(b.FullName));
    } else if (sortOption === 'FullName_Z_A') {
        filteredData.sort((a, b) => b.FullName.localeCompare(a.FullName));
    } else if (sortOption === 'Location_A_Z') {
        filteredData.sort((a, b) => a.Location.localeCompare(b.Location));
    } else if (sortOption === 'Location_Z_A') {
        filteredData.sort((a, b) => b.Location.localeCompare(a.Location));
    }
    renderTable();
}

// Render table with filtered and sorted data
function renderTable() {
    const tableBody = document.getElementById('requestTableBody');
    tableBody.innerHTML = '';

    let serial = (currentPage - 1) * rowsPerPage + 1;
    const paginatedData = filteredData.slice((currentPage - 1) * rowsPerPage, currentPage * rowsPerPage);

    paginatedData.forEach(user => {
        let row = `<tr class="mainRow">
            <td>${serial}</td>
            <td class="user-name">${user.FullName}</td>
            <td class="user-contact">${user.ContactNumber}</td>
            <td class="user-location">${user.Location}</td>
            <td class="spouse-name">${user.SpouseInfo ? user.SpouseInfo.FullName : ''}</td>
            <td class="spouse-contact">${user.SpouseInfo ? user.SpouseInfo.ContactNumber : ''}</td>
            <td class="spouse-location">${user.SpouseInfo ? user.SpouseInfo.Location : ''}</td>
        </tr>`;
        tableBody.innerHTML += row;
        serial++;
    });

    // Reinitialize pagination after rendering the table
    initializePagination();
}

// Initialize pagination
function initializePagination() {
    const paginationContainer = document.getElementById('requestTableBody-pagination');
    paginationContainer.innerHTML = '';

    const totalPages = Math.ceil(filteredData.length / rowsPerPage);

    // Create previous page button
    const prevButton = document.createElement('button');
    prevButton.textContent = "<<";
    prevButton.disabled = currentPage === 1;
    prevButton.addEventListener('click', () => {
        if (currentPage > 1) {
            currentPage--;
            renderTable();
        }
    });
    paginationContainer.appendChild(prevButton);

    // Create page number buttons
    for (let i = 1; i <= totalPages; i++) {
        const pageButton = document.createElement('button');
        pageButton.textContent = i;
        pageButton.classList.toggle('active', i === currentPage);
        pageButton.addEventListener('click', () => {
            currentPage = i;
            renderTable();
        });
        paginationContainer.appendChild(pageButton);
    }

    // Create next page button
    const nextButton = document.createElement('button');
    nextButton.textContent = ">>";
    nextButton.disabled = currentPage === totalPages;
    nextButton.addEventListener('click', () => {
        if (currentPage < totalPages) {
            currentPage++;
            renderTable();
        }
    });
    paginationContainer.appendChild(nextButton);
}
