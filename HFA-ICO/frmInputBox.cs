using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.ComponentModel;

namespace HFA_ICO
{
    public partial class frmInputBox : Form
    {
        // Fields
        private Color primaryColor = Color.CornflowerBlue;
        private int borderSize = 2;
        public string InputText { get; private set; }

        // Properties
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]

        // Properties
        public Color PrimaryColor
        {
            get { return primaryColor; }
            set
            {
                primaryColor = value;
                this.BackColor = primaryColor;
                this.panelTitleBar.BackColor = primaryColor;
            }
        }

        // Constructors
        public frmInputBox(string prompt)
        {
            InitializeComponent();
            InitializeItems();
            this.PrimaryColor = primaryColor;
            this.labelPrompt.Text = prompt;
            this.labelCaption.Text = "";
            this.textBoxInput.Text = "";
            SetFormSize();
        }

        public frmInputBox(string prompt, string caption)
        {
            InitializeComponent();
            InitializeItems();
            this.PrimaryColor = primaryColor;
            this.labelPrompt.Text = prompt;
            this.labelCaption.Text = caption;
            this.textBoxInput.Text = "";
            SetFormSize();
        }

        public frmInputBox(string prompt, string caption, string defaultResponse)
        {
            InitializeComponent();
            InitializeItems();
            this.PrimaryColor = primaryColor;
            this.labelPrompt.Text = prompt;
            this.labelCaption.Text = caption;
            this.textBoxInput.Text = defaultResponse;
            SetFormSize();
        }

        // Private Methods
        private void InitializeItems()
        {
            this.FormBorderStyle = FormBorderStyle.None;
            this.Padding = new Padding(borderSize);
            this.labelPrompt.MaximumSize = new Size(550, 0);
            this.btnClose.DialogResult = DialogResult.Cancel;
            this.buttonOK.DialogResult = DialogResult.OK;
            this.buttonCancel.DialogResult = DialogResult.Cancel;
            this.textBoxInput.Text = "";
            this.AcceptButton = buttonOK;
            this.CancelButton = buttonCancel;
        }

        private void SetFormSize()
        {
            int width = Math.Max(this.labelPrompt.Width, this.textBoxInput.Width) + this.pictureBoxIcon.Width + this.panelBody.Padding.Left + 20;
            int height = this.panelTitleBar.Height + this.labelPrompt.Height + this.textBoxInput.Height + this.panelButtons.Height + this.panelBody.Padding.Top + 20;
            this.Size = new Size(width, height);
        }

        // Events
        private void btnClose_Click(object sender, EventArgs e)
        {
            InputText = "";
            this.Close();
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            InputText = textBoxInput.Text;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            InputText = "";
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        [DllImport("user32.DLL", EntryPoint = "SendMessage")]
        private extern static void SendMessage(System.IntPtr hWnd, int wMsg, int wParam, int lParam);
        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();

        private void panelTitleBar_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
        }
    }
}