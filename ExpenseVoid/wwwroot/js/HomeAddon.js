// Mobile Menu Functionality
const menuIcon = document.getElementById("menu-icon");
const navLinks = document.getElementById("nav-links");

// Function to toggle the mobile menu
function toggleMenu() {
    navLinks.classList.toggle("nav-active");
    menuIcon.classList.toggle("toggle");
    document.body.classList.toggle("no-scroll");
}

// Click event for menu icon
menuIcon.addEventListener("click", toggleMenu);

// Keydown event for accessibility (Enter or Space key)
menuIcon.addEventListener("keydown", (e) => {
    if (e.key === "Enter" || e.key === " ") {
        e.preventDefault();
        toggleMenu();
    }
});

// Close menu when a link is clicked
const navLinksItems = document.querySelectorAll(".nav-links li a");
navLinksItems.forEach((link) => {
    link.addEventListener("click", () => {
        navLinks.classList.remove("nav-active");
        menuIcon.classList.remove("toggle");
        document.body.classList.remove("no-scroll");
    });
});
