<!-- 

Copyright (c) University of Pennsylvania 2004
With permission of Carl A. Gunter, this code can be used for teaching, experimentation,
and research.

Programmer: Kevin Lux

-->
<%@ Page language="c#" Codebehind="Inbox.aspx.cs" AutoEventWireup="false" Inherits="webmail.Inbox" %>
<HTML>
	<HEAD>
		<title>WSEmail Web Access : Inbox</title><!-- InstanceBegin template="/Templates/MainTemplate.dwt.aspx" codeOutsideHTMLIsLocked="false" -->
		<!-- InstanceBeginEditable name="doctitle" -->
		<!-- InstanceEndEditable -->
		<meta http-equiv="Content-Type" content="text/html; charset=iso-8859-1">
		<!-- InstanceBeginEditable name="head" -->
		<!-- InstanceEndEditable -->
	</HEAD>
	<body bgColor="#ffffcc">
		<table width="100%">
			<tr>
				<td align="right"><IMG height="57" src="logo-non.gif" width="480"><i>
						<h2>WSEmail Project<br>
							<br>
							<center>WSEmail Web Access</center>
						</h2>
					</i>
					<br>
					<A href="Send.aspx">Send Message</A> | <A href="Inbox.aspx">Check Mail</A> | <A href="Logout.aspx">
						Logout</A></td>
			</tr>
			<tr>
				<td align="center" bgColor="#ffff99"><br> <!-- InstanceBeginEditable name="Content" -->
					<form runat="server">
						<asp:datagrid id="dgInbox" runat="server" Width="664px" AutoGenerateColumns="False" BackColor="White">
							<Columns>
								<asp:HyperLinkColumn DataNavigateUrlField="MessageID" DataNavigateUrlFormatString="Read.aspx?id={0}"
									DataTextField="MessageID" HeaderText="Message #"></asp:HyperLinkColumn>
								<asp:HyperLinkColumn DataNavigateUrlField="Sender" DataNavigateUrlFormatString="Send.aspx?to={0}" DataTextField="Sender"
									HeaderText="From"></asp:HyperLinkColumn>
								<asp:BoundColumn DataField="Subject" HeaderText="Subject"></asp:BoundColumn>
								<asp:BoundColumn DataField="Timestamp" HeaderText="Date"></asp:BoundColumn>
							</Columns>
						</asp:datagrid></form> <!-- InstanceEndEditable --></td>
			</tr>
		</table>
		<!-- InstanceEnd -->
	</body>
</HTML>
