function Toastified(toast, toastTimer, closeToastBtn) {
    let countdown;

    const closeToast = () => {
        toast.style.animation = "close 0.3s cubic-bezier(.87,-1,.57,.97) forwards";
        toastTimer.classList.remove("timer-animation");
        clearTimeout(countdown);
    };

    const openToast = (type) => {
        toast.style.animation = "open 0.3s cubic-bezier(.47,.02,.44,2) forwards";
        toastTimer.classList.add("timer-animation");
        clearTimeout(countdown);
        countdown = setTimeout(() => {
            closeToast();
        }, 5000);
    };

    closeToastBtn.addEventListener("click", closeToast);

    // Expose a method to reopen the toast
    return {
        openToast: (type) => openToast(type),
        closeToast: () => closeToast(),
    };
}

let toastInstance;

function initializeToastified() {
    const toast = document.querySelector("#toast");
    const toastTimer = document.querySelector("#timer");
    const closeToastBtn = document.querySelector("#toast-close");

    if (toast && toastTimer && closeToastBtn) {
        toastInstance = Toastified(toast, toastTimer, closeToastBtn);
    } else {
        console.error("Failed to initialize Toastified: Elements not found.");
    }
}

function showNewToast(type) {
    if (toastInstance) {
        toastInstance.openToast(type);
    } else {
        console.error("Toastified is not initialized.");
    }
}
