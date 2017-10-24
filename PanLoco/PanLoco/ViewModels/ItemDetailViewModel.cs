using PanLoco.Models;

namespace PanLoco.ViewModels
{
    public class ItemDetailViewModel : BaseViewModel
    {
        public Cliente Item { get; set; }
        public ItemDetailViewModel(Cliente item = null)
        {
            Title = item.NombreDeFantasia;
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