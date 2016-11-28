

new function main() {

    if (navigator.geolocation) {
        navigator.geolocation.getCurrentPosition(showPosition, showError);
    } else {
        console.log("Geolocation is not supported by this browser.");
    }
}

function showPosition(position) {
    var url = "LocationStatus";

    var param = { Latitude: position.coords.latitude, Longitude: position.coords.longitude };
         
    //console.log(position.coords.latitude, position.coords.longitude)
    $.post(url, param, function (res) {
        $("#locationstatusdiv").html(res);
    })
}

function showError(error) {
    switch (error.code) {
        case error.PERMISSION_DENIED:
            console.warn("User denied the request for Geolocation.")
            break;
        case error.POSITION_UNAVAILABLE:
            console.warn("Location information is unavailable.")
            break;
        case error.TIMEOUT:
            console.warn("The request to get user location timed out.")
            break;
        case error.UNKNOWN_ERROR:
            console.warn("An unknown error occurred.")
            break;
    }
}

