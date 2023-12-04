using OpenTK.Mathematics;
using OpenTK.Wpf;
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
using OpenTK;
using System.Reflection;

namespace WpfApp1
{
   /// <summary>
   /// Interaction logic for MainWindow.xaml
   /// </summary>
   public partial class MainWindow : Window
   {
      Spline _spline;
      Render? _render;


      public MainWindow()
      {
         InitializeComponent();
         OpenTkControl.Start(new GLWpfControlSettings { MajorVersion = 4, MinorVersion = 6 });
         MyWindow.MinHeight = 255;
         MyWindow.MinWidth = 566;
         _spline = new Spline();
      }

      #region Блок работы с CheckBox

      private void SplineCheckbox_Checked(object sender, RoutedEventArgs e)
      {
         SplineCheckbox.Foreground = Brushes.DarkRed;
         SplineCheckbox.FontWeight = FontWeights.Bold;
      }

      private void DerivativeCheckbox_Checked(object sender, RoutedEventArgs e)
      {
         DerivativeCheckbox.Foreground = Brushes.DarkGreen;
         DerivativeCheckbox.FontWeight = FontWeights.Bold;

      }

      private void PrimalCheckbox_Checked(object sender, RoutedEventArgs e)
      {
         PrimalCheckbox.Foreground = Brushes.DarkBlue;
         PrimalCheckbox.FontWeight = FontWeights.Bold;
      }

      private void SplineCheckbox_Unhecked(object sender, RoutedEventArgs e)
      {
         SplineCheckbox.Foreground = Brushes.Black;
         SplineCheckbox.FontWeight = FontWeights.Regular;
      }

      private void DerivativeCheckbox_Unhecked(object sender, RoutedEventArgs e)
      {
         DerivativeCheckbox.Foreground = Brushes.Black;
         DerivativeCheckbox.FontWeight = FontWeights.Regular;
      }

      private void PrimalCheckbox_Unhecked(object sender, RoutedEventArgs e)
      {
         PrimalCheckbox.Foreground = Brushes.Black;
         PrimalCheckbox.FontWeight = FontWeights.Regular;
      }

      private void SplineSquareCheckbox_Unchecked(object sender, RoutedEventArgs e)
      {
         SplineSquareCheckbox.FontWeight = FontWeights.Regular;
      }

      private void SplineTangentialCheckbox_Checked(object sender, RoutedEventArgs e)
      {
         SplineTangentialCheckbox.FontWeight = FontWeights.Bold;
      }

      private void SplineTangentialCheckbox_Unchecked(object sender, RoutedEventArgs e)
      {
         SplineTangentialCheckbox.FontWeight = FontWeights.Regular;
      }

      private void SplineSquareCheckbox_Checked(object sender, RoutedEventArgs e)
      {
         SplineSquareCheckbox.FontWeight = FontWeights.Bold;
      }

      #endregion

      private void OnRenderRun(TimeSpan obj)
      {
         _render = new Render(OpenTkControl);
         _render.Initialize();
         _render.Begin();
         _render.DrawGrid();
         _render.DrawAxis();
         _render.DrawSpline(_spline);
         _render.DrawPoints(_spline);
         _render.End(); 
      }

      private void WindowChanged(object sender, SizeChangedEventArgs e)
      {
         double leftMargin = OpenTkControl.ActualWidth + 20;
         SplineCheckbox.Margin = new Thickness(leftMargin, 25, 0, 0);
         DerivativeCheckbox.Margin = new Thickness(leftMargin, 65, 0, 0);
         SplineTangentialCheckbox.Margin = new Thickness(leftMargin, 105, 0, 0);
         PrimalCheckbox.Margin = new Thickness(leftMargin, 145, 0, 0);
         SplineSquareCheckbox.Margin = new Thickness(leftMargin, 185, 0, 0);
      }

      private void MouseWheel_Control(object sender, MouseWheelEventArgs e)
      {
         if (e.Delta < 0) _render.ScaleOut();
         else _render.ScaleIn();
         _render.Refresh();
      }
    }
}