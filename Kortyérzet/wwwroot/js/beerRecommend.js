
function selectBeers(chosenStyle, breweryID, beerID) {
    console.log(chosenStyle);
    console.log(breweryID);
    console.log(beerID);
    var xhr = new XMLHttpRequest();
    xhr.open('POST', '/Home/Recommendation', true);
    xhr.setRequestHeader('Content-Type', 'application/x-www-form-urlencoded; charset=utf-8');
    xhr.onreadystatechange = function () {
        if (xhr.readyState == 4 && xhr.status == 200) {
            onRecommendationReceived(xhr.responseText);
        }
    }
    xhr.send(`chosenStyle=${chosenStyle}&breweryID=${breweryID}&beerID=${beerID}`);

}

function onRecommendationReceived(response) {
    const beers = JSON.parse(response);
    const divEl = document.getElementById('beer-recommendation');
    while (divEl.firstChild) {
        divEl.removeChild(divEl.firstChild);
    }
    console.log(beers)
    for (let i = 0; i < beers.length; i++) {
        const beer = beers[i];
        console.log(beer);
        const divELcard = document.createElement("div");
        divELcard.classList.add("card");
        divELcard.style.width = "18rem";

        const divELcardbody = document.createElement("div");
        divELcardbody.classList.add("card-body");

        const aELbtn = document.createElement("a");
        aELbtn.classList.add("btn", "btn-primary", "btn-block");
        aELbtn.href = `/Home/BeerDetails/${beer.id}`
        aELbtn.innerText = `${beer.name}`

        const pELtxt = document.createElement("p");
        pELtxt.classList.add("card-text")

        const imgEL = document.createElement("img");
        imgEL.style.float = "left"
        imgEL.style.display = "inline-block"
        if ((beer.logo).startsWith("http")) {
            imgEL.src = `${beer.logo}`
        }
        else {
            imgEL.src = `/uploads/${beer.logo}`
        }

        imgEL.height = 50;
        imgEL.weight = 50;

        divELcardbody.appendChild(aELbtn);
        pELtxt.appendChild(imgEL);
        pELtxt.innerHTML += `${beer.brewery.name}`
        divELcardbody.appendChild(pELtxt);
        divELcardbody.innerHTML += `${beer.style} - Rating: ${beer.rating}`

        divELcard.appendChild(divELcardbody);

        divEl.appendChild(divELcard);
    }
}

function newCheckIn() {
    var newText = document.getElementById("checkInText");
    var checkInText = newText.value;

    var newRating = document.getElementById("checkInRating");
    var checkInRating = newRating.innerHTML;

    console.log(checkInRating);
    console.log(checkInText);

    var xhr = new XMLHttpRequest();

    xhr.open('POST', '/Home/Style', true);
    xhr.setRequestHeader('Content-Type', 'application/x-www-form-urlencoded; charset=utf-8');
    xhr.onreadystatechange = function () {
        if (xhr.readyState == 4 && xhr.status == 200) {
            onSearchResultReceived(xhr.responseText);
        }
    }
    xhr.send(`chosenStyle=${chosenStyle}`);

}