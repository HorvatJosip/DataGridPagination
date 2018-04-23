using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace GridPagination
{
    /// <summary>
    /// Interaction logic for PagedDataGrid.xaml
    /// </summary>
    public partial class PagedDataGrid : UserControl
    {
        #region Fields

        #region Page Number Constants

        private const int NumPageButtons = 10;
        private const int NumPagesDisplayedOnTheLeft = 5;
        private const int NumPagesDisplayedOnTheRight = 4;

        #endregion

        private int currentPage;
        private List<object> originalData;
        private List<object> filteredData;
        private bool filterMode;
        private bool forceOriginalCollection;

        #endregion

        #region Properties

        public int CurrentPage
        {
            get => currentPage;
            private set
            {
                if (value != currentPage && PageExists(value))
                {
                    currentPage = value;

                    DisplayPage();
                    AddPageButtons();
                }
            }
        }

        public int MaxPage =>
            CurrentList.Count % PageSize != 0
                ? (CurrentList.Count / PageSize) + 1
                : (CurrentList.Count / PageSize);

        private ICollection<object> CurrentList
        {
            get
            {
                if (filterMode)
                    return filteredData;
                else
                    return originalData;
            }
        }

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




        public ObservableCollection<object> ItemsSource
        {
            get { return (ObservableCollection<object>)GetValue(ItemsSourceProperty); }
            set
            {
                SetValue(ItemsSourceProperty, value);

                if (originalData == null)
                {
                    originalData = value?.ToList();
                    CurrentPage = 1;
                    AddPageButtons();
                    CreateFilterMenu();
                }
            }
        }

        // Using a DependencyProperty as the backing store for ItemsSource.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ItemsSourceProperty =
            DependencyProperty.Register("ItemsSource", typeof(ObservableCollection<object>), typeof(PagedDataGrid), new PropertyMetadata(null));



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



        public ICommand ClearFiltersCmd
        {
            get { return (ICommand)GetValue(ClearFiltersCmdProperty); }
            set { SetValue(ClearFiltersCmdProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ClearFiltersCmd.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ClearFiltersCmdProperty =
            DependencyProperty.Register("ClearFiltersCmd", typeof(ICommand), typeof(PagedDataGrid), new PropertyMetadata(null));




        public int PageSize
        {
            get { return (int)GetValue(PageSizeProperty); }
            set
            {
                SetValue(PageSizeProperty, value);

                DisplayPage();
            }
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

            ClearFiltersCmd = new RelayCommand(obj =>
            {
                if (filterMode)
                {
                    filterMode = false;

                    forceOriginalCollection = true;
                    currentPage = 2;
                    CurrentPage = 1;
                    forceOriginalCollection = false;
                }
            });
        }

        #endregion

        #region Methods

        private bool PageExists(int page) => page >= 1 && page <= MaxPage;

        private void DisplayPage()
        {
            var collection = forceOriginalCollection ? originalData : CurrentList;

            var startingPoint = collection.Skip((currentPage - 1) * PageSize);

            if (startingPoint == null || startingPoint.Count() == 0)
                return;

            ItemsSource = new ObservableCollection<object>();

            for (int i = 0; i < PageSize && i < startingPoint.Count(); i++)
                ItemsSource.Add(startingPoint.ElementAt(i));

            PageNumberInfo = $"Page {CurrentPage} of {MaxPage}";
        }

        private void AddPageButtons()
        {
            PageButtons.Children.Clear();

            int startPage = currentPage - NumPagesDisplayedOnTheLeft;
            int endPage = currentPage + NumPagesDisplayedOnTheRight;

            if (startPage < 1)
            {
                startPage = 1;
                endPage = NumPageButtons;
            }

            if (endPage > MaxPage)
                endPage = MaxPage;

            if (MaxPage >= NumPageButtons && endPage - startPage != (NumPageButtons - 1))
                startPage = endPage - (NumPageButtons - 1);

            for (int i = startPage; i <= endPage; i++)
            {
                int pageNum = i;

                if (PageExists(pageNum))
                    PageButtons.Children.Add(new Button
                    {
                        Content = pageNum.ToString(),
                        Command = new RelayCommand(obj => CurrentPage = pageNum)
                    });
            }
        }

        private void CreateFilterMenu()
        {
            var obj = originalData[0];

            foreach (var prop in obj.GetType().GetProperties())
            {
                FilterMenu.Items.Add(new MenuItem
                {
                    Header = prop.Name,
                    Command = new RelayCommand(param =>
                    {
                        var window = new Prompt("Filter by " + prop.Name, "Value to find:");

                        if (window.ShowDialog() != true)
                            return;

                        filterMode = true;
                        filteredData = originalData.Where(item => Equals(prop.GetValue(item)?.ToString(), window.Input)).ToList();

                        currentPage = 2;
                        CurrentPage = 1;
                    })
                });
            }
        }

        #endregion
    }
}
