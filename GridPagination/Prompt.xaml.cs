using System.Windows;

namespace GridPagination
{
    /// <summary>
    /// Interaction logic for Prompt.xaml
    /// </summary>
    public partial class Prompt : Window
    {
        public string Input => txtInput.Text;

        public Prompt(string title, string label, string defaultValue = null)
        {
            InitializeComponent();

            Title.Text = title;
            lblInput.Content = label;
            txtInput.Text = defaultValue;
            txtInput.Focus();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }

        private void Grid_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e) => DragMove();
    }
}
