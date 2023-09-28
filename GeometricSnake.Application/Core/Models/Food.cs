using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace GeometricSnake.Application.Core.Models
{
    internal class Food
    {
        public const string GAME_TAG = "food";
        private Rectangle? rect;
        private Label? label;
        public int SizeBlock { get; set; } = 20;
        public Color Color { get; set; } = Color.FromRgb(0, 0, 150);
        public bool IsActive { get; private set; }
        public int CounterTimer { get; private set; }

        private DispatcherTimer foodCounterTimer;

        public Food()
        {
            foodCounterTimer = new DispatcherTimer();
            foodCounterTimer.Interval = TimeSpan.FromSeconds(1);
            foodCounterTimer.Tick += new EventHandler(FoodCounterTick);
            foodCounterTimer.Start();
        }

        public void Init(Canvas canvas, Point point)
        {
            CounterTimer = Random.Shared.Next(6, 9);
            rect = new Rectangle
            {
                Tag = GAME_TAG,
                Width = SizeBlock, 
                Height = SizeBlock,
                Fill = new SolidColorBrush(Color)
            };

            label = new Label { 
                Tag = GAME_TAG,
                Content = CounterTimer.ToString(),
                Foreground = new SolidColorBrush(Color.FromRgb(255,255,255)),
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                FontWeight = FontWeights.Bold,
                FontSize = 13
            };
            
            Canvas.SetTop(rect, point.Y);
            Canvas.SetLeft(rect, point.X);

            Canvas.SetTop(label, point.Y - 6);
            Canvas.SetLeft(label, point.X + 2);

            canvas.Children.Add(rect);
            canvas.Children.Add(label);
            IsActive = true;
        }

        public void Loop()
        {
            if (IsActive && label is not null)
            {
                label.Content = CounterTimer;
            }
        }

        private void FoodCounterTick(object? sender, EventArgs e)
        {
            if (IsActive && CounterTimer > 0)
            {
                CounterTimer--;
            }

            if (CounterTimer == 0)
                SetActive(false);
        }

        public void SetActive(bool value) => IsActive = value;

        public void Destroy(Canvas canvas)
        {
            if(rect != null)
            {
                canvas.Children.Remove(rect);
                canvas.Children.Remove(label);
            }
        }

        public bool CheckIntersect(Rectangle r)
        {
            if (rect is null)
                return false;

            Rect tempBox = new Rect(Canvas.GetLeft(rect) + 1, Canvas.GetTop(rect) + 1, rect.Width - 2, rect.Height - 2);
            Rect comparerTempBox = new Rect(Canvas.GetLeft(r), Canvas.GetTop(r), r.Width, r.Height);
            return tempBox.IntersectsWith(comparerTempBox);
        }

        public void Pause()
        {
            foodCounterTimer.Stop();
        }

        public void Resume()
        {
            foodCounterTimer.Start();
        }
        
    }
}
