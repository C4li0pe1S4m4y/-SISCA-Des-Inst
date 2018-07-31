<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ListadoUsuarios.aspx.cs" Inherits="SistemaGdC.InformeResultados.Acciones.ListadoUsuarios" %>

<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.3500.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" Namespace="CrystalDecisions.Web" TagPrefix="CR" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h2>&nbsp;</h2>
    <h2 style="color: white"><b>Lista de Usuarios</b></h2>

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

    <div>
    </div>

    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div class="row">

                <div class="col-md-12" id="pn1" runat="server">
                    <div class="panel panel-default">
                        <div class="panel-heading">
                            <label id="lblFuente" runat="server">Control de Usuarios</label>
                        </div>
                        <div class="panel-body">
                            <div class="row">
                                <div class="row col-md-8 col-md-offset-2">
                                    <div class="col-md-6">
                                        <div class="form-group">
                                            <label>Usuario: </label>
                                            <asp:TextBox ID="txtUsuario" CssClass="form-control input" runat="server"></asp:TextBox>
                                            <asp:RequiredFieldValidator ValidationGroup="validar" ID="RFVtxtUsuario" Style="color: red;" SetFocusOnError="true" ControlToValidate="txtUsuario" InitialValue="" runat="server" ErrorMessage="Ingrese Usuario." Display="Dynamic" />
                                        </div>
                                        <div class="form-group">
                                            <label>Contraseña: </label>
                                            <asp:TextBox ID="txtcontra" CssClass="form-control input" runat="server" TextMode="Password"></asp:TextBox>
                                            <asp:RequiredFieldValidator ValidationGroup="validar" ID="RFVtxtcontra" Style="color: red;" SetFocusOnError="true" ControlToValidate="txtcontra" InitialValue="" runat="server" ErrorMessage="Ingrese Contraseña." Display="Dynamic" />
                                        </div>
                                        <div class="form-group">
                                            <label>Correo: </label>
                                            <asp:TextBox ID="txtCorreo" CssClass="form-control input" runat="server"></asp:TextBox>
                                            <asp:RequiredFieldValidator ValidationGroup="validar" ID="RFVtxtCorreo" Style="color: red;" SetFocusOnError="true" ControlToValidate="txtCorreo" InitialValue="" runat="server" ErrorMessage="Ingrese Correo Electrónico." Display="Dynamic" />
                                        </div>
                                    </div>
                                    <div class="col-md-6">
                                        <div class="form-group">
                                            <label>Empleado: </label>
                                            <asp:DropDownList ID="ddlEmpleado" CssClass="form-control input" Width="100%" runat="server" Enabled="true"></asp:DropDownList>
                                            <asp:RequiredFieldValidator ValidationGroup="validar" ID="RFVddlEmpleado" Style="color: red;" SetFocusOnError="true" ControlToValidate="ddlEmpleado" InitialValue="0" runat="server" ErrorMessage="Seleccione Empleado." Display="Dynamic" />
                                        </div>
                                        <div class="form-group">
                                            <label>Tipo de Usuario: </label>
                                            <asp:DropDownList ID="ddlTipoUsuario" CssClass="form-control input" Width="100%" runat="server" Enabled="true"></asp:DropDownList>
                                            <asp:RequiredFieldValidator ValidationGroup="validar" ID="RFVddlTipoUsuario" Style="color: red;" SetFocusOnError="true" ControlToValidate="ddlTipoUsuario" InitialValue="0" runat="server" ErrorMessage="Seleccione Tipo de Usuario." Display="Dynamic" />
                                        </div>
                                        <div class="col-md-6 col-md-offset-3">
                                            <label>&nbsp;</label>
                                            <asp:Button ID="btnGuardar" Text="Guardar" OnClick="btnGuardar_Click" ValidationGroup="validar" CssClass="btn btn-success btn-lg" runat="server" Width="100%"
                                                OnClientClick="return confirm('¿Desea ingresar el Usuario?');" />
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <br />
                                <br />
                                <div class="col-md-12">
                                    <asp:GridView ID="gvListadoUsuarios" runat="server" DataKeyNames="id"
                                        AllowPaging="true" OnPageIndexChanging="gvListadoUsuarios_PageIndexChanging" PageSize="10"
                                        BackColor="#fdffe6" CssClass="table table-hover table-bordered" AutoGenerateColumns="false" Width="100%"
                                        OnRowCommand="gvListadoUsuarios_RowCommand">
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
                                            <asp:BoundField DataField="Usuario" HeaderText="Usuario" />
                                            <asp:BoundField DataField="Nombre" HeaderText="Nombre" />
                                            <asp:BoundField DataField="Tipo Usuario" HeaderText="Tipo Usuario" />
                                            <asp:BoundField DataField="Correo" HeaderText="Correo" />
                                        </Columns>
                                        <HeaderStyle BackColor="#33CCFF" />
                                    </asp:GridView>
                                </div>

                            </div>

                        </div>
                    </div>
                </div>
            </div>
            <div>
                <CR:CrystalReportViewer ID="CrystalReportViewer2" runat="server" AutoDataBind="true" />
            </div>
            <asp:Button ID="btnReporte" Text="Reporte" OnClick="btnReporte_Click" CssClass="btn btn-success btn-lg" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>
