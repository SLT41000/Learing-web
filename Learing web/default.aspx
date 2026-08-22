<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="Learing_web.home" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Learing Web - Home</title>
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap@4.6.1/dist/css/bootstrap.min.css" />
    <link href="StyleSheethome.css" rel="stylesheet" />
    <link href="navbar.css" rel="stylesheet" />
    <style>
        .loading-spinner {
            text-align: center;
            padding: 40px 0;
            color: #6c757d;
        }
        .loading-spinner .spinner-border {
            width: 3rem;
            height: 3rem;
            color: #0d6efd;
        }
        .subject-select {
            max-width: 300px;
        }
        .card img {
            transition: transform 0.3s ease;
        }
        .card:hover img {
            transform: scale(1.05);
        }
    </style>
</head>
<body class="p-3 mb-2 bg-secondary text-white">
    <form id="form1" runat="server">
        <nav class="navbar navbar-expand-lg navbar-dark bg-dark">
            <button class="navbar-toggler" type="button" data-toggle="collapse" data-target="#navbarNavDropdown"
                    aria-controls="navbarNavDropdown" aria-expanded="false" aria-label="Toggle navigation">
                <span class="navbar-toggler-icon"></span>
            </button>
            <div class="collapse navbar-collapse" id="navbarNavDropdown">
                <ul class="navbar-nav mr-auto">
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
                            <asp:LinkButton class="nav-link" ID="LinkButton1" runat="server"
                                            OnClick="Blogout_Click">Log out</asp:LinkButton>
                        </div>
                    </li>
                    <li class="nav-item">
                        <label class="nav-link" id="sslog" runat="server"></label>
                    </li>
                </ul>
                <!-- Subject filter dropdown -->
                <select id="classDrpDwn" class="form-control form-control-sm subject-select ml-3"
                        onchange="bindClass()" runat="server">
                    <option value="1">Physics</option>
                    <option value="2">Chemical</option>
                    <option value="3">Biology</option>
                    <option value="4">Calculus</option>
                    <option value="5">Statistics</option>
                    <option value="6">Applied Mathematics</option>
                </select>
            </div>
        </nav>

        <section class="wrapper">
            <div class="container-fostrap">
                <div class="content">
                    <div class="container">
                        <div class="main-card">
                            <!-- Loading indicator -->
                            <div id="loadingIndicator" class="loading-spinner">
                                <div class="spinner-border" role="status">
                                    <span class="sr-only">Loading courses...</span>
                                </div>
                                <p class="mt-2">Loading courses, please wait...</p>
                            </div>

                            <!-- Cards container -->
                            <div id="cardsContainer" style="display:none;">
                                <div class="row" id="cardsRow"></div>
                            </div>

                            <!-- Empty state -->
                            <div id="emptyState" style="display:none;" class="text-center py-5">
                                <p class="text-muted">No courses available for this subject.</p>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </section>
    </form>

    <script>
        // Subject-to-image mapping
        var SUBJECT_IMAGES = {
            '1': 'img/phy.png',
            '2': 'img/Chemical.png',
            '3': 'img/Biology.jfif',
            '4': 'img/Calculus.png',
            '5': 'img/Stt.png',
            '6': 'img/Applied mathematics.jfif'
        };

        // Global data store (populated by AJAX)
        var GDATA = null;

        // Check if user is logged in (set by server-side ASP.NET)
        var isLoggedIn = '<%= Session["uname"] %>' !== '';

        function bindClass() {
            var selectedMid = document.getElementById('classDrpDwn').value;
            createCards(selectedMid);
        }

        function createCards(selectedMid) {
            var row = document.getElementById('cardsRow');
            row.innerHTML = '';

            if (!GDATA) return;

            var hasCards = false;

            for (var i = 0; i < GDATA.length; i++) {
                var e = GDATA[i];
                if (e.mid !== selectedMid) continue;
                hasCards = true;

                var imgSrc = SUBJECT_IMAGES[e.mid] || 'img/phy.png';

                var link;
                if (isLoggedIn) {
                    link = 'watch.aspx?v=' + encodeURIComponent(e.vid) +
                            '&vname=' + encodeURIComponent(e.vname) +
                            '&vlink=' + encodeURIComponent(e.vlink) +
                            '&vdes=' + encodeURIComponent(e.description);
                } else {
                    link = 'Login.aspx';
                }

                var cardHTML =
                    '<div class="col-xs-12 col-sm-4 col-md-3 mb-4">' +
                        '<div class="card h-100">' +
                            '<a class="img-card" href="' + link + '">' +
                                '<img src="' + imgSrc + '" class="card-img-top" alt="' + e.vname + '">' +
                            '</a>' +
                            '<div class="card-body">' +
                                '<h4 class="card-title">' +
                                    '<a href="' + link + '" class="text-dark" style="text-decoration:none;">' + e.vname + '</a>' +
                                '</h4>' +
                                '<p class="text-muted small">' + (e.description || '') + '</p>' +
                            '</div>' +
                        '</div>' +
                    '</div>';

                row.innerHTML += cardHTML;
            }

            if (!hasCards) {
                document.getElementById('emptyState').style.display = 'block';
            }
        }

        $(document).ready(function () {
            var initialMid = document.getElementById('classDrpDwn').value;

            $.ajax({
                type: 'GET',
                url: '/Service1.svc/getScreenData',
                dataType: 'json',
                timeout: 10000,
                success: function (data) {
                    GDATA = data;

                    // Hide loading, show cards
                    document.getElementById('loadingIndicator').style.display = 'none';
                    document.getElementById('cardsContainer').style.display = 'block';

                    createCards(initialMid);
                },
                error: function (xhr, status, err) {
                    document.getElementById('loadingIndicator').innerHTML =
                        '<p class="text-danger">Failed to load courses. Please refresh the page.</p>' +
                        '<p class="text-muted small">Error: ' + err + '</p>';

                    console.error('Failed to load screen data:', err);
                }
            });
        });
    </script>
</body>
</html>
