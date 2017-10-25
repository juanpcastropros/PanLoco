using PanLoco.Helpers;
using PanLoco.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PanLoco.Views
{

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddProduct : ContentPage
    {
        EntregaItemVendido item = new EntregaItemVendido();
        public AddProduct()
        {
            InitializeComponent();
            BindingContext = new AddProductViewModel();
        }

        private void Entry_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (e.NewTextValue.Trim().Length == 3)
                {
                    var prod = App.ProductoDB.GetItemByCode(e.NewTextValue.Trim()).Result;
                    if (prod != null)
                    {
                        item.Producto = prod;
                        item.PrecioUnitario = prod.PrecioUnitario;
                        ProductoSelected.Text = string.Concat(item.Producto.Codigo, " - ", item.Producto.Nombre, " - $ ", item.PrecioUnitario);
                    }
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
                DisplayAlert("Error", messag, "OK");
            }
        }

        private void Cantidad_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (item != null)
                {
                    item.Cantidad = int.Parse(e.NewTextValue);
                    SubTotal.Text = string.Concat("$ ", item.SubTotal);
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
                DisplayAlert("Error", messag, "OK");
            }
        }


        async void Agregar_Clicked(object sender, EventArgs e)
        {
            try
            {
                MessagingCenter.Send(this, "AddItemVendido", item);

                await Navigation.PopModalAsync();//.PopToRootAsync();
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

        async void Cerrar_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }
    }

    class AddProductViewModel : INotifyPropertyChanged
    {

        public AddProductViewModel()
        {
            IncreaseCountCommand = new Command(IncreaseCount);
        }

        int count;

        string countDisplay = "You clicked 0 times.";
        public string CountDisplay
        {
            get { return countDisplay; }
            set { countDisplay = value; OnPropertyChanged(); }
        }

        public ICommand IncreaseCountCommand { get; }

        void IncreaseCount() =>
            CountDisplay = $"You clicked {++count} times";


        public event PropertyChangedEventHandler PropertyChanged;
        void OnPropertyChanged([CallerMemberName]string propertyName = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

    }
}
