using GeometricSnake.Application.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace GeometricSnake.Application.Core.Models
{
    internal class Snake
    {
        public List<Rectangle> parts { get; set; }
        private List<Point> memoryPoints;

        private string blockTag = "block";

        private double startX = 0;
        private double startY = 0;

        private int sizeBlock = 20;
        private double moveBlock;

        public double Speed { get; private set; } = 5;

        public int SizeBlock { get; private set; } = 20;
        public Color Color { get; private set; }

        public Direction Direction { get; private set; } = Direction.RIGHT;

        public Snake()
        {
            parts = new List<Rectangle>();
            memoryPoints = new List<Point>();
            Color = Color.FromRgb(100, 0, 0);
        }

        public Snake(double startX, double startY) : this()
        {
            this.startX = startX;
            this.startY = startY;
        }

        public void Init(Canvas canva)
        {
            Speed = 5;
            SetDirection(Direction.RIGHT);
            Rectangle rect = new Rectangle
            {
                Tag = blockTag,
                Width = SizeBlock,
                Height = SizeBlock,
                Fill = new SolidColorBrush(Color.FromRgb(255, 0, 0))
            };

            Canvas.SetLeft(rect, startX);
            Canvas.SetTop(rect, startY);

            memoryPoints.Add(new Point(startX, startY));

            canva.Children.Add(rect);
            parts.Add(rect);
        }

        public void Loop()
        {
            MoveHead();
            MoveBody();
        }

        private void MoveHead()
        {
            moveBlock += Speed;
            if (moveBlock > 0 && moveBlock >= sizeBlock)
            {
                var part = parts[0];
                double left = Canvas.GetLeft(part);
                double top = Canvas.GetTop(part);
                memoryPoints[0] = new Point(left, top);

                switch (Direction)
                {
                    case Direction.UP:
                        top -= sizeBlock;
                        break;
                    case Direction.DOWN:
                        top += sizeBlock;
                        break;
                    case Direction.LEFT:
                        left -= sizeBlock;
                        break;
                    case Direction.RIGHT:
                        left += sizeBlock;
                        break;
                    default:
                        break;
                }

                Canvas.SetLeft(part, left);
                Canvas.SetTop(part, top);

                moveBlock = 0;
            }
        }

        private void MoveBody()
        {
            if (parts.Count > 1) {
                for (int i = 1; i < parts.Count; i++)
                {
                    double x = Canvas.GetLeft(parts[i]);
                    double y = Canvas.GetTop(parts[i]);
                    memoryPoints[i] = new Point(x, y);
                    Canvas.SetLeft(parts[i], memoryPoints[i - 1].X);
                    Canvas.SetTop(parts[i], memoryPoints[i - 1].Y);
                }
            }
        }
        public void SetDirection(Direction direction){ 
            Direction = direction;
            moveBlock = sizeBlock;
        }

        public void AddPart(Canvas canvas)
        {
            Rectangle rect = new Rectangle
            {
                Tag = blockTag,
                Width = SizeBlock,
                Height = SizeBlock,
                Fill = new SolidColorBrush(Color)
            };
            var x = Canvas.GetLeft(parts.Last());
            var y = Canvas.GetTop(parts.Last());
            Canvas.SetLeft(rect, x);
            Canvas.SetTop(rect, y);
            memoryPoints.Add(new Point(x, y));
            canvas.Children.Add(rect);
            parts.Add(rect);
        }

        public bool CheckGameOver(int width, int height)
        {
            if (Canvas.GetLeft(parts[0]) < 0 || Canvas.GetTop(parts[0]) < 0 || (Canvas.GetLeft(parts[0]) + parts[0].Width) >= width || (Canvas.GetTop(parts[0]) + parts[0].Height + sizeBlock) >= height)
                return true;

            Rect headRect = new Rect(Canvas.GetLeft(parts[0]) + 1, Canvas.GetTop(parts[0]) + 1, parts[0].Width - 2, parts[0].Height - 2);

            for(int i = 1; i < parts.Count; i++)
            {
                var r = new Rect(Canvas.GetLeft(parts[i]) + 1, Canvas.GetTop(parts[i]) + 1, parts[i].Width - 2, parts[i].Height - 2);
                if (headRect.IntersectsWith(r))
                    return true;
            }
            return false;
        }

        public void Destroy(Canvas canvas)
        {
            foreach (var item in parts)
                canvas.Children.Remove(item);

            parts.Clear();
            memoryPoints.Clear();
        }
    }
}
