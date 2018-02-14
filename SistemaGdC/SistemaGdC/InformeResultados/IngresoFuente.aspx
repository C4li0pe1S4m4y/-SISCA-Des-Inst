<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="IngresoFuente.aspx.cs" Inherits="SistemaGdC.InformeResultados.IngresoFuente" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <link href="~/Content/bootstrap.css" rel="stylesheet" media="screen" />
    <h2>&nbsp;</h2>
    <h2>Fuentes de entradas</h2>
    <div class="row">
        <div class="col-xs-3">
            <div class="panel panel-default">
                <div class="panel-heading">Opcion</div>
                <div class="panel-body">
                    <asp:DropDownList ID="ddlOpcion" OnSelectedIndexChanged="ddlOpcion_SelectedIndexChanged" AutoPostBack="true" runat="server" CssClass="form-control"></asp:DropDownList>
                </div>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-xs-12">
            <div class="panel panel-default">
                <div class="panel-heading">Informacion</div>
                <div class="panel-body">
                    <div class="col-sm-4">
                        <label><span style="font-size: small">Proceso Relacionado</span>: </label>
                        <asp:DropDownList ID="ddlProceso" CssClass="dropdown input-sm" Width="100%" runat="server" Enabled="true"></asp:DropDownList>
                    </div>

                    <div class="col-sm-4">
                        <label class="col-sm-2"><span style="font-size: small">Unidad</span>: </label>
                        <asp:DropDownList ID="ddlUnidad" AutoPostBack="true" CssClass="dropdown input-sm" Width="100%" runat="server" Enabled="true"></asp:DropDownList>
                    </div>

                    <div class="col-sm-4">
                        <label class="col-sm-2"><span style="font-size: small">Dependencia</span>: </label>
                        <asp:DropDownList ID="ddlDependencia" CssClass="dropdown input-sm" Width="100%" runat="server" Enabled="true"></asp:DropDownList>
                    </div>




                    <div class="col-sm-12">
                        <label class="col-sm-2"><span style="font-size: small">Descripcion</span>: </label>
                        <asp:TextBox ID="txtDescripcion" Width="100%" CssClass="form-control col-lg-8" TextMode="MultiLine" runat="server"></asp:TextBox>
                        <br />
                    </div>

                </div>
            </div>
            <asp:Panel ID="pSatisfaccionCliente" runat="server" Visible="false">
                <div class="col-sm-4">
                    <label class="col-sm-8"><span style="font-size: small">Numero de Encuensta</span>: </label>
                    <asp:TextBox ID="txtEncuentas" CssClass="form-control" Width="100%" runat="server" Enabled="true"></asp:TextBox>
                </div>
            </asp:Panel>
            <asp:Panel ID="pRevAltaDirec" runat="server" Visible="false">
                <div class="col-sm-4">
                    <label class="col-sm-4"><span style="font-size: small">No. Revision</span>: </label>
                    <asp:TextBox ID="txtNoRevision" CssClass="form-control" Width="100%" runat="server" Enabled="true"></asp:TextBox>
                </div>
                <div class="col-sm-4">
                    <label class="col-sm-8"><span style="font-size: small">No. Compromiso</span>: </label>
                    <asp:TextBox ID="txtNoCompromiso" CssClass="form-control" Width="100%" runat="server" Enabled="true"></asp:TextBox>
                </div>
                <div class="col-sm-4">
                    <label class="col-sm-8"><span style="font-size: small">Fecha Revision</span>: </label>
                    <asp:TextBox ID="txtFechaRevision" TextMode="Date" CssClass="form-control" Width="100%" runat="server" Enabled="true"></asp:TextBox>
                </div>
            </asp:Panel>
             <asp:Panel ID="pSNC" runat="server" Visible="false">
                <div class="col-sm-4">
                    <label class="col-sm-8"><span style="font-size: small">Mes</span>: </label>
                    <asp:DropDownList ID="ddlMesesSNC" CssClass="dropdown input-sm" Width="100%" runat="server" Enabled="true"></asp:DropDownList>
                </div>
                  <div class="col-sm-4">
                    <label class="col-sm-8"><span style="font-size: small">Año</span>: </label>
                    <asp:DropDownList ID="ddlAnioSNC" CssClass="dropdown input-sm" Width="100%" runat="server" Enabled="true"></asp:DropDownList>
                </div>
            </asp:Panel>
             <asp:Panel ID="pQuejas" runat="server" Visible="false">
                <div class="col-sm-4">
                    <label class="col-sm-8"><span style="font-size: small">Tipo Expresion</span>: </label>
                    <asp:DropDownList ID="ddlTipoExpresion" CssClass="dropdown input-sm" Width="100%" runat="server" Enabled="true"></asp:DropDownList>
                </div>
                  <div class="col-sm-4">
                    <label class="col-sm-8"><span style="font-size: small"> Codigo/s</span>: </label>
                    <asp:TextBox ID="txtcodigos" CssClass="form-control" TextMode="MultiLine" Width="100%" runat="server" Enabled="true"></asp:TextBox>
                </div>
            </asp:Panel>
        </div>
    </div>
    <br />
    <div class="row">
        <div class="col-xs-4">
            <asp:Button ID="btnGuardar" runat="server" Text="Guardar" CssClass="btn btn-success" Width="100%" />
        </div>
         <div class="col-xs-4">
            <asp:Button ID="btnNuevo" runat="server" Text="Nuevo" CssClass="btn btn-warning" Width="100%" />
        </div>
         <div class="col-xs-4">
            <asp:Button ID="btnListado" runat="server" Text="Listado" CssClass="btn btn-info" Width="100%" />
        </div>
    </div>
</asp:Content>
