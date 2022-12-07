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
using System.Collections.ObjectModel;
using System.IO;

namespace pr1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            grid.DataContext = this;
            kubiks = new ObservableCollection<Kubik>();
            
           
        }
        private ObservableCollection<Kubik> kubiks;
        public ObservableCollection<Kubik> Kubiks
        {
            get
            {
                return kubiks;
            }
        }

        private void add_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                bool norm = false;
                if (ry.IsChecked ?? false)
                {
                    norm = true;
                }
                Kubik kube = new Kubik(name.Text, int.Parse(colors.Text), double.Parse(volume.Text), norm);
                kubiks.Add(kube);
                name.Text = "";
                colors.Text = "";
                volume.Text = "";
            }
            catch 
            {
                MessageBox.Show("Incorrect format of input data!" );
            }
            
        }

        
        private void del_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                kubiks.RemoveAt(lb.SelectedIndex);
            }
            catch
            {
                MessageBox.Show("No one kubik is selected!");
            }
        }

 

    }
}
