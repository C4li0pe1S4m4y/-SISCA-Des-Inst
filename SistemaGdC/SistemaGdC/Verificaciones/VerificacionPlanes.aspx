﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="VerificacionPlanes.aspx.cs" Inherits="SistemaGdC.Verificaciones.VerificacionPlanes" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h2>&nbsp;</h2>
    <h2>Verificación Planes de Acción</h2>

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
                        <div class="panel-heading">Encabezado</div>
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
                                        <div class="col-md-4" visible="false">
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
                            <div class="row" id="panel2" runat="server">
                                <div class="col-md-12">
                                    <div class="col-md-12" style="overflow: auto; height: 100%">
                                        <asp:GridView ID="gvListadoAcciones" runat="server" DataKeyNames="Correlativo"
                                            BackColor="#fdffe6" CssClass="table table-hover table-bordered" AutoGenerateColumns="false" Width="1500px"
                                            OnRowCommand="gvListadoAcciones_RowCommand">
                                            <AlternatingRowStyle BackColor="#f2fffc" />
                                            <Columns>
                                                <asp:BoundField DataField="id" HeaderText="ID" />
                                                <asp:TemplateField ItemStyle-Width="30px" HeaderText="CustomerID">
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
                                    <asp:TextBox ID="txtCausa" TextMode="MultiLine" runat="server" CssClass="form-control input" Style="height: 140px"></asp:TextBox>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-12">
                                    <div class="col-md-4">
                                        <br />
                                        <asp:Button ID="btnCancelar" OnClick="btnCancelar_Click" Text="Cancelar" CssClass="btn" runat="server" Width="100%" />
                                    </div>

                                    <div class="col-md-4">
                                        <br />
                                        <asp:Button ID="btnEvidencia" Text="Evidencia" CssClass="btn btn-primary" OnClick="btnDescargar_Click" runat="server" Width="100%" />
                                    </div>
                                    <div class="col-md-1"></div>
                                    <div class="col-md-1">
                                        <br />
                                        <asp:LinkButton ID="btnValidar" OnClick="btnValidar_Click" CssClass="btn btn-success" runat="server"
                                            OnClientClick="return confirm('¿Desea validar el Plan de Acción?');"><span class="glyphicon glyphicon-ok"/></asp:LinkButton>
                                    </div>
                                    <div class="col-md-1">
                                        <br />
                                        <asp:LinkButton ID="btnRechazar" OnClick="btnRechazar_Click" CssClass="btn btn-danger" runat="server"
                                            OnClientClick="return confirm('¿Desea rechazar el Plan de Acción?');"><span class="glyphicon glyphicon-remove"/></asp:LinkButton>
                                    </div>
                                </div>
                            </div>

                        </div>
                    </div>
                </div>
                <div class="col-md-6">
                    <div class="panel panel-default">
                        <div class="panel-heading">Lista de Actividades </div>
                        <div class="panel-body">
                            <div class="row">
                                <div class="col-md-12" style="overflow: auto; height: 100%">
                                    <asp:GridView ID="gvListadoActividades" runat="server"
                                        BackColor="#fdffe6" CssClass="table table-hover table-bordered" Width="1500px">
                                        <AlternatingRowStyle BackColor="#f2fffc" />
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
    </asp:UpdatePanel>
</asp:Content>
