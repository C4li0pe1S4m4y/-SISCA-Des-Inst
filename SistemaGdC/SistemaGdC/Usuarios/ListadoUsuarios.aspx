<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ListadoUsuarios.aspx.cs" Inherits="SistemaGdC.Usuarios.ListadoUsuarios" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Listado Usuarios</h2>
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
</asp:Content>
