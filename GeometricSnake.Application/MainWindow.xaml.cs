using GeometricSnake.Application.Core;
using GeometricSnake.Application.Core.Models;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace GeometricSnake.Application
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DispatcherTimer timer;
        Game game;
        public const int SPEED = 33;
        public MainWindow()
        {
            InitializeComponent();

            pause_panel.Fill = new SolidColorBrush(Color.FromArgb(100, 0, 0, 0));
            gameover_panel.Fill = new SolidColorBrush(Color.FromArgb(100, 90, 0, 0));

            game = new Game((int)System.Windows.Application.Current.MainWindow.Width,
                            (int)System.Windows.Application.Current.MainWindow.Height, size, 
                            new SimplePanel(pause_desc, pause_panel), new SimplePanel(gameover_desc, gameover_panel), canvas);

            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(SPEED);
            timer.Tick += new EventHandler(GameLoop);
            timer.Start();

            BackCanvasLoop();

            game.Init();

            canvas.Focus();
        }

        private void GameLoop(object? sender, EventArgs e)
        {
            game.Loop();
        }

        private void BackCanvasLoop()
        {
            ColorAnimation animation = new ColorAnimation();
            animation.From = Colors.LightBlue;
            animation.To = Colors.LightPink;
            animation.Duration = TimeSpan.FromSeconds(7);
            animation.AutoReverse = true;
            animation.RepeatBehavior = RepeatBehavior.Forever;

            Storyboard story = new Storyboard();
            story.Children.Add(animation);
            Storyboard.SetTarget(animation, canvas);
            Storyboard.SetTargetProperty(animation, new PropertyPath("(Panel.Background).(SolidColorBrush.Color)"));
            story.Begin();
        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            game.KeyControlDown(e.Key);
        }

        private void OnKeyUp(object sender, KeyEventArgs e)
        {
            game.KeyControlUp(e.Key);
        }
    }
}
