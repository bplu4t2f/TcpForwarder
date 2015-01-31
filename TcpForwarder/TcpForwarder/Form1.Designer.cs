namespace TcpForwarder
{
	partial class Form1
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.textBoxSourcePort = new System.Windows.Forms.TextBox();
			this.labelSourcePort = new System.Windows.Forms.Label();
			this.labelTargetIP = new System.Windows.Forms.Label();
			this.textBoxTargetIP = new System.Windows.Forms.TextBox();
			this.buttonStart = new System.Windows.Forms.Button();
			this.buttonStop = new System.Windows.Forms.Button();
			this.textBoxTargetPort = new System.Windows.Forms.TextBox();
			this.labelTargetPort = new System.Windows.Forms.Label();
			this.labelStatusHint = new System.Windows.Forms.Label();
			this.labelStatus = new System.Windows.Forms.Label();
			this.labelVersion = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// textBoxSourcePort
			// 
			this.textBoxSourcePort.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.textBoxSourcePort.Location = new System.Drawing.Point(106, 12);
			this.textBoxSourcePort.Name = "textBoxSourcePort";
			this.textBoxSourcePort.Size = new System.Drawing.Size(149, 20);
			this.textBoxSourcePort.TabIndex = 1;
			// 
			// labelSourcePort
			// 
			this.labelSourcePort.AutoSize = true;
			this.labelSourcePort.Location = new System.Drawing.Point(12, 15);
			this.labelSourcePort.Name = "labelSourcePort";
			this.labelSourcePort.Size = new System.Drawing.Size(66, 13);
			this.labelSourcePort.TabIndex = 0;
			this.labelSourcePort.Text = "Source Port:";
			// 
			// labelTargetIP
			// 
			this.labelTargetIP.AutoSize = true;
			this.labelTargetIP.Location = new System.Drawing.Point(12, 41);
			this.labelTargetIP.Name = "labelTargetIP";
			this.labelTargetIP.Size = new System.Drawing.Size(54, 13);
			this.labelTargetIP.TabIndex = 2;
			this.labelTargetIP.Text = "Target IP:";
			// 
			// textBoxTargetIP
			// 
			this.textBoxTargetIP.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.textBoxTargetIP.Location = new System.Drawing.Point(106, 38);
			this.textBoxTargetIP.Name = "textBoxTargetIP";
			this.textBoxTargetIP.Size = new System.Drawing.Size(149, 20);
			this.textBoxTargetIP.TabIndex = 3;
			// 
			// buttonStart
			// 
			this.buttonStart.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonStart.Location = new System.Drawing.Point(180, 152);
			this.buttonStart.Name = "buttonStart";
			this.buttonStart.Size = new System.Drawing.Size(75, 23);
			this.buttonStart.TabIndex = 7;
			this.buttonStart.Text = "Start";
			this.buttonStart.UseVisualStyleBackColor = true;
			this.buttonStart.Click += new System.EventHandler(this.buttonStart_Click);
			// 
			// buttonStop
			// 
			this.buttonStop.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonStop.Location = new System.Drawing.Point(99, 152);
			this.buttonStop.Name = "buttonStop";
			this.buttonStop.Size = new System.Drawing.Size(75, 23);
			this.buttonStop.TabIndex = 6;
			this.buttonStop.Text = "Stop";
			this.buttonStop.UseVisualStyleBackColor = true;
			this.buttonStop.Click += new System.EventHandler(this.buttonStop_Click);
			// 
			// textBoxTargetPort
			// 
			this.textBoxTargetPort.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.textBoxTargetPort.Location = new System.Drawing.Point(106, 64);
			this.textBoxTargetPort.Name = "textBoxTargetPort";
			this.textBoxTargetPort.Size = new System.Drawing.Size(149, 20);
			this.textBoxTargetPort.TabIndex = 5;
			// 
			// labelTargetPort
			// 
			this.labelTargetPort.AutoSize = true;
			this.labelTargetPort.Location = new System.Drawing.Point(12, 67);
			this.labelTargetPort.Name = "labelTargetPort";
			this.labelTargetPort.Size = new System.Drawing.Size(63, 13);
			this.labelTargetPort.TabIndex = 4;
			this.labelTargetPort.Text = "Target Port:";
			// 
			// labelStatusHint
			// 
			this.labelStatusHint.AutoSize = true;
			this.labelStatusHint.Location = new System.Drawing.Point(12, 107);
			this.labelStatusHint.Name = "labelStatusHint";
			this.labelStatusHint.Size = new System.Drawing.Size(40, 13);
			this.labelStatusHint.TabIndex = 8;
			this.labelStatusHint.Text = "Status:";
			// 
			// labelStatus
			// 
			this.labelStatus.AutoSize = true;
			this.labelStatus.Location = new System.Drawing.Point(103, 107);
			this.labelStatus.Name = "labelStatus";
			this.labelStatus.Size = new System.Drawing.Size(24, 13);
			this.labelStatus.TabIndex = 9;
			this.labelStatus.Text = "Idle";
			// 
			// labelVersion
			// 
			this.labelVersion.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.labelVersion.AutoSize = true;
			this.labelVersion.Location = new System.Drawing.Point(12, 157);
			this.labelVersion.Name = "labelVersion";
			this.labelVersion.Size = new System.Drawing.Size(47, 13);
			this.labelVersion.TabIndex = 10;
			this.labelVersion.Text = "(version)";
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(267, 187);
			this.Controls.Add(this.labelVersion);
			this.Controls.Add(this.labelStatus);
			this.Controls.Add(this.labelStatusHint);
			this.Controls.Add(this.buttonStop);
			this.Controls.Add(this.buttonStart);
			this.Controls.Add(this.textBoxTargetIP);
			this.Controls.Add(this.labelTargetIP);
			this.Controls.Add(this.labelTargetPort);
			this.Controls.Add(this.labelSourcePort);
			this.Controls.Add(this.textBoxTargetPort);
			this.Controls.Add(this.textBoxSourcePort);
			this.Name = "Form1";
			this.Text = "TCP Forwarder";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TextBox textBoxSourcePort;
		private System.Windows.Forms.Label labelSourcePort;
		private System.Windows.Forms.Label labelTargetIP;
		private System.Windows.Forms.TextBox textBoxTargetIP;
		private System.Windows.Forms.Button buttonStart;
		private System.Windows.Forms.Button buttonStop;
		private System.Windows.Forms.TextBox textBoxTargetPort;
		private System.Windows.Forms.Label labelTargetPort;
		private System.Windows.Forms.Label labelStatusHint;
		private System.Windows.Forms.Label labelStatus;
		private System.Windows.Forms.Label labelVersion;
	}
}

