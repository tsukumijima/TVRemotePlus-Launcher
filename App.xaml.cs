namespace TVRemotePlus_Launcher
{
    using System.Windows;
    using System.Diagnostics;
    using System.Reflection;
    using System.IO;
    using System;
	using System.Collections.ObjectModel;
	using System.Collections.Generic;

	/// <summary>
	/// App.xaml の相互作用ロジック
	/// 参考: https://garafu.blogspot.com/2015/06/dev-tasktray-residentapplication.html
	/// </summary>
	public partial class App : Application
    {
    
        private NotifyIconWrapper notifyIcon;
        private List<string> Log;
        private Process Apache;
        private string CurrentFolder;

        /// <summary>
        /// System.Windows.Application.Startup イベント を発生させます。
        /// </summary>
        /// <param name="e">イベントデータ を格納している StartupEventArgs</param>
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            this.ShutdownMode = ShutdownMode.OnExplicitShutdown;
            this.notifyIcon = new NotifyIconWrapper();

            Debug.WriteLine("Event: OnStartup");

            // ログのコレクションを作成
            this.Log = new List<string>();

            // 現在のフォルダを取得
            this.CurrentFolder = Directory.GetParent(Assembly.GetExecutingAssembly().Location).FullName;

            // Process オブジェクトを作成
            this.Apache = new Process();

            // 実行するファイル
            this.Apache.StartInfo.FileName = this.CurrentFolder + "\\bin\\Apache\\bin\\httpd.exe";
            // 引数
            this.Apache.StartInfo.Arguments = "-e debug";
            // 作業フォルダ
            this.Apache.StartInfo.WorkingDirectory = this.CurrentFolder + "\\bin\\Apache\\bin";
            // コンソールウインドウを開くか
            this.Apache.StartInfo.CreateNoWindow = true;
            // シェル機能を使うか
            this.Apache.StartInfo.UseShellExecute = false;
            // 出力をストリームに書き込む
            this.Apache.StartInfo.RedirectStandardOutput = true;
            this.Apache.StartInfo.RedirectStandardError = true;
            this.Apache.StartInfo.RedirectStandardInput = false;

            // イベントハンドラーを登録
            this.Apache.OutputDataReceived += OnOutputDataReceived;
            this.Apache.ErrorDataReceived += OnErrorDataReceived;

            // Apache を起動
            this.Apache.Start();

            //非同期で出力の読み取りを開始
            this.Apache.BeginOutputReadLine();
            this.Apache.BeginErrorReadLine();
        }

        /// <summary>
        /// System.Windows.Application.Exit イベント を発生させます。
        /// </summary>
        /// <param name="e">イベントデータ を格納している ExitEventArgs</param>
        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);
            this.notifyIcon.Dispose();

            Debug.WriteLine("Event: OnExit");

			try
            {
                // Apache を（強制的に）終了
                this.Apache.Kill();
            }
			catch (InvalidOperationException exception)
			{
                // Apache がエラーなどで既に終了している場合にスローされる
                Debug.WriteLine("Error: " + exception.Message);
            }
        }

        /// <summary>
        /// System.Diagnostics.Process.OutputDataReceived のイベントハンドラー。 
        // 行が出力されるたびに呼び出されます。
        /// </summary>
        protected void OnOutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            // 出力された文字列を表示する
            Debug.WriteLine("Apache: ((" + e.Data + "))");

            // ログを追加
            if (e.Data != "" && e.Data != null) // 空でないなら
            {
                this.Log.Add(e.Data);
            }

            // ログを全てのページで見られるように保存
            Application.Current.Properties["Log"] = this.Log;
        }

        /// <summary>
        /// System.Diagnostics.Process.ErrorDataReceived のイベントハンドラー。 
        // エラー行が出力されるたびに呼び出されます。
        /// </summary>
        protected void OnErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            // 出力された文字列を表示する
            // 本来はエラーのみ出力されるはずだが Apache の場合はなぜか普通のメッセージも入る
            Debug.WriteLine("Apache: ((" + e.Data + "))");

            // ログを追加
            if (e.Data != "" && e.Data != null) // 空でないなら
            {
                this.Log.Add(e.Data);
            }

            // ログを全てのページで見られるように保存
            Application.Current.Properties["Log"] = this.Log;
        }
    }
}