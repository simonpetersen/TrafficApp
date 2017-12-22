﻿// Write your JavaScript code.

function calcRoute() {
    var startAddress = document.getElementById("startAddressField").value;
    var destinationAddress = document.getElementById("destinationAddressField").value;
    var date = document.getElementById("dateTimeField").value;
    if (date == '') {
        var today = new Date();
        var dd = today.getDate();
        var mm = today.getMonth() + 1; //January is 0!
        var yyyy = today.getFullYear();

        if (dd < 10) {
            dd = '0' + dd
        }

        if (mm < 10) {
            mm = '0' + mm
        }

        today = yyyy + "-" + mm + "-" + dd;
        
        document.getElementById("dateTimeField").value = today;
    }
    if (startAddress == "" || destinationAddress == "") {
        showErrorMessage("You must enter a Start and Destination Address.");
        return;
    }
    else {
        getCoordinatesFromAddress(startAddress, destinationAddress);
        var inputs = document.getElementById("inputs");
        inputs.style.display = "none";
        var loader = document.getElementById("loader");
        loader.style.display = "block";
        var travelInfo = document.getElementById("travelInfo");
        travelInfo.style.display = "block";
    }
}

function newRoute() {
    var travelInfo = document.getElementById("travelInfo");
    travelInfo.style.display = "none";
    var inputs = document.getElementById("inputs");
    inputs.style.display = "block";
    var loader = document.getElementById("loader");
    loader.style.display = "none";
    var map = document.getElementById("mapid");
    map.innerHTML = "";
    map.style.display = "none";
    var travelTime1 = document.getElementById("travelTime");
    var baseDuration1 = document.getElementById("baseDuration");
    var distance1 = document.getElementById("distance");
    travelTime1.innerHTML = "";
    baseDuration1.innerHTML = "";
    distance1.innerHTML = "";

    
}

function getCoordinatesFromAddress(startAddress, destinationAddress) {
    var nominatimBase = 'http://nominatim.openstreetmap.org/search/';
    var urlStart = nominatimBase + startAddress + '?format=xml';
    var urlDestination = nominatimBase + destinationAddress + '?format=xml';
    var xhttp = new XMLHttpRequest();
    xhttp.onreadystatechange = function() {
        if (this.readyState == 4) {
            if (this.status == 200) {
                var startCoordinates = getCoordinatesFromXml(startAddress, this.responseXML);
                var xmlhttp = new XMLHttpRequest();
                xmlhttp.onreadystatechange = function() {               
                    if (this.readyState == 4) {
                        if (this.status == 200) {
                            var destinationCoordinates = getCoordinatesFromXml(destinationAddress, this.responseXML);
                            getRoute(startCoordinates, destinationCoordinates);
                        } else {
                            showErrorMessage(this.responseText);
                        }
                    }
                }
                xmlhttp.open("GET", urlDestination, true);
                xmlhttp.send();
            } else {
                showErrorMessage(this.responseText);
            }
        }
    };
    xhttp.open("GET", urlStart, true);
    xhttp.send();
}

function getCoordinatesFromXml(address, xml) {
    var placeElement = xml.getElementsByTagName("place");
    if (placeElement.length > 0) {
        for (i = 0; i < placeElement.length; i++) {
            if (placeElement[i].getAttribute("osm_type") == 'node') {
                return { latitude: placeElement[i].getAttribute("lat"), longitude: placeElement[i].getAttribute("lon") };
            }
        }
    }

    showErrorMessage(address + ' is not a valid address.');
}

function getRoute(startCoordinates, destinationCoordinates) {
    if (startCoordinates == null || destinationCoordinates == null) {
        return;
    }

    var dateValue = document.getElementById('dateTimeField').value;
    var url = baseUrl + 'route/' + startCoordinates.latitude + '/' + startCoordinates.longitude + '/' + destinationCoordinates.latitude + '/' + destinationCoordinates.longitude + '/' + dateValue + '?apiKey=' + apiKey;
    var xmlhttp = new XMLHttpRequest();
    xmlhttp.onreadystatechange = function() {
        if (this.readyState == 4) {
            var loader = document.getElementById("loader");
            loader.style.display = "none";
            if (this.status == 200) {
                var map = document.getElementById("mapid");
                map.style.display = "block";
                map.style.width = "800px";
                map.style.height = "500px";
                setUpMap(this.responseXML);
                setRouteInfoText(this.responseXML);
            } else {
                showErrorMessage(this.responseText);
            }
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
        showErrorMessage("Couldn't determine route.");
    }
}

function setRouteInfoText(xml) {
    var duration = xml.getElementsByTagName("duration")[0].childNodes[0].nodeValue;
    var baseDuration = xml.getElementsByTagName("baseDuration")[0].childNodes[0].nodeValue;
    var distance = xml.getElementsByTagName("distance")[0].childNodes[0].nodeValue;


    var timeText = document.getElementById("travelTime");
    timeText.innerHTML = formatTimeText(duration);

    var baseTimeText = document.getElementById("baseDuration");
    baseTimeText.innerHTML = formatTimeText(baseDuration);

    var distanceText = document.getElementById("distance");
    distanceText.innerHTML = formatDistanceText(distance);

}

function formatTimeText(duration) {
    var seconds = 0;
    var minutes = 0;
    var hours = 0;
    if (duration > 60) {
        if (duration / 60 > 60) {
            hours = Math.floor(duration/3600);
        }
        minutes = Math.floor((duration - hours * 3600) / 60);
    }
    seconds = duration - hours * 3600 - minutes * 60;
    if (hours < 10)
        hours = '0' + hours;
    if (minutes < 10)
        minutes = '0' + minutes;
    if (seconds < 10)
        seconds = '0' + seconds;

    return hours + ":" + minutes + ":" + seconds;
    
}

function formatDistanceText(distance) {
    var km = 0;
    var m = 0;

    if (distance > 1000) {
        km = Math.floor(distance / 1000);
    }
    m = distance - km * 1000;

    if (km == 0)
        return m + 'm.';

    return km + '.' + m + 'km.';
}

function showErrorMessage(message) {
    document.getElementById("mapid").innerHTML = "<p>Error: " + message + "</p>";
}