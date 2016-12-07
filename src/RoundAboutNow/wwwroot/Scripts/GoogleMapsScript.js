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
    map.setOptions({
        draggable: false,
        scrollwheel: false,
        disableDoubleClickZoom: true
    });

    var styledMapType = new google.maps.StyledMapType(
        [
  {
      "elementType": "geometry",
      "stylers": [
        {
            "color": "#f5f5f5"
        }
      ]
  },
  {
      "elementType": "labels.icon",
      "stylers": [
        {
            "visibility": "off"
        }
      ]
  },
  {
      "elementType": "labels.text.fill",
      "stylers": [
        {
            "color": "#616161"
        }
      ]
  },
  {
      "elementType": "labels.text.stroke",
      "stylers": [
        {
            "color": "#f5f5f5"
        }
      ]
  },
  {
      "featureType": "administrative.land_parcel",
      "elementType": "labels",
      "stylers": [
        {
            "visibility": "off"
        }
      ]
  },
  {
      "featureType": "administrative.land_parcel",
      "elementType": "labels.text.fill",
      "stylers": [
        {
            "color": "#bdbdbd"
        }
      ]
  },
  {
      "featureType": "poi",
      "elementType": "geometry",
      "stylers": [
        {
            "color": "#eeeeee"
        }
      ]
  },
  {
      "featureType": "poi",
      "elementType": "labels.text",
      "stylers": [
        {
            "visibility": "off"
        }
      ]
  },
  {
      "featureType": "poi",
      "elementType": "labels.text.fill",
      "stylers": [
        {
            "color": "#757575"
        }
      ]
  },
  {
      "featureType": "poi.business",
      "stylers": [
        {
            "visibility": "off"
        }
      ]
  },
  {
      "featureType": "poi.park",
      "elementType": "geometry",
      "stylers": [
        {
            "color": "#e5e5e5"
        }
      ]
  },
  {
      "featureType": "poi.park",
      "elementType": "labels.text",
      "stylers": [
        {
            "visibility": "off"
        }
      ]
  },
  {
      "featureType": "poi.park",
      "elementType": "labels.text.fill",
      "stylers": [
        {
            "color": "#9e9e9e"
        }
      ]
  },
  {
      "featureType": "road",
      "elementType": "geometry",
      "stylers": [
        {
            "color": "#ffffff"
        }
      ]
  },
  {
      "featureType": "road.arterial",
      "elementType": "labels.text.fill",
      "stylers": [
        {
            "color": "#757575"
        }
      ]
  },
  {
      "featureType": "road.highway",
      "elementType": "geometry",
      "stylers": [
        {
            "color": "#dadada"
        }
      ]
  },
  {
      "featureType": "road.highway",
      "elementType": "labels.text.fill",
      "stylers": [
        {
            "color": "#616161"
        }
      ]
  },
  {
      "featureType": "road.local",
      "elementType": "labels",
      "stylers": [
        {
            "visibility": "off"
        }
      ]
  },
  {
      "featureType": "road.local",
      "elementType": "labels.text.fill",
      "stylers": [
        {
            "color": "#9e9e9e"
        }
      ]
  },
  {
      "featureType": "transit.line",
      "elementType": "geometry",
      "stylers": [
        {
            "color": "#e5e5e5"
        }
      ]
  },
  {
      "featureType": "transit.station",
      "elementType": "geometry",
      "stylers": [
        {
            "color": "#eeeeee"
        }
      ]
  },
  {
      "featureType": "water",
      "elementType": "geometry",
      "stylers": [
        {
            "color": "#c9c9c9"
        }
      ]
  },
  {
      "featureType": "water",
      "elementType": "labels.text.fill",
      "stylers": [
        {
            "color": "#9e9e9e"
        }
      ]
  }
        ],
        {name: 'Styled Map'});

    var imageUrl = "/images/youarehere.png";

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
        strokeColor: '#bdb7bf',
        strokeOpacity: 0.8,
        strokeWeight: 2,
        fillColor: '#bdb7bf',
        fillOpacity: 0.20,
        map: map,
        center: uluru,
        radius: 500
    });

    addreziselistner();

    //visa trafikering
    var trafficLayer = new google.maps.TrafficLayer();
    trafficLayer.setMap(map);

    map.mapTypes.set('styled_map', styledMapType);
    map.setMapTypeId('styled_map');

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
                        $("#areaTitle").text("Lokaltrafikinformation för " + areaName.split(",")[0]);
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
    google.maps.event.addDomListener(window, "resize", function () {
        var center = map.getCenter();
        google.maps.event.trigger(map, "resize");
        map.setCenter(center);
    });
};

