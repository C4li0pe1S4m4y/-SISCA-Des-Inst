<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="VerificacionInforme.aspx.cs" Inherits="SistemaGdC.Verificaciones.VerificacionInforme" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <link href="~/Content/bootstrap.css" rel="stylesheet" media="screen" />
    <h2>&nbsp;</h2>
    <h2 style="color: white"><b>Verificación de Informe de Evaluación Interna</b></h2>

    <style>
        .chart {
            width: 100%;
            height: 100%;
            min-height: 100%;
        }

        .table {
            margin-bottom: 0;
            border: 1px solid black;
            -webkit-border-radius: 8px;
            -moz-border-radius: 8px;
            border-radius: 8px;
            overflow: hidden;
            border: solid 1px #000000;
        }
    </style>
    <asp:UpdatePanel ID="update1" runat="server">
        <ContentTemplate>


            <div class="row" id="panel1" runat="server">
                <div class="col-md-12 col-xs-12">
                    <div class="panel panel-default">
                        <div class="panel-heading">Informes</div>
                        <div class="panel-body">
                            <div class="row">
                                <div class="col-md-12">
                                    <asp:GridView ID="gvListadoInformes" runat="server"
                                        AllowPaging="true" OnPageIndexChanging="gvListadoInformes_PageIndexChanging"
                                        BackColor="#fdffe6" CssClass="table table-hover table-bordered" AutoGenerateColumns="false" Width="1500px"
                                        OnRowCommand="gvListadoInformes_RowCommand" PageSize="5">
                                        <AlternatingRowStyle BackColor="#f2fffc" />
                                        <Columns>
                                            <asp:BoundField DataField="id_informe_ei" HeaderText="ID" />
                                            <asp:TemplateField ItemStyle-Width="30px" HeaderText="CustomerID">
                                                <ItemTemplate>
                                                    <asp:Button ID="btIngresar" runat="server" ControlStyle-CssClass="btn btn-info"
                                                        CommandName="Revisar" CommandArgument="<%# Container.DataItemIndex %>"
                                                        Text="Revisar" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="anio" HeaderText="Año" />
                                            <asp:BoundField DataField="no_informe" HeaderText="No. de Informe" />
                                            <asp:BoundField DataField="fecha" HeaderText="Fecha" />
                                            <asp:BoundField DataField="nombre" HeaderText="Status" />
                                        </Columns>
                                        <HeaderStyle BackColor="#33CCFF" />
                                    </asp:GridView>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <div class="row" id="panel2" runat="server">
                <div class="col-md-12">
                    <div class="panel panel-default">
                        <div class="panel-heading">Encabezado</div>
                        <div class="panel-body">
                            <div class="row">
                                <asp:Label runat="server" Visible="false" ID="lblPanel1" CssClass="label label-info"></asp:Label>
                                <div class="col-md-1">
                                    <div class="form-group">
                                        <div>
                                            <label visible="false">No.:</label>
                                        </div>
                                        <div style="margin-top: 0.3em">
                                            <asp:Label runat="server" ID="lblCorrelativo" CssClass="label label-info" Style="font-size: large"></asp:Label>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-1">
                                    <label>Año:</label>
                                    <asp:TextBox ID="txtanio" Enabled="false" CssClass="form-control input" runat="server"></asp:TextBox>
                                </div>
                                <div class="col-md-2">
                                    <label>No. de Evaluación:</label>
                                    <asp:TextBox ID="txtInforme" Enabled="false" CssClass="form-control input" runat="server"></asp:TextBox>
                                </div>
                                <div class="col-md-3">
                                    <label>Fecha Informe:</label>
                                    <asp:TextBox ID="txtFechaInforme" Enabled="false" CssClass="form-control input" TextMode="Date" runat="server"></asp:TextBox>
                                </div>
                                <div class="col-md-1">
                                    <div>
                                        <label>&nbsp;</label>
                                    </div>
                                    <div>
                                        <asp:Button ID="btnCancelar" Text="Cancelar" runat="server" CssClass="btn btn-block" OnClick="btnCancelar_Click" />
                                    </div>
                                </div>
                                <div class="col-md-2">
                                    <div>
                                        <label>&nbsp;</label>
                                    </div>
                                    <div>
                                        <asp:Button ID="btnValidar" Text="Validar" runat="server" CssClass="btn btn-success btn-block" OnClick="btnValidar_Click"
                                            OnClientClick="return confirm('¿Desea validar el Informe?');" />
                                    </div>
                                </div>
                                <div class="col-md-2">
                                    <div>
                                        <label>&nbsp;</label>
                                    </div>
                                    <div>
                                        
                                        <button type="button" class="btn btn-danger btn-block" data-toggle="modal" data-target="#myModal">Rechazar</button>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>


            <!-- Modal -->
            <div class="modal fade" id="myModal" role="dialog">
                <div class="modal-dialog">
                    <!-- Modal content-->
                    <div class="modal-content">
                        <div class="modal-header">
                            <button type="button" class="close" data-dismiss="modal">&times;</button>
                            <h4 class="modal-title">Modal Header</h4>
                        </div>
                        <div class="modal-body">
                            <label>Observaciones: </label>
                            <asp:TextBox ID="txtRechazo" Enabled="true" Width="100%" CssClass="form-control" TextMode="MultiLine" Style="height: 205px" runat="server"></asp:TextBox>
                        </div>
                        <div class="modal-footer">
                            <button type="button" class="btn btn-default" data-dismiss="modal">Cancelar</button>                            
                            <asp:Button ID="btnRechazar" Text="Rechazar" runat="server" CssClass="btn btn-danger" OnClick="btnRechazar_Click" />
                        </div>
                    </div>
                </div>
            </div>
            
            <div class="row" id="panel3" runat="server">
                <div class="col-md-12" id="pn1" runat="server">
                    <div class="panel panel-default">
                        <div class="panel-heading">Acciones</div>
                        <div class="panel-body">
                            <div class="row">
                                <div class="col-md-4">
                                    <div class="form-group">
                                        <label>Acción Generada: </label>
                                        <asp:DropDownList ID="ddlAccionGenerada" Enabled="false" CssClass="form-control input" Width="100%" runat="server" required="true"></asp:DropDownList>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-6">
                                            <div class="form-group">
                                                <label>No. de Hallazgo: </label>
                                                <asp:TextBox ID="txtHallazgo" Enabled="false" CssClass="form-control input" runat="server"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="col-md-6">
                                            <div class="form-group">
                                                <label>Punto de Norma: </label>
                                                <asp:TextBox ID="txtPuntoNorma" Enabled="false" CssClass="form-control input" runat="server"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label>Proceso Relacionado: </label>
                                        <asp:DropDownList ID="ddlProceso" Enabled="false" CssClass="form-control input" Width="100%" runat="server"></asp:DropDownList>
                                    </div>
                                    <div class="form-group">
                                        <label>Unidad: </label>
                                        <asp:DropDownList ID="ddlUnidad" Enabled="false" OnSelectedIndexChanged="ddlUnidad_SelectedIndexChanged" AutoPostBack="true" CssClass="form-control input" Width="100%" runat="server"></asp:DropDownList>
                                    </div>
                                    <div class="form-group">
                                        <label>Dependencia: </label>
                                        <asp:DropDownList ID="ddlDependencia" Enabled="false" CssClass="form-control input" Width="100%" runat="server"></asp:DropDownList>
                                    </div>
                                </div>
                                <div class="col-md-4">
                                    <div class="form-group">
                                        <label>Responsable: </label>
                                        <asp:DropDownList ID="ddlResponsable" Enabled="false" CssClass="form-control input" Width="100%" runat="server"></asp:DropDownList>
                                    </div>
                                    <div class="form-group">
                                        <label>Analista: </label>
                                        <asp:DropDownList ID="ddlAnalista" Enabled="false" CssClass="form-control input" Width="100%" runat="server"></asp:DropDownList>
                                    </div>
                                    <div class="form-group">
                                        <label>Tipo Acción/Informe: </label>
                                        <asp:DropDownList ID="dllTipoAccion" Enabled="false" CssClass="form-control input" Width="100%" runat="server"></asp:DropDownList>
                                    </div>
                                    <div class="form-group">
                                        <label>Fecha Recepción: </label>
                                        <asp:TextBox ID="txtFechaRecepcion" Enabled="false" CssClass="form-control input" TextMode="Date" runat="server"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="col-md-4">
                                    <div class="form-group">
                                        <label>Descripción: </label>
                                        <asp:TextBox ID="txtDescripcion" Enabled="false" Width="100%" CssClass="form-control" TextMode="MultiLine" Style="height: 205px" runat="server"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                            <div class="row">

                                <div class="col-md-12" style="overflow: auto; height: 100%">
                                    <asp:GridView ID="gvListadoAcciones" runat="server" DataKeyNames="Correlativo"
                                        AllowPaging="true" OnPageIndexChanging="gvListadoAcciones_PageIndexChanging" PageSize="3"
                                        BackColor="#fdffe6" CssClass="table table-hover table-bordered" AutoGenerateColumns="false" Width="1500px"
                                        OnRowCommand="gvListadoAcciones_RowCommand">
                                        <AlternatingRowStyle BackColor="#f2fffc" />
                                        <Columns>
                                            <asp:BoundField DataField="id" HeaderText="ID" />
                                            <asp:TemplateField ItemStyle-Width="30px" HeaderText="">
                                                <ItemTemplate>
                                                    <asp:Button ID="btIngresar" runat="server" ControlStyle-CssClass="btn btn-info"
                                                        CommandName="Ver" CommandArgument="<%# Container.DataItemIndex %>"
                                                        Text="Ver" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="Acción" HeaderText="Acción" />
                                            <asp:BoundField DataField="Correlativo" HeaderText="Correlativo" />
                                            <asp:BoundField DataField="Status" HeaderText="Status" />
                                            <asp:BoundField DataField="Punto de Norma" HeaderText="Punto de Norma" />
                                            <asp:BoundField DataField="Proceso" HeaderText="Proceso" />
                                            <asp:BoundField DataField="Dependencia" HeaderText="Dependencia" />
                                            <asp:BoundField DataField="Descripción" HeaderText="Descripción" />
                                            <asp:BoundField DataField="Enlace" HeaderText="Enlace" />
                                            <asp:BoundField DataField="Tipo Acción" HeaderText="Tipo Acción" />
                                        </Columns>
                                        <HeaderStyle BackColor="#33CCFF" />
                                    </asp:GridView>
                                </div>
                                <asp:Label runat="server" ID="lblCorr"></asp:Label>

                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
