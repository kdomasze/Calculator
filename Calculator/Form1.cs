﻿using System;
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
        private List<Operation> _operations = new List<Operation>();
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

        public void BackspaceEvent(object sender, EventArgs e)
        {
            RemoveSymbol();
        }

        public void OperationEvent(object sender, EventArgs e)
        {
            AddOperation(((Button)sender).Name);
        }

        public void EqualsEvent(object sender, EventArgs e)
        {
            OutputResult();
        }

        public void ClearEverythingEvent(object sender, EventArgs e)
        {
            ResetLists();
        }

        public void ClearEntryEvent(object sender, EventArgs e)
        {
            _values[_currentValue] = "0";
            _decimalMode = false;
            
            Output.Text = _values[_currentValue];
        }

        private void ResetLists()
        {
            _values.Clear();
            _values.Add("0");

            _operations.Clear();
            _operations.Add(Operation.Null);

            _currentValue = 0;
            _decimalMode = false;

            Output.Text = _values[_currentValue];
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

        private void RemoveSymbol()
        {
            if (_values[_currentValue] == "0")
            {
                return;
            }

            if (_values[_currentValue].EndsWith("."))
            {
                _decimalMode = false;
                _values[_currentValue] = _values[_currentValue].Remove(_values[_currentValue].Length - 1, 1);
            }
            else if (_values[_currentValue].Length == 2 && _values[_currentValue].StartsWith("-"))
            {
                _values[_currentValue] = "0";
            }
            else if (_values[_currentValue].Length == 1)
            {
                _values[_currentValue] = "0";
            }
            else
            {
                _values[_currentValue] = _values[_currentValue].Remove(_values[_currentValue].Length - 1, 1);
            }

            Output.Text = _values[_currentValue];
        }

        private void AddOperation(string name)
        {
            switch (name)
            {
                case "buttonDivision":
                    _operations.Add(Operation.Division);
                    break;
                case "buttonAddition":
                    _operations.Add(Operation.Addition);
                    break;
                case "buttonMultiplication":
                    _operations.Add(Operation.Multiplication);
                    break;
                case "buttonSubtraction":
                    _operations.Add(Operation.Subtraction);
                    break;
            }

            _values.Add("0");
            _currentValue++;
            _decimalMode = false;

            Output.Text = _values[_currentValue];
        }

        private void OutputResult()
        {
            double result = 0;
            int operationIndex = 0;

            foreach (var value in _values)
            {
                double parse;

                if (!double.TryParse(value, out parse))
                {
                    throw new ApplicationException("Failed to parse value: " + value);
                }

                switch (_operations[operationIndex])
                {
                    case Operation.Null:
                        result = parse;
                        break;
                    case Operation.Division:
                        result /= parse;
                        break;
                    case Operation.Addition:
                        result += parse;
                        break;
                    case Operation.Multiplication:
                        result *= parse;
                        break;
                    case Operation.Subtraction:
                        result -= parse;
                        break;
                }

                operationIndex++;
            }

            ResetLists();
            _values[_currentValue] = result.ToString();
            Output.Text = result.ToString();
        }

        public enum Operation
        {
            Addition, Subtraction, Multiplication, Division, Null
        }
    }
}
