<!-- 

Copyright (c) University of Pennsylvania 2004
With permission of Carl A. Gunter, this code can be used for teaching, experimentation,
and research.

Programmer: Kevin Lux

-->
<%@ Page language="c#" Codebehind="default.aspx.cs" AutoEventWireup="false" Inherits="WSEmailServer.Default" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>WS Email : Server Home Page</title>
		<meta name="GENERATOR" Content="Microsoft Visual Studio 7.0">
		<meta name="CODE_LANGUAGE" Content="C#">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
	</HEAD>
	<body MS_POSITIONING="GridLayout" bgColor="#ffffcc">
		<form id="default" method="post" runat="server">
			<asp:Image id="Image1" style="Z-INDEX: 101; LEFT: 8px; POSITION: absolute; TOP: 8px" runat="server"
				ImageUrl="logo-non.gif" BackColor="Transparent"></asp:Image>
			<asp:HyperLink id="HyperLink2" style="Z-INDEX: 105; LEFT: 187px; POSITION: absolute; TOP: 185px"
				runat="server" NavigateUrl="ListUsers.aspx">List Accounts</asp:HyperLink>
			<asp:Label id="Label5" style="Z-INDEX: 102; LEFT: 297px; POSITION: absolute; TOP: 69px" runat="server"
				Font-Names="Arial" Font-Bold="True" Font-Italic="True" Font-Size="Large" Height="28px"
				Width="393px">WS Email Server</asp:Label>
			<HR style="Z-INDEX: 103; LEFT: 11px; POSITION: absolute; TOP: 121px; HEIGHT: 1px" width="480"
				SIZE="1">
			<asp:HyperLink id="HyperLink1" style="Z-INDEX: 104; LEFT: 182px; POSITION: absolute; TOP: 155px"
				runat="server" NavigateUrl="NewAccount.aspx">Create Account</asp:HyperLink>
			<asp:Label id="lblWarning" style="Z-INDEX: 106; LEFT: 23px; POSITION: absolute; TOP: 295px"
				runat="server" Width="462px" Height="99px" Font-Size="Large" Font-Bold="True" ForeColor="#C00000">Label</asp:Label>
		</form>
	</body>
</HTML>
