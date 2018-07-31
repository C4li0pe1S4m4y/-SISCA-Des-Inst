<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="SistemaGdC.Login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <link href="content/bootstrap.css" rel="stylesheet" type="text/css" media="screen" />
    <title></title>

</head>
<body style="background-image: url(../SISCA/Content/fondoLogin.png); background-repeat: no-repeat; background-attachment: fixed">
    <form>
    </form>

    <form id="form1" runat="server">
        <br />
        <br />
        <br />
        <br />
        <div class="col-md-10" />
        <div class="col-md-10" />
        <div class="row">

            <div class="col-md-8 col-md-offset-5">
                <div class="col-md-6">
                    <asp:Image ID="Image1" runat="server" Height="334px" ImageUrl="~/fonts/logoCdagNuevo.png" Width="210px" />

                    <h3>Sistema Gestion de la Calidad - SISCA</h3>
                </div>
                <div class="col-md-6">
                    <h4 style="color: rgb(15, 44, 82)"><b>Usuario:</b></h4>
                    <asp:TextBox ID="txtusuario" Text="" CssClass="form-control" Width="80%" runat="server"></asp:TextBox>
                    <br />
                    <h4 style="color: rgb(15, 44, 82)"><b>Contraseña:</b></h4>
                    <asp:TextBox ID="txtpassword" TextMode="Password" Text="" CssClass="form-control" Width="80%" runat="server"></asp:TextBox>
                    <br />
                    <br />
                    <asp:Button ID="btnIngresar" CssClass="btn btn-primary" Text="Ingresar" Width="80%" OnClick="btnIngresar_Click" runat="server" />
                    <asp:Label ID="lblError" CssClass="label label-danger" runat="server"></asp:Label>

                </div>
            </div>


        </div>


    </form>
</body>
</html>
