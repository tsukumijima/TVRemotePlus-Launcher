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
        
        private ObservableCollection<string> Log;

        public MainWindow()
        {
            InitializeComponent();

            // App.xaml.cs で取得した Apache の起動ログ
            this.Log = (ObservableCollection<string>) Application.Current.Properties["Log"];
            this.Log.CollectionChanged += OnLogChanged;

            // ListBox にログを追加
            if (this.Log != null) // null でないなら
            {
                foreach (var Item in this.Log)
                {
                    ListBox.Items.Add(Item);
                    // ListBox.Items.Add("TestLog TestLog TestLog TestLog");
                }
            }
        }

        /// <summary>
        /// System.Collections.ObjectModel.ObservableCollection.CollectionChanged のイベントハンドラー。 
        // ログが変更されたときに呼び出されます。
        /// </summary>
        private void OnLogChanged (object sender, NotifyCollectionChangedEventArgs e)
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
    }
}
