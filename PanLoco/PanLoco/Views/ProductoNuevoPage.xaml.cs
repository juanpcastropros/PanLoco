using System;

using PanLoco.Models;

using Xamarin.Forms;
using PanLoco.Helpers;

namespace PanLoco.Views
{
    public partial class ProductoNuevoPage : ContentPage
    {
        ViewModels.Productos.ProductoViewModelCRUD vModel;
        //public Producto Item { get; set; }

        public ProductoNuevoPage()
        {
            try
            {
                BindingContext = vModel = new ViewModels.Productos.ProductoViewModelCRUD();
                vModel.Item = new Producto
                {
                };
                vModel.IsNew = true;
                InitializeComponent();
            }
            catch (Exception ex)
            {
                string messag = "";
                Exception er = ex;
                while (er != null)
                {
                    messag += er.Message + "!!!";
                    er = er.InnerException;
                }

                DisplayAlert("Error", messag, "OK");
            }
        }
        public ProductoNuevoPage(Producto producto)
        {
            try
            {
                BindingContext = vModel = new ViewModels.Productos.ProductoViewModelCRUD();
                vModel.IsNew = false;
                vModel.Item = producto;
                InitializeComponent();
                cta_eliminar.IsEnabled = true;
                cta_agregar.IsEnabled = true;
            }
            catch (Exception ex)
            {
                string messag = "";
                Exception er = ex;
                while (er != null)
                {
                    messag += er.Message + "!!!";
                    er = er.InnerException;
                }
                DisplayAlert("Error", messag, "OK");
            }
        }

        async void Save_Clicked(object sender, EventArgs e)
        {
            try
            {
                cta_agregar.IsEnabled = false;
                string error = string.Empty;
                if (vModel.isValid(out error))
                {
                    cta_agregar.IsEnabled = false;
                    MessagingCenter.Send(this, "AddItem", vModel.Item);
                    await Navigation.PopToRootAsync();
                }
                else
                {
                    await DisplayAlert("Atención", error, "Ok");
                    cta_agregar.IsEnabled = true;
                }
            }
            catch (Exception ex)
            {
                string messag = "";
                Exception er = ex;
                while (er != null)
                {
                    messag += er.Message + "!!!";
                    er = er.InnerException;
                }
                ((Entry)sender).TextColor = Const.TextInvalidColor;
                await DisplayAlert("Error", messag, "OK");
            }
        }

        async void cta_eliminar_Clicked(object sender, EventArgs e)
        {
            try
            {
                this.vModel.EliminarProducto(vModel.Item);
                await Navigation.PopToRootAsync();
            }
            catch (Exception ex)
            {
                string messag = "";
                Exception er = ex;
                while (er != null)
                {
                    messag += er.Message + "!!!";
                    er = er.InnerException;
                }
                ((Entry)sender).TextColor = Const.TextInvalidColor;
                await DisplayAlert("Error", messag, "OK");
            }
        }
    }
}