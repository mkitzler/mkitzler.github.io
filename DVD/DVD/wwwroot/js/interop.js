window.getDimensions = function () {
    return {
        width: window.innerWidth,
        height: window.innerHeight
    };
};

window.getDVDDimensions = function () {
    let dvd = document.getElementById("dvd");
    console.log(dvd);
    if (dvd) {
        return {
            width: dvd.width,
            height: dvd.height
        };
    }
    else {
        return null;
    }
};