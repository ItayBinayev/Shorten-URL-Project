const options = {
    method: 'GET',
    headers: {
        'X-RapidAPI-Key': '689c4fe7f1msh9486a2c7271cbb0p17e3fbjsn4416960ba2f1',
        'X-RapidAPI-Host': 'coingecko.p.rapidapi.com'
    }

}
fetch('https://api.coingecko.com/api/v3/simple/price?ids=bitcoin&vs_currencies=usd&include_24hr_change=true', options)
    .then(response => response.json())
    .then(response => {
        let btcpr = document.getElementById("btcprice")
        let price = 0
        let change24 = 0
        console.log(response)
        price = response.bitcoin.usd;
        change24 = response.bitcoin.usd_24h_change;
        btcpr.innerHTML = price + '$ ';
        let i = document.createElement("i")
        btcpr.appendChild(i)
        if (change24 >= 0) {
            btcpr.classList.add("green")
            i.classList.remove("fa-arrow-down")
            i.classList.add("fa-solid", "fa-arrow-up")
        }
        else {
            btcpr.classList.add("text-danger")
            i.classList.remove("fa-arrow-up")
            i.classList.add("fa-solid", "fa-arrow-down")
        }

    }
    )
