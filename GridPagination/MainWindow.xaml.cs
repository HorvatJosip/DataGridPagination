using System.Collections.ObjectModel;
using System.Windows;

namespace GridPagination
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            MyGrid.ItemsSource = new ObservableCollection<string>
            {
                "Bla", "Bla 2", "Bla 11", "Bla 16", "Shit 1", "Shit 2", "Shit 3", "Shit 4", "Shit 5",
                "Bla", "Bla 2", "Bla 11", "Bla 16", "Shit 1", "Shit 2", "Shit 3", "Shit 4", "Shit 5",
                "Bla", "Bla 2", "Bla 11", "Bla 16", "Shit 1", "Shit 2", "Shit 3", "Shit 4", "Shit 5",
                "Bla", "Bla 2", "Bla 11", "Bla 16", "Shit 1", "Shit 2", "Shit 3", "Shit 4", "Shit 5",
                "Bla", "Bla 2", "Bla 11", "Bla 16", "Shit 1", "Shit 2", "Shit 3", "Shit 4", "Shit 5",
                "Bla", "Bla 2", "Bla 11", "Bla 16", "Shit 1", "Shit 2", "Shit 3", "Shit 4", "Shit 5"
            };
        }
    }
}
