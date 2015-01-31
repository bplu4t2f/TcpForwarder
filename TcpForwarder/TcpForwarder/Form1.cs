using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.Diagnostics;

namespace TcpForwarder
{
	public partial class Form1 : Form
	{
		public Form1()
		{
			InitializeComponent();

			this.labelVersion.Text = Application.ProductVersion;

			var settings = this.tempSettingsManager.Load();
			this.textBoxSourcePort.Text = settings.SourcePort;
			this.textBoxTargetIP.Text = settings.TargetIP;
			this.textBoxTargetPort.Text = settings.TargetPort;
		}

		private readonly TempSettingsManager tempSettingsManager = new TempSettingsManager();
		private readonly object sync = new object();
		private TcpListener listener;
		private TcpClient sourceClient;
		private NetworkStream sourceStream;
		private TcpClient targetClient;
		private NetworkStream targetStream;

		private bool isRunning
		{
			get { return this.listener != null; }
		}

		private readonly byte[] sourceBuffer = new byte[1024];
		private readonly byte[] targetBuffer = new byte[1024];

		private void buttonStart_Click(object sender, EventArgs e)
		{
			if (this.isRunning)
			{
				MessageBox.Show("Already routing...");
				return;
			}

			int sourcePort;
			IPAddress targetIP;
			int targetPort;

			try
			{
				sourcePort = Int32.Parse(this.textBoxSourcePort.Text);
			}
			catch (Exception ex)
			{
				MessageBox.Show("Invalid source port: " + ex.Message);
				return;
			}
			try
			{
				targetIP = IPAddress.Parse(this.textBoxTargetIP.Text);
			}
			catch (Exception ex)
			{
				MessageBox.Show("Invalid IP address: " + ex.Message);
				return;
			}
			try
			{
				targetPort = Int32.Parse(this.textBoxTargetPort.Text);
			}
			catch (Exception ex)
			{
				MessageBox.Show("Invalid target port: " + ex.Message);
				return;
			}

			lock (this.sync)
			{
				try
				{
					this.listener = new TcpListener(IPAddress.Any, sourcePort);
					this.listener.Start();
					this.listener.BeginAcceptTcpClient(this.ClientConnectedCallback, this.listener);
				}
				catch (Exception ex)
				{
					Debug.WriteLine(ex.ToString());
					this.Stop();
					MessageBox.Show("Could not begin listening: " + ex.Message);
					return;
				}

				try
				{
					this.targetClient = new TcpClient();
					var ar = this.targetClient.BeginConnect(targetIP, targetPort, null, null);
					var success = ar.AsyncWaitHandle.WaitOne(500);
					if (!success)
					{
						throw new Exception("The connection attempt timed out.");
					}
					this.targetClient.EndConnect(ar);
					this.targetStream = this.targetClient.GetStream();
				}
				catch (Exception ex)
				{
					Debug.WriteLine(ex.ToString());
					this.Stop();
					MessageBox.Show("Could not connect to the target end point: " + ex.Message);
					return;
				}

				this.labelStatus.Text = "Connected";
			}
		}

		private void ClientConnectedCallback(IAsyncResult ar)
		{
			try
			{
				lock (this.sync)
				{
					if (!this.isRunning)
					{
						return;
					}
					this.sourceClient = listener.EndAcceptTcpClient(ar);
					this.sourceStream = this.sourceClient.GetStream();
					this.listener.Stop();

					Debug.WriteLine("Connected");

					this.ReadSourceStream();
					this.ReadTargetStream();
				}
			}
			catch (Exception ex)
			{
				Debug.WriteLine(ex.ToString());
				this.Stop();
			}
		}

		private void ReadSourceStream()
		{
			this.sourceStream.BeginRead(this.sourceBuffer, 0, this.sourceBuffer.Length, this.SourceReceivedCallback, this.sourceStream);
		}

		private void SourceReceivedCallback(IAsyncResult ar)
		{
			try
			{
				lock (this.sync)
				{
					if (!this.isRunning)
					{
						return;
					}
					var numReceived = ((NetworkStream)ar.AsyncState).EndRead(ar);
					if (numReceived <= 0)
					{
						// connection closed
						this.Stop();
						return;
					}
					var sb = new StringBuilder();
					for (int i = 0; i < numReceived; ++i)
					{
						sb.Append(this.sourceBuffer[i].ToString("x2"));
						sb.Append(" ");
					}
					this.targetStream.Write(this.sourceBuffer, 0, numReceived);
					Debug.WriteLine("From source: " + sb.ToString());
					this.ReadSourceStream();
				}
			}
			catch (Exception ex)
			{
				Debug.WriteLine(ex.ToString());
				this.Stop();
			}
		}

		private void ReadTargetStream()
		{
			this.targetStream.BeginRead(this.targetBuffer, 0, this.targetBuffer.Length, this.TargetReceivedCallback, this.targetStream);
		}

		private void TargetReceivedCallback(IAsyncResult ar)
		{
			try
			{
				lock (this.sync)
				{
					if (!this.isRunning)
					{
						return;
					}
					var numReceived = ((NetworkStream)ar.AsyncState).EndRead(ar);
					if (numReceived <= 0)
					{
						// connection closed
						this.Stop();
						return;
					}
					var sb = new StringBuilder();
					for (int i = 0; i < numReceived; ++i)
					{
						sb.Append(this.targetBuffer[i].ToString("x2"));
						sb.Append(" ");
					}
					this.sourceStream.Write(this.targetBuffer, 0, numReceived);
					Debug.WriteLine("From target: " + sb.ToString());
					this.ReadTargetStream();
				}
			}
			catch (Exception ex)
			{
				Debug.WriteLine(ex.ToString());
				this.Stop();
			}
		}

		private void buttonStop_Click(object sender, EventArgs e)
		{
			this.Stop();
		}

		protected override void OnClosing(CancelEventArgs e)
		{
			this.Stop();

			var settings = new TcpForwarderSettings();
			settings.SourcePort = this.textBoxSourcePort.Text;
			settings.TargetIP = this.textBoxTargetIP.Text;
			settings.TargetPort = this.textBoxTargetPort.Text;
			this.tempSettingsManager.Save(settings);

			base.OnClosing(e);
		}

		private void Stop()
		{
			lock (this.sync)
			{
				if (this.sourceClient != null)
				{
					this.sourceClient.Close();
					this.sourceClient = null;
				}
				if (this.targetClient != null)
				{
					this.targetClient.Close();
					this.targetClient = null;
				}
				if (this.listener != null)
				{
					this.listener.Stop();
					this.listener = null;
				}

				this.SetStatusDisconnected();
			}
		}

		private void SetStatusDisconnected()
		{
			if (this.InvokeRequired)
			{
				this.Invoke(new MethodInvoker(this.SetStatusDisconnected));
				return;
			}
			this.labelStatus.Text = "Disconnected";
		}
	}
}
