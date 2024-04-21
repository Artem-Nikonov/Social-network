function showError(message) {
    var errorWindow = document.getElementById("errorWindow");
    var errorMessage = document.getElementById("errorMessage");

    errorMessage.textContent = message;
    errorWindow.classList.add("active");

    setTimeout(function () {
        errorWindow.classList.remove("active");
        errorWindow.classList.add("fade-out");
        setTimeout(function () {
            errorWindow.classList.remove("fade-out");
        }, 500); // После завершения анимации исчезновения
    }, 5000); // После 5 секунд исчезает
}