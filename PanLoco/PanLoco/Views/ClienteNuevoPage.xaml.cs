using System;

using PanLoco.Models;

using Xamarin.Forms;

namespace PanLoco.Views
{
    public partial class ClienteNuevoPage : ContentPage
    {
        public Cliente Item { get; set; }

        public ClienteNuevoPage()
        {
            try
            {
                InitializeComponent();

                Item = new Cliente
                {
                    //NombreDeFantasia = "This is a nice description"
                };

                BindingContext = this;
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
        public ClienteNuevoPage(Cliente cliente)
        {
            try
            {
                InitializeComponent();

                Item = cliente;
                //this.UIMayorista.On = Item.Mayorista;
                BindingContext = this;
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
                MessagingCenter.Send(this, "AddItem", Item);
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

                await DisplayAlert("Error", messag, "OK");
            }
        }
        async void Close_Clicked(object sender, EventArgs e)
        {
            try
            {
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

                await DisplayAlert("Error", messag, "OK");
            }
        }

        async void Delete_Clicked(object sender, EventArgs e)
        {
            try
            {
                MessagingCenter.Send(this, "DeleteItem", Item);
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

                await DisplayAlert("Error", messag, "OK");
            }
        }
    }
}