function ChangeProfilePicture() {
    // create input element
    const fileInput = document.createElement("input");
    fileInput.type = "file";
    fileInput.id = "profileImageInput";
    fileInput.accept = "image/jpeg, image/jpg";
    fileInput.hidden = true;
    fileInput.addEventListener("change", () => {
        const file = fileInput.files[0];
        if (file) {
            uploadProfileImage(file);
        }
    });

    // open it
    fileInput.click();
}

async function uploadProfileImage(file) {
    try {
        // create form with data
        const formData = new FormData();
        formData.append("profileImage", file);

        // Make request to server
        const response = await fetch('/User/UploadProfileImage', {
            method: 'POST',
            body: formData
        });

        if (response.ok) {
            // console.log("Image uploaded successfully!");
            location.reload();
        } else {
            Alert("Failed to upload image.");
        }
    } catch (error) {
        console.error("Error uploading image:", error);
    }
}