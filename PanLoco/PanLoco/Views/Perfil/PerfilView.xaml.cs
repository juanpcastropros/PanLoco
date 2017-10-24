using PanLoco.ViewModels.Perfil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PanLoco.Views.Perfil
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PerfilView : ContentPage
    {
        PerfilViewModel vmodel;
        public PerfilView()
        {
            try
            {
                BindingContext = vmodel = new PerfilViewModel();
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

        private void ToolbarItem_Clicked(object sender, EventArgs e)
        {
            try
            {
                vmodel.Save();

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

        
    }
}