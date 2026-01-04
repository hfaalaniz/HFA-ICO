using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.ComponentModel;

namespace HFA_ICO
{
    /// <summary>
    /// Control personalizado para edición pixel por pixel con zoom
    /// </summary>
    public class EditorPixeles : Control
    {
        // Propiedades
        private Bitmap imagenOriginal;
        private Bitmap imagenTrabajo;
        private int nivelZoom = 10; // 10x zoom por defecto
        private Point pixelSeleccionado = new Point(-1, -1);
        private Color colorActual = Color.Black;
        private bool dibujando = false;
        private HerramientaDibujo herramientaActual = HerramientaDibujo.Pincel;

        // Configuración de cuadrícula
        private bool mostrarCuadricula = true;
        private Color colorCuadricula = Color.LightGray;
        private Color colorBorde = Color.Black;

        // Eventos
        public event EventHandler ImagenModificada;
        public event EventHandler<PixelSeleccionadoEventArgs> PixelSeleccionado;

        // Enumeración de herramientas
        public enum HerramientaDibujo
        {
            Pincel,
            Borrador,
            Cuentagotas,
            Relleno,
            VaritaMagica  // ⭐ NUEVA HERRAMIENTA
        }

        // Configuración de varita mágica
        private int toleranciaColor = 30; // Tolerancia de similitud de color (0-255)

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public int ToleranciaVaritaMagica
        {
            get => toleranciaColor;
            set => toleranciaColor = Math.Max(0, Math.Min(255, value));
        }

        // Propiedades públicas
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public Bitmap Imagen
        {
            get => imagenTrabajo;
            set
            {
                if (value != null)
                {
                    imagenOriginal?.Dispose();
                    imagenTrabajo?.Dispose();

                    imagenOriginal = new Bitmap(value);
                    imagenTrabajo = new Bitmap(value);

                    AjustarTamañoControl();
                    Invalidate();
                }
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public int Zoom
        {
            get => nivelZoom;
            set
            {
                if (value >= 1 && value <= 50)
                {
                    nivelZoom = value;
                    AjustarTamañoControl();
                    Invalidate();
                }
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public Color ColorDibujo
        {
            get => colorActual;
            set => colorActual = value;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public HerramientaDibujo Herramienta
        {
            get => herramientaActual;
            set
            {
                herramientaActual = value;
                ActualizarCursor(); // ⭐ NUEVO
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public bool MostrarCuadricula
        {
            get => mostrarCuadricula;
            set
            {
                mostrarCuadricula = value;
                Invalidate();
            }
        }

        // Constructor
        public EditorPixeles()
        {
            DoubleBuffered = true;
            ResizeRedraw = true;

            // Eventos del mouse
            MouseDown += EditorPixeles_MouseDown;
            MouseMove += EditorPixeles_MouseMove;
            MouseUp += EditorPixeles_MouseUp;
            MouseLeave += EditorPixeles_MouseLeave;
        }

        // Métodos privados
        private void EliminarFondoMagico(Point inicio)
        {
            if (imagenTrabajo == null) return;

            Color colorObjetivo = ObtenerColorPixel(inicio);

            // Si el píxel ya es transparente, no hacer nada
            if (colorObjetivo.A < 10)
            {
                System.Windows.Forms.MessageBox.Show(
                    "El píxel seleccionado ya es transparente. Selecciona un píxel de color.",
                    "Varita Mágica",
                    System.Windows.Forms.MessageBoxButtons.OK,
                    System.Windows.Forms.MessageBoxIcon.Information
                );
                return;
            }

            // Crear una copia temporal para trabajar
            Bitmap imagenTemp = new Bitmap(imagenTrabajo);

            // Algoritmo de flood fill optimizado con 8 direcciones
            var cola = new System.Collections.Generic.Queue<Point>();
            var visitados = new bool[imagenTrabajo.Width, imagenTrabajo.Height];
            int pixelesModificados = 0;

            cola.Enqueue(inicio);
            visitados[inicio.X, inicio.Y] = true;

            // 8 direcciones (incluye diagonales para mejor precisión)
            Point[] direcciones = new Point[]
            {
        new Point(-1, -1), new Point(0, -1), new Point(1, -1),  // Arriba
        new Point(-1,  0),                    new Point(1,  0),  // Lados
        new Point(-1,  1), new Point(0,  1), new Point(1,  1)   // Abajo
            };

            while (cola.Count > 0)
            {
                Point p = cola.Dequeue();

                Color colorActualPixel = imagenTemp.GetPixel(p.X, p.Y);

                // Verificar si el color es similar
                if (ColoresSimilares(colorActualPixel, colorObjetivo, toleranciaColor))
                {
                    // Hacer el píxel transparente
                    imagenTrabajo.SetPixel(p.X, p.Y, Color.Transparent);
                    pixelesModificados++;

                    // Explorar píxeles adyacentes (8 direcciones)
                    foreach (Point dir in direcciones)
                    {
                        int nx = p.X + dir.X;
                        int ny = p.Y + dir.Y;

                        // Verificar límites
                        if (nx >= 0 && nx < imagenTrabajo.Width &&
                            ny >= 0 && ny < imagenTrabajo.Height &&
                            !visitados[nx, ny])
                        {
                            visitados[nx, ny] = true;
                            cola.Enqueue(new Point(nx, ny));
                        }
                    }
                }
            }

            imagenTemp.Dispose();

            // Notificar resultados
            if (pixelesModificados > 0)
            {
                ImagenModificada?.Invoke(this, EventArgs.Empty);
                Invalidate();

                // Log opcional del resultado
                System.Diagnostics.Debug.WriteLine(
                    $"Varita Mágica: {pixelesModificados} píxeles eliminados con tolerancia {toleranciaColor}"
                );
            }
            else
            {
                System.Windows.Forms.MessageBox.Show(
                    $"No se encontraron píxeles similares con la tolerancia actual ({toleranciaColor}).\n\n" +
                    "Intenta aumentar la tolerancia para seleccionar más píxeles.",
                    "Varita Mágica",
                    System.Windows.Forms.MessageBoxButtons.OK,
                    System.Windows.Forms.MessageBoxIcon.Information
                );
            }
        }

        // Agregar después del constructor
        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);
            ActualizarCursor();
        }

        private void ActualizarCursor()
        {
            if (herramientaActual == HerramientaDibujo.VaritaMagica)
            {
                // Cursor personalizado para varita mágica
                this.Cursor = Cursors.Cross;
            }
            else
            {
                this.Cursor = Cursors.Default;
            }
        }

        private bool ColoresSimilares(Color c1, Color c2, int tolerancia)
        {
            // Ignorar píxeles ya transparentes
            if (c1.A < 10) return false;

            // Calcular distancia euclidiana en el espacio RGB
            // Esta es una forma más precisa de medir la similitud de colores
            double diffR = c1.R - c2.R;
            double diffG = c1.G - c2.G;
            double diffB = c1.B - c2.B;

            double distancia = Math.Sqrt(diffR * diffR + diffG * diffG + diffB * diffB);

            // La distancia máxima en RGB es √(255² + 255² + 255²) ≈ 441
            // Convertir tolerancia (0-100) a escala de distancia (0-441)
            double umbral = (tolerancia / 100.0) * 441.0;

            return distancia <= umbral;
        }

        private void AjustarTamañoControl()
        {
            if (imagenTrabajo != null)
            {
                Width = imagenTrabajo.Width * nivelZoom + 1;
                Height = imagenTrabajo.Height * nivelZoom + 1;
            }
        }

        private Point ObtenerPixelDesdeCoordenadas(Point posicionMouse)
        {
            if (imagenTrabajo == null) return new Point(-1, -1);

            int x = posicionMouse.X / nivelZoom;
            int y = posicionMouse.Y / nivelZoom;

            if (x >= 0 && x < imagenTrabajo.Width && y >= 0 && y < imagenTrabajo.Height)
            {
                return new Point(x, y);
            }

            return new Point(-1, -1);
        }

        private void DibujarPixel(Point pixel, Color color)
        {
            if (pixel.X >= 0 && pixel.X < imagenTrabajo.Width &&
                pixel.Y >= 0 && pixel.Y < imagenTrabajo.Height)
            {
                imagenTrabajo.SetPixel(pixel.X, pixel.Y, color);
                ImagenModificada?.Invoke(this, EventArgs.Empty);

                // Redibujar solo el área afectada
                Rectangle areaAfectada = new Rectangle(
                    pixel.X * nivelZoom,
                    pixel.Y * nivelZoom,
                    nivelZoom + 1,
                    nivelZoom + 1
                );
                Invalidate(areaAfectada);
            }
        }

        private Color ObtenerColorPixel(Point pixel)
        {
            if (pixel.X >= 0 && pixel.X < imagenTrabajo.Width &&
                pixel.Y >= 0 && pixel.Y < imagenTrabajo.Height)
            {
                return imagenTrabajo.GetPixel(pixel.X, pixel.Y);
            }
            return Color.Transparent;
        }

        private void RellenarArea(Point inicio, Color colorNuevo)
        {
            Color colorOriginal = ObtenerColorPixel(inicio);

            if (colorOriginal.ToArgb() == colorNuevo.ToArgb()) return;

            // Algoritmo de relleno por inundación (flood fill)
            var pila = new System.Collections.Generic.Stack<Point>();
            pila.Push(inicio);

            while (pila.Count > 0)
            {
                Point p = pila.Pop();

                if (p.X < 0 || p.X >= imagenTrabajo.Width ||
                    p.Y < 0 || p.Y >= imagenTrabajo.Height)
                    continue;

                Color colorActualPixel = imagenTrabajo.GetPixel(p.X, p.Y);

                if (colorActualPixel.ToArgb() != colorOriginal.ToArgb())
                    continue;

                imagenTrabajo.SetPixel(p.X, p.Y, colorNuevo);

                // Agregar píxeles adyacentes
                pila.Push(new Point(p.X + 1, p.Y));
                pila.Push(new Point(p.X - 1, p.Y));
                pila.Push(new Point(p.X, p.Y + 1));
                pila.Push(new Point(p.X, p.Y - 1));
            }

            ImagenModificada?.Invoke(this, EventArgs.Empty);
            Invalidate();
        }

        // Eventos del mouse
        private void EditorPixeles_MouseDown(object sender, MouseEventArgs e)
        {
            Point pixel = ObtenerPixelDesdeCoordenadas(e.Location);

            if (pixel.X == -1) return;

            pixelSeleccionado = pixel;

            // Solo activar dibujando para herramientas continuas
            if (herramientaActual == HerramientaDibujo.Pincel ||
                herramientaActual == HerramientaDibujo.Borrador)
            {
                dibujando = true;
            }

            // Notificar selección de píxel
            PixelSeleccionado?.Invoke(this, new PixelSeleccionadoEventArgs(pixel, ObtenerColorPixel(pixel)));

            switch (herramientaActual)
            {
                case HerramientaDibujo.Pincel:
                    DibujarPixel(pixel, colorActual);
                    break;

                case HerramientaDibujo.Borrador:
                    DibujarPixel(pixel, Color.Transparent);
                    break;

                case HerramientaDibujo.Cuentagotas:
                    colorActual = ObtenerColorPixel(pixel);
                    break;

                case HerramientaDibujo.Relleno:
                    RellenarArea(pixel, colorActual);
                    break;

                case HerramientaDibujo.VaritaMagica:
                    // Cambiar cursor temporalmente
                    Cursor cursorAnterior = this.Cursor;
                    this.Cursor = Cursors.WaitCursor;

                    // Ejecutar eliminación
                    EliminarFondoMagico(pixel);

                    // Restaurar cursor
                    this.Cursor = cursorAnterior;
                    break;
            }
        }

        /// <summary>
        /// Previsualiza qué píxeles serán eliminados sin aplicar cambios
        /// </summary>
        public int ContarPixelesAfectados(Point inicio)
        {
            if (imagenTrabajo == null) return 0;

            Color colorObjetivo = ObtenerColorPixel(inicio);

            if (colorObjetivo.A < 10) return 0;

            var cola = new System.Collections.Generic.Queue<Point>();
            var visitados = new bool[imagenTrabajo.Width, imagenTrabajo.Height];
            int contador = 0;

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
                Color colorActualPixel = imagenTrabajo.GetPixel(p.X, p.Y);

                if (ColoresSimilares(colorActualPixel, colorObjetivo, toleranciaColor))
                {
                    contador++;

                    foreach (Point dir in direcciones)
                    {
                        int nx = p.X + dir.X;
                        int ny = p.Y + dir.Y;

                        if (nx >= 0 && nx < imagenTrabajo.Width &&
                            ny >= 0 && ny < imagenTrabajo.Height &&
                            !visitados[nx, ny])
                        {
                            visitados[nx, ny] = true;
                            cola.Enqueue(new Point(nx, ny));
                        }
                    }
                }
            }

            return contador;
        }

        private void EditorPixeles_MouseMove(object sender, MouseEventArgs e)
        {
            Point pixel = ObtenerPixelDesdeCoordenadas(e.Location);

            if (pixel.X == -1)
            {
                pixelSeleccionado = new Point(-1, -1);
                Invalidate();
                return;
            }

            // Actualizar píxel resaltado
            if (pixelSeleccionado != pixel)
            {
                Point anterior = pixelSeleccionado;
                pixelSeleccionado = pixel;

                // Redibujar píxel anterior y nuevo
                if (anterior.X >= 0)
                {
                    Rectangle areaAnterior = new Rectangle(
                        anterior.X * nivelZoom,
                        anterior.Y * nivelZoom,
                        nivelZoom + 1,
                        nivelZoom + 1
                    );
                    Invalidate(areaAnterior);
                }

                Rectangle areaNueva = new Rectangle(
                    pixel.X * nivelZoom,
                    pixel.Y * nivelZoom,
                    nivelZoom + 1,
                    nivelZoom + 1
                );
                Invalidate(areaNueva);

                // Notificar cambio de píxel
                Color colorPixel = ObtenerColorPixel(pixel);
                PixelSeleccionado?.Invoke(this, new PixelSeleccionadoEventArgs(pixel, colorPixel));

                // ⭐ NUEVO: Mostrar preview para varita mágica
                if (herramientaActual == HerramientaDibujo.VaritaMagica && colorPixel.A >= 10)
                {
                    // Opcional: Mostrar tooltip con cantidad de píxeles que serán afectados
                    // int pixelesAfectados = ContarPixelesAfectados(pixel);
                    // this.ToolTipText = $"Eliminará ~{pixelesAfectados} píxeles";
                }
            }

            // Dibujar si está presionado el botón (solo para pincel y borrador)
            if (dibujando && (herramientaActual == HerramientaDibujo.Pincel ||
                              herramientaActual == HerramientaDibujo.Borrador))
            {
                Color color = herramientaActual == HerramientaDibujo.Pincel ?
                              colorActual : Color.Transparent;
                DibujarPixel(pixel, color);
            }
        }

        private void EditorPixeles_MouseUp(object sender, MouseEventArgs e)
        {
            dibujando = false;
        }

        private void EditorPixeles_MouseLeave(object sender, EventArgs e)
        {
            dibujando = false;
            pixelSeleccionado = new Point(-1, -1);
            Invalidate();
        }

        // Renderizado
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            if (imagenTrabajo == null) return;

            Graphics g = e.Graphics;
            g.InterpolationMode = InterpolationMode.NearestNeighbor;
            g.PixelOffsetMode = PixelOffsetMode.Half;

            // Dibujar fondo de tablero de ajedrez para transparencia
            DibujarFondoTransparencia(g);

            // Dibujar la imagen con zoom
            for (int y = 0; y < imagenTrabajo.Height; y++)
            {
                for (int x = 0; x < imagenTrabajo.Width; x++)
                {
                    Color colorPixel = imagenTrabajo.GetPixel(x, y);

                    Rectangle rectPixel = new Rectangle(
                        x * nivelZoom,
                        y * nivelZoom,
                        nivelZoom,
                        nivelZoom
                    );

                    // Dibujar píxel
                    if (colorPixel.A > 0)
                    {
                        using (SolidBrush brush = new SolidBrush(colorPixel))
                        {
                            g.FillRectangle(brush, rectPixel);
                        }
                    }
                }
            }

            // Dibujar cuadrícula
            if (mostrarCuadricula && nivelZoom >= 4)
            {
                using (Pen penCuadricula = new Pen(colorCuadricula))
                {
                    // Líneas verticales
                    for (int x = 0; x <= imagenTrabajo.Width; x++)
                    {
                        g.DrawLine(penCuadricula,
                            x * nivelZoom, 0,
                            x * nivelZoom, imagenTrabajo.Height * nivelZoom);
                    }

                    // Líneas horizontales
                    for (int y = 0; y <= imagenTrabajo.Height; y++)
                    {
                        g.DrawLine(penCuadricula,
                            0, y * nivelZoom,
                            imagenTrabajo.Width * nivelZoom, y * nivelZoom);
                    }
                }
            }

            // Resaltar píxel seleccionado
            if (pixelSeleccionado.X >= 0 && pixelSeleccionado.Y >= 0)
            {
                Rectangle rectSeleccion = new Rectangle(
                    pixelSeleccionado.X * nivelZoom,
                    pixelSeleccionado.Y * nivelZoom,
                    nivelZoom,
                    nivelZoom
                );

                using (Pen penSeleccion = new Pen(Color.Yellow, 2))
                {
                    g.DrawRectangle(penSeleccion, rectSeleccion);
                }
            }

            // Borde exterior
            using (Pen penBorde = new Pen(colorBorde))
            {
                g.DrawRectangle(penBorde, 0, 0, Width - 1, Height - 1);
            }
        }

        private void DibujarFondoTransparencia(Graphics g)
        {
            // Patrón de tablero de ajedrez para mostrar transparencia
            int tamañoCuadro = Math.Max(1, nivelZoom / 2);

            using (SolidBrush brushClaro = new SolidBrush(Color.White))
            using (SolidBrush brushOscuro = new SolidBrush(Color.LightGray))
            {
                for (int y = 0; y < imagenTrabajo.Height; y++)
                {
                    for (int x = 0; x < imagenTrabajo.Width; x++)
                    {
                        bool esClaroX = (x / tamañoCuadro) % 2 == 0;
                        bool esClaroY = (y / tamañoCuadro) % 2 == 0;
                        bool esClaro = esClaroX == esClaroY;

                        Rectangle rect = new Rectangle(
                            x * nivelZoom,
                            y * nivelZoom,
                            nivelZoom,
                            nivelZoom
                        );

                        g.FillRectangle(esClaro ? brushClaro : brushOscuro, rect);
                    }
                }
            }
        }

        // Métodos públicos
        public void Deshacer()
        {
            if (imagenOriginal != null)
            {
                imagenTrabajo?.Dispose();
                imagenTrabajo = new Bitmap(imagenOriginal);
                Invalidate();
            }
        }

        public void Limpiar()
        {
            if (imagenTrabajo != null)
            {
                using (Graphics g = Graphics.FromImage(imagenTrabajo))
                {
                    g.Clear(Color.Transparent);
                }
                ImagenModificada?.Invoke(this, EventArgs.Empty);
                Invalidate();
            }
        }

        public Bitmap ObtenerImagen()
        {
            return imagenTrabajo != null ? new Bitmap(imagenTrabajo) : null;
        }

        public void GuardarEstadoOriginal()
        {
            if (imagenTrabajo != null)
            {
                imagenOriginal?.Dispose();
                imagenOriginal = new Bitmap(imagenTrabajo);
            }
        }

        /// <summary>
        /// Elimina automáticamente el fondo detectando las esquinas de la imagen
        /// </summary>
        public void EliminarFondoAutomatico()
        {
            if (imagenTrabajo == null) return;

            // Detectar color de fondo desde las 4 esquinas
            Color[] coloresEsquinas = new Color[]
            {
                imagenTrabajo.GetPixel(0, 0),  // Superior izquierda
                imagenTrabajo.GetPixel(imagenTrabajo.Width - 1, 0),  // Superior derecha
                imagenTrabajo.GetPixel(0, imagenTrabajo.Height - 1),  // Inferior izquierda
                imagenTrabajo.GetPixel(imagenTrabajo.Width - 1, imagenTrabajo.Height - 1)  // Inferior derecha
            };

            // Encontrar el color más común en las esquinas
            Color colorFondo = coloresEsquinas[0];

            // Eliminar desde las 4 esquinas
            EliminarFondoMagico(new Point(0, 0));
            EliminarFondoMagico(new Point(imagenTrabajo.Width - 1, 0));
            EliminarFondoMagico(new Point(0, imagenTrabajo.Height - 1));
            EliminarFondoMagico(new Point(imagenTrabajo.Width - 1, imagenTrabajo.Height - 1));
        }

        // Cleanup
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                imagenOriginal?.Dispose();
                imagenTrabajo?.Dispose();
            }
            base.Dispose(disposing);
        }
    }

    // Clase para el evento de píxel seleccionado
    public class PixelSeleccionadoEventArgs : EventArgs
    {
        public Point Posicion { get; }
        public Color Color { get; }

        public PixelSeleccionadoEventArgs(Point posicion, Color color)
        {
            Posicion = posicion;
            Color = color;
        }
    }
}