<!-- 

Copyright (c) University of Pennsylvania 2004
With permission of Carl A. Gunter, this code can be used for teaching, experimentation,
and research.

Programmer: Kevin Lux

-->
<%@ Page language="c#" Codebehind="ListUsers.aspx.cs" AutoEventWireup="false" Inherits="WSEmailServer.ListUsers" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>WS Email : List Accounts</title>
		<meta name="GENERATOR" Content="Microsoft Visual Studio 7.0">
		<meta name="CODE_LANGUAGE" Content="C#">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
	</HEAD>
	<body MS_POSITIONING="GridLayout" bgColor="#ffffcc">
		<form id="ListUsers" method="post" runat="server">
			<asp:Image id="Image1" style="Z-INDEX: 101; LEFT: 8px; POSITION: absolute; TOP: 8px" runat="server" ImageUrl="logo-non.gif" BackColor="Transparent"></asp:Image>
			<asp:Label id="Label5" style="Z-INDEX: 102; LEFT: 227px; POSITION: absolute; TOP: 69px" runat="server" Font-Names="Arial" Font-Bold="True" Font-Italic="True" Font-Size="Large" Height="28px" Width="262px">WSEmail Account List</asp:Label>
			<HR style="Z-INDEX: 103; LEFT: 7px; POSITION: absolute; TOP: 123px; HEIGHT: 1px" width="480" SIZE="1">
			<asp:HyperLink id="HyperLink1" style="Z-INDEX: 104; LEFT: 382px; POSITION: absolute; TOP: 96px" runat="server" NavigateUrl="NewAccount.aspx">Create Account</asp:HyperLink>
			<%
			
			string output = GetUsers();
			Response.Write("<br><br><br><br><br><br><font face=arial>"+output+"</font>");

			
			%>
		</form>
	</body>
</HTML>
