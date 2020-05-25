let loginDiv;
let regDiv;

function ShowLoginForm() {
    loginDiv.style.display = "block";
    regDiv.style.display = "none";
}

function ShowRegistrationForm() {
    loginDiv.style.display = "none";
    regDiv.style.display = "block";
}

document.addEventListener('DOMContentLoaded', (event) => {
    loginDiv = document.getElementById("loginCard");
    regDiv = document.getElementById("registrationCard");
    loginDiv.style.display = "block";
    regDiv.style.display = "none";
});
