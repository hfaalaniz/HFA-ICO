namespace HFA_ICO
{
    partial class MainForm
    {
        /// <summary>
        /// Variable del diseñador necesaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpiar los recursos que se estén usando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido de este método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            tabControl1 = new TabControl();
            tabIndividual = new TabPage();
            gbHerramientasEdicion = new GroupBox();
            chkVistaPrevia = new CheckBox();
            trackBarToleranciaIndividual = new TrackBar();
            btnEliminarFondo = new Button();
            lblToleranciaIndividual = new Label();
            lblToleranciaIndividal = new Label();
            lblTitulo = new Label();
            btnSeleccionar = new Button();
            lblArchivo = new Label();
            lblDimensiones = new Label();
            pbPreview = new PictureBox();
            lblEstado = new Label();
            chkMantenerProporcion = new CheckBox();
            btnLimpiarSeleccion = new Button();
            pbRecorte = new PictureBox();
            lblRecorte = new Label();
            lblTamaños = new Label();
            clbTamaños = new CheckedListBox();
            btnConvertir = new BotonGlifo();
            progressBar = new ProgressBar();
            txtLog = new TextBox();
            btnLimpiarLog = new Button();
            tabLote = new TabPage();
            lblTituloLote = new Label();
            btnSeleccionarCarpeta = new Button();
            btnAgregarArchivos = new Button();
            lblCarpetaActual = new Label();
            lvArchivosLote = new ListView();
            btnSeleccionarTodos = new Button();
            btnDeseleccionarTodos = new Button();
            btnLimpiarLista = new Button();
            lblCantidadArchivos = new Label();
            lblSeleccionados = new Label();
            groupBoxTamañosLote = new GroupBox();
            clbTamañosLote = new CheckedListBox();
            panelPreviewLote = new Panel();
            lblPreviewLote = new Label();
            pbPreviewLote = new PictureBox();
            btnConvertirLote = new Button();
            progressBarLote = new ProgressBar();
            progressBarIndividual = new ProgressBar();
            lblProgresoLote = new Label();
            lblArchivoActual = new Label();
            tabEditor = new TabPage();
            lblInfoIcono = new Label();
            btnGuardarIcono = new Button();
            btnRevertir = new Button();
            panelControlsPixeles = new Panel();
            lblZoom = new Label();
            trackBarZoom = new TrackBar();
            lblNivelZoom = new Label();
            btnZoomMas = new Button();
            btnZoomMenos = new Button();
            btnZoomAjustar = new Button();
            groupBoxHerramientasDibujo = new GroupBox();
            lblTolerancia = new Label();
            rbPincel = new RadioButton();
            trackBarTolerancia = new TrackBar();
            lblNivelTolerancia = new Label();
            rbBorrador = new RadioButton();
            rbCuentagotas = new RadioButton();
            rbVaritaMagica = new RadioButton();
            rbRelleno = new RadioButton();
            panelColorActual = new Panel();
            btnSeleccionarColor = new Button();
            lblInfoPixel = new Label();
            chkMostrarCuadricula = new CheckBox();
            btnDeshacerPixeles = new Button();
            btnLimpiarCanvas = new Button();
            btnAplicarEdicionPixeles = new Button();
            lblTituloEditor = new Label();
            btnCargarIcono = new Button();
            btnExplorarCarpetaIconos = new Button();
            lvIconos = new ListView();
            groupBoxTamañosIcono = new GroupBox();
            pbIconoEditor = new PictureBox();
            groupBoxHerramientas = new GroupBox();
            lblValorAjuste = new Label();
            trackBarAjuste = new TrackBar();
            btnEscalaGrises = new Button();
            btnContraste = new Button();
            btnBrillo = new Button();
            btnEspejo = new Button();
            btnRotar = new Button();
            btnRedimensionar = new Button();
            btnAplicarCambios = new Button();
            btnExportarTamaño = new Button();
            lblTamañoActual = new Label();
            pbTamañoSeleccionado = new PictureBox();
            lstTamañosIcono = new ListBox();
            panelEdicionPixeles = new Panel();
            panelEditorPrincipal = new Panel();
            BtnRestaurarImagen = new Button();
            tabControl1.SuspendLayout();
            tabIndividual.SuspendLayout();
            gbHerramientasEdicion.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)trackBarToleranciaIndividual).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pbPreview).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pbRecorte).BeginInit();
            tabLote.SuspendLayout();
            groupBoxTamañosLote.SuspendLayout();
            panelPreviewLote.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pbPreviewLote).BeginInit();
            tabEditor.SuspendLayout();
            panelControlsPixeles.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)trackBarZoom).BeginInit();
            groupBoxHerramientasDibujo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)trackBarTolerancia).BeginInit();
            groupBoxTamañosIcono.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pbIconoEditor).BeginInit();
            groupBoxHerramientas.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)trackBarAjuste).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pbTamañoSeleccionado).BeginInit();
            panelEdicionPixeles.SuspendLayout();
            SuspendLayout();
            // 
            // tabControl1
            // 
            tabControl1.Controls.Add(tabIndividual);
            tabControl1.Controls.Add(tabLote);
            tabControl1.Controls.Add(tabEditor);
            tabControl1.Dock = DockStyle.Fill;
            tabControl1.Font = new Font("Segoe UI", 9F);
            tabControl1.Location = new Point(0, 0);
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new Size(1289, 691);
            tabControl1.TabIndex = 0;
            // 
            // tabIndividual
            // 
            tabIndividual.BackColor = Color.White;
            tabIndividual.Controls.Add(gbHerramientasEdicion);
            tabIndividual.Controls.Add(lblTitulo);
            tabIndividual.Controls.Add(btnSeleccionar);
            tabIndividual.Controls.Add(lblArchivo);
            tabIndividual.Controls.Add(lblDimensiones);
            tabIndividual.Controls.Add(pbPreview);
            tabIndividual.Controls.Add(lblEstado);
            tabIndividual.Controls.Add(chkMantenerProporcion);
            tabIndividual.Controls.Add(btnLimpiarSeleccion);
            tabIndividual.Controls.Add(pbRecorte);
            tabIndividual.Controls.Add(lblRecorte);
            tabIndividual.Controls.Add(lblTamaños);
            tabIndividual.Controls.Add(clbTamaños);
            tabIndividual.Controls.Add(btnConvertir);
            tabIndividual.Controls.Add(progressBar);
            tabIndividual.Controls.Add(txtLog);
            tabIndividual.Controls.Add(btnLimpiarLog);
            tabIndividual.Location = new Point(4, 24);
            tabIndividual.Name = "tabIndividual";
            tabIndividual.Padding = new Padding(3);
            tabIndividual.Size = new Size(1281, 663);
            tabIndividual.TabIndex = 0;
            tabIndividual.Text = "Conversión Individual";
            // 
            // gbHerramientasEdicion
            // 
            gbHerramientasEdicion.Controls.Add(chkVistaPrevia);
            gbHerramientasEdicion.Controls.Add(trackBarToleranciaIndividual);
            gbHerramientasEdicion.Controls.Add(BtnRestaurarImagen);
            gbHerramientasEdicion.Controls.Add(btnEliminarFondo);
            gbHerramientasEdicion.Controls.Add(lblToleranciaIndividual);
            gbHerramientasEdicion.Controls.Add(lblToleranciaIndividal);
            gbHerramientasEdicion.Location = new Point(880, 51);
            gbHerramientasEdicion.Name = "gbHerramientasEdicion";
            gbHerramientasEdicion.Size = new Size(391, 395);
            gbHerramientasEdicion.TabIndex = 16;
            gbHerramientasEdicion.TabStop = false;
            gbHerramientasEdicion.Text = "groupBox1";
            // 
            // chkVistaPrevia
            // 
            chkVistaPrevia.AutoSize = true;
            chkVistaPrevia.Location = new Point(27, 189);
            chkVistaPrevia.Name = "chkVistaPrevia";
            chkVistaPrevia.Size = new Size(86, 19);
            chkVistaPrevia.TabIndex = 3;
            chkVistaPrevia.Text = "Vista Previa";
            chkVistaPrevia.UseVisualStyleBackColor = true;
            // 
            // trackBarToleranciaIndividual
            // 
            trackBarToleranciaIndividual.Location = new Point(19, 91);
            trackBarToleranciaIndividual.Name = "trackBarToleranciaIndividual";
            trackBarToleranciaIndividual.Size = new Size(351, 45);
            trackBarToleranciaIndividual.TabIndex = 2;
            // 
            // btnEliminarFondo
            // 
            btnEliminarFondo.BackColor = Color.FromArgb(45, 137, 239);
            btnEliminarFondo.FlatAppearance.BorderSize = 0;
            btnEliminarFondo.FlatStyle = FlatStyle.Flat;
            btnEliminarFondo.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnEliminarFondo.ForeColor = Color.White;
            btnEliminarFondo.Location = new Point(24, 39);
            btnEliminarFondo.Name = "btnEliminarFondo";
            btnEliminarFondo.Size = new Size(183, 40);
            btnEliminarFondo.TabIndex = 1;
            btnEliminarFondo.Text = "\U0001fa84 Eliminar Fondo";
            btnEliminarFondo.UseVisualStyleBackColor = false;
            btnEliminarFondo.Click += BtnEliminarFondo_Click;
            // 
            // lblToleranciaIndividual
            // 
            lblToleranciaIndividual.AutoSize = true;
            lblToleranciaIndividual.Font = new Font("Segoe UI", 9F);
            lblToleranciaIndividual.ForeColor = Color.Gray;
            lblToleranciaIndividual.Location = new Point(106, 144);
            lblToleranciaIndividual.Name = "lblToleranciaIndividual";
            lblToleranciaIndividual.Size = new Size(13, 15);
            lblToleranciaIndividual.TabIndex = 2;
            lblToleranciaIndividual.Text = "0";
            // 
            // lblToleranciaIndividal
            // 
            lblToleranciaIndividal.AutoSize = true;
            lblToleranciaIndividal.Font = new Font("Segoe UI", 9F);
            lblToleranciaIndividal.ForeColor = Color.Gray;
            lblToleranciaIndividal.Location = new Point(24, 144);
            lblToleranciaIndividal.Name = "lblToleranciaIndividal";
            lblToleranciaIndividal.Size = new Size(63, 15);
            lblToleranciaIndividal.TabIndex = 2;
            lblToleranciaIndividal.Text = "Tolerancia:";
            // 
            // lblTitulo
            // 
            lblTitulo.AutoSize = true;
            lblTitulo.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            lblTitulo.ForeColor = Color.FromArgb(45, 137, 239);
            lblTitulo.Location = new Point(20, 20);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Size = new Size(338, 30);
            lblTitulo.TabIndex = 0;
            lblTitulo.Text = "Convertidor de Imágenes a ICO";
            // 
            // btnSeleccionar
            // 
            btnSeleccionar.BackColor = Color.FromArgb(45, 137, 239);
            btnSeleccionar.FlatAppearance.BorderSize = 0;
            btnSeleccionar.FlatStyle = FlatStyle.Flat;
            btnSeleccionar.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnSeleccionar.ForeColor = Color.White;
            btnSeleccionar.Location = new Point(540, 20);
            btnSeleccionar.Name = "btnSeleccionar";
            btnSeleccionar.Size = new Size(180, 40);
            btnSeleccionar.TabIndex = 1;
            btnSeleccionar.Text = "📁 Seleccionar Imagen";
            btnSeleccionar.UseVisualStyleBackColor = false;
            // 
            // lblArchivo
            // 
            lblArchivo.AutoSize = true;
            lblArchivo.Font = new Font("Segoe UI", 9F);
            lblArchivo.ForeColor = Color.Gray;
            lblArchivo.Location = new Point(735, 25);
            lblArchivo.Name = "lblArchivo";
            lblArchivo.Size = new Size(161, 15);
            lblArchivo.TabIndex = 2;
            lblArchivo.Text = "Ningún archivo seleccionado";
            // 
            // lblDimensiones
            // 
            lblDimensiones.AutoSize = true;
            lblDimensiones.Font = new Font("Segoe UI", 9F);
            lblDimensiones.ForeColor = Color.Gray;
            lblDimensiones.Location = new Point(735, 45);
            lblDimensiones.Name = "lblDimensiones";
            lblDimensiones.Size = new Size(47, 15);
            lblDimensiones.TabIndex = 3;
            lblDimensiones.Text = "0 x 0 px";
            // 
            // pbPreview
            // 
            pbPreview.BackColor = Color.FromArgb(240, 240, 240);
            pbPreview.BorderStyle = BorderStyle.FixedSingle;
            pbPreview.Location = new Point(25, 53);
            pbPreview.Name = "pbPreview";
            pbPreview.Size = new Size(500, 377);
            pbPreview.SizeMode = PictureBoxSizeMode.Zoom;
            pbPreview.TabIndex = 4;
            pbPreview.TabStop = false;
            // 
            // lblEstado
            // 
            lblEstado.AutoSize = true;
            lblEstado.Font = new Font("Segoe UI", 9F);
            lblEstado.ForeColor = Color.Gray;
            lblEstado.Location = new Point(25, 440);
            lblEstado.Name = "lblEstado";
            lblEstado.Size = new Size(179, 15);
            lblEstado.TabIndex = 5;
            lblEstado.Text = "Selecciona un área con el mouse";
            // 
            // chkMantenerProporcion
            // 
            chkMantenerProporcion.AutoSize = true;
            chkMantenerProporcion.Enabled = false;
            chkMantenerProporcion.Font = new Font("Segoe UI", 9F);
            chkMantenerProporcion.Location = new Point(250, 438);
            chkMantenerProporcion.Name = "chkMantenerProporcion";
            chkMantenerProporcion.Size = new Size(157, 19);
            chkMantenerProporcion.TabIndex = 6;
            chkMantenerProporcion.Text = "Mantener proporción 1:1";
            chkMantenerProporcion.UseVisualStyleBackColor = true;
            chkMantenerProporcion.CheckedChanged += ChkMantenerProporcion_CheckedChanged;
            // 
            // btnLimpiarSeleccion
            // 
            btnLimpiarSeleccion.BackColor = Color.FromArgb(244, 67, 54);
            btnLimpiarSeleccion.Enabled = false;
            btnLimpiarSeleccion.FlatAppearance.BorderSize = 0;
            btnLimpiarSeleccion.FlatStyle = FlatStyle.Flat;
            btnLimpiarSeleccion.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnLimpiarSeleccion.ForeColor = Color.White;
            btnLimpiarSeleccion.Location = new Point(420, 433);
            btnLimpiarSeleccion.Name = "btnLimpiarSeleccion";
            btnLimpiarSeleccion.Size = new Size(105, 30);
            btnLimpiarSeleccion.TabIndex = 7;
            btnLimpiarSeleccion.Text = "🗑️ Limpiar";
            btnLimpiarSeleccion.UseVisualStyleBackColor = false;
            btnLimpiarSeleccion.Click += BtnLimpiarSeleccion_Click;
            // 
            // pbRecorte
            // 
            pbRecorte.BackColor = Color.White;
            pbRecorte.BorderStyle = BorderStyle.FixedSingle;
            pbRecorte.Location = new Point(540, 250);
            pbRecorte.Name = "pbRecorte";
            pbRecorte.Size = new Size(180, 180);
            pbRecorte.SizeMode = PictureBoxSizeMode.Zoom;
            pbRecorte.TabIndex = 8;
            pbRecorte.TabStop = false;
            pbRecorte.Visible = false;
            // 
            // lblRecorte
            // 
            lblRecorte.AutoSize = true;
            lblRecorte.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblRecorte.Location = new Point(540, 225);
            lblRecorte.Name = "lblRecorte";
            lblRecorte.Size = new Size(80, 15);
            lblRecorte.TabIndex = 9;
            lblRecorte.Text = "Preview: 0x0";
            lblRecorte.Visible = false;
            // 
            // lblTamaños
            // 
            lblTamaños.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblTamaños.Location = new Point(726, 261);
            lblTamaños.Name = "lblTamaños";
            lblTamaños.Size = new Size(134, 19);
            lblTamaños.TabIndex = 10;
            lblTamaños.Text = "Tamaños de Icono:";
            // 
            // clbTamaños
            // 
            clbTamaños.CheckOnClick = true;
            clbTamaños.Font = new Font("Segoe UI", 9F);
            clbTamaños.FormattingEnabled = true;
            clbTamaños.Items.AddRange(new object[] { "16x16", "24x24", "32x32", "48x48", "64x64", "128x128", "256x256" });
            clbTamaños.Location = new Point(726, 289);
            clbTamaños.Name = "clbTamaños";
            clbTamaños.Size = new Size(134, 166);
            clbTamaños.TabIndex = 11;
            // 
            // btnConvertir
            // 
            btnConvertir.BackColorHover = Color.FromArgb(58, 150, 245);
            btnConvertir.BackColorNormal = Color.FromArgb(45, 137, 239);
            btnConvertir.BackColorPressed = Color.FromArgb(32, 120, 220);
            btnConvertir.BorderColorButton = Color.FromArgb(38, 122, 213);
            btnConvertir.BorderRadius = 8;
            btnConvertir.BottomText = "Convertir ICO";
            btnConvertir.Enabled = false;
            btnConvertir.ForeColor = Color.White;
            btnConvertir.ForeColorButton = Color.White;
            btnConvertir.GlifoFuente = "FontAwesome";
            btnConvertir.Glyph = "\\uF1C5";
            btnConvertir.Location = new Point(540, 90);
            btnConvertir.Name = "btnConvertir";
            btnConvertir.ProgressBackColor = Color.FromArgb(30, 30, 30);
            btnConvertir.ProgressBarHeight = 4;
            btnConvertir.ProgressColor = Color.FromArgb(76, 175, 80);
            btnConvertir.Size = new Size(320, 120);
            btnConvertir.TabIndex = 12;
            btnConvertir.TopRightText = "F9";
            btnConvertir.Click += BtnConvertir_Click;
            // 
            // progressBar
            // 
            progressBar.Location = new Point(907, 20);
            progressBar.Name = "progressBar";
            progressBar.Size = new Size(134, 15);
            progressBar.TabIndex = 13;
            progressBar.Visible = false;
            // 
            // txtLog
            // 
            txtLog.BackColor = Color.Black;
            txtLog.Font = new Font("Consolas", 9F);
            txtLog.ForeColor = Color.Lime;
            txtLog.Location = new Point(25, 475);
            txtLog.Multiline = true;
            txtLog.Name = "txtLog";
            txtLog.ReadOnly = true;
            txtLog.ScrollBars = ScrollBars.Vertical;
            txtLog.Size = new Size(835, 146);
            txtLog.TabIndex = 14;
            // 
            // btnLimpiarLog
            // 
            btnLimpiarLog.BackColor = Color.FromArgb(100, 100, 100);
            btnLimpiarLog.FlatAppearance.BorderSize = 0;
            btnLimpiarLog.FlatStyle = FlatStyle.Flat;
            btnLimpiarLog.Font = new Font("Segoe UI", 8F);
            btnLimpiarLog.ForeColor = Color.White;
            btnLimpiarLog.Location = new Point(540, 437);
            btnLimpiarLog.Name = "btnLimpiarLog";
            btnLimpiarLog.Size = new Size(180, 23);
            btnLimpiarLog.TabIndex = 15;
            btnLimpiarLog.Text = "Limpiar Log";
            btnLimpiarLog.UseVisualStyleBackColor = false;
            btnLimpiarLog.Click += BtnLimpiarLog_Click;
            // 
            // tabLote
            // 
            tabLote.BackColor = Color.White;
            tabLote.Controls.Add(lblTituloLote);
            tabLote.Controls.Add(btnSeleccionarCarpeta);
            tabLote.Controls.Add(btnAgregarArchivos);
            tabLote.Controls.Add(lblCarpetaActual);
            tabLote.Controls.Add(lvArchivosLote);
            tabLote.Controls.Add(btnSeleccionarTodos);
            tabLote.Controls.Add(btnDeseleccionarTodos);
            tabLote.Controls.Add(btnLimpiarLista);
            tabLote.Controls.Add(lblCantidadArchivos);
            tabLote.Controls.Add(lblSeleccionados);
            tabLote.Controls.Add(groupBoxTamañosLote);
            tabLote.Controls.Add(panelPreviewLote);
            tabLote.Controls.Add(btnConvertirLote);
            tabLote.Controls.Add(progressBarLote);
            tabLote.Controls.Add(progressBarIndividual);
            tabLote.Controls.Add(lblProgresoLote);
            tabLote.Controls.Add(lblArchivoActual);
            tabLote.Location = new Point(4, 24);
            tabLote.Name = "tabLote";
            tabLote.Padding = new Padding(3);
            tabLote.Size = new Size(1281, 663);
            tabLote.TabIndex = 1;
            tabLote.Text = "Conversión por Lotes";
            // 
            // lblTituloLote
            // 
            lblTituloLote.AutoSize = true;
            lblTituloLote.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            lblTituloLote.ForeColor = Color.FromArgb(45, 137, 239);
            lblTituloLote.Location = new Point(20, 20);
            lblTituloLote.Name = "lblTituloLote";
            lblTituloLote.Size = new Size(231, 30);
            lblTituloLote.TabIndex = 0;
            lblTituloLote.Text = "Conversión por Lotes";
            // 
            // btnSeleccionarCarpeta
            // 
            btnSeleccionarCarpeta.BackColor = Color.FromArgb(45, 137, 239);
            btnSeleccionarCarpeta.FlatAppearance.BorderSize = 0;
            btnSeleccionarCarpeta.FlatStyle = FlatStyle.Flat;
            btnSeleccionarCarpeta.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnSeleccionarCarpeta.ForeColor = Color.White;
            btnSeleccionarCarpeta.Location = new Point(25, 70);
            btnSeleccionarCarpeta.Name = "btnSeleccionarCarpeta";
            btnSeleccionarCarpeta.Size = new Size(160, 35);
            btnSeleccionarCarpeta.TabIndex = 1;
            btnSeleccionarCarpeta.Text = "📁 Seleccionar Carpeta";
            btnSeleccionarCarpeta.UseVisualStyleBackColor = false;
            // 
            // btnAgregarArchivos
            // 
            btnAgregarArchivos.BackColor = Color.FromArgb(76, 175, 80);
            btnAgregarArchivos.FlatAppearance.BorderSize = 0;
            btnAgregarArchivos.FlatStyle = FlatStyle.Flat;
            btnAgregarArchivos.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnAgregarArchivos.ForeColor = Color.White;
            btnAgregarArchivos.Location = new Point(195, 70);
            btnAgregarArchivos.Name = "btnAgregarArchivos";
            btnAgregarArchivos.Size = new Size(160, 35);
            btnAgregarArchivos.TabIndex = 2;
            btnAgregarArchivos.Text = "➕ Agregar Archivos";
            btnAgregarArchivos.UseVisualStyleBackColor = false;
            // 
            // lblCarpetaActual
            // 
            lblCarpetaActual.AutoSize = true;
            lblCarpetaActual.Font = new Font("Segoe UI", 9F);
            lblCarpetaActual.ForeColor = Color.Gray;
            lblCarpetaActual.Location = new Point(365, 79);
            lblCarpetaActual.Name = "lblCarpetaActual";
            lblCarpetaActual.Size = new Size(166, 15);
            lblCarpetaActual.TabIndex = 3;
            lblCarpetaActual.Text = "Ninguna carpeta seleccionada";
            // 
            // lvArchivosLote
            // 
            lvArchivosLote.CheckBoxes = true;
            lvArchivosLote.Font = new Font("Segoe UI", 9F);
            lvArchivosLote.FullRowSelect = true;
            lvArchivosLote.GridLines = true;
            lvArchivosLote.Location = new Point(25, 125);
            lvArchivosLote.Name = "lvArchivosLote";
            lvArchivosLote.Size = new Size(601, 330);
            lvArchivosLote.TabIndex = 4;
            lvArchivosLote.UseCompatibleStateImageBehavior = false;
            lvArchivosLote.View = View.Details;
            // 
            // btnSeleccionarTodos
            // 
            btnSeleccionarTodos.Font = new Font("Segoe UI", 9F);
            btnSeleccionarTodos.Location = new Point(25, 465);
            btnSeleccionarTodos.Name = "btnSeleccionarTodos";
            btnSeleccionarTodos.Size = new Size(140, 30);
            btnSeleccionarTodos.TabIndex = 6;
            btnSeleccionarTodos.Text = "✓ Seleccionar Todos";
            btnSeleccionarTodos.UseVisualStyleBackColor = true;
            // 
            // btnDeseleccionarTodos
            // 
            btnDeseleccionarTodos.Font = new Font("Segoe UI", 9F);
            btnDeseleccionarTodos.Location = new Point(175, 465);
            btnDeseleccionarTodos.Name = "btnDeseleccionarTodos";
            btnDeseleccionarTodos.Size = new Size(150, 30);
            btnDeseleccionarTodos.TabIndex = 7;
            btnDeseleccionarTodos.Text = "✗ Deseleccionar Todos";
            btnDeseleccionarTodos.UseVisualStyleBackColor = true;
            // 
            // btnLimpiarLista
            // 
            btnLimpiarLista.BackColor = Color.FromArgb(244, 67, 54);
            btnLimpiarLista.FlatAppearance.BorderSize = 0;
            btnLimpiarLista.FlatStyle = FlatStyle.Flat;
            btnLimpiarLista.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnLimpiarLista.ForeColor = Color.White;
            btnLimpiarLista.Location = new Point(335, 465);
            btnLimpiarLista.Name = "btnLimpiarLista";
            btnLimpiarLista.Size = new Size(110, 30);
            btnLimpiarLista.TabIndex = 8;
            btnLimpiarLista.Text = "🗑️ Limpiar Lista";
            btnLimpiarLista.UseVisualStyleBackColor = false;
            // 
            // lblCantidadArchivos
            // 
            lblCantidadArchivos.AutoSize = true;
            lblCantidadArchivos.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblCantidadArchivos.Location = new Point(460, 472);
            lblCantidadArchivos.Name = "lblCantidadArchivos";
            lblCantidadArchivos.Size = new Size(63, 15);
            lblCantidadArchivos.TabIndex = 9;
            lblCantidadArchivos.Text = "0 archivos";
            // 
            // lblSeleccionados
            // 
            lblSeleccionados.AutoSize = true;
            lblSeleccionados.Font = new Font("Segoe UI", 9F);
            lblSeleccionados.ForeColor = Color.Blue;
            lblSeleccionados.Location = new Point(580, 472);
            lblSeleccionados.Name = "lblSeleccionados";
            lblSeleccionados.Size = new Size(115, 15);
            lblSeleccionados.TabIndex = 10;
            lblSeleccionados.Text = "0 seleccionados de 0";
            // 
            // groupBoxTamañosLote
            // 
            groupBoxTamañosLote.Controls.Add(clbTamañosLote);
            groupBoxTamañosLote.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            groupBoxTamañosLote.Location = new Point(922, 125);
            groupBoxTamañosLote.Name = "groupBoxTamañosLote";
            groupBoxTamañosLote.Size = new Size(172, 230);
            groupBoxTamañosLote.TabIndex = 11;
            groupBoxTamañosLote.TabStop = false;
            groupBoxTamañosLote.Text = "Tamaños de Icono";
            // 
            // clbTamañosLote
            // 
            clbTamañosLote.CheckOnClick = true;
            clbTamañosLote.Font = new Font("Segoe UI", 9F);
            clbTamañosLote.FormattingEnabled = true;
            clbTamañosLote.Items.AddRange(new object[] { "16x16", "24x24", "32x32", "48x48", "64x64", "128x128", "256x256" });
            clbTamañosLote.Location = new Point(15, 25);
            clbTamañosLote.Name = "clbTamañosLote";
            clbTamañosLote.Size = new Size(128, 184);
            clbTamañosLote.TabIndex = 0;
            // 
            // panelPreviewLote
            // 
            panelPreviewLote.BackColor = Color.FromArgb(245, 245, 245);
            panelPreviewLote.BorderStyle = BorderStyle.FixedSingle;
            panelPreviewLote.Controls.Add(lblPreviewLote);
            panelPreviewLote.Controls.Add(pbPreviewLote);
            panelPreviewLote.Location = new Point(632, 125);
            panelPreviewLote.Name = "panelPreviewLote";
            panelPreviewLote.Size = new Size(280, 330);
            panelPreviewLote.TabIndex = 5;
            // 
            // lblPreviewLote
            // 
            lblPreviewLote.BackColor = Color.FromArgb(45, 137, 239);
            lblPreviewLote.Dock = DockStyle.Top;
            lblPreviewLote.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblPreviewLote.ForeColor = Color.White;
            lblPreviewLote.Location = new Point(0, 0);
            lblPreviewLote.Name = "lblPreviewLote";
            lblPreviewLote.Padding = new Padding(5);
            lblPreviewLote.Size = new Size(278, 35);
            lblPreviewLote.TabIndex = 0;
            lblPreviewLote.Text = "Vista Previa";
            lblPreviewLote.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // pbPreviewLote
            // 
            pbPreviewLote.BackColor = Color.White;
            pbPreviewLote.Location = new Point(10, 45);
            pbPreviewLote.Name = "pbPreviewLote";
            pbPreviewLote.Size = new Size(258, 275);
            pbPreviewLote.SizeMode = PictureBoxSizeMode.Zoom;
            pbPreviewLote.TabIndex = 1;
            pbPreviewLote.TabStop = false;
            // 
            // btnConvertirLote
            // 
            btnConvertirLote.BackColor = Color.FromArgb(255, 152, 0);
            btnConvertirLote.FlatAppearance.BorderSize = 0;
            btnConvertirLote.FlatStyle = FlatStyle.Flat;
            btnConvertirLote.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            btnConvertirLote.ForeColor = Color.White;
            btnConvertirLote.Location = new Point(25, 515);
            btnConvertirLote.Name = "btnConvertirLote";
            btnConvertirLote.Size = new Size(1340, 50);
            btnConvertirLote.TabIndex = 12;
            btnConvertirLote.Text = "🚀 CONVERTIR TODOS LOS ARCHIVOS SELECCIONADOS";
            btnConvertirLote.UseVisualStyleBackColor = false;
            // 
            // progressBarLote
            // 
            progressBarLote.Location = new Point(25, 580);
            progressBarLote.Name = "progressBarLote";
            progressBarLote.Size = new Size(1180, 25);
            progressBarLote.TabIndex = 13;
            progressBarLote.Visible = false;
            // 
            // progressBarIndividual
            // 
            progressBarIndividual.Location = new Point(1215, 580);
            progressBarIndividual.Name = "progressBarIndividual";
            progressBarIndividual.Size = new Size(150, 25);
            progressBarIndividual.TabIndex = 14;
            progressBarIndividual.Visible = false;
            // 
            // lblProgresoLote
            // 
            lblProgresoLote.AutoSize = true;
            lblProgresoLote.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblProgresoLote.Location = new Point(25, 615);
            lblProgresoLote.Name = "lblProgresoLote";
            lblProgresoLote.Size = new Size(82, 15);
            lblProgresoLote.TabIndex = 15;
            lblProgresoLote.Text = "Progreso: 0/0";
            // 
            // lblArchivoActual
            // 
            lblArchivoActual.AutoSize = true;
            lblArchivoActual.Font = new Font("Segoe UI", 9F);
            lblArchivoActual.ForeColor = Color.Gray;
            lblArchivoActual.Location = new Point(120, 615);
            lblArchivoActual.Name = "lblArchivoActual";
            lblArchivoActual.Size = new Size(71, 15);
            lblArchivoActual.TabIndex = 16;
            lblArchivoActual.Text = "Sin procesar";
            // 
            // tabEditor
            // 
            tabEditor.BackColor = Color.White;
            tabEditor.Controls.Add(lblInfoIcono);
            tabEditor.Controls.Add(btnGuardarIcono);
            tabEditor.Controls.Add(btnRevertir);
            tabEditor.Controls.Add(panelControlsPixeles);
            tabEditor.Controls.Add(lblTituloEditor);
            tabEditor.Controls.Add(btnCargarIcono);
            tabEditor.Controls.Add(btnExplorarCarpetaIconos);
            tabEditor.Controls.Add(lvIconos);
            tabEditor.Controls.Add(groupBoxTamañosIcono);
            tabEditor.Controls.Add(panelEdicionPixeles);
            tabEditor.Location = new Point(4, 24);
            tabEditor.Name = "tabEditor";
            tabEditor.Padding = new Padding(3);
            tabEditor.Size = new Size(1281, 663);
            tabEditor.TabIndex = 2;
            tabEditor.Text = "Editor de Iconos";
            // 
            // lblInfoIcono
            // 
            lblInfoIcono.BackColor = Color.FromArgb(45, 137, 239);
            lblInfoIcono.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblInfoIcono.ForeColor = Color.White;
            lblInfoIcono.Location = new Point(503, 6);
            lblInfoIcono.Name = "lblInfoIcono";
            lblInfoIcono.Padding = new Padding(5);
            lblInfoIcono.Size = new Size(316, 35);
            lblInfoIcono.TabIndex = 0;
            lblInfoIcono.Text = "Ningún icono cargado";
            lblInfoIcono.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // btnGuardarIcono
            // 
            btnGuardarIcono.BackColor = Color.FromArgb(255, 152, 0);
            btnGuardarIcono.FlatAppearance.BorderSize = 0;
            btnGuardarIcono.FlatStyle = FlatStyle.Flat;
            btnGuardarIcono.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            btnGuardarIcono.ForeColor = Color.White;
            btnGuardarIcono.Location = new Point(392, 585);
            btnGuardarIcono.Name = "btnGuardarIcono";
            btnGuardarIcono.Size = new Size(350, 37);
            btnGuardarIcono.TabIndex = 7;
            btnGuardarIcono.Text = "💾 GUARDAR ICONO EDITADO";
            btnGuardarIcono.UseVisualStyleBackColor = false;
            btnGuardarIcono.Click += BtnGuardarIcono_Click;
            // 
            // btnRevertir
            // 
            btnRevertir.BackColor = Color.FromArgb(244, 67, 54);
            btnRevertir.FlatAppearance.BorderSize = 0;
            btnRevertir.FlatStyle = FlatStyle.Flat;
            btnRevertir.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnRevertir.ForeColor = Color.White;
            btnRevertir.Location = new Point(392, 628);
            btnRevertir.Name = "btnRevertir";
            btnRevertir.Size = new Size(350, 31);
            btnRevertir.TabIndex = 8;
            btnRevertir.Text = "↩️ Revertir Cambios";
            btnRevertir.UseVisualStyleBackColor = false;
            btnRevertir.Click += BtnRevertir_Click;
            // 
            // panelControlsPixeles
            // 
            panelControlsPixeles.BackColor = Color.White;
            panelControlsPixeles.BorderStyle = BorderStyle.FixedSingle;
            panelControlsPixeles.Controls.Add(lblZoom);
            panelControlsPixeles.Controls.Add(trackBarZoom);
            panelControlsPixeles.Controls.Add(lblNivelZoom);
            panelControlsPixeles.Controls.Add(btnZoomMas);
            panelControlsPixeles.Controls.Add(btnZoomMenos);
            panelControlsPixeles.Controls.Add(btnZoomAjustar);
            panelControlsPixeles.Controls.Add(groupBoxHerramientasDibujo);
            panelControlsPixeles.Controls.Add(panelColorActual);
            panelControlsPixeles.Controls.Add(btnSeleccionarColor);
            panelControlsPixeles.Controls.Add(lblInfoPixel);
            panelControlsPixeles.Controls.Add(chkMostrarCuadricula);
            panelControlsPixeles.Controls.Add(btnDeshacerPixeles);
            panelControlsPixeles.Controls.Add(btnLimpiarCanvas);
            panelControlsPixeles.Controls.Add(btnAplicarEdicionPixeles);
            panelControlsPixeles.Location = new Point(825, 48);
            panelControlsPixeles.Name = "panelControlsPixeles";
            panelControlsPixeles.Size = new Size(168, 614);
            panelControlsPixeles.TabIndex = 1;
            // 
            // lblZoom
            // 
            lblZoom.AutoSize = true;
            lblZoom.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblZoom.Location = new Point(2, 4);
            lblZoom.Name = "lblZoom";
            lblZoom.Size = new Size(42, 15);
            lblZoom.TabIndex = 0;
            lblZoom.Text = "Zoom:";
            // 
            // trackBarZoom
            // 
            trackBarZoom.Location = new Point(2, 24);
            trackBarZoom.Maximum = 50;
            trackBarZoom.Minimum = 2;
            trackBarZoom.Name = "trackBarZoom";
            trackBarZoom.Size = new Size(158, 45);
            trackBarZoom.TabIndex = 1;
            trackBarZoom.TickFrequency = 5;
            trackBarZoom.Value = 10;
            // 
            // lblNivelZoom
            // 
            lblNivelZoom.AutoSize = true;
            lblNivelZoom.Font = new Font("Segoe UI", 9F);
            lblNivelZoom.Location = new Point(62, 4);
            lblNivelZoom.Name = "lblNivelZoom";
            lblNivelZoom.Size = new Size(25, 15);
            lblNivelZoom.TabIndex = 2;
            lblNivelZoom.Text = "10x";
            // 
            // btnZoomMas
            // 
            btnZoomMas.Location = new Point(2, 69);
            btnZoomMas.Name = "btnZoomMas";
            btnZoomMas.Size = new Size(30, 28);
            btnZoomMas.TabIndex = 3;
            btnZoomMas.Text = "➕";
            btnZoomMas.UseVisualStyleBackColor = true;
            // 
            // btnZoomMenos
            // 
            btnZoomMenos.Location = new Point(38, 69);
            btnZoomMenos.Name = "btnZoomMenos";
            btnZoomMenos.Size = new Size(30, 28);
            btnZoomMenos.TabIndex = 4;
            btnZoomMenos.Text = "➖";
            btnZoomMenos.UseVisualStyleBackColor = true;
            // 
            // btnZoomAjustar
            // 
            btnZoomAjustar.Location = new Point(74, 69);
            btnZoomAjustar.Name = "btnZoomAjustar";
            btnZoomAjustar.Size = new Size(30, 28);
            btnZoomAjustar.TabIndex = 5;
            btnZoomAjustar.Text = "🔍";
            btnZoomAjustar.UseVisualStyleBackColor = true;
            // 
            // groupBoxHerramientasDibujo
            // 
            groupBoxHerramientasDibujo.Controls.Add(lblTolerancia);
            groupBoxHerramientasDibujo.Controls.Add(rbPincel);
            groupBoxHerramientasDibujo.Controls.Add(trackBarTolerancia);
            groupBoxHerramientasDibujo.Controls.Add(lblNivelTolerancia);
            groupBoxHerramientasDibujo.Controls.Add(rbBorrador);
            groupBoxHerramientasDibujo.Controls.Add(rbCuentagotas);
            groupBoxHerramientasDibujo.Controls.Add(rbVaritaMagica);
            groupBoxHerramientasDibujo.Controls.Add(rbRelleno);
            groupBoxHerramientasDibujo.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            groupBoxHerramientasDibujo.Location = new Point(2, 109);
            groupBoxHerramientasDibujo.Name = "groupBoxHerramientasDibujo";
            groupBoxHerramientasDibujo.Size = new Size(158, 215);
            groupBoxHerramientasDibujo.TabIndex = 6;
            groupBoxHerramientasDibujo.TabStop = false;
            groupBoxHerramientasDibujo.Text = "Herramientas";
            // 
            // lblTolerancia
            // 
            lblTolerancia.AutoSize = true;
            lblTolerancia.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblTolerancia.Location = new Point(11, 151);
            lblTolerancia.Name = "lblTolerancia";
            lblTolerancia.Size = new Size(42, 15);
            lblTolerancia.TabIndex = 0;
            lblTolerancia.Text = "Zoom:";
            // 
            // rbPincel
            // 
            rbPincel.AutoSize = true;
            rbPincel.Checked = true;
            rbPincel.Font = new Font("Segoe UI", 9F);
            rbPincel.Location = new Point(10, 25);
            rbPincel.Name = "rbPincel";
            rbPincel.Size = new Size(72, 19);
            rbPincel.TabIndex = 0;
            rbPincel.TabStop = true;
            rbPincel.Text = "🖌️ Pincel";
            rbPincel.UseVisualStyleBackColor = true;
            // 
            // trackBarTolerancia
            // 
            trackBarTolerancia.Location = new Point(4, 169);
            trackBarTolerancia.Maximum = 50;
            trackBarTolerancia.Minimum = 2;
            trackBarTolerancia.Name = "trackBarTolerancia";
            trackBarTolerancia.Size = new Size(149, 45);
            trackBarTolerancia.TabIndex = 1;
            trackBarTolerancia.TickFrequency = 5;
            trackBarTolerancia.Value = 10;
            // 
            // lblNivelTolerancia
            // 
            lblNivelTolerancia.AutoSize = true;
            lblNivelTolerancia.Font = new Font("Segoe UI", 9F);
            lblNivelTolerancia.Location = new Point(71, 151);
            lblNivelTolerancia.Name = "lblNivelTolerancia";
            lblNivelTolerancia.Size = new Size(25, 15);
            lblNivelTolerancia.TabIndex = 2;
            lblNivelTolerancia.Text = "10x";
            // 
            // rbBorrador
            // 
            rbBorrador.AutoSize = true;
            rbBorrador.Font = new Font("Segoe UI", 9F);
            rbBorrador.Location = new Point(10, 50);
            rbBorrador.Name = "rbBorrador";
            rbBorrador.Size = new Size(86, 19);
            rbBorrador.TabIndex = 1;
            rbBorrador.Text = "🗑️ Borrador";
            rbBorrador.UseVisualStyleBackColor = true;
            // 
            // rbCuentagotas
            // 
            rbCuentagotas.AutoSize = true;
            rbCuentagotas.Font = new Font("Segoe UI", 9F);
            rbCuentagotas.Location = new Point(10, 75);
            rbCuentagotas.Name = "rbCuentagotas";
            rbCuentagotas.Size = new Size(104, 19);
            rbCuentagotas.TabIndex = 2;
            rbCuentagotas.Text = "💧 Cuentagotas";
            rbCuentagotas.UseVisualStyleBackColor = true;
            // 
            // rbVaritaMagica
            // 
            rbVaritaMagica.AutoSize = true;
            rbVaritaMagica.Font = new Font("Segoe UI", 9F);
            rbVaritaMagica.Location = new Point(9, 125);
            rbVaritaMagica.Name = "rbVaritaMagica";
            rbVaritaMagica.Size = new Size(96, 19);
            rbVaritaMagica.TabIndex = 3;
            rbVaritaMagica.Text = "Varita Mágica";
            rbVaritaMagica.UseVisualStyleBackColor = true;
            // 
            // rbRelleno
            // 
            rbRelleno.AutoSize = true;
            rbRelleno.Font = new Font("Segoe UI", 9F);
            rbRelleno.Location = new Point(10, 100);
            rbRelleno.Name = "rbRelleno";
            rbRelleno.Size = new Size(73, 19);
            rbRelleno.TabIndex = 3;
            rbRelleno.Text = "\U0001faa3 Relleno";
            rbRelleno.UseVisualStyleBackColor = true;
            // 
            // panelColorActual
            // 
            panelColorActual.BackColor = Color.Black;
            panelColorActual.BorderStyle = BorderStyle.FixedSingle;
            panelColorActual.Location = new Point(2, 330);
            panelColorActual.Name = "panelColorActual";
            panelColorActual.Size = new Size(80, 80);
            panelColorActual.TabIndex = 7;
            // 
            // btnSeleccionarColor
            // 
            btnSeleccionarColor.Location = new Point(88, 330);
            btnSeleccionarColor.Name = "btnSeleccionarColor";
            btnSeleccionarColor.Size = new Size(72, 53);
            btnSeleccionarColor.TabIndex = 8;
            btnSeleccionarColor.Text = "🎨 Color";
            btnSeleccionarColor.UseVisualStyleBackColor = true;
            // 
            // lblInfoPixel
            // 
            lblInfoPixel.BackColor = Color.FromArgb(230, 230, 230);
            lblInfoPixel.BorderStyle = BorderStyle.FixedSingle;
            lblInfoPixel.Font = new Font("Consolas", 8F);
            lblInfoPixel.Location = new Point(2, 420);
            lblInfoPixel.Name = "lblInfoPixel";
            lblInfoPixel.Padding = new Padding(5);
            lblInfoPixel.Size = new Size(158, 78);
            lblInfoPixel.TabIndex = 9;
            lblInfoPixel.Text = "X: -\r\nY: -\r\nRGB: -";
            // 
            // chkMostrarCuadricula
            // 
            chkMostrarCuadricula.AutoSize = true;
            chkMostrarCuadricula.Checked = true;
            chkMostrarCuadricula.CheckState = CheckState.Checked;
            chkMostrarCuadricula.Font = new Font("Segoe UI", 9F);
            chkMostrarCuadricula.Location = new Point(88, 389);
            chkMostrarCuadricula.Name = "chkMostrarCuadricula";
            chkMostrarCuadricula.Size = new Size(83, 19);
            chkMostrarCuadricula.TabIndex = 10;
            chkMostrarCuadricula.Text = "Cuadrícula";
            chkMostrarCuadricula.UseVisualStyleBackColor = true;
            // 
            // btnDeshacerPixeles
            // 
            btnDeshacerPixeles.BackColor = Color.FromArgb(255, 193, 7);
            btnDeshacerPixeles.FlatAppearance.BorderSize = 0;
            btnDeshacerPixeles.FlatStyle = FlatStyle.Flat;
            btnDeshacerPixeles.Font = new Font("Segoe UI", 9F);
            btnDeshacerPixeles.ForeColor = Color.White;
            btnDeshacerPixeles.Location = new Point(2, 501);
            btnDeshacerPixeles.Name = "btnDeshacerPixeles";
            btnDeshacerPixeles.Size = new Size(158, 32);
            btnDeshacerPixeles.TabIndex = 11;
            btnDeshacerPixeles.Text = "↩️ Deshacer";
            btnDeshacerPixeles.UseVisualStyleBackColor = false;
            // 
            // btnLimpiarCanvas
            // 
            btnLimpiarCanvas.BackColor = Color.FromArgb(244, 67, 54);
            btnLimpiarCanvas.FlatAppearance.BorderSize = 0;
            btnLimpiarCanvas.FlatStyle = FlatStyle.Flat;
            btnLimpiarCanvas.Font = new Font("Segoe UI", 9F);
            btnLimpiarCanvas.ForeColor = Color.White;
            btnLimpiarCanvas.Location = new Point(2, 539);
            btnLimpiarCanvas.Name = "btnLimpiarCanvas";
            btnLimpiarCanvas.Size = new Size(158, 32);
            btnLimpiarCanvas.TabIndex = 12;
            btnLimpiarCanvas.Text = "🗑️ Limpiar Todo";
            btnLimpiarCanvas.UseVisualStyleBackColor = false;
            // 
            // btnAplicarEdicionPixeles
            // 
            btnAplicarEdicionPixeles.BackColor = Color.FromArgb(76, 175, 80);
            btnAplicarEdicionPixeles.FlatAppearance.BorderSize = 0;
            btnAplicarEdicionPixeles.FlatStyle = FlatStyle.Flat;
            btnAplicarEdicionPixeles.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnAplicarEdicionPixeles.ForeColor = Color.White;
            btnAplicarEdicionPixeles.Location = new Point(2, 577);
            btnAplicarEdicionPixeles.Name = "btnAplicarEdicionPixeles";
            btnAplicarEdicionPixeles.Size = new Size(158, 32);
            btnAplicarEdicionPixeles.TabIndex = 13;
            btnAplicarEdicionPixeles.Text = "✓ Aplicar Cambios";
            btnAplicarEdicionPixeles.UseVisualStyleBackColor = false;
            // 
            // lblTituloEditor
            // 
            lblTituloEditor.AutoSize = true;
            lblTituloEditor.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            lblTituloEditor.ForeColor = Color.FromArgb(45, 137, 239);
            lblTituloEditor.Location = new Point(6, 3);
            lblTituloEditor.Name = "lblTituloEditor";
            lblTituloEditor.Size = new Size(181, 30);
            lblTituloEditor.TabIndex = 0;
            lblTituloEditor.Text = "Editor de Iconos";
            // 
            // btnCargarIcono
            // 
            btnCargarIcono.BackColor = Color.FromArgb(45, 137, 239);
            btnCargarIcono.FlatAppearance.BorderSize = 0;
            btnCargarIcono.FlatStyle = FlatStyle.Flat;
            btnCargarIcono.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnCargarIcono.ForeColor = Color.White;
            btnCargarIcono.Location = new Point(195, 6);
            btnCargarIcono.Name = "btnCargarIcono";
            btnCargarIcono.Size = new Size(140, 35);
            btnCargarIcono.TabIndex = 1;
            btnCargarIcono.Text = "📂 Cargar Icono";
            btnCargarIcono.UseVisualStyleBackColor = false;
            btnCargarIcono.Click += BtnCargarIcono_Click;
            // 
            // btnExplorarCarpetaIconos
            // 
            btnExplorarCarpetaIconos.BackColor = Color.FromArgb(76, 175, 80);
            btnExplorarCarpetaIconos.FlatAppearance.BorderSize = 0;
            btnExplorarCarpetaIconos.FlatStyle = FlatStyle.Flat;
            btnExplorarCarpetaIconos.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnExplorarCarpetaIconos.ForeColor = Color.White;
            btnExplorarCarpetaIconos.Location = new Point(345, 6);
            btnExplorarCarpetaIconos.Name = "btnExplorarCarpetaIconos";
            btnExplorarCarpetaIconos.Size = new Size(150, 35);
            btnExplorarCarpetaIconos.TabIndex = 2;
            btnExplorarCarpetaIconos.Text = "📁 Explorar Carpeta";
            btnExplorarCarpetaIconos.UseVisualStyleBackColor = false;
            btnExplorarCarpetaIconos.Click += BtnExplorarCarpetaIconos_Click;
            // 
            // lvIconos
            // 
            lvIconos.Font = new Font("Segoe UI", 9F);
            lvIconos.FullRowSelect = true;
            lvIconos.GridLines = true;
            lvIconos.Location = new Point(8, 47);
            lvIconos.Name = "lvIconos";
            lvIconos.Size = new Size(300, 615);
            lvIconos.TabIndex = 3;
            lvIconos.UseCompatibleStateImageBehavior = false;
            lvIconos.View = View.Details;
            lvIconos.SelectedIndexChanged += LvIconos_SelectedIndexChanged;
            // 
            // groupBoxTamañosIcono
            // 
            groupBoxTamañosIcono.Controls.Add(pbIconoEditor);
            groupBoxTamañosIcono.Controls.Add(groupBoxHerramientas);
            groupBoxTamañosIcono.Controls.Add(lblTamañoActual);
            groupBoxTamañosIcono.Controls.Add(pbTamañoSeleccionado);
            groupBoxTamañosIcono.Controls.Add(lstTamañosIcono);
            groupBoxTamañosIcono.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            groupBoxTamañosIcono.Location = new Point(996, 40);
            groupBoxTamañosIcono.Name = "groupBoxTamañosIcono";
            groupBoxTamañosIcono.Size = new Size(279, 622);
            groupBoxTamañosIcono.TabIndex = 5;
            groupBoxTamañosIcono.TabStop = false;
            groupBoxTamañosIcono.Text = "Tamaños Disponibles";
            // 
            // pbIconoEditor
            // 
            pbIconoEditor.BackColor = Color.White;
            pbIconoEditor.Location = new Point(6, 46);
            pbIconoEditor.Name = "pbIconoEditor";
            pbIconoEditor.Size = new Size(126, 141);
            pbIconoEditor.SizeMode = PictureBoxSizeMode.Zoom;
            pbIconoEditor.TabIndex = 1;
            pbIconoEditor.TabStop = false;
            // 
            // groupBoxHerramientas
            // 
            groupBoxHerramientas.Controls.Add(lblValorAjuste);
            groupBoxHerramientas.Controls.Add(trackBarAjuste);
            groupBoxHerramientas.Controls.Add(btnEscalaGrises);
            groupBoxHerramientas.Controls.Add(btnContraste);
            groupBoxHerramientas.Controls.Add(btnBrillo);
            groupBoxHerramientas.Controls.Add(btnEspejo);
            groupBoxHerramientas.Controls.Add(btnRotar);
            groupBoxHerramientas.Controls.Add(btnRedimensionar);
            groupBoxHerramientas.Controls.Add(btnAplicarCambios);
            groupBoxHerramientas.Controls.Add(btnExportarTamaño);
            groupBoxHerramientas.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            groupBoxHerramientas.Location = new Point(7, 339);
            groupBoxHerramientas.Name = "groupBoxHerramientas";
            groupBoxHerramientas.Size = new Size(260, 274);
            groupBoxHerramientas.TabIndex = 6;
            groupBoxHerramientas.TabStop = false;
            groupBoxHerramientas.Text = "Herramientas";
            // 
            // lblValorAjuste
            // 
            lblValorAjuste.AutoSize = true;
            lblValorAjuste.Font = new Font("Segoe UI", 9F);
            lblValorAjuste.Location = new Point(14, 137);
            lblValorAjuste.Name = "lblValorAjuste";
            lblValorAjuste.Size = new Size(52, 15);
            lblValorAjuste.TabIndex = 7;
            lblValorAjuste.Text = "Ajuste: 0";
            // 
            // trackBarAjuste
            // 
            trackBarAjuste.Location = new Point(5, 150);
            trackBarAjuste.Maximum = 100;
            trackBarAjuste.Minimum = -100;
            trackBarAjuste.Name = "trackBarAjuste";
            trackBarAjuste.Size = new Size(249, 45);
            trackBarAjuste.TabIndex = 6;
            trackBarAjuste.TickFrequency = 10;
            // 
            // btnEscalaGrises
            // 
            btnEscalaGrises.Font = new Font("Segoe UI", 9F);
            btnEscalaGrises.Location = new Point(134, 100);
            btnEscalaGrises.Name = "btnEscalaGrises";
            btnEscalaGrises.Size = new Size(120, 35);
            btnEscalaGrises.TabIndex = 5;
            btnEscalaGrises.Text = "⚪ Escala Grises";
            btnEscalaGrises.UseVisualStyleBackColor = true;
            btnEscalaGrises.Click += BtnContraste_Click;
            // 
            // btnContraste
            // 
            btnContraste.Font = new Font("Segoe UI", 9F);
            btnContraste.Location = new Point(4, 100);
            btnContraste.Name = "btnContraste";
            btnContraste.Size = new Size(120, 35);
            btnContraste.TabIndex = 4;
            btnContraste.Text = "🌓 Contraste";
            btnContraste.UseVisualStyleBackColor = true;
            btnContraste.Click += BtnContraste_Click;
            // 
            // btnBrillo
            // 
            btnBrillo.Font = new Font("Segoe UI", 9F);
            btnBrillo.Location = new Point(135, 61);
            btnBrillo.Name = "btnBrillo";
            btnBrillo.Size = new Size(120, 35);
            btnBrillo.TabIndex = 3;
            btnBrillo.Text = "☀️ Brillo";
            btnBrillo.UseVisualStyleBackColor = true;
            btnBrillo.Click += BtnBrillo_Click;
            // 
            // btnEspejo
            // 
            btnEspejo.Font = new Font("Segoe UI", 9F);
            btnEspejo.Location = new Point(5, 61);
            btnEspejo.Name = "btnEspejo";
            btnEspejo.Size = new Size(120, 35);
            btnEspejo.TabIndex = 2;
            btnEspejo.Text = "↔️ Espejo H";
            btnEspejo.UseVisualStyleBackColor = true;
            btnEspejo.Click += BtnEspejo_Click;
            // 
            // btnRotar
            // 
            btnRotar.Font = new Font("Segoe UI", 9F);
            btnRotar.Location = new Point(135, 24);
            btnRotar.Name = "btnRotar";
            btnRotar.Size = new Size(120, 35);
            btnRotar.TabIndex = 1;
            btnRotar.Text = "🔄 Rotar 90°";
            btnRotar.UseVisualStyleBackColor = true;
            btnRotar.Click += BtnRotar_Click;
            // 
            // btnRedimensionar
            // 
            btnRedimensionar.Font = new Font("Segoe UI", 9F);
            btnRedimensionar.Location = new Point(5, 24);
            btnRedimensionar.Name = "btnRedimensionar";
            btnRedimensionar.Size = new Size(120, 35);
            btnRedimensionar.TabIndex = 0;
            btnRedimensionar.Text = "📐 Redimensionar";
            btnRedimensionar.UseVisualStyleBackColor = true;
            btnRedimensionar.Click += BtnRedimensionar_Click;
            // 
            // btnAplicarCambios
            // 
            btnAplicarCambios.BackColor = Color.FromArgb(76, 175, 80);
            btnAplicarCambios.FlatAppearance.BorderSize = 0;
            btnAplicarCambios.FlatStyle = FlatStyle.Flat;
            btnAplicarCambios.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnAplicarCambios.ForeColor = Color.White;
            btnAplicarCambios.Location = new Point(4, 196);
            btnAplicarCambios.Name = "btnAplicarCambios";
            btnAplicarCambios.Size = new Size(250, 34);
            btnAplicarCambios.TabIndex = 8;
            btnAplicarCambios.Text = "✓ Aplicar Cambios";
            btnAplicarCambios.UseVisualStyleBackColor = false;
            // 
            // btnExportarTamaño
            // 
            btnExportarTamaño.BackColor = Color.FromArgb(45, 137, 239);
            btnExportarTamaño.FlatAppearance.BorderSize = 0;
            btnExportarTamaño.FlatStyle = FlatStyle.Flat;
            btnExportarTamaño.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnExportarTamaño.ForeColor = Color.White;
            btnExportarTamaño.Location = new Point(4, 233);
            btnExportarTamaño.Name = "btnExportarTamaño";
            btnExportarTamaño.Size = new Size(250, 34);
            btnExportarTamaño.TabIndex = 9;
            btnExportarTamaño.Text = "💾 Exportar Tamaño";
            btnExportarTamaño.UseVisualStyleBackColor = false;
            // 
            // lblTamañoActual
            // 
            lblTamañoActual.AutoSize = true;
            lblTamañoActual.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblTamañoActual.Location = new Point(6, 20);
            lblTamañoActual.Name = "lblTamañoActual";
            lblTamañoActual.Size = new Size(75, 15);
            lblTamañoActual.TabIndex = 1;
            lblTamañoActual.Text = "Vista Previa:";
            // 
            // pbTamañoSeleccionado
            // 
            pbTamañoSeleccionado.BackColor = Color.White;
            pbTamañoSeleccionado.BorderStyle = BorderStyle.FixedSingle;
            pbTamañoSeleccionado.Location = new Point(138, 46);
            pbTamañoSeleccionado.Name = "pbTamañoSeleccionado";
            pbTamañoSeleccionado.Size = new Size(124, 141);
            pbTamañoSeleccionado.SizeMode = PictureBoxSizeMode.CenterImage;
            pbTamañoSeleccionado.TabIndex = 2;
            pbTamañoSeleccionado.TabStop = false;
            // 
            // lstTamañosIcono
            // 
            lstTamañosIcono.Font = new Font("Segoe UI", 9F);
            lstTamañosIcono.FormattingEnabled = true;
            lstTamañosIcono.Location = new Point(7, 191);
            lstTamañosIcono.Name = "lstTamañosIcono";
            lstTamañosIcono.Size = new Size(255, 124);
            lstTamañosIcono.TabIndex = 0;
            // 
            // panelEdicionPixeles
            // 
            panelEdicionPixeles.AutoScroll = true;
            panelEdicionPixeles.BackColor = Color.FromArgb(245, 245, 245);
            panelEdicionPixeles.BorderStyle = BorderStyle.FixedSingle;
            panelEdicionPixeles.Controls.Add(panelEditorPrincipal);
            panelEdicionPixeles.Location = new Point(325, 47);
            panelEdicionPixeles.Name = "panelEdicionPixeles";
            panelEdicionPixeles.Size = new Size(494, 532);
            panelEdicionPixeles.TabIndex = 20;
            // 
            // panelEditorPrincipal
            // 
            panelEditorPrincipal.BackColor = Color.FromArgb(245, 245, 245);
            panelEditorPrincipal.BorderStyle = BorderStyle.FixedSingle;
            panelEditorPrincipal.Location = new Point(158, 71);
            panelEditorPrincipal.Name = "panelEditorPrincipal";
            panelEditorPrincipal.Size = new Size(129, 83);
            panelEditorPrincipal.TabIndex = 4;
            panelEditorPrincipal.Visible = false;
            // 
            // BtnRestaurarImagen
            // 
            BtnRestaurarImagen.BackColor = Color.FromArgb(45, 137, 239);
            BtnRestaurarImagen.FlatAppearance.BorderSize = 0;
            BtnRestaurarImagen.FlatStyle = FlatStyle.Flat;
            BtnRestaurarImagen.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            BtnRestaurarImagen.ForeColor = Color.White;
            BtnRestaurarImagen.Location = new Point(213, 39);
            BtnRestaurarImagen.Name = "BtnRestaurarImagen";
            BtnRestaurarImagen.Size = new Size(172, 40);
            BtnRestaurarImagen.TabIndex = 1;
            BtnRestaurarImagen.Text = "Restaurar Original";
            BtnRestaurarImagen.UseVisualStyleBackColor = false;
            BtnRestaurarImagen.Click += BtnRestaurarImagen_Click;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1289, 691);
            Controls.Add(tabControl1);
            Font = new Font("Segoe UI", 9F);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            Name = "MainForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "HFA-ICO - Convertidor y Editor de Iconos";
            tabControl1.ResumeLayout(false);
            tabIndividual.ResumeLayout(false);
            tabIndividual.PerformLayout();
            gbHerramientasEdicion.ResumeLayout(false);
            gbHerramientasEdicion.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)trackBarToleranciaIndividual).EndInit();
            ((System.ComponentModel.ISupportInitialize)pbPreview).EndInit();
            ((System.ComponentModel.ISupportInitialize)pbRecorte).EndInit();
            tabLote.ResumeLayout(false);
            tabLote.PerformLayout();
            groupBoxTamañosLote.ResumeLayout(false);
            panelPreviewLote.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)pbPreviewLote).EndInit();
            tabEditor.ResumeLayout(false);
            tabEditor.PerformLayout();
            panelControlsPixeles.ResumeLayout(false);
            panelControlsPixeles.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)trackBarZoom).EndInit();
            groupBoxHerramientasDibujo.ResumeLayout(false);
            groupBoxHerramientasDibujo.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)trackBarTolerancia).EndInit();
            groupBoxTamañosIcono.ResumeLayout(false);
            groupBoxTamañosIcono.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pbIconoEditor).EndInit();
            groupBoxHerramientas.ResumeLayout(false);
            groupBoxHerramientas.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)trackBarAjuste).EndInit();
            ((System.ComponentModel.ISupportInitialize)pbTamañoSeleccionado).EndInit();
            panelEdicionPixeles.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        // ==========================================
        // Variables de Controles
        // ==========================================

        // TabControl
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabIndividual;
        private System.Windows.Forms.TabPage tabLote;
        private System.Windows.Forms.TabPage tabEditor;

        // Tab Individual
        private System.Windows.Forms.Label lblTitulo;
        private System.Windows.Forms.Button btnSeleccionar;
        private System.Windows.Forms.Label lblArchivo;
        private System.Windows.Forms.Label lblDimensiones;
        private System.Windows.Forms.PictureBox pbPreview;
        private System.Windows.Forms.Label lblEstado;
        private System.Windows.Forms.CheckBox chkMantenerProporcion;
        private System.Windows.Forms.Button btnLimpiarSeleccion;
        private System.Windows.Forms.PictureBox pbRecorte;
        private System.Windows.Forms.Label lblRecorte;
        private System.Windows.Forms.Label lblTamaños;
        private System.Windows.Forms.CheckedListBox clbTamaños;
        private HFA_ICO.BotonGlifo btnConvertir;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.TextBox txtLog;
        private System.Windows.Forms.Button btnLimpiarLog;

        // Tab Lote
        private System.Windows.Forms.Label lblTituloLote;
        private System.Windows.Forms.Button btnSeleccionarCarpeta;
        private System.Windows.Forms.Button btnAgregarArchivos;
        private System.Windows.Forms.Label lblCarpetaActual;
        private System.Windows.Forms.ListView lvArchivosLote;
        private System.Windows.Forms.Button btnSeleccionarTodos;
        private System.Windows.Forms.Button btnDeseleccionarTodos;
        private System.Windows.Forms.Button btnLimpiarLista;
        private System.Windows.Forms.Label lblCantidadArchivos;
        private System.Windows.Forms.Label lblSeleccionados;
        private System.Windows.Forms.GroupBox groupBoxTamañosLote;
        private System.Windows.Forms.CheckedListBox clbTamañosLote;
        private System.Windows.Forms.Panel panelPreviewLote;
        private System.Windows.Forms.Label lblPreviewLote;
        private System.Windows.Forms.PictureBox pbPreviewLote;
        private System.Windows.Forms.Button btnConvertirLote;
        private System.Windows.Forms.ProgressBar progressBarLote;
        private System.Windows.Forms.ProgressBar progressBarIndividual;
        private System.Windows.Forms.Label lblProgresoLote;
        private System.Windows.Forms.Label lblArchivoActual;

        // Tab Editor
        private System.Windows.Forms.Label lblTituloEditor;
        private System.Windows.Forms.Button btnCargarIcono;
        private System.Windows.Forms.Button btnExplorarCarpetaIconos;
        private System.Windows.Forms.ListView lvIconos;
        private System.Windows.Forms.Panel panelEditorPrincipal;
        private System.Windows.Forms.PictureBox pbIconoEditor;
        private System.Windows.Forms.Label lblInfoIcono;
        private System.Windows.Forms.GroupBox groupBoxTamañosIcono;
        private System.Windows.Forms.ListBox lstTamañosIcono;
        private System.Windows.Forms.Label lblTamañoActual;
        private System.Windows.Forms.PictureBox pbTamañoSeleccionado;
        private System.Windows.Forms.GroupBox groupBoxHerramientas;
        private System.Windows.Forms.Button btnRedimensionar;
        private System.Windows.Forms.Button btnRotar;
        private System.Windows.Forms.Button btnEspejo;
        private System.Windows.Forms.Button btnBrillo;
        private System.Windows.Forms.Button btnContraste;
        private System.Windows.Forms.Button btnEscalaGrises;
        private System.Windows.Forms.TrackBar trackBarAjuste;
        private System.Windows.Forms.Label lblValorAjuste;
        private System.Windows.Forms.Button btnAplicarCambios;
        private System.Windows.Forms.Button btnExportarTamaño;
        private System.Windows.Forms.Button btnGuardarIcono;
        private System.Windows.Forms.Button btnRevertir;
        // Tab Editor - Nuevos controles para edición por píxeles
        private System.Windows.Forms.Panel panelEdicionPixeles;
        private System.Windows.Forms.Panel panelControlsPixeles;
        private System.Windows.Forms.Label lblZoom;
        private System.Windows.Forms.TrackBar trackBarZoom;
        private System.Windows.Forms.Label lblNivelZoom;
        private System.Windows.Forms.Button btnZoomMas;
        private System.Windows.Forms.Button btnZoomMenos;
        private System.Windows.Forms.Button btnZoomAjustar;
        private System.Windows.Forms.GroupBox groupBoxHerramientasDibujo;
        private System.Windows.Forms.RadioButton rbPincel;
        private System.Windows.Forms.RadioButton rbBorrador;
        private System.Windows.Forms.RadioButton rbCuentagotas;
        private System.Windows.Forms.RadioButton rbRelleno;
        private System.Windows.Forms.Panel panelColorActual;
        private System.Windows.Forms.Button btnSeleccionarColor;
        private System.Windows.Forms.Label lblInfoPixel;
        private System.Windows.Forms.CheckBox chkMostrarCuadricula;
        private System.Windows.Forms.Button btnDeshacerPixeles;
        private System.Windows.Forms.Button btnLimpiarCanvas;
        private System.Windows.Forms.Button btnAplicarEdicionPixeles;
        private RadioButton rbVaritaMagica;
        private TrackBar trackBarTolerancia;
        private Label label2;
        private Label lblNivelTolerancia;
        private Label lblTolerancia;
        private GroupBox gbHerramientasEdicion;
        private CheckBox chkVistaPrevia;
        private TrackBar trackBarToleranciaIndividual;
        private Button btnEliminarFondo;
        private Label lblToleranciaIndividual;
        private Label lblToleranciaIndividal;
        private Button BtnRestaurarImagen;
    }
}