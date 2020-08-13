using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MahApps.Metro.Controls;

namespace TVRemotePlus_Launcher
{

    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        
        // ログ
        private ObservableCollection<string> Log;

        // リストをスクロールしたかのフラグ
        // なぜか別のタブを開いてるとスクロールしてくれないので
        private bool IsListScrolled = false;

        public MainWindow()
        {
            InitializeComponent();

            Debug.WriteLine("Event: OpenWindow");

            // App.xaml.cs で取得した Apache の起動ログ
            this.Log = (ObservableCollection<string>) Application.Current.Properties["Log"];
            this.Log.CollectionChanged += OnLogChanged;

            // コントロールに設定状態を表示
            ServerRoot.Text = (string) Application.Current.Properties["ServerRoot"];
            ServerIP.Text = (string)Application.Current.Properties["ServerIP"];
            ServerHTTPPort.Text = (string)Application.Current.Properties["ServerHTTPPort"];
            ServerHTTPSPort.Text = (string)Application.Current.Properties["ServerHTTPSPort"];

            TabControl.SelectionChanged += this.OnLogTabSelected;

            // ListBox コレクションが変更されたときのイベント
            ((INotifyCollectionChanged) ListBox.Items).CollectionChanged += this.OnListBoxCollectionChanged;

            // ListBox にログを追加
            if (this.Log != null) // null でないなら
            {
                foreach (var Item in this.Log)
                {
                    ListBox.Items.Add(Item);
                }
            }
        }

        /// <summary>
        /// 選択されているリストをクリップボードにコピーします。
        /// ListBox 内のリストを右クリック後、コンテキストメニューで
        /// 「クリップボードにコピー」をクリックしたときに呼び出されます。
        /// </summary>
        private void CopyClipboard(object sender, RoutedEventArgs e)
        {
            // 何か選択されていれば
            if (ListBox.SelectedIndex != -1)
            {
                string items = "";
                for (int i = 0; i < ListBox.SelectedItems.Count; i++)
                {
                    if (i != (ListBox.SelectedItems.Count - 1))
                    {
                        items += ListBox.SelectedItems[i].ToString() + "\r\n";
                    }
                    else
                    {
                        // 最後の行は改行をつけない
                        items += ListBox.SelectedItems[i].ToString();
                    }
                }

                // クリップボードにコピー
                Clipboard.SetText(items);
            }
        }

        /// <summary>
        /// 「ログ」タブが選択されたときに呼び出されます。
        /// </summary>
        private void OnLogTabSelected(object sender, EventArgs e)
        {
            // 選択されたタブの番号を取得
            int selectedIndex = TabControl.SelectedIndex + 1;

            if (selectedIndex == 2) // 「ログ」タブ
            {
                if (IsListScrolled == false) // まだリストをスクロールしていなかったら
                {
                    // ListBox をスクロール
                    this.ListBox.ScrollIntoView(this.ListBox.Items[ListBox.Items.Count - 1]);

                    // フラグを true に
                    this.IsListScrolled = true; 
                }
            }
        }

        /// <summary>
        /// MainWindow.ListBox.CollectionChanged のイベントハンドラー。 
        /// ListBox コレクションが変更されたときに呼び出されます。
        /// 参考: https://qiita.com/naminodarie/items/ee7270ae4dae94424d68
        /// </summary>
        private void OnListBoxCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    // ListBox をスクロール
                    this.ListBox.ScrollIntoView(this.ListBox.Items[e.NewStartingIndex]);
                    break;
            }
        }

        /// <summary>
        /// System.Collections.ObjectModel.ObservableCollection.CollectionChanged のイベントハンドラー。 
        /// ログが変更されたときに呼び出されます。
        /// </summary>
        private void OnLogChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            // 新しいログを追加
            var NewLog = e.NewItems;

            if (NewLog != null) // null でないなら
            {
                foreach (var Item in NewLog)
                {
                    // 別スレッドからコントロールを操作する
                    // 参考: https://araramistudio.jimdo.com/2017/05/02/c-%E3%81%A7%E5%88%A5%E3%82%B9%E3%83%AC%E3%83%83%E3%83%89%E3%81%8B%E3%82%89%E3%82%B3%E3%83%B3%E3%83%88%E3%83%AD%E3%83%BC%E3%83%AB%E3%82%92%E6%93%8D%E4%BD%9C%E3%81%99%E3%82%8B/
                    this.Dispatcher.Invoke((Action)(() =>
                    {
                        // ListBox に新しいログを追加
                        ListBox.Items.Add(Item);
                    }));
                }
            }
        }

        /// <summary>
        /// System.Windows.Window.Closing のイベントハンドラー。 
        /// ウインドウが閉じられるときに呼び出されます。
        /// </summary>
        private void CloseWindow(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Debug.WriteLine("Event: CloseWindow");
        }
    }
}
