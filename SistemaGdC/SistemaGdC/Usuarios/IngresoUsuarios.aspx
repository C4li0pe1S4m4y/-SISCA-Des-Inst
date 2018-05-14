<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="IngresoUsuarios.aspx.cs" Inherits="SistemaGdC.Usuarios.IngresoUsuarios" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <br />
    <br />
    <h2 style="color: white"><b>Ingreso de Usuario</b></h2>
    <div class="jumbotron">
        <div class="row">
            <div class="row col-md-8 col-md-offset-2">
                <div class="col-md-6">
                    <div class="form-group">
                        <label>Usuario: </label>
                        <asp:TextBox ID="txtUsuario" CssClass="form-control input" runat="server"></asp:TextBox>
                        <asp:RequiredFieldValidator ValidationGroup="validar" ID="RFVtxtUsuario" Style="color: red;" SetFocusOnError="true" ControlToValidate="txtUsuario" InitialValue="" runat="server" ErrorMessage="Ingrese Usuario." Display="Dynamic" />
                    </div>
                    <div class="form-group">
                        <label>Contraseña: </label>
                        <asp:TextBox ID="txtcontra" CssClass="form-control input" runat="server" TextMode="Password"></asp:TextBox>
                        <asp:RequiredFieldValidator ValidationGroup="validar" ID="RFVtxtcontra" Style="color: red;" SetFocusOnError="true" ControlToValidate="txtcontra" InitialValue="" runat="server" ErrorMessage="Ingrese Contraseña." Display="Dynamic" />
                    </div>
                    <div class="form-group">
                        <label>Correo: </label>
                        <asp:TextBox ID="txtCorreo" CssClass="form-control input" runat="server"></asp:TextBox>
                        <asp:RequiredFieldValidator ValidationGroup="validar" ID="RFVtxtCorreo" Style="color: red;" SetFocusOnError="true" ControlToValidate="txtCorreo" InitialValue="" runat="server" ErrorMessage="Ingrese Correo Electrónico." Display="Dynamic" />
                    </div>
                </div>
                <div class="col-md-6">
                    <div class="form-group">
                        <label>Empleado: </label>
                        <asp:DropDownList ID="ddlEmpleado" CssClass="form-control input" Width="100%" runat="server" Enabled="true"></asp:DropDownList>
                        <asp:RequiredFieldValidator ValidationGroup="validar" ID="RFVddlEmpleado" Style="color: red;" SetFocusOnError="true" ControlToValidate="ddlEmpleado" InitialValue="0" runat="server" ErrorMessage="Seleccione Empleado." Display="Dynamic" />
                    </div>
                    <div class="form-group">
                        <label>Tipo de Usuario: </label>
                        <asp:DropDownList ID="ddlTipoUsuario" CssClass="form-control input" Width="100%" runat="server" Enabled="true"></asp:DropDownList>
                        <asp:RequiredFieldValidator ValidationGroup="validar" ID="RFVddlTipoUsuario" Style="color: red;" SetFocusOnError="true" ControlToValidate="ddlTipoUsuario" InitialValue="0" runat="server" ErrorMessage="Seleccione Tipo de Usuario." Display="Dynamic" />
                    </div>
                    <div class="col-md-6 col-md-offset-3">
                        <label>&nbsp;</label>
                        <asp:Button ID="btnGuardar" Text="Guardar" OnClick="btnGuardar_Click" ValidationGroup="validar" CssClass="btn btn-success btn-lg" runat="server" Width="100%" 
                        OnClientClick="return confirm('¿Desea ingresar el Usuario?');"/>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
