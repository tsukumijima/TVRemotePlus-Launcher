namespace TVRemotePlus_Launcher
{
    using System.Windows;
    using System.Diagnostics;
    using System.Reflection;
    using System.IO;
    using System;
    using System.Collections.ObjectModel;
    using System.Collections.Generic;
    using System.Text;
    using System.Text.RegularExpressions;
    using Microsoft.WindowsAPICodePack.Dialogs;

    /// <summary>
    /// App.xaml の相互作用ロジック
    /// 参考: https://garafu.blogspot.com/2015/06/dev-tasktray-residentapplication.html
    /// </summary>
    public partial class App : Application
    {

        private NotifyIconWrapper notifyIcon;
        private ObservableCollection<string> Log;
        private Process Apache;
        private string CurrentFolder;
        private string CurrentFilePath;
        private string CurrentFileName;
        private string CurrentFileNameWithoutExtension;

        /// <summary>
        /// System.Windows.Application.Startup イベント を発生させます。
        /// </summary>
        /// <param name="e">イベントデータ を格納している StartupEventArgs</param>
        protected override void OnStartup(StartupEventArgs e)
        {
            Debug.WriteLine("Event: OnStartup");

            // 実行ファイル名を取得
            this.CurrentFilePath = Assembly.GetExecutingAssembly().Location;
            this.CurrentFileName = Path.GetFileName(this.CurrentFilePath);
            this.CurrentFileNameWithoutExtension = Path.GetFileNameWithoutExtension(this.CurrentFilePath);

            // 現在のフォルダを取得
            this.CurrentFolder = Directory.GetParent(this.CurrentFilePath).FullName;

            // ログのコレクションを作成
            this.Log = new ObservableCollection<string>();

            // ---------- 多重起動を防止 ----------

            // 同じ実行ファイル名のプロセスを取得
            Process[] processes = Process.GetProcessesByName(this.CurrentFileNameWithoutExtension);

            // 自分自身と同じパスのプロセスを探す
            var count = 0;
            foreach (var process in processes)
            {
                // 自分自身と同じパスのプロセス
                if (process.MainModule.FileName == this.CurrentFilePath)
                {
                    if (count > 0) // 自分自身はカウントに含めない
                    {
                        // エラーダイアログ
                        var dialog = new TaskDialog();
                        dialog.Caption = "エラー";
                        dialog.InstructionText = "同じサーバー (Apache) を複数起動することはできません";
                        dialog.Text = "複数の TVRemotePlus を起動したい場合は、フォルダを別にした上で Apache のポートが衝突しないように設定してください。";
                        dialog.Icon = TaskDialogStandardIcon.Error;
                        dialog.StandardButtons = TaskDialogStandardButtons.Ok;
                        dialog.Show();

                        // 現在のアプリケーションを終了
                        Application.Current.Shutdown();
                        return;
                    }
                    count++;
                }
            }

            // ---------- 起動処理 ----------

            base.OnStartup(e);
            this.ShutdownMode = ShutdownMode.OnExplicitShutdown;

            // ---------- Apache の設定を取得 ----------

            // httpd.conf を開く
            StreamReader sr = new StreamReader(this.CurrentFolder + "\\bin\\Apache\\conf\\httpd.conf", Encoding.GetEncoding("UTF-8"));
            string httpd_conf = sr.ReadToEnd();

            // Apache の設定を取得
            // Apache のサーバールート
            Application.Current.Properties["ServerRoot"] = Regex.Match(httpd_conf, @"Define SRVROOT\s""(?<SRVROOT>.*)""").Groups["SRVROOT"].Value;
            // Apache の html ルート (/htdocs 付き)
            Application.Current.Properties["DocumentRoot"] = Application.Current.Properties["ServerRoot"].ToString().TrimEnd('/') + Regex.Match(httpd_conf, @"DocumentRoot ""\$\{SRVROOT\}(?<FOLDER>.*)""").Groups["FOLDER"].Value;
            // Apache のローカル IP アドレス
            Application.Current.Properties["ServerIP"] = Regex.Match(httpd_conf, @"Define SRVIP\s""(?<SRVIP>.*)""").Groups["SRVIP"].Value;
            // Apache の HTTP ポート
            Application.Current.Properties["ServerHTTPPort"] = Regex.Match(httpd_conf, @"Define HTTP_PORT\s""(?<HTTP_PORT>.*)""").Groups["HTTP_PORT"].Value;
            // Apache の HTTPS ポート
            Application.Current.Properties["ServerHTTPSPort"] = Regex.Match(httpd_conf, @"Define HTTPS_PORT\s""(?<HTTPS_PORT>.*)""").Groups["HTTPS_PORT"].Value;

            Debug.WriteLine("ServerIP: " + Application.Current.Properties["ServerIP"]);
            Debug.WriteLine("ServerHTTPPort: " + Application.Current.Properties["ServerHTTPPort"]);
            Debug.WriteLine("ServerHTTPSPort: " + Application.Current.Properties["ServerHTTPSPort"]);
            Debug.WriteLine("ServerRoot: " + Application.Current.Properties["ServerRoot"]);
            Debug.WriteLine("DocumentRoot: " + Application.Current.Properties["DocumentRoot"]);

            // タスクトレイにアイコンを表示
            // Apache の設定が格納されるのを待ってから
            this.notifyIcon = new NotifyIconWrapper();

            // ---------- Apache を起動 ----------

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

            // プロセスの終了時にイベントを送る
            this.Apache.EnableRaisingEvents = true;
            this.Apache.Exited += new EventHandler(OnProcessExited);

            // イベントハンドラーを登録
            this.Apache.OutputDataReceived += OnOutputDataReceived;
            this.Apache.ErrorDataReceived += OnErrorDataReceived;

            try
            {
                // Apache を起動
                this.Apache.Start();

                //非同期で出力の読み取りを開始
                this.Apache.BeginOutputReadLine();
                this.Apache.BeginErrorReadLine();


                this.Log.Add("サーバー (Apache) を起動しました。 開始時刻: " + this.Apache.StartTime);
                Application.Current.Properties["Log"] = this.Log;
            }
            catch (System.ComponentModel.Win32Exception Exception)
            {
                // httpd.exe が存在しないなど、プロセスを開始できない場合
                Debug.WriteLine("Error: " + Exception.Message);
                if (Exception.Message != "" && Exception.Message != null) // 空でないなら
                {
                    this.Log.Add("サーバー (Apache) を起動できませんでした。 " + Exception.Message);

                    // ログを全てのページで見られるように保存
                    Application.Current.Properties["Log"] = this.Log;
                }
            }
        }

        /// <summary>
        /// Process.OutputDataReceived のイベントハンドラー。 
        // 行が出力されるたびに呼び出されます。
        /// </summary>
        protected void OnOutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            // 出力された文字列を表示する
            Debug.WriteLine("Apache: " + e.Data + "");

            // ログを追加
            if (e.Data != "" && e.Data != null) // 空でないなら
            {
                this.Log.Add(e.Data);
            }

            // ログを全てのページで見られるように保存
            Application.Current.Properties["Log"] = this.Log;
        }

        /// <summary>
        /// Process.ErrorDataReceived のイベントハンドラー。 
        // エラー行が出力されるたびに呼び出されます。
        /// </summary>
        protected void OnErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            // 出力された文字列を表示する
            // 本来はエラーのみ出力されるはずだが Apache の場合はなぜか普通のメッセージも入る
            Debug.WriteLine("Apache: " + e.Data + "");

            // ログを追加
            if (e.Data != "" && e.Data != null) // 空でないなら
            {
                this.Log.Add(e.Data);
            }

            // ログを全てのページで見られるように保存
            Application.Current.Properties["Log"] = this.Log;
        }

        /// <summary>
        /// Process.Exited のイベントハンドラー。 
        // プロセスが終了したときに呼び出されます。
        /// </summary>
        public void OnProcessExited(object sender, System.EventArgs e)
        {
            Debug.WriteLine(
                $"Exit time    : {this.Apache.ExitTime}\n" +
                $"Exit code    : {this.Apache.ExitCode}\n" +
                $"Elapsed time : {Math.Round((this.Apache.ExitTime - this.Apache.StartTime).TotalMilliseconds)}");

            // ログを全てのページで見られるように保存
            if (this.Log != null && Application.Current != null) // 正常終了時に行わない
            {
                // エラーダイアログ
                var dialog = new TaskDialog();
                dialog.Caption = "エラー";
                dialog.InstructionText = "サーバー (Apache) が異常終了しました";
                dialog.Text = "サーバー (Apache) の設定を開き、ログを確認してください。";
                dialog.Icon = TaskDialogStandardIcon.Error;
                dialog.StandardButtons = TaskDialogStandardButtons.Ok;
                dialog.Show();

                this.Log.Add("サーバー (Apache) が異常終了しました。 " + 
                             "終了コード: " + this.Apache.ExitCode + " 終了時刻: " + this.Apache.ExitTime + " " +
                             "経過時間: " + Math.Round((this.Apache.ExitTime - this.Apache.StartTime).TotalMilliseconds) + "ms");

                Application.Current.Properties["Log"] = this.Log;
            }
        }

        /// <summary>
        /// System.Windows.Application.Exit イベント を発生させます。
        /// </summary>
        /// <param name="e">イベントデータ を格納している ExitEventArgs</param>
        protected override void OnExit(ExitEventArgs e)
        {
            Debug.WriteLine("Event: OnExit");

            base.OnExit(e);
            if (this.notifyIcon != null && this.Apache != null)
            {
                this.notifyIcon.Dispose();

                try
                {
                    // Apache を（強制的に）終了
                    this.Apache.Kill();
                }
                catch (InvalidOperationException Exception)
                {
                    // Apache がエラーなどで既に終了している場合にスローされる
                    Debug.WriteLine("Error: " + Exception.Message);
                }
            }
        }
    }
}