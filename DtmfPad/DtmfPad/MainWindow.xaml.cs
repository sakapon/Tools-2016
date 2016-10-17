using System;
using System.Collections.Generic;
using System.Linq;
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

namespace DtmfPad
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        Dictionary<string, MediaElement> sounds = new Dictionary<string, MediaElement>();
        Dictionary<string, Rectangle> clickSurfaces = new Dictionary<string, Rectangle>();

        public MainWindow()
        {
            InitializeComponent();

            Loaded += MainWindow_Loaded;
        }

        void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            var appModel = (AppModel)DataContext;
        }

        void SignalRoot_Loaded(object sender, RoutedEventArgs e)
        {
            var signalRoot = (Grid)sender;
            var signal = (DtmfSignal)signalRoot.DataContext;
            var sound = (MediaElement)signalRoot.Children[1];
            var clickSurface = (Rectangle)signalRoot.Children[2];

            sounds[signal.Id] = sound;
            clickSurfaces[signal.Id] = clickSurface;

            sound.IsMuted = true;
            sound.Position = TimeSpan.FromSeconds(0.8);
            sound.Play();
        }

        void SignalButton_Click(object sender, RoutedEventArgs e)
        {
            var signalButton = (Button)sender;
            var signal = (DtmfSignal)signalButton.DataContext;

            var sound = sounds[signal.Id];
            sound.Stop();
            sound.IsMuted = false;
            sound.Position = TimeSpan.FromSeconds(0.8);
            sound.Play();

            var clickSurface = clickSurfaces[signal.Id];
            var animation = CreateClickAnimation(clickSurface);
            animation.Begin();
        }

        static Storyboard CreateClickAnimation(UIElement element)
        {
            var storyboard = new Storyboard();

            var colorFrames = new ColorAnimationUsingKeyFrames();
            Storyboard.SetTarget(colorFrames, element);
            Storyboard.SetTargetProperty(colorFrames, new PropertyPath("(Shape.Fill).(SolidColorBrush.Color)"));
            colorFrames.KeyFrames.Add(new EasingColorKeyFrame(Color.FromArgb(153, 255, 153, 51), TimeSpan.Zero));
            colorFrames.KeyFrames.Add(new EasingColorKeyFrame(Color.FromArgb(0, 255, 255, 255), TimeSpan.FromSeconds(0.4), new CubicEase { EasingMode = EasingMode.EaseOut }));
            storyboard.Children.Add(colorFrames);

            var visibilityFrames = new ObjectAnimationUsingKeyFrames();
            Storyboard.SetTarget(visibilityFrames, element);
            Storyboard.SetTargetProperty(visibilityFrames, new PropertyPath("(UIElement.Visibility)"));
            visibilityFrames.KeyFrames.Add(new DiscreteObjectKeyFrame(Visibility.Visible, TimeSpan.Zero));
            visibilityFrames.KeyFrames.Add(new DiscreteObjectKeyFrame(Visibility.Collapsed, TimeSpan.FromSeconds(0.4)));
            storyboard.Children.Add(visibilityFrames);

            return storyboard;
        }
    }
}
