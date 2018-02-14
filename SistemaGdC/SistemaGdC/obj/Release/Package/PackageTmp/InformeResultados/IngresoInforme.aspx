<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="IngresoInforme.aspx.cs" Inherits="SistemaGdC.Informe.IngresoInforme" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <link href="~/Content/bootstrap.css" rel="stylesheet" media="screen" />
    <h2>&nbsp;</h2>
    <h2>Ingreso de Informe De Evaluacion Interna/Auditoria Externa</h2>
    <div class="row">
        <div class="row">
            <div class="col-xs-12">
                <div class="panel panel-default">
                    <div class="panel-heading">Encabezado</div>
                    <div class="panel-body">
                        <div class="col-xs-1">
                            <label><span style="font-size: medium">No.</span>:</label>
                             <asp:Label runat="server" ID="lblCorrelativo" >
                            
                        </asp:Label>
                        </div>
                       
                        <asp:Label ID="txtTelefono" CssClass="label" Text="xxx" runat="server"></asp:Label>
                        <div class="col-sm-1">
                            <label>
                                <span style="font-size: medium">año</span>: 
                            </label>
                            <asp:TextBox ID="txtanio" CssClass="form-control input " TextMode="Number" Width="100%" runat="server" Style="font-size: x-small" required=""></asp:TextBox>
                        </div>
                        <div class="col-sm-3">
                            <label><span style="font-size: small">No.Evaluacion</span>: </label>
                            <asp:TextBox ID="txtInforme" CssClass="form-control input " runat="server" required="true"></asp:TextBox>
                        </div>

                        <div class="col-sm-3">
                            <label>
                                <span style="font-size: small">Fecha Informe</span>: 
                            </label>
                            <asp:TextBox ID="txtFechaInforme" CssClass="form-control input" TextMode="Date" runat="server" ></asp:TextBox>
                        </div>
                        <div class="col-sm-1">
                            <br />
                            <asp:Button ID="btnGuardarEncabezado" OnClick="btnGuardarEncabezado_Click" Text="Almacenar" runat="server" CssClass="btn btn-success" />
                        </div>
                        <div class="col-sm-1">
                            <br />
                            <asp:Button ID="btnBuscarEncabezado" OnClick="btnBuscarEncabezado_Click"  Text="Buscar" runat="server" CssClass="btn btn-info" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="col-xs-12">
                <div class="panel panel-default">
                    <div class="panel-heading">Acciones</div>
                    <div class="panel-body">

                        <div class="col-sm-4">
                            <label><span style="font-size: small">Accion Generada</span>: </label>
                            <asp:DropDownList ID="ddlAccionGenerada"  CssClass="dropdown input-sm" Width="100%" runat="server" Enabled="true" required="true"></asp:DropDownList>
                        </div>

                        <div class="col-sm-4">
                            <label><span style="font-size: small">No.Hallazgo</span>: </label>
                            <asp:TextBox ID="txtHallazgo" CssClass="form-control input" runat="server"  ></asp:TextBox>
                        </div>

                        <div class="col-sm-4">
                            <label><span style="font-size: small">Punto de Norma</span>: </label>
                            <asp:TextBox ID="txtPuntoNorma" CssClass="form-control input" runat="server"></asp:TextBox>
                        </div>

                        <div class="col-sm-4">
                            <label><span style="font-size: small">Proceso Relacionado</span>: </label>
                            <asp:DropDownList ID="ddlProceso"   CssClass="dropdown input-sm" Width="100%" runat="server" Enabled="true" ></asp:DropDownList>
                        </div>

                        <div class="col-sm-4">
                            <label class="col-sm-2"><span style="font-size: small">Unidad</span>: </label>
                            <asp:DropDownList ID="ddlUnidad" OnSelectedIndexChanged="ddlUnidad_SelectedIndexChanged" AutoPostBack="true" CssClass="dropdown input-sm" Width="100%" runat="server" Enabled="true"></asp:DropDownList>
                        </div>

                        <div class="col-sm-4">
                            <label class="col-sm-2"><span style="font-size: small">Dependencia</span>: </label>
                            <asp:DropDownList ID="ddlDependencia"  CssClass="dropdown input-sm" Width="100%" runat="server" Enabled="true"></asp:DropDownList>
                        </div>




                        <div class="col-sm-12">
                            <label class="col-sm-2"><span style="font-size: small">Descripcion</span>: </label>
                            <asp:TextBox ID="txtDescripcion" Width="100%" CssClass="form-control col-lg-8" TextMode="MultiLine" runat="server"></asp:TextBox>
                        </div>



                        <div class="col-sm-4">
                            <label class="col-sm-2"><span style="font-size: small">Responsable</span>: </label>
                            <asp:DropDownList ID="ddlResponsable" CssClass="dropdown input-sm" Width="100%" runat="server" Enabled="true"></asp:DropDownList>
                        </div>

                        <div class="col-sm-4">
                            <label class="col-sm-2"><span style="font-size: small">Analista</span>: </label>
                            <asp:DropDownList ID="ddlAnalista" CssClass="dropdown input-sm" Width="100%" runat="server" Enabled="true"></asp:DropDownList>
                        </div>

                     

                        <div class="col-sm-4">
                            <label><span style="font-size: small">Tipo Accion/Informe</span>: </label>
                            <asp:DropDownList ID="dllTipoAccion"  CssClass="dropdown input-sm" Width="100%" runat="server" Enabled="true"></asp:DropDownList>
                        </div>

                        <div class="col-sm-4">
                            <label><span style="font-size: small">Fecha Recepcion</span>: </label>
                            <asp:TextBox ID="txtFechaRecepcion" CssClass="form-control input" TextMode="Date" runat="server" ></asp:TextBox>
                        </div>

                        <div class="col-sm-4">
                            <label><span style="font-size: small">No.Plan Accion/Informe</span>: </label>
                            <asp:TextBox ID="txtNoPlanAccion" CssClass="form-control input" runat="server" ></asp:TextBox>
                        </div>
                       <div class="col-sm-5">
                           <label></label>
                       </div>
                        <div class="col-sm-3">
                             <br />
                            <asp:Button ID="btnGuardar" Text="Guardar" CssClass="btn btn-success" runat="server" Width="100%" OnClick="btnGuardar_Click" />
                        </div>
                        <div class="col-sm-3">
                             <br />
                            <asp:Button ID="btNuevo" Text="Nuevo" OnClick="btNuevo_Click" CssClass="btn btn-warning" runat="server" Width="100%" />
                        </div>
                    </div>
                </div>
            </div>

            <div class="form-group">


                <div class="col-sm-3">
                    <asp:Button ID="btnListado" OnClick="btnListado_Click" Text="Listado" CssClass="btn btn-info" runat="server" Width="100%" />
                </div>
            </div>
        </div>
    </div>
</asp:Content>
