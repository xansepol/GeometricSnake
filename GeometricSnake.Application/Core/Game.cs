using GeometricSnake.Application.Core.Enums;
using GeometricSnake.Application.Core.Models;
using System;
using System.Collections.Generic;
using System.DirectoryServices.ActiveDirectory;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Shapes;

namespace GeometricSnake.Application.Core
{
    internal class Game
    {
        private Food food { get; set; }
        private Snake player { get; set; }
        public int Width { get; private set; }
        public int Height { get; private set; }
        public bool IsPlaying { get; private set; } = true;
        public bool GameOver { get; private set; } = false;
        private Label labelSize;
        private SimplePanel pausePanel;
        private SimplePanel gameOverPanel;
        private Canvas canvas;

        public int Score { get; private set; }

        public Game(int width, int height, Label labelSize, SimplePanel pausePanel, SimplePanel gameOverPanel, Canvas canvas)
        {
            Width = width;
            Height = height;
            player = new Snake();
            food = new Food();
            this.labelSize = labelSize;
            this.pausePanel = pausePanel;
            this.gameOverPanel = gameOverPanel;
            this.canvas = canvas;
        }

        public void Init()
        {
            player.Init(canvas);
            SpawnFood();
        }

        public void ClearCanvas()
        {
            food.Destroy(canvas);
            player.Destroy(canvas);
        }

        public void ResetGame()
        {
            if (!GameOver)
                return;
            
            ClearCanvas();
            Init();
            Score = 0;
            labelSize.Content = $"Score: {Score}";
            gameOverPanel.Hidden();
            GameOver = false;
            SetPlaying(true);
        }

        public void Loop()
        {
            if (IsPlaying)
            {
                player.Loop();
                food.Loop();
                SpawnFood();
                CheckIntersect();
            }
        }

        private void SpawnFood()
        {
            if (!food.IsActive)
            {
                food.Destroy(canvas);
                food.Init(canvas, GetRandomPoint());
            }
        }

        public void CheckIntersect()
        {
            if (food.IsActive)
            {
                if (food.CheckIntersect(player.parts[0]))
                {
                    player.AddPart(canvas);
                    Score += 10 * food.CounterTimer;
                    labelSize.Content = $"Score: {Score}";
                    food.SetActive(false);
                }else if (player.CheckGameOver(Width, Height))
                    SetGameOver();
            }
        }

        private void SetDirection(Direction direction)
        {
            if (!IsPlaying)
                return;
            if (direction == Direction.UP && player.Direction == Direction.DOWN ||
                direction == Direction.DOWN && player.Direction == Direction.UP ||
                direction == Direction.LEFT && player.Direction == Direction.RIGHT ||
                direction == Direction.RIGHT && player.Direction == Direction.LEFT)
                return;
            
            player.SetDirection(direction);
        }

        public void KeyControlDown(Key key)
        {
            switch (key)
            {
                case Key.Left:
                    SetDirection(Core.Enums.Direction.LEFT);
                    break;
                case Key.Down:
                    SetDirection(Core.Enums.Direction.DOWN);
                    break;
                case Key.Up:
                    SetDirection(Core.Enums.Direction.UP);
                    break;
                case Key.Right:
                    SetDirection(Core.Enums.Direction.RIGHT);
                    break;
                default:
                    break;
            }
        }

        public void KeyControlUp(Key key)
        {
            switch (key)
            {
                case Key.P:
                    Pause();
                    break;
                case Key.R:
                    ResetGame();
                    break;
                default:
                    break;
            }
        }

        private void Pause()
        {
            if (!GameOver)
            {
                if (IsPlaying)
                {
                    SetPlaying(false);
                    food.Pause();
                    pausePanel.Show();
                }
                else
                {
                    pausePanel.Hidden();
                    SetPlaying(true);
                    food.Resume();
                }
            }
        }

        public void SetPlaying(bool value) => IsPlaying = value;

        private void SetGameOver()
        {
            GameOver = true;
            SetPlaying(false);
            gameOverPanel.Show();
        }

        private Point GetRandomPoint()
        {
            var points = GetAvaiblePoints();
            return points[Random.Shared.Next(0, points.Count - 1)];
        }

        private List<Point> GetAvaiblePoints()
        {
            List<Point> pontos = new List<Point>();
            for(int i = 1;i <= (Width - food.SizeBlock); i += food.SizeBlock)
            {
                for(int j = 1;j<=(Height - (food.SizeBlock * 2));j += food.SizeBlock)
                {
                    bool existe = false;
                    foreach(var part in player.parts)
                    {
                        Rect r = new Rect(Canvas.GetLeft(part), Canvas.GetTop(part), part.Width, part.Height);
                        if (r.IntersectsWith(new(i + (int)(food.SizeBlock / 2), j + (int)(food.SizeBlock / 2), 2, 2)))
                        {
                            existe = true;
                            break;
                        }
                    }
                    if (!existe)
                        pontos.Add(new Point(GetFixedValue(i, food.SizeBlock), GetFixedValue(j, food.SizeBlock)));
                }
            }
            return pontos;
        }

        private int GetFixedValue(int value, int sizeBlock)
        {
            if (value <= sizeBlock)
                return 1;

            return (int)Math.Round((double)value / sizeBlock) * sizeBlock;
        }
    }
}
