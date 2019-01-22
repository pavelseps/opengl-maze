using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OpenGlMaze
{
    static class Program
    {
        /// <summary>
        /// Hlavní vstupní bod aplikace.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            App app = new App();
            Form2 settingsForm = new Form2();
            Form1 mazeForm = new Form1(); 

            mazeForm.App = app;
            settingsForm.App = app;
            app.Game = mazeForm;
            app.Settings = settingsForm;

            mazeForm.Hide();
            settingsForm.Show();

            while (!mazeForm.IsDisposed && !settingsForm.IsDisposed)
            {
                if (mazeForm.Visible)
                {
                    mazeForm.Draw();
                }
                Application.DoEvents();
            }
        }
    }
}
