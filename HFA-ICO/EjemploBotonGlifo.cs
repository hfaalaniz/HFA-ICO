
using HFA_ICO;
using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HFA_ICO
{
    public partial class EjemploBotonGlifo : Form
    {
        private BotonGlifo btnSincronizar;
        private BotonGlifo btnDescargar;
        private BotonGlifo btnProcesar;
        private BotonGlifo btnGuardar;
        private BotonGlifo btnEliminar;
        private BotonGlifo btnBuscar;

        public EjemploBotonGlifo()
        {
            //InitializeComponent();
            InitializeBotones();
        }

        private void InitializeBotones()
        {
            int x = 20;
            int y = 20;
            int spacing = 130;

            // ===== BOTÓN 1: Sincronizar con progreso determinado =====
            btnSincronizar = new BotonGlifo
            {
                Location = new Point(x, y),
                Size = new Size(120, 120),
                Glyph = "\\uF021", // Icono de Sync
                TopRightText = "F1",
                BottomText = "Sincronizar",
                BackColorNormal = Color.FromArgb(45, 137, 239),
                BackColorHover = Color.FromArgb(58, 150, 245),
                BackColorPressed = Color.FromArgb(32, 120, 220),
                BorderRadius = 12,
                ShowShadow = true,
                ProgressBarHeight = 3
            };
            btnSincronizar.Click += BtnSincronizar_Click;
            this.Controls.Add(btnSincronizar);

            // ===== BOTÓN 2: Descargar con progreso indeterminado =====
            btnDescargar = new BotonGlifo
            {
                Location = new Point(x + spacing, y),
                Size = new Size(120, 120),
                Glyph = "\\uF019", // Icono de Download
                TopRightText = "F2",
                BottomText = "Descargar",
                BackColorNormal = Color.FromArgb(76, 175, 80),
                BackColorHover = Color.FromArgb(92, 191, 96),
                BackColorPressed = Color.FromArgb(60, 159, 64),
                BorderRadius = 12,
                ShowShadow = true
            };
            btnDescargar.Click += BtnDescargar_Click;
            this.Controls.Add(btnDescargar);

            // ===== BOTÓN 3: Procesar con progreso paso a paso =====
            btnProcesar = new BotonGlifo
            {
                Location = new Point(x + spacing * 2, y),
                Size = new Size(120, 120),
                Glyph = "\\uF013", // Icono de Settings/Gear
                TopRightText = "F3",
                BottomText = "Procesar",
                BackColorNormal = Color.FromArgb(255, 152, 0),
                BackColorHover = Color.FromArgb(255, 167, 38),
                BackColorPressed = Color.FromArgb(230, 137, 0),
                BorderRadius = 12,
                ShowShadow = true,
                ProgressColor = Color.FromArgb(255, 193, 7)
            };
            btnProcesar.Click += BtnProcesar_Click;
            this.Controls.Add(btnProcesar);

            // ===== BOTÓN 4: Guardar rápido =====
            btnGuardar = new BotonGlifo
            {
                Location = new Point(x, y + spacing),
                Size = new Size(120, 120),
                Glyph = "\\uF00C", // Icono de Check
                TopRightText = "F4",
                BottomText = "Guardar",
                BackColorNormal = Color.FromArgb(103, 58, 183),
                BackColorHover = Color.FromArgb(119, 74, 199),
                BackColorPressed = Color.FromArgb(87, 42, 167),
                BorderRadius = 12,
                ShowShadow = true,
                ProgressColor = Color.FromArgb(156, 39, 176)
            };
            btnGuardar.Click += BtnGuardar_Click;
            this.Controls.Add(btnGuardar);

            // ===== BOTÓN 5: Eliminar con confirmación =====
            btnEliminar = new BotonGlifo
            {
                Location = new Point(x + spacing, y + spacing),
                Size = new Size(120, 120),
                Glyph = "\\uF2ED", // Icono de Trash
                TopRightText = "F5",
                BottomText = "Eliminar",
                BackColorNormal = Color.FromArgb(244, 67, 54),
                BackColorHover = Color.FromArgb(255, 87, 74),
                BackColorPressed = Color.FromArgb(220, 51, 38),
                BorderRadius = 12,
                ShowShadow = true,
                ProgressColor = Color.FromArgb(229, 57, 53)
            };
            btnEliminar.Click += BtnEliminar_Click;
            this.Controls.Add(btnEliminar);

            // ===== BOTÓN 6: Buscar con animación =====
            btnBuscar = new BotonGlifo
            {
                Location = new Point(x + spacing * 2, y + spacing),
                Size = new Size(120, 120),
                Glyph = "\\uF002", // Icono de Search
                TopRightText = "F6",
                BottomText = "Buscar",
                BackColorNormal = Color.FromArgb(0, 150, 136),
                BackColorHover = Color.FromArgb(0, 166, 152),
                BackColorPressed = Color.FromArgb(0, 134, 120),
                BorderRadius = 12,
                ShowShadow = true,
                ProgressColor = Color.FromArgb(0, 188, 212)
            };
            btnBuscar.Click += BtnBuscar_Click;
            this.Controls.Add(btnBuscar);
        }

        // ===== EJEMPLO 1: Progreso determinado con pasos =====
        private async void BtnSincronizar_Click(object sender, EventArgs e)
        {
            btnSincronizar.Enabled = false;
            btnSincronizar.ShowProgress = true;
            btnSincronizar.ProgressIndeterminate = false;

            try
            {
                // Simular sincronización por pasos
                for (int i = 0; i <= 100; i += 10)
                {
                    btnSincronizar.ProgressValue = i;
                    btnSincronizar.BottomText = $"Sincronizando {i}%";
                    await Task.Delay(300);
                }

                btnSincronizar.BottomText = "¡Completado!";
                await Task.Delay(800);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnSincronizar.StopProgress();
                btnSincronizar.BottomText = "Sincronizar";
                btnSincronizar.Enabled = true;
            }
        }

        // ===== EJEMPLO 2: Progreso indeterminado =====
        private async void BtnDescargar_Click(object sender, EventArgs e)
        {
            btnDescargar.Enabled = false;
            btnDescargar.BottomText = "Descargando...";

            // Iniciar progreso indeterminado (barra animada)
            btnDescargar.StartIndeterminateProgress();

            try
            {
                // Simular descarga
                await Task.Delay(3000);

                // Completar con animación
                btnDescargar.CompleteProgress();
                btnDescargar.BottomText = "¡Descargado!";
                await Task.Delay(1000);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnDescargar.StopProgress();
            }
            finally
            {
                btnDescargar.BottomText = "Descargar";
                btnDescargar.Enabled = true;
            }
        }

        // ===== EJEMPLO 3: Progreso con método SetProgress =====
        private async void BtnProcesar_Click(object sender, EventArgs e)
        {
            btnProcesar.Enabled = false;

            try
            {
                string[] pasos = new[]
                {
                    "Validando...",
                    "Procesando...",
                    "Calculando...",
                    "Guardando...",
                    "Finalizando..."
                };

                for (int i = 0; i < pasos.Length; i++)
                {
                    btnProcesar.BottomText = pasos[i];
                    int progreso = (int)(((i + 1) / (float)pasos.Length) * 100);
                    btnProcesar.SetProgress(progreso, true);
                    await Task.Delay(800);
                }

                btnProcesar.BottomText = "¡Completado!";
                await Task.Delay(800);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnProcesar.StopProgress();
                btnProcesar.BottomText = "Procesar";
                btnProcesar.Enabled = true;
            }
        }

        // ===== EJEMPLO 4: Progreso rápido =====
        private async void BtnGuardar_Click(object sender, EventArgs e)
        {
            btnGuardar.Enabled = false;
            btnGuardar.BottomText = "Guardando...";
            btnGuardar.ShowProgress = true;

            try
            {
                // Progreso rápido
                for (int i = 0; i <= 100; i += 25)
                {
                    btnGuardar.ProgressValue = i;
                    await Task.Delay(100);
                }

                btnGuardar.BottomText = "¡Guardado!";
                await Task.Delay(500);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnGuardar.StopProgress();
                btnGuardar.BottomText = "Guardar";
                btnGuardar.Enabled = true;
            }
        }

        // ===== EJEMPLO 5: Progreso con confirmación =====
        private async void BtnEliminar_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show(
                "¿Está seguro de que desea eliminar?",
                "Confirmar eliminación",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                btnEliminar.Enabled = false;
                btnEliminar.BottomText = "Eliminando...";
                btnEliminar.StartIndeterminateProgress();

                try
                {
                    await Task.Delay(2000);
                    btnEliminar.StopProgress();
                    btnEliminar.BottomText = "Eliminado";
                    await Task.Delay(800);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    btnEliminar.BottomText = "Eliminar";
                    btnEliminar.Enabled = true;
                }
            }
        }

        // ===== EJEMPLO 6: Búsqueda con múltiples resultados =====
        private async void BtnBuscar_Click(object sender, EventArgs e)
        {
            btnBuscar.Enabled = false;
            btnBuscar.BottomText = "Buscando...";
            btnBuscar.StartIndeterminateProgress();

            try
            {
                // Simular búsqueda
                await Task.Delay(1500);

                // Cambiar a progreso determinado al procesar resultados
                btnBuscar.ProgressIndeterminate = false;
                btnBuscar.BottomText = "Procesando...";

                for (int i = 0; i <= 100; i += 20)
                {
                    btnBuscar.ProgressValue = i;
                    await Task.Delay(200);
                }

                btnBuscar.BottomText = "10 resultados";
                await Task.Delay(1000);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnBuscar.StopProgress();
                btnBuscar.BottomText = "Buscar";
                btnBuscar.Enabled = true;
            }

            SimularProcesoMultiple();
        }

        // ===== EJEMPLO ADICIONAL: Uso desde un método externo =====
        public async Task EjecutarProcesoLargo(BotonGlifo boton, string mensaje)
        {
            boton.Enabled = false;
            string textoOriginal = boton.BottomText;

            try
            {
                // Fase 1: Progreso indeterminado
                boton.BottomText = mensaje + "...";
                boton.StartIndeterminateProgress();
                await Task.Delay(2000);

                // Fase 2: Progreso determinado
                boton.ProgressIndeterminate = false;
                for (int i = 0; i <= 100; i += 10)
                {
                    boton.SetProgress(i);
                    boton.BottomText = $"{mensaje} {i}%";
                    await Task.Delay(300);
                }

                boton.BottomText = "¡Completado!";
                await Task.Delay(1000);
            }
            finally
            {
                boton.StopProgress();
                boton.BottomText = textoOriginal;
                boton.Enabled = true;
            }
        }

        // ===== EJEMPLO: Manejo de múltiples botones simultáneos =====
        private async Task SimularProcesoMultiple()
        {
            // Deshabilitar todos los botones
            btnSincronizar.Enabled = false;
            btnDescargar.Enabled = false;
            btnProcesar.Enabled = false;

            // Iniciar progreso en todos
            btnSincronizar.StartIndeterminateProgress();
            btnDescargar.StartIndeterminateProgress();
            btnProcesar.StartIndeterminateProgress();

            try
            {
                // Simular procesos paralelos
                var task1 = Task.Run(async () =>
                {
                    await Task.Delay(2000);
                    btnSincronizar.CompleteProgress();
                });

                var task2 = Task.Run(async () =>
                {
                    await Task.Delay(3000);
                    btnDescargar.CompleteProgress();
                });

                var task3 = Task.Run(async () =>
                {
                    await Task.Delay(2500);
                    btnProcesar.CompleteProgress();
                });

                await Task.WhenAll(task1, task2, task3);
            }
            finally
            {
                btnSincronizar.Enabled = true;
                btnDescargar.Enabled = true;
                btnProcesar.Enabled = true;
            }
        }
    }
}

// ===== EJEMPLO DE USO EN TU MainForm.cs =====
/*
public partial class MainForm : Form
{
    private BotonGlifo btnConvertir;

    private void InitializeComponent()
    {
        btnConvertir = new BotonGlifo
        {
            Location = new Point(20, 400),
            Size = new Size(150, 150),
            Glyph = "\\uF021", // Icono de conversión
            TopRightText = "F9",
            BottomText = "Convertir ICO",
            BackColorNormal = Color.FromArgb(45, 137, 239),
            BackColorHover = Color.FromArgb(58, 150, 245),
            BorderRadius = 15,
            ShowShadow = true,
            ProgressBarHeight = 4,
            ProgressColor = Color.FromArgb(76, 175, 80)
        };
        btnConvertir.Click += BtnConvertir_Click;
        this.Controls.Add(btnConvertir);
    }

    private async void BtnConvertir_Click(object sender, EventArgs e)
    {
        // Validaciones...
        if (clbTamaños.CheckedItems.Count == 0)
        {
            MessageBox.Show("Selecciona al menos un tamaño");
            return;
        }

        using (SaveFileDialog sfd = new SaveFileDialog())
        {
            sfd.Filter = "Icono|*.ico";
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                btnConvertir.Enabled = false;
                btnConvertir.BottomText = "Iniciando...";
                btnConvertir.ShowProgress = true;

                try
                {
                    int[] tamaños = ObtenerTamañosSeleccionados();
                    var converter = new IconConverter(logger);
                    
                    // Configurar callback de progreso
                    int totalPasos = tamaños.Length;
                    int pasoActual = 0;

                    converter.OnProgress += (mensaje, progreso) =>
                    {
                        if (btnConvertir.InvokeRequired)
                        {
                            btnConvertir.Invoke(new Action(() =>
                            {
                                btnConvertir.ProgressValue = progreso;
                                btnConvertir.BottomText = mensaje;
                            }));
                        }
                    };

                    // Realizar conversión
                    await Task.Run(() =>
                    {
                        converter.ConvertirAIconoDesdeImagen(
                            imagenAConvertir,
                            sfd.FileName,
                            tamaños
                        );
                    });

                    // Completar
                    btnConvertir.CompleteProgress();
                    btnConvertir.BottomText = "¡Completado!";
                    await Task.Delay(1000);

                    MessageBox.Show("¡Icono creado exitosamente!");
                }
                catch (Exception ex)
                {
                    btnConvertir.StopProgress();
                    MessageBox.Show($"Error: {ex.Message}");
                }
                finally
                {
                    btnConvertir.BottomText = "Convertir ICO";
                    btnConvertir.Enabled = true;
                }
            }
        }
    }
}
*/