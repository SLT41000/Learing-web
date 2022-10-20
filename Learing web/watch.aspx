<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="watch.aspx.cs" Inherits="Learing_web.watch" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Watch</title>
        
<link href="login.css" rel="stylesheet"/>        
<link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap@4.6.1/dist/css/bootstrap.min.css"/>
  <script src="https://cdn.jsdelivr.net/npm/jquery@3.6.0/dist/jquery.slim.min.js"></script>
  <script src="https://cdn.jsdelivr.net/npm/popper.js@1.16.1/dist/umd/popper.min.js"></script>
  <script src="https://cdn.jsdelivr.net/npm/bootstrap@4.6.1/dist/js/bootstrap.bundle.min.js"></script>  
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/1.10.2/jquery.min.js" type="text/javascript"></script>
    <script src="file/jquery.min.js"></script>
  <!-- jQuery UI 1.11.4 -->
  <script src="file/jquery-ui.min.js"></script>
    <script src="videos.css"></script>
    
  <!-- Resolve conflict in jQuery UI tooltip with Bootstrap tooltip -->
</head>

<body class="p-3 mb-2 bg-secondary text-white">
    <form id="form1" runat="server">
    <nav class="navbar navbar-expand-lg navbar-dark bg-dark" >
  
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
    
    

       <section class="wrapper" >
            
    <div class="container-fostrap">

        <div class="content">
        <asp:ScriptManager ID="Scriptmanager1" runat="server" EnablePageMethods="true" />
        <div class="content">
            <div class="container navbar-dark bg-dark rounded-lg ">

                


                <iframe class="responsive-iframe" id="video" width="550" height="315"    allowfullscreen="allowfullscreen" frameborder="0"  onclick="onchickvideo" runat="server" style="border-radius:20px;padding:5px; ">
</iframe><br />
                
                <h2 id="headname"></h2>
                <p  id="p"></p>

                 <asp:Button ID="Checkin" runat="server" Text="Check in" OnClick="onchickvideo"  />
                <asp:Button ID="alreadywatch" runat="server" Text="Mark at watched" OnClick="onchickalreadyw"   />
                <script src="urldata.js"></script>
                    
                </div>
                
              
            </div>
        </div>

    </div>
</section>
        
    </form>
</body>
</html>
