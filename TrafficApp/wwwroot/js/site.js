// Write your JavaScript code.

function calcRoute() {
    var startAddress = document.getElementById("startAddressField").value;
    var destinationAddress = document.getElementById("destinationAddressField").value;
    getCoordinatesFromAddress(startAddress, destinationAddress);
}

function getCoordinatesFromAddress(startAddress, destinationAddress) {
    var nominatimBase = 'http://nominatim.openstreetmap.org/search/';
    var urlStart = nominatimBase + startAddress + '?format=xml';
    var urlDestination = nominatimBase + destinationAddress + '?format=xml';
    var xhttp = new XMLHttpRequest();
    xhttp.onreadystatechange = function() {
        if (this.readyState == 4 && this.status == 200) {
            var startCoordinates = getCoordinatesFromXml(this.responseXML);
            var xmlhttp = new XMLHttpRequest();
            xmlhttp.onreadystatechange = function() {               
                if (this.readyState == 4 && this.status == 200) {
                    var destinationCoordinates = getCoordinatesFromXml(this.responseXML);
                    getRoute(startCoordinates, destinationCoordinates);
                }
            }
            xmlhttp.open("GET", urlDestination, true);
            xmlhttp.send();
        }
    };
    xhttp.open("GET", urlStart, true);
    xhttp.send();
}

function getCoordinatesFromXml(xml) {
    var placeElement = xml.getElementsByTagName("place");
    if (placeElement.length > 0) {
        for (i = 0; i < placeElement.length; i++) {
            if (placeElement[i].getAttribute("osm_type") == 'node') {
                return { latitude: placeElement[i].getAttribute("lat"), longitude: placeElement[i].getAttribute("lon") };
            }
        }
    }
}

function getRoute(startCoordinates, destinationCoordinates) {
    var dateValue = document.getElementById('dateTimeField').value;
    var url = baseUrl + 'route/' + startCoordinates.latitude + '/' + startCoordinates.longitude + '/' + destinationCoordinates.latitude + '/' + destinationCoordinates.longitude + '/' + dateValue + '?apiKey=' + apiKey;
    var xmlhttp = new XMLHttpRequest();
    xmlhttp.onreadystatechange = function() {
        if (this.readyState == 4 && this.status == 200) {
            setUpMap(this.responseXML);
        }
    }
    xmlhttp.open("GET", url, true);
    xmlhttp.send();
}

function setUpMap(xml) {
    var nodeElements = xml.getElementsByTagName("node");
    if (nodeElements.length > 0) {
        var coordinates = [];
        for (i = 0; i < nodeElements.length; i++) {
            var latitude = nodeElements[i].getElementsByTagName("latitude")[0].childNodes[0].nodeValue;
            var longitude = nodeElements[i].getElementsByTagName("longitude")[0].childNodes[0].nodeValue;

            coordinates.push([latitude, longitude]);
        }

        var mymap = L.map('mapid').setView(coordinates[0], 13);

        L.tileLayer('https://api.tiles.mapbox.com/v4/{id}/{z}/{x}/{y}.png?access_token={accessToken}', {
            attribution: 'Map data &copy; <a href="http://openstreetmap.org">OpenStreetMap</a> contributors, <a href="http://creativecommons.org/licenses/by-sa/2.0/">CC-BY-SA</a>, Imagery © <a href="http://mapbox.com">Mapbox</a>',
            maxZoom: 18,
            id: 'mapbox.streets',
            accessToken: 'pk.eyJ1Ijoic2ltb25wZXRlcnNlbiIsImEiOiJjamFjdnllbDkxaTVqMndxZTU1c3NxaXZyIn0.SR6Kp-S_MnhPmayhHvX-Og'
        }).addTo(mymap);

        L.polyline(coordinates, {color: 'red'}).addTo(mymap);
    } else {
        document.getElementById("mapid").innerHTML = "<p>Error: Couldn't determine route.</p>";
    }
}