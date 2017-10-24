using PanLoco.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace PanLoco.Helpers
{
    public class Const
    {
        public static Color TextInvalidColor { get { return Color.Red; } }
        public static Color TextValidColor { get { return Color.Default; } }

        public class Validation
        {

            public static bool IsNumeric(string value, bool AllowDecimal = true)
            {
                bool isValid = false;
                if (AllowDecimal)
                {
                    double result;
                    isValid = double.TryParse(value, out result);
                }
                else
                {
                    long result;
                    isValid = long.TryParse(value, out result);
                }
                return isValid;
            }
            public static bool IsStockValid(EntregaItemVendido iv)
            {

                bool IsValid = true;
                if (iv != null)
                {
                    if (!(iv.Producto.Stock >= iv.CantidadDev + iv.CantidadNor))
                    {
                        IsValid = false;
                    }
                }
                else
                {
                    IsValid = false;
                }
                return IsValid;

            }
        }
    }
}
