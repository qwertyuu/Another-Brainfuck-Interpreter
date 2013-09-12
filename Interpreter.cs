using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace BrainfuckInterpret
{
    public partial class Interpreter : Form
    {
        public Interpreter()
        {
            InitializeComponent();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            _t = new System.Threading.Thread(DoEet);
            _outputs = new StringBuilder();
            _endValues = new Dictionary<int, int>();
            _state = new Stack<int>();
            _timer = new Timer();
            _timer.Tick += timer_Tick;
            _done = false;
            _isSeekingEndOfLoop = false;
            _inputs = input.Text;
            output.Text = string.Empty;
            _t.IsBackground = true;
            _t.Start();
            _timer.Interval = 10;
            _timer.Start();
        }

        void timer_Tick(object sender, EventArgs e)
        {
            lock (_swag)
            {
                if (_done && output.Text == _outputs.ToString())
                {
                    _timer.Stop();
                }
                else
                {
                    output.Text = _outputs.ToString();
                }
            }
        }
        char _lastChar;
        readonly object _swag = new object();
        static Stack<int> _state;
        static Dictionary<int, int> _endValues;
        static int[] _buffer;
        static string _inputs;
        StringBuilder _outputs;
        static int _pointer;
        Timer _timer;
        System.Threading.Thread _t;

        private void DoEet(object obj)
        {
            _buffer = new int[256];
            _pointer = 0;
            _i = 0;
            while (_i < _inputs.Length)
            {
                Think();
                _i++;
            }
            _done = true;
        }
        bool _done;
        static int _layer;
        static int _i;
        static bool _isSeekingEndOfLoop;

        private void Think()
        {
            if (!_isSeekingEndOfLoop)
            {
                switch (_inputs[_i])
                {
                    case '>':
                        _pointer++;
                        if (_pointer > 255)
                        {
                            _pointer = 0;
                        }
                        break;
                    case '<':
                        _pointer--;
                        if (_pointer < 0)
                        {
                            _pointer = 255;
                        }
                        break;
                    case '+':
                        _buffer[_pointer]++;
                        if (_buffer[_pointer] > 255)
                        {
                            _buffer[_pointer] = 0;
                        }
                        break;
                    case '-':
                        _buffer[_pointer]--;
                        if (_buffer[_pointer] < 0)
                        {
                            _buffer[_pointer] = 255;
                        }
                        break;
                    case '.':
                        AppendChar(_outputs, (char)_buffer[_pointer]);
                        break;
                    case ',':
                        new InputPrompt().ShowDialog();
                        _buffer[_pointer] = PromptValue;
                        break;
                    case '[':
                        if (_buffer[_pointer] == 0)
                        {
                            if (!_endValues.ContainsKey(_i - 1))
                            {
                                _isSeekingEndOfLoop = true;
                                _state.Push(_i - 1);
                            }
                            else
                            {
                                _i = _endValues[_i - 1];
                            }
                        }
                        else
                        {
                            _state.Push(_i - 1);
                        }
                        break;
                    case ']':
                        int buf = _state.Pop();
                        if (!_endValues.ContainsKey(buf))
                        {
                            _endValues.Add(buf, _i);
                        }
                        _i = buf;
                        break;
                    case '#':
                        break;
                }
            }
            else
            {
                switch (_inputs[_i])
                {
                    case '[':
                        _layer++;
                        break;
                    case ']':
                        if (_layer > 0)
                        {
                            _layer--;
                        }
                        else
                        {
                            int buf = _state.Pop();
                            _endValues.Add(buf, _i);
                            _isSeekingEndOfLoop = false;
                        }
                        break;
                }
            }
        }
        public void AppendChar(StringBuilder sB, char c)
        {
            lock (_swag)
            {
                sB.Append(c);
            }
        } 
        public static int PromptValue { get; set; }

        private void button2_Click(object sender, EventArgs e)
        {
            var a = new StringBuilder();
            foreach (char j in input.Text)
            {
                switch (j)
                {
                    case '+':
                        if (_lastChar == '-')
                        {
                            Remove(a);
                        }
                        else
                        {
                            a.Append(j);
                        }
                        break;
                    case '-':
                        if (_lastChar == '+')
                        {
                            Remove(a);
                        }
                        else
                        {
                            a.Append(j);
                        }
                        break;
                    case '>':
                        if (_lastChar == '<')
                        {
                            Remove(a);
                        }
                        else
                        {
                            a.Append(j);
                        }
                        break;
                    case '<':
                        if (_lastChar == '>')
                        {
                            Remove(a);
                        }
                        else
                        {
                            a.Append(j);
                        }
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
                }
                _lastChar = (a.Length > 0) ? a[a.Length - 1] : (char)0;
            }
            input.Text = a.ToString();
        }

        private void Remove(StringBuilder a)
        {
            a.Remove(a.Length - 1, 1);
        }
    }
}