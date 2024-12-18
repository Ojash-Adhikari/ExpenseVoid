function Toastified() {
    const toast = document.querySelector("#toast");
const toastTimer = document.querySelector("#timer");
const closeToastBtn = document.querySelector("#toast-close");
let countdown

const closeToast = () => {
    toast.style.animation = "close 0.3s cubic-bezier(.87,-1,.57,.97) forwards";
    toastTimer.classList.remove("timer-animation");
    clearTimeout(countdown)
}

const openToast = (type) => {
    toast.classList = [type];
    toast.style.animation = "open 0.3s cubic-bezier(.47,.02,.44,2) forwards";
    toastTimer.classList.add("timer-animation");
    clearTimeout(countdown)
    countdown = setTimeout(() => {
        closeToast();
    }, 5000)
}

    closeToastBtn.addEventListener("click", closeToast)
    document.addEventListener("DOMContentLoaded", function () {
        initializeToasts();
}
