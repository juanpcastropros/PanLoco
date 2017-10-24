using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PanLoco.ViewModels.Perfil
{
    public class PerfilViewModel: BaseViewModel
    {
        public PanLoco.Models.Perfil Item { get; set; }
        public void Save()
        {
            if (isValid())
            {
                App.PerfilDB.SaveItemAsync(Item);
            }
        }
        public PerfilViewModel()
        {
            Item = App.PerfilDB.GetItemAsync(1).Result;
            if(Item==null)
            {
                Item = new Models.Perfil();
            }
        }
        private bool isValid()
        {
            if (Item.Cuil.Length > 0 && Item.Calle.Length > 0 && Item.FullName.Length > 0 && Item.Localidad.Length > 0)
                return true;
            else
                throw new Exception("Todos los campos son obligatorios. Los mismo sale en el ticket.");

        }
    }
}
