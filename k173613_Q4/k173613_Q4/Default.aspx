<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="WebApplication2.Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>PSX Web Search</title>

    <!-- Latest compiled and minified CSS -->
    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/css/bootstrap.min.css" integrity="sha384-BVYiiSIFeK1dGmJRAkycuHAHRg32OmUcww7on3RYdg4Va+PmSTsz/K68vbdEjh4u" crossorigin="anonymous">
    <!-- Optional theme -->
    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/css/bootstrap-theme.min.css" integrity="sha384-rHyoN1iRsVXV4nD0JutlnGaslCJuC7uwjduW9SVrLvRYooPp2bWYgmgJQIXwl/Sp" crossorigin="anonymous">
    <!-- Latest compiled and minified JavaScript -->
    <script src="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/js/bootstrap.min.js" integrity="sha384-Tc5IQib027qvyjSMfHjOMaLkfuWVxZxUPnCJA7l2mCWNIpG9mGCD8wGNIcPD7Txa" crossorigin="anonymous"></script>
    <!-- Jquery CDN Link-->
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.1.1/jquery.min.js"></script>
</head>
    
<body style="background-color:#3EAF86">
    <script type="text/javascript">

        // Load Data
        function Searching() {
            // Onload Load Categories to Options
            $.ajax({
                type: "POST",
                url: 'Default.aspx/LoadCategory',
                //data: "{CategoryNames:'" + Category + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                cache: false,
                async: true,
                success: function (data) {
                    $.each(data.d, function (key, value) {
                        $("#categorySelected").append('<option value=' + key + '>' + value + '</option>');
                    });
                },
                error: function (x, e) {
                    //in case of any error , it will come in this function;
                    alert('Error');
                }
            });

            // Set Table Data
            $.ajax({
                type: "POST",
                url: 'Default.aspx/GetAllData',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                cache: false,
                async: true,
                success: function (data) {
                    dataFetched = JSON.stringify(data.d);
                    var response = JSON.parse(dataFetched);
                    for (var i = 0; i < response.Item1.length; i++) {
                        $('#myTable tbody').append("<tr><td>" + response.Item1[i] + "</td><td>" + response.Item2[i] + "</td><td>" + response.Item3[i] + "</td></tr>");
                    }
                },
                error: function (x, e) {
                    //in case of any error , it will come in this function;
                    alert('Error');
                }
            });
        }

        $(document).ready(function () {
            // Load All Data on load
            Searching();

            // Load Data on Refresh BTN Click
            $("#btnRefresh").click(function () {
                Searching();
                var $rowCount = $('#myTable tr').length;
            });

            // Table Search by Category
            $("#btnSearch").click(function () {
                $.each($("#myTable tbody tr"), function () {
                    if ($(this).text().toLowerCase().indexOf($('#categorySelected').find(":selected").text().toLowerCase()) === -1) {
                        $(this).hide();
                    }
                    else {
                        $(this).show();
                    }
                    var rowCount = $('#myTable tr').length;
                    $("#tableRows").text = rowCount;
                });
            }); 
        });
    </script>

    <form id="form2" runat="server">
        <div class="container-fluid">
            <div class="row">
                <h1 class="display-3 text-center" style="color:aliceblue;">PSX Web Search</h1>
            </div>
            <div class="row">
                <div class="col-lg-12">
                    <table class="table table-bordered" style="margin: 10px 5px; padding: 1px 5px;">
                        <tbody>
                            <tr>
                                <td>
                                    <select id="categorySelected" class="form-control">
                                        <!-- Category Option on Runtime -->
                                        <option>...</option>
                                    </select>
                                </td>
                                <td>
                                    <input class="btn btn-primary btn-block" type="button" id="btnSearch" value="Search" />
                                </td>
                                <td>
                                    <input class="btn btn-success btn-block" type="button" id="btnRefresh" value="Refresh" /> 
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </div>
            </div>

            <div class="row">
                <div class="col-lg-12">
                    <h5 class="text-center" style="color:aliceblue;">About <span id="tableRows"></span> results ( )</h5>
                </div>
            </div>

            <div class="row">
                <div class="col-lg-12">
                    <table id="myTable" class="table table-bordered table-hover" style="margin: 10px 5px; padding: 1px 5px;">
                        <thead>
                            <tr style="background-color:black; color:aqua">
                                <th  class="text-center">Category</th>
                                <th  class="text-center">Scrip</th>
                                <th  class="text-center">Price</th>
                            </tr>
                        </thead>

                        <tbody>
                            <!-- Table Data on Runtime -->
                        </tbody>
                    </table>
                </div>
            </div>
        </div>
    </form>
</body>
</html>
