const requestUrl =
    'https://api.unsplash.com/search/photos?query=motorcycle&client_id=T-FCIQ-C317KwK9CTjTFtgTnc0fEq13ZwCDt9mZ8ECE';
const getImagesButton = document.querySelector('.getImagesButton');
const imageToDisplay = document.querySelector('.imageToDisplay');

getImagesButton.addEventListener('click', async () => {
    let randomImage = await getNewImage();
    imageToDisplay.src = randomImage;
});

async function getNewImage() {
    let randomNumber = Math.floor(Math.random() * 10);
    return fetch(requestUrl)
        .then((response) => response.json())
        .then((data) => {
            let allImages = data.results[randomNumber];
            return allImages.urls.regular;
        });
}