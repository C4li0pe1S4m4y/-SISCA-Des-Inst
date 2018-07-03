<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="SistemaGdC._Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <%if (Session["id_tipo_usuario"].ToString() == "2")
        {%>

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

        .GridPager a,
        .GridPager span {
            display: inline-block;
            padding: 0px 9px;
            margin-right: 4px;
            border-radius: 3px;
            border: solid 1px #c0c0c0;
            background: #e9e9e9;
            box-shadow: inset 0px 1px 0px rgba(255,255,255, .8), 0px 1px 3px rgba(0,0,0, .1);
            font-size: .875em;
            font-weight: bold;
            text-decoration: none;
            color: #717171;
            text-shadow: 0px 1px 0px rgba(255,255,255, 1);
        }

        .GridPager a {
            background-color: #f5f5f5;
            color: #969696;
            border: 1px solid #969696;
        }

        .GridPager span {
            background: #616161;
            box-shadow: inset 0px 0px 8px rgba(0,0,0, .5), 0px 1px 0px rgba(255,255,255, .8);
            color: #f0f0f0;
            text-shadow: 0px 0px 3px rgba(0,0,0, .5);
            border: 1px solid #3AC0F2;
        }
    </style>

    <script type="text/javascript">
        (function ($) {
            var buttons,
                estado = 0;

            function init() {
                buttons = $('.glyphicon-plus, .glyphicon-minus');
                buttons.mas = buttons.eq(0);
                buttons.menos = buttons.eq(1);

                buttons.on('click', buttonClick);
                updateUI();
            }

            function buttonClick(e) {
                estado += (this === buttons.mas[0]) ? 1 : -1;
                updateUI();
                e.preventDefault();
            }

            function updateUI() {
                buttons.mas.toggle(estado == 0);
                buttons.menos.toggle(estado !== 0);
            }
            $(init);
        }(jQuery));

        $(window).resize(function () {
            drawChart1();
            drawChart2();
            drawChart3();
            drawChart4();
            drawChart5();
        });
    </script>

    <script type="text/javascript" src="https://www.gstatic.com/charts/loader.js"></script>
    <script type="text/javascript">
        google.charts.load('current', { 'packages': ['corechart'] });
        google.charts.load('current', { 'packages': ['corechart', 'bar'] });
        google.charts.load('current', { 'packages': ['line'] });
        google.charts.load('current', { 'packages': ['bar'] });

        google.charts.setOnLoadCallback(drawChart1);
        google.charts.setOnLoadCallback(drawChart2);
        google.charts.setOnLoadCallback(drawChart3);
        google.charts.setOnLoadCallback(drawChart4);
        google.charts.setOnLoadCallback(drawChart5);

        function alertar() {
            alert("grafica");
            drawChart1();
            drawChart2();
            drawChart3();
            drawChart4();
            //drawChart5();
            //PageMethods.Index_Changed("nada");

        }

        function drawChart1() {
            var data = google.visualization.arrayToDataTable(<%=graficaAcciones()%>);

            var options = {
                title: 'Acciones del SGC',
                chartArea: { left: 5, right: 5, top: 25, bottom: 10 },
                legend: { position: 'labeled', texStyle: { fontSize: 11 } },
                annotations: { textStyle: { fontSize: 9 } },
                pieHole: 0.4
            };

            var chart = new google.visualization.PieChart(document.getElementById('graficaAcciones'));
            chart.draw(data, options);
        }

        function drawChart2() {
            var data = google.visualization.arrayToDataTable([
             ['Cuatrimestre', 'Resultados', 'Meta'],
             ['Cuatrimestre 1', 1, 0.85],
             ['Cuatrimestre 2', 0.9, 0.85],
             ['Cuatrimestre 3', 1, 0.85]
            ]);

            var options = {
                vAxis: { format: 'percent' },
                seriesType: 'bars',
                series: { 1: { type: 'line' } },
                chartArea: { 'left': '45', 'right': '105', 'top': '10', 'bottom': '15', 'height': '40%' },
                colors: ['#3366cc', '#ff9900']
            };

            var chart = new google.visualization.ComboChart(document.getElementById('graficaPlanesAccionEficaces'));
            chart.draw(data, options);
        }

        function drawChart3() {
            var data = google.visualization.arrayToDataTable(<%=graficaPlanesAccion()%>);

            var options = {
                legend: { position: 'top', maxLines: 3, textStyle: { fontSize: 11, bold: true } },
                bar: { groupWidth: '75%' },
                isStacked: true,
                chartArea: { 'left': '195', 'right': '5', 'top': '30', 'bottom': '20', 'height': '40%' },
                colors: ['#ff9900', '#109618', '#3366cc', ],
                annotations: { textStyle: { fontSize: 9 } },
                vAxis: { textStyle: { fontSize: 11 } }
            };

            var chart = new google.visualization.BarChart(document.getElementById('graficaPlanesAccion'));
            chart.draw(data, options);
        }

        function drawChart4() {
            var data = google.visualization.arrayToDataTable(<%=graficaPlanesAccionAbiertos()%>);

            var options = {
                chartArea: { 'left': '195', 'right': '5', 'top': '30', 'bottom': '20', 'height': '40%' },
                bars: 'horizontal',
                legend: { position: 'top', textStyle: { fontSize: 11, bold: true } },
                annotations: { alwaysOutside: true, textStyle: { fontSize: 9 } },
                colors: ['#dc3912', '#109618', ],
                vAxis: { textStyle: { fontSize: 11 } },

            };

            var view = new google.visualization.DataView(data);
            view.setColumns([0, 1,
                             {
                                 calc: "stringify",
                                 sourceColumn: 1,
                                 type: "string",
                                 role: "annotation",
                             },
                             2, {
                                 calc: "stringify",
                                 sourceColumn: 2,
                                 type: "string",
                                 role: "annotation"
                             }]);

            var chart = new google.visualization.BarChart(document.getElementById('graficaPlanesAccionAbiertos'));
            chart.draw(view, options);
        }
    </script>

    <h2>&nbsp;</h2>    
    <h2 style="color:white"><b>Panel Principal</b></h2>
    <div class="row">
        <div class="col-md-12">
            <div class="panel panel-default" data-toggle="collapse" data-target=".navbar-collapse">
                <div class="panel-heading">
                    <h4 class="panel-title">
                        <a data-toggle="collapse">Filtro de Búsqueda</a>
                        <a data-toggle="collapse" href="#collapse1" id="mas"><span class="glyphicon glyphicon-plus pull-right"></span></a>
                        <a data-toggle="collapse" href="#collapse1" id="menos"><span class="glyphicon glyphicon-minus pull-right"></span></a>
                    </h4>
                </div>
                <div id="collapse1" class="panel-collapse collapse">
                    <div class="panel-body">
                        <div class="row">
                            <div class="col-md-3">
                                <div class="form-group">
                                    <label>Inicio:</label>
                                    <asp:Label ID="lblUser" runat="server"></asp:Label>
                                    <asp:Label ID="lblId" runat="server"></asp:Label>
                                    <asp:TextBox ID="txtFechaInicio" CssClass="form-control input" TextMode="Date" runat="server"></asp:TextBox>
                                </div>
                                <div class="form-group">
                                    <label>Fin:</label>
                                    <asp:TextBox ID="txtFechaFin" CssClass="form-control input" TextMode="Date" runat="server"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-md-3">
                                <div class="form-group">
                                    <label>Unidad:</label>
                                    <asp:TextBox title="Ingrese Unidad" ID="txtInforme" CssClass="form-control input" runat="server" required=""></asp:TextBox>
                                </div>
                                <div class="form-group">
                                    <label>Dependencia:</label>
                                    <asp:TextBox title="Ingrese Dependencia" ID="TextBox1" CssClass="form-control input" runat="server" required=""></asp:TextBox>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-md-12">
            <div class="panel panel-default" data-toggle="collapse" data-target=".navbar-collapse">
                <div class="panel-heading">
                    <h4 class="panel-title">
                        <a data-toggle="collapse">Planes de Acción</a>
                    </h4>
                </div>
                <div class="panel-body">
                    <asp:UpdatePanel ID="upListado" runat="server">
                        <ContentTemplate>
                            <div class="col-md-12">
                                <asp:GridView ID="gvListadoPlanes" runat="server" DataKeyNames="ID"
                                    AllowPaging="True" OnPageIndexChanging="gvListadoPlanes_PageIndexChanging" PageSize="3"
                                    BackColor="#FDFFE6" CssClass="table table-hover table-bordered" AutoGenerateColumns="False"
                                    OnRowDataBound="gvListadoPlanes_RowDataBound"
                                    OnSelectedIndexChanged="gvListadoPlanes_SelectedIndexChanged">
                                    <PagerStyle CssClass="GridPager" />
                                    <AlternatingRowStyle BackColor="#f2fffc" />
                                    <Columns>
                                        <asp:BoundField DataField="ID" HeaderText="ID">
                                            <ItemStyle Width="5%" />
                                        </asp:BoundField>
                                        <asp:CommandField ButtonType="Button" HeaderText="" ControlStyle-CssClass="btn btn-info" ShowSelectButton="True" SelectText="Ver">
                                            <ControlStyle CssClass="btn btn-info" />
                                            <ItemStyle Width="1%" />
                                        </asp:CommandField>
                                        <asp:BoundField DataField="Informe" HeaderText="Informe">
                                            <ItemStyle Width="10%" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="id_plan" HeaderText="No. Plan">
                                            <ItemStyle Width="10%" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="causa_raiz" HeaderText="Causa Raíz" />
                                        <asp:TemplateField ShowHeader="true" HeaderText="Progreso" ItemStyle-Width="25%">
                                            <ItemTemplate>
                                                <div class="progress" style="margin-bottom: 0">
                                                    <div id="progbarP" class="progress-bar" role="progressbar" aria-valuemin="0" aria-valuemax="100" runat="server">
                                                    </div>
                                                </div>
                                            </ItemTemplate>
                                            <ItemStyle Width="20%" />
                                        </asp:TemplateField>
                                    </Columns>
                                    <HeaderStyle BackColor="#00b4b4" />
                                </asp:GridView>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>

                </div>
            </div>
        </div>
    </div>

    <div class="row">
        <div class="col-md-6">
            <div class="panel panel-default" data-toggle="collapse" data-target=".navbar-collapse">
                <div class="panel-heading">
                    <h4 class="panel-title">
                        <a data-toggle="collapse">Informes de Corrección</a>
                    </h4>
                </div>
                <div class="panel-body">
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>
                            <div class="col-md-12">
                                <asp:GridView ID="gvListadoInformesCO" runat="server" DataKeyNames="ID"
                                    AllowPaging="True" OnPageIndexChanging="gvListadoInformesCO_PageIndexChanging" PageSize="3"
                                    BackColor="#FDFFE6" CssClass="table table-hover table-bordered" AutoGenerateColumns="False"
                                    OnRowDataBound="gvListadoInformesCO_RowDataBound"
                                    OnSelectedIndexChanged="gvListadoInformesCO_SelectedIndexChanged">
                                    <PagerStyle CssClass="GridPager" />
                                    <AlternatingRowStyle BackColor="#f2fffc" />
                                    <Columns>
                                        <asp:BoundField DataField="ID" HeaderText="ID">
                                            <ItemStyle Width="5%" />
                                        </asp:BoundField>
                                        <asp:CommandField ButtonType="Button" HeaderText="" ControlStyle-CssClass="btn btn-info" ShowSelectButton="True" SelectText="Ver">
                                            <ControlStyle CssClass="btn btn-info" />
                                            <ItemStyle Width="1%" />
                                        </asp:CommandField>
                                        <asp:BoundField DataField="Descripción" HeaderText="Descripción">
                                            <ItemStyle Width="30%" />
                                        </asp:BoundField>
                                        <asp:TemplateField ShowHeader="true" HeaderText="Progreso" ItemStyle-Width="25%">
                                            <ItemTemplate>
                                                <div class="progress" style="margin-bottom: 0">
                                                    <div id="progbarICO" class="progress-bar" role="progressbar" aria-valuemin="0" aria-valuemax="100" runat="server">
                                                    </div>
                                                </div>
                                            </ItemTemplate>
                                            <ItemStyle Width="20%" />
                                        </asp:TemplateField>
                                    </Columns>
                                    <HeaderStyle BackColor="#00b4b4" />
                                </asp:GridView>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
        <div class="col-md-6">
            <div class="panel panel-default" data-toggle="collapse" data-target=".navbar-collapse">
                <div class="panel-heading">
                    <h4 class="panel-title">
                        <a data-toggle="collapse">Informes de Oportunidad de Mejora</a>
                    </h4>
                </div>
                <div class="panel-body">
                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                        <ContentTemplate>
                            <div class="col-md-12">
                                <asp:GridView ID="gvListadoInformesOM" runat="server" DataKeyNames="ID"
                                    AllowPaging="True" OnPageIndexChanging="gvListadoInformesOM_PageIndexChanging" PageSize="3"
                                    BackColor="#FDFFE6" CssClass="table table-hover table-bordered" AutoGenerateColumns="False"
                                    OnRowDataBound="gvListadoInformesOM_RowDataBound"
                                    OnSelectedIndexChanged="gvListadoInformesOM_SelectedIndexChanged">
                                    <PagerStyle CssClass="GridPager" />
                                    <AlternatingRowStyle BackColor="#f2fffc" />
                                    <Columns>
                                        <asp:BoundField DataField="ID" HeaderText="ID">
                                            <ItemStyle Width="5%" />
                                        </asp:BoundField>
                                        <asp:CommandField ButtonType="Button" HeaderText="" ControlStyle-CssClass="btn btn-info" ShowSelectButton="True" SelectText="Ver">
                                            <ControlStyle CssClass="btn btn-info" />
                                            <ItemStyle Width="1%" />
                                        </asp:CommandField>
                                        <asp:BoundField DataField="Descripción" HeaderText="Descripción">
                                            <ItemStyle Width="30%" />
                                        </asp:BoundField>
                                        <asp:TemplateField ShowHeader="true" HeaderText="Progreso" ItemStyle-Width="25%">
                                            <ItemTemplate>
                                                <div class="progress" style="margin-bottom: 0">
                                                    <div id="progbarIOM" class="progress-bar" role="progressbar" aria-valuemin="0" aria-valuemax="100" runat="server">
                                                    </div>
                                                </div>
                                            </ItemTemplate>
                                            <ItemStyle Width="20%" />
                                        </asp:TemplateField>
                                    </Columns>
                                    <HeaderStyle BackColor="#00b4b4" />
                                </asp:GridView>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>


    <div class="row">
        <div class="col-md-6">
            <div class="panel panel-default" data-toggle="collapse" data-target=".navbar-collapse">
                <div class="panel-heading">
                    <h4 class="panel-title">
                        <a data-toggle="collapse">Estatus Acciones</a>
                    </h4>
                </div>
                <div class="panel-body">
                    <div id="graficaAcciones" class="chart"></div>
                </div>
            </div>
        </div>
        <div class="col-md-6">
            <div class="panel panel-default" data-toggle="collapse" data-target=".navbar-collapse">
                <div class="panel-heading">
                    <h4 class="panel-title">
                        <a data-toggle="collapse">Planes de Acciones Eficaces</a>
                    </h4>
                </div>
                <div class="panel-body">
                    <div id="graficaPlanesAccionEficaces" class="chart"></div>
                </div>
            </div>

        </div>
    </div>
    <div class="row">
        <div class="col-md-12">
            <div class="panel panel-default" data-toggle="collapse" data-target=".navbar-collapse">
                <div class="panel-heading">
                    <h4 class="panel-title">
                        <a data-toggle="modal" data-target="#modalGraficaPlanesAccion" data-backdrop="static">Estatus Planes de Acción del SGC</a>
                    </h4>
                </div>
                <div class="panel-body" style="height: 350px">
                    <div id="graficaPlanesAccion" class="chart"></div>
                </div>
            </div>
        </div>
        <!-- Modal-->
        <div id="modalGraficaPlanesAccion" class="modal fade" role="dialog">
            <div class="modal-dialog">
                <!-- Modal content-->
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal">&times;</button>
                        <h4 class="modal-title">Unidades</h4>
                    </div>
                    <div class="modal-body">
                        <asp:CheckBoxList ID="checkboxPAccion" runat="server">
                        </asp:CheckBoxList>
                    </div>
                    <div class="modal-footer">
                        <asp:Button ID="btnActualizarSGC" Text="Actualizar" UseSubmitBehavior="false" class="btn btn-info" data-dismiss="modal" OnClick="actualizarCkeckbox_Click" runat="server" />
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div class="row">

        <div class="col-md-12">
            <div class="panel panel-default" data-toggle="collapse" data-target=".navbar-collapse">
                <div class="panel-heading">
                    <h4 class="panel-title">
                        <a data-toggle="modal" data-target="#modalGraficaPlanesAccionAbiertos" data-backdrop="static">Estatus Planes de Acción Abiertos</a>
                    </h4>
                </div>
                <div class="panel-body" style="height: 350px">
                    <div id="graficaPlanesAccionAbiertos" class="chart"></div>
                </div>
            </div>
        </div>

        <!-- Modal-->
        <div id="modalGraficaPlanesAccionAbiertos" class="modal fade" role="dialog">
            <div class="modal-dialog">

                <!-- Modal content-->
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal">&times;</button>
                        <h4 class="modal-title">Unidades</h4>
                    </div>
                    <div class="modal-body">
                        <asp:CheckBoxList ID="checkboxPAccionAbiertos" RepeatColumns="2" CellPadding="5" CellSpacing="8" runat="server">
                        </asp:CheckBoxList>
                    </div>
                    <div class="modal-footer">
                        <asp:Button ID="btnActualizarPlanesAbiertos" Text="Actualizar" UseSubmitBehavior="false" class="btn btn-info" data-dismiss="modal" OnClick="actualizarCkeckbox_Click" runat="server" />
                    </div>
                </div>
            </div>
        </div>

    </div>
    <div class="row">
        <div class="col-md-12">
            <div class="panel panel-default" data-toggle="collapse" data-target=".navbar-collapse">
                <div class="panel-heading">
                    <h4 class="panel-title">
                        <a data-toggle="collapse">Verificación de Eficacia Planes de Acción Cerrados</a>
                    </h4>
                </div>
                <div class="panel-body">
                    <div id="graficaVerificacionEficacia" class="chart"></div>
                </div>
            </div>
        </div>
    </div>
    <%}
        else
        {%>
    <h2>&nbsp;</h2>    
    <h2 style="color:white"><b>Bienvenido al Sistema de Gestión de Calidad</b></h2>
    <%} %>
</asp:Content>
