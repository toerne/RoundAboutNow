var map;

function initMap() {
    if (navigator.geolocation) {
        navigator.geolocation.getCurrentPosition(showGoogleMap, showError);
    } else {
        console.log("Geolocation is not supported by this browser.");
    }
}

function showGoogleMap(position) {
    var uluru = { lat: position.coords.latitude, lng: position.coords.longitude };
    var geocoder = new google.maps.Geocoder;
    geocodeLatLng(geocoder, uluru);

    map = new google.maps.Map(document.getElementById('googlemap'), {
        zoom: 14,
        center: uluru
    });



    var imageUrl = "http://localhost:22416/images/youarehere.png";

    var image = {
        url: imageUrl,
        size: new google.maps.Size(32, 32),
        origin: new google.maps.Point(0, 0),
        anchor: new google.maps.Point(16, 32)
    };

    var marker = new google.maps.Marker({
        position: uluru,
        map: map,
        title: "Nuvarande position",
        animation: google.maps.Animation.DROP,
        icon: image
    });

    new google.maps.Circle({
        strokeColor: '#14e9fc',
        strokeOpacity: 0.8,
        strokeWeight: 2,
        fillColor: '#14e9fc',
        fillOpacity: 0.20,
        map: map,
        center: uluru,
        radius: 500
    });

    addreziselistner();

    function geocodeLatLng(geocoder, uluru) {

        geocoder.geocode({ 'location': uluru }, function (results, status) {
            if (status === 'OK') {
                var areaName;
                for (var i = 0; i < results.length; i++) {
                    areaName = results[i].formatted_address;
                    var breakloop = false;
                    for (var j = 0; j < results[i].types.length; j++) {
                        if (results[i].types[j] === "neighborhood" || results[i].types[j] === "sublocality") {
                            breakloop = true;
                            break;
                        }
                    }
                    if (breakloop) {
                        $("#areaTitle").text("Status för " + areaName.split(",")[0]);
                        break;
                    }
                }
            } else {
                $("#areaTitle").text("Kunde ej hitta område");
            }
        });
    }
}


function addreziselistner() {
    //google.maps.event.addDomListener(window, 'load', initialize);
    google.maps.event.addDomListener(window, "resize", function () {
        var center = map.getCenter();
        google.maps.event.trigger(map, "resize");
        map.setCenter(center);
    });

};

