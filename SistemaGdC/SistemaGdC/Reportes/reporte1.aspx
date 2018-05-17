<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="reporte1.aspx.cs" Inherits="SistemaGdC.Reportes.WebForm1" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>


<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <rsweb:ReportViewer runat="server"></rsweb:ReportViewer>
        </div>
    </form>
</body>
</html>
