<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ListadoAcciones.aspx.cs" Inherits="SistemaGdC.InformeResultados.ListadoAcciones" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
     <h2>&nbsp;</h2>
    <h2>Listado de Acciones</h2>
    <div class="row">
        <div class="col-xs-10">
            <asp:GridView ID="gvListadoAcciones" runat="server" CssClass="table table-hover table-bordered">
                <AlternatingRowStyle BackColor="#B5F0FF" />
                <HeaderStyle BackColor="#33CCFF" />

            </asp:GridView>
        </div>
    </div>
</asp:Content>
