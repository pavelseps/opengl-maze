using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OpenGlMaze
{
    public partial class Form2 : Form
    {
        private App _app;
        private string _mazeUrl;
        internal App App { get => _app; set => _app = value; }

        public Form2()
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {

        }

        private void mazeFile_Click(object sender, EventArgs e)
        {
            if (mazeFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                _mazeUrl = mazeFileDialog.FileName;
                text.Text = "Bludiště nahráno. Můžete spustit hru.";
                start.Visible = true;
            }
        }

        private void start_Click(object sender, EventArgs e)
        {
            _app.Start(_mazeUrl);
        }

        public void Finished(TimeSpan timeDiff)
        {
            text.Text = $"Vyhrál jste v čase {timeDiff.Minutes}:{timeDiff.Seconds}! Můžete nahrát další bludiště.";
            start.Visible = false;
        }
    }
}
