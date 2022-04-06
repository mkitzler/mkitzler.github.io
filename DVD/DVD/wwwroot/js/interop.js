﻿export function getDimensions() {
    let dvd = document.getElementById("dvd");
    return {
        Item1: {
            width: window.innerWidth,
            height: window.innerHeight
        },
        Item2: dvd ? {
            width: dvd.clientWidth,
            height: dvd.clientHeight
        } : null
    };
};