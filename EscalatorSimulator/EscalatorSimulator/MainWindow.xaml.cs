using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace EscalatorSimulator
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        void EscalatorPanel_Loaded(object sender, RoutedEventArgs e)
        {
            var panel = (DockPanel)sender;
            var escalator = (Escalator)panel.DataContext;
            var canvas = (Canvas)panel.FindName("TheCanvas");

            escalator.People1_L
                .ObserveOn(Dispatcher)
                .Subscribe(_ => SpawnCircle(canvas, escalator.Lane_L, 60));
            escalator.People1_R
                .ObserveOn(Dispatcher)
                .Subscribe(_ => SpawnCircle(canvas, escalator.Lane_R, 120));
        }

        void SpawnCircle(Canvas canvas, Lane lane, double left)
        {
            var circle = new Ellipse();
            Canvas.SetLeft(circle, left);
            circle.Style = (Style)Resources["CircleStyle"];
            circle.RenderTransform = new TranslateTransform();
            canvas.Children.Add(circle);

            var sb = CreateMovingStoryboard(lane, circle);
            sb.Completed += (o, e) => canvas.Children.Remove(circle);
            sb.Begin();
        }

        static Storyboard CreateMovingStoryboard(Lane lane, Ellipse circle)
        {
            var sb = new Storyboard();

            var animation = lane.CreateAnimation();
            Storyboard.SetTarget(animation, circle);
            Storyboard.SetTargetProperty(animation, new PropertyPath("(UIElement.RenderTransform).(TranslateTransform.Y)"));
            sb.Children.Add(animation);

            return sb;
        }
    }
}
