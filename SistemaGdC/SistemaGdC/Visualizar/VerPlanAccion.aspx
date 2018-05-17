<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="VerPlanAccion.aspx.cs" Inherits="SistemaGdC.Visualizar.VerPlanAccion" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <link href="~/Content/bootstrap.css" rel="stylesheet" media="screen" />
    <h2>&nbsp;</h2>
    <h2 style="color: white"><b>Plan de Acción</b></h2>
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
                                        <asp:DropDownList ID="ddlTecnicaAnalisis" runat="server" CssClass="form-control input"></asp:DropDownList>

                                    </div>
                                </div>
                                <div class="col-md-7">
                                    <div class="form-group">
                                        <label>Lider</label>
                                        <asp:DropDownList ID="ddlLider" runat="server" CssClass="form-control input"></asp:DropDownList>

                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-12">
                                    <label>Causa Raíz:</label>
                                    <textarea id="txtCausa" type="text" textmode="MultiLine" class="form-control" name="option" runat="server" style="height: 100px" onkeypress="return descripcion(event);" />

                                </div>
                            </div>
                            <div class="col-md-4">
                                <br />
                                <asp:Button ID="btnCancelar" OnClick="btnCancelar_Click" Text="Cancelar" CssClass="btn" runat="server" Width="100%" />
                            </div>
                            <div class="col-md-4">
                                <br />
                                <asp:Button ID="btnDescargar" Visible="true" Text="Causa Raíz" OnClick="btnDescargar_Click" CssClass="btn btn-info" runat="server" Width="100%" />
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
                                                <input id="txtAccionRealizar" type="text" class="form-control" name="option" runat="server" onkeypress="return descripcion(event);" />
                                                
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                    </div>
                                    <div class="row">
                                        <div class="col-md-4">
                                            <div class="form-group">
                                                <label>Responsable:</label>
                                                <asp:TextBox ID="txtResponsable" runat="server" CssClass="form-control"></asp:TextBox>
                                                
                                            </div>
                                        </div>
                                        <div class="col-md-4">
                                            <div class="form-group">
                                                <label>Fecha Inicio:</label>
                                                <asp:TextBox ID="txtFechaInicio" TextMode="Date" runat="server" CssClass="form-control"></asp:TextBox>
                                                
                                            </div>
                                        </div>
                                        <div class="col-md-4">
                                            <div class="form-group">
                                                <label>Fecha Final:</label>
                                                <asp:TextBox ID="txtFechaFin" TextMode="Date" runat="server" CssClass="form-control"></asp:TextBox>
                                                
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-9">
                                            <label>Observaciones:</label>
                                            <textarea id="txtObservaciones" type="text" textmode="MultiLine" class="form-control" name="option" runat="server" style="height: 120px" onkeypress="return descripcion(event);" />
                                        </div>
                                        <div class="col-md-3">
                                            <h2>&nbsp;</h2>
                                            <asp:Button ID="btnDescargarEvidencia" Visible="false" Text="Evidencia" OnClick="btnDescargarEvidencia_Click" CssClass="btn btn-info" runat="server" Width="100%" />
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </ContentTemplate>
                        <Triggers>
                            <asp:PostBackTrigger ControlID="btnDescargarEvidencia" />
                        </Triggers>
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
                                    <asp:GridView ID="gvListado" runat="server" DataKeyNames="No."
                                        BackColor="#fdffe6" CssClass="table table-hover table-bordered" Width="1500px"
                                        OnSelectedIndexChanged="gvListado_SelectedIndexChanged">
                                        <AlternatingRowStyle BackColor="#f2fffc" />
                                        <Columns>
                                            <asp:CommandField ButtonType="Button" HeaderText="Eviencia" ControlStyle-CssClass="btn btn-info" ShowSelectButton="True" SelectText="Ver">
                                                <ControlStyle CssClass="btn btn-info" />
                                                <ItemStyle Width="1%" />
                                            </asp:CommandField>
                                        </Columns>
                                        <HeaderStyle BackColor="#00b4b4" />
                                    </asp:GridView>
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
    </asp:UpdatePanel>
</asp:Content>
