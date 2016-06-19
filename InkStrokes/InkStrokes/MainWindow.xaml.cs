using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace InkStrokes
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        const double Radius = 4.0;
        const string Dir_Images = "Images";
        const string Dir_Data = "Data";

        static Lazy<Matrix> TransformToDevice { get; } = new Lazy<Matrix>(() =>
        {
            var source = PresentationSource.FromVisual(Application.Current.MainWindow);
            return source.CompositionTarget.TransformToDevice;
        });

        public MainWindow()
        {
            InitializeComponent();

            TheInkCanvas.DefaultDrawingAttributes.Width = Radius;
            TheInkCanvas.DefaultDrawingAttributes.Height = Radius;

            Directory.CreateDirectory(Dir_Images);
            Directory.CreateDirectory(Dir_Data);

            ClearButton.Click += (o, e) =>
            {
                Clear();
            };
            SaveButton.Click += (o, e) =>
            {
                SaveStrokes();
                Clear();
            };
        }

        void Clear()
        {
            TheInkCanvas.Strokes.Clear();
        }

        void SaveStrokes()
        {
            var fileName = CreateFileName();
            var imagePath = $@"{Dir_Images}\{fileName}.png";
            var dataPath = $@"{Dir_Data}\{fileName}.dat";

            var bitmap = BitmapUtility.CreateImage(TheInkCanvas);
            BitmapUtility.SaveImage(imagePath, bitmap);

            var data = ToString(TheInkCanvas.Strokes);
            File.WriteAllText(dataPath, data);
        }

        static string CreateFileName() =>
            $"{DateTime.Now:yyyyMMdd-HHmmss}";

        public const char StrokeDelimiter = '\n';
        public const char PointDelimiter = ';';
        public const char ElementDelimiter = ',';

        static string ToString(StrokeCollection strokes) =>
            string.Join(StrokeDelimiter.ToString(), strokes.Select(ToString));

        static string ToString(Stroke stroke) =>
            string.Join(PointDelimiter.ToString(), stroke.StylusPoints.Select(p => ((Point)p) * TransformToDevice.Value).Select(ToString));

        static string ToString(Point p) =>
            $"{p.X:F0}{ElementDelimiter}{p.Y:F0}";

        static string ToString(StylusPoint p) =>
            $"{p.X:F3}{ElementDelimiter}{p.Y:F3}";
        //$"{p.X:F3}{ElementDelimiter}{p.Y:F3}{ElementDelimiter}{p.PressureFactor:F3}";
    }
}
