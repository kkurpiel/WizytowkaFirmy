// Funkcja ukrywająca alert po 5 sekundach
document.addEventListener("DOMContentLoaded", function () {
    var alertElement = document.getElementById("wyslanoEmailAlert");
    if (alertElement) {
        setTimeout(function () {
            alertElement.style.display = "none";
        }, 5000);
    }
});

// Funkcje używane do reCAPTCHA
function onloadCallback() {
    grecaptcha.render('html_element', {
        'sitekey': clientKey
    });
}
function onSubmit(event) {
    var token = grecaptcha.getResponse();
    document.getElementById("recaptchaTokenInputId").value = token;
}
