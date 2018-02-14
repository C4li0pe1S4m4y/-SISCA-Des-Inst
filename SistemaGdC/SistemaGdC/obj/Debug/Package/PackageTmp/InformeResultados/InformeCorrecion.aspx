<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="InformeCorrecion.aspx.cs" Inherits="SistemaGdC.InformeResultados.InformeCorrecion" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <link href="~/Content/bootstrap.css" rel="stylesheet" media="screen" />
    <h2>&nbsp;</h2>
    <h2>Ingreso de Informe De Correcion</h2>
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
                        <asp:DropDownList ID="ddlEvaluacion" AutoPostBack="true" OnSelectedIndexChanged="ddlEvaluacion_SelectedIndexChanged" runat="server" CssClass="form-control"></asp:DropDownList>
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
        
        <div class="col-xs-12">
            <div class="panel panel-default">
                <div class="panel-heading">Informe</div>
                <div class="panel-body">
                    <div class="col-sm-2">
                        <label>Estado</label>
                        <asp:DropDownList ID="ddlEstado" runat="server" CssClass="form-control"></asp:DropDownList>
                    </div>
                    <div class="col-sm-10">
                        <label>Descripcion de la Accion Realizada</label>
                        <asp:TextBox ID="txtOportunidades" TextMode="MultiLine" runat="server" CssClass="form-control input"></asp:TextBox>
                    </div>
                    <div class="col-sm-11">
                        <label>Descripcion Evidencia</label>
                        <asp:TextBox ID="txtDesEvidencia" TextMode="MultiLine" runat="server" CssClass="form-control input"></asp:TextBox>
                    </div>
                    <div class="col-sm-2">
                        <label>Adjutar Evidencia</label>
                        <asp:CheckBox ID="cbxEvidencia" runat="server" Checked="false" CssClass="form-control" Width="75%" />
                    </div>
                    <div class="col-sm-3">
                        <label>Evidencia</label>
                        <asp:TextBox ID="txtRutaEvidencia" runat="server" CssClass="form-control input"></asp:TextBox>
                    </div>
                     <div class="col-sm-3">
                        <label>Lider</label>
                        <asp:DropDownList ID="ddlLider" runat="server" CssClass="form-control input"></asp:DropDownList>
                    </div>

                </div>
            </div>
        </div>
        <div class="col-sm-3">
            <asp:Button ID="btnGuardar" Text="Guardar" OnClick="btnGuardar_Click" CssClass="btn btn-success" runat="server" Width="100%" />
        </div>
        <div class="col-sm-3">
            <asp:Button ID="btnNuevo" Text="Nuevo" OnClick="btnNuevo_Click" CssClass="btn btn-warning" runat="server" Width="100%" />
        </div>
    </div>
</asp:Content>
