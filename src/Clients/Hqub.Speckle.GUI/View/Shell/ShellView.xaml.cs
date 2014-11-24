using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace Hqub.Speckle.GUI.View.Shell
{
    /// <summary>
    /// Interaction logic for ShellView.xaml
    /// </summary>
    public partial class ShellView : BaseUserControlView
    {
        public ShellView(ViewModel.Shell.ShellViewModel viewModel) : base(viewModel)
        {
            InitializeComponent();
        }
    }
}
