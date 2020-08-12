namespace TVRemotePlus_Launcher
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
			this.contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.toolStripMenuItem_Access = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem_Access_HTTPS = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem_Open = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem_Exit = new System.Windows.Forms.ToolStripMenuItem();
			this.notifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
			this.contextMenuStrip.SuspendLayout();
			// 
			// contextMenuStrip
			// 
			this.contextMenuStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
			this.contextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem_Access,
            this.toolStripMenuItem_Access_HTTPS,
            this.toolStripMenuItem_Open,
            this.toolStripMenuItem_Exit});
			this.contextMenuStrip.Name = "contextMenuStrip";
			this.contextMenuStrip.Size = new System.Drawing.Size(319, 108);
			// 
			// toolStripMenuItem_Access
			// 
			this.toolStripMenuItem_Access.Image = ((System.Drawing.Image)(resources.GetObject("toolStripMenuItem_Access.Image")));
			this.toolStripMenuItem_Access.Name = "toolStripMenuItem_Access";
			this.toolStripMenuItem_Access.Size = new System.Drawing.Size(318, 26);
			this.toolStripMenuItem_Access.Text = "TVRemotePlus へアクセス";
			// 
			// toolStripMenuItem_Access_HTTPS
			// 
			this.toolStripMenuItem_Access_HTTPS.Image = ((System.Drawing.Image)(resources.GetObject("toolStripMenuItem_Access_HTTPS.Image")));
			this.toolStripMenuItem_Access_HTTPS.Name = "toolStripMenuItem_Access_HTTPS";
			this.toolStripMenuItem_Access_HTTPS.Size = new System.Drawing.Size(318, 26);
			this.toolStripMenuItem_Access_HTTPS.Text = "TVRemotePlus にアクセス (HTTPS)";
			// 
			// toolStripMenuItem_Open
			// 
			this.toolStripMenuItem_Open.Image = ((System.Drawing.Image)(resources.GetObject("toolStripMenuItem_Open.Image")));
			this.toolStripMenuItem_Open.Name = "toolStripMenuItem_Open";
			this.toolStripMenuItem_Open.Size = new System.Drawing.Size(318, 26);
			this.toolStripMenuItem_Open.Text = "サーバー (Apache) の設定";
			// 
			// toolStripMenuItem_Exit
			// 
			this.toolStripMenuItem_Exit.Image = ((System.Drawing.Image)(resources.GetObject("toolStripMenuItem_Exit.Image")));
			this.toolStripMenuItem_Exit.Name = "toolStripMenuItem_Exit";
			this.toolStripMenuItem_Exit.Size = new System.Drawing.Size(318, 26);
			this.toolStripMenuItem_Exit.Text = "サーバー (Apache) を終了";
			// 
			// notifyIcon
			// 
			this.notifyIcon.ContextMenuStrip = this.contextMenuStrip;
			this.notifyIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon.Icon")));
			this.notifyIcon.Text = "TVRemotePlus-Launcher";
			this.notifyIcon.Visible = true;
			this.contextMenuStrip.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.NotifyIcon notifyIcon;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem_Open;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem_Exit;
		private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem_Access;
		private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem_Access_HTTPS;
	}
}
