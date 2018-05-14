<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="PlanAccion.aspx.cs" Inherits="SistemaGdC.InformeResultados.PlanAccion" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <link href="~/Content/bootstrap.css" rel="stylesheet" media="screen" />
    <h2>&nbsp;</h2>
    <h2 style="color:white"><b>Plan de Acción</b></h2>
    <asp:UpdatePanel ID="updatepanel1" runat="server">
        <ContentTemplate>
            <div class="row">
                <div class="col-md-12">
                    <div class="panel panel-default">
                        <div class="panel-heading">Encabezado</div>
                        <div class="panel-body">
                            <div class="row">
                                <div class="col-md-8">
                                    <div class="row">
                                        <div class="col-md-2">
                                            <div class="form-group">
                                                <label>Año:</label>
                                                <asp:TextBox ID="txtanio" Enabled="false" CssClass="form-control" runat="server"></asp:TextBox>
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
                </div>
            </div>
            <div class="row">
                <div class="col-md-5">
                    <div class="panel panel-default">
                        <div class="panel-heading">Causa Raíz</div>
                        <div class="panel-body">
                            <div class="row">
                                <div class="col-md-5">
                                    <div class="form-group">
                                        <label>Técnica de Análisis:</label>
                                        <asp:DropDownList ID="ddlTecnicaAnalisis" runat="server" CssClass="form-control input" OnSelectedIndexChanged="ddlTecnicaAnalisis_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RFVddlTecnicaAnalisis" ValidationGroup="validarCausa" Style="color: red;" SetFocusOnError="true" ControlToValidate="ddlTecnicaAnalisis" InitialValue="0" runat="server" ErrorMessage="Seleccione Técnica de Análisis." Display="Dynamic" />
                                    </div>
                                </div>
                                <div class="col-md-7">
                                    <div class="form-group">
                                        <label>Lider</label>
                                        <asp:DropDownList ID="ddlLider" runat="server" CssClass="form-control input"></asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RFVddlLider" ValidationGroup="validarCausa" Style="color: red;" SetFocusOnError="true" ControlToValidate="ddlLider" InitialValue="0" runat="server" ErrorMessage="Seleccione Líder." Display="Dynamic" />
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-12">
                                    <label>Causa Raíz:</label>
                                    <asp:TextBox ID="txtCausa" TextMode="MultiLine" runat="server" CssClass="form-control input" Style="height: 100px"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RFVtxtCausa" ValidationGroup="validarCausa" Style="color: red;" SetFocusOnError="true" ControlToValidate="txtCausa" InitialValue="" runat="server" ErrorMessage="Ingrese Causa." Display="Dynamic" />
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-12">
                                    <br />
                                    <asp:FileUpload ID="FileEvidencia" CssClass="btn btn-primary btn-sm" runat="server" Width="100%" />
                                </div>
                            </div>
                            <div class="col-md-4">
                                <br />
                                <asp:Button ID="btnCancelar" OnClick="btnCancelar_Click" Text="Cancelar" CssClass="btn" runat="server" Width="100%" />
                            </div>
                            <div class="col-md-4">
                                <br />
                                <asp:Button ID="btnDescargar" Visible="false" Text="Down" OnClick="btnDescargar_Click" CssClass="btn btn-info" runat="server" Width="100%" />
                            </div>
                            <div class="col-md-4">
                                <br />
                                <asp:Button ID="btnGuardarCausa" ValidationGroup="validarCausa" Text="Guardar" OnClick="btnGuardarCausa_Click" CssClass="btn btn-success" runat="server" Width="100%" />
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-md-7">
                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                        <ContentTemplate>
                            <div class="panel panel-default">
                                <div class="panel-heading">Actividades del Plan de Acción</div>
                                <div class="panel-body">
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="form-group">
                                                <label>Actividad a Realizar:</label>
                                                <input id="txtAccionRealizar" type="text" class="form-control" name="option" runat="server" onkeypress="return prueba(event);"/>
                                                <asp:RequiredFieldValidator ID="RFVtxtAccionRealizar" ValidationGroup="validarActividad" Style="color: red;" SetFocusOnError="true" ControlToValidate="txtAccionRealizar" InitialValue="" runat="server" ErrorMessage="Ingrese Acción a Realizar." Display="Dynamic" />
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                    </div>
                                    <div class="row">
                                        <div class="col-md-4">
                                            <div class="form-group">
                                                <label>Responsable:</label>
                                                <asp:DropDownList ID="ddlResponsable" runat="server" CssClass="form-control"></asp:DropDownList>
                                                <asp:RequiredFieldValidator ID="RFVddlResponsable" ValidationGroup="validarActividad" Style="color: red;" SetFocusOnError="true" ControlToValidate="ddlResponsable" InitialValue="0" runat="server" ErrorMessage="Seleccione Responsable." Display="Dynamic" />
                                            </div>
                                        </div>
                                        <div class="col-md-4">
                                            <div class="form-group">
                                                <label>Fecha Inicio:</label>
                                                <asp:TextBox ID="txtFechaInicio" TextMode="Date" runat="server" CssClass="form-control"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="RFVtxtFechaInicio" ValidationGroup="validarActividad" Style="color: red;" SetFocusOnError="true" ControlToValidate="txtFechaInicio" InitialValue="" runat="server" ErrorMessage="Ingrese Fecha." Display="Dynamic" />
                                            </div>
                                        </div>
                                        <div class="col-md-4">
                                            <div class="form-group">
                                                <label>Fecha Final:</label>
                                                <asp:TextBox ID="txtFechaFin" TextMode="Date" runat="server" CssClass="form-control"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="RFVtxtFechaFin" ValidationGroup="validarActividad" Style="color: red;" SetFocusOnError="true" ControlToValidate="txtFechaFin" InitialValue="" runat="server" ErrorMessage="Ingrese Fecha." Display="Dynamic" />
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-9">
                                            <label>Observaciones:</label>
                                            <asp:TextBox ID="txtObservaciones" TextMode="MultiLine" runat="server" CssClass="form-control" Style="height: 120px"></asp:TextBox>
                                        </div>
                                        <div class="col-md-3">
                                            <br />
                                            <br />
                                            <asp:Button ID="btnGuardar" ValidationGroup="validarActividad" Text="Guardar" OnClick="btnGuardar_Click" CssClass="btn btn-success btn-block" runat="server" Width="100%" />
                                            <br />
                                            <asp:Button ID="btnNuevo" Text="Nuevo" OnClick="btnNuevo_Click" CssClass="btn btn-warning btn-block" runat="server" Width="100%" />
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
            <div class="row">
                <div class="col-md-12">
                    <div class="panel panel-default">
                        <div class="panel-heading">Lista de Actividades</div>
                        <div class="panel-body">
                            <div class="row">
                                <div class="col-md-12" style="overflow: auto; height: 100%">
                                    <asp:GridView ID="gvListado" runat="server"
                                        BackColor="#fdffe6" CssClass="table table-hover table-bordered" Width="1500px"
                                        OnRowCommand="gvListado_RowCommand">
                                        <AlternatingRowStyle BackColor="#f2fffc" />
                                        <Columns>
                                            <asp:TemplateField ItemStyle-Width="30px" HeaderText="">
                                                <ItemTemplate>
                                                    <asp:Button ID="btnEliminar" runat="server" CssClass="btn btn-danger"
                                                        CommandName="Eliminar" CommandArgument="<%# Container.DataItemIndex %>"
                                                        OnClientClick="return confirm('¿Desea eliminar esta Actividad?');"
                                                        Text="Eliminar" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <HeaderStyle BackColor="#00b4b4" />
                                    </asp:GridView>
                                </div>
                            </div>
                            <div class="row">
                                <div class="form-group">
                                    <div class="col-sm-3">
                                        <asp:Button Visible="true" ID="btnFinalizar" Text="Finalizar" OnClick="btnFinalizar_Click" CssClass="btn btn-success" runat="server" Width="100%"
                                            OnClientClick="return confirm('¿Desea finalizar el Plan de Acción?');" />
                                    </div>
                                </div>
                            </div>
                    </div>
                </div>
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnDescargar" />
        </Triggers>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnGuardarCausa" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
