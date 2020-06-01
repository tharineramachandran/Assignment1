<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Task1Jquery.aspx.cs" Inherits="Assignment1.WebForm1" %>


<!DOCTYPE html>
<html>
<head>

    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.5.1/jquery.min.js"></script>
    <script>
        $(document).ready(function () {
            const d = new Date();

            var currentMonth = ('0' + (d.getMonth() + 1)).slice(-2)

            var date = d.getFullYear() + "-" + currentMonth + "-" + d.getDate();

            const url = 'https://api.data.gov.sg/v1/environment/24-hour-weather-forecast?date_time=' + date + 'T15%3A28%3A01&date=' + date;
            $.get(url, function (data, status) {
                console.log(data);
                if (data) {

                    data1 = data.items[0].general;    // 100
                    forecast = data1.forecast;
                    console.log(forecast);
                    relative_humidity = data1.relative_humidity;
                    temperature = data1.temperature;
                    period = data.items[0].periods;
                    console.log(period);

                    document.getElementById("forecast").innerHTML = "Forcast  for today " + date +" is " + data1.forecast;


                    var table1 = document.getElementById("table");
                    for (i = 0; i < period.length; i++) {


                        var row = table1.insertRow(1);
                        var cell5 = row.insertCell(0);
                        var cell6 = row.insertCell(1);
                        var cell0 = row.insertCell(2);
                        var cell1 = row.insertCell(3);
                        var cell2 = row.insertCell(4);
                        var cell3 = row.insertCell(5);
                        var cell4 = row.insertCell(6);

                         

                        var start = new Date(period[i].time.start);


                        var end = new Date(period[i].time.end);

                        cell5.innerHTML = start.getHours().toString() + " : 00";
                        cell6.innerHTML = end.getHours().toString() + " : 00";


                        cell0.innerHTML = period[i].regions.east;
                        cell1.innerHTML = period[i].regions.west;
                        cell2.innerHTML = period[i].regions.north;

                        cell3.innerHTML = period[i].regions.south;
                        cell4.innerHTML = period[i].regions.central;


                       

                    }




                    function addtable(dic, type) {
                        var table = document.getElementById("myTable");
                        var row = table.insertRow(1);
                        var cell3 = row.insertCell(0);
                        var cell1 = row.insertCell(1);
                        var cell2 = row.insertCell(2);

                        cell3.innerHTML = type;
                        high = dic['high'];
                        low = dic['low'];
                        cell1.innerHTML = high;
                        cell2.innerHTML = low;
                    }


                    addtable(relative_humidity, "relative humidity")

                    addtable(temperature, "temperature")
                }
            });

        });
    </script>
</head>
<body>
    <h1 id="forecast"></h1>

     <table id="myTable">
        <thead>
            <tr>
                 <td>       </td>
                <td>Hightest</td>
                <td>Lowest</td>
            </tr>
        </thead>
    </table>

    <br>
 <h2>Forecast by region and time</h2>
    <table id="table">
        <thead>
            <tr>
                <td>Start time</td>
                <td>End time</td>
                <td>East</td>
                <td>West</td>
                <td>North</td>
                <td>South</td>
                <td>Central</td>
                 
            </tr>
        </thead>
    </table>
           <style>
#myTable,#table {
  font-family: "Trebuchet MS", Arial, Helvetica, sans-serif;
  border-collapse: collapse;
  width: 100%;
}
 h1{
  font-family: "Trebuchet MS", Arial, Helvetica, sans-serif;
 
}



#myTable td, #myTable th,#table td, #table th {
  border: 1px solid #ddd;
  padding: 8px;
}

#myTable tr:nth-child(even),#table tr:nth-child(even) {background-color: #f2f2f2;}

#myTable tr:hover,#table tr:hover {background-color: #ddd;}

#myTable th , #table th {
  padding-top: 12px;
  padding-bottom: 12px;
  text-align: left;
  background-color: #4CAF50;
  color: white;
}
</style>
</body>
</html>
