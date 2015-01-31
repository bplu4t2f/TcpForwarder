using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace TcpForwarder
{
	class TempSettingsManager
	{
		public TempSettingsManager()
		{
			var tempPath = Path.GetTempPath();
			this.fullPath = Path.Combine(tempPath, this.fileName);
			this.ser = new XmlSerializer(typeof(TcpForwarderSettings));
		}

		private readonly string fileName = "TcpReplicatorSettings.xml";
		private readonly string fullPath;
		private readonly XmlSerializer ser;

		public TcpForwarderSettings Load()
		{
			try
			{
				using (var file = File.OpenRead(this.fullPath))
				{
					return (TcpForwarderSettings)ser.Deserialize(file);
				}
			}
			catch
			{
				return new TcpForwarderSettings();
			}
		}

		public void Save(TcpForwarderSettings settings)
		{
			try
			{
				using (var file = File.OpenWrite(this.fullPath))
				{
					this.ser.Serialize(file, settings);
				}
			}
			catch
			{
			}
		}
	}

	public class TcpForwarderSettings
	{
		public string SourcePort { get; set; }
		public string TargetIP { get; set; }
		public string TargetPort { get; set; }
	}
}
