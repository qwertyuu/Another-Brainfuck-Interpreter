using System;
using System.Windows.Forms;

namespace BrainfuckInterpret
{
    public partial class InputPrompt : Form
    {
        public InputPrompt()
        {
            InitializeComponent();
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter) return;
            if (radioButton1.Checked)
            {
                Interpreter.PromptValue = textBox1.Text == string.Empty ? 0 : textBox1.Text[0];
            }
            else
            {
                if (char.IsDigit(textBox1.Text[0]))
                {
                    Interpreter.PromptValue = Convert.ToInt32(textBox1.Text[0].ToString());
                }
            }
            Close();
        }
    }
}
