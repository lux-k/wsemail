<!-- 

Copyright (c) University of Pennsylvania 2004
With permission of Carl A. Gunter, this code can be used for teaching, experimentation,
and research.

Programmer: Kevin Lux

-->
<%@ Page language="c#" Codebehind="RegisterServer.aspx.cs" AutoEventWireup="false" Inherits="WSEmailServer.RegisterServer" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>RegisterServer</title>
		<meta name="GENERATOR" Content="Microsoft Visual Studio 7.0">
		<meta name="CODE_LANGUAGE" Content="C#">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
	</HEAD>
	<body MS_POSITIONING="GridLayout" bgColor="#ffffcc">
		<form id="RegisterServer" method="post" runat="server">
			<asp:label id="Label3" style="Z-INDEX: 117; LEFT: 140px; POSITION: absolute; TOP: 217px" runat="server" Font-Names="Arial">Administrator:</asp:label>
			<asp:label id="Label2" style="Z-INDEX: 118; LEFT: 140px; POSITION: absolute; TOP: 177px" runat="server" Font-Names="Arial">URL:</asp:label>
			<asp:label id="Label1" style="Z-INDEX: 119; LEFT: 140px; POSITION: absolute; TOP: 137px" runat="server" Font-Names="Arial">Server Name:</asp:label>
			<asp:textbox id="txtAdmin" style="Z-INDEX: 120; LEFT: 335px; POSITION: absolute; TOP: 217px" tabIndex="3" runat="server" Font-Names="Arial"></asp:textbox>
			<asp:textbox id="txtUrl" style="Z-INDEX: 121; LEFT: 335px; POSITION: absolute; TOP: 177px" tabIndex="2" runat="server" Font-Names="Arial"></asp:textbox>
			<asp:textbox id="txtServer" style="Z-INDEX: 122; LEFT: 335px; POSITION: absolute; TOP: 137px" tabIndex="1" runat="server" Font-Names="Arial"></asp:textbox>
			<asp:button id="Button1" style="Z-INDEX: 123; LEFT: 382px; POSITION: absolute; TOP: 264px" tabIndex="5" runat="server" Width="108px" Font-Names="Arial" Height="50px" Text="Create..."></asp:button>
			<asp:label id="lblResponse" style="Z-INDEX: 124; LEFT: 45px; POSITION: absolute; TOP: 422px" runat="server" Width="488px" Font-Names="Arial" Height="129px"></asp:label>
			<asp:Image id="Image1" style="Z-INDEX: 125; LEFT: 8px; POSITION: absolute; TOP: 8px" runat="server" ImageUrl="logo-non.gif" BackColor="Transparent"></asp:Image>
			<asp:Label id="Label5" style="Z-INDEX: 126; LEFT: 161px; POSITION: absolute; TOP: 67px" runat="server" Width="335px" Font-Names="Arial" Height="28px" Font-Bold="True" Font-Italic="True" Font-Size="Large">WSEmail Server Registration</asp:Label>
			<HR style="Z-INDEX: 127; LEFT: 11px; POSITION: absolute; TOP: 121px; HEIGHT: 1px" width="480" SIZE="1">
		</form>
	</body>
</HTML>
