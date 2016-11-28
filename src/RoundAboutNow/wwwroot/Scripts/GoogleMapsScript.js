var map;

function initMap() {
    //TODO bygg om 
    if (navigator.geolocation) {
        navigator.geolocation.getCurrentPosition(showGoogleMap, showError);
    } else {
        console.log("Geolocation is not supported by this browser.");
    }
}

function showGoogleMap(position) {
    var uluru = { lat: position.coords.latitude, lng: position.coords.longitude };

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
        //H för hållplats, icon för person
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
}

