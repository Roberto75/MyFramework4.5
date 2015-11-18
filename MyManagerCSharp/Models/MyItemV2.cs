using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyManagerCSharp.Models
{
    public class MyItemV2
    {
        private int? _iValue = null;
        private float? _fValue = null;
        private decimal? _dValue = null;
        private string _sValue = "";


        public string getValue()
        {
            if (_iValue != null)
            {
                return _iValue.ToString().Replace(",",".") ;
            }
            if (_dValue != null)
            {
                return _dValue.ToString().Replace(",", ".");
            }
            return _sValue;
        }

        public string getStringValue()
        {
              return _sValue;
        }

        public int getIntValue()
        {
            return (int)_iValue;
        }

        public decimal getDecimalValue()
        {
            return (decimal)_dValue;
        }

        public object Value
        {
            set
            {
                if (value.GetType() == typeof(string))
                {
                    _sValue = value.ToString() ;
                }
                else if (value.GetType() == typeof(int))
                {
                    _iValue = (int) value;
                }
                else if (value.GetType() == typeof(decimal))
                {
                    _dValue = (decimal)value;
                }
                else
                {
                    throw new MyException("Tipo di dato non supportato: " + value.GetType().Name);
                }


            }
        }

        public string Text { get; set; }


        public MyItemV2()
        {

        }

        public MyItemV2(string text, string value)
        {
            _sValue = value;
            Text = text;
        }

        public MyItemV2(string text, int value)
        {
            _iValue = value;
            Text = text;
        }

        public MyItemV2(string text, decimal value)
        {
            _dValue = value;
            Text = text;
        }

    }
}
