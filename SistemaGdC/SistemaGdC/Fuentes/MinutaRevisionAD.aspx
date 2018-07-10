<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MinutaRevisionAD.aspx.cs" Inherits="SistemaGdC.Informe.MinutaRevisionAD" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h2>&nbsp;</h2>
    <h2 style="color: white"><b>Minuta de Revisión de la Alta Dirección</b></h2>

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

    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div class="row">
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
                                    <asp:TextBox ID="txtanio" CssClass="form-control input" runat="server" OnTextChanged="txtanio_TextChanged" AutoPostBack="true"></asp:TextBox>
                                </div>
                                <div class="col-md-3">
                                    <label>No. de Evaluación:</label>
                                    <asp:TextBox ID="txtInforme" CssClass="form-control input" runat="server"></asp:TextBox>
                                </div>
                                <div class="col-md-3">
                                    <label>Fecha Informe:</label>
                                    <asp:TextBox ID="txtFechaInforme" CssClass="form-control input" TextMode="Date" runat="server"></asp:TextBox>
                                </div>
                                <div class="col-md-2">
                                    <div>
                                        <label>&nbsp;</label>
                                    </div>
                                    <div>
                                        <asp:Button ID="btnGuardarEncabezado" OnClick="btnGuardarEncabezado_Click" Text="Almacenar" runat="server" CssClass="btn btn-success btn-block" />
                                    </div>
                                </div>
                                <div class="col-md-2">
                                    <div>
                                        <label>&nbsp;</label>
                                    </div>
                                    <div>
                                        <asp:Button ID="btnBuscarEncabezado" OnClick="btnBuscarEncabezado_Click" Text="Buscar" runat="server" CssClass="btn btn-info btn-block" />
                                    </div>
                                </div>
                            </div>

                        </div>
                    </div>
                </div>
                <div class="col-md-12" id="pn1" runat="server">
                    <div class="panel panel-default">
                        <div class="panel-heading">Acciones</div>
                        <div class="panel-body">
                            <div class="row">
                                <div class="col-md-4">
                                    <div class="row">
                                        <div class="col-md-6">
                                            <div class="form-group">
                                                <label>No. de Compromiso: </label>
                                                <asp:TextBox ID="txtCompromiso" CssClass="form-control input" runat="server"></asp:TextBox>
                                                <asp:RequiredFieldValidator ValidationGroup="validar" ID="RFVtxtCompromiso" Style="color: red;" SetFocusOnError="true" ControlToValidate="txtCompromiso" InitialValue="" runat="server" ErrorMessage="Ingrese No. Compromiso." Display="Dynamic" />
                                            </div>
                                        </div>
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
                                    <!--
                                    <div class="form-group">
                                        <label>No. Plan de Acción/Informe: </label>
                                        <asp:TextBox ID="txtNoPlanAccion" CssClass="form-control input" runat="server"></asp:TextBox>
                                    </div>-->
                                </div>
                                <div class="col-md-4">
                                    <div class="form-group">
                                        <label>Descripción: </label>
                                        <asp:TextBox ID="txtDescripcion" Width="100%" CssClass="form-control" TextMode="MultiLine" Style="height: 205px" runat="server"></asp:TextBox>
                                        <asp:RequiredFieldValidator ValidationGroup="validar" ID="RFVtxtDescripcion" Style="color: red;" SetFocusOnError="true" ControlToValidate="txtDescripcion" InitialValue="" runat="server" ErrorMessage="Ingrese una Descripción." Display="Dynamic" />
                                    </div>
                                    <div class="col-md-8 col-md-offset-2">
                                        <asp:Button ID="btnGuardar" Text="Guardar" ValidationGroup="validar" OnClick="btnGuardar_Click" CssClass="btn btn-success btn-block" runat="server" Width="100%" />
                                        <asp:Button ID="btnEditar" Text="Editar" ValidationGroup="validar" OnClick="btnEditar_Click" CssClass="btn btn-info btn-block" runat="server" Width="100%" />
                                        <br />
                                        <asp:Button ID="btNuevo" Text="Nuevo" OnClick="btNuevo_Click" CssClass="btn btn-warning btn-block" runat="server" Width="100%" />
                                        <br />
                                        <asp:Button ID="btnEliminar" Text="Eliminar" OnClick="btnEliminar_Click" CssClass="btn btn-danger btn-block" runat="server" Width="100%"
                                            OnClientClick="return confirm('¿Desea eliminar el registro?');" />
                                        <br />
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-12" style="overflow: auto; height: 100%">
                                    <asp:GridView ID="gvListadoAcciones" runat="server" DataKeyNames="Compromiso"
                                        AllowPaging="true" OnPageIndexChanging="gvListadoAcciones_PageIndexChanging" PageSize="10"
                                        BackColor="#fdffe6" CssClass="table table-hover table-bordered" AutoGenerateColumns="false" Width="1500px"
                                        OnRowCommand="gvListadoAcciones_RowCommand" OnRowDataBound="gvListado_RowDataBound">
                                        <AlternatingRowStyle BackColor="#f2fffc" />
                                        <Columns>
                                            <asp:BoundField DataField="id" HeaderText="ID" />
                                            <asp:TemplateField ItemStyle-Width="30px" HeaderText="">
                                                <ItemTemplate>
                                                    <asp:Button ID="btIngresar" CausesValidation="false" runat="server" CssClass="btn btn-info"
                                                        CommandName="Ver" CommandArgument="<%# Container.DataItemIndex %>"
                                                        Text="Ver" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="Compromiso" HeaderText="Compromiso" />
                                            <asp:BoundField DataField="Status" HeaderText="Status" />
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
                            <br />

                            <div class="row">
                                <div class="form-group">
                                    <div class="col-sm-3">
                                        <asp:Button Visible="false" ID="btnFinalizar" OnClick="btnFinalizar_Click" Text="Finalizar" CssClass="btn btn-success" runat="server" Width="100%"
                                            OnClientClick="return confirm('¿Desea finalizar el Informe?');" />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
