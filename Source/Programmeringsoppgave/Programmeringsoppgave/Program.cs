using System;
using System.Windows.Forms;

namespace Programmeringsoppgave
{
    static class Program
    {
        /// <summary>
        /// A program that reads a file of shapes and finds the smallest bounding box. 
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            string filePath = null;
            using (OpenFileDialog diag = new OpenFileDialog())
            {
                diag.InitialDirectory = "c:\\";
                diag.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
                diag.FilterIndex = 1;
                if (diag.ShowDialog() == DialogResult.OK)
                {
                    filePath = diag.FileName;
                }
            }

            if(filePath != null)
            {
                Application.Run(new DrawForm(filePath));
            }
        }
    }
}
