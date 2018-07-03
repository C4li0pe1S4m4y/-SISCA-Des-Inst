<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ListadoAcciones.aspx.cs" Inherits="SistemaGdC.Acciones.ListadoAcciones" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h2>&nbsp;</h2>    
    <h2 style="color:white"><b>Lista de Acciones</b></h2>

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

                <div class="col-md-12" id="pn1" runat="server">
                    <div class="panel panel-default">
                        <div class="panel-heading">Acciones</div>
                        <div class="panel-body">
                            <div class="row" id="panel1" runat="server">
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
                                        <asp:DropDownList ID="dllTipoAccion" Enabled="true" OnSelectedIndexChanged="dllTipoAccion_SelectedIndexChanged" AutoPostBack="true" CssClass="form-control input" Width="100%" runat="server"></asp:DropDownList>
                                    </div>
                                    <div class="form-group">
                                        <label>Fecha Recepción: </label>
                                        <asp:TextBox ID="txtFechaRecepcion" Enabled="false" CssClass="form-control input" TextMode="Date" runat="server"></asp:TextBox>
                                    </div>
                                    <div class="form-group">
                                        <br />
                                        <asp:Button ID="btnActualizar" Text="Actualizar" OnClick="btnActualizar_Click" Visible="false" CssClass="btn btn-info btn-block" runat="server" Width="100%" />
                                    </div>
                                    <!--<div class="form-group">
                                        <label>No. Plan de Acción/Informe: </label>
                                        <asp:TextBox ID="txtNoPlanAccion" Enabled="false" CssClass="form-control input" runat="server"></asp:TextBox>
                                    </div>-->
                                </div>
                                <div class="col-md-4">
                                    <div class="form-group">
                                        <label>Descripción: </label>
                                        <asp:TextBox ID="txtDescripcion" Enabled="false" Width="100%" CssClass="form-control" TextMode="MultiLine" Style="height: 205px" runat="server"></asp:TextBox>
                                    </div>
                                    <div class="col-md-8 col-md-offset-2">
                                        <br />
                                        <div class="form-group">
                                            <asp:Button ID="btnAccion" Text="Crear Plan de Acción" Visible="false" OnClick="btnAccion_Click" CssClass="btn btn-primary btn-block" runat="server" Width="100%" />
                                            <asp:Button ID="btnInformeCO" Text="Crear Informe CO" Visible="false" OnClick="btnInformeCO_Click" CssClass="btn btn-primary btn-block" runat="server" Width="100%" />
                                            <asp:Button ID="btnInformeOM" Text="Crear Informe OM" Visible="false" OnClick="btnInformeOM_Click" CssClass="btn btn-primary btn-block" runat="server" Width="100%" />
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="row">

                                <div class="col-md-12" style="height: 100%">
                                    <asp:GridView ID="gvListadoAcciones" runat="server" DataKeyNames="Correlativo"
                                        AllowPaging="true" OnPageIndexChanging="gvListadoAcciones_PageIndexChanging" PageSize="5"
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
        </ContentTemplate>
    </asp:UpdatePanel>


</asp:Content>
