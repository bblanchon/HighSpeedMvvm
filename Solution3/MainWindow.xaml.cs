using System;
using System.Windows;
using System.Windows.Media;

namespace HighSpeedMvvm.Solution3
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            DataContext = new ViewModel();

            CompositionTarget.Rendering += OnRendering;
        }

        void OnRendering(object sender, EventArgs e)
        {
            if (DataContext is IRefresh)
                ((IRefresh)DataContext).Refresh();
        }
    }
}
