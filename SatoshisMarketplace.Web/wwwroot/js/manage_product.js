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