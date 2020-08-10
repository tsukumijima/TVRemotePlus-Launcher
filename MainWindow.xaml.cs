using System;
using System.Collections.Generic;
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

namespace TVRemotePlus_Launcher
{

	/// <summary>
	/// MainWindow.xaml の相互作用ロジック
	/// </summary>
	public partial class MainWindow : Window
	{
		
		private List<string> Log;

		public MainWindow()
		{
			InitializeComponent();

			// Apache の起動ログを取得
			this.Log = (List<string>) Application.Current.Properties["Log"];

			// ListBox に追加
			foreach (var Item in this.Log)
			{
				ListBox.Items.Add(Item);
			}
		}
	}
}
