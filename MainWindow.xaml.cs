using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using 替换关键词.Model;

namespace 替换关键词 {
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window {
        public ObservableCollection<KeyWord> SourceList = new ObservableCollection<KeyWord>();
        public MainWindow() {
            InitializeComponent();
            DataContext = this;
            KeyWordList.ItemsSource = SourceList;
            // init
            JsonUtils.Init();
            foreach (KeyWord keyWord in JsonUtils.Ks)
            {
                SourceList.Add(keyWord);
            }

        }

        /// <summary>
        /// 删除元素
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void List_Delete_Click(object sender, RoutedEventArgs e) {
            var item = (sender as Button).Tag as KeyWord;
            if (item != null)
            {
                var ms = MessageBox.Show("确定要删除吗", "警告", MessageBoxButton.YesNo);
               
                if (ms == MessageBoxResult.Yes)
                {
                    JsonUtils.remove(item);
                    SourceList.Remove(item);
                }
            }
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            if (KeyWord.Text.Length <= 0)
            {
                MessageBox.Show("请输入关键词");
                return;
            }
            if (Content.Text.Length <= 0)
            {
                MessageBox.Show("请输入要替换的值");
                return;
            }
            var data = new KeyWord()
            {
                Key = KeyWord.Text,
                Value = Content.Text
            };
            JsonUtils.add(data);
            var find = SourceList.FirstOrDefault(I => I == data);
            if (find != null)
            {
                find.Value = data.Value;
            }
            else
            {
                SourceList.Add(data);
            }
            // clean
            KeyWord.Text = "";
            Content.Text = "";
        }

        private void KeyWordList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selector = (sender as ListBox).SelectedItem as KeyWord;
            if (selector != null)
            {
                KeyWord.Text = selector.Key;
                Content.Text = selector.Value;
            }
        }

        private void update_Click(object sender, RoutedEventArgs e)
        {
            string Text = frontText.Text;
            if (Text.Length > 0)
            {
                foreach (var item in JsonUtils.Ks)
                {
                    string[] splitText = item.Value.Split(new char[] { ',' });
                    string newString = splitText[new Random().Next(splitText.Length)];
                    // 随机选择一个
                    Text = Text.Replace(item.Key, newString);

                }
                behindText.Text = Text;
            }
            else
            {
                MessageBox.Show("请输入要替换的文本");
            }
        }
        private void clean_Click(object sender, RoutedEventArgs e)
        {
            behindText.Text = "";
            frontText.Text = "";
        }
    }
}
