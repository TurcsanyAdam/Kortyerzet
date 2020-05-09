// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

function selectValue() {
    var d = document.getElementById("styleDDL");
    var chosenStyle = d.options[d.selectedIndex].text;
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
function onSearchResultReceived(response) {
    const beers = JSON.parse(response);
    const divEl = document.getElementById('beer-list');
    while (divEl.firstChild) {
        divEl.removeChild(divEl.firstChild);
    }
    for (let i = 0; i < beers.length; i++) {
        const beer = beers[i];
        console.log(beer);
        const divELrow = document.createElement("div");
        divELrow.classList.add("row");

        const divELimg = document.createElement("div");
        divELimg.classList.add("col-xs-2");
        const aELimg = document.createElement("a");
        aELimg.style.display = "inline-block";
        aELimg.classList.add("label");
        var imgEL = document.createElement("img")
        imgEL.src = `${beer.logo}`
        imgEL.height = 150;
        imgEL.weight = 150;
        aELimg.appendChild(imgEL);

        const divELdesc = document.createElement("div");
        divELdesc.classList.add("col-xl-6");
        const pELname = document.createElement("p");
        pELname.textContent = `${beer.name}`;
        const pELbrew = document.createElement("p");
        pELbrew.textContent = `${beer.brewery}`;
        const pELstyle = document.createElement("p");
        pELstyle.textContent = `${beer.style}`;
        const pELdesc = document.createElement("p");
        pELdesc.textContent = `${beer.description}`;
        const divELdescRow = document.createElement("div");
        divELdescRow.classList.add("row");


        const divELdescABV = document.createElement("div");
        divELdescABV.className = "col-xs-4 col-md-3 border bg-light border-left-0";
        divELdescABV.textContent = `${beer.abv}  % ABV`;
        const divELdescIBU = document.createElement("div");
        divELdescIBU.className = "col-xs-4 col-md-3 border bg-light";
        divELdescIBU.textContent = `${beer.ibu} IBU`;
        const divELdescRating = document.createElement("div");
        divELdescRating.className = "col-xs-4 col-md-3 border bg-light";
        divELdescRating.textContent = `Rating: ${beer.rating}`;
        const divELdescTimesRate = document.createElement("div");
        divELdescTimesRate.className = "col-xs-4 col-md-3 border bg-light border-right-0";
        divELdescTimesRate.textContent = `${beer.timesRated} Ratings`;

        divELdescRow.appendChild(divELdescABV);
        divELdescRow.appendChild(divELdescIBU);
        divELdescRow.appendChild(divELdescRating);
        divELdescRow.appendChild(divELdescTimesRate);

        divELdesc.appendChild(pELname);
        divELdesc.appendChild(pELbrew);
        divELdesc.appendChild(pELstyle);
        divELdesc.appendChild(pELdesc);
        divELdesc.appendChild(divELdescRow);





        divELimg.appendChild(aELimg);

        divELrow.appendChild(divELimg);
        divELrow.appendChild(divELdesc);

        divEl.appendChild(divELrow);

    }

}
