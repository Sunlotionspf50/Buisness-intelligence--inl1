let tabledata = document.getElementById('tabledata')

fetch("https://iot20-af-new.azurewebsites.net/api/GetDataFromTable?limit=10&orderby=desc")
.then(res => res.json())
.then(data => {
    for(let row of data){
        console.log(row)
        tabledata.innerHTML += `<tr>
        <td>${row.deviceId}</td>
        <td>${row.epochTime}</td>
        <td>${row.fibonacci}</td>
        <td>${row.lottoVinstChansProcent}</td>
        <td>${row.name}</td>`
    }
})