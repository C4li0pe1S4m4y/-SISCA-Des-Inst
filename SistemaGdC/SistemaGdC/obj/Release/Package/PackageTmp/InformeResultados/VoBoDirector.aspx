<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="VoBoDirector.aspx.cs" Inherits="SistemaGdC.Informe.VoBoDirector" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Visto Bueno Director</h2>
    <asp:TextBox ID ="txtBusqueda" runat="server" CssClass="dropdown input-sm" Width="100%" />
    <asp:Button ID="btnBusqueda" Text="Buscar" CssClass="btn btn-info"  runat="server" Width="100%" />
    <asp:GridView ID="gvListado" runat="server"  AllowPaging="True" DataKeyNames="numero" CssClass="table table-hover table-responsive">
        <AlternatingRowStyle BackColor="#F0F0F0" />
        <Columns>
            
            <asp:CommandField ButtonType="Button" HeaderText="Seleccionar"  ControlStyle-CssClass="btn btn-primary" ShowSelectButton="True">
                <HeaderStyle BorderStyle="Inset"  HorizontalAlign="Center" VerticalAlign="Middle" />
                <ItemStyle BorderStyle="Inset" HorizontalAlign="Center" VerticalAlign="Middle" />
            </asp:CommandField>
           

        </Columns>
         <HeaderStyle BackColor="#0099FF" />
    </asp:GridView>
    <div class="form-group"></div>
    <asp:Button ID="btnAprobar" Text="Aprobar" CssClass="btn btn-success"  runat="server" Width="100%" />
    <asp:Button ID="btnRechazar" Text="Rechazar" CssClass="btn btn-danger"  runat="server" Width="100%" />
</asp:Content>
