document.addEventListener("scroll", () => {
    const navbar = document.querySelector(".navbar-questinator");
    if (!navbar) return;

    navbar.classList.toggle("scrolled", window.scrollY > 20);
});
