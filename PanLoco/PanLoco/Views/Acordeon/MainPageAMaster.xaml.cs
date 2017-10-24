using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PanLoco.Views.Acordeon
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPageAMaster : ContentPage
    {
        public ListView ListView => ListViewMenuItems;

        public MainPageAMaster()
        {
            InitializeComponent();
            BindingContext = new MainPageAMasterViewModel();
        }



        class MainPageAMasterViewModel : INotifyPropertyChanged
        {
            public ObservableCollection<MainPageAMenuItem> MenuItems { get; set; }
            public MainPageAMasterViewModel()
            {
                MenuItems = new ObservableCollection<MainPageAMenuItem>(new[]
                {
                    new MainPageAMenuItem { Id = 0, Title = "Entregas", TargetType=typeof(EntregasListPage)},
                    new MainPageAMenuItem { Id = 0, Title = "Productos", TargetType=typeof(ProductosListPage) },
                    new MainPageAMenuItem { Id = 1, Title = "Clientes", TargetType=typeof(ClientesListPage) },
                    new MainPageAMenuItem { Id = 2, Title = "Configuración", TargetType=typeof(Perfil.PerfilView) }

                });
            }
            public event PropertyChangedEventHandler PropertyChanged;
            void OnPropertyChanged([CallerMemberName]string propertyName = "") =>
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));


        }
    }
}
