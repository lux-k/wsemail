/*

Copyright (c) University of Pennsylvania 2004
With permission of Carl A. Gunter, this code can be used for teaching, experimentation,
and research.

Programmer: Kevin Lux

*/

using System;

namespace SysManCommon
{
	/// <summary>
	/// Summary description for Class1.
	/// </summary>
	public enum RequestType {DataRequest, CoreStatus, UnloadPlugin, LoadPlugin, ServerRestart,PluginStatus};
	public enum DataRequested {ServerName, ServerURL, StartTime, Uptime, SMTPRelay, PluginList, Router,
		DNSServer, Certificate,	Database, LocalMTA, DeliveryQueue, MessageAccessor,DataAccessor};
	public enum PluginTypes {Service, ExtensionProcessor, DeliveryProcessor, MappedAddress, SendingProcessor, All};
	public enum CorePlugins {DatabaseManager,MessageQueue,LocalMTA,MessageAccessor,DataAccessor};
}
