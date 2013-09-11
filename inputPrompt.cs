using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            if (e.KeyCode == Keys.Enter)
            {
                if (radioButton1.Checked)
                {
                    if (textBox1.Text == string.Empty)
                    {
                        Interpreter.PromptValue = 0;
                    }
                    else
                    {
                        Interpreter.PromptValue = textBox1.Text[0];
                    }
                }
                else
                {
                    if (char.IsDigit(textBox1.Text[0]))
                    {
                        Interpreter.PromptValue = Convert.ToInt32(textBox1.Text[0].ToString());
                    }
                }
                this.Close();
            }
        }
    }
}
