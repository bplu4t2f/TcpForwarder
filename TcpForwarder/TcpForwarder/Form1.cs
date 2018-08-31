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

		private class State
		{
			public State(Form1 owner, IPAddress targetIP, int targetPort)
			{
				this.owner = owner ?? throw new ArgumentNullException(nameof(owner));
				this.targetIP = targetIP ?? throw new ArgumentNullException(nameof(targetIP));
				this.targetPort = targetPort;
			}

			private readonly Form1 owner;
			private readonly IPAddress targetIP;
			public readonly int targetPort;
			private readonly object sync = new object();
			private readonly byte[] sourceBuffer = new byte[1024];
			private readonly byte[] targetBuffer = new byte[1024];

			private TcpClient sourceClient;
			private NetworkStream sourceStream;
			private TcpClient targetClient;
			private NetworkStream targetStream;
			private bool isRunning;


			public void Start(TcpClient sourceClient)
			{
				lock (this.sync)
				{
					this.isRunning = true;

					this.sourceClient = sourceClient;
					this.sourceClient.NoDelay = true;
					this.sourceStream = this.sourceClient.GetStream();

					Debug.WriteLine("Client connected");

					this.targetClient = new TcpClient();
					this.targetClient.NoDelay = true;
					this.targetClient.BeginConnect(this.targetIP, this.targetPort, this.ConnectedToTargetCallback, null);
				}
			}

			private void ConnectedToTargetCallback(IAsyncResult ar)
			{
				lock (this.sync)
				{
					if (!this.isRunning)
					{
						return;
					}
					try
					{
						this.targetClient.EndConnect(ar);
					}
					catch
					{
						// If this doesn't work the target is gone. Replicate to source by closing.
						this.Stop();
						this.NotifyAnyClientDisconnected();
						return;
					}

					Debug.WriteLine("Connected to target");
					this.owner.SetStatus("Target connected");

					this.targetStream = this.targetClient.GetStream();

					this.ReadSourceStream();
					this.ReadTargetStream();
				}
			}

			public void Stop()
			{
				lock (this.sync)
				{
					this.isRunning = false;
					this.sourceStream?.Dispose();
					this.sourceClient?.Close();
					this.targetStream?.Dispose();
					this.targetClient?.Close();
				}
			}

			private void NotifyAnyClientDisconnected()
			{
				this.owner.NotifyAnyClientDisconnected();
			}

			private void ReadSourceStream()
			{
				this.sourceStream.BeginRead(this.sourceBuffer, 0, this.sourceBuffer.Length, this.SourceReceivedCallback, this.sourceStream);
			}

			private void SourceReceivedCallback(IAsyncResult ar)
			{
				lock (this.sync)
				{
					if (!this.isRunning)
					{
						return;
					}
					int numReceived;
					try
					{
						numReceived = ((NetworkStream)ar.AsyncState).EndRead(ar);
					}
					catch (Exception ex)
					{
						Debug.WriteLine("source died: " + ex);
						this.Stop();
						this.NotifyAnyClientDisconnected();
						return;
					}
					if (numReceived <= 0)
					{
						Debug.WriteLine("source disconnected");
						// source closed -> replicate to target by closing target as well.
						this.Stop();
						// wait for a new source
						this.NotifyAnyClientDisconnected();
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

			private void ReadTargetStream()
			{
				this.targetStream.BeginRead(this.targetBuffer, 0, this.targetBuffer.Length, this.TargetReceivedCallback, this.targetStream);
			}

			private void TargetReceivedCallback(IAsyncResult ar)
			{
				lock (this.sync)
				{
					if (!this.isRunning)
					{
						return;
					}
					int numReceived;
					try
					{
						numReceived = ((NetworkStream)ar.AsyncState).EndRead(ar);
					}
					catch (Exception ex)
					{
						Debug.WriteLine("source died: " + ex);
						this.Stop();
						this.NotifyAnyClientDisconnected();
						return;
					}
					if (numReceived <= 0)
					{
						Debug.WriteLine("target disconnected");
						// target close -> replicate to source by closing source as well.
						this.Stop();
						// wait for a new source
						this.NotifyAnyClientDisconnected();
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
		}

		private bool isRunning
		{
			get { return this.listener != null; }
		}

		private int sourcePort;
		private IPAddress targetIP;
		private int targetPort;

		private void buttonStart_Click(object sender, EventArgs e)
		{
			if (this.isRunning)
			{
				MessageBox.Show("Already routing...");
				return;
			}

			try
			{
				this.sourcePort = Int32.Parse(this.textBoxSourcePort.Text);
			}
			catch (Exception ex)
			{
				MessageBox.Show("Invalid source port: " + ex.Message);
				return;
			}
			try
			{
				this.targetIP = IPAddress.Parse(this.textBoxTargetIP.Text);
			}
			catch (Exception ex)
			{
				MessageBox.Show("Invalid IP address: " + ex.Message);
				return;
			}
			try
			{
				this.targetPort = Int32.Parse(this.textBoxTargetPort.Text);
			}
			catch (Exception ex)
			{
				MessageBox.Show("Invalid target port: " + ex.Message);
				return;
			}

			lock (this.sync)
			{
				this.listener = new TcpListener(IPAddress.Any, this.sourcePort);
				this.Listen();
			}
		}

		private void Listen()
		{
			this.listener.Start();
			this.listener.BeginAcceptTcpClient(this.ClientConnectedCallback, this.listener);
			this.SetStatus("Listening");
		}

		private void NotifyAnyClientDisconnected()
		{
			lock (this.sync)
			{
				this.state = null;
				this.Listen();
			}
		}

		State state;

		private void ClientConnectedCallback(IAsyncResult ar)
		{
			lock (this.sync)
			{
				if (!this.isRunning)
				{
					return;
				}

				Debug.Assert(this.state == null);
				this.state = new State(this, this.targetIP, this.targetPort);
				var sourceClient = this.listener.EndAcceptTcpClient(ar);

				this.listener.Stop();

				this.SetStatus("Source connected");

				this.state.Start(sourceClient);
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
				this.state?.Stop();
				this.state = null;
				if (this.listener != null)
				{
					this.listener.Stop();
					this.listener = null;
				}

				this.SetStatus("Idle");
			}
		}

		private void SetStatus(string text)
		{
			if (this.InvokeRequired)
			{
				this.Invoke(new MethodInvoker(() => this.SetStatus(text)));
				return;
			}
			this.labelStatus.Text = text;
		}
	}
}
