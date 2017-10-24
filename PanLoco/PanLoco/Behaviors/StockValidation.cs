using PanLoco.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace PanLoco.Behaviors
{
    public class StockValidation: Behavior<Entry>
    {
        protected override void OnAttachedTo(Entry bindable)
        {
            bindable.TextChanged += HandleTextChanged;
            base.OnAttachedTo(bindable);
        }

        void HandleTextChanged(object sender, TextChangedEventArgs e)
        {
            bool IsValid = true;
            Entry ety = sender as Entry;
            EntregaItemVendido iv = ety.BindingContext as EntregaItemVendido;
            if (iv != null)
            {
                if (!(iv.Producto.Stock >= iv.CantidadDev + iv.CantidadNor ))
                {
                    IsValid = false;
                }
            }
            else
            {
                IsValid= false;
            }
            ((EntregaItemVendido)ety.BindingContext).IsValid = IsValid;
            ((Entry)sender).TextColor = IsValid ? PanLoco.Helpers.Const.TextValidColor : PanLoco.Helpers.Const.TextInvalidColor;
        }

        protected override void OnDetachingFrom(Entry bindable)
        {
            bindable.TextChanged -= HandleTextChanged;
            base.OnDetachingFrom(bindable);

        }
    }
}
