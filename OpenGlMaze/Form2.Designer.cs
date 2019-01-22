namespace OpenGlMaze
{
    partial class Form2
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.mazeFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.mazeFile = new System.Windows.Forms.Button();
            this.start = new System.Windows.Forms.Button();
            this.text = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // mazeFileDialog
            // 
            this.mazeFileDialog.FileName = "mazeFileDialog";
            // 
            // mazeFile
            // 
            this.mazeFile.Location = new System.Drawing.Point(31, 96);
            this.mazeFile.Name = "mazeFile";
            this.mazeFile.Size = new System.Drawing.Size(141, 23);
            this.mazeFile.TabIndex = 0;
            this.mazeFile.Text = "Vyberte bludiště.";
            this.mazeFile.UseVisualStyleBackColor = true;
            this.mazeFile.Click += new System.EventHandler(this.mazeFile_Click);
            // 
            // start
            // 
            this.start.Location = new System.Drawing.Point(83, 183);
            this.start.Name = "start";
            this.start.Size = new System.Drawing.Size(201, 82);
            this.start.TabIndex = 1;
            this.start.Text = "Start";
            this.start.UseVisualStyleBackColor = true;
            this.start.Visible = false;
            this.start.Click += new System.EventHandler(this.start_Click);
            // 
            // text
            // 
            this.text.AutoSize = true;
            this.text.Location = new System.Drawing.Point(28, 57);
            this.text.Name = "text";
            this.text.Size = new System.Drawing.Size(121, 13);
            this.text.TabIndex = 2;
            this.text.Text = "Nahrajte prosím bludiště";
            // 
            // Form2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(376, 302);
            this.Controls.Add(this.text);
            this.Controls.Add(this.start);
            this.Controls.Add(this.mazeFile);
            this.Name = "settings";
            this.Text = "Nastavení";
            this.Load += new System.EventHandler(this.Form2_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.OpenFileDialog mazeFileDialog;
        private System.Windows.Forms.Button mazeFile;
        private System.Windows.Forms.Button start;
        private System.Windows.Forms.Label text;
    }
}