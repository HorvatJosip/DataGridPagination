using System.Linq;
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

            using (var model = new DAL.DataModel())
                MyGrid.ItemsSource = new ObservableCollection<object>(model.Kupacs.Select(kupac => new
                {
                    kupac.IDKupac,
                    kupac.Ime,
                    kupac.Prezime,
                    kupac.Telefon,
                    kupac.GradID
                }));
        }
    }
}
