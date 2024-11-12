async function confirmMakePublic(productId) {
    const userConfirmed = confirm(
        "This will make the product public. It will be listed and searchable! The product must have at least one image and one file to perform this action. Are you sure?"
    );

    if (userConfirmed) {
        try {
            const response = await fetch(`/Product/MakeProductPublic/${productId}`, {
                method: 'PUT',
                headers: { 'Content-Type': 'application/json' }
            });

            if (!response.ok) {
                // Extract and log error message
                const errorMessage = await response.json();
                // console.error('Error:', errorMessage.error);
                // alert('Failed to make product public: ' + errorMessage.error);
                // location.reload();
            } else {
                // Extract success message
                const data = await response.json();
                // console.log('Success:', data.message);
                // alert(data.message);
                // location.reload();
            }
        } catch (error) {
            console.error('Error:', error);
            // alert('An error occurred while making the product public.');
        }

        location.reload();
    }
}

function AddProductPicture(productId) {
    // create input element
    const fileInput = document.createElement("input");
    fileInput.type = "file";
    fileInput.id = "productImageInput";
    fileInput.accept = "image/jpeg, image/jpg";
    fileInput.hidden = true;
    fileInput.addEventListener("change", () => {
        const file = fileInput.files[0];
        if (file) {
            uploadProductImage(productId, file);
        }
    });

    // open it
    fileInput.click();
}

async function uploadProductImage(productId, file) {
    try {
        // create form with data
        const formData = new FormData();
        formData.append("image", file);

        // Make request to server
        const response = await fetch(`/Product/UploadProductImage/${productId}`, {
            method: 'POST',
            body: formData
        });

        if (response.ok) {
            // console.log("Image uploaded successfully!");
            // location.reload();
        } else { }
    } catch (error) {
        console.error("Error uploading image:", error);
    }

    location.reload();
}

function removeImage(imageId) {
    const userConfirmed = confirm(
        "Do you really want to remove this image from product?"
    );

    if (userConfirmed) {
        fetch('/Product/RemoveProductFile/' + imageId, {
            method: 'DELETE',
            headers: {
                'Content-Type': 'application/json'
            }
        })
            .then(response => {
                if (response.ok) {
                    console.log('File deleted successfully');
                    location.reload();
                } else {
                    console.error('Failed to delete the file');
                    location.reload();
                }
            })
            .catch(error => console.error('Error:', error));
    }
}

function startUploadFile(productId) {
    const fileName = document.getElementById('namefileupload').value;
    const fileInput = document.getElementById('fileupload');
    const file = fileInput.files[0];

    // Check if a file is selected
    if (!file) {
        alert("Please select a file to upload.");
        return;
    }

    // Create a FormData object to hold the file and name data
    const formData = new FormData();
    formData.append('fileName', fileName);
    formData.append('productId', productId);
    formData.append('file', file);

    // Send the PUT request
    fetch('/Product/UploadProductFile', {
        method: 'PUT',
        body: formData
    })
        .then(response => {
            if (response.ok) {
                console.log('File uploaded successfully');
                location.reload();
            } else {
                console.error('Failed to upload the file');
                location.reload();
            }
        })
        .catch(error => console.error('Error:', error));
}

function updateBasicProductInfo(productId) {
    const productName = document.getElementById('ProductName').value;
    const productPrice = document.getElementById('ProductPrice').value;
    const productDescription = document.getElementById('ProductDescription').value;

    // Create a data object to send in the request
    const data = {
        Id: productId,
        name: productName,
        price: parseFloat(productPrice),
        description: productDescription
    };

    // Send the PUT request
    fetch(`/Product/UpdateBasicInfo/`, {
        method: 'PUT',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(data)
    })
        .then(response => {
            if (response.ok) {
                // alert('Product information updated successfully.');
                location.reload();
            } else {
                // alert('Failed to update product information.');
                location.reload();
            }
        })
        .catch(error => {
            console.error('Error:', error);
            // alert('An error occurred while updating product information.');
        });
}

function removeProductCategory(tagId, productId) {
    const userConfirmed = confirm(
        "Do you really want to remove tag from this product?"
    );

    if (userConfirmed) {
        const formData = new FormData();
        formData.append('tagId', tagId);
        formData.append('productId', productId);

        fetch('/Product/RemoveProductTag/', {
            method: 'DELETE',
            body: formData
        })
            .then(response => {
                if (response.ok) {
                    console.log('Tag removed successfully');
                    location.reload();
                } else {
                    console.error('Failed to remove the tag');
                    location.reload();
                }
            })
            .catch(error => console.error('Error:', error));
    }
}

function renderTable(data) {
    // clear table if exist
    const displayDiv = document.getElementById("displaySearchTable");
    displayDiv.innerHTML = "";

    // create new table
    const table = document.createElement("table");
    table.classList.add("table");

    // title of table
    const headerRow = document.createElement("tr");
    const headers = ["Id", "DisplayName", "Action"];
    headers.forEach(headerText => {
        const header = document.createElement("th");
        header.textContent = headerText;
        headerRow.appendChild(header);
    });
    table.appendChild(headerRow);

    // generating rows
    data.forEach(item => {
        const row = document.createElement("tr");

        // col for Id
        const idCell = document.createElement("td");
        idCell.textContent = item.Id;
        row.appendChild(idCell);

        // col for DisplayName
        const nameCell = document.createElement("td");
        nameCell.textContent = item.DisplayName;
        row.appendChild(nameCell);

        // col for button
        const actionCell = document.createElement("td");
        const button = document.createElement("button");
        button.textContent = "Add";
        button.classList.add("btn", "btn-outline-primary");
        button.onclick = function () {
            // button is 'add' clicked

            fetch('/Product/AddProductTag?tagId=' + item.Id + "&productId=" + getProductIdFromUrl(), {
                method: 'GET'
            })
                .then(response => {
                    if (response.ok) {
                        // console.log('Tag added successfully');
                        location.reload();
                    } else {
                        // console.error('Failed to add the tag');
                        location.reload();
                    }
                })
                .catch(error => console.error('Error:', error));
        };
        actionCell.appendChild(button);
        row.appendChild(actionCell);

        // add row to table
        table.appendChild(row);
    });

    // add table to displayDiv
    displayDiv.appendChild(table);
}

function getProductIdFromUrl() {
    // Получаваме текущия URL
    const url = window.location.href;

    // Използваме регулярни изрази, за да намерим числовото ID след последния слеш
    const regex = /\/(\d+)$/;
    const match = url.match(regex);

    if (match && match[1]) {
        return match[1]; // Връщаме ID-то
    } else {
        return null; // Връща null, ако няма ID в URL-то
    }
}


document.getElementById("tagSearchField").addEventListener("input", function () {
    const searchValue = this.value;

    if (searchValue.length > 2) {
        fetch(`/Product/GetTags?val=${encodeURIComponent(searchValue)}`)
            .then(response => {
                if (!response.ok) {
                    throw new Error("Network response was not ok");
                }
                return response.json();
            })
            .then(data => {
                console.log("Tags:", data);
                renderTable(data)

                // Тук можете да добавите логика за обработка на получените тагове
            })
            .catch(error => {
                console.error("There was a problem with the fetch operation:", error);
            });
    }
});
