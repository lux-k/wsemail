<!-- 

Copyright (c) University of Pennsylvania 2004
With permission of Carl A. Gunter, this code can be used for teaching, experimentation,
and research.

Programmer: Kevin Lux

-->
<%@ Page language="c#" Codebehind="Read.aspx.cs" AutoEventWireup="false" Inherits="webmail.Read" %>
<HTML>
	<HEAD>
		<title>WSEmail Web Access : Read Message</title><!-- InstanceBegin template="/Templates/MainTemplate.dwt.aspx" codeOutsideHTMLIsLocked="false" -->
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
					<form id="Form1" method="post" runat="server">
						<table>
							<tr>
								<td>To:</td>
								<td><asp:TextBox id="To" runat="server" ReadOnly="True"></asp:TextBox></td>
							</tr>
							<tr>
								<td>From:</td>
								<td><asp:TextBox id="From" runat="server" ReadOnly="True"></asp:TextBox></td>
							</tr>
							<tr>
								<td>Subject:</td>
								<td><asp:TextBox id="Subject" runat="server" ReadOnly="True"></asp:TextBox></td>
							</tr>
							<tr>
								<td>Date:</td>
								<td><asp:TextBox id="Date" runat="server" ReadOnly="True"></asp:TextBox></td>
							</tr>
							<tr>
								<td>Message:</td>
								<td><asp:TextBox id="Message" runat="server" TextMode="MultiLine" Width="344" Height="240" ReadOnly="True"></asp:TextBox></td>
							</tr>
							<tr>
								<td>&nbsp;</td>
								<td><asp:hyperlink ID="linkReply" runat="server">Reply...</asp:hyperlink>&nbsp;</td>
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
