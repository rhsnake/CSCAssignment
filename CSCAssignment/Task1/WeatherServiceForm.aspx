 <%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WeatherServiceForm.aspx.cs" Inherits="CSCAssignment.Task1.WeatherServiceForm" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Task 1</title>
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1, shrink-to-fit=no" />
    <link rel="stylesheet" href="../Content/bootstrap.min.css" />
    <style>
        #content {
            display:flex;
            justify-content:center;
            align-items: center;
            flex-direction:column;
        }

        .card {
            margin: 1em;
            max-width: 75%;
        }
    </style>
</head>
<body>
    <section id="content">
        <h2>Task 1 JSON Data Responses</h2>
        <div class="card">
          <div class="card-body">
            <h5 class="card-title">From C#</h5>
            <p class="card-text" id="CSharpJSONResponse" runat="server"></p>
          </div>
        </div>
        <div class="card">
          <div class="card-body">
            <h5 class="card-title">From jQuery</h5>
            <p class="card-text" id="jq-response"></p>
          </div>
        </div>
    </section>
</body>
<script src="../Scripts/jquery-3.5.1.min.js"></script>
<script src="../Scripts/bootstrap.min.js"></script>
<script>
    $(() => {
        const $responseBox = $("#jq-response")
        $.ajax({
            url: "https://api.data.gov.sg/v1/environment/air-temperature",
            method: "GET",
        }).done((data) => {
            console.log(data);
            $responseBox.text(JSON.stringify(data));
        }).fail(() => {
            $responseBox.text("Failed to fetch data using jQuery");
        });
    })
</script>
</html>
