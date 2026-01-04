using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;


namespace HFA_ICO
{
    public partial class MainForm : Form
    {
        private string imagenSeleccionada;
        private System.Drawing.Image imagenOriginal;
        private bool seleccionando = false;
        private bool moviendoSeleccion = false;
        private bool redimensionando = false;
        private Point puntoInicio;
        private Point offsetArrastre;
        private Rectangle areaSeleccion;
        private System.Drawing.Image imagenRecortada;
        private TipoAgarre agarreActivo = TipoAgarre.Ninguno;
        private Logger logger;


        private enum TipoAgarre
        {
            Ninguno,
            EsquinaSuperiorIzquierda,
            EsquinaSuperiorDerecha,
            EsquinaInferiorIzquierda,
            EsquinaInferiorDerecha,
            LadoSuperior,
            LadoInferior,
            LadoIzquierdo,
            LadoDerecho,
            Centro
        }

        // Variables del Editor de Iconos
        private string iconoActualPath = "";
        private Icon iconoActual = null;
        private System.Drawing.Image imagenOriginalIcono = null;
        private System.Drawing.Image imagenEditadaIcono = null;
        private Dictionary<int, System.Drawing.Image> tamañosIcono = new Dictionary<int, System.Drawing.Image>();
        private int tamañoSeleccionado = 0;
        private string herramientaActiva = "";

        private EditorPixeles editorPixeles;

        // Variables para eliminación de fondo en conversión individual
        private Bitmap imagenSinFondo = null;
        private int toleranciaFondoIndividual = 30;
        private bool fondoEliminado = false;


        // ===== CONSTRUCTOR MODIFICADO =====
        public MainForm()
        {
            InitializeComponent();
            logger = new Logger(txtLog);
            InicializarEventos();
            InicializarConversionLote(); // ← AGREGAR ESTA LÍNEA
            InicializarEditorIconos();
            logger.Info("Aplicación iniciada");
        }

        // ===== MÉTODO PARA INICIALIZAR EVENTOS (modificado) =====
        private void InicializarEventos()
        {
            // Eventos tab individual
            btnSeleccionar.Click += BtnSeleccionar_Click;
            btnConvertir.Click += BtnConvertir_Click;
            btnLimpiarSeleccion.Click += BtnLimpiarSeleccion_Click;
            chkMantenerProporcion.CheckedChanged += ChkMantenerProporcion_CheckedChanged;
            btnLimpiarLog.Click += BtnLimpiarLog_Click;

            // Eventos PictureBox
            pbPreview.MouseDown += PbPreview_MouseDown;
            pbPreview.MouseMove += PbPreview_MouseMove;
            pbPreview.MouseUp += PbPreview_MouseUp;
            pbPreview.Paint += PbPreview_Paint;

            // Marcar todos los tamaños por defecto (tab individual)
            for (int i = 0; i < clbTamaños.Items.Count; i++)
                clbTamaños.SetItemChecked(i, true);

            // Marcar todos los tamaños por defecto (tab lote)
            for (int i = 0; i < clbTamañosLote.Items.Count; i++)
                clbTamañosLote.SetItemChecked(i, true);

            ConfigurarBoton();
            InicializarHerramientasEdicionIndividual(); // ⭐ AGREGAR ESTA LÍNEA

            logger.Debug("Eventos inicializados");
        }

        private void InicializarHerramientasEdicionIndividual()
        {
            // Crear GroupBox si no existe en el diseñador
            if (gbHerramientasEdicion == null)
            {
                gbHerramientasEdicion = new GroupBox
                {
                    Text = "Herramientas de Edición",
                    Location = new Point(pbPreview.Left, pbPreview.Bottom + 10),
                    Size = new Size(pbPreview.Width, 100),
                    Visible = false
                };

                // Botón eliminar fondo
                btnEliminarFondo = new Button
                {
                    Text = "🪄 Eliminar Fondo",
                    Location = new Point(10, 25),
                    Size = new Size(150, 35),
                    BackColor = Color.FromArgb(220, 53, 69),
                    ForeColor = Color.White,
                    FlatStyle = FlatStyle.Flat,
                    Font = new System.Drawing.Font("Segoe UI", 9F, FontStyle.Bold),
                    Cursor = Cursors.Hand
                };
                btnEliminarFondo.FlatAppearance.BorderSize = 0;
                btnEliminarFondo.Click += BtnEliminarFondo_Click;

                // Botón restaurar imagen
                Button btnRestaurarImagen = new Button
                {
                    Text = "↺ Restaurar Original",
                    Location = new Point(170, 25),
                    Size = new Size(150, 35),
                    BackColor = Color.FromArgb(108, 117, 125),
                    ForeColor = Color.White,
                    FlatStyle = FlatStyle.Flat,
                    Font = new System.Drawing.Font("Segoe UI", 9F, FontStyle.Bold),
                    Cursor = Cursors.Hand
                };
                btnRestaurarImagen.FlatAppearance.BorderSize = 0;
                btnRestaurarImagen.Click += BtnRestaurarImagen_Click;

                // Label tolerancia
                lblToleranciaIndividual = new Label
                {
                    Text = "Tolerancia: 30",
                    Location = new Point(10, 70),
                    Size = new Size(100, 20),
                    Font = new System.Drawing.Font("Segoe UI", 9F)
                };

                // TrackBar tolerancia
                trackBarToleranciaIndividual = new TrackBar
                {
                    Location = new Point(110, 65),
                    Size = new Size(210, 45),
                    Minimum = 0,
                    Maximum = 100,
                    Value = 30,
                    TickFrequency = 10
                };
                trackBarToleranciaIndividual.ValueChanged += (s, e) =>
                {
                    toleranciaFondoIndividual = trackBarToleranciaIndividual.Value;
                    lblToleranciaIndividual.Text = $"Tolerancia: {trackBarToleranciaIndividual.Value}";
                };

                // Agregar controles al GroupBox
                gbHerramientasEdicion.Controls.Add(btnEliminarFondo);
                gbHerramientasEdicion.Controls.Add(btnRestaurarImagen);
                gbHerramientasEdicion.Controls.Add(lblToleranciaIndividual);
                gbHerramientasEdicion.Controls.Add(trackBarToleranciaIndividual);

                // Agregar GroupBox al tab individual
                tabControl1.TabPages[0].Controls.Add(gbHerramientasEdicion);
            }

            logger?.Debug("Herramientas de edición individual inicializadas");
        }

        private void BtnLimpiarLog_Click(object sender, EventArgs e)
        {
            logger.Limpiar();
        }

        //===== NUEVO: MÉTODO PARA ELIMINAR FONDO (tab individual) =====
        // ===== ELIMINACIÓN DE FONDO EN CONVERSIÓN INDIVIDUAL =====
        private void BtnEliminarFondo_Click(object sender, EventArgs e)
        {
            if (imagenOriginal == null)
            {
                MessageBox.Show("Primero debes cargar una imagen", "Sin imagen",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Cambiar cursor del pbPreview temporalmente
            Cursor cursorOriginal = pbPreview.Cursor;
            pbPreview.Cursor = Cursors.Cross;

            // Cambiar estado visual del botón
            btnEliminarFondo.Text = "👆 Haz clic en el fondo...";
            btnEliminarFondo.Enabled = false;

            // Variable para controlar si ya se hizo clic
            bool clickRealizado = false;

            // Crear manejador temporal para el clic
            MouseEventHandler manejadorClick = null;
            manejadorClick = (s, ev) =>
            {
                if (clickRealizado || ev.Button != MouseButtons.Left) return;

                clickRealizado = true;

                // Remover el manejador para evitar múltiples clics
                pbPreview.MouseClick -= manejadorClick;
                pbPreview.Cursor = cursorOriginal;

                try
                {
                    // Obtener la imagen actual (puede ser la original o ya procesada)
                    Bitmap imagenActual = fondoEliminado && imagenSinFondo != null ?
                        new Bitmap(imagenSinFondo) : new Bitmap(imagenOriginal);

                    // Calcular coordenadas reales del clic considerando SizeMode.Zoom
                    Point puntoReal = ObtenerCoordenadaRealEnImagen(ev.Location, pbPreview, imagenActual);

                    if (puntoReal.X < 0 || puntoReal.Y < 0)
                    {
                        MessageBox.Show("Haz clic dentro de la imagen", "Fuera de rango",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        btnEliminarFondo.Text = "🪄 Eliminar Fondo";
                        btnEliminarFondo.Enabled = true;
                        return;
                    }

                    Color colorSeleccionado = imagenActual.GetPixel(puntoReal.X, puntoReal.Y);

                    logger?.Info($"Color seleccionado: RGB({colorSeleccionado.R}, {colorSeleccionado.G}, {colorSeleccionado.B})");
                    logger?.Info($"Punto: ({puntoReal.X}, {puntoReal.Y})");

                    // Mostrar progreso
                    lblEstado.Text = "Eliminando fondo...";
                    lblEstado.ForeColor = Color.Orange;
                    System.Windows.Forms.Application.DoEvents();

                    // Eliminar fondo
                    EliminarFondoEnBitmap(imagenActual, puntoReal, toleranciaFondoIndividual);

                    // Guardar resultado
                    imagenSinFondo?.Dispose();
                    imagenSinFondo = imagenActual;
                    fondoEliminado = true;

                    // Actualizar pbPreview
                    pbPreview.Image?.Dispose();
                    pbPreview.Image = new Bitmap(imagenSinFondo);
                    pbPreview.Invalidate();

                    // Si hay un área de selección, recortar de nuevo
                    if (!areaSeleccion.IsEmpty)
                    {
                        RecortarImagenSeleccionada();
                    }

                    lblEstado.Text = "Fondo eliminado exitosamente";
                    lblEstado.ForeColor = Color.Green;

                    logger?.Info("✓ Fondo eliminado exitosamente");

                    MessageBox.Show(
                        "Fondo eliminado exitosamente.\n\n" +
                        "• La imagen en el preview ahora tiene fondo transparente\n" +
                        "• Puedes eliminar más colores de fondo si es necesario\n" +
                        "• Usa 'Restaurar Original' para volver a la imagen inicial",
                        "Éxito",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information
                    );
                }
                catch (Exception ex)
                {
                    logger?.Error($"Error al eliminar fondo: {ex.Message}", ex);
                    MessageBox.Show($"Error al eliminar fondo:\n{ex.Message}", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);

                    lblEstado.Text = "Error al eliminar fondo";
                    lblEstado.ForeColor = Color.Red;
                }
                finally
                {
                    btnEliminarFondo.Text = "🪄 Eliminar Fondo";
                    btnEliminarFondo.Enabled = true;
                }
            };

            // Agregar manejador temporal
            pbPreview.MouseClick += manejadorClick;

            // Mostrar instrucciones
            lblEstado.Text = "Haz clic en el color del fondo que deseas eliminar";
            lblEstado.ForeColor = Color.Blue;

            logger?.Info("Modo eliminación de fondo activado - esperando clic en pbPreview");
        }

        // Botón para restaurar imagen original
        private void BtnRestaurarImagen_Click(object sender, EventArgs e)
        {
            if (imagenOriginal == null) return;

            var resultado = MessageBox.Show(
                "¿Deseas restaurar la imagen original?\n\n" +
                "Se perderán todos los cambios de eliminación de fondo.",
                "Restaurar Original",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (resultado == DialogResult.Yes)
            {
                // Restaurar imagen original
                imagenSinFondo?.Dispose();
                imagenSinFondo = null;
                fondoEliminado = false;

                pbPreview.Image?.Dispose();
                pbPreview.Image = new Bitmap(imagenOriginal);
                pbPreview.Invalidate();

                // Si hay un área de selección, recortar de nuevo
                if (!areaSeleccion.IsEmpty)
                {
                    RecortarImagenSeleccionada();
                }
                else
                {
                    // Limpiar el preview de recorte
                    pbRecorte.Image = null;
                    pbRecorte.Visible = false;
                    lblRecorte.Visible = false;
                }

                lblEstado.Text = "Imagen original restaurada";
                lblEstado.ForeColor = Color.Green;

                logger?.Info("Imagen original restaurada");
            }
        }

        // Método auxiliar para calcular coordenadas reales en la imagen
        private Point ObtenerCoordenadaRealEnImagen(Point clickPosicion, PictureBox pb, Bitmap imagen)
        {
            // Calcular cómo se está mostrando la imagen con SizeMode.Zoom
            float ratioImagen = (float)imagen.Width / imagen.Height;
            float ratioPictureBox = (float)pb.Width / pb.Height;

            int offsetX = 0, offsetY = 0;
            int anchoMostrado, altoMostrado;

            if (ratioImagen > ratioPictureBox)
            {
                // La imagen es más ancha, se ajusta al ancho
                anchoMostrado = pb.Width;
                altoMostrado = (int)(pb.Width / ratioImagen);
                offsetY = (pb.Height - altoMostrado) / 2;
            }
            else
            {
                // La imagen es más alta, se ajusta al alto
                altoMostrado = pb.Height;
                anchoMostrado = (int)(pb.Height * ratioImagen);
                offsetX = (pb.Width - anchoMostrado) / 2;
            }

            // Ajustar coordenadas del clic
            int clickX = clickPosicion.X - offsetX;
            int clickY = clickPosicion.Y - offsetY;

            // Verificar si está dentro del área de la imagen
            if (clickX < 0 || clickY < 0 || clickX >= anchoMostrado || clickY >= altoMostrado)
            {
                return new Point(-1, -1);
            }

            // Calcular escalas
            float escalaX = (float)imagen.Width / anchoMostrado;
            float escalaY = (float)imagen.Height / altoMostrado;

            // Convertir a coordenadas de la imagen real
            int x = (int)(clickX * escalaX);
            int y = (int)(clickY * escalaY);

            // Asegurar que esté dentro de los límites
            x = Math.Max(0, Math.Min(x, imagen.Width - 1));
            y = Math.Max(0, Math.Min(y, imagen.Height - 1));

            return new Point(x, y);
        }

        // Método de eliminación de fondo (sin cambios)
        private void EliminarFondoEnBitmap(Bitmap imagen, Point inicio, int tolerancia)
        {
            if (imagen == null) return;

            Color colorObjetivo = imagen.GetPixel(inicio.X, inicio.Y);

            if (colorObjetivo.A < 10)
            {
                MessageBox.Show(
                    "El píxel seleccionado ya es transparente.\n\n" +
                    "Selecciona un píxel de color para eliminar.",
                    "Píxel transparente",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );
                return;
            }

            var cola = new Queue<Point>();
            var visitados = new bool[imagen.Width, imagen.Height];
            int pixelesEliminados = 0;

            cola.Enqueue(inicio);
            visitados[inicio.X, inicio.Y] = true;

            Point[] direcciones = new Point[]
            {
        new Point(-1, -1), new Point(0, -1), new Point(1, -1),
        new Point(-1,  0),                    new Point(1,  0),
        new Point(-1,  1), new Point(0,  1), new Point(1,  1)
            };

            while (cola.Count > 0)
            {
                Point p = cola.Dequeue();
                Color colorActual = imagen.GetPixel(p.X, p.Y);

                if (ColoresSimilaresIndividual(colorActual, colorObjetivo, tolerancia))
                {
                    imagen.SetPixel(p.X, p.Y, Color.Transparent);
                    pixelesEliminados++;

                    foreach (Point dir in direcciones)
                    {
                        int nx = p.X + dir.X;
                        int ny = p.Y + dir.Y;

                        if (nx >= 0 && nx < imagen.Width &&
                            ny >= 0 && ny < imagen.Height &&
                            !visitados[nx, ny])
                        {
                            visitados[nx, ny] = true;
                            cola.Enqueue(new Point(nx, ny));
                        }
                    }
                }
            }

            logger?.Info($"Píxeles eliminados: {pixelesEliminados}");
        }


        private bool ColoresSimilaresIndividual(Color c1, Color c2, int tolerancia)
        {
            if (c1.A < 10) return false;

            double diffR = c1.R - c2.R;
            double diffG = c1.G - c2.G;
            double diffB = c1.B - c2.B;

            double distancia = Math.Sqrt(diffR * diffR + diffG * diffG + diffB * diffB);
            double umbral = (tolerancia / 100.0) * 441.0;

            return distancia <= umbral;
        }

        // ===== NUEVO: MÉTODO PARA OBTENER TAMAÑOS (tab individual) =====
        private int[] ObtenerTamañosSeleccionados()
        {
            var tamaños = new List<int>();
            foreach (var item in clbTamaños.CheckedItems)
            {
                string texto = item.ToString();
                int tamaño = int.Parse(texto.Split('x')[0]);
                tamaños.Add(tamaño);
            }
            return tamaños.OrderByDescending(x => x).ToArray();
        }

        // ===== NUEVO: MÉTODO PARA OBTENER TAMAÑOS (tab lote) =====
        private int[] ObtenerTamañosSeleccionadosLote()
        {
            var tamaños = new List<int>();
            foreach (var item in clbTamañosLote.CheckedItems)
            {
                string texto = item.ToString();
                int tamaño = int.Parse(texto.Split('x')[0]);
                tamaños.Add(tamaño);
            }
            return tamaños.OrderByDescending(x => x).ToArray();
        }
        private void ConfigurarBoton()
        {
            btnConvertir.BackColorNormal = Color.FromArgb(45, 137, 239);
            btnConvertir.BackColorHover = Color.FromArgb(58, 150, 245);
            btnConvertir.BorderRadius = 6;
            btnConvertir.ShowShadow = true;
            btnConvertir.ProgressBarHeight = 4;
            btnConvertir.ProgressColor = Color.FromArgb(76, 175, 80);
        }
        private void PbPreview_MouseDown(object sender, MouseEventArgs e)
        {
            if (imagenOriginal == null || e.Button != MouseButtons.Left) return;

            puntoInicio = e.Location;
            logger.Debug($"MouseDown en: {e.Location}");

            // Si ya existe un área de selección, verificar si se hace clic en un agarre
            if (!areaSeleccion.IsEmpty)
            {
                agarreActivo = ObtenerAgarreEnPosicion(e.Location);
                logger.Debug($"Agarre detectado: {agarreActivo}");

                if (agarreActivo != TipoAgarre.Ninguno)
                {
                    if (agarreActivo == TipoAgarre.Centro)
                    {
                        moviendoSeleccion = true;
                        offsetArrastre = new Point(e.X - areaSeleccion.X, e.Y - areaSeleccion.Y);
                        logger.Debug("Iniciando movimiento de selección");
                    }
                    else
                    {
                        redimensionando = true;
                        logger.Debug("Iniciando redimensionamiento");
                    }
                    return;
                }
            }

            // Si no se hizo clic en un agarre, iniciar nueva selección
            seleccionando = true;
            areaSeleccion = new Rectangle(e.Location, new Size(0, 0));
            pbPreview.Cursor = Cursors.Cross;
            logger.Debug("Iniciando nueva selección");
        }

        private void PbPreview_MouseMove(object sender, MouseEventArgs e)
        {
            if (imagenOriginal == null) return;

            // Actualizar cursor según la posición
            if (!seleccionando && !moviendoSeleccion && !redimensionando)
            {
                ActualizarCursor(e.Location);
            }

            // Mover selección completa
            if (moviendoSeleccion)
            {
                int nuevoX = e.X - offsetArrastre.X;
                int nuevoY = e.Y - offsetArrastre.Y;

                // Limitar al área del PictureBox
                nuevoX = Math.Max(0, Math.Min(nuevoX, pbPreview.Width - areaSeleccion.Width));
                nuevoY = Math.Max(0, Math.Min(nuevoY, pbPreview.Height - areaSeleccion.Height));

                areaSeleccion.Location = new Point(nuevoX, nuevoY);
                pbPreview.Invalidate();
                ActualizarEtiquetas();
                return;
            }

            // Redimensionar desde agarres
            if (redimensionando)
            {
                RedimensionarDesdeAgarre(e.Location);
                return;
            }

            // Crear nueva selección
            if (seleccionando)
            {
                int x = Math.Min(puntoInicio.X, e.X);
                int y = Math.Min(puntoInicio.Y, e.Y);
                int ancho = Math.Abs(e.X - puntoInicio.X);
                int alto = Math.Abs(e.Y - puntoInicio.Y);

                // Mantener proporción 1:1 si está activado
                if (chkMantenerProporcion.Checked)
                {
                    int tamMin = Math.Min(ancho, alto);
                    ancho = alto = tamMin;

                    if (e.X < puntoInicio.X) x = puntoInicio.X - ancho;
                    if (e.Y < puntoInicio.Y) y = puntoInicio.Y - alto;
                }

                // Limitar al área del PictureBox
                if (x < 0) { ancho += x; x = 0; }
                if (y < 0) { alto += y; y = 0; }
                if (x + ancho > pbPreview.Width) ancho = pbPreview.Width - x;
                if (y + alto > pbPreview.Height) alto = pbPreview.Height - y;

                areaSeleccion = new Rectangle(x, y, ancho, alto);
                pbPreview.Invalidate();
                ActualizarEtiquetas();
            }
        }

        private void PbPreview_MouseUp(object sender, MouseEventArgs e)
        {
            if (!seleccionando && !moviendoSeleccion && !redimensionando) return;

            if (seleccionando)
            {
                seleccionando = false;
                if (areaSeleccion.Width > 10 && areaSeleccion.Height > 10)
                {
                    RecortarImagenSeleccionada();
                    btnLimpiarSeleccion.Enabled = true;
                    lblEstado.Text = "Área seleccionada - Arrastra para mover o redimensionar";
                    lblEstado.ForeColor = Color.Green;
                }
                else
                {
                    areaSeleccion = Rectangle.Empty;
                    pbPreview.Invalidate();
                }
            }
            else if (moviendoSeleccion || redimensionando)
            {
                moviendoSeleccion = false;
                redimensionando = false;
                RecortarImagenSeleccionada();
            }

            agarreActivo = TipoAgarre.Ninguno;
            pbPreview.Cursor = Cursors.Default;
        }

        private void PbPreview_Paint(object sender, PaintEventArgs e)
        {
            if (areaSeleccion.Width > 0 && areaSeleccion.Height > 0)
            {
                // Dibujar overlay oscuro
                using (SolidBrush brush = new SolidBrush(Color.FromArgb(100, 0, 0, 0)))
                {
                    Region region = new Region(pbPreview.ClientRectangle);
                    region.Exclude(areaSeleccion);
                    e.Graphics.FillRegion(brush, region);
                }

                // Dibujar borde de selección
                using (Pen pen = new Pen(Color.FromArgb(255, 0, 120, 215), 2))
                {
                    pen.DashStyle = DashStyle.Dash;
                    e.Graphics.DrawRectangle(pen, areaSeleccion);
                }

                // Dibujar agarres (esquinas)
                DibujarAgarre(e.Graphics, areaSeleccion.Left, areaSeleccion.Top); // Superior izquierda
                DibujarAgarre(e.Graphics, areaSeleccion.Right, areaSeleccion.Top); // Superior derecha
                DibujarAgarre(e.Graphics, areaSeleccion.Left, areaSeleccion.Bottom); // Inferior izquierda
                DibujarAgarre(e.Graphics, areaSeleccion.Right, areaSeleccion.Bottom); // Inferior derecha

                // Dibujar agarres (laterales)
                DibujarAgarre(e.Graphics, areaSeleccion.Left + areaSeleccion.Width / 2, areaSeleccion.Top); // Superior
                DibujarAgarre(e.Graphics, areaSeleccion.Left + areaSeleccion.Width / 2, areaSeleccion.Bottom); // Inferior
                DibujarAgarre(e.Graphics, areaSeleccion.Left, areaSeleccion.Top + areaSeleccion.Height / 2); // Izquierdo
                DibujarAgarre(e.Graphics, areaSeleccion.Right, areaSeleccion.Top + areaSeleccion.Height / 2); // Derecho
            }
        }

        private void DibujarAgarre(Graphics g, int x, int y)
        {
            int tamaño = 8;
            using (SolidBrush brush = new SolidBrush(Color.White))
            using (Pen pen = new Pen(Color.FromArgb(0, 120, 215), 2))
            {
                Rectangle rect = new Rectangle(x - tamaño / 2, y - tamaño / 2, tamaño, tamaño);
                g.FillRectangle(brush, rect);
                g.DrawRectangle(pen, rect);
            }
        }

        private void RecortarImagenSeleccionada()
        {
            if (imagenOriginal == null || areaSeleccion.IsEmpty) return;

            try
            {
                logger.Debug($"Recortando imagen. Área selección: {areaSeleccion}");

                // ⭐ MODIFICADO: Usar imagen con fondo eliminado si existe
                Bitmap imagenBase = fondoEliminado && imagenSinFondo != null ?
                    imagenSinFondo : (Bitmap)imagenOriginal;

                logger.Debug($"PictureBox tamaño: {pbPreview.Width}x{pbPreview.Height}");
                logger.Debug($"Imagen base: {imagenBase.Width}x{imagenBase.Height}");
                logger.Debug($"Usando imagen con fondo eliminado: {fondoEliminado}");

                // Calcular proporción del PictureBox respecto a la imagen
                float ratioImagen = (float)imagenBase.Width / imagenBase.Height;
                float ratioPictureBox = (float)pbPreview.Width / pbPreview.Height;

                float escalaX, escalaY;
                int offsetX = 0, offsetY = 0;
                int anchoMostrado, altoMostrado;

                if (ratioImagen > ratioPictureBox)
                {
                    anchoMostrado = pbPreview.Width;
                    altoMostrado = (int)(pbPreview.Width / ratioImagen);
                    offsetY = (pbPreview.Height - altoMostrado) / 2;
                }
                else
                {
                    altoMostrado = pbPreview.Height;
                    anchoMostrado = (int)(pbPreview.Height * ratioImagen);
                    offsetX = (pbPreview.Width - anchoMostrado) / 2;
                }

                logger.Debug($"Imagen mostrada en PictureBox: {anchoMostrado}x{altoMostrado}, Offset: {offsetX},{offsetY}");

                int selX = areaSeleccion.X - offsetX;
                int selY = areaSeleccion.Y - offsetY;
                int selW = areaSeleccion.Width;
                int selH = areaSeleccion.Height;

                if (selX < 0) { selW += selX; selX = 0; }
                if (selY < 0) { selH += selY; selY = 0; }
                if (selX + selW > anchoMostrado) selW = anchoMostrado - selX;
                if (selY + selH > altoMostrado) selH = altoMostrado - selY;

                logger.Debug($"Selección ajustada: X={selX}, Y={selY}, W={selW}, H={selH}");

                escalaX = (float)imagenBase.Width / anchoMostrado;
                escalaY = (float)imagenBase.Height / altoMostrado;
                logger.Debug($"Escalas - X: {escalaX}, Y: {escalaY}");

                Rectangle areaReal = new Rectangle(
                    (int)(selX * escalaX),
                    (int)(selY * escalaY),
                    (int)(selW * escalaX),
                    (int)(selH * escalaY)
                );
                logger.Debug($"Área real calculada: {areaReal}");

                areaReal.Intersect(new Rectangle(0, 0, imagenBase.Width, imagenBase.Height));
                logger.Debug($"Área real ajustada: {areaReal}");

                if (areaReal.Width <= 0 || areaReal.Height <= 0)
                {
                    logger.Warning("Área de recorte inválida");
                    return;
                }

                // Recortar imagen
                imagenRecortada?.Dispose();
                imagenRecortada = new Bitmap(areaReal.Width, areaReal.Height);

                using (Graphics g = Graphics.FromImage(imagenRecortada))
                {
                    g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    g.Clear(Color.Transparent); // ⭐ IMPORTANTE: Fondo transparente
                    g.DrawImage(imagenBase,
                        new Rectangle(0, 0, areaReal.Width, areaReal.Height),
                        areaReal,
                        GraphicsUnit.Pixel);
                }

                // Mostrar preview del recorte
                pbRecorte.Image?.Dispose();
                pbRecorte.Image = new Bitmap(imagenRecortada);
                pbRecorte.Visible = true;
                lblRecorte.Visible = true;
                lblRecorte.Text = $"Preview: {areaReal.Width}x{areaReal.Height}";

                logger.Info($"Imagen recortada exitosamente: {areaReal.Width}x{areaReal.Height}");
            }
            catch (Exception ex)
            {
                logger.Error($"Error al recortar imagen: {ex.Message}");
                logger.Debug($"StackTrace: {ex.StackTrace}");
            }
        }

        private void BtnLimpiarSeleccion_Click(object sender, EventArgs e)
        {
            areaSeleccion = Rectangle.Empty;
            imagenRecortada?.Dispose();
            imagenRecortada = null;

            pbPreview.Invalidate();
            pbRecorte.Visible = false;
            lblRecorte.Visible = false;
            btnLimpiarSeleccion.Enabled = false;
            lblEstado.Text = "Selecciona un área con el mouse";
            lblEstado.ForeColor = Color.Gray;

            // ⭐ MODIFICADO: Mantener la imagen con fondo eliminado si existe
            if (fondoEliminado && imagenSinFondo != null)
            {
                lblDimensiones.Text = $"{imagenSinFondo.Width} x {imagenSinFondo.Height} px (sin fondo)";
            }
            else
            {
                lblDimensiones.Text = $"{imagenOriginal.Width} x {imagenOriginal.Height} px";
            }
        }

        private void ChkMantenerProporcion_CheckedChanged(object sender, EventArgs e)
        {
            lblEstado.Text = chkMantenerProporcion.Checked
                ? "Proporción 1:1 activada"
                : "Selecciona un área con el mouse";
        }

        private void BtnSeleccionar_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "Imágenes|*.png;*.jpg;*.jpeg;*.bmp;*.gif|Todos los archivos|*.*";
                ofd.Title = "Seleccionar imagen";

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        imagenSeleccionada = ofd.FileName;
                        imagenOriginal = System.Drawing.Image.FromFile(imagenSeleccionada);
                        pbPreview.Image = new Bitmap(imagenOriginal);
                        lblArchivo.Text = Path.GetFileName(imagenSeleccionada);
                        lblArchivo.ForeColor = Color.Black;
                        lblDimensiones.Text = $"{imagenOriginal.Width} x {imagenOriginal.Height} px";
                        lblEstado.Text = "Selecciona un área con el mouse";
                        lblEstado.ForeColor = Color.Gray;
                        btnConvertir.Enabled = true;
                        chkMantenerProporcion.Enabled = true;
                        // ⭐ NUEVO: Mostrar herramientas de edición
                        gbHerramientasEdicion.Visible = true;
                        // ⭐ NUEVO: Resetear estado de fondo eliminado
                        fondoEliminado = false;
                        imagenSinFondo?.Dispose();
                        imagenSinFondo = null;

                        logger.Info($"Imagen cargada: {Path.GetFileName(imagenSeleccionada)}");
                        logger.Debug($"Dimensiones: {imagenOriginal.Width}x{imagenOriginal.Height}");

                        // Limpiar selección anterior
                        BtnLimpiarSeleccion_Click(null, null);
                    }
                    catch (Exception ex)
                    {
                        DialogResult result;
                        using (var msgForm = new frmMsgBox($"Error al cargar imagen: {ex.Message}", "Error"))
                            result = msgForm.ShowDialog();
                    }
                }
            }
        }

        // ===== IMPLEMENTACIÓN LISTA PARA USAR =====
        // Reemplaza tu método BtnConvertir_Click actual con este código
        private async void BtnConvertir_Click(object sender, EventArgs e)
        {
            // Validación de tamaños
            if (clbTamaños.CheckedItems.Count == 0)
            {
                DialogResult result;
                using (var msgForm = new frmMsgBox("Selecciona al menos un tamaño", "Advertencia"))
                    result = msgForm.ShowDialog();
                return;
            }

            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.Filter = "Icono|*.ico";
                sfd.Title = "Guardar icono";
                sfd.FileName = Path.GetFileNameWithoutExtension(imagenSeleccionada) + ".ico";

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    btnConvertir.Enabled = false;
                    btnConvertir.ShowProgress = true;
                    btnConvertir.BottomText = "Iniciando...";

                    try
                    {
                        logger.Info("Iniciando conversión a ICO");
                        logger.Separator('=', 60);

                        int[] tamaños = ObtenerTamañosSeleccionados();
                        logger.Debug($"Tamaños seleccionados: {string.Join(", ", tamaños)}");

                        // ⭐ MODIFICADO: Determinar qué imagen usar
                        System.Drawing.Image imagenAConvertir;

                        if (imagenRecortada != null)
                        {
                            // Si hay recorte, usar el recorte (ya incluye fondo eliminado si aplica)
                            imagenAConvertir = imagenRecortada;
                            logger.Info("Usando imagen recortada" + (fondoEliminado ? " (con fondo eliminado)" : ""));
                        }
                        else if (fondoEliminado && imagenSinFondo != null)
                        {
                            // Si se eliminó el fondo pero no hay recorte
                            imagenAConvertir = imagenSinFondo;
                            logger.Info("Usando imagen con fondo eliminado");
                        }
                        else
                        {
                            // Imagen original sin modificaciones
                            imagenAConvertir = imagenOriginal;
                            logger.Info("Usando imagen original");
                        }

                        logger.Debug($"Imagen a convertir: {imagenAConvertir.Width}x{imagenAConvertir.Height}");
                        logger.Debug($"Archivo destino: {sfd.FileName}");

                        btnConvertir.BottomText = "Procesando...";

                        // Crear adaptador y converter
                        var adapter = btnConvertir.AsProgressReporter();
                        var converter = new IconConverter(adapter, logger);

                        // Convertir en background
                        await Task.Run(() =>
                        {
                            converter.ConvertirAIconoDesdeImagen(imagenAConvertir, sfd.FileName, tamaños);
                        });

                        logger.Separator('-', 60);
                        logger.Info($"Icono creado exitosamente: {sfd.FileName}");

                        FileInfo fileInfo = new FileInfo(sfd.FileName);
                        logger.Info($"Tamaño del archivo: {fileInfo.Length / 1024.0:F2} KB");

                        // Completar con animación
                        btnConvertir.CompleteProgress();
                        btnConvertir.BottomText = "¡Completado!";
                        await Task.Delay(1000);

                        MessageBox.Show("¡Icono creado exitosamente!",
                            "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        logger.Error($"Error al convertir: {ex.Message}");
                        logger.Debug($"StackTrace: {ex.StackTrace}");

                        btnConvertir.StopProgress();
                        btnConvertir.BottomText = "Error";

                        MessageBox.Show($"Error al convertir: {ex.Message}",
                            "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                        await Task.Delay(2000);
                    }
                    finally
                    {
                        btnConvertir.StopProgress();
                        btnConvertir.BottomText = "Convertir ICO";
                        btnConvertir.Enabled = true;
                    }
                }
            }
        }


        private TipoAgarre ObtenerAgarreEnPosicion(Point punto)
        {
            if (areaSeleccion.IsEmpty) return TipoAgarre.Ninguno;

            int tolerancia = 8;
            int centroX = areaSeleccion.Left + areaSeleccion.Width / 2;
            int centroY = areaSeleccion.Top + areaSeleccion.Height / 2;

            // Verificar esquinas
            if (Math.Abs(punto.X - areaSeleccion.Left) <= tolerancia && Math.Abs(punto.Y - areaSeleccion.Top) <= tolerancia)
                return TipoAgarre.EsquinaSuperiorIzquierda;
            if (Math.Abs(punto.X - areaSeleccion.Right) <= tolerancia && Math.Abs(punto.Y - areaSeleccion.Top) <= tolerancia)
                return TipoAgarre.EsquinaSuperiorDerecha;
            if (Math.Abs(punto.X - areaSeleccion.Left) <= tolerancia && Math.Abs(punto.Y - areaSeleccion.Bottom) <= tolerancia)
                return TipoAgarre.EsquinaInferiorIzquierda;
            if (Math.Abs(punto.X - areaSeleccion.Right) <= tolerancia && Math.Abs(punto.Y - areaSeleccion.Bottom) <= tolerancia)
                return TipoAgarre.EsquinaInferiorDerecha;

            // Verificar laterales
            if (Math.Abs(punto.X - centroX) <= tolerancia && Math.Abs(punto.Y - areaSeleccion.Top) <= tolerancia)
                return TipoAgarre.LadoSuperior;
            if (Math.Abs(punto.X - centroX) <= tolerancia && Math.Abs(punto.Y - areaSeleccion.Bottom) <= tolerancia)
                return TipoAgarre.LadoInferior;
            if (Math.Abs(punto.X - areaSeleccion.Left) <= tolerancia && Math.Abs(punto.Y - centroY) <= tolerancia)
                return TipoAgarre.LadoIzquierdo;
            if (Math.Abs(punto.X - areaSeleccion.Right) <= tolerancia && Math.Abs(punto.Y - centroY) <= tolerancia)
                return TipoAgarre.LadoDerecho;

            // Verificar centro (para mover)
            if (areaSeleccion.Contains(punto))
                return TipoAgarre.Centro;

            return TipoAgarre.Ninguno;
        }

        private void ActualizarCursor(Point punto)
        {
            TipoAgarre agarre = ObtenerAgarreEnPosicion(punto);

            switch (agarre)
            {
                case TipoAgarre.EsquinaSuperiorIzquierda:
                case TipoAgarre.EsquinaInferiorDerecha:
                    pbPreview.Cursor = Cursors.SizeNWSE;
                    break;
                case TipoAgarre.EsquinaSuperiorDerecha:
                case TipoAgarre.EsquinaInferiorIzquierda:
                    pbPreview.Cursor = Cursors.SizeNESW;
                    break;
                case TipoAgarre.LadoSuperior:
                case TipoAgarre.LadoInferior:
                    pbPreview.Cursor = Cursors.SizeNS;
                    break;
                case TipoAgarre.LadoIzquierdo:
                case TipoAgarre.LadoDerecho:
                    pbPreview.Cursor = Cursors.SizeWE;
                    break;
                case TipoAgarre.Centro:
                    pbPreview.Cursor = Cursors.SizeAll;
                    break;
                default:
                    pbPreview.Cursor = Cursors.Default;
                    break;
            }
        }

        private void RedimensionarDesdeAgarre(Point punto)
        {
            Rectangle nuevo = areaSeleccion;

            switch (agarreActivo)
            {
                case TipoAgarre.EsquinaSuperiorIzquierda:
                    nuevo.Width += nuevo.X - punto.X;
                    nuevo.Height += nuevo.Y - punto.Y;
                    nuevo.X = punto.X;
                    nuevo.Y = punto.Y;
                    break;
                case TipoAgarre.EsquinaSuperiorDerecha:
                    nuevo.Width = punto.X - nuevo.X;
                    nuevo.Height += nuevo.Y - punto.Y;
                    nuevo.Y = punto.Y;
                    break;
                case TipoAgarre.EsquinaInferiorIzquierda:
                    nuevo.Width += nuevo.X - punto.X;
                    nuevo.X = punto.X;
                    nuevo.Height = punto.Y - nuevo.Y;
                    break;
                case TipoAgarre.EsquinaInferiorDerecha:
                    nuevo.Width = punto.X - nuevo.X;
                    nuevo.Height = punto.Y - nuevo.Y;
                    break;
                case TipoAgarre.LadoSuperior:
                    nuevo.Height += nuevo.Y - punto.Y;
                    nuevo.Y = punto.Y;
                    break;
                case TipoAgarre.LadoInferior:
                    nuevo.Height = punto.Y - nuevo.Y;
                    break;
                case TipoAgarre.LadoIzquierdo:
                    nuevo.Width += nuevo.X - punto.X;
                    nuevo.X = punto.X;
                    break;
                case TipoAgarre.LadoDerecho:
                    nuevo.Width = punto.X - nuevo.X;
                    break;
            }

            // Mantener proporción si está activado
            if (chkMantenerProporcion.Checked && (
                agarreActivo == TipoAgarre.EsquinaSuperiorIzquierda ||
                agarreActivo == TipoAgarre.EsquinaSuperiorDerecha ||
                agarreActivo == TipoAgarre.EsquinaInferiorIzquierda ||
                agarreActivo == TipoAgarre.EsquinaInferiorDerecha))
            {
                int tamMin = Math.Min(nuevo.Width, nuevo.Height);
                nuevo.Width = nuevo.Height = tamMin;
            }

            // Limitar al área del PictureBox y tamaño mínimo
            if (nuevo.X < 0) { nuevo.Width += nuevo.X; nuevo.X = 0; }
            if (nuevo.Y < 0) { nuevo.Height += nuevo.Y; nuevo.Y = 0; }
            if (nuevo.Right > pbPreview.Width) nuevo.Width = pbPreview.Width - nuevo.X;
            if (nuevo.Bottom > pbPreview.Height) nuevo.Height = pbPreview.Height - nuevo.Y;
            if (nuevo.Width < 20) nuevo.Width = 20;
            if (nuevo.Height < 20) nuevo.Height = 20;

            areaSeleccion = nuevo;
            pbPreview.Invalidate();
            ActualizarEtiquetas();
        }

        private void ActualizarEtiquetas()
        {
            lblDimensiones.Text = $"Selección: {areaSeleccion.Width} x {areaSeleccion.Height} px";
        }

        // ===== NUEVAS VARIABLES PARA CONVERSIÓN POR LOTES =====
        private List<ArchivoImagen> archivosLote = new List<ArchivoImagen>();
        private string carpetaActual = "";
        private bool conversionLoteEnProceso = false;

        // ===== CLASE PARA MANEJAR ARCHIVOS EN LOTE =====
        private class ArchivoImagen
        {
            public string RutaCompleta { get; set; }
            public string NombreArchivo { get; set; }
            public System.Drawing.Image Imagen { get; set; }
            public int Ancho { get; set; }
            public int Alto { get; set; }
            public long TamañoBytes { get; set; }
            public bool Seleccionado { get; set; }
            public EstadoConversion Estado { get; set; }
            public string Mensaje { get; set; }

            public string Info => $"{NombreArchivo} ({Ancho}x{Alto})";
            public string TamañoFormateado => $"{TamañoBytes / 1024.0:F2} KB";
        }

        private enum EstadoConversion
        {
            Pendiente,
            Procesando,
            Completado,
            Error
        }

        // ===== MÉTODO PARA INICIALIZAR CONTROLES DE LOTE =====
        // Llamar este método en el constructor después de InitializeComponent()
        private void InicializarConversionLote()
        {
            // Configurar ListView para archivos
            lvArchivosLote.View = View.Details;
            lvArchivosLote.FullRowSelect = true;
            lvArchivosLote.CheckBoxes = true;
            lvArchivosLote.GridLines = true;

            // Columnas
            lvArchivosLote.Columns.Add("Archivo", 300);
            lvArchivosLote.Columns.Add("Dimensiones", 100);
            lvArchivosLote.Columns.Add("Tamaño", 80);
            lvArchivosLote.Columns.Add("Estado", 110);

            // Eventos
            btnSeleccionarCarpeta.Click += BtnSeleccionarCarpeta_Click;
            btnAgregarArchivos.Click += BtnAgregarArchivos_Click;
            btnSeleccionarTodos.Click += BtnSeleccionarTodos_Click;
            btnDeseleccionarTodos.Click += BtnDeseleccionarTodos_Click;
            btnLimpiarLista.Click += BtnLimpiarLista_Click;
            btnConvertirLote.Click += BtnConvertirLote_Click;
            lvArchivosLote.ItemChecked += LvArchivosLote_ItemChecked;

            lvArchivosLote.SelectedIndexChanged += LvArchivosLote_SelectedIndexChanged;


            logger.Debug("Controles de conversión por lotes inicializados");
        }

        private void LvArchivosLote_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lvArchivosLote.SelectedItems.Count == 0)
            {
                pbPreviewLote.Image = null;
                lblPreviewLote.Text = "Vista Previa";
                return;
            }

            try
            {
                // Obtener el archivo seleccionado
                var item = lvArchivosLote.SelectedItems[0];
                var archivo = item.Tag as ArchivoImagen;

                if (archivo != null && archivo.Imagen != null)
                {
                    // Mostrar imagen en el preview
                    pbPreviewLote.Image = new Bitmap(archivo.Imagen);
                    lblPreviewLote.Text = $"{archivo.NombreArchivo} - {archivo.Ancho}x{archivo.Alto}";

                    logger?.Debug($"Preview mostrado: {archivo.NombreArchivo}");
                }
            }
            catch (Exception ex)
            {
                logger?.Error($"Error al mostrar preview: {ex.Message}");
                pbPreviewLote.Image = null;
                lblPreviewLote.Text = "Error al cargar preview";
            }
        }

        // ===== SELECCIONAR CARPETA =====
        private void BtnSeleccionarCarpeta_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog fbd = new FolderBrowserDialog())
            {
                fbd.Description = "Selecciona una carpeta con imágenes";
                fbd.ShowNewFolderButton = false;

                if (fbd.ShowDialog() == DialogResult.OK)
                {
                    carpetaActual = fbd.SelectedPath;
                    CargarImagenesDeCarpeta(carpetaActual);
                }
            }
        }

        // ===== CARGAR IMÁGENES DE CARPETA =====
        private void CargarImagenesDeCarpeta(string carpeta)
        {
            try
            {
                logger.Info($"Cargando imágenes desde: {carpeta}");

                string[] extensiones = { "*.png", "*.jpg", "*.jpeg", "*.bmp", "*.gif" };
                List<string> archivos = new List<string>();

                foreach (string extension in extensiones)
                {
                    archivos.AddRange(Directory.GetFiles(carpeta, extension, SearchOption.TopDirectoryOnly));
                }

                if (archivos.Count == 0)
                {
                    MessageBox.Show("No se encontraron imágenes en la carpeta seleccionada.",
                        "Sin imágenes", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                // Limpiar lista actual
                archivosLote.Clear();
                lvArchivosLote.Items.Clear();

                // Cargar archivos
                int cargados = 0;
                foreach (string archivo in archivos)
                {
                    if (AgregarArchivoALista(archivo))
                        cargados++;
                }

                lblCantidadArchivos.Text = $"{cargados} archivos cargados";
                lblCarpetaActual.Text = $"Carpeta: {Path.GetFileName(carpeta)}";

                logger.Info($"✓ {cargados} imágenes cargadas exitosamente");

                // Seleccionar todos por defecto
                BtnSeleccionarTodos_Click(null, null);
            }
            catch (Exception ex)
            {
                logger.Error($"Error al cargar carpeta: {ex.Message}", ex);
                MessageBox.Show($"Error al cargar la carpeta:\n{ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ===== AGREGAR ARCHIVOS INDIVIDUALES =====
        private void BtnAgregarArchivos_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "Imágenes|*.png;*.jpg;*.jpeg;*.bmp;*.gif";
                ofd.Title = "Seleccionar imágenes";
                ofd.Multiselect = true;

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    int agregados = 0;
                    foreach (string archivo in ofd.FileNames)
                    {
                        if (AgregarArchivoALista(archivo))
                            agregados++;
                    }

                    lblCantidadArchivos.Text = $"{archivosLote.Count} archivos en lista";
                    logger.Info($"✓ {agregados} archivos agregados");
                }
            }
        }

        // ===== AGREGAR ARCHIVO A LA LISTA =====
        private bool AgregarArchivoALista(string rutaArchivo)
        {
            try
            {
                // Verificar si ya existe
                if (archivosLote.Any(a => a.RutaCompleta == rutaArchivo))
                {
                    logger.Debug($"Archivo ya existe en lista: {Path.GetFileName(rutaArchivo)}");
                    return false;
                }

                // Cargar imagen para obtener información
                using (System.Drawing.Image img = System.Drawing.Image.FromFile(rutaArchivo))
                {
                    FileInfo fi = new FileInfo(rutaArchivo);

                    ArchivoImagen archivo = new ArchivoImagen
                    {
                        RutaCompleta = rutaArchivo,
                        NombreArchivo = Path.GetFileName(rutaArchivo),
                        Ancho = img.Width,
                        Alto = img.Height,
                        TamañoBytes = fi.Length,
                        Seleccionado = false,
                        Estado = EstadoConversion.Pendiente,
                        Mensaje = "Listo para convertir"
                    };

                    // Crear copia de la imagen para uso posterior
                    archivo.Imagen = new Bitmap(img);

                    archivosLote.Add(archivo);

                    // Agregar a ListView
                    ListViewItem item = new ListViewItem(archivo.NombreArchivo);
                    item.SubItems.Add($"{archivo.Ancho}x{archivo.Alto}");
                    item.SubItems.Add(archivo.TamañoFormateado);
                    item.SubItems.Add("Pendiente");
                    item.Tag = archivo;
                    item.Checked = false;

                    lvArchivosLote.Items.Add(item);

                    return true;
                }
            }
            catch (Exception ex)
            {
                logger.Warning($"No se pudo cargar: {Path.GetFileName(rutaArchivo)} - {ex.Message}");
                return false;
            }
        }

        // ===== SELECCIONAR/DESELECCIONAR TODOS =====
        private void BtnSeleccionarTodos_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in lvArchivosLote.Items)
            {
                item.Checked = true;
            }
            ActualizarContadorSeleccionados();
        }

        private void BtnDeseleccionarTodos_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in lvArchivosLote.Items)
            {
                item.Checked = false;
            }
            ActualizarContadorSeleccionados();
        }

        // ===== LIMPIAR LISTA =====
        private void BtnLimpiarLista_Click(object sender, EventArgs e)
        {
            var resultado = MessageBox.Show(
                "¿Deseas limpiar toda la lista de archivos?",
                "Confirmar",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (resultado == DialogResult.Yes)
            {
                // Liberar imágenes
                foreach (var archivo in archivosLote)
                {
                    archivo.Imagen?.Dispose();
                }

                archivosLote.Clear();
                lvArchivosLote.Items.Clear();
                lblCantidadArchivos.Text = "0 archivos";
                lblCarpetaActual.Text = "Ninguna carpeta seleccionada";

                logger.Info("Lista de archivos limpiada");
            }
        }

        // ===== ACTUALIZAR CONTADOR =====
        private void LvArchivosLote_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            if (e.Item.Tag is ArchivoImagen archivo)
            {
                archivo.Seleccionado = e.Item.Checked;
            }
            ActualizarContadorSeleccionados();
        }

        private void ActualizarContadorSeleccionados()
        {
            int seleccionados = archivosLote.Count(a => a.Seleccionado);
            lblSeleccionados.Text = $"{seleccionados} seleccionados de {archivosLote.Count}";
        }

        // ===== CONVERTIR POR LOTES =====
        private async void BtnConvertirLote_Click(object sender, EventArgs e)
        {
            var archivosSeleccionados = archivosLote.Where(a => a.Seleccionado).ToList();

            if (archivosSeleccionados.Count == 0)
            {
                MessageBox.Show("Selecciona al menos un archivo para convertir.",
                    "Sin archivos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (clbTamaños.CheckedItems.Count == 0)
            {
                MessageBox.Show("Selecciona al menos un tamaño de icono.",
                    "Sin tamaños", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Seleccionar carpeta de destino
            using (FolderBrowserDialog fbd = new FolderBrowserDialog())
            {
                fbd.Description = "Selecciona la carpeta donde guardar los iconos";
                fbd.ShowNewFolderButton = true;

                if (fbd.ShowDialog() != DialogResult.OK)
                    return;

                string carpetaDestino = fbd.SelectedPath;

                // Confirmar conversión
                var resultado = MessageBox.Show(
                    $"¿Convertir {archivosSeleccionados.Count} imágenes a formato ICO?\n\n" +
                    $"Carpeta destino: {carpetaDestino}",
                    "Confirmar conversión",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question
                );

                if (resultado != DialogResult.Yes)
                    return;

                // Iniciar conversión
                await ConvertirLoteAsync(archivosSeleccionados, carpetaDestino);
            }
        }

        // ===== PROCESO DE CONVERSIÓN POR LOTES =====
        private async Task ConvertirLoteAsync(List<ArchivoImagen> archivos, string carpetaDestino)
        {
            conversionLoteEnProceso = true;

            // Deshabilitar controles
            btnConvertirLote.Enabled = false;
            btnSeleccionarCarpeta.Enabled = false;
            btnAgregarArchivos.Enabled = false;
            btnLimpiarLista.Enabled = false;

            // Configurar progreso general
            progressBarLote.Maximum = archivos.Count;
            progressBarLote.Value = 0;
            progressBarLote.Visible = true;

            int[] tamaños = ObtenerTamañosSeleccionados();
            int exitosos = 0;
            int errores = 0;

            logger.Info("=== CONVERSIÓN POR LOTES INICIADA ===");
            logger.Separator('=', 80);
            logger.Info($"Total de archivos: {archivos.Count}");
            logger.Info($"Tamaños: {string.Join(", ", tamaños)}");
            logger.Info($"Carpeta destino: {carpetaDestino}");
            logger.Separator('-', 80);

            try
            {
                for (int i = 0; i < archivos.Count; i++)
                {
                    var archivo = archivos[i];

                    // Actualizar UI
                    ActualizarEstadoArchivo(archivo, EstadoConversion.Procesando, "Procesando...");
                    lblArchivoActual.Text = $"Procesando: {archivo.NombreArchivo}";

                    try
                    {
                        string nombreSalida = Path.GetFileNameWithoutExtension(archivo.NombreArchivo) + ".ico";
                        string rutaSalida = Path.Combine(carpetaDestino, nombreSalida);

                        logger.Debug($"[{i + 1}/{archivos.Count}] Procesando: {archivo.NombreArchivo}");

                        // Crear adaptador para progreso individual (opcional)
                        var adapter = progressBarIndividual.AsProgressReporter();
                        var converter = new IconConverter(adapter, logger);

                        // Convertir
                        await Task.Run(() =>
                        {
                            converter.ConvertirAIconoDesdeImagen(
                                archivo.Imagen,
                                rutaSalida,
                                tamaños
                            );
                        });

                        // Éxito
                        FileInfo fi = new FileInfo(rutaSalida);
                        ActualizarEstadoArchivo(archivo, EstadoConversion.Completado,
                            $"Completado ({fi.Length / 1024.0:F2} KB)");

                        exitosos++;
                        logger.Info($"✓ [{i + 1}/{archivos.Count}] {archivo.NombreArchivo} -> {fi.Length / 1024.0:F2} KB");
                    }
                    catch (Exception ex)
                    {
                        // Error
                        ActualizarEstadoArchivo(archivo, EstadoConversion.Error, $"Error: {ex.Message}");
                        errores++;
                        logger.Error($"✗ [{i + 1}/{archivos.Count}] {archivo.NombreArchivo}: {ex.Message}");
                    }

                    // Actualizar progreso general
                    progressBarLote.Value = i + 1;
                    lblProgresoLote.Text = $"Progreso: {i + 1}/{archivos.Count}";

                    // Pequeña pausa para que la UI se actualice
                    await Task.Delay(50);
                }

                // Resumen final
                logger.Separator('-', 80);
                logger.Info("=== CONVERSIÓN POR LOTES COMPLETADA ===");
                logger.Info($"Exitosos: {exitosos}");
                logger.Info($"Errores: {errores}");
                logger.Info($"Total procesado: {archivos.Count}");
                logger.Separator('=', 80);

                // Mostrar resumen
                MessageBox.Show(
                    $"Conversión por lotes completada\n\n" +
                    $"Exitosos: {exitosos}\n" +
                    $"Errores: {errores}\n" +
                    $"Total: {archivos.Count}\n\n" +
                    $"Carpeta destino: {carpetaDestino}",
                    "Conversión completada",
                    MessageBoxButtons.OK,
                    exitosos == archivos.Count ? MessageBoxIcon.Information : MessageBoxIcon.Warning
                );

                // Abrir carpeta destino
                var abrirCarpeta = MessageBox.Show(
                    "¿Deseas abrir la carpeta con los iconos creados?",
                    "Abrir carpeta",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question
                );

                if (abrirCarpeta == DialogResult.Yes)
                {
                    System.Diagnostics.Process.Start("explorer.exe", carpetaDestino);
                }
            }
            finally
            {
                // Restaurar UI
                conversionLoteEnProceso = false;
                btnConvertirLote.Enabled = true;
                btnSeleccionarCarpeta.Enabled = true;
                btnAgregarArchivos.Enabled = true;
                btnLimpiarLista.Enabled = true;
                progressBarLote.Visible = false;
                lblArchivoActual.Text = "Sin procesar";
                lblProgresoLote.Text = "0/0";
            }
        }

        // ===== ACTUALIZAR ESTADO EN LISTVIEW =====
        private void ActualizarEstadoArchivo(ArchivoImagen archivo, EstadoConversion estado, string mensaje)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => ActualizarEstadoArchivo(archivo, estado, mensaje)));
                return;
            }

            archivo.Estado = estado;
            archivo.Mensaje = mensaje;

            // Buscar item en ListView
            foreach (ListViewItem item in lvArchivosLote.Items)
            {
                if (item.Tag == archivo)
                {
                    item.SubItems[3].Text = mensaje;

                    // Colorear según estado
                    switch (estado)
                    {
                        case EstadoConversion.Procesando:
                            item.BackColor = Color.LightYellow;
                            break;
                        case EstadoConversion.Completado:
                            item.BackColor = Color.LightGreen;
                            break;
                        case EstadoConversion.Error:
                            item.BackColor = Color.LightCoral;
                            break;
                        default:
                            item.BackColor = Color.White;
                            break;
                    }

                    item.EnsureVisible();
                    break;
                }
            }
        }

        // ===== LIMPIAR RECURSOS AL CERRAR =====
        // ===== OVERRIDE FORMCLOSING =====
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (conversionLoteEnProceso)
            {
                var resultado = MessageBox.Show(
                    "Hay una conversión en proceso. ¿Deseas cancelar y cerrar?",
                    "Conversión en proceso",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning
                );

                if (resultado != DialogResult.Yes)
                {
                    e.Cancel = true;
                    return;
                }
            }

            // Liberar imágenes del lote
            foreach (var archivo in archivosLote)
            {
                archivo.Imagen?.Dispose();
            }

            // ⭐ NUEVO: Liberar imagen con fondo eliminado
            imagenSinFondo?.Dispose();

            logger?.Info("Aplicación cerrándose...");
            logger?.Dispose();

            base.OnFormClosing(e);
        }

        // ===== FIN DE LA IMPLEMENTACIÓN DE CONVERSIÓN POR LOTES =====
        // ============================================================
        // ===== INICIALIZAR EDITOR DE ICONOS =========================
        #region EditorIconos
        private void InicializarEditorIconos()
        {
            // ⭐ AGREGAR ESTAS LÍNEAS AL INICIO:
            editorPixeles = new EditorPixeles();
            //editorPixeles.Dock = DockStyle.Fill;
            panelEdicionPixeles.Controls.Add(editorPixeles);

            // Configurar ListView de iconos
            lvIconos.View = View.Details;
            lvIconos.FullRowSelect = true;
            lvIconos.GridLines = true;

            // Columnas
            lvIconos.Columns.Clear();
            lvIconos.Columns.Add("Nombre", 200);
            lvIconos.Columns.Add("Tamaños", 90);

            // Control de tolerancia para varita mágica
            trackBarTolerancia.Minimum = 0;
            trackBarTolerancia.Maximum = 100;
            trackBarTolerancia.Value = 30;
            trackBarTolerancia.ValueChanged += (s, e) =>
            {
                editorPixeles.ToleranciaVaritaMagica = trackBarTolerancia.Value;
                lblNivelTolerancia.Text = $"Tolerancia: {trackBarTolerancia.Value}";
            };

            // Eventos
            btnCargarIcono.Click += BtnCargarIcono_Click;
            btnExplorarCarpetaIconos.Click += BtnExplorarCarpetaIconos_Click;
            lvIconos.SelectedIndexChanged += LvIconos_SelectedIndexChanged;
            lstTamañosIcono.SelectedIndexChanged += LstTamañosIcono_SelectedIndexChanged;

            // Eventos de herramientas
            btnRedimensionar.Click += BtnRedimensionar_Click;
            btnRotar.Click += BtnRotar_Click;
            btnEspejo.Click += BtnEspejo_Click;
            btnBrillo.Click += BtnBrillo_Click;
            btnContraste.Click += BtnContraste_Click;
            btnEscalaGrises.Click += BtnEscalaGrises_Click;
            trackBarAjuste.ValueChanged += TrackBarAjuste_ValueChanged;
            btnAplicarCambios.Click += BtnAplicarCambios_Click;
            btnGuardarIcono.Click += BtnGuardarIcono_Click;
            btnExportarTamaño.Click += BtnExportarTamaño_Click;
            btnRevertir.Click += BtnRevertir_Click;

            // Eventos del editor de píxeles
            trackBarZoom.ValueChanged += TrackBarZoom_ValueChanged;
            btnZoomMas.Click += BtnZoomMas_Click;
            btnZoomMenos.Click += BtnZoomMenos_Click;
            btnZoomAjustar.Click += BtnZoomAjustar_Click;

            rbPincel.CheckedChanged += HerramientaDibujo_CheckedChanged;
            rbBorrador.CheckedChanged += HerramientaDibujo_CheckedChanged;
            rbCuentagotas.CheckedChanged += HerramientaDibujo_CheckedChanged;
            rbRelleno.CheckedChanged += HerramientaDibujo_CheckedChanged;
            rbVaritaMagica.CheckedChanged += HerramientaDibujo_CheckedChanged;  // ⭐ NUEVO

            btnSeleccionarColor.Click += BtnSeleccionarColor_Click;
            chkMostrarCuadricula.CheckedChanged += ChkMostrarCuadricula_CheckedChanged;

            btnDeshacerPixeles.Click += BtnDeshacerPixeles_Click;
            btnLimpiarCanvas.Click += BtnLimpiarCanvas_Click;
            btnAplicarEdicionPixeles.Click += BtnAplicarEdicionPixeles_Click;

            // Eventos del control EditorPixeles
            //editorPixeles.PixelSeleccionado += EditorPixeles_PixelSeleccionado;
            // En InicializarEditorIconos(), después de los eventos de herramientas
            editorPixeles.PixelSeleccionado += (s, args) =>
            {
                if (rbVaritaMagica.Checked && args.Color.A >= 10)
                {
                    // Mostrar información del color seleccionado
                    lblInfoPixel.Text = $"X: {args.Posicion.X}\n" +
                                       $"Y: {args.Posicion.Y}\n" +
                                       $"RGB: {args.Color.R},{args.Color.G},{args.Color.B}\n" +
                                       $"A: {args.Color.A}\n" +
                                       $"Click para eliminar";
                }
                else if (args.Posicion.X >= 0)
                {
                    lblInfoPixel.Text = $"X: {args.Posicion.X}\n" +
                                       $"Y: {args.Posicion.Y}\n" +
                                       $"RGB: {args.Color.R},{args.Color.G},{args.Color.B}\n" +
                                       $"A: {args.Color.A}";
                }
                else
                {
                    lblInfoPixel.Text = "X: -\nY: -\nRGB: -";
                }
            };
            editorPixeles.ImagenModificada += EditorPixeles_ImagenModificada;

            logger?.Debug("Editor de iconos inicializado");
        }

        // ===== CARGAR ICONO INDIVIDUAL =====

        private void BtnCargarIcono_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "Archivos de Icono|*.ico|Todos los archivos|*.*";
                ofd.Title = "Seleccionar icono";

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    CargarIcono(ofd.FileName);
                }
            }
        }

        // ===== EXPLORAR CARPETA DE ICONOS =====

        private void BtnExplorarCarpetaIconos_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog fbd = new FolderBrowserDialog())
            {
                fbd.Description = "Selecciona una carpeta con iconos";

                if (fbd.ShowDialog() == DialogResult.OK)
                {
                    CargarIconosDeCarpeta(fbd.SelectedPath);
                }
            }
        }

        private void CargarIconosDeCarpeta(string carpeta)
        {
            try
            {
                logger?.Info($"Cargando iconos desde: {carpeta}");

                string[] archivos = Directory.GetFiles(carpeta, "*.ico", SearchOption.TopDirectoryOnly);

                if (archivos.Length == 0)
                {
                    MessageBox.Show("No se encontraron archivos .ico en la carpeta seleccionada.",
                        "Sin iconos", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                lvIconos.Items.Clear();

                foreach (string archivo in archivos)
                {
                    try
                    {
                        using (Icon icono = new Icon(archivo))
                        {
                            ListViewItem item = new ListViewItem(Path.GetFileName(archivo));
                            item.SubItems.Add(ObtenerTamañosIcono(archivo));
                            item.Tag = archivo;
                            lvIconos.Items.Add(item);
                        }
                    }
                    catch (Exception ex)
                    {
                        logger?.Warning($"Error cargando {Path.GetFileName(archivo)}: {ex.Message}");
                    }
                }

                logger?.Info($"✓ {lvIconos.Items.Count} iconos cargados");
            }
            catch (Exception ex)
            {
                logger?.Error($"Error al cargar carpeta de iconos: {ex.Message}", ex);
                MessageBox.Show($"Error al cargar carpeta:\n{ex.Message}", "Error");
            }
        }

        private string ObtenerTamañosIcono(string rutaIcono)
        {
            try
            {
                var tamaños = new List<int>();
                using (FileStream fs = new FileStream(rutaIcono, FileMode.Open, FileAccess.Read))
                using (BinaryReader br = new BinaryReader(fs))
                {
                    // Leer header ICO
                    br.ReadInt16(); // Reserved
                    short type = br.ReadInt16(); // Type (1 = icon)
                    short count = br.ReadInt16(); // Number of images

                    if (type != 1) return "N/A";

                    for (int i = 0; i < count; i++)
                    {
                        byte width = br.ReadByte();
                        byte height = br.ReadByte();
                        br.ReadBytes(14); // Skip rest of entry

                        int tamaño = width == 0 ? 256 : width;
                        tamaños.Add(tamaño);
                    }
                }

                tamaños.Sort();
                tamaños.Reverse();
                return string.Join(", ", tamaños);
            }
            catch
            {
                return "Error";
            }
        }

        // ===== CARGAR ICONO SELECCIONADO =====

        private void LvIconos_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lvIconos.SelectedItems.Count == 0) return;

            string rutaIcono = lvIconos.SelectedItems[0].Tag as string;
            if (!string.IsNullOrEmpty(rutaIcono))
            {
                CargarIcono(rutaIcono);
            }
        }

        private void CargarIcono(string rutaIcono)
        {
            try
            {
                logger?.Info($"Cargando icono: {Path.GetFileName(rutaIcono)}");

                // Liberar recursos anteriores
                iconoActual?.Dispose();
                imagenOriginalIcono?.Dispose();
                imagenEditadaIcono?.Dispose();
                foreach (var img in tamañosIcono.Values)
                    img?.Dispose();
                tamañosIcono.Clear();

                // Cargar icono
                iconoActualPath = rutaIcono;
                iconoActual = new Icon(rutaIcono);

                // Extraer todos los tamaños
                ExtraerTamañosIcono(rutaIcono);

                // Seleccionar el tamaño más grande como principal
                if (tamañosIcono.Count > 0)
                {
                    int tamañoMax = tamañosIcono.Keys.Max();
                    imagenOriginalIcono = new Bitmap(tamañosIcono[tamañoMax]);
                    imagenEditadaIcono = new Bitmap(imagenOriginalIcono);

                    // Mostrar en el editor
                    pbIconoEditor.Image = new Bitmap(imagenEditadaIcono);
                    lblInfoIcono.Text = $"{Path.GetFileName(rutaIcono)} - {tamañosIcono.Count} tamaños";

                    // Actualizar lista de tamaños
                    ActualizarListaTamaños();

                    // Resetear herramientas
                    trackBarAjuste.Value = 0;
                    herramientaActiva = "";

                    // ⭐ NUEVO: Cargar en el editor de píxeles
                    if (tamañosIcono.Count > 0)
                    {
                        //int tamañoMax = tamañosIcono.Keys.Max();
                        editorPixeles.Imagen = new Bitmap(tamañosIcono[tamañoMax]);
                        AjustarZoomSegunTamaño(tamañoMax);
                    }

                    logger?.Info($"✓ Icono cargado: {tamañosIcono.Count} tamaños encontrados");
                }
            }
            catch (Exception ex)
            {
                logger?.Error($"Error al cargar icono: {ex.Message}", ex);
                MessageBox.Show($"Error al cargar icono:\n{ex.Message}", "Error");
            }
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            // Solo procesar si estamos en el tab del editor
            if (tabControl1.SelectedTab == tabEditor)
            {
                switch (keyData)
                {
                    case Keys.B: // Pincel
                        rbPincel.Checked = true;
                        return true;

                    case Keys.E: // Borrador
                        rbBorrador.Checked = true;
                        return true;

                    case Keys.I: // Cuentagotas
                        rbCuentagotas.Checked = true;
                        return true;

                    case Keys.G: // Relleno (G de "llenar")
                        rbRelleno.Checked = true;
                        return true;
                    case Keys.W: // Varita Mágica (W de "Wand")  ⭐ NUEVO
                        rbVaritaMagica.Checked = true;
                        return true;

                    case Keys.Add: // Zoom +
                    case Keys.Oemplus:
                        if (trackBarZoom.Value < trackBarZoom.Maximum)
                            trackBarZoom.Value++;
                        return true;

                    case Keys.Subtract: // Zoom -
                    case Keys.OemMinus:
                        if (trackBarZoom.Value > trackBarZoom.Minimum)
                            trackBarZoom.Value--;
                        return true;

                    case Keys.D0: // Zoom ajustar
                        BtnZoomAjustar_Click(null, null);
                        return true;

                    case (Keys.Control | Keys.Z): // Deshacer
                        BtnDeshacerPixeles_Click(null, null);
                        return true;
                }
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void ExtraerTamañosIcono(string rutaIcono)
        {
            try
            {
                using (FileStream fs = new FileStream(rutaIcono, FileMode.Open, FileAccess.Read))
                using (BinaryReader br = new BinaryReader(fs))
                {
                    // Leer header
                    br.ReadInt16(); // Reserved
                    br.ReadInt16(); // Type
                    short count = br.ReadInt16(); // Count

                    // Leer cada entrada
                    var entries = new List<(int tamaño, int offset, int size)>();

                    for (int i = 0; i < count; i++)
                    {
                        byte width = br.ReadByte();
                        byte height = br.ReadByte();
                        br.ReadBytes(6); // Color count, reserved, planes, bpp
                        int size = br.ReadInt32();
                        int offset = br.ReadInt32();

                        int tamaño = width == 0 ? 256 : width;
                        entries.Add((tamaño, offset, size));
                    }

                    // Extraer cada imagen
                    foreach (var entry in entries)
                    {
                        fs.Seek(entry.offset, SeekOrigin.Begin);
                        byte[] data = br.ReadBytes(entry.size);

                        using (MemoryStream ms = new MemoryStream(data))
                        {
                            try
                            {
                                System.Drawing.Image img = System.Drawing.Image.FromStream(ms);
                                tamañosIcono[entry.tamaño] = new Bitmap(img);
                            }
                            catch
                            {
                                logger?.Warning($"No se pudo extraer tamaño {entry.tamaño}x{entry.tamaño}");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger?.Error($"Error extrayendo tamaños: {ex.Message}");
            }
        }

        private void ActualizarListaTamaños()
        {
            lstTamañosIcono.Items.Clear();

            foreach (var tamaño in tamañosIcono.Keys.OrderByDescending(t => t))
            {
                lstTamañosIcono.Items.Add($"{tamaño}x{tamaño} px");
            }

            if (lstTamañosIcono.Items.Count > 0)
            {
                lstTamañosIcono.SelectedIndex = 0;
            }
        }

        // ===== CONTROL DE ZOOM =====

        private void AjustarZoomSegunTamaño(int tamaño)
        {
            // Calcular zoom automático para que quepa bien en el panel
            int anchoPanel = panelEdicionPixeles.ClientSize.Width - panelControlsPixeles.Width - 40;
            int altoPanel = panelEdicionPixeles.ClientSize.Height - 40;

            int zoomPorAncho = Math.Max(1, anchoPanel / tamaño);
            int zoomPorAlto = Math.Max(1, altoPanel / tamaño);
            int zoomOptimo = Math.Min(zoomPorAncho, zoomPorAlto);

            // Limitar el zoom entre 2 y 50
            zoomOptimo = Math.Max(2, Math.Min(50, zoomOptimo));

            trackBarZoom.Value = zoomOptimo;
            editorPixeles.Zoom = zoomOptimo;
            lblNivelZoom.Text = $"{zoomOptimo}x";
        }

        private void TrackBarZoom_ValueChanged(object sender, EventArgs e)
        {
            int zoom = trackBarZoom.Value;
            editorPixeles.Zoom = zoom;
            lblNivelZoom.Text = $"{zoom}x";

            logger?.Debug($"Zoom ajustado a {zoom}x");
        }

        private void BtnZoomMas_Click(object sender, EventArgs e)
        {
            if (trackBarZoom.Value < trackBarZoom.Maximum)
            {
                trackBarZoom.Value++;
            }
        }

        private void BtnZoomMenos_Click(object sender, EventArgs e)
        {
            if (trackBarZoom.Value > trackBarZoom.Minimum)
            {
                trackBarZoom.Value--;
            }
        }

        private void BtnZoomAjustar_Click(object sender, EventArgs e)
        {
            if (tamañoSeleccionado > 0)
            {
                AjustarZoomSegunTamaño(tamañoSeleccionado);
            }
        }

        // ===== HERRAMIENTAS DE DIBUJO =====

        private void HerramientaDibujo_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rb = sender as RadioButton;
            if (rb == null || !rb.Checked) return;

            if (rb == rbPincel)
            {
                editorPixeles.Herramienta = EditorPixeles.HerramientaDibujo.Pincel;
                logger?.Debug("Herramienta: Pincel");
            }
            else if (rb == rbBorrador)
            {
                editorPixeles.Herramienta = EditorPixeles.HerramientaDibujo.Borrador;
                logger?.Debug("Herramienta: Borrador");
            }
            else if (rb == rbCuentagotas)
            {
                editorPixeles.Herramienta = EditorPixeles.HerramientaDibujo.Cuentagotas;
                logger?.Debug("Herramienta: Cuentagotas");
            }
            else if (rb == rbRelleno)
            {
                editorPixeles.Herramienta = EditorPixeles.HerramientaDibujo.Relleno;
                logger?.Debug("Herramienta: Relleno");
            }
            else if (rb == rbVaritaMagica)  // ⭐ NUEVO
            {
                editorPixeles.Herramienta = EditorPixeles.HerramientaDibujo.VaritaMagica;
                logger?.Debug("Herramienta: Varita Mágica");
            }
        }


        // ===== SELECTOR DE COLOR =====

        private void BtnSeleccionarColor_Click(object sender, EventArgs e)
        {
            using (ColorDialog cd = new ColorDialog())
            {
                cd.AllowFullOpen = true;
                cd.AnyColor = true;
                cd.FullOpen = true;
                cd.Color = editorPixeles.ColorDibujo;

                if (cd.ShowDialog() == DialogResult.OK)
                {
                    editorPixeles.ColorDibujo = cd.Color;
                    panelColorActual.BackColor = cd.Color;

                    logger?.Debug($"Color seleccionado: R={cd.Color.R}, G={cd.Color.G}, B={cd.Color.B}");
                }
            }
        }

        // ===== CUADRÍCULA =====

        private void ChkMostrarCuadricula_CheckedChanged(object sender, EventArgs e)
        {
            editorPixeles.MostrarCuadricula = chkMostrarCuadricula.Checked;
        }

        // ===== EVENTOS DEL EDITOR DE PÍXELES =====

        private void EditorPixeles_PixelSeleccionado(object sender, PixelSeleccionadoEventArgs e)
        {
            // Actualizar información del píxel
            if (e.Posicion.X >= 0 && e.Posicion.Y >= 0)
            {
                lblInfoPixel.Text = $"X: {e.Posicion.X}\n" +
                                   $"Y: {e.Posicion.Y}\n" +
                                   $"RGB: {e.Color.R},{e.Color.G},{e.Color.B}\n" +
                                   $"A: {e.Color.A}";

                // Si la herramienta es cuentagotas, actualizar el color actual
                if (rbCuentagotas.Checked)
                {
                    editorPixeles.ColorDibujo = e.Color;
                    panelColorActual.BackColor = e.Color;
                }
            }
            else
            {
                lblInfoPixel.Text = "X: -\nY: -\nRGB: -";
            }
        }

        private void EditorPixeles_ImagenModificada(object sender, EventArgs e)
        {
            // La imagen fue modificada en el editor de píxeles
            logger?.Debug("Imagen modificada en el editor de píxeles");
        }

        // ===== ACCIONES =====

        private void BtnDeshacerPixeles_Click(object sender, EventArgs e)
        {
            var resultado = MessageBox.Show(
                "¿Deshacer todos los cambios de píxeles?",
                "Deshacer",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (resultado == DialogResult.Yes)
            {
                editorPixeles.Deshacer();
                logger?.Info("Cambios de píxeles deshechos");
            }
        }

        private void BtnLimpiarCanvas_Click(object sender, EventArgs e)
        {
            var resultado = MessageBox.Show(
                "¿Limpiar todo el canvas (fondo transparente)?",
                "Limpiar",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning
            );

            if (resultado == DialogResult.Yes)
            {
                editorPixeles.Limpiar();
                logger?.Info("Canvas limpiado");
            }
        }

        private void BtnAplicarEdicionPixeles_Click(object sender, EventArgs e)
        {
            try
            {
                // Obtener la imagen editada del editor de píxeles
                Bitmap imagenEditadaPorPixeles = editorPixeles.ObtenerImagen();

                if (imagenEditadaPorPixeles == null)
                {
                    MessageBox.Show("No hay imagen editada para aplicar", "Sin cambios");
                    return;
                }

                // Actualizar la imagen principal
                imagenEditadaIcono?.Dispose();
                imagenEditadaIcono = new Bitmap(imagenEditadaPorPixeles);
                pbIconoEditor.Image = new Bitmap(imagenEditadaIcono);

                // Actualizar el tamaño en el diccionario
                if (tamañoSeleccionado > 0 && tamañosIcono.ContainsKey(tamañoSeleccionado))
                {
                    tamañosIcono[tamañoSeleccionado]?.Dispose();
                    tamañosIcono[tamañoSeleccionado] = new Bitmap(imagenEditadaIcono);

                    // Actualizar vista previa del tamaño
                    pbTamañoSeleccionado.Image = new Bitmap(imagenEditadaIcono);
                }

                // Guardar el estado original para futuras reversiones
                editorPixeles.GuardarEstadoOriginal();

                logger?.Info($"✓ Cambios de píxeles aplicados al tamaño {tamañoSeleccionado}x{tamañoSeleccionado}");
                MessageBox.Show(
                    $"Cambios aplicados correctamente al tamaño {tamañoSeleccionado}x{tamañoSeleccionado}",
                    "Éxito",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );
            }
            catch (Exception ex)
            {
                logger?.Error($"Error al aplicar cambios de píxeles: {ex.Message}", ex);
                MessageBox.Show($"Error al aplicar cambios:\n{ex.Message}", "Error");
            }
        }

        // ===== SELECCIONAR TAMAÑO =====

        private void LstTamañosIcono_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstTamañosIcono.SelectedIndex == -1) return;

            try
            {
                string seleccion = lstTamañosIcono.SelectedItem.ToString();
                tamañoSeleccionado = int.Parse(seleccion.Split('x')[0]);

                if (tamañosIcono.ContainsKey(tamañoSeleccionado))
                {
                    // Vista previa del tamaño
                    pbTamañoSeleccionado.Image = new Bitmap(tamañosIcono[tamañoSeleccionado]);
                    lblTamañoActual.Text = $"Vista Previa: {tamañoSeleccionado}x{tamañoSeleccionado}";

                    // Actualizar imagen principal con este tamaño
                    imagenOriginalIcono?.Dispose();
                    imagenEditadaIcono?.Dispose();
                    imagenOriginalIcono = new Bitmap(tamañosIcono[tamañoSeleccionado]);
                    imagenEditadaIcono = new Bitmap(imagenOriginalIcono);
                    pbIconoEditor.Image = new Bitmap(imagenEditadaIcono);

                    // ⭐ NUEVO: Cargar en el editor de píxeles
                    editorPixeles.Imagen = new Bitmap(imagenEditadaIcono);

                    // Ajustar zoom según el tamaño
                    AjustarZoomSegunTamaño(tamañoSeleccionado);

                    logger?.Debug($"Tamaño seleccionado: {tamañoSeleccionado}x{tamañoSeleccionado}");
                }
            }
            catch (Exception ex)
            {
                logger?.Error($"Error al seleccionar tamaño: {ex.Message}");
            }
        }


        // ===== HERRAMIENTAS DE EDICIÓN =====

        private void BtnRedimensionar_Click(object sender, EventArgs e)
        {
            if (imagenEditadaIcono == null)
            {
                MessageBox.Show("Carga un icono primero", "Sin icono");
                return;
            }

            using (Form formRedimensionar = new Form())
            {
                formRedimensionar.Text = "Redimensionar Imagen";
                formRedimensionar.Size = new Size(300, 150);
                formRedimensionar.StartPosition = FormStartPosition.CenterParent;
                formRedimensionar.FormBorderStyle = FormBorderStyle.FixedDialog;
                formRedimensionar.MaximizeBox = false;
                formRedimensionar.MinimizeBox = false;

                Label lblAncho = new Label { Text = "Ancho:", Location = new Point(20, 20) };
                NumericUpDown nudAncho = new NumericUpDown
                {
                    Location = new Point(80, 20),
                    Width = 80,
                    Minimum = 16,
                    Maximum = 512,
                    Value = imagenEditadaIcono.Width
                };

                Label lblAlto = new Label { Text = "Alto:", Location = new Point(20, 50) };
                NumericUpDown nudAlto = new NumericUpDown
                {
                    Location = new Point(80, 50),
                    Width = 80,
                    Minimum = 16,
                    Maximum = 512,
                    Value = imagenEditadaIcono.Height
                };

                Button btnOk = new Button
                {
                    Text = "Aceptar",
                    Location = new Point(50, 85),
                    DialogResult = DialogResult.OK
                };

                Button btnCancelar = new Button
                {
                    Text = "Cancelar",
                    Location = new Point(150, 85),
                    DialogResult = DialogResult.Cancel
                };

                formRedimensionar.Controls.AddRange(new Control[]
                {
            lblAncho, nudAncho, lblAlto, nudAlto, btnOk, btnCancelar
                });

                if (formRedimensionar.ShowDialog() == DialogResult.OK)
                {
                    int nuevoAncho = (int)nudAncho.Value;
                    int nuevoAlto = (int)nudAlto.Value;

                    Bitmap redimensionada = new Bitmap(nuevoAncho, nuevoAlto);
                    using (Graphics g = Graphics.FromImage(redimensionada))
                    {
                        g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                        g.DrawImage(imagenEditadaIcono, 0, 0, nuevoAncho, nuevoAlto);
                    }

                    imagenEditadaIcono.Dispose();
                    imagenEditadaIcono = redimensionada;
                    pbIconoEditor.Image = new Bitmap(imagenEditadaIcono);

                    logger?.Info($"Imagen redimensionada a {nuevoAncho}x{nuevoAlto}");
                }
            }
        }

        private void BtnRotar_Click(object sender, EventArgs e)
        {
            if (imagenEditadaIcono == null) return;

            imagenEditadaIcono.RotateFlip(RotateFlipType.Rotate90FlipNone);
            pbIconoEditor.Image = new Bitmap(imagenEditadaIcono);
            logger?.Info("Imagen rotada 90°");
        }

        private void BtnEspejo_Click(object sender, EventArgs e)
        {
            if (imagenEditadaIcono == null) return;

            imagenEditadaIcono.RotateFlip(RotateFlipType.RotateNoneFlipX);
            pbIconoEditor.Image = new Bitmap(imagenEditadaIcono);
            logger?.Info("Espejo horizontal aplicado");
        }

        private void BtnBrillo_Click(object sender, EventArgs e)
        {
            herramientaActiva = "brillo";
            trackBarAjuste.Value = 0;
            lblValorAjuste.Text = "Brillo: 0";
        }

        private void BtnContraste_Click(object sender, EventArgs e)
        {
            herramientaActiva = "contraste";
            trackBarAjuste.Value = 0;
            lblValorAjuste.Text = "Contraste: 0";
        }

        private void BtnEscalaGrises_Click(object sender, EventArgs e)
        {
            if (imagenEditadaIcono == null) return;

            Bitmap escalGrises = new Bitmap(imagenEditadaIcono.Width, imagenEditadaIcono.Height);

            for (int y = 0; y < imagenEditadaIcono.Height; y++)
            {
                for (int x = 0; x < imagenEditadaIcono.Width; x++)
                {
                    Color pixel = ((Bitmap)imagenEditadaIcono).GetPixel(x, y);
                    int gris = (int)(pixel.R * 0.3 + pixel.G * 0.59 + pixel.B * 0.11);
                    Color nuevoColor = Color.FromArgb(pixel.A, gris, gris, gris);
                    escalGrises.SetPixel(x, y, nuevoColor);
                }
            }

            imagenEditadaIcono.Dispose();
            imagenEditadaIcono = escalGrises;
            pbIconoEditor.Image = new Bitmap(imagenEditadaIcono);
            logger?.Info("Escala de grises aplicada");
        }

        private void TrackBarAjuste_ValueChanged(object sender, EventArgs e)
        {
            int valor = trackBarAjuste.Value;

            if (herramientaActiva == "brillo")
            {
                lblValorAjuste.Text = $"Brillo: {valor}";
            }
            else if (herramientaActiva == "contraste")
            {
                lblValorAjuste.Text = $"Contraste: {valor}";
            }
        }

        private void BtnAplicarCambios_Click(object sender, EventArgs e)
        {
            if (imagenEditadaIcono == null || imagenOriginalIcono == null) return;

            try
            {
                int valor = trackBarAjuste.Value;

                if (herramientaActiva == "brillo")
                {
                    AplicarBrillo(valor);
                }
                else if (herramientaActiva == "contraste")
                {
                    AplicarContraste(valor);
                }

                logger?.Info($"Cambios aplicados: {herramientaActiva} = {valor}");
            }
            catch (Exception ex)
            {
                logger?.Error($"Error al aplicar cambios: {ex.Message}");
            }
        }

        private void AplicarBrillo(int valor)
        {
            Bitmap resultado = new Bitmap(imagenOriginalIcono.Width, imagenOriginalIcono.Height);

            for (int y = 0; y < imagenOriginalIcono.Height; y++)
            {
                for (int x = 0; x < imagenOriginalIcono.Width; x++)
                {
                    Color pixel = ((Bitmap)imagenOriginalIcono).GetPixel(x, y);

                    int r = Math.Max(0, Math.Min(255, pixel.R + valor));
                    int g = Math.Max(0, Math.Min(255, pixel.G + valor));
                    int b = Math.Max(0, Math.Min(255, pixel.B + valor));

                    resultado.SetPixel(x, y, Color.FromArgb(pixel.A, r, g, b));
                }
            }

            imagenEditadaIcono.Dispose();
            imagenEditadaIcono = resultado;
            pbIconoEditor.Image = new Bitmap(imagenEditadaIcono);
        }

        private void AplicarContraste(int valor)
        {
            float factor = (259f * (valor + 255f)) / (255f * (259f - valor));

            Bitmap resultado = new Bitmap(imagenOriginalIcono.Width, imagenOriginalIcono.Height);

            for (int y = 0; y < imagenOriginalIcono.Height; y++)
            {
                for (int x = 0; x < imagenOriginalIcono.Width; x++)
                {
                    Color pixel = ((Bitmap)imagenOriginalIcono).GetPixel(x, y);

                    int r = (int)Math.Max(0, Math.Min(255, factor * (pixel.R - 128) + 128));
                    int g = (int)Math.Max(0, Math.Min(255, factor * (pixel.G - 128) + 128));
                    int b = (int)Math.Max(0, Math.Min(255, factor * (pixel.B - 128) + 128));

                    resultado.SetPixel(x, y, Color.FromArgb(pixel.A, r, g, b));
                }
            }

            imagenEditadaIcono.Dispose();
            imagenEditadaIcono = resultado;
            pbIconoEditor.Image = new Bitmap(imagenEditadaIcono);
        }

        // ===== GUARDAR Y EXPORTAR =====

        private void BtnGuardarIcono_Click(object sender, EventArgs e)
        {
            if (imagenEditadaIcono == null)
            {
                MessageBox.Show("No hay cambios para guardar", "Sin cambios");
                return;
            }

            // ⭐ NUEVO: Aplicar cambios pendientes del editor de píxeles
            Bitmap imagenPixeles = editorPixeles.ObtenerImagen();
            if (imagenPixeles != null && tamañoSeleccionado > 0)
            {
                tamañosIcono[tamañoSeleccionado]?.Dispose();
                tamañosIcono[tamañoSeleccionado] = new Bitmap(imagenPixeles);
            }

            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.Filter = "Icono|*.ico";
                sfd.Title = "Guardar icono editado";
                sfd.FileName = Path.GetFileNameWithoutExtension(iconoActualPath) + "_editado.ico";

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        // Guardar icono con todos los tamaños
                        GuardarIconoMultitamaño(sfd.FileName);

                        logger?.Info($"✓ Icono guardado: {sfd.FileName}");
                        MessageBox.Show("Icono guardado exitosamente", "Éxito");
                    }
                    catch (Exception ex)
                    {
                        logger?.Error($"Error al guardar icono: {ex.Message}", ex);
                        MessageBox.Show($"Error al guardar:\n{ex.Message}", "Error");
                    }
                }
            }
        }

        private void GuardarIconoMultitamaño(string ruta)
        {
            using (FileStream fs = new FileStream(ruta, FileMode.Create))
            using (BinaryWriter bw = new BinaryWriter(fs))
            {
                // Header ICO
                bw.Write((short)0); // Reserved
                bw.Write((short)1); // Type
                bw.Write((short)tamañosIcono.Count); // Count

                // Calcular offset
                long offset = 6 + (16 * tamañosIcono.Count);

                // Guardar imágenes en memoria
                var imagenesData = new Dictionary<int, byte[]>();

                foreach (var kvp in tamañosIcono.OrderByDescending(k => k.Key))
                {
                    using (MemoryStream ms = new MemoryStream())
                    {
                        kvp.Value.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                        imagenesData[kvp.Key] = ms.ToArray();
                    }
                }

                // Escribir entradas del directorio
                foreach (var kvp in imagenesData.OrderByDescending(k => k.Key))
                {
                    int tamaño = kvp.Key;
                    byte[] data = kvp.Value;

                    byte ancho = (byte)(tamaño == 256 ? 0 : tamaño);
                    byte alto = (byte)(tamaño == 256 ? 0 : tamaño);

                    bw.Write(ancho);
                    bw.Write(alto);
                    bw.Write((byte)0); // Color count
                    bw.Write((byte)0); // Reserved
                    bw.Write((short)1); // Color planes
                    bw.Write((short)32); // Bits per pixel
                    bw.Write(data.Length); // Size
                    bw.Write((int)offset); // Offset

                    offset += data.Length;
                }

                // Escribir datos de imágenes
                foreach (var data in imagenesData.Values)
                {
                    bw.Write(data);
                }
            }
        }

        private void BtnExportarTamaño_Click(object sender, EventArgs e)
        {
            if (tamañoSeleccionado == 0 || !tamañosIcono.ContainsKey(tamañoSeleccionado))
            {
                MessageBox.Show("Selecciona un tamaño primero", "Sin tamaño");
                return;
            }

            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.Filter = "PNG|*.png|BMP|*.bmp|JPG|*.jpg";
                sfd.Title = "Exportar tamaño como imagen";
                sfd.FileName = $"icono_{tamañoSeleccionado}x{tamañoSeleccionado}.png";

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        var formato = Path.GetExtension(sfd.FileName).ToLower() switch
                        {
                            ".png" => System.Drawing.Imaging.ImageFormat.Png,
                            ".bmp" => System.Drawing.Imaging.ImageFormat.Bmp,
                            ".jpg" => System.Drawing.Imaging.ImageFormat.Jpeg,
                            _ => System.Drawing.Imaging.ImageFormat.Png
                        };

                        tamañosIcono[tamañoSeleccionado].Save(sfd.FileName, formato);

                        logger?.Info($"✓ Tamaño exportado: {sfd.FileName}");
                        MessageBox.Show("Imagen exportada exitosamente", "Éxito");
                    }
                    catch (Exception ex)
                    {
                        logger?.Error($"Error al exportar: {ex.Message}", ex);
                        MessageBox.Show($"Error al exportar:\n{ex.Message}", "Error");
                    }
                }
            }
        }

        private void BtnRevertir_Click(object sender, EventArgs e)
        {
            if (imagenOriginalIcono == null) return;

            var resultado = MessageBox.Show(
                "¿Deseas revertir todos los cambios?",
                "Revertir cambios",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (resultado == DialogResult.Yes)
            {
                imagenEditadaIcono?.Dispose();
                imagenEditadaIcono = new Bitmap(imagenOriginalIcono);
                pbIconoEditor.Image = new Bitmap(imagenEditadaIcono);

                trackBarAjuste.Value = 0;
                herramientaActiva = "";

                logger?.Info("Cambios revertidos");
            }
        }
        #endregion
        // =================================================================
    }
}
