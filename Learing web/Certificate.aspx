<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Certificate.aspx.cs" Inherits="Learing_web.Certificate" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Certificate - Learing Web</title>
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap@4.6.1/dist/css/bootstrap.min.css" />
    <link href="StyleSheethome.css" rel="stylesheet" />
    <style>
        body {
            background-color: #343a40;
        }
        .cert-header {
            margin: 1.5rem 0;
            text-align: center;
        }
        .cert-header h2 {
            font-weight: 700;
            color: #ffc107;
        }
        .cert-header p {
            color: #adb5bd;
        }
        .loading-spinner {
            text-align: center;
            padding: 40px 0;
        }
        .loading-spinner .spinner-border {
            width: 3rem;
            height: 3rem;
            color: #0d6efd;
        }
        .cert-card {
            background: #2d3748;
            border: 1px solid #4a5568;
            border-radius: 12px;
            overflow: hidden;
            transition: transform 0.2s ease, box-shadow 0.2s ease;
        }
        .cert-card:hover {
            transform: translateY(-4px);
            box-shadow: 0 8px 24px rgba(0,0,0,0.3);
        }
        .cert-card-img {
            width: 100%;
            height: 200px;
            object-fit: cover;
            display: block;
        }
        .cert-card-body {
            padding: 16px 20px;
        }
        .cert-card-body h4 {
            color: #ffc107;
            font-weight: 700;
            margin-bottom: 0.25rem;
        }
        .cert-badge {
            display: inline-block;
            background: #48bb78;
            color: #fff;
            padding: 4px 12px;
            border-radius: 20px;
            font-size: 0.8rem;
            font-weight: 600;
            margin-top: 8px;
        }
        .empty-state {
            text-align: center;
            padding: 60px 0;
            color: #a0aec0;
        }
        .empty-state .icon {
            font-size: 4rem;
            margin-bottom: 1rem;
        }
    </style>
</head>
<body class="p-3 mb-2 text-white">
    <nav class="navbar navbar-expand-lg navbar-dark bg-dark">
        <button class="navbar-toggler" type="button" data-toggle="collapse" data-target="#navbarNavDropdown"
                aria-controls="navbarNavDropdown" aria-expanded="false" aria-label="Toggle navigation">
            <span class="navbar-toggler-icon"></span>
        </button>
        <div class="collapse navbar-collapse" id="navbarNavDropdown">
            <ul class="navbar-nav">
                <li class="nav-item active">
                    <a class="nav-link" href="default.aspx">Home</a>
                </li>
                <li class="nav-item">
                    <a class="nav-link" href="Catalog.aspx">Catalog</a>
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
                            <div class="cert-header">
                                <h2>My Certificates</h2>
                                <p>Completed courses and earned certificates</p>
                            </div>

                            <!-- Loading -->
                            <div id="loadingIndicator" class="loading-spinner">
                                <div class="spinner-border" role="status">
                                    <span class="sr-only">Loading certificates...</span>
                                </div>
                                <p class="mt-2">Loading certificates...</p>
                            </div>

                            <!-- Certificate cards container -->
                            <div id="certContainer" style="display:none;">
                                <div class="row" id="certRow"></div>
                            </div>

                            <!-- Empty state -->
                            <div id="emptyState" style="display:none;" class="empty-state">
                                <div class="icon">🎓</div>
                                <p class="lead">No certificates earned yet.</p>
                                <p class="text-muted">Complete video courses to earn your certificates.</p>
                                <a href="default.aspx" class="btn btn-primary mt-2">Start Learning</a>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </section>
    </form>

    <script>
        var GDATA = null;

        function createCertCards() {
            var row = document.getElementById('certRow');
            row.innerHTML = '';

            if (!GDATA || GDATA.length === 0) {
                document.getElementById('certContainer').style.display = 'none';
                document.getElementById('emptyState').style.display = 'block';
                return;
            }

            document.getElementById('certContainer').style.display = 'block';
            document.getElementById('emptyState').style.display = 'none';

            for (var i = 0; i < GDATA.length; i++) {
                var e = GDATA[i];
                var certHTML =
                    '<div class="col-xs-12 col-sm-6 col-md-4 mb-4">' +
                        '<div class="cert-card">' +
                            '<img src="img/tp204-certificate-05.jpg" alt="Certificate" class="cert-card-img">' +
                            '<div class="cert-card-body">' +
                                '<h4>' + e.vname + '</h4>' +
                                '<span class="cert-badge">Completed</span>' +
                            '</div>' +
                        '</div>' +
                    '</div>';

                row.innerHTML += certHTML;
            }
        }

        $(document).ready(function () {
            var aid = '<%= Session["aid"] %>';

            if (!aid) {
                document.getElementById('loadingIndicator').innerHTML =
                    '<p class="text-danger">Session expired. Please log in again.</p>';
                return;
            }

            $.ajax({
                type: 'GET',
                url: '/Service3.svc/getalrdy',
                data: 'aid=' + encodeURIComponent(aid),
                dataType: 'json',
                timeout: 10000,
                success: function (data) {
                    GDATA = data;
                    document.getElementById('loadingIndicator').style.display = 'none';
                    createCertCards();
                },
                error: function (xhr, status, err) {
                    document.getElementById('loadingIndicator').innerHTML =
                        '<p class="text-danger">Failed to load certificates.</p>';
                    console.error('Certificate load error:', err);
                }
            });
        });
    </script>
</body>
</html>
