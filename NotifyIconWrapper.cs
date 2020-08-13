namespace TVRemotePlus_Launcher
{
    using System;
    using System.ComponentModel;
    using System.Linq;
    using System.Windows;

    /// <summary>
    /// タスクトレイ通知アイコン
    /// 参考: https://garafu.blogspot.com/2015/06/dev-tasktray-residentapplication.html
    /// </summary>
    public partial class NotifyIconWrapper : Component
    {
        /// <summary>
        /// NotifyIconWrapper クラス を生成、初期化します。
        /// </summary>
        public NotifyIconWrapper()
        {
            // コンポーネントの初期化
            this.InitializeComponent();

            // コンテキストメニューのイベントを設定
            this.toolStripMenuItem_Open.Click += this.toolStripMenuItem_Open_Click;
            this.toolStripMenuItem_Access.Click += this.toolStripMenuItem_Access_Click;
            this.toolStripMenuItem_Access_HTTPS.Click += this.toolStripMenuItem_Access_HTTPS_Click;
            this.toolStripMenuItem_Exit.Click += this.toolStripMenuItem_Exit_Click;
        }

        /// <summary>
        /// コンテナ を指定して NotifyIconWrapper クラス を生成、初期化します。
        /// </summary>
        /// <param name="container">コンテナ</param>
        public NotifyIconWrapper(IContainer container)
        {
            container.Add(this);

            this.InitializeComponent();
        }

        /// <summary>
        /// コンテキストメニュー "TVRemotePlus にアクセス" を選択したとき呼ばれます。
        /// </summary>
        /// <param name="sender">呼び出し元オブジェクト</param>
        /// <param name="e">イベントデータ</param>
        private void toolStripMenuItem_Access_Click(object sender, EventArgs e)
        {
            // Web ページを開く
            System.Diagnostics.Process.Start("http://" + Application.Current.Properties["ServerIP"] + ":" + Application.Current.Properties["ServerHTTPPort"] + "/");
        }

        /// <summary>
        /// コンテキストメニュー "TVRemotePlus にアクセス (HTTPS)" を選択したとき呼ばれます。
        /// </summary>
        /// <param name="sender">呼び出し元オブジェクト</param>
        /// <param name="e">イベントデータ</param>
        private void toolStripMenuItem_Access_HTTPS_Click(object sender, EventArgs e)
        {
            // Web ページを開く
            System.Diagnostics.Process.Start("https://" + Application.Current.Properties["ServerIP"] + ":" + Application.Current.Properties["ServerHTTPSPort"] + "/");
        }

        /// <summary>
        /// コンテキストメニュー "TVRemotePlus サーバー設定" を選択したとき呼ばれます。
        /// </summary>
        /// <param name="sender">呼び出し元オブジェクト</param>
        /// <param name="e">イベントデータ</param>
        private void toolStripMenuItem_Open_Click(object sender, EventArgs e)
        {
            // MainWindow が複数立ち上がるのを防ぐ
            // 対象のウインドウが開かれているかを探す
            var window = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();

            if (window == null)
            {
                // MainWindow が開かれてなかったら開く
                window = new MainWindow();
                window.Show();
            } else
            {
                // 既に開かれていたらアクティブにする
                window.Activate();

                // 最小化されてたら戻す
                if (window.WindowState == WindowState.Minimized)
                {
                    window.WindowState = WindowState.Normal;
                }
            }
        }

        /// <summary>
        /// コンテキストメニュー "TVRemotePlus を終了" を選択したとき呼ばれます。
        /// </summary>
        /// <param name="sender">呼び出し元オブジェクト</param>
        /// <param name="e">イベントデータ</param>
        private void toolStripMenuItem_Exit_Click(object sender, EventArgs e)
        {
            // 現在のアプリケーションを終了
            Application.Current.Shutdown();
        }
    }
}