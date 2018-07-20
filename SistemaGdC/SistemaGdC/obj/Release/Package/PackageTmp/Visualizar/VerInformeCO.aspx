<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="VerInformeCO.aspx.cs" Inherits="SistemaGdC.Visualizar.VerInformeCO" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <link href="~/Content/bootstrap.css" rel="stylesheet" media="screen" />
    <h2>&nbsp;</h2>
    <h2 style="color: white"><b>Visualización de Informe de Corrección</b></h2>
    <div class="row">
        <div class="col-xs-12">
            <asp:UpdatePanel ID="UpdatePanelResponsable" runat="server">
                <ContentTemplate>
                    <div class="panel panel-default">
                        <div class="panel-heading">Encabezado</div>
                        <div class="panel-body">
                            <div class="row">
                                <div class="col-md-8">
                                    <div class="row">
                                        <div class="col-md-2">
                                            <div class="form-group">
                                                <label>Año:</label>
                                                <asp:TextBox ID="txtanio" Enabled="false" CssClass="form-control" runat="server" required=""></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="col-md-5">
                                            <label>Unidad:</label>
                                            <asp:DropDownList ID="ddlunidad" Enabled="false" AutoPostBack="true" OnSelectedIndexChanged="ddlunidad_SelectedIndexChanged" runat="server" CssClass="form-control"></asp:DropDownList>
                                        </div>
                                        <div class="col-md-5">
                                            <label>Dependecia:</label>
                                            <asp:DropDownList ID="ddldependencia" Enabled="false" runat="server" CssClass="form-control"></asp:DropDownList>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-4">
                                            <label>No. de Evaluación:</label>
                                            <asp:TextBox ID="txtEvaluacion" Enabled="false" AutoPostBack="true" runat="server" CssClass="form-control"></asp:TextBox>
                                        </div>
                                        <div class="col-md-4">
                                            <label>No. de Hallazgo:</label>
                                            <asp:TextBox ID="txtHallazgo" Enabled="false" AutoPostBack="true" runat="server" CssClass="form-control"></asp:TextBox>
                                        </div>
                                        <div class="col-md-4">
                                            <label>Tipo de Acción/Informe:</label>
                                            <asp:DropDownList ID="ddlTipoAccionInforme" Enabled="false" runat="server" CssClass="form-control"></asp:DropDownList>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-4">
                                    <div class="form-group">
                                        <label>Descripción:</label>
                                        <asp:TextBox ID="txtDescripcion" Enabled="false" TextMode="MultiLine" runat="server" CssClass="form-control input" Style="height: 105px"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>

        <div class="col-xs-12">
            <div class="panel panel-default">
                <div class="panel-heading">Informe</div>
                <div class="panel-body">
                    <div class="row">

                        <div class="form-group col-md-2">
                            <label>Estado:</label>
                        <div class="col-md-4">
                            <div class="form-group">
                                <label>&nbsp;</label>
                                <asp:Button ID="btnDescargarEvidencia" Visible="true" Text="Evidencia" OnClick="btnDescargarEvidencia_Click" CssClass="btn btn-info" runat="server" Width="100%" />
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-6">
                            <div class="form-group">
                                <label>Descripción de la Acción Realizada</label>
                                <asp:TextBox ID="txtAccionRealizada" Enabled="false" TextMode="MultiLine" runat="server" CssClass="form-control input" Style="height: 200px"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-md-6">
                            <div class="form-group">
                                <label>Descripción Evidencia</label>
                                <asp:TextBox ID="txtDesEvidencia" Enabled="false" TextMode="MultiLine" runat="server" CssClass="form-control input" Style="height: 200px"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-md-12">
                <div class="form-group">
                    <div class="col-md-2">
                        <asp:Button ID="btnCancelar" OnClick="btnCancelar_Click" Text="Cancelar" CssClass="btn" runat="server" Width="100%" />
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
