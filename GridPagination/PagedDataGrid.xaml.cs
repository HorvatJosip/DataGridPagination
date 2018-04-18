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
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GridPagination
{
    /// <summary>
    /// Interaction logic for PagedDataGrid.xaml
    /// </summary>
    public partial class PagedDataGrid : UserControl
    {
        #region Fields

        private int currentPage;
        private List<string> originalData;

        #endregion

        #region Properties

        public int CurrentPage
        {
            get => currentPage;
            private set
            {
                if(value != currentPage && value >= 1 && value <= MaxPage)
                {
                    currentPage = value;

                    DisplayPage();
                }
            }
        }

        private int MaxPage => (originalData.Count / PageSize) + 1;

        #endregion

        #region Dependency Properties




        public string PageNumberInfo
        {
            get { return (string)GetValue(PageNumberInfoProperty); }
            set { SetValue(PageNumberInfoProperty, value); }
        }

        // Using a DependencyProperty as the backing store for PageNumberInfo.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PageNumberInfoProperty =
            DependencyProperty.Register("PageNumberInfo", typeof(string), typeof(PagedDataGrid), new PropertyMetadata(null));




        public ObservableCollection<string> ItemsSource
        {
            get { return (ObservableCollection<string>)GetValue(ItemsSourceProperty); }
            set
            {
                SetValue(ItemsSourceProperty, value);

                if (originalData == null)
                {
                    originalData = value?.ToList();
                    CurrentPage = 1;
                }
            }
        }

        // Using a DependencyProperty as the backing store for ItemsSource.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ItemsSourceProperty =
            DependencyProperty.Register("ItemsSource", typeof(ObservableCollection<string>), typeof(PagedDataGrid), new PropertyMetadata(null));



        public ICommand FirstCmd
        {
            get { return (ICommand)GetValue(FirstCmdProperty); }
            set { SetValue(FirstCmdProperty, value); }
        }

        // Using a DependencyProperty as the backing store for FirstCmd.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FirstCmdProperty =
            DependencyProperty.Register("FirstCmd", typeof(ICommand), typeof(PagedDataGrid), new PropertyMetadata(null));




        public ICommand PreviousCmd
        {
            get { return (ICommand)GetValue(PreviousCmdProperty); }
            set { SetValue(PreviousCmdProperty, value); }
        }

        // Using a DependencyProperty as the backing store for PreviousCmd.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PreviousCmdProperty =
            DependencyProperty.Register("PreviousCmd", typeof(ICommand), typeof(PagedDataGrid), new PropertyMetadata(null));




        public ICommand NextCmd
        {
            get { return (ICommand)GetValue(NextCmdProperty); }
            set { SetValue(NextCmdProperty, value); }
        }

        // Using a DependencyProperty as the backing store for NextCmd.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty NextCmdProperty =
            DependencyProperty.Register("NextCmd", typeof(ICommand), typeof(PagedDataGrid), new PropertyMetadata(null));




        public ICommand LastCmd
        {
            get { return (ICommand)GetValue(LastCmdProperty); }
            set { SetValue(LastCmdProperty, value); }
        }

        // Using a DependencyProperty as the backing store for LastCmd.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LastCmdProperty =
            DependencyProperty.Register("LastCmd", typeof(ICommand), typeof(PagedDataGrid), new PropertyMetadata(null));




        public int PageSize
        {
            get { return (int)GetValue(PageSizeProperty); }
            set { SetValue(PageSizeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for PageSize.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PageSizeProperty =
            DependencyProperty.Register("PageSize", typeof(int), typeof(PagedDataGrid), new PropertyMetadata(10));




        #endregion

        #region Constructor

        public PagedDataGrid()
        {
            InitializeComponent();

            DataContext = this;

            FirstCmd = new RelayCommand(obj => CurrentPage = 1);

            PreviousCmd = new RelayCommand(obj => CurrentPage--);
            NextCmd = new RelayCommand(obj => CurrentPage++);

            LastCmd = new RelayCommand(obj => CurrentPage = MaxPage);
        }

        #endregion

        #region Methods

        private void DisplayPage()
        {
            var startingPoint = originalData.Skip((currentPage - 1) * PageSize);

            if (startingPoint == null || startingPoint.Count() == 0)
                return;

            ItemsSource = new ObservableCollection<string>();

            for (int i = 0; i < PageSize && i < startingPoint.Count(); i++)
                ItemsSource.Add(startingPoint.ElementAt(i));

            PageNumberInfo = $"Page {CurrentPage} of {MaxPage}";
        }

        #endregion
    }
}
