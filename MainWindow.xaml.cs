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

            // ListBox に追加
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

            // ListBox に追加
            if (NewLog != null) // null でないなら
            {
                foreach (var Item in NewLog)
                {
                    Debug.WriteLine(Item);
                    //ListBox.Items.Add(Item);
                }
            }
        }
    }
}
