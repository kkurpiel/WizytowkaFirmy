﻿// Funkcja ukrywająca alert o wysłaniu wiadomości e-mail po 5 sekundach
document.addEventListener("DOMContentLoaded", function () {
    var alertElement = document.getElementById("wyslanoEmailAlert");
    if (alertElement) {
        setTimeout(function () {
            alertElement.style.display = "none";
        }, 5000);
    }
});
// Funkcja ukrywająca alert o wystawieniu oceny po 5 sekundach
document.addEventListener("DOMContentLoaded", function () {
    var alertElement = document.getElementById("wystawionoOpinieAlert");
    if (alertElement) {
        setTimeout(function () {
            alertElement.style.display = "none";
        }, 5000);
    }
});

// Funkcje używane do reCAPTCHA
function onloadCallback() {
    grecaptcha.render('html_element', {
        'sitekey': siteKey
    });
}
function onSubmit(event) {
    var token = grecaptcha.getResponse();
    document.getElementById("recaptchaTokenInputId").value = token;
}

// gwiadki
document.addEventListener("DOMContentLoaded", function () {
    const stars = document.querySelectorAll("#rating-stars span");
    const hiddenInput = document.querySelector("#Ocena");
    const ratingError = document.querySelector("#rating-error");

    let selectedRating = 5; // Ustawiamy domyślną ocenę na 5

    // Podświetl wszystkie gwiazdki na żółto przy załadowaniu strony
    highlightStars(selectedRating - 1);

    stars.forEach((star, index) => {
        // Podświetlenie gwiazdek podczas najechania myszką
        star.addEventListener("mouseover", () => {
            highlightStars(index);
        });

        // Usunięcie podświetlenia po opuszczeniu myszki
        star.addEventListener("mouseout", () => {
            highlightStars(selectedRating - 1);
        });

        // Kliknięcie gwiazdki wybiera ocenę
        star.addEventListener("click", () => {
            selectedRating = index + 1;
            hiddenInput.value = selectedRating; // Zapisz wartość w ukrytym polu
            highlightStars(selectedRating - 1);
            ratingError.textContent = ""; // Usuwa komunikat o błędzie
        });
    });

    function highlightStars(index) {
        stars.forEach((star, i) => {
            if (i <= index) {
                star.classList.add("selected");
            } else {
                star.classList.remove("selected");
            }
        });
    }
});





