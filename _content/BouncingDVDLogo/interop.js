// This is a JavaScript module that is loaded on demand. It can export any number of
// functions, and may import other JavaScript modules if required.

export function getDimensions() {
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