using System.Drawing.Drawing2D;
using System.ComponentModel;

namespace HFA_ICO
{

    public class BarraProgreso : Control, IProgressReporter
    {
        public enum ProgressStyle
        {
            Solid,
            Gradient,
            Striped,
            Glow,
            Rounded
        }

        private int _value = 0;
        private int _maximum = 100;
        private string _text = "";
        private ProgressStyle _style = ProgressStyle.Gradient;
        private Color _progressColor = Color.FromArgb(33, 150, 243);
        private Color _backgroundColor = Color.FromArgb(50, 50, 50);
        private Image _icon = null;
        private bool _showPercentage = true;
        private System.Windows.Forms.Timer _animationTimer;
        private int _animationOffset = 0;

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public int Value
        {
            get => _value;
            set
            {
                _value = Math.Max(0, Math.Min(_maximum, value));
                Invalidate();
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public int Maximum
        {
            get => _maximum;
            set { _maximum = Math.Max(1, value); Invalidate(); }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public string ProgressText
        {
            get => _text;
            set { _text = value; Invalidate(); }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public ProgressStyle Style
        {
            get => _style;
            set { _style = value; Invalidate(); }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public Color ProgressColor
        {
            get => _progressColor;
            set { _progressColor = value; Invalidate(); }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public Color BarBackgroundColor
        {
            get => _backgroundColor;
            set { _backgroundColor = value; Invalidate(); }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public Image Icon
        {
            get => _icon;
            set { _icon = value; Invalidate(); }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public bool ShowPercentage
        {
            get => _showPercentage;
            set { _showPercentage = value; Invalidate(); }
        }

        public BarraProgreso()
        {
            SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.OptimizedDoubleBuffer, true);
            Height = 30;

            _animationTimer = new System.Windows.Forms.Timer { Interval = 50 };
            _animationTimer.Tick += (s, e) =>
            {
                _animationOffset = (_animationOffset + 2) % 40;
                if (_style == ProgressStyle.Striped || _style == ProgressStyle.Glow)
                    Invalidate();
            };
            _animationTimer.Start();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;

            // Fondo
            Rectangle bgRect = new Rectangle(0, 0, Width - 1, Height - 1);
            using (SolidBrush bgBrush = new SolidBrush(_backgroundColor))
            {
                if (_style == ProgressStyle.Rounded)
                    g.FillRoundedRectangle(bgBrush, bgRect, 15);
                else
                    g.FillRectangle(bgBrush, bgRect);
            }

            // Barra de progreso
            int progressWidth = (int)((Width - 2) * (_value / (float)_maximum));
            if (progressWidth > 0)
            {
                Rectangle progressRect = new Rectangle(1, 1, progressWidth, Height - 2);
                DrawProgress(g, progressRect);
            }

            // Borde
            using (Pen borderPen = new Pen(Color.FromArgb(100, 100, 100), 1))
            {
                if (_style == ProgressStyle.Rounded)
                    g.DrawRoundedRectangle(borderPen, bgRect, 15);
                else
                    g.DrawRectangle(borderPen, bgRect);
            }

            // Texto e ícono
            DrawTextAndIcon(g);
        }

        private void DrawProgress(Graphics g, Rectangle rect)
        {
            switch (_style)
            {
                case ProgressStyle.Solid:
                    using (SolidBrush brush = new SolidBrush(_progressColor))
                        g.FillRectangle(brush, rect);
                    break;

                case ProgressStyle.Gradient:
                    using (LinearGradientBrush brush = new LinearGradientBrush(
                        rect, Color.FromArgb(255, _progressColor),
                        Color.FromArgb(180, _progressColor), 90f))
                        g.FillRectangle(brush, rect);
                    break;

                case ProgressStyle.Striped:
                    using (LinearGradientBrush brush = new LinearGradientBrush(
                        rect, _progressColor, Color.FromArgb(200, _progressColor), 90f))
                    {
                        g.FillRectangle(brush, rect);
                        using (Pen stripePen = new Pen(Color.FromArgb(50, 255, 255, 255), 8))
                        {
                            for (int x = rect.Left - 40 + _animationOffset; x < rect.Right; x += 20)
                                g.DrawLine(stripePen, x, rect.Top, x + 15, rect.Bottom);
                        }
                    }
                    break;

                case ProgressStyle.Glow:
                    using (GraphicsPath path = new GraphicsPath())
                    {
                        path.AddRectangle(rect);
                        using (PathGradientBrush brush = new PathGradientBrush(path))
                        {
                            brush.CenterColor = Color.FromArgb(255, _progressColor);
                            brush.SurroundColors = new[] { Color.FromArgb(150, _progressColor) };
                            brush.FocusScales = new PointF(0.5f, 0.3f + (float)Math.Sin(_animationOffset * 0.1) * 0.1f);
                            g.FillRectangle(brush, rect);
                        }
                    }
                    break;

                case ProgressStyle.Rounded:
                    using (GraphicsPath path = GetRoundedRect(rect, 12))
                    using (LinearGradientBrush brush = new LinearGradientBrush(
                        rect, _progressColor, Color.FromArgb(180, _progressColor), 90f))
                        g.FillPath(brush, path);
                    break;
            }
        }

        private void DrawTextAndIcon(Graphics g)
        {
            string displayText = _text;
            if (_showPercentage)
            {
                int percentage = (int)((_value / (float)_maximum) * 100);
                displayText = string.IsNullOrEmpty(displayText)
                    ? $"{percentage}%"
                    : $"{displayText} - {percentage}%";
            }

            if (!string.IsNullOrEmpty(displayText))
            {
                using (Font font = new Font("Segoe UI", 9, FontStyle.Bold))
                using (StringFormat sf = new StringFormat
                { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center })
                {
                    // Sombra de texto
                    using (SolidBrush shadowBrush = new SolidBrush(Color.FromArgb(100, 0, 0, 0)))
                        g.DrawString(displayText, font, shadowBrush,
                            new RectangleF(1, 1, Width, Height), sf);

                    // Texto principal
                    using (SolidBrush textBrush = new SolidBrush(Color.White))
                        g.DrawString(displayText, font, textBrush,
                            new RectangleF(0, 0, Width, Height), sf);
                }
            }

            // Ícono
            if (_icon != null)
            {
                int iconSize = Height - 8;
                g.DrawImage(_icon, new Rectangle(4, 4, iconSize, iconSize));
            }
        }

        private GraphicsPath GetRoundedRect(Rectangle rect, int radius)
        {
            GraphicsPath path = new GraphicsPath();
            path.AddArc(rect.X, rect.Y, radius, radius, 180, 90);
            path.AddArc(rect.Right - radius, rect.Y, radius, radius, 270, 90);
            path.AddArc(rect.Right - radius, rect.Bottom - radius, radius, radius, 0, 90);
            path.AddArc(rect.X, rect.Bottom - radius, radius, radius, 90, 90);
            path.CloseFigure();
            return path;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                _animationTimer?.Dispose();
            base.Dispose(disposing);
        }
    }

    // Extension para rectángulos redondeados
    public static class GraphicsExtensions
    {
        public static void FillRoundedRectangle(this Graphics g, Brush brush, Rectangle rect, int radius)
        {
            using (GraphicsPath path = new GraphicsPath())
            {
                path.AddArc(rect.X, rect.Y, radius, radius, 180, 90);
                path.AddArc(rect.Right - radius, rect.Y, radius, radius, 270, 90);
                path.AddArc(rect.Right - radius, rect.Bottom - radius, radius, radius, 0, 90);
                path.AddArc(rect.X, rect.Bottom - radius, radius, radius, 90, 90);
                path.CloseFigure();
                g.FillPath(brush, path);
            }
        }

        public static void DrawRoundedRectangle(this Graphics g, Pen pen, Rectangle rect, int radius)
        {
            using (GraphicsPath path = new GraphicsPath())
            {
                path.AddArc(rect.X, rect.Y, radius, radius, 180, 90);
                path.AddArc(rect.Right - radius, rect.Y, radius, radius, 270, 90);
                path.AddArc(rect.Right - radius, rect.Bottom - radius, radius, radius, 0, 90);
                path.AddArc(rect.X, rect.Bottom - radius, radius, radius, 90, 90);
                path.CloseFigure();
                g.DrawPath(pen, path);
            }
        }
    }

}