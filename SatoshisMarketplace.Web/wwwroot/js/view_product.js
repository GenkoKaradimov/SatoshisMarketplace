// favorite products
function addToFavoritesList(productId) {
    // Add product from favorite list of user - Send the PUT request
    fetch(`/Product/AddFavoriteProducts?productId=` + productId, {
        method: 'PUT'
    }).then(response => {
        if (response.ok) {
            location.reload();
        } else {
            console.error('Server Error:', error);
        }
    }).catch(error => {
        console.error('Error:', error);
    });
}
function removeFromFavoritesList(productId) {
    // Remove product from favorite list of user - Send the PUT request
    fetch(`/Product/RemoveFavoriteProducts?productId=` + productId, {
        method: 'PUT'
    }).then(response => {
        if (response.ok) {
            location.reload();
        } else {
            console.error('Server Error:', error);
        }
    }).catch(error => {
        console.error('Error:', error);
    });
}

