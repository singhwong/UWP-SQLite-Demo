using SQLite.Net;
using SQLite.Net.Platform.WinRT;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x804 上介绍了“空白页”项模板

namespace sqlite
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private readonly string path = Path.Combine(ApplicationData.Current.LocalFolder.Path, "mydb.sqlite");
        private SQLiteConnection conn;
        private ObservableCollection<DataTemple> list;
        public MainPage()
        {
            this.InitializeComponent();
            list = new ObservableCollection<DataTemple>();
            conn = new SQLiteConnection(new SQLitePlatformWinRT(), path);
            //conn.CreateTable<DataTemple>();
            //conn.CreateTable(typeof(DataTemple));
            //path_textBlock.Text = path;
        }

        private int index = 0;
        private void Add_Click(object sender, RoutedEventArgs e)
        {
            DataTemple temple = new DataTemple
            {
                Id = index,
                Age = textBox1.Text,
                Name = textBox.Text
            };
            List<DataTemple> datalist = conn.Query<DataTemple>("select * from DataTemple where Name = ?", textBox.Text);//防止重复添加
            if (datalist.Count == 0)
            {
                conn.Insert(temple);
                index++;
                list.Add(temple);
            }           
            //SearchItem();
        }

        private void Select_Click(object sender, RoutedEventArgs e)
        {
            list.Clear();
            //textBlock2.Text = "";
            List<DataTemple> datalist = conn.Query<DataTemple>("select * from DataTemple where Name = ?", textBox.Text);
            foreach (var item in datalist)
            {
                //textBlock2.Text = item.Id + " " + item.Name + " " + item.Age + "\n";                
                list.Add(item);
            }
        }

        private void Retrieve_Click(object sender, RoutedEventArgs e)
        {
            //textBlock2.Text = "";
            SearchItem();
        }

        private void Del_Click(object sender, RoutedEventArgs e)
        {
            conn.Execute("delete from DataTemple where Name = ?", textBox.Text);
            SearchItem();
        }

        private void DelAll_Click(object sender, RoutedEventArgs e)
        {
            conn.DeleteAll<DataTemple>();
            SearchItem();
        }

        private void SearchItem()
        {
            list.Clear();
            List<DataTemple> datalist = conn.Query<DataTemple>("select * from DataTemple");
            foreach (var item in datalist)
            {
                //textBlock2.Text += item.Id + " " + item.Name + " " + item.Age + "\n";
                list.Add(item);
            }
        }
    }
}
