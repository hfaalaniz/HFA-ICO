namespace HFA_ICO
{
    partial class frmInputBox
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            panelTitleBar = new Panel();
            labelCaption = new Label();
            btnClose = new Button();
            panelButtons = new Panel();
            buttonCancel = new Button();
            buttonOK = new Button();
            panelBody = new Panel();
            textBoxInput = new TextBox();
            labelPrompt = new Label();
            pictureBoxIcon = new PictureBox();
            panelTitleBar.SuspendLayout();
            panelButtons.SuspendLayout();
            panelBody.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBoxIcon).BeginInit();
            SuspendLayout();
            // 
            // panelTitleBar
            // 
            panelTitleBar.BackColor = Color.CornflowerBlue;
            panelTitleBar.Controls.Add(labelCaption);
            panelTitleBar.Controls.Add(btnClose);
            panelTitleBar.Dock = DockStyle.Top;
            panelTitleBar.Location = new Point(2, 2);
            panelTitleBar.Margin = new Padding(4, 3, 4, 3);
            panelTitleBar.Name = "panelTitleBar";
            // Controls.SetChildIndex(TitleBar, 0); = new Size(404, 40);
            panelTitleBar.TabIndex = 0;
            panelTitleBar.MouseDown += panelTitleBar_MouseDown;
            // 
            // labelCaption
            // 
            labelCaption.AutoSize = true;
            labelCaption.Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            labelCaption.ForeColor = Color.White;
            labelCaption.Location = new Point(10, 9);
            labelCaption.Margin = new Padding(4, 0, 4, 0);
            labelCaption.Name = "labelCaption";
            labelCaption.Size = new Size(86, 17);
            labelCaption.TabIndex = 4;
            labelCaption.Text = "labelCaption";
            // 
            // btnClose
            // 
            btnClose.Dock = DockStyle.Right;
            btnClose.FlatAppearance.BorderSize = 0;
            btnClose.FlatAppearance.MouseOverBackColor = Color.FromArgb(224, 79, 95);
            btnClose.FlatStyle = FlatStyle.Flat;
            btnClose.Font = new Font("Microsoft Sans Serif", 13F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnClose.ForeColor = Color.White;
            btnClose.Location = new Point(357, 0);
            btnClose.Margin = new Padding(4, 3, 4, 3);
            btnClose.Name = "btnClose";
            btnClose.Size = new Size(47, 40);
            btnClose.TabIndex = 3;
            btnClose.Text = "X";
            btnClose.UseVisualStyleBackColor = false;
            btnClose.Click += btnClose_Click;
            // 
            // panelButtons
            // 
            panelButtons.BackColor = Color.FromArgb(235, 235, 235);
            panelButtons.Controls.Add(buttonCancel);
            panelButtons.Controls.Add(buttonOK);
            panelButtons.Dock = DockStyle.Bottom;
            panelButtons.Location = new Point(2, 125);
            panelButtons.Margin = new Padding(4, 3, 4, 3);
            panelButtons.Name = "panelButtons";
            panelButtons.Size = new Size(404, 69);
            panelButtons.TabIndex = 1;
            // 
            // buttonCancel
            // 
            buttonCancel.BackColor = Color.DimGray;
            buttonCancel.FlatAppearance.BorderSize = 0;
            buttonCancel.FlatStyle = FlatStyle.Flat;
            buttonCancel.Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            buttonCancel.ForeColor = Color.WhiteSmoke;
            buttonCancel.Location = new Point(270, 14);
            buttonCancel.Margin = new Padding(4, 3, 4, 3);
            buttonCancel.Name = "buttonCancel";
            buttonCancel.Size = new Size(117, 40);
            buttonCancel.TabIndex = 2;
            buttonCancel.Text = "Cancelar";
            buttonCancel.UseVisualStyleBackColor = false;
            buttonCancel.Click += buttonCancel_Click;
            // 
            // buttonOK
            // 
            buttonOK.BackColor = Color.SeaGreen;
            buttonOK.FlatAppearance.BorderSize = 0;
            buttonOK.FlatStyle = FlatStyle.Flat;
            buttonOK.Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            buttonOK.ForeColor = Color.WhiteSmoke;
            buttonOK.Location = new Point(146, 14);
            buttonOK.Margin = new Padding(4, 3, 4, 3);
            buttonOK.Name = "buttonOK";
            buttonOK.Size = new Size(117, 40);
            buttonOK.TabIndex = 1;
            buttonOK.Text = "Aceptar";
            buttonOK.UseVisualStyleBackColor = false;
            buttonOK.Click += buttonOK_Click;
            // 
            // panelBody
            // 
            panelBody.BackColor = Color.WhiteSmoke;
            panelBody.Controls.Add(textBoxInput);
            panelBody.Controls.Add(labelPrompt);
            panelBody.Controls.Add(pictureBoxIcon);
            panelBody.Dock = DockStyle.Fill;
            panelBody.Location = new Point(2, 42);
            panelBody.Margin = new Padding(4, 3, 4, 3);
            panelBody.Name = "panelBody";
            panelBody.Padding = new Padding(12, 12, 0, 0);
            panelBody.Size = new Size(404, 83);
            panelBody.TabIndex = 2;
            // 
            // textBoxInput
            // 
            textBoxInput.Location = new Point(58, 43);
            textBoxInput.Margin = new Padding(4, 3, 4, 3);
            textBoxInput.Name = "textBoxInput";
            textBoxInput.Size = new Size(333, 23);
            textBoxInput.TabIndex = 2;
            // 
            // labelPrompt
            // 
            labelPrompt.AutoSize = true;
            labelPrompt.Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            labelPrompt.ForeColor = Color.FromArgb(85, 85, 85);
            labelPrompt.Location = new Point(58, 12);
            labelPrompt.Margin = new Padding(4, 0, 4, 0);
            labelPrompt.MaximumSize = new Size(700, 0);
            labelPrompt.Name = "labelPrompt";
            labelPrompt.Padding = new Padding(6, 6, 12, 17);
            labelPrompt.Size = new Size(101, 40);
            labelPrompt.TabIndex = 1;
            labelPrompt.Text = "labelPrompt";
            labelPrompt.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // pictureBoxIcon
            // 
            pictureBoxIcon.Dock = DockStyle.Left;
            pictureBoxIcon.Image = Properties.Resources.chat;
            pictureBoxIcon.Location = new Point(12, 12);
            pictureBoxIcon.Margin = new Padding(4, 3, 4, 3);
            pictureBoxIcon.Name = "pictureBoxIcon";
            pictureBoxIcon.Size = new Size(47, 71);
            pictureBoxIcon.TabIndex = 0;
            pictureBoxIcon.TabStop = false;
            // 
            // frmInputBox
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.CornflowerBlue;
            ClientSize = new Size(408, 196);
            Controls.Add(panelBody);
            Controls.Add(panelButtons);
            Controls.Add(panelTitleBar);
            Margin = new Padding(4, 3, 4, 3);
            MinimumSize = new Size(406, 190);
            Name = "frmInputBox";
            Padding = new Padding(2);
            StartPosition = FormStartPosition.CenterParent;
            Text = "InputBox";
            panelTitleBar.ResumeLayout(false);
            panelTitleBar.PerformLayout();
            panelButtons.ResumeLayout(false);
            panelBody.ResumeLayout(false);
            panelBody.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBoxIcon).EndInit();
            ResumeLayout(false);
        }

        private System.Windows.Forms.Panel panelTitleBar;
        private System.Windows.Forms.Label labelCaption;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Panel panelButtons;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.Panel panelBody;
        private System.Windows.Forms.TextBox textBoxInput;
        private System.Windows.Forms.Label labelPrompt;
        private System.Windows.Forms.PictureBox pictureBoxIcon;
    }
}