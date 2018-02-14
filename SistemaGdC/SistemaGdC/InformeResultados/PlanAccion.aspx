<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="PlanAccion.aspx.cs" Inherits="SistemaGdC.InformeResultados.PlanAccion" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <link href="~/Content/bootstrap.css" rel="stylesheet" media="screen" />
    <h2>&nbsp;</h2>
    <h2>Ingreso de Accion a Tomar (Informe/Plan)</h2>
    <div class="row">
        <div class="col-xs-12">
            <div class="panel panel-default">
                <div class="panel-heading">Encabezado</div>
                <div class="panel-body">
                    <div class="col-sm-1">
                        <label>
                            <span style="font-size: medium">año</span>: 
                        </label>
                        <asp:TextBox ID="txtanio" CssClass="form-control" Height="85%" TextMode="Number" Width="100%" runat="server" Style="font-size: x-small" required=""></asp:TextBox>
                    </div>
                    <div class="col-sm-4">
                        <label>Unidad</label>
                        <asp:DropDownList ID="ddlunidad" AutoPostBack="true" OnSelectedIndexChanged="ddlunidad_SelectedIndexChanged" runat="server" CssClass="form-control"></asp:DropDownList>
                    </div>
                    <div class="col-sm-4">
                        <label>Dependecia</label>
                        <asp:DropDownList ID="ddldependencia" runat="server" CssClass="form-control"></asp:DropDownList>
                    </div>
                    <div class="col-sm-3">
                        <label>Numero de Evaluacion</label>
                        <asp:DropDownList ID="lblEvaluacion" AutoPostBack="true" OnSelectedIndexChanged="lblEvaluacion_SelectedIndexChanged" runat="server" CssClass="form-control"></asp:DropDownList>
                    </div>

                    <div class="col-sm-3">
                        <label>No. Hallazgo</label>
                        <asp:DropDownList ID="ddlHallazgo" AutoPostBack="true" OnSelectedIndexChanged="ddlHallazgo_SelectedIndexChanged" runat="server" CssClass="form-control"></asp:DropDownList>
                    </div>
                    <div class="col-sm-3">
                        <label>Tipo Accion/Informe</label>
                        <asp:DropDownList ID="ddlTipoAccionInforme" runat="server" CssClass="form-control"></asp:DropDownList>
                    </div>
                    <div class="col-sm-11">
                        <label>Descripcion</label>
                        <asp:TextBox ID="txtDescripcion" Enabled="false" TextMode="MultiLine" runat="server" CssClass="form-control input"></asp:TextBox>
                    </div>
                </div>
            </div>
        </div>
        <div class="col-sm-12">
            <div class="panel panel-default">
                <div class="panel-heading">Causa Raiz</div>
                <div class="panel-body">
                    <div class="col-sm-3">
                        <label>Tecnica de Analisis</label>
                        <asp:DropDownList ID="ddlTecnicaAnalisis" runat="server" CssClass="form-control input"></asp:DropDownList>
                    </div>
                    <div class="col-sm-9">
                        <label>Causa Raiz</label>
                        <asp:TextBox ID="txtCausa" TextMode="MultiLine" runat="server" CssClass="form-control input"></asp:TextBox>
                    </div>
                    <div class="col-sm-3">
                        <label>Lider</label>
                        <asp:DropDownList ID="ddlLider" runat="server" CssClass="form-control input"></asp:DropDownList>
                    </div>
                    <div class="col-sm-2">
                        <br />
                        <asp:Button ID="btnGuardarCausa" Text="Guardar" OnClick="btnGuardarCausa_Click" CssClass="btn btn-success" runat="server" Width="100%" />
                    </div>
                    <div class="col-xs-1">
                        <asp:Label ID="lblPlan" runat="server" Visible="false"></asp:Label>
                    </div>
                    <br />
                </div>
            </div>
        </div>
        <div class="col-sm-12">
            <div class="panel panel-default">
                <div class="panel-heading">Plan de Accion</div>
                <div class="panel-body">
                    <div class="col-sm-3">
                        <label>Accion a Realizar</label>
                        <asp:TextBox ID="txtAccionRealizar" runat="server" CssClass="form-control"></asp:TextBox>
                    </div>
                    <div class="col-sm-3">
                        <label>Responsable</label>
                        <asp:DropDownList ID="ddlResponsable" runat="server" CssClass="form-control"></asp:DropDownList>
                    </div>
                    <div class="col-sm-3">
                        <label>Fecha Inicio</label>
                        <asp:TextBox ID="txtFechaInicio" TextMode="Date" runat="server" CssClass="form-control"></asp:TextBox>
                    </div>
                    <div class="col-sm-3">
                        <label>Fecha Final</label>
                        <asp:TextBox ID="txtFechaFin" TextMode="Date" runat="server" CssClass="form-control"></asp:TextBox>
                    </div>
                    <div class="col-sm-7">
                        <label>Observaciones</label>
                        <asp:TextBox ID="txtObservaciones" TextMode="MultiLine" runat="server" CssClass="form-control"></asp:TextBox>
                    </div>
                    <div class="col-sm-2">
                        <br />
                        <asp:Button ID="btnGuardar" Text="Guardar" OnClick="btnGuardar_Click" CssClass="btn btn-success" runat="server" Width="100%" />
                    </div>
                    <div class="col-sm-2">
                        <br />
                        <asp:Button ID="btnNuevo" Text="Nuevo" OnClick="btnNuevo_Click" CssClass="btn btn-warning" runat="server" Width="100%" />
                    </div>
                </div>
            </div>
        </div>
        <div class="col-md-12">
            <asp:GridView ID="gvListado" runat="server" CssClass="table table-responsive table-bordered" OnSelectedIndexChanged="gvListado_SelectedIndexChanged">
                <AlternatingRowStyle BackColor="#B5F0FF" />
                <HeaderStyle BackColor="#33CCCC" />
                <RowStyle BackColor="#CCCCCC" />
            </asp:GridView>
        </div>
    </div>
</asp:Content>
