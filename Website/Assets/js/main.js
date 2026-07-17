/* ============================================ */
/* main.js - Main JavaScript */
/* Restaurant Website - Premium Design */
/* ============================================ */

$(document).ready(function () {
    'use strict';

    // ============================================
    // NAVBAR SCROLL EFFECT
    // ============================================
    $(window).scroll(function () {
        var scroll = $(window).scrollTop();
        if (scroll > 50) {
            $('.navbar').addClass('scrolled');
        } else {
            $('.navbar').removeClass('scrolled');
        }
    });

    // ============================================
    // BACK TO TOP BUTTON
    // ============================================
    $(window).scroll(function () {
        var scroll = $(window).scrollTop();
        if (scroll > 300) {
            $('#backToTop').addClass('visible');
        } else {
            $('#backToTop').removeClass('visible');
        }
    });

    $('#backToTop').click(function () {
        $('html, body').animate({
            scrollTop: 0
        }, 800);
    });

    // ============================================
    // SMOOTH SCROLL FOR ANCHOR LINKS
    // ============================================
    $('a[href^="#"]').on('click', function (e) {
        var target = $(this.getAttribute('href'));
        if (target.length) {
            e.preventDefault();
            $('html, body').animate({
                scrollTop: target.offset().top - 80
            }, 800);
        }
    });

    // ============================================
    // ACTIVE NAV LINK ON SCROLL
    // ============================================
    $(window).scroll(function () {
        var scrollPos = $(window).scrollTop();
        var navHeight = $('.navbar').outerHeight();

        $('.nav-link').each(function () {
            var target = $($(this).attr('href'));
            if (target.length) {
                var targetOffset = target.offset().top - navHeight - 20;
                if (scrollPos >= targetOffset) {
                    $('.nav-link').removeClass('active');
                    $(this).addClass('active');
                }
            }
        });
    });

    // ============================================
    // TOOLTIP INITIALIZATION
    // ============================================
    $('[data-bs-toggle="tooltip"]').tooltip();

    // ============================================
    // COUNTER ANIMATION
    // ============================================
    function animateCounter(element, target) {
        var current = 0;
        var increment = target / 80;
        var duration = 2000;
        var steps = Math.ceil(duration / 16);
        var step = 0;

        var interval = setInterval(function () {
            step++;
            current += increment;
            if (step >= steps) {
                current = target;
                clearInterval(interval);
            }
            $(element).text(Math.round(current));
        }, 16);
    }

    // ============================================
    // PRELOADER
    // ============================================
    $(window).on('load', function () {
        $('#preloader').fadeOut('slow');
    });

    // ============================================
    // LAZY LOADING FOR IMAGES
    // ============================================
    $('img[data-src]').each(function () {
        $(this).attr('src', $(this).data('src'));
        $(this).removeAttr('data-src');
    });

    console.log('Restaurant Website loaded successfully!');
});