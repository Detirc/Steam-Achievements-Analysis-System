using Steam_Achievements_Analysis_System.ViewModel;
using Steam_Achievements_Analysis_System.YourOutputDirectory;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Steam_Achievements_Analysis_System
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            SteamGameAchivmentContext steamGameAchivmentContext = new SteamGameAchivmentContext();
            MainWindowViewModel viewModel = new MainWindowViewModel(steamGameAchivmentContext);
            DataContext = viewModel;
        }
    }
}