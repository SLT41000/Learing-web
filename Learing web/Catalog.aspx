<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Catalog.aspx.cs" Inherits="Learing_web.Catalog" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Catalog - Learing Web</title>
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap@4.6.1/dist/css/bootstrap.min.css" />
    <link href="StyleSheethome.css" rel="stylesheet" />
    <style>
        body {
            background-color: #343a40;
        }
        .catalog-header {
            margin: 1.5rem 0;
        }
        .catalog-header h2 {
            font-weight: 700;
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
        #catalog tbody tr:hover {
            background-color: #2b3035;
        }
        #catalog td, #catalog th {
            vertical-align: middle;
        }
        .no-data {
            text-align: center;
            padding: 40px 0;
            color: #adb5bd;
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
                    <a class="nav-link" href="Certificate.aspx">Certificate</a>
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
                            <div class="catalog-header">
                                <h2>My Watch History</h2>
                                <p class="text-muted">Videos you have started watching</p>
                            </div>

                            <!-- Loading indicator -->
                            <div id="loadingIndicator" class="loading-spinner">
                                <div class="spinner-border" role="status">
                                    <span class="sr-only">Loading catalog...</span>
                                </div>
                                <p class="mt-2">Loading your catalog...</p>
                            </div>

                            <!-- Data table -->
                            <div id="tableContainer" style="display:none;">
                                <table class="table table-dark table-striped" id="catalog">
                                    <thead>
                                        <tr>
                                            <th scope="col">#</th>
                                            <th scope="col">Video Name</th>
                                            <th scope="col">Date Watched</th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                    </tbody>
                                </table>
                            </div>

                            <!-- Empty state -->
                            <div id="emptyState" style="display:none;" class="no-data">
                                <p class="lead">No videos watched yet.</p>
                                <a href="default.aspx" class="btn btn-primary">Browse courses</a>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </section>
    </form>

    <script>
        var GDATA = null;

        function createTable() {
            var tbody = document.querySelector('#catalog tbody');
            tbody.innerHTML = '';

            if (!GDATA || GDATA.length === 0) {
                document.getElementById('tableContainer').style.display = 'none';
                document.getElementById('emptyState').style.display = 'block';
                return;
            }

            document.getElementById('tableContainer').style.display = 'block';
            document.getElementById('emptyState').style.display = 'none';

            for (var i = 0; i < GDATA.length; i++) {
                var e = GDATA[i];
                var row = tbody.insertRow();
                row.insertCell(0).textContent = i + 1;
                row.insertCell(1).textContent = e.vname;

                // Format the date nicely
                var dateStr = e.ontime;
                if (dateStr) {
                    try {
                        var d = new Date(dateStr);
                        if (!isNaN(d.getTime())) {
                            dateStr = d.toLocaleDateString() + ' ' + d.toLocaleTimeString([], { hour: '2-digit', minute: '2-digit' });
                        }
                    } catch (ex) { /* keep original */ }
                }
                row.insertCell(2).textContent = dateStr;
            }
        }

        $(document).ready(function () {
            var aid = '<%= Session["aid"] %>';

            if (!aid) {
                document.getElementById('loadingIndicator').innerHTML =
                    '<p class="text-danger">Session expired. Please log in again.</p>' +
                    '<a href="Login.aspx" class="btn btn-primary mt-2">Log in</a>';
                return;
            }

            $.ajax({
                type: 'GET',
                url: '/Service2.svc/Submit_Click',
                data: 'aid=' + encodeURIComponent(aid),
                dataType: 'json',
                timeout: 10000,
                success: function (data) {
                    GDATA = data;
                    document.getElementById('loadingIndicator').style.display = 'none';
                    createTable();
                },
                error: function (xhr, status, err) {
                    document.getElementById('loadingIndicator').innerHTML =
                        '<p class="text-danger">Failed to load catalog. Please try again.</p>';
                    console.error('Catalog load error:', err);
                }
            });
        });
    </script>
</body>
</html>
