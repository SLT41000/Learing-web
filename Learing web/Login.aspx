<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="Learing_web.Login" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Login - Learing Web</title>
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap@4.6.1/dist/css/bootstrap.min.css" />
    <link href="StyleSheethome.css" rel="stylesheet" />
    <link href="login.css" rel="stylesheet" />
    <style>
        body {
            background: linear-gradient(135deg, #1e3c72 0%, #2a5298 100%);
            min-height: 100vh;
        }
    </style>
</head>
<body class="text-white">
    <nav class="navbar navbar-expand-lg navbar-dark bg-dark">
        <button class="navbar-toggler" type="button" data-toggle="collapse" data-target="#navbarNavDropdown"
                aria-controls="navbarNavDropdown" aria-expanded="false" aria-label="Toggle navigation">
            <span class="navbar-toggler-icon"></span>
        </button>
        <div class="collapse navbar-collapse" id="navbarNavDropdown">
            <ul class="navbar-nav">
                <li class="nav-item">
                    <a class="nav-link" href="default.aspx">Back to Home</a>
                </li>
            </ul>
        </div>
    </nav>

    <form id="form1" runat="server">
        <section class="wrapper">
            <div class="container-fostrap">
                <div class="content">
                    <div id="middlebox" class="middlebox">
                        <h2 style="text-align:center; font-weight:700;">Welcome Back</h2>
                        <p class="text-center text-muted" style="font-size:0.9rem;">Sign in to continue learning</p>

                        <div class="form-group">
                            <input type="text" id="userbox" name="login" placeholder="Username" runat="server"
                                   class="form-control" tabindex="1" autofocus />
                        </div>
                        <div class="form-group">
                            <input type="password" id="pbox" name="login" placeholder="Password" runat="server"
                                   class="form-control" tabindex="2" />
                        </div>

                        <div class="text-center">
                            <asp:Button ID="Submit" runat="server" Text="Sign In"
                                        CssClass="btn btn-primary btn-block"
                                        OnClick="Submit_Click" />
                        </div>

                        <div id="errorArea" class="mt-3 text-center">
                            <asp:Label ID="Label1" runat="server" CssClass="text-danger"></asp:Label>
                        </div>

                        <div class="text-center mt-3">
                            <span class="text-muted">Don't have an account? </span>
                            <a href="Signin.aspx" class="text-info" style="font-weight:600;">Sign Up</a>
                        </div>
                    </div>
                </div>
            </div>
        </section>
    </form>

    <script src="https://cdn.jsdelivr.net/npm/jquery@3.6.0/dist/jquery.min.js"></script>
    <script src="https://cdn.jsdelivr.net/npm/popper.js@1.16.1/dist/umd/popper.min.js"></script>
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@4.6.1/dist/js/bootstrap.bundle.min.js"></script>
</body>
</html>
