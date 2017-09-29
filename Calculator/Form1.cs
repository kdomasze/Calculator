using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections;

namespace Calculator
{
    public partial class Calculator : Form
    {
        private List<string> _values = new List<string>();
        private int _currentValue;
        private bool _decimalMode;

        public Calculator()
        {
            InitializeComponent();
            ResetLists();
        }

        public void DecimalEvent(object sender, EventArgs e)
        {
            InsertDecimal();
        }

        public void NumberButtonEvent(object sender, EventArgs e)
        {
            InsertNumber(((Button)sender).Text);
        }

        public void PlusMinusEvent(object sender, EventArgs e)
        {
            ToggleSign();
        }
        private void ResetLists()
        {
            _values.Clear();
            _values.Add("0");

            _currentValue = 0;
            _decimalMode = false;

            Output.Text = "0";
        }

        private void InsertDecimal()
        {
            // check if we already added a decimal so we don't add more than one
            if (Output.Text.Length < 16 && !_decimalMode)
            {
                _values[_currentValue] += ".";
                Output.Text = _values[_currentValue];

                _decimalMode = true;
            }
        }

        private void InsertNumber(string buttonText)
        {
            if (_values[_currentValue] == "0")
            {
                // if the number is 0, we want to leave it as that and not add additional 0s
                if (buttonText == "0")
                {
                    return;
                }

                // if we have 0 already and insert a different number, we want to replace it entirely
                _values[_currentValue] = "";
            }

            if (Output.Text.Length < 17)
            {
                _values[_currentValue] += buttonText;
            }

            Output.Text = _values[_currentValue];
        }

        private void ToggleSign()
        {
            // if the current number is 0, we do not want to toggle the sign
            if (_values[_currentValue] == "0")
            {
                return;
            }
            if (_values[_currentValue].StartsWith("-"))
            {
                // removes the negative sign
                _values[_currentValue] = _values[_currentValue].Remove(0, 1);
            }
            else
            {
                // adds the negative sign
                _values[_currentValue] = "-" + _values[_currentValue];
            }

            Output.Text = _values[_currentValue];
        }
    }
}
