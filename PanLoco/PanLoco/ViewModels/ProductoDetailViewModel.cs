using PanLoco.Models;

namespace PanLoco.ViewModels
{
    public class ProductoDetailViewModel : BaseViewModel
    {
        public Producto Item { get; set; }
        public ProductoDetailViewModel(Producto item = null)
        {
            Title = item.Nombre;
            Item = item;
        }

        int quantity = 1;
        public int Quantity
        {
            get { return quantity; }
            set { SetProperty(ref quantity, value); }
        }
    }
}