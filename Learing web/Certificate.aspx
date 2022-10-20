<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Certificate.aspx.cs" Inherits="Learing_web.Certificate" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Certificate</title>
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
                                    </div>
                                </div>
                            </div>
                        </div>

                    </div>
                </section>
        <script>


            function createcard() {

                const main = document.querySelector('.main-card');
                const card = document.createElement('div');
                card.className = "row";

                GDATA.forEach((e, i) => {
                    console.log(e.vname)

                    const cardcontent = `
        
    <div class="col-xs-12 col-sm-4">
                        <div class="card" >
                            <a class="img-card" href="img/tp204-certificate-05.jpg">
                            <img src="img/tp204-certificate-05.jpg" />
                          </a>
                            <div class="card-content">
                                <h4 class="card-title text-dark">
                                    ${e.vname}
                                    <a href="img/tp204-certificate-05.jpg> 
                                  </a>
                                </h4>

                            </div>
                            <div class="card-read-more">
                                
                            </div>
                        </div>
                           
                    </div>
`
                    card.innerHTML += cardcontent;
                    main.appendChild(card);
                })


            }



            var GDATA;

            var a = <%=Session["aid"] %>;


            $.ajax({
                type: 'GET',
                url: '/Service3.svc/getalrdy',
                data: "aid=" + a + "",
                dataType: 'json',
                success: (data) => {
                    GDATA = data;


                    createcard()

                },

            });
        </script>
        <div>
        </div>
    </form>
</body>
</html>
