using System.Windows;

namespace Hqub.Speckle.GUI.View
{
    /// <summary>
    /// Interaction logic for MainWindowView.xaml
    /// </summary>
    public partial class MainWindowView : BaseWindowView
    {
        public MainWindowView(ViewModel.MainViewModel viewModel) : base(viewModel)
        {
            InitializeComponent();
        }
    }
}
