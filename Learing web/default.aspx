<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="Learing_web.home" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">

<head runat="server">
    <title>home</title>
    <link href="StyleSheethome.css" rel="stylesheet" />

    
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap@4.6.1/dist/css/bootstrap.min.css" />
    <script src="https://cdn.jsdelivr.net/npm/jquery@3.6.0/dist/jquery.slim.min.js"></script>
    <script src="https://cdn.jsdelivr.net/npm/popper.js@1.16.1/dist/umd/popper.min.js"></script>
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@4.6.1/dist/js/bootstrap.bundle.min.js"></script>

    <script src="file/jquery.min.js"></script>
    <!-- jQuery UI 1.11.4 -->
    <script src="file/jquery-ui.min.js"></script>
    <!-- Resolve conflict in jQuery UI tooltip with Bootstrap tooltip -->



</head>
<body class="p-3 mb-2 bg-secondary text-white">



    <form id="form1" runat="server">
        <div class="main" id="main">
            <div class="bar" id="bar">

                <nav class="navbar navbar-expand-lg  navbar-dark bg-dark">

                    <button class="navbar-toggler" type="button" data-toggle="collapse" data-target="#navbarNavDropdown" aria-controls="navbarNavDropdown" aria-expanded="false" aria-label="Toggle navigation">
                        <span class="navbar-toggler-icon"></span>
                    </button>
                    <div class="collapse navbar-collapse" id="navbarNavDropdown">
                        <ul class="navbar-nav">
                            <li class="nav-item active">
                                <a class="nav-link" href="default.aspx">Home <span class="sr-only">(current)</span></a>
                            </li>

                            <li class="nav-item">

                                <div id="login" runat="server">
                                    <a class="nav-link" href="Login.aspx">Login</a>
                                </div>

                            </li>

                            <li class="nav-item">
                                <div id="signin" runat="server">
                                    <a class="nav-link" href="Signin.aspx">Sign in</a>
                                </div>
                            </li>
                            <li class="nav-item">
                                <div id="catalog" runat="server" visible="false">
                                    <a class="nav-link" href="Catalog.aspx">Catalog</a>
                                </div>
                            </li>
                            <li class="nav-item">
                                <div id="Certificate" runat="server" visible="false">
                                    <a class="nav-link" href="Certificate.aspx">Certificate</a>
                                </div>
                            </li>
                            <li class="nav-item">


                                <div id="logout" runat="server" visible="false">
                                    <asp:LinkButton class="nav-link" ID="LinkButton1" runat="server" OnClick="Blogout_Click">Log out</asp:LinkButton>

                                </div>
                            </li>
                            <li class="nav-item dropdown">


                                <select id="classDrpDwn" class="form-control" onchange="bindClass()" runat="server">
                                    <option value="1" itemid="1">Physics</option>
                                    <option value="2">Chemical</option>
                                    <option value="3">Biology</option>
                                    <option value="4">Calculus</option>
                                    <option value="5">Statistics</option>
                                    <option value="6">Applied mathematics</option>
                                </select>

                            </li>

                            <li class="nav-item">

                                <label class="nav-link" id="sslog" runat="server"></label>
                                <label class="nav-link" id="hidden" runat="server" visible="false"></label>
                            </li>



                        </ul>
                    </div>
                </nav>


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
            </div>
        </div>






    </form>
    <script>

        function bindClass() {
            removeElementsByClass("col-xs-12 col-sm-4")
            m = $('#classDrpDwn').val();
            createcard(m);





        }

        function removeElementsByClass(className) {
            const elements = document.getElementsByClassName(className);
            while (elements.length > 0) {
                elements[0].parentNode.removeChild(elements[0]);
            }
        }

        function alertm() {
            alert("member type incorrect!");
        }
        function createcard(m) {

            const main = document.querySelector('.main-card');
            const card = document.createElement('div');
            card.className = "row";

            GDATA.forEach((e, i) => {
                if (m != e.mid) {
                    return
                }

                if (e.mid == "1") {
                    img = "img/phy.png";
                } else if (e.mid == "2") {
                    img = "img/Chemical.png";
                } else if (e.mid == "3") {
                    img = "img/Biology.jfif";
                } else if (e.mid == "4") {
                    img = "img/Calculus.png";
                } else if (e.mid == "5") {
                    img = "img/Stt.png";
                } else if (e.mid == "6") {
                    img = "img/Applied mathematics.jfif";
                }
                if ('<%= Session["uname"] %>' != "") {

                    link = "watch.aspx?vid=" + e.vid + "&vname=" + e.vname + "&vlink=" + e.vlink + "&vdes=" + e.description;

                } else {
                    link = "Login.aspx"
                }

                const cardcontent = `
        
    <div class="col-xs-12 col-sm-4">
                        <div class="card" id="mid-${e.mid}">
                            <a class="img-card" href="${link}">
                            <img src="${img}" />
                          </a>
                            <div class="card-content">
                                <h4 class="card-title">
                                    <a href="${link}"> ${e.vname}
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
        var tmp = '<%= Session["mid"] %>'
        console.log(tmp);
        $(document).ready(function () {
            let m = $('#classDrpDwn').val();



            $.ajax({
                type: 'GET',
                url: '/service1.svc/getscreendata',
                dataType: 'json',
                success: (data) => {
                    GDATA = data;


                    createcard(m);

                },

            });
        });
    </script>
</body>


</html>
