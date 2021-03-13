let tabledata = document.getElementById('tabledata')

fetch("https://iot20-af-new.azurewebsites.net/api/GetFromCosmos?limit=10&orderby=desc")
.then(res => res.json())
.then(data => {
    for(let row of data){
        tabledata.innerHTML += `<tr><td>${row.deviceId}</td><td>${row.epochtime}</td><td>${row.temperature}</td><td>${row.humidity}</td><td>${row.name}</td>`
    }
})