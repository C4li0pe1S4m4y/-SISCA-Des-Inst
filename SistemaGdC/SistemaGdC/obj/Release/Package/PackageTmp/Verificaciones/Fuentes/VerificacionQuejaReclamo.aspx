<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="VerificacionQuejaReclamo.aspx.cs" Inherits="SistemaGdC.Verificaciones.Fuentes.VerificacionQuejaReclamo" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <link href="~/Content/bootstrap.css" rel="stylesheet" media="screen" />
    <h2>&nbsp;</h2>
    <h2 style="color: white"><b>Verificación de Queja o Reclamo</b></h2>

    <style type="text/css">
        .chart {
            width: 100%;
            panel1 .Visible = false;
            panel2 .Visible = true;
            panel3 .Visible = true;
            panel4 .Visible = false;
            mostrarBotones(false);
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

        #Background {
            position: fixed;
            top: 0px;
            bottom: 0px;
            left: 0px;
            right: 0px;
            overflow: hidden;
            padding: 0;
            margin: 0;
            background-color: #F0F0F0;
            filter: alpha(opacity=80);
            opacity: 0.8;
            z-index: 100000;
        }

        #Progress {
            position: fixed;
            top: 40%;
            left: 25%;
            height: 20%;
            width: 50%;
            z-index: 100001;

            background-image: url(../Content/loading.gif);
            background-repeat: no-repeat;
            background-position: center;
        }
    </style>

    <script type="text/javascript">
        function closeMaccion() {
            $(".modal-fade").modal("hide");
            $(".modal-backdrop").remove();
            
            $('#mRechazarUnaAccion').modal('hide');
            $('#mRechazarTodasAcciones').modal('hide');
        }
    </script>
    <asp:UpdatePanel ID="update1" runat="server">
        <ContentTemplate>

            <div class="row" id="panel1" runat="server">
                <div class="col-md-12 col-xs-12">
                    <div class="panel panel-default">
                        <div class="panel-heading">Informes</div>
                        <div class="panel-body">
                            <div class="row">
                                <div class="col-md-12" style="overflow: auto; height: 100%">
                                    <asp:GridView ID="gvListadoInformes" runat="server"
                                        AllowPaging="true" OnPageIndexChanging="gvListadoInformes_PageIndexChanging"
                                        BackColor="#fdffe6" CssClass="table table-hover table-bordered" AutoGenerateColumns="false" Width="100%"
                                        OnRowCommand="gvListadoInformes_RowCommand" PageSize="5">
                                        <AlternatingRowStyle BackColor="#f2fffc" />
                                        <Columns>
                                            <asp:BoundField DataField="id_fuente" HeaderText="ID" />
                                            <asp:TemplateField ItemStyle-Width="30px" HeaderText="CustomerID">
                                                <ItemTemplate>
                                                    <asp:Button ID="btnRevisar" runat="server" ControlStyle-CssClass="btn btn-info"
                                                        CommandName="Revisar" CommandArgument="<%# Container.DataItemIndex %>"
                                                        Text="Revisar" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="anio" HeaderText="Año" />
                                            <asp:BoundField DataField="no_queja" HeaderText="No. de Informe" />
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
                                        <asp:Button ID="btnCancelar" Text="Regresar" runat="server" CssClass="btn btn-warning btn-block" OnClick="btnCancelar_Click" />
                                    </div>
                                </div>
                                <div class="col-md-2">
                                    <div>
                                        <label>&nbsp;</label>
                                    </div>
                                    <div>
                                        <asp:Button ID="btnValidarTodo" Text="Validar Pends." runat="server" CssClass="btn btn-success btn-block" OnClick="btnValidarTodo_Click"
                                            OnClientClick="return confirm('¿Desea validar todas las Acciones pendientes?');" />
                                    </div>
                                </div>
                                <div class="col-md-2">
                                    <div>
                                        <label>&nbsp;</label>
                                    </div>
                                    <div>
                                        <%--<asp:Button ID="btnRechazarTodo" Text="Rechazar Pends." runat="server" CssClass="btn btn-danger btn-block"
                                            OnClick="btnRechazarTodo_Click" OnClientClick="return confirm('¿Desea rechazar todas las Acciones pendientes?');" />--%>
                                        <asp:Button ID="btnRechazarTodo" Text="Rechazar Pends." runat="server" CssClass="btn btn-danger btn-block"
                                            data-toggle="modal" data-target="#mRechazarTodasAcciones"/>
                                    </div>
                                </div>
                            </div>
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
                                <div class="col-md-12" style="height: 100%">
                                    <asp:GridView ID="gvListadoAcciones" runat="server" DataKeyNames="id"
                                        AllowPaging="true" OnPageIndexChanging="gvListadoAcciones_PageIndexChanging" PageSize="3"
                                        BackColor="#fdffe6" CssClass="table table-hover table-bordered" AutoGenerateColumns="false" Width="100%"
                                        OnRowCommand="gvListadoAcciones_RowCommand" OnRowDataBound="gvListado_RowDataBound">
                                        <AlternatingRowStyle BackColor="#f2fffc" />
                                        <Columns>
                                            <asp:BoundField DataField="id" HeaderText="ID" />
                                            <asp:TemplateField ItemStyle-Width="30px" HeaderText="">
                                                <ItemTemplate>
                                                    <asp:Button ID="btnRevisar" CausesValidation="false" runat="server" CssClass="btn btn-info"
                                                        CommandName="Ver" CommandArgument="<%# Container.DataItemIndex %>"
                                                        Text="Ver" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="Status" HeaderText="Status" />
                                            <asp:BoundField DataField="Federación" HeaderText="Federación" />
                                            <asp:BoundField DataField="Instalación" HeaderText="Instalación" />                                            
                                            <asp:BoundField DataField="Proceso" HeaderText="Proceso" />
                                            <asp:BoundField DataField="Dependencia" HeaderText="Dependencia" />
                                            <asp:BoundField DataField="Descripción" HeaderText="Descripción" />
                                            <asp:BoundField DataField="Enlace" HeaderText="Enlace" />
                                            <asp:BoundField DataField="Tipo Acción" HeaderText="Tipo Acción" />
                                            <asp:BoundField DataField="aprobado" HeaderText="Aprobado" />
                                        </Columns>
                                        <HeaderStyle BackColor="#33CCFF" />
                                    </asp:GridView>
                                </div>
                                <asp:Label runat="server" ID="lblCorr"></asp:Label>
                            </div>

                            <div class="row" id="panel4" runat="server">
                                </br>
                                </br>
                                <div class="col-md-4">
                                    <div class="form-group">
                                        <label>FADN: </label>
                                        <asp:DropDownList ID="ddlFadn" CssClass="form-control input" Width="100%" runat="server" Enabled="true"></asp:DropDownList>
                                        <asp:RequiredFieldValidator ValidationGroup="validar" ID="RFVddlFadn" Style="color: red;" SetFocusOnError="true" ControlToValidate="ddlFadn" InitialValue="0" runat="server" ErrorMessage="Seleccione FADN." Display="Dynamic" />
                                    </div>
                                    <div class="form-group">
                                        <label>Instalación:</label>
                                        <asp:TextBox ID="txtInstalacion" CssClass="form-control input" runat="server"></asp:TextBox>
                                        <asp:RequiredFieldValidator ValidationGroup="validar" ID="RFVtxtInstalacion" Style="color: red;" SetFocusOnError="true" ControlToValidate="txtInstalacion" InitialValue="" runat="server" ErrorMessage="Ingrese Instalación." Display="Dynamic" />
                                    </div>
                                    <div class="form-group">
                                        <label>Proceso Relacionado: </label>
                                        <asp:DropDownList ID="ddlProceso" CssClass="form-control input" Width="100%" runat="server" Enabled="true"></asp:DropDownList>
                                        <asp:RequiredFieldValidator ValidationGroup="validar" ID="RFVddlProceso" Style="color: red;" SetFocusOnError="true" ControlToValidate="ddlProceso" InitialValue="0" runat="server" ErrorMessage="Seleccione Proceso." Display="Dynamic" />
                                    </div>
                                    <div class="form-group">
                                        <label>Unidad: </label>
                                        <asp:DropDownList ID="ddlUnidad" OnSelectedIndexChanged="ddlUnidad_SelectedIndexChanged" AutoPostBack="true" CssClass="form-control input" Width="100%" runat="server" Enabled="true"></asp:DropDownList>
                                        <asp:RequiredFieldValidator ValidationGroup="validar" ID="RFVddlUnidad" Style="color: red;" SetFocusOnError="true" ControlToValidate="ddlUnidad" InitialValue="0" runat="server" ErrorMessage="Seleccione Unidad." Display="Dynamic" />
                                    </div>
                                    <div class="form-group">
                                        <label>Dependencia: </label>
                                        <asp:DropDownList ID="ddlDependencia" OnSelectedIndexChanged="ddlDependencia_SelectedIndexChanged" AutoPostBack="true" CssClass="form-control input" Width="100%" runat="server" Enabled="true"></asp:DropDownList>
                                        <asp:RequiredFieldValidator ValidationGroup="validar" ID="RFVddlDependencia" Style="color: red;" SetFocusOnError="true" ControlToValidate="ddlDependencia" InitialValue="0" runat="server" ErrorMessage="Seleccione Dependencia." Display="Dynamic" />
                                    </div>
                                </div>                                
                                <div class="col-md-4">
                                    <div class="form-group">
                                        <label>Enlace: </label>
                                        <asp:DropDownList ID="ddlEnlace" CssClass="form-control input" Width="100%" runat="server" Enabled="true"></asp:DropDownList>
                                        <asp:RequiredFieldValidator ValidationGroup="validar" ID="RFVddlEnlace" Style="color: red;" SetFocusOnError="true" ControlToValidate="ddlEnlace" InitialValue="0" runat="server" ErrorMessage="Seleccione Enlace." Display="Dynamic" />
                                    </div>
                                    <div class="form-group">
                                        <label>Líder de Unidad: </label>
                                        <asp:DropDownList ID="ddlLider" CssClass="form-control input" Width="100%" runat="server" Enabled="true"></asp:DropDownList>
                                        <asp:RequiredFieldValidator ValidationGroup="validar" ID="RFVddlLider" Style="color: red;" SetFocusOnError="true" ControlToValidate="ddlLider" InitialValue="0" runat="server" ErrorMessage="Seleccione Líder." Display="Dynamic" />
                                    </div>
                                    <div class="form-group">
                                        <label>Analista: </label>
                                        <asp:TextBox ID="txtAnalista" Enabled="false" CssClass="form-control input" runat="server"></asp:TextBox>
                                    </div>
                                    <div class="form-group">
                                        <label>Tipo Acción/Informe: </label>
                                        <asp:DropDownList ID="dllTipoAccion" CssClass="form-control input" Width="100%" runat="server" Enabled="true"></asp:DropDownList>
                                        <asp:RequiredFieldValidator ValidationGroup="validar" ID="RFVdllTipoAccion" Style="color: red;" SetFocusOnError="true" ControlToValidate="dllTipoAccion" InitialValue="0" runat="server" ErrorMessage="Seleccione Tipo de Acción." Display="Dynamic" />
                                    </div>
                                </div>
                                <div class="col-md-4">
                                    <div class="row">
                                        <div class="form-group">
                                            <div class="col-md-12">
                                                <label>Descripción: </label>
                                                <asp:TextBox ID="txtDescripcion" Width="100%" CssClass="form-control" TextMode="MultiLine" Style="height: 205px" runat="server"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                    </div>
                                    <div class="row">
                                        <div class="col-md-6">
                                            <div>
                                                <label>&nbsp;</label>
                                            </div>
                                            <div>
                                                <asp:Button ID="btnActualizar" Text="Actualizar" runat="server" CssClass="btn btn-info btn-block" OnClick="btnActualizar_Click"
                                                    OnClientClick="return confirm('¿Desea actualizar la Acción?');" />
                                            </div>
                                        </div>
                                        <div class="col-md-2">
                                            <div>
                                                <label>&nbsp;</label>
                                            </div>
                                            <asp:LinkButton ID="btnValidar" OnClick="btnValidar_Click" CssClass="btn btn-success" runat="server"
                                                OnClientClick="return confirm('¿Desea validar la Acción seleccionada?');"><span class="glyphicon glyphicon-ok"/></asp:LinkButton>
                                        </div>
                                        <div class="col-md-2">
                                            <div>
                                                <label>&nbsp;</label>
                                            </div>
                                            <asp:LinkButton ID="btnRechazar" CssClass="btn btn-danger" runat="server"
                                                data-toggle="modal" data-target="#mRechazarUnaAccion"><span class="glyphicon glyphicon-remove"/></asp:LinkButton>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <!-- Modal -->
                            <div class="modal fade" id="mRechazarUnaAccion" role="dialog">
                                <div class="modal-dialog">
                                    <!-- Modal content-->
                                    <div class="modal-content">
                                        <div class="modal-header">
                                            <button type="button" class="close" data-dismiss="modal">&times;</button>
                                            <h4 class="modal-title">¿Desea rechazar la Acción?</h4>
                                        </div>
                                        <div class="modal-body">
                                            <label>Observaciones: </label>
                                            <asp:TextBox ID="txtRechazoAccion" Enabled="true" Width="100%" CssClass="form-control" TextMode="MultiLine" Style="height: 205px" runat="server"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="RFVtxtRechazarAccion" ValidationGroup="rechazarAccion" Style="color: red;" SetFocusOnError="true" ControlToValidate="txtRechazoAccion" InitialValue="" runat="server" ErrorMessage="Por favor agregue una observación." Display="Dynamic" />
                                        </div>
                                        <div class="modal-footer">
                                            <button type="button" class="btn btn-default" data-dismiss="modal">Cancelar</button>
                                            <asp:Button ID="btnRechazoAccion" ValidationGroup="rechazarAccion" Text="Rechazar" runat="server" CssClass="btn btn-danger" OnClientClick="return closeMaccion();" OnClick="btnRechazar_Click" />
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <!-- Modal -->
                            <div class="modal fade" id="mRechazarTodasAcciones" role="dialog">
                                <div class="modal-dialog">
                                    <!-- Modal content-->
                                    <div class="modal-content">
                                        <div class="modal-header">
                                            <button type="button" class="close" data-dismiss="modal">&times;</button>
                                            <h4 class="modal-title">¿Desea rechazar la Acción?</h4>
                                        </div>
                                        <div class="modal-body">
                                            <label>Observaciones: </label>
                                            <asp:TextBox ID="txtRechazoAccionTodo" Enabled="true" Width="100%" CssClass="form-control" TextMode="MultiLine" Style="height: 205px" runat="server"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="RFVtxtRechazarAccionTodo" ValidationGroup="rechazarAccionTodo" Style="color: red;" SetFocusOnError="true" ControlToValidate="txtRechazoAccionTodo" InitialValue="" runat="server" ErrorMessage="Por favor agregue una observación." Display="Dynamic" />
                                        </div>
                                        <div class="modal-footer">
                                            <button type="button" class="btn btn-default" data-dismiss="modal">Cancelar</button>
                                            <asp:Button ID="btnRechazoAccionTodo" ValidationGroup="rechazarAccion" Text="Rechazar" runat="server" CssClass="btn btn-danger" OnClientClick="return closeMaccion();" OnClick="btnRechazarTodo_Click" />
                                        </div>
                                    </div>
                                </div>
                            </div>

                        </div>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="update1">
        <ProgressTemplate>
            <div id="Background"></div>
            <div id="Progress">
                <h6>
                    <p style="text-align: center">
                        <b>Cargando...
                        <br />
                        </b>
                    </p>
                </h6>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>
</asp:Content>
