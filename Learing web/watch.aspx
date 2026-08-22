<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="watch.aspx.cs" Inherits="Learing_web.watch" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Watch - Learing Web</title>
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap@4.6.1/dist/css/bootstrap.min.css" />
    <link href="login.css" rel="stylesheet" />
    <link href="videos.css" rel="stylesheet" />
    <style>
        .video-container {
            position: relative;
            padding-bottom: 56.25%; /* 16:9 aspect ratio */
            height: 0;
            overflow: hidden;
            border-radius: 12px;
            box-shadow: 0 4px 24px rgba(0,0,0,0.3);
            background: #000;
        }
        .video-container iframe {
            position: absolute;
            top: 0;
            left: 0;
            width: 100%;
            height: 100%;
            border: none;
        }
        .video-header {
            margin-top: 1.5rem;
        }
        .video-header h2 {
            font-weight: 700;
            margin-bottom: 0.5rem;
        }
        .video-header p {
            color: #adb5bd;
            margin-bottom: 1.5rem;
        }
        .action-buttons .btn {
            margin-right: 0.75rem;
            margin-bottom: 0.5rem;
        }
    </style>
</head>
<body class="p-3 mb-2 bg-secondary text-white">
    <nav class="navbar navbar-expand-lg navbar-dark bg-dark">
        <button class="navbar-toggler" type="button" data-toggle="collapse" data-target="#navbarNavDropdown"
                aria-controls="navbarNavDropdown" aria-expanded="false" aria-label="Toggle navigation">
            <span class="navbar-toggler-icon"></span>
        </button>
        <div class="collapse navbar-collapse" id="navbarNavDropdown">
            <ul class="navbar-nav">
                <li class="nav-item active">
                    <a class="nav-link" href="default.aspx">Home <span class="sr-only">(current)</span></a>
                </li>
                <li class="nav-item">
                    <a class="nav-link" href="Catalog.aspx">Catalog</a>
                </li>
                <li class="nav-item">
                    <a class="nav-link" href="Certificate.aspx">Certificate</a>
                </li>
                <li class="nav-item">
                    <asp:LinkButton class="nav-link" ID="LinkButtonLogout" runat="server"
                                    OnClick="Blogout_Click">Log out</asp:LinkButton>
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
                                <div class="col-12">
                                    <!-- Responsive 16:9 video container -->
                                    <div class="video-container">
                                        <iframe id="video" runat="server"
                                                allowfullscreen="allowfullscreen"
                                                frameborder="0"
                                                title="Video player">
                                        </iframe>
                                    </div>

                                    <div class="video-header">
                                        <h2 id="headname" class="text-light"></h2>
                                        <p id="p"></p>
                                    </div>

                                    <div class="action-buttons">
                                        <asp:Button ID="Checkin" runat="server" Text="Check In"
                                                    CssClass="btn btn-primary"
                                                    OnClick="onchickvideo" />
                                        <asp:Button ID="alreadywatch" runat="server" Text="Mark as Watched"
                                                    CssClass="btn btn-success"
                                                    OnClick="onchickalreadyw" />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </section>
    </form>

    <script src="https://cdn.jsdelivr.net/npm/jquery@3.6.0/dist/jquery.min.js"></script>
    <script src="https://cdn.jsdelivr.net/npm/popper.js@1.16.1/dist/umd/popper.min.js"></script>
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@4.6.1/dist/js/bootstrap.bundle.min.js"></script>
    <script src="urldata.js"></script>
</body>
</html>
