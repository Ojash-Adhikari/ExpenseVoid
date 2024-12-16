window.loadVantaScripts = function () {
    // Load three.js script
    const threeScript = document.createElement("script");
    threeScript.src = "https://cdnjs.cloudflare.com/ajax/libs/three.js/r134/three.min.js";
    threeScript.onload = () => {
        console.log("Three.js loaded");
        loadVantaScript();
    };
    threeScript.onerror = () => {
        console.error("Failed to load Three.js");
    };
    document.head.appendChild(threeScript);

    function loadVantaScript() {
        const vantaScript = document.createElement("script");
        vantaScript.src = "https://cdn.jsdelivr.net/npm/vanta@0.5.24/dist/vanta.globe.min.js";
        vantaScript.onload = () => {
            console.log("Vanta.js loaded");
        };
        vantaScript.onerror = () => {
            console.error("Failed to load Vanta.js");
        };
        document.head.appendChild(vantaScript);
    }
};

window.initializeVanta = function () {
    if (typeof VANTA !== "undefined" && typeof VANTA.GLOBE === "function") {
        VANTA.GLOBE({
            el: "#vanta-bg",
            mouseControls: true,
            touchControls: true,
            gyroControls: false,
            minHeight: 200.0,
            minWidth: 200.0,
            scale: 1.0,
            scaleMobile: 1.0,
        });
        console.log("Vanta.js initialized");
    } else {
        console.error("Vanta.js or Three.js is not ready. Retrying...");
        setTimeout(window.initializeVanta, 100);
    }
};
