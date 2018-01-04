"use strict"

dawaAutocomplete.dawaAutocomplete(document.getElementById("startAddressField"), {
    select: function (selected) {
        document.getElementById("startAddressField").innerHTML = selected.tekst;
    },
    params: {
        postnr: 4930,
        per_side: 5
    }
});

dawaAutocomplete.dawaAutocomplete(document.getElementById("destinationAddressField"), {
    select: function (selected) {
        document.getElementById("destinationAddressField").innerHTML = selected.tekst;
    },
    params: {
        postnr: 4930,
        per_side: 5
    }
});