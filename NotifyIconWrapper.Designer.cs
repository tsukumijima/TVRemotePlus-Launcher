﻿namespace TVRemotePlus_Launcher
{
	partial class NotifyIconWrapper
	{
		/// <summary>
		/// 必要なデザイナー変数です。
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary> 
		/// 使用中のリソースをすべてクリーンアップします。
		/// </summary>
		/// <param name="disposing">マネージド リソースを破棄する場合は true を指定し、その他の場合は false を指定します。</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region コンポーネント デザイナーで生成されたコード

		/// <summary>
		/// デザイナー サポートに必要なメソッドです。このメソッドの内容を
		/// コード エディターで変更しないでください。
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NotifyIconWrapper));
			this.notifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
			this.contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.toolStripMenuItem_Open = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem_Exit = new System.Windows.Forms.ToolStripMenuItem();
			this.contextMenuStrip.SuspendLayout();
			// 
			// notifyIcon
			// 
			this.notifyIcon.ContextMenuStrip = this.contextMenuStrip;
			this.notifyIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon.Icon")));
			this.notifyIcon.Text = "TVRemotePlus-Launcher";
			this.notifyIcon.Visible = true;
			// 
			// contextMenuStrip
			// 
			this.contextMenuStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
			this.contextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem_Open,
            this.toolStripMenuItem_Exit});
			this.contextMenuStrip.Name = "contextMenuStrip";
			this.contextMenuStrip.Size = new System.Drawing.Size(235, 56);
			// 
			// toolStripMenuItem_Open
			// 
			this.toolStripMenuItem_Open.Image = ((System.Drawing.Image)(resources.GetObject("toolStripMenuItem_Open.Image")));
			this.toolStripMenuItem_Open.Name = "toolStripMenuItem_Open";
			this.toolStripMenuItem_Open.Size = new System.Drawing.Size(234, 26);
			this.toolStripMenuItem_Open.Text = "サーバー設定…";
			// 
			// toolStripMenuItem_Exit
			// 
			this.toolStripMenuItem_Exit.Image = ((System.Drawing.Image)(resources.GetObject("toolStripMenuItem_Exit.Image")));
			this.toolStripMenuItem_Exit.Name = "toolStripMenuItem_Exit";
			this.toolStripMenuItem_Exit.Size = new System.Drawing.Size(234, 26);
			this.toolStripMenuItem_Exit.Text = "TVRemotePlus を終了";
			this.contextMenuStrip.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.NotifyIcon notifyIcon;
		private System.Windows.Forms.ContextMenuStrip contextMenuStrip;
		private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem_Open;
		private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem_Exit;
	}
}
