function Toastified(toast, toastTimer, closeToastBtn) {
    console.log("Toast element:", toast);
    console.log("Timer element:", toastTimer);
    console.log("Close button:", closeToastBtn);

    if (!toast || !toastTimer || !closeToastBtn) {
        console.error("One or more elements are missing.");
        return;
    }

    let countdown;

    const closeToast = () => {
        toast.style.animation = "close 0.3s cubic-bezier(.87,-1,.57,.97) forwards";
        toastTimer.classList.remove("timer-animation");
        clearTimeout(countdown);
    };

    const openToast = (type) => {
        toast.classList = [type];
        toast.style.animation = "open 0.3s cubic-bezier(.47,.02,.44,2) forwards";
        toastTimer.classList.add("timer-animation");
        clearTimeout(countdown);
        countdown = setTimeout(() => {
            closeToast();
        }, 5000);
    };

    closeToastBtn.addEventListener("click", closeToast);
}

function initializeToastified() {
    const toast = document.querySelector("#toast");
    const toastTimer = document.querySelector("#timer");
    const closeToastBtn = document.querySelector("#toast-close");

    if (toast && toastTimer && closeToastBtn) {
        Toastified(toast, toastTimer, closeToastBtn);
    } else {
        console.error("Failed to initialize Toastified: Elements not found.");
    }
}
