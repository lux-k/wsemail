<!-- 

Copyright (c) University of Pennsylvania 2004
With permission of Carl A. Gunter, this code can be used for teaching, experimentation,
and research.

Programmer: Kevin Lux

-->
<%@ Page language="c#" Codebehind="NewAccount.aspx.cs" AutoEventWireup="false" Inherits="WSEmailServer.NewAccount" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>WS Email : Create Account</title>
		<meta content="Microsoft Visual Studio 7.0" name="GENERATOR">
		<meta content="C#" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
	</HEAD>
	<body bgColor="#ffffcc" MS_POSITIONING="GridLayout" link="#000ea0">
		<form id="NewAccount" method="post" runat="server">
			<asp:dropdownlist id="DropDownList1" style="Z-INDEX: 107; LEFT: 332px; POSITION: absolute; TOP: 261px" tabIndex="4" runat="server" Width="146px" Enabled="False" Font-Names="Arial"></asp:dropdownlist><asp:label id="Label4" style="Z-INDEX: 110; LEFT: 137px; POSITION: absolute; TOP: 261px" runat="server" Font-Names="Arial">Server:</asp:label><asp:label id="Label3" style="Z-INDEX: 106; LEFT: 137px; POSITION: absolute; TOP: 221px" runat="server" Font-Names="Arial">Confirm Password:</asp:label><asp:label id="Label2" style="Z-INDEX: 103; LEFT: 137px; POSITION: absolute; TOP: 181px" runat="server" Font-Names="Arial">Password:</asp:label><asp:label id="Label1" style="Z-INDEX: 102; LEFT: 137px; POSITION: absolute; TOP: 141px" runat="server" Font-Names="Arial">User name:</asp:label><asp:textbox id="txtPass2" style="Z-INDEX: 105; LEFT: 332px; POSITION: absolute; TOP: 221px" tabIndex="3" runat="server" TextMode="Password" Font-Names="Arial"></asp:textbox><asp:textbox id="txtPass1" style="Z-INDEX: 104; LEFT: 332px; POSITION: absolute; TOP: 181px" tabIndex="2" runat="server" TextMode="Password" Font-Names="Arial"></asp:textbox><asp:textbox id="txtUser" style="Z-INDEX: 101; LEFT: 332px; POSITION: absolute; TOP: 141px" tabIndex="1" runat="server" Font-Names="Arial"></asp:textbox><asp:button id="Button1" style="Z-INDEX: 108; LEFT: 372px; POSITION: absolute; TOP: 328px" tabIndex="5" runat="server" Width="108px" Height="50px" Text="Create..." Font-Names="Arial"></asp:button><asp:label id="lblResponse" style="Z-INDEX: 109; LEFT: 42px; POSITION: absolute; TOP: 426px" runat="server" Width="488px" Height="129px" Font-Names="Arial"></asp:label>
			<asp:Image id="Image1" style="Z-INDEX: 111; LEFT: 5px; POSITION: absolute; TOP: 12px" runat="server" ImageUrl="logo-non.gif" BackColor="Transparent"></asp:Image>
			<asp:Label id="Label5" style="Z-INDEX: 112; LEFT: 176px; POSITION: absolute; TOP: 71px" runat="server" Width="316px" Height="28px" Font-Names="Arial" Font-Bold="True" Font-Italic="True" Font-Size="Large">WSEmail Account Creation</asp:Label>
			<HR style="Z-INDEX: 113; LEFT: 8px; POSITION: absolute; TOP: 125px; HEIGHT: 1px" width="480" SIZE="1">
			<asp:HyperLink id="HyperLink1" style="Z-INDEX: 114; LEFT: 423px; POSITION: absolute; TOP: 100px" runat="server" NavigateUrl="ListUsers.aspx">List Users</asp:HyperLink>
		</form>
	</body>
</HTML>
