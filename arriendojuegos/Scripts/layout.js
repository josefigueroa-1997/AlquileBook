// Cerrar submenús al hacer clic en cualquier lugar fuera del menú
document.addEventListener('click', function (event) {
    var nav = document.querySelector('nav');
    var isClickInsideNav = nav.contains(event.target);
    if (!isClickInsideNav) {
        document.querySelectorAll('nav a:not(.expander)').forEach(function (link) {
            link.classList.remove('expanded');
        });
    }
});
document.querySelectorAll('nav a').forEach(function (link) {
    link.addEventListener('mouseover', function () {
        this.parentElement.classList.add('expanded');
    });

    link.addEventListener('mouseout', function () {
        this.parentElement.classList.remove('expanded');
    });
});
let currentSlide = 0;

/*function nextSlide() {
    const banner = document.getElementById('banner');
    const totalSlides = banner.children.length;
    const slideWidth = banner.clientWidth / totalSlides;

    if (currentSlide < totalSlides - 1) {
        currentSlide++;
    } else {
        currentSlide = 0;
    }

    updateSlide(slideWidth);
}

function prevSlide() {
    const banner = document.getElementById('banner');
    const totalSlides = banner.children.length;
    const slideWidth = banner.clientWidth / totalSlides;

    if (currentSlide > 0) {
        currentSlide--;
    } else {
        currentSlide = totalSlides - 1;
    }

    updateSlide(slideWidth);
}

function updateSlide(slideWidth) {
    const banner = document.getElementById('banner');

    banner.style.transform = `translateX(-${currentSlide * slideWidth}px)`;
}

// Cambiar la imagen automáticamente cada 3 segundos
setInterval(() => {
    nextSlide();
}, 3000);
*/