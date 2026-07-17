/* ============================================ */
/* slider.js - Hero Slider Custom Functions */
/* ============================================ */

$(document).ready(function () {
    'use strict';

    // Auto-play hero slider
    var heroSlider = $('#heroSlider');
    if (heroSlider.length) {
        // Auto-play with 5-second interval
        heroSlider.carousel({
            interval: 5000,
            wrap: true,
            pause: 'hover'
        });

        // Pause on hover
        heroSlider.on('mouseenter', function () {
            heroSlider.carousel('pause');
        });

        heroSlider.on('mouseleave', function () {
            heroSlider.carousel('cycle');
        });
    }

    // ============================================
    // TESTIMONIALS SLIDER
    // ============================================
    if ($('.testimonials-slider').length) {
        $('.testimonials-slider').slick({
            dots: true,
            arrows: true,
            infinite: true,
            speed: 500,
            slidesToShow: 3,
            slidesToScroll: 1,
            responsive: [
                {
                    breakpoint: 992,
                    settings: {
                        slidesToShow: 2,
                        slidesToScroll: 1
                    }
                },
                {
                    breakpoint: 576,
                    settings: {
                        slidesToShow: 1,
                        slidesToScroll: 1
                    }
                }
            ]
        });
    }
});