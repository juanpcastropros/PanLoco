using PanLoco.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace PanLoco.ViewModels.Productos
{
    public class ProductoViewModelCRUD : BaseViewModel
    {
        public bool IsNew { get; set; }
        public Producto Item { get; set; }

        internal bool isValid(out string error)
        {
            try
            {
                if (Item != null)
                {
                    if (string.IsNullOrEmpty(Item.Codigo))
                    {
                        error = "Código vacio";
                        return false;
                    }
                    else
                    {
                        if (IsNew)
                        {
                            var temp = App.ProductoDB.IsCodeExist(Item.Codigo);
                            if (temp != null)
                            {
                                error = "el Código " + Item.Codigo + " ya existe";
                                return false;
                            }
                        }
                        if (string.IsNullOrEmpty(Item.Nombre))
                        {
                            error = "De ingresar un Nombre para el producto";
                            return false;
                        }
                        if (Item.PrecioUnitario <= 0)
                        {
                            error = "El Precio Unitario es obligatorio";
                            return false;
                        }
                        if (Item.PrecioMayorista < 0)
                        {
                            error = "El Precio Mayorista no puede ser menor a 0";
                            return false;
                        }
                        if (Item.PrecioOferta < 0)
                        {
                            error = "El Precio de Oferta no puede ser menor a 0";
                            return false;
                        }
                        if (Item.Stock <= 0)
                        {
                            error = "Debe ingresar Stock para el producto";
                            return false;
                        }
                    }
                }
                else
                {
                    error = "No se ha generado un producto";
                    return false;
                }
                error = "";
                return true;
            }
            catch
            {
                error = "Error General";
                return false;
            }
        }

        public void EliminarProducto(Producto prod)
        {
            try
            {
                
                MessagingCenter.Send(this, "Producto Eliminado", this.Item);
            }
            catch
            {

            }
        }
    }
}
