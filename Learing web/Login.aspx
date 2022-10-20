<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="Learing_web.Login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Login</title>
<link href="StyleSheethome.css" rel="stylesheet"/>
    
        <link href="StyleSheethome.css" rel="stylesheet"/>
    <link href="login.css" rel="stylesheet"/>
<link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap@4.6.1/dist/css/bootstrap.min.css"/>
  <script src="https://cdn.jsdelivr.net/npm/jquery@3.6.0/dist/jquery.slim.min.js"></script>
  <script src="https://cdn.jsdelivr.net/npm/popper.js@1.16.1/dist/umd/popper.min.js"></script>
  <script src="https://cdn.jsdelivr.net/npm/bootstrap@4.6.1/dist/js/bootstrap.bundle.min.js"></script>  

    <script src="file/jquery.min.js"></script>
  <!-- jQuery UI 1.11.4 -->
  <script src="file/jquery-ui.min.js"></script>
  <!-- Resolve conflict in jQuery UI tooltip with Bootstrap tooltip -->
</head>
<body class="p-3 mb-2 bg-secondary text-white">

    <nav class="navbar navbar-expand-lg  navbar-dark bg-dark" >
  
  <button class="navbar-toggler" type="button" data-toggle="collapse" data-target="#navbarNavDropdown" aria-controls="navbarNavDropdown" aria-expanded="false" aria-label="Toggle navigation">
    <span class="navbar-toggler-icon"></span>
  </button>
  <div class="collapse navbar-collapse" id="navbarNavDropdown">
    <ul class="navbar-nav">
      <li class="nav-item active">
        <a class="nav-link" href="default.aspx">Home </a>
      </li>
        
      <li class="nav-item">
          
        

      </li>
            
      <li class="nav-item">
        <div  id="signin" runat="server">
            <a class="nav-link" href="Signin.aspx">Sign in</a>
        </div>
      </li>
      
    
       
     
    </ul>
  </div>
</nav>

    <form id="form1" runat="server">
         <section class="wrapper" >
            
    <div class="container-fostrap">
        
        <div class="content">
        <div id="middlebox" class="middlebox">
            <h2 style=" text-align: center;">Login</h2><br/>

            <input type="text" id="userbox" name="login" placeholder="Username" runat="server"/>
      <input type="password" id="pbox"  name="login" placeholder="password" runat="server"/>
            <asp:Button ID="Submit" runat="server" Text="Submit" OnClick="Submit_Click"  /><br />
            <asp:Label ID="Label1" runat="server" ></asp:Label>
            <asp:Label ID="Label2" runat="server" ></asp:Label>
        </div>


            </div>
        </div>
             </section>

    </form>
</body>
</html>
