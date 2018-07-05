<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="SeguimientoPlanAccion.aspx.cs" Inherits="SistemaGdC.Seguimientos.SeguimientoPlanAccion" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h2>&nbsp;</h2>
    <h2 style="color: white"><b>Seguimiento Plan de Acción</b></h2>

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
    <script type="text/javascript">
        function closeMactividad() {
            console.log(document.getElementById("<%=RFVtxtRechazo.ClientID %>").isvalid)
            if (document.getElementById("<%=RFVtxtRechazo.ClientID %>").isvalid)
            $('#myModalActividad').modal('hide');
        }

        function closeMeficacia() {
            $('#myModalEficacia').modal('hide');
        }
    </script>


    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div class="row">
                <div class="col-md-12" id="pn1" runat="server">


                    <div class="panel panel-default">
                        <div class="panel-heading">Acción</div>
                        <div class="panel-body">
                            <div class="row" id="panel1" runat="server">
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
                                            <label>No. de Hallazgo:</label>
                                            <asp:TextBox ID="txtHallazgo" Enabled="false" AutoPostBack="true" runat="server" CssClass="form-control"></asp:TextBox>
                                        </div>
                                        <div class="col-md-4">
                                            <label>No. de Evaluación:</label>
                                            <asp:TextBox ID="txtEvaluacion" Enabled="false" AutoPostBack="true" runat="server" CssClass="form-control"></asp:TextBox>
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
                            <div class="row" id="panel2" runat="server">
                                <div class="col-md-12">
                                    <div class="col-md-12" style="overflow: auto; height: 100%">
                                        <asp:GridView ID="gvListadoAcciones" runat="server" DataKeyNames="Correlativo"
                                            BackColor="#fdffe6" CssClass="table table-hover table-bordered" AutoGenerateColumns="false" Width="1500px"
                                            OnRowCommand="gvListadoAcciones_RowCommand">
                                            <AlternatingRowStyle BackColor="#f2fffc" />
                                            <Columns>
                                                <asp:BoundField DataField="id" HeaderText="ID" />
                                                <asp:TemplateField ItemStyle-Width="30px" HeaderText="">
                                                    <ItemTemplate>
                                                        <asp:Button ID="btVer" runat="server" ControlStyle-CssClass="btn btn-info"
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
                                </div>
                            </div>
                        </div>
                    </div>



                </div>
            </div>
            <div class="row" id="panel3" runat="server">
                <div class="col-md-6">
                    <div class="panel panel-default">
                        <div class="panel-heading">Causa Raíz</div>
                        <div class="panel-body">
                            <div class="row">
                                <div class="col-md-5">
                                    <div class="form-group">
                                        <label>Técnica de Análisis:</label>
                                        <asp:DropDownList ID="ddlTecnicaAnalisis" runat="server" CssClass="form-control input"></asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-12">
                                    <label>Causa Raíz:</label>
                                    <asp:TextBox ID="txtCausa" TextMode="MultiLine" runat="server" CssClass="form-control input" Style="height: 140px"></asp:TextBox>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-12">
                                    <br />
                                    <asp:FileUpload ID="FileEficacia" Visible="false" CssClass="btn btn-primary btn-sm" runat="server" Width="100%" />
                                </div>
                            </div>
                            <div class="col-md-4">
                                <br />
                                <asp:Button ID="btnCancelar" OnClick="btnCancelar_Click" Text="Cancelar" CssClass="btn" runat="server" Width="100%" />
                            </div>
                            <div class="col-md-4">
                                <br />
                                <asp:Button ID="btnAdjuntarEficacia" OnClick="btnAdjuntarEficacia_Click" Text="Adj. Eficacia" CssClass="btn btn-primary" runat="server" Width="100%" />
                                <asp:Button ID="btnDescargarEficacia" OnClick="btnDescargarEficacia_Click" Text="Eficacia" CssClass="btn btn-primary" runat="server" Width="100%" />
                            </div>
                            <div class="col-md-1">
                                <br />
                                <asp:LinkButton ID="btnValidarEficacia" OnClick="btnValidarEficacia_Click" CssClass="btn btn-success" runat="server"
                                    OnClientClick="return confirm('¿Desea validar la Eficacia del Plan de Acción?');"><span class="glyphicon glyphicon-ok"/></asp:LinkButton>
                            </div>
                            <div class="col-md-1">
                                <br />
                                <asp:LinkButton ID="btnRechazarEficacia" CssClass="btn btn-danger" runat="server"
                                    data-toggle="modal" data-target="#myModalEficacia"><span class="glyphicon glyphicon-remove"/></asp:LinkButton>
                            </div>
                        </div>
                    </div>
                </div>

                <!-- Modal -->
                <div class="modal fade" id="myModalEficacia" role="dialog">
                    <div class="modal-dialog">
                        <!-- Modal content-->
                        <div class="modal-content">
                            <div class="modal-header">
                                <button type="button" class="close" data-dismiss="modal">&times;</button>
                                <h4 class="modal-title">¿Desea rechazar la Eficacia del Plan de Acción?</h4>
                            </div>
                            <div class="modal-body">
                                <label>Observaciones: </label>
                                <asp:TextBox ID="txtRechazOEficacia" Enabled="true" Width="100%" CssClass="form-control" TextMode="MultiLine" Style="height: 205px" runat="server"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RFVtxtRechazarEficacia" ValidationGroup="rechazarEficacia" Style="color: red;" SetFocusOnError="true" ControlToValidate="txtRechazOEficacia" InitialValue="" runat="server" ErrorMessage="Por favor agregue una observación." Display="Dynamic" />
                            </div>
                            <div class="modal-footer">
                                <button type="button" class="btn btn-default" data-dismiss="modal">Cancelar</button>
                                <asp:Button ID="btnRechazoEficacia" ValidationGroup="rechazarEficacia" Text="Rechazar" runat="server" CssClass="btn btn-danger" OnClientClick="return closeMeficacia();" OnClick="btnRechazarEficacia_Click" />
                            </div>
                        </div>
                    </div>
                </div>

                <div class="col-md-6">
                    <div class="panel panel-default">
                        <div class="panel-heading">Actividad</div>
                        <div class="panel-body">
                            <div class="row">
                                <div class="col-md-12">
                                    <div class="row">
                                        <div class="col-md-2">
                                            <div class="form-group">
                                                <label>ID:</label>
                                                <asp:TextBox ID="txtIdActividad" Enabled="false" CssClass="form-control" runat="server" required=""></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="col-md-10">
                                            <label>Responsable:</label>
                                            <asp:TextBox ID="txtResponsableAct" Enabled="false" CssClass="form-control" runat="server" required=""></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-7">
                                            <div class="form-group">
                                                <label>Actividad:</label>
                                                <asp:TextBox ID="txtActividad" TextMode="MultiLine" Enabled="false" runat="server" CssClass="form-control input" Style="height: 110px"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="col-md-5">
                                            <div class="form-group">
                                                <label>Fecha Inicio:</label>
                                                <asp:TextBox ID="txtFechaInicioActividad" Enabled="false" CssClass="form-control" TextMode="Date" runat="server" required=""></asp:TextBox>
                                            </div>
                                            <div class="form-group">
                                                <label>Fecha Fin:</label>
                                                <asp:TextBox ID="txtFechaFinActividad" Enabled="false" CssClass="form-control" TextMode="Date" runat="server"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-12">
                                            <label>Observaciones:</label>
                                            <asp:TextBox ID="txtObservacionAct" TextMode="MultiLine" Enabled="true" runat="server" CssClass="form-control input" Style="height: 100px"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="row" id="panelBtEnlace" runat="server">
                                        <div class="row">
                                            <div class="col-md-12">
                                                <div class="col-md-12">
                                                    <br />
                                                    <asp:FileUpload ID="FileEvidencia" CssClass="btn btn-primary btn-sm" runat="server" Width="100%" />
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-md-12">
                                                <div class="col-md-4">
                                                    <br />
                                                    <asp:Button ID="btnFinalizar" Text="Finalizar" CssClass="btn btn-success" OnClick="btnFinalizar_Click" runat="server" Width="100%"
                                                        OnClientClick="return confirm('¿Desea finalizar la Actividad?');" />
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row" id="panelBtAnalista" runat="server">
                                        <div class="col-md-4">
                                            <br />
                                            <asp:Button ID="btnEvidencia" Text="Evidencia" CssClass="btn btn-primary" OnClick="btnDescargar_Click" runat="server" Width="100%" />
                                        </div>
                                        <div class="col-md-5"></div>
                                        <div class="col-md-1">
                                            <br />
                                            <asp:LinkButton ID="btnValidar" OnClick="btnValidar_Click" CssClass="btn btn-success" runat="server"
                                                OnClientClick="return confirm('¿Desea validar la Actividad?');"><span class="glyphicon glyphicon-ok"/></asp:LinkButton>
                                        </div>
                                        <div class="col-md-1">
                                            <br />
                                            <asp:LinkButton ID="btnRechazar" CssClass="btn btn-danger" runat="server"
                                                data-toggle="modal" data-target="#myModalActividad"><span class="glyphicon glyphicon-remove"/></asp:LinkButton>
                                        </div>
                                    </div>

                                    <!-- Modal -->
                                    <div class="modal fade" id="myModalActividad" role="dialog">
                                        <div class="modal-dialog">
                                            <!-- Modal content-->
                                            <div class="modal-content">
                                                <div class="modal-header">
                                                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                                                    <h4 class="modal-title">¿Desea rechazar la Actividad?</h4>
                                                </div>
                                                <div class="modal-body">
                                                    <label>Observaciones: </label>
                                                    <asp:TextBox ID="txtRechazo" Enabled="true" Width="100%" CssClass="form-control" TextMode="MultiLine" Style="height: 205px" runat="server"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ValidationGroup="rechazarActividad" ID="RFVtxtRechazo" Style="color: red;" SetFocusOnError="true" ControlToValidate="txtRechazo" InitialValue="" runat="server" ErrorMessage="Ingrese No. Hallazgo." Display="Dynamic" />                                                    
                                                </div>
                                                <div class="modal-footer">
                                                    <button type="button" class="btn btn-default" data-dismiss="modal">Cancelar</button>
                                                    <asp:Button ID="btnModalRechazar" ValidationGroup="validar" Text="Rechazar" runat="server" CssClass="btn btn-danger" CausesValidation="true" OnClientClick="return closeMactividad();" OnClick="btnRechazar_Click" />
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row" id="panel4" runat="server">
                <div class="col-md-12">
                    <div class="panel panel-warning">
                        <div class="panel-heading">Lista de Actividades Pendientes </div>
                        <div class="panel-body">
                            <div class="row">
                                <div class="col-md-12" style="overflow: auto; height: 100%">
                                    <asp:GridView ID="gvListadoActividadesPendientes" runat="server" DataKeyNames="No."
                                        BackColor="#fdffe6" CssClass="table table-hover table-bordered" AutoGenerateColumns="false" Width="1500px"
                                        OnRowCommand="gvListadoActividades_RowCommand">
                                        <AlternatingRowStyle BackColor="#f2fffc" />
                                        <Columns>
                                            <asp:BoundField DataField="No." HeaderText="ID" />
                                            <asp:TemplateField ItemStyle-Width="30px" HeaderText=" ">
                                                <ItemTemplate>
                                                    <asp:Button ID="btVer" runat="server" ControlStyle-CssClass="btn btn-info"
                                                        CommandName="VerPendientes" CommandArgument="<%# Container.DataItemIndex %>"
                                                        Text="Ver" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="accion" HeaderText="Actividad" />
                                            <asp:BoundField DataField="responsable" HeaderText="Responsable" />
                                            <asp:BoundField DataField="Fecha Inicio" HeaderText="Fecha Inicio" />
                                            <asp:BoundField DataField="Fecha Fin" HeaderText="Fecha Fin" />
                                            <asp:BoundField DataField="observaciones" HeaderText="Observaciones" />
                                        </Columns>
                                        <HeaderStyle BackColor="#33CCFF" />
                                    </asp:GridView>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-md-12">
                    <div class="panel panel-info">
                        <div class="panel-heading">Lista de Actividades Terminadas </div>
                        <div class="panel-body">
                            <div class="row">
                                <div class="col-md-12" style="overflow: auto; height: 100%">
                                    <asp:GridView ID="gvListadoActividadesTerminadas" runat="server" DataKeyNames="No."
                                        BackColor="#fdffe6" CssClass="table table-hover table-bordered" AutoGenerateColumns="false" Width="1500px"
                                        OnRowCommand="gvListadoActividades_RowCommand">
                                        <AlternatingRowStyle BackColor="#f2fffc" />
                                        <Columns>
                                            <asp:BoundField DataField="No." HeaderText="ID" />
                                            <asp:TemplateField ItemStyle-Width="30px" HeaderText=" ">
                                                <ItemTemplate>
                                                    <asp:Button ID="btVer" runat="server" ControlStyle-CssClass="btn btn-info"
                                                        CommandName="VerTerminadas" CommandArgument="<%# Container.DataItemIndex %>"
                                                        Text="Ver" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="accion" HeaderText="Actividad" />
                                            <asp:BoundField DataField="responsable" HeaderText="Responsable" />
                                            <asp:BoundField DataField="Fecha Inicio" HeaderText="Fecha Inicio" />
                                            <asp:BoundField DataField="Fecha Fin" HeaderText="Fecha Fin" />
                                            <asp:BoundField DataField="observaciones" HeaderText="Observaciones" />
                                        </Columns>
                                        <HeaderStyle BackColor="#33CCFF" />
                                    </asp:GridView>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-md-12">
                    <div class="panel panel-success">
                        <div class="panel-heading">Lista de Actividades Validadas </div>
                        <div class="panel-body">
                            <div class="row">
                                <div class="col-md-12" style="overflow: auto; height: 100%">
                                    <asp:GridView ID="gvListadoActividadesValidadas" runat="server" DataKeyNames="No."
                                        BackColor="#fdffe6" CssClass="table table-hover table-bordered" AutoGenerateColumns="false" Width="1500px"
                                        OnRowCommand="gvListadoActividades_RowCommand">
                                        <AlternatingRowStyle BackColor="#f2fffc" />
                                        <Columns>
                                            <asp:BoundField DataField="No." HeaderText="ID" />
                                            <asp:TemplateField ItemStyle-Width="30px" HeaderText=" ">
                                                <ItemTemplate>
                                                    <asp:Button ID="btVer" runat="server" ControlStyle-CssClass="btn btn-info"
                                                        CommandName="VerValidadas" CommandArgument="<%# Container.DataItemIndex %>"
                                                        Text="Ver" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="accion" HeaderText="Actividad" />
                                            <asp:BoundField DataField="responsable" HeaderText="Responsable" />
                                            <asp:BoundField DataField="Fecha Inicio" HeaderText="Fecha Inicio" />
                                            <asp:BoundField DataField="Fecha Fin" HeaderText="Fecha Fin" />
                                            <asp:BoundField DataField="observaciones" HeaderText="Observaciones" />
                                        </Columns>
                                        <HeaderStyle BackColor="#33CCFF" />
                                    </asp:GridView>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-md-12">
                    <div class="panel panel-danger">
                        <div class="panel-heading">Lista de Actividades Rechazadas </div>
                        <div class="panel-body">
                            <div class="row">
                                <div class="col-md-12" style="overflow: auto; height: 100%">
                                    <asp:GridView ID="gvListadoActividadesRechazadas" runat="server" DataKeyNames="No."
                                        BackColor="#fdffe6" CssClass="table table-hover table-bordered" AutoGenerateColumns="false" Width="1500px"
                                        OnRowCommand="gvListadoActividades_RowCommand">
                                        <AlternatingRowStyle BackColor="#f2fffc" />
                                        <Columns>
                                            <asp:BoundField DataField="No." HeaderText="ID" />
                                            <asp:TemplateField ItemStyle-Width="30px" HeaderText=" ">
                                                <ItemTemplate>
                                                    <asp:Button ID="btVer" runat="server" ControlStyle-CssClass="btn btn-info"
                                                        CommandName="VerRechazadas" CommandArgument="<%# Container.DataItemIndex %>"
                                                        Text="Ver" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="accion" HeaderText="Actividad" />
                                            <asp:BoundField DataField="responsable" HeaderText="Responsable" />
                                            <asp:BoundField DataField="Fecha Inicio" HeaderText="Fecha Inicio" />
                                            <asp:BoundField DataField="Fecha Fin" HeaderText="Fecha Fin" />
                                            <asp:BoundField DataField="observaciones" HeaderText="Observaciones" />
                                        </Columns>
                                        <HeaderStyle BackColor="#33CCFF" />
                                    </asp:GridView>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnEvidencia" />
        </Triggers>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnFinalizar" />
        </Triggers>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnAdjuntarEficacia" />
        </Triggers>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnDescargarEficacia" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
