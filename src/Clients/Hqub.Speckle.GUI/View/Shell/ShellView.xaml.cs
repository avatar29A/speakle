using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Hqub.Speckle.GUI.View.Shell
{
    /// <summary>
    /// Interaction logic for ShellView.xaml
    /// </summary>
    public partial class ShellView : BaseUserControlView
    {
        enum GridType
        {
            X1 = 1, // 1x1
            X2 = 2, // 2x2
            X4 = 4, // 4x4
            
        }

        public ShellView(ViewModel.Shell.ShellViewModel viewModel) : base(viewModel)
        {
            InitializeComponent();
        }

        #region Настройка грид системы

        private void SetupGrid(int size=1)
        {
            ClearGrid();

            switch (size)
            {
                case (int)GridType.X2:
                    SetupGrid2X();
                    break;

                default:
                    SetupGrid1X();
                    break;
            }
        }

        private void ClearGrid()
        {
            ContentGrid.ColumnDefinitions.Clear();
            ContentGrid.RowDefinitions.Clear();
        }

        private void SetupGrid1X()
        {
            ContentGrid.ColumnDefinitions.Add(new ColumnDefinition
            {
                Width = new GridLength(1, GridUnitType.Star)
            });
        }

        private void SetupGrid2X()
        {
            ContentGrid.ColumnDefinitions.Add(new ColumnDefinition());
            ContentGrid.ColumnDefinitions.Add(new ColumnDefinition());

            ContentGrid.RowDefinitions.Add(new RowDefinition());
            ContentGrid.RowDefinitions.Add(new RowDefinition());
        }

        #endregion

        private void ChangeGridSize_OnClick(object sender, RoutedEventArgs e)
        {
            var tb = (ToggleButton) sender;

            SetupGrid(tb.IsChecked == true ? (int) GridType.X2 : (int) GridType.X1);
        }
    }
}
