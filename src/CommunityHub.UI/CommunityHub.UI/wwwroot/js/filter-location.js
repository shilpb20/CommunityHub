document.addEventListener("DOMContentLoaded", function () {
    function filterByLocation() {
        var selectedLocation = document.getElementById('locationFilter').value;
        var rows = document.querySelectorAll('#requestTableBody .mainRow');
        var count = 1;
        rows.forEach(function (row) {
            var locationCell = row.cells[3].textContent.trim();
            if (selectedLocation === "" || locationCell === selectedLocation) {
                row.style.display = "";
                row.cells[0].textContent = count;
                count++;
            } else {
                row.style.display = "none";
            }
        });
    }
});
