<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Signin.aspx.cs" Inherits="Learing_web.Signin" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Sign Up - Learing Web</title>
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap@4.6.1/dist/css/bootstrap.min.css" />
    <link href="StyleSheethome.css" rel="stylesheet" />
    <link href="login.css" rel="stylesheet" />
    <style>
        body {
            background: linear-gradient(135deg, #1e3c72 0%, #2a5298 100%);
            min-height: 100vh;
        }
        .form-section {
            background: rgba(255,255,255,0.08);
            border-radius: 16px;
            padding: 24px;
            backdrop-filter: blur(10px);
        }
        .subject-checkboxes label {
            display: inline-block;
            margin-right: 16px;
            margin-bottom: 8px;
            padding: 8px 16px;
            background: rgba(255,255,255,0.1);
            border-radius: 8px;
            cursor: pointer;
            transition: background 0.2s;
        }
        .subject-checkboxes label:hover {
            background: rgba(255,255,255,0.2);
        }
        .subject-checkboxes input[type="checkbox"] {
            margin-right: 6px;
            accent-color: #56baed;
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
                    <div class="middlebox form-section">
                        <h2 class="text-center mb-4" style="font-weight:700;">Create Your Account</h2>
                        <p class="text-center text-muted mb-4">Join Learing Web and start your learning journey</p>

                        <div class="form-group">
                            <label for="userbox" class="text-info">Username</label>
                            <input type="text" id="userbox" name="login" placeholder="Choose a username" runat="server"
                                   class="form-control" tabindex="1" />
                        </div>
                        <div class="form-group">
                            <label for="pbox" class="text-info">Password</label>
                            <input type="password" id="pbox" name="login" placeholder="Choose a password" runat="server"
                                   class="form-control" tabindex="2" />
                        </div>

                        <div class="form-group subject-checkboxes">
                            <label class="text-info" style="display:block; margin-bottom:12px;">
                                Select Subjects You Want to Learn
                            </label>
                            <asp:CheckBoxList ID="CheckBoxList1" runat="server" RepeatDirection="Horizontal"
                                                CssClass="subject-checkboxes">
                            </asp:CheckBoxList>
                        </div>

                        <div class="text-center mt-3">
                            <asp:Button ID="Submit" runat="server" Text="Create Account"
                                        CssClass="btn btn-primary btn-block"
                                        OnClick="Submit_Click" />
                        </div>

                        <div id="messageArea" class="mt-3 text-center">
                            <asp:Label ID="Label1" runat="server"></asp:Label>
                        </div>

                        <div class="text-center mt-3">
                            <span class="text-muted">Already have an account? </span>
                            <a href="Login.aspx" class="text-info" style="font-weight:600;">Sign In</a>
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
