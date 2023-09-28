using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace GeometricSnake.Application.Core.Models
{
    internal class SimplePanel
    {
        private Label _text;
        private Rectangle _panel;

        public SimplePanel(Label text, Rectangle panel)
        {
            _text = text;
            _panel = panel;
        }

        public void Show() {
            _text.Visibility = System.Windows.Visibility.Visible;
            _panel.Visibility = System.Windows.Visibility.Visible;
        }

        public void Hidden()
        {
            _text.Visibility = System.Windows.Visibility.Hidden;
            _panel.Visibility = System.Windows.Visibility.Hidden;
        }
    }
}
