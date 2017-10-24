using System;
using PanLoco.DataBase;
using PanLoco.Helpers;
using PanLoco.Views;
using SQLite;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace PanLoco
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            SetMainPage();
        }
        protected override void OnResume()
        {
            base.OnResume();
        }
        protected override void OnSleep()
        {
            base.OnSleep();
        }

        protected override void OnStart()
        {
            base.OnStart();
        }
        static SQLiteAsyncConnection database;
        static ClienteDatabase clientedatabase;
        public static ClienteDatabase ClienteDB
        {
            get
            {
                
                if (clientedatabase == null)
                {
                    InitDataBase();
                    clientedatabase = new ClienteDatabase(database);
                }
                return clientedatabase;
            }
        }

        private static void InitDataBase()
        {
            if(database==null)
                database = new SQLiteAsyncConnection(DependencyService.Get<IFileHelper>().GetLocalFilePath("TodoSQLite.db3"));
        }

        static ProductoDataBase productodatabase;
        public static ProductoDataBase ProductoDB
        {
            get
            {
                
                if (productodatabase == null)
                {
                    InitDataBase();
                    //productodatabase = new ProductoDataBase(DependencyService.Get<IFileHelper>().GetLocalFilePath("TodoSQLite.db3"));
                    productodatabase = new ProductoDataBase(database);
                }
                return productodatabase;
            }
        }
        static EntregaDataBase entregadatabase;
        public static EntregaDataBase EntregaDB
        {
            get
            {
                if (entregadatabase == null)
                {
                    InitDataBase();
                    //entregadatabase = new EntregaDataBase(DependencyService.Get<IFileHelper>().GetLocalFilePath("TodoSQLite.db3"));
                    entregadatabase = new EntregaDataBase(database);
                }
                return entregadatabase;
            }
        }
        static PerfilDataBase perfildatabase;
        public static PerfilDataBase PerfilDB
        {
            get
            {
                if (perfildatabase == null)
                {
                    InitDataBase();
                    //entregadatabase = new EntregaDataBase(DependencyService.Get<IFileHelper>().GetLocalFilePath("TodoSQLite.db3"));
                    perfildatabase = new PerfilDataBase(database);
                }
                return perfildatabase;
            }
        }

        public static void SetMainPage()
        {
            //Current.MainPage = new TabbedPage
            //{
            //    BarBackgroundColor=Color.FromHex("#ACACAC"),
            //    Children =
            //    {
            //        new NavigationPage(new ProductosListPage())
            //        {
            //            Title = "Productos",
            //            Icon = Device.OnPlatform("tab_feed.png",null,null)
            //        },
            //        new NavigationPage(new ClientesListPage())
            //        {
            //            Title = "Clientes",
            //            Icon = Device.OnPlatform("tab_feed.png",null,null)
            //        },
            //        new NavigationPage(new EntregasListPage())
            //        {
            //            Title = "Entregas",
            //            Icon = Device.OnPlatform("tab_feed.png",null,null)
            //        },
            //        new NavigationPage(new AboutPage())
            //        {
            //            Title = "About",
            //            Icon = Device.OnPlatform("tab_about.png",null,null)
            //        },
            //    }
            //};
            Current.MainPage = new Views.Acordeon.MainPageA();
        }
    }
}
