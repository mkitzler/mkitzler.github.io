window.getDimensions = function () {
    return {
        width: window.innerWidth,
        height: window.innerHeight
    };
};

window.getDVDDimensions = function () {
    let dvd = document.getElementById("dvd");
    if (dvd) {
        return {
            width: dvd.clientWidth,
            height: dvd.clientHeight
        };
    }
    else {
        return null;
    }
};