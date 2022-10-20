<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Catalog.aspx.cs" Inherits="Learing_web.Catalog" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Catalog</title>
    <link href="StyleSheethome.css" rel="stylesheet" />
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap@4.6.1/dist/css/bootstrap.min.css" />
    <script src="https://cdn.jsdelivr.net/npm/jquery@3.6.0/dist/jquery.slim.min.js"></script>
    <script src="https://cdn.jsdelivr.net/npm/popper.js@1.16.1/dist/umd/popper.min.js"></script>
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@4.6.1/dist/js/bootstrap.bundle.min.js"></script>

    <script src="file/jquery.min.js"></script>
    <!-- jQuery UI 1.11.4 -->
    <script src="file/jquery-ui.min.js"></script>
</head>
<body class="p-3 mb-2 bg-secondary text-white">
    <nav class="navbar navbar-expand-lg navbar-dark bg-dark">

        <button class="navbar-toggler" type="button" data-toggle="collapse" data-target="#navbarNavDropdown" aria-controls="navbarNavDropdown" aria-expanded="false" aria-label="Toggle navigation">
            <span class="navbar-toggler-icon"></span>
        </button>
        <div class="collapse navbar-collapse" id="navbarNavDropdown">
            <ul class="navbar-nav">
                <li class="nav-item active">
                    <a class="nav-link" href="default.aspx">Home <span class="sr-only">(current)</span></a>
                </li>


            </ul>
        </div>
    </nav>
    <form id="form1" runat="server">
        
        <section class="wrapper">
                    <div class="container-fostrap">

                        <div class="content">
                            <div class="container">
                                <div class="main-card">
                                    <div class="row">
                                        <table class="table table-dark" id="catalog"  >
                <thead>
                    <tr>
                        <th scope="col">#</th>
                        <th scope="col">Video name</th>
                        <th scope="col">Ontime</th>
                        
                    </tr>
                </thead>

                       
                <tbody>

                    
                </tbody>
                                            
                            
            </table>
                                    </div>
                                </div>
                            </div>
                        </div>
                        
                    </div>
                </section>
           
            
        
        <script>
            function createtable() {
                var m = document.getElementById("catalog");
                
                GDATA.forEach((e, i) => {
                    
                    
                    var row = m.insertRow();
                    var cell1 = row.insertCell(0);
                    var cell2 = row.insertCell(1);
                    var cell3 = row.insertCell(2);
                    cell1.innerHTML = i;
                    cell2.innerHTML = e.vname;
                    cell3.innerHTML = e.ontime;
                })
                }
            var GDATA;
           
                var a = <%=Session["aid"] %>;

            
                $.ajax({
                    type: 'GET',
                    url: '/service2.svc/Submit_Click',
                    data:"aid="+a+"",
                    dataType: 'json',
                    success: (data) => {
                        GDATA = data;

                        
                        createtable()

                    },

                });
        </script>
    </form>
</body>
</html>
