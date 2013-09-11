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
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            t = new System.Threading.Thread(new System.Threading.ParameterizedThreadStart(DoEet));
            outputs = new StringBuilder();
            endValues = new Dictionary<int, int>();
            state = new Stack<int>();
            timer = new Timer();
            timer.Tick += timer_Tick;
            done = false;
            isSeekingEndOfLoop = false;
            inputs = input.Text;
            output.Text = string.Empty;
            t.IsBackground = true;
            t.Start();
            timer.Interval = 10;
            timer.Start();
        }

        void timer_Tick(object sender, EventArgs e)
        {
            lock (swag)
            {
                if (done && output.Text == outputs.ToString())
                {
                    timer.Stop();
                }
                else
                {
                    output.Text = outputs.ToString();
                }
            }
        }
        object swag = new object();
        static Stack<int> state;
        static Dictionary<int, int> endValues;
        static int[] buffer;
        static string inputs;
        StringBuilder outputs;
        static int pointer;
        Timer timer;
        System.Threading.Thread t;

        private void DoEet(object obj)
        {
            buffer = new int[256];
            pointer = 0;
            i = 0;
            while (i < inputs.Length)
            {
                Think();
                i++;
            }
            done = true;
        }
        bool done;
        static int layer = 0;
        static int i;
        static bool isSeekingEndOfLoop;

        private void Think()
        {
            if (!isSeekingEndOfLoop)
            {
                switch (inputs[i])
                {
                    case '>':
                        pointer++;
                        if (pointer > 255)
                        {
                            pointer = 0;
                        }
                        break;
                    case '<':
                        pointer--;
                        if (pointer < 0)
                        {
                            pointer = 255;
                        }
                        break;
                    case '+':
                        buffer[pointer]++;
                        if (buffer[pointer] > 255)
                        {
                            buffer[pointer] = 0;
                        }
                        break;
                    case '-':
                        buffer[pointer]--;
                        if (buffer[pointer] < 0)
                        {
                            buffer[pointer] = 255;
                        }
                        break;
                    case '.':
                        AppendChar(outputs, (char)buffer[pointer]);
                        break;
                    case ',':
                        new inputPrompt().ShowDialog();
                        buffer[pointer] = PromptValue;
                        break;
                    case '[':
                        if (buffer[pointer] == 0)
                        {
                            if (!endValues.ContainsKey(i - 1))
                            {
                                isSeekingEndOfLoop = true;
                                state.Push(i - 1);
                            }
                            else
                            {
                                i = endValues[i - 1];
                            }
                        }
                        else
                        {
                            state.Push(i - 1);
                        }
                        break;
                    case ']':
                        int buf = state.Pop();
                        if (!endValues.ContainsKey(buf))
                        {
                            endValues.Add(buf, i);
                        }
                        i = buf;
                        break;
                    case '#':
                        break;
                    default:
                        break;
                }
            }
            else
            {
                switch (inputs[i])
                {
                    case '[':
                        layer++;
                        break;
                    case ']':
                        if (layer > 0)
                        {
                            layer--;
                        }
                        else
                        {
                            int buf = state.Pop();
                            endValues.Add(buf, i);
                            isSeekingEndOfLoop = false;
                        }
                        break;
                    default:
                        break;
                }
            }
        }
        public void AppendChar(StringBuilder sB, char c)
        {
            lock (swag)
            {
                sB.Append(c);
            }
        } 
        public static int PromptValue { get; set; }

        private void button2_Click(object sender, EventArgs e)
        {
            StringBuilder a = new StringBuilder();
            foreach (char j in input.Text)
            {
                switch (j)
                {
                    case '+':
                        a.Append(j);
                        break;
                    case '-':
                        a.Append(j);
                        break;
                    case '>':
                        a.Append(j);
                        break;
                    case '<':
                        a.Append(j);
                        break;
                    case '[':
                        a.Append(j);
                        break;
                    case ']':
                        a.Append(j);
                        break;
                    case '.':
                        a.Append(j);
                        break;
                    case ',':
                        a.Append(j);
                        break;
                    default:
                        break;
                }
            }
            input.Text = a.ToString();
        }
    }
}