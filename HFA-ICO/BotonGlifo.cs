
using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace HFA_ICO
{
    [DesignerCategory("UserControl")]
    public partial class BotonGlifo : UserControl
    {
        private string _glyph = "\\uF00C";
        private string _topRightText = "F1";
        private string _bottomText = "Sincronizar";
        private string _glifoFuente = "FontAwesome";
        private Font _topRightFont;
        private Font _bottomTextFont;
        private Logger _logger;

        // Colores modernos con esquema mejorado
        private Color _backColorNormal = Color.FromArgb(45, 137, 239);      // Azul vibrante
        private Color _backColorHover = Color.FromArgb(58, 150, 245);       // Azul hover más brillante
        private Color _backColorPressed = Color.FromArgb(32, 120, 220);     // Azul pressed más oscuro
        private Color _foreColorNormal = Color.White;
        private Color _borderColor = Color.FromArgb(38, 122, 213);          // Borde sutil

        // Propiedades de barra de progreso
        private int _progressValue = 0;
        private int _progressMaximum = 100;
        private bool _showProgress = false;
        private int _progressBarHeight = 3;
        private Color _progressColor = Color.FromArgb(76, 175, 80);         // Verde éxito
        private Color _progressBackColor = Color.FromArgb(30, 30, 30);      // Fondo oscuro
        private System.Windows.Forms.Timer _progressAnimationTimer;
        private int _progressAnimationOffset = 0;
        private bool _progressIndeterminate = false;

        // Efectos visuales mejorados
        private int _borderRadius = 12;
        private int _borderWidth = 0;
        private bool _showShadow = true;
        private int _shadowDepth = 4;
        private float _glowIntensity = 0f;
        private System.Windows.Forms.Timer _glowTimer;

        // Estado del botón
        private ButtonState _currentState = ButtonState.Normal;
        private float _scaleEffect = 1.0f;
        private System.Windows.Forms.Timer _scaleTimer;

        private enum ButtonState
        {
            Normal,
            Hover,
            Pressed,
            Disabled
        }

        #region Propiedades de Barra de Progreso

        [Category("Progreso")]
        [Description("Valor actual del progreso (0 a Maximum)")]
        [DefaultValue(0)]
        public int ProgressValue
        {
            get => _progressValue;
            set
            {
                _progressValue = Math.Max(0, Math.Min(_progressMaximum, value));
                Invalidate();
            }
        }

        [Category("Progreso")]
        [Description("Valor máximo del progreso")]
        [DefaultValue(100)]
        public int ProgressMaximum
        {
            get => _progressMaximum;
            set
            {
                _progressMaximum = Math.Max(1, value);
                Invalidate();
            }
        }

        [Category("Progreso")]
        [Description("Muestra u oculta la barra de progreso")]
        [DefaultValue(false)]
        public bool ShowProgress
        {
            get => _showProgress;
            set
            {
                _showProgress = value;
                if (_progressAnimationTimer != null)
                {
                    if (value && _progressIndeterminate)
                        _progressAnimationTimer.Start();
                    else
                        _progressAnimationTimer.Stop();
                }
                Invalidate();
            }
        }

        [Category("Progreso")]
        [Description("Altura de la barra de progreso en píxeles")]
        [DefaultValue(3)]
        public int ProgressBarHeight
        {
            get => _progressBarHeight;
            set
            {
                _progressBarHeight = Math.Max(1, Math.Min(10, value));
                Invalidate();
            }
        }

        [Category("Progreso")]
        [Description("Color de la barra de progreso")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public Color ProgressColor
        {
            get => _progressColor;
            set
            {
                _progressColor = value;
                Invalidate();
            }
        }

        [Category("Progreso")]
        [Description("Color de fondo de la barra de progreso")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public Color ProgressBackColor
        {
            get => _progressBackColor;
            set
            {
                _progressBackColor = value;
                Invalidate();
            }
        }

        [Category("Progreso")]
        [Description("Modo de progreso indeterminado (animación continua)")]
        [DefaultValue(false)]
        public bool ProgressIndeterminate
        {
            get => _progressIndeterminate;
            set
            {
                _progressIndeterminate = value;
                if (_progressAnimationTimer != null)
                {
                    if (value && _showProgress)
                        _progressAnimationTimer.Start();
                    else
                        _progressAnimationTimer.Stop();
                }
                Invalidate();
            }
        }

        #endregion

        #region Propiedades de Apariencia Moderna

        [Category("Apariencia Moderna")]
        [Description("Color de fondo normal del botón")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public Color BackColorNormal
        {
            get => _backColorNormal;
            set
            {
                _backColorNormal = value;
                if (_currentState == ButtonState.Normal)
                    Invalidate();
            }
        }

        [Category("Apariencia Moderna")]
        [Description("Color de fondo cuando el mouse está sobre el botón")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public Color BackColorHover
        {
            get => _backColorHover;
            set
            {
                _backColorHover = value;
                if (_currentState == ButtonState.Hover)
                    Invalidate();
            }
        }

        [Category("Apariencia Moderna")]
        [Description("Color de fondo cuando el botón está presionado")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public Color BackColorPressed
        {
            get => _backColorPressed;
            set
            {
                _backColorPressed = value;
                if (_currentState == ButtonState.Pressed)
                    Invalidate();
            }
        }

        [Category("Apariencia Moderna")]
        [Description("Color del texto y el glifo")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public Color ForeColorButton
        {
            get => _foreColorNormal;
            set
            {
                _foreColorNormal = value;
                this.ForeColor = value;
                Invalidate();
            }
        }

        [Category("Apariencia Moderna")]
        [Description("Color del borde del botón")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public Color BorderColorButton
        {
            get => _borderColor;
            set
            {
                _borderColor = value;
                Invalidate();
            }
        }

        [Category("Apariencia Moderna")]
        [Description("Radio de las esquinas redondeadas del botón")]
        [DefaultValue(12)]
        public int BorderRadius
        {
            get => _borderRadius;
            set
            {
                _borderRadius = Math.Max(0, Math.Min(value, Math.Min(Width, Height) / 2));
                UpdateRegion();
                Invalidate();
            }
        }

        [Category("Apariencia Moderna")]
        [Description("Ancho del borde del botón")]
        [DefaultValue(0)]
        public int BorderWidth
        {
            get => _borderWidth;
            set
            {
                _borderWidth = Math.Max(0, value);
                Invalidate();
            }
        }

        [Category("Apariencia Moderna")]
        [Description("Muestra sombra debajo del botón")]
        [DefaultValue(true)]
        public bool ShowShadow
        {
            get => _showShadow;
            set
            {
                _showShadow = value;
                Invalidate();
            }
        }

        [Category("Apariencia Moderna")]
        [Description("Profundidad de la sombra en píxeles")]
        [DefaultValue(4)]
        public int ShadowDepth
        {
            get => _shadowDepth;
            set
            {
                _shadowDepth = Math.Max(0, Math.Min(value, 20));
                Invalidate();
            }
        }

        #endregion

        #region Propiedades Originales

        [Category("Apariencia")]
        [Description("El glifo a mostrar en el centro del botón (ej., Unicode escape como \\uF00C).")]
        [Editor(typeof(GlyphEditor), typeof(UITypeEditor))]
        [TypeConverter(typeof(GlyphConverter))]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public string Glyph
        {
            get => _glyph;
            set
            {
                if (value != _glyph)
                {
                    _glyph = value ?? "\\uF00C";
                    _logger?.Debug($"Glyph char resolved to: '{GetGlyphChar(_glyph)}'");
                    Invalidate();
                }
            }
        }

        [Category("Apariencia")]
        [Description("La fuente a usar para el glifo (ej., FontAwesome).")]
        [Editor(typeof(GlyphFontEditor), typeof(UITypeEditor))]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public string GlifoFuente
        {
            get => _glifoFuente;
            set
            {
                if (value != _glifoFuente)
                {
                    _glifoFuente = string.IsNullOrEmpty(value) ? "FontAwesome" : value;
                    Invalidate();
                    _logger?.Debug($"GlifoFuente changed to: {_glifoFuente}");
                }
            }
        }

        [Category("Apariencia")]
        [Description("El texto a mostrar en la esquina superior derecha (ej., 'F1').")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public string TopRightText
        {
            get => _topRightText;
            set
            {
                _topRightText = value;
                Invalidate();
            }
        }

        [Category("Apariencia")]
        [Description("El texto a mostrar en la parte inferior central (ej., 'Sincronizar').")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public string BottomText
        {
            get => _bottomText;
            set
            {
                _bottomText = value;
                Invalidate();
            }
        }

        #endregion

        public BotonGlifo()
        {
            InitializeComponent();
            //_logger = CustomLoggerFactory.GetLogger() ?? throw new InvalidOperationException("No se pudo obtener CustomLogger");

            try
            {
                //FontManager.Initialize();
                this.ForeColor = _foreColorNormal;

                // Timer para animación de progreso indeterminado
                _progressAnimationTimer = new System.Windows.Forms.Timer { Interval = 30 };
                _progressAnimationTimer.Tick += ProgressAnimationTimer_Tick;

                // Timer para efecto de glow
                _glowTimer = new System.Windows.Forms.Timer { Interval = 50 };
                _glowTimer.Tick += GlowTimer_Tick;

                // Timer para efecto de escala
                _scaleTimer = new System.Windows.Forms.Timer { Interval = 10 };
                _scaleTimer.Tick += ScaleTimer_Tick;

                UpdateFonts();
                UpdateRegion();
            }
            catch (Exception ex)
            {
                _logger?.Error($"Error in constructor: {ex.Message}");
                this.BackColor = _backColorNormal;
                this.ForeColor = _foreColorNormal;
                _glifoFuente = "FontAwesome";
            }

            DoubleBuffered = true;
            SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw |
                     ControlStyles.SupportsTransparentBackColor, true);

            Load += BotonGlifo_Load;
            Resize += BotonGlifo_Resize;
            MouseEnter += BotonGlifo_MouseEnter;
            MouseLeave += BotonGlifo_MouseLeave;
            MouseDown += BotonGlifo_MouseDown;
            MouseUp += BotonGlifo_MouseUp;
            EnabledChanged += BotonGlifo_EnabledChanged;
        }

        #region Métodos de Animación

        private void ProgressAnimationTimer_Tick(object sender, EventArgs e)
        {
            _progressAnimationOffset = (_progressAnimationOffset + 2) % 100;
            if (_showProgress && _progressIndeterminate)
                Invalidate();
        }

        private void GlowTimer_Tick(object sender, EventArgs e)
        {
            if (_currentState == ButtonState.Hover)
            {
                _glowIntensity += 0.05f;
                if (_glowIntensity >= 1.0f)
                {
                    _glowIntensity = 1.0f;
                    _glowTimer.Stop();
                }
                Invalidate();
            }
            else if (_glowIntensity > 0)
            {
                _glowIntensity -= 0.1f;
                if (_glowIntensity <= 0)
                {
                    _glowIntensity = 0;
                    _glowTimer.Stop();
                }
                Invalidate();
            }
        }

        private void ScaleTimer_Tick(object sender, EventArgs e)
        {
            float targetScale = _currentState == ButtonState.Pressed ? 0.95f : 1.0f;
            float diff = targetScale - _scaleEffect;

            if (Math.Abs(diff) < 0.01f)
            {
                _scaleEffect = targetScale;
                _scaleTimer.Stop();
            }
            else
            {
                _scaleEffect += diff * 0.3f;
            }

            Invalidate();
        }

        #endregion

        #region Métodos de Progreso Públicos

        /// <summary>
        /// Establece el progreso y opcionalmente muestra la barra
        /// </summary>
        public void SetProgress(int value, bool show = true)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => SetProgress(value, show)));
                return;
            }

            _progressValue = Math.Max(0, Math.Min(_progressMaximum, value));
            _showProgress = show;
            _progressIndeterminate = false;
            Invalidate();
        }

        /// <summary>
        /// Inicia modo de progreso indeterminado
        /// </summary>
        public void StartIndeterminateProgress()
        {
            if (InvokeRequired)
            {
                Invoke(new Action(StartIndeterminateProgress));
                return;
            }

            _showProgress = true;
            _progressIndeterminate = true;
            _progressAnimationTimer.Start();
            Invalidate();
        }

        /// <summary>
        /// Detiene y oculta la barra de progreso
        /// </summary>
        public void StopProgress()
        {
            if (InvokeRequired)
            {
                Invoke(new Action(StopProgress));
                return;
            }

            _showProgress = false;
            _progressIndeterminate = false;
            _progressAnimationTimer.Stop();
            _progressValue = 0;
            Invalidate();
        }

        /// <summary>
        /// Completa el progreso al 100% y lo oculta después de un delay
        /// </summary>
        public async void CompleteProgress()
        {
            if (InvokeRequired)
            {
                Invoke(new Action(CompleteProgress));
                return;
            }

            _progressIndeterminate = false;
            _progressAnimationTimer.Stop();

            // Animación hasta 100%
            while (_progressValue < _progressMaximum)
            {
                _progressValue = Math.Min(_progressValue + 5, _progressMaximum);
                Invalidate();
                await System.Threading.Tasks.Task.Delay(10);
            }

            // Mantener visible por un momento
            await System.Threading.Tasks.Task.Delay(500);

            // Ocultar
            _showProgress = false;
            _progressValue = 0;
            Invalidate();
        }

        #endregion

        #region Eventos del Mouse

        private void BotonGlifo_MouseEnter(object sender, EventArgs e)
        {
            if (!Enabled) return;

            try
            {
                _currentState = ButtonState.Hover;
                _glowTimer.Start();
                Invalidate();
                Cursor = Cursors.Hand;
            }
            catch (Exception ex)
            {
                _logger?.Error($"Error in MouseEnter: {ex.Message}");
            }
        }

        private void BotonGlifo_MouseLeave(object sender, EventArgs e)
        {
            try
            {
                _currentState = ButtonState.Normal;
                _glowTimer.Start();
                Invalidate();
                Cursor = Cursors.Default;
            }
            catch (Exception ex)
            {
                _logger?.Error($"Error in MouseLeave: {ex.Message}");
            }
        }

        private void BotonGlifo_MouseDown(object sender, MouseEventArgs e)
        {
            if (!Enabled) return;

            try
            {
                _currentState = ButtonState.Pressed;
                _scaleTimer.Start();
                Invalidate();
            }
            catch (Exception ex)
            {
                _logger?.Error($"Error in MouseDown: {ex.Message}");
            }
        }

        private void BotonGlifo_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                _currentState = ClientRectangle.Contains(e.Location) ? ButtonState.Hover : ButtonState.Normal;
                _scaleTimer.Start();
                Invalidate();
            }
            catch (Exception ex)
            {
                _logger?.Error($"Error in MouseUp: {ex.Message}");
            }
        }

        private void BotonGlifo_EnabledChanged(object sender, EventArgs e)
        {
            _currentState = Enabled ? ButtonState.Normal : ButtonState.Disabled;
            Invalidate();
        }

        #endregion

        #region Métodos de Dibujo

        private void UpdateRegion()
        {
            if (_borderRadius > 0)
            {
                using (GraphicsPath path = GetRoundedRectPath(ClientRectangle, _borderRadius))
                {
                    this.Region = new Region(path);
                }
            }
            else
            {
                this.Region = null;
            }
        }

        private GraphicsPath GetRoundedRectPath(Rectangle rect, int radius)
        {
            GraphicsPath path = new GraphicsPath();
            int diameter = radius * 2;

            path.AddArc(rect.X, rect.Y, diameter, diameter, 180, 90);
            path.AddLine(rect.X + radius, rect.Y, rect.Right - radius, rect.Y);
            path.AddArc(rect.Right - diameter, rect.Y, diameter, diameter, 270, 90);
            path.AddLine(rect.Right, rect.Y + radius, rect.Right, rect.Bottom - radius);
            path.AddArc(rect.Right - diameter, rect.Bottom - diameter, diameter, diameter, 0, 90);
            path.AddLine(rect.Right - radius, rect.Bottom, rect.X + radius, rect.Bottom);
            path.AddArc(rect.X, rect.Bottom - diameter, diameter, diameter, 90, 90);
            path.AddLine(rect.X, rect.Bottom - radius, rect.X, rect.Y + radius);

            path.CloseFigure();
            return path;
        }

        private void BotonGlifo_Load(object sender, EventArgs e)
        {
            if (!DesignMode)
            {
                try
                {
                    UpdateFonts();
                    UpdateRegion();
                    Invalidate();
                }
                catch (Exception ex)
                {
                    _logger?.Error($"Error on load: {ex.Message}");
                }
            }
        }

        private void UpdateFonts()
        {
            float textFontSize = Math.Min(Width, Height) * 0.13f;

            try
            {
                _topRightFont?.Dispose();
                _bottomTextFont?.Dispose();

                _topRightFont = new Font("Segoe UI", textFontSize, FontStyle.Bold);
                _bottomTextFont = new Font("Segoe UI", textFontSize, FontStyle.Regular);
            }
            catch (Exception ex)
            {
                _logger?.Warning($"Error loading fonts: {ex.Message}");
                _topRightFont = new Font("Arial", textFontSize, FontStyle.Bold);
                _bottomTextFont = new Font("Arial", textFontSize, FontStyle.Regular);
            }
        }

        private char GetGlyphChar(string input)
        {
            if (string.IsNullOrEmpty(input))
                return '\uF00C';

            if (input.StartsWith("\\u", StringComparison.OrdinalIgnoreCase))
            {
                try
                {
                    string hex = input.Substring(2);
                    int codePoint = Convert.ToInt32(hex, 16);
                    return char.ConvertFromUtf32(codePoint)[0];
                }
                catch
                {
                    return '\uF00C';
                }
            }

            return input.Length == 1 ? input[0] : '\uF00C';
        }

        private void BotonGlifo_Resize(object sender, EventArgs e)
        {
            UpdateFonts();
            UpdateRegion();
            Invalidate();
        }

        private Color GetCurrentBackColor()
        {
            if (!Enabled)
                return Color.FromArgb(100, _backColorNormal);

            switch (_currentState)
            {
                case ButtonState.Hover:
                    return _backColorHover;
                case ButtonState.Pressed:
                    return _backColorPressed;
                default:
                    return _backColorNormal;
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            e.Graphics.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
            e.Graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;

            // Aplicar efecto de escala
            if (_scaleEffect != 1.0f)
            {
                float offsetX = Width * (1 - _scaleEffect) / 2;
                float offsetY = Height * (1 - _scaleEffect) / 2;
                e.Graphics.TranslateTransform(offsetX, offsetY);
                e.Graphics.ScaleTransform(_scaleEffect, _scaleEffect);
            }

            Color currentColor = GetCurrentBackColor();

            // Dibujar sombra
            if (_showShadow && Enabled && _currentState != ButtonState.Pressed)
            {
                DrawShadow(e.Graphics);
            }

            // Dibujar fondo con gradiente sutil
            using (GraphicsPath path = GetRoundedRectPath(ClientRectangle, _borderRadius))
            {
                // Gradiente sutil de arriba hacia abajo
                using (LinearGradientBrush brush = new LinearGradientBrush(
                    ClientRectangle,
                    Color.FromArgb(20, 255, 255, 255), // Brillo sutil arriba
                    Color.FromArgb(0, 0, 0, 0),         // Transparente abajo
                    90f))
                {
                    using (SolidBrush baseBrush = new SolidBrush(currentColor))
                    {
                        e.Graphics.FillPath(baseBrush, path);
                        e.Graphics.FillPath(brush, path);
                    }
                }

                // Efecto glow en hover
                if (_glowIntensity > 0 && Enabled)
                {
                    using (PathGradientBrush glowBrush = new PathGradientBrush(path))
                    {
                        glowBrush.CenterColor = Color.FromArgb((int)(40 * _glowIntensity), 255, 255, 255);
                        glowBrush.SurroundColors = new[] { Color.FromArgb(0, 255, 255, 255) };
                        glowBrush.FocusScales = new PointF(0.7f, 0.7f);
                        e.Graphics.FillPath(glowBrush, path);
                    }
                }

                // Dibujar borde si está configurado
                if (_borderWidth > 0)
                {
                    using (Pen pen = new Pen(_borderColor, _borderWidth))
                    {
                        e.Graphics.DrawPath(pen, path);
                    }
                }
            }

            // Dibujar glifo centrado
            DrawGlyph(e.Graphics);

            // Dibujar texto superior derecho
            DrawTopRightText(e.Graphics);

            // Dibujar texto inferior central
            DrawBottomText(e.Graphics);

            // Resetear transformación
            if (_scaleEffect != 1.0f)
            {
                e.Graphics.ResetTransform();
            }

            // Dibujar barra de progreso (siempre al final, sin transformación)
            if (_showProgress)
            {
                DrawProgressBar(e.Graphics);
            }
        }

        private void DrawShadow(Graphics g)
        {
            Rectangle shadowRect = ClientRectangle;
            shadowRect.Offset(0, _shadowDepth);

            using (GraphicsPath shadowPath = GetRoundedRectPath(shadowRect, _borderRadius))
            {
                using (PathGradientBrush shadowBrush = new PathGradientBrush(shadowPath))
                {
                    shadowBrush.CenterColor = Color.FromArgb(60, 0, 0, 0);
                    shadowBrush.SurroundColors = new[] { Color.FromArgb(0, 0, 0, 0) };
                    shadowBrush.FocusScales = new PointF(0.8f, 0.8f);
                    g.FillPath(shadowBrush, shadowPath);
                }
            }
        }

        private void DrawGlyph(Graphics g)
        {
            if (string.IsNullOrEmpty(_glyph)) return;

            float glyphAreaSize = Math.Min(Width, Height) * 0.4f;
            float fontSize = glyphAreaSize * 0.85f;
            int bitmapSize = (int)(glyphAreaSize * 1.2f);

            try
            {
                char glyphChar = GetGlyphChar(_glyph);
                Color glyphColor = Enabled ? _foreColorNormal : Color.FromArgb(100, _foreColorNormal);

                using (var glyphImage = FontManager.DrawGlyph(_glifoFuente, glyphChar, fontSize, glyphColor, new Size(bitmapSize, bitmapSize)))
                {
                    float x = (Width - glyphImage.Width) / 2f;
                    float y = (Height - glyphImage.Height) / 2f;
                    g.DrawImage(glyphImage, x, y);
                }
            }
            catch (Exception ex)
            {
                _logger?.Error($"Error rendering glyph: {ex.Message}");
                // Fallback: dibujar un círculo simple
                using (SolidBrush brush = new SolidBrush(_foreColorNormal))
                {
                    int size = (int)(glyphAreaSize * 0.5f);
                    int x = (Width - size) / 2;
                    int y = (Height - size) / 2;
                    g.FillEllipse(brush, x, y, size, size);
                }
            }
        }

        private void DrawTopRightText(Graphics g)
        {
            if (string.IsNullOrEmpty(_topRightText)) return;

            using (SolidBrush brush = new SolidBrush(GetContrastColor()))
            {
                var textSize = g.MeasureString(_topRightText, _topRightFont);
                float x = Width - textSize.Width - 10;
                float y = 8;

                // Sombra sutil
                using (SolidBrush shadowBrush = new SolidBrush(Color.FromArgb(60, 0, 0, 0)))
                {
                    g.DrawString(_topRightText, _topRightFont, shadowBrush, x + 1, y + 1);
                }

                g.DrawString(_topRightText, _topRightFont, brush, x, y);
            }
        }

        private void DrawBottomText(Graphics g)
        {
            if (string.IsNullOrEmpty(_bottomText)) return;

            Color textColor = Enabled ? _foreColorNormal : Color.FromArgb(100, _foreColorNormal);

            using (SolidBrush brush = new SolidBrush(textColor))
            {
                var textSize = g.MeasureString(_bottomText, _bottomTextFont);
                float x = (Width - textSize.Width) / 2;
                float y = Height - textSize.Height - 10;

                // Ajustar si hay barra de progreso visible
                if (_showProgress)
                {
                    y -= _progressBarHeight + 4;
                }

                // Sombra sutil
                using (SolidBrush shadowBrush = new SolidBrush(Color.FromArgb(60, 0, 0, 0)))
                {
                    g.DrawString(_bottomText, _bottomTextFont, shadowBrush, x + 1, y + 1);
                }

                g.DrawString(_bottomText, _bottomTextFont, brush, x, y);
            }
        }

        private void DrawProgressBar(Graphics g)
        {
            // Posición en la parte inferior del botón
            int progressY = Height - _progressBarHeight - 2;
            Rectangle progressBgRect = new Rectangle(4, progressY, Width - 8, _progressBarHeight);

            // Fondo de la barra de progreso
            using (SolidBrush bgBrush = new SolidBrush(_progressBackColor))
            using (GraphicsPath bgPath = GetRoundedRectPath(progressBgRect, _progressBarHeight / 2))
            {
                g.FillPath(bgBrush, bgPath);
            }

            // Barra de progreso
            if (_progressIndeterminate)
            {
                // Modo indeterminado: barra animada
                int barWidth = (Width - 8) / 3;
                int barX = 4 + (int)((_progressAnimationOffset / 100f) * (Width - 8 - barWidth));
                Rectangle progressRect = new Rectangle(barX, progressY, barWidth, _progressBarHeight);

                using (LinearGradientBrush progressBrush = new LinearGradientBrush(
                    progressRect,
                    Color.FromArgb(100, _progressColor),
                    _progressColor,
                    LinearGradientMode.Horizontal))
                using (GraphicsPath progressPath = GetRoundedRectPath(progressRect, _progressBarHeight / 2))
                {
                    g.FillPath(progressBrush, progressPath);
                }
            }
            else if (_progressValue > 0)
            {
                // Modo determinado: barra basada en valor
                int progressWidth = (int)(((Width - 8) * (_progressValue / (float)_progressMaximum)));
                if (progressWidth > 0)
                {
                    Rectangle progressRect = new Rectangle(4, progressY, progressWidth, _progressBarHeight);

                    using (LinearGradientBrush progressBrush = new LinearGradientBrush(
                        progressRect,
                        _progressColor,
                        Color.FromArgb(200, _progressColor),
                        LinearGradientMode.Horizontal))
                    using (GraphicsPath progressPath = GetRoundedRectPath(progressRect, _progressBarHeight / 2))
                    {
                        g.FillPath(progressBrush, progressPath);
                    }
                }
            }
        }

        private Color GetContrastColor()
        {
            Color currentBg = GetCurrentBackColor();
            double luminance = (0.299 * currentBg.R + 0.587 * currentBg.G + 0.114 * currentBg.B) / 255;

            return luminance < 0.5
                ? Color.FromArgb(200, 255, 255, 255)
                : Color.FromArgb(200, 0, 0, 0);
        }

        #endregion

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _topRightFont?.Dispose();
                _bottomTextFont?.Dispose();
                _progressAnimationTimer?.Dispose();
                _glowTimer?.Dispose();
                _scaleTimer?.Dispose();
            }
            base.Dispose(disposing);
        }
    }

    #region TypeConverters and Editors (Sin cambios)

    public class GlyphConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
        {
            if (value is string str)
            {
                //CustomLoggerFactory.GetLogger()?.Debug($"GlyphConverter.ConvertFrom: Input='{str}'");
                return str;
            }
            return base.ConvertFrom(context, culture, value);
        }

        public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(string) && value is string str)
            {
                //CustomLoggerFactory.GetLogger()?.Debug($"GlyphConverter.ConvertTo: Output='{str}'");
                return str;
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }
    }

    public class GlyphEditor : UITypeEditor
    {
        private Logger _logger;
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.DropDown;
        }

        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            //var logger = CustomLoggerFactory.GetLogger();
            if (provider != null)
            {
                var service = (System.Windows.Forms.Design.IWindowsFormsEditorService)provider.GetService(typeof(System.Windows.Forms.Design.IWindowsFormsEditorService));
                if (service != null)
                {
                    using (var listBox = new ListBox())
                    {
                        var glyphs = new[]
                        {
                            new { Name = "Checkmark (\\uF00C)", Code = "\\uF00C" },
                            new { Name = "Download (\\uF019)", Code = "\\uF019" },
                            new { Name = "Sync (\\uF021)", Code = "\\uF021" },
                            new { Name = "Trash (\\uF2ED)", Code = "\\uF2ED" },
                            new { Name = "Plus (\\uF067)", Code = "\\uF067" },
                            new { Name = "Settings (\\uF013)", Code = "\\uF013" },
                            new { Name = "Search (\\uF002)", Code = "\\uF002" },
                            new { Name = "Home (\\uF015)", Code = "\\uF015" },
                            new { Name = "User (\\uF007)", Code = "\\uF007" },
                            new { Name = "Heart (\\uF004)", Code = "\\uF004" }
                        };

                        foreach (var glyph in glyphs)
                        {
                            listBox.Items.Add(glyph);
                        }

                        listBox.DisplayMember = "Name";
                        listBox.SelectedIndexChanged += (s, e) =>
                        {
                            if (listBox.SelectedItem != null)
                            {
                                var selectedGlyph = (dynamic)listBox.SelectedItem;
                                value = selectedGlyph.Code;
                                _logger?.Debug($"GlyphEditor selected: '{value}'");
                                service.CloseDropDown();
                            }
                        };

                        _logger?.Debug($"GlyphEditor opened with current value: '{value}'");
                        service.DropDownControl(listBox);
                    }
                }
            }
            return value;
        }
    }

    public class GlyphFontEditor : UITypeEditor
    {
        private Logger _logger;

        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.DropDown;
        }

        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            if (provider != null)
            {
                var service = (System.Windows.Forms.Design.IWindowsFormsEditorService)provider.GetService(typeof(System.Windows.Forms.Design.IWindowsFormsEditorService));
                if (service != null)
                {
                    using (var listBox = new ListBox())
                    {
                        var fontResources = typeof(Properties.Resources).GetProperties(BindingFlags.NonPublic | BindingFlags.Static)
                            .Where(p => p.PropertyType == typeof(byte[]) && p.Name.EndsWith("_otf", StringComparison.OrdinalIgnoreCase))
                            .Select(p => new
                            {
                                Name = p.Name.EndsWith("_otf") ? p.Name.Substring(0, p.Name.Length - 4) : p.Name,
                                Value = p.Name
                            })
                            .ToList();

                        if (!fontResources.Any(r => r.Name == "FontAwesome"))
                        {
                            fontResources.Insert(0, new { Name = "FontAwesome", Value = "FA7_Solid_900" });
                        }

                        foreach (var font in fontResources)
                        {
                            listBox.Items.Add(font);
                        }

                        listBox.DisplayMember = "Name";
                        listBox.SelectedIndexChanged += (s, e) =>
                        {
                            if (listBox.SelectedItem != null)
                            {
                                var selectedFont = (dynamic)listBox.SelectedItem;
                                value = selectedFont.Name;
                                service.CloseDropDown();
                            }
                        };

                        service.DropDownControl(listBox);
                    }
                }
            }
            return value;
        }
    }

    #endregion
}

