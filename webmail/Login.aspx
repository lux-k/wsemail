<!-- 

Copyright (c) University of Pennsylvania 2004
With permission of Carl A. Gunter, this code can be used for teaching, experimentation,
and research.

Programmer: Kevin Lux

-->
<%@ Page language="c#" Codebehind="Login.aspx.cs" AutoEventWireup="false" Inherits="webmail.WebForm1" %>
<HTML>
	<HEAD>
		<title>WSEmail Web Access : Login</title><!-- InstanceBegin template="/Templates/MainTemplate.dwt.aspx" codeOutsideHTMLIsLocked="false" -->
		<!-- InstanceBeginEditable name="doctitle" -->
		<!-- InstanceEndEditable -->
		<meta http-equiv="Content-Type" content="text/html; charset=iso-8859-1">
		<!-- InstanceBeginEditable name="head" -->
		<!-- InstanceEndEditable -->
	</HEAD>
	<body bgcolor="#ffffcc">
		<table width="100%">
			<tr>
				<td align="right"><img src="logo-non.gif" width="480" height="57"><i><h2>WSEmail Project<br>
							<br>
							<center>WSEmail Web Access</center>
						</h2>
					</i>
					<br>
					<a href="Send.aspx">Send Message</a> | <a href="Inbox.aspx">Check Mail</a> | <a href="Logout.aspx">
						Logout</a></td>
			</tr>
			<tr>
				<td bgcolor="#ffff99" align="center"><br>
					<!-- InstanceBeginEditable name="Content" -->
					<p>You are required to login with your WSEmail credentials to access that function!</p>
					<form id="Form1" method="post" runat="server">
						<table>
							<tr>
								<td>Username:</td>
								<td><asp:TextBox id="Username" runat="server"></asp:TextBox>
								</td>
							</tr>
							<tr>
								<td>Password:</td>
								<td><asp:TextBox id="Password" runat="server" TextMode="Password"></asp:TextBox></td>
							</tr>
							<tr>
								<td>&nbsp;</td>
								<td><asp:Button id="Button1" runat="server" Text="Login"></asp:Button></td>
							</tr>
						</table>
					</form>
					<!-- InstanceEndEditable --><br>
				</td>
			</tr>
		</table>
		<!-- InstanceEnd -->
	</body>
</HTML>
