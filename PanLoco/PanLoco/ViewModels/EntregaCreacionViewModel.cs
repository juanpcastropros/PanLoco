using PanLoco.Helpers;
using PanLoco.Models;
using PanLoco.Services;
using PanLoco.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace PanLoco.ViewModels
{
    public class EntregaCreacionViewModel : BaseViewModel
    {

        private bool _isSavana = false;
        public Dictionary<string, int> _stock = new Dictionary<string, int>();
        public int GetStock(string codigo)
        {
            try
            {
                int _rtStock = -1;
                if (_stock.TryGetValue(codigo, out _rtStock))
                    return _rtStock;
                else
                {
                    Producto tmpProd = App.ProductoDB.GetItemAsyncByCode(codigo).Result;
                    if (tmpProd != null)
                    {
                        this._stock.Add(codigo, tmpProd.Stock);
                        _rtStock = tmpProd.Stock;
                    }
                    else
                        _rtStock = -1;
                }
                return _rtStock;
            }
            catch (Exception ex)
            {
                return -100;
            }
        }
        public void SetStock(string codigo, int stock, bool incrementar)
        {
            try
            {
                int tmp = GetStock(codigo);
                if (incrementar)
                    tmp += stock;
                else
                    tmp -= stock;
                if (_stock.ContainsKey(codigo))
                {
                    _stock[codigo] = tmp;
                }
            }
            catch (Exception ex)
            { }
        }
        //public new IDataStore<Entrega> DataStore => DependencyService.Get<IDataStore<Entrega>>();
        public Entrega Item { get; set; }

        Cliente cliente = null;
        public EntregaMock ItemMaster { get; set; }
        public ObservableRangeCollection<EntregaItemVendido> iVendidos { get; set; }

        private int editingPosition = -1;

        public int EditingPosition
        {
            get { return editingPosition; }
            set { editingPosition = value; }
        }

        public EntregaCreacionViewModel()
        {
            ItemMaster = new EntregaMock();
            Item = new Entrega();
            iVendidos = new ObservableRangeCollection<EntregaItemVendido>();
            MessagingCenter.Subscribe<EntregaAddProducto, EntregaItemVendido>(this, "AddItemVendido", (obj, item) =>
            {
                try
                {
                    var _item = item as EntregaItemVendido;
                    if (!editingPosition.Equals(-1))
                        iVendidos.RemoveAt(editingPosition);
                    editingPosition = -1;
                    iVendidos.Add(_item);
                    //stock[_item.Producto.Codigo] -= _item.Cantidad;
                    SetStock(_item.Producto.Codigo, _item.Cantidad, false);
                    Item.SubTotal = Item.SubTotal + _item.SubTotal;
                    Item.Total = 0;
                    foreach (EntregaItemVendido eiv in iVendidos)
                    {
                        if (!eiv.Devolucion && !eiv.Oferta)
                            Item.Total += (eiv.SubTotal - (eiv.SubTotal * (Item.ClienteDescuento / 100)));
                        else
                            Item.Total += eiv.SubTotal;
                    }
                    MessagingCenter.Send(this, "EntregatotalNuevo", Item);
                    //this.TotalAmount.Text = "$ " + Item.Total.ToString();
                }
                catch (Exception ex)
                { }
            });
            MessagingCenter.Subscribe<EntregaAddProducto, int>(this, "DeleteItemVendido", (obj, item) =>
            {
                try
                {
                    if (!editingPosition.Equals(-1))
                        iVendidos.RemoveAt(editingPosition);
                    editingPosition = -1;
                    Item.SubTotal = 0;
                    Item.Total = 0;
                    foreach (EntregaItemVendido eiv in iVendidos)
                    {

                        if (!eiv.Devolucion && !eiv.Oferta)
                            Item.Total += (eiv.SubTotal - (eiv.SubTotal * (Item.ClienteDescuento / 100)));
                        else
                            Item.Total += eiv.SubTotal;
                        Item.SubTotal += eiv.SubTotal;
                    }
                    MessagingCenter.Send(this, "EntregatotalNuevo", Item);
                    //this.TotalAmount.Text = "$ " + Item.Total.ToString();
                }
                catch (Exception ex)
                { }
            });
            MessagingCenter.Subscribe<EntregaAddProducto, int>(this, "CerrarItemVendido", (obj, item) =>
            {
                try
                {
                    if (!editingPosition.Equals(-1))
                    {
                        var _item = iVendidos[editingPosition];
                        if (_item != null)
                        {
                            SetStock(_item.Producto.Codigo, _item.Cantidad, false);
                        }
                        editingPosition = -1;
                    }
                }
                catch (Exception ex)
                { }
            });
        }

        public EntregaCreacionViewModel(bool isSavana)
        {
            _isSavana = isSavana;
            ItemMaster = new EntregaMock();
            Item = new Entrega();
            iVendidos = new ObservableRangeCollection<EntregaItemVendido>();
            var list = App.ProductoDB.getItemsAsync().Result;
            EntregaItemVendido tmp;
            _stock = new Dictionary<string, int>();
            foreach (Producto iv in list)
            {
                this._stock.Add(iv.Codigo, iv.Stock);
                tmp = new EntregaItemVendido();
                tmp.Producto = iv;
                tmp.CostoUnitario = iv.CostoUnitario;
                iVendidos.Add(tmp);
            }
        }
        public double GetClienteDescuento()
        {
            double value = cliente.Descuento;
            if (_isSavana)
            {
                if (iVendidos != null)
                {
                    foreach (EntregaItemVendido e in iVendidos)
                    {
                        double t = 0;
                        double.TryParse(value.ToString(), out t);
                        e.Porcentaje = t;
                    }
                }
            }
            return value;
        }
        public void setCliente(string selection)
        {

            Item.ClienteID = GetClienteID(selection);
            cliente = App.ClienteDB.GetItemAsync(Item.ClienteID).Result;
            Item.ClienteNombre = GetClienteName(selection);
            Item.ClienteDescuento = GetClienteDescuento();
            if (cliente != null)
                Item.EsMayorista = cliente.Mayorista;
        }

        public int GetClienteID(object selectedItem)
        {
            string temp = selectedItem.ToString();
            temp = temp.Substring(1, temp.IndexOf("]", 1) - 1);
            return int.Parse(temp);
        }
        public string GetClienteName(object selectedItem)
        {
            string temp = selectedItem.ToString();
            temp = temp.Substring(temp.IndexOf(" ", 1)).Trim();// ("]".ToCharArray())[1];
            //temp = temp.Split("[".ToCharArray())[1];
            return temp;
        }
        #region Agregar producto

        public void ProductoIncrementarStockPorEdicion(string codigo, int cantidad)
        {
            //stock[codigo] += cantidad;
            SetStock(codigo, cantidad, true);
        }
        public Producto GetProducto(string codigo)
        {
            Producto prodReturn = null;
            if (string.IsNullOrEmpty(codigo))
                return prodReturn;
            prodReturn = App.ProductoDB.GetItemAsyncByCode(codigo).Result;
            if (prodReturn != null)
            {
                int tmp = GetStock(prodReturn.Codigo);
                //if (!stock.ContainsKey(prodReturn.Codigo))
                //{
                //    stock.Add(prodReturn.Codigo, prodReturn.Stock);
                //}
            }
            return prodReturn;
        }
        public double GetPrecio(Producto prod, bool devolucion, bool oferta)
        {
            if (devolucion)
                return 0;
            if (Item.EsMayorista)
                return prod.PrecioMayorista;
            if (oferta)
                return prod.PrecioOferta;
            return prod.PrecioUnitario;
        }
        public bool IsProductoSelectionValid(Producto prod, int cantidad, out string message)
        {
            try
            {

                if (prod != null)
                {
                    int tmpStock = GetStock(prod.Codigo);
                    if (tmpStock > 0)
                    {
                        if (cantidad.Equals(0))
                        {
                            message = string.Empty;
                            return false;
                        }
                        else if (cantidad > tmpStock)
                        {
                            message = string.Format("No alcanza el STOCK de {0}, hay {1} ", new object[] { prod.Nombre, tmpStock });
                            return false;
                        }
                        message = string.Empty;
                        return true;
                    }
                    else
                    {
                        message = string.Format("No hay STOCK de {0}", new object[] { prod.Nombre });
                        return false;
                    }
                }
                message = "Debe seleccionar un Producto";
                return false;
            }
            catch
            {
                message = string.Empty;
                return false;
            }
        }
        #endregion

        public void Save()
        {
            if (_isSavana)
            {
                if (IsEntregaValida())
                {
                    this.Item.ClienteID = this.Item.Cliente.Id;
                    this.Item.ClienteNombre = this.cliente.NombreDeFantasia;
                    this.Item.ClienteDescuento = this.cliente.Descuento;
                    this.Item.ClienteMayorista = this.cliente.Mayorista;
                    this.Item.Fecha = DateTime.Now;
                    ProccessItemsVendidos();
                }
            }
            MessagingCenter.Send(this, "Entrega_Crear", this.Item);
        }

        private void ProccessItemsVendidos()
        {
            EntregaItemVendido iTemp = null;
            Item.Total = 0;
            foreach (EntregaItemVendido eiv in iVendidos)
            {
                
                if (eiv.CantidadNor > 0)
                {
                    iTemp = new EntregaItemVendido
                    {
                        Producto = eiv.Producto,
                        CantidadNor = eiv.CantidadNor,
                        CantidadDev = 0,
                        Devolucion = false,
                        Oferta = eiv.Oferta,
                        PrecioMayorista = eiv.Producto.PrecioMayorista,
                        PrecioOferta = eiv.Producto.PrecioOferta,
                        PrecioUnitario = eiv.Producto.PrecioUnitario,
                        ProductoID = eiv.Producto.Id
                    };
                    if (iTemp.Oferta)
                    {
                        iTemp.SubTotal = iTemp.CantidadNor * iTemp.PrecioOferta;
                    }
                    else
                    {
                        iTemp.SubTotal = iTemp.CantidadNor * iTemp.PrecioUnitario;
                        if (this.Item.ClienteDescuento > 0)
                        {
                            iTemp.SubTotal -= iTemp.SubTotal * (this.Item.ClienteDescuento / 100);
                        }
                    }
                    Item.ItemVendidos.Add(iTemp);
                    Item.Total += iTemp.SubTotal;
                }
                if(eiv.CantidadDev>0)
                {
                    iTemp = new EntregaItemVendido
                    {
                        Producto = eiv.Producto,
                        CantidadNor = 0,
                        CantidadDev = eiv.CantidadDev,
                        Devolucion = true,
                        Oferta = eiv.Oferta,
                        PrecioMayorista = eiv.Producto.PrecioMayorista,
                        PrecioOferta = eiv.Producto.PrecioOferta,
                        PrecioUnitario = eiv.Producto.PrecioUnitario,
                        ProductoID = eiv.Producto.Id
                    };
                    Item.ItemVendidos.Add(iTemp);
                }
                //if (eiv.Oferta && eiv.CantidadNor > 0)
                //{
                //    eiv.PrecioOferta = eiv.Producto.PrecioOferta;
                //    eiv.SubTotal = eiv.PrecioOferta * eiv.CantidadNor;
                //}
                //else if (!eiv.Oferta && eiv.CantidadNor > 0)
                //{
                //    eiv.PrecioUnitario = eiv.Producto.PrecioUnitario;
                //    eiv.SubTotal = eiv.PrecioUnitario * eiv.CantidadNor;
                //}
                if (eiv.CantidadNor > 0 || eiv.CantidadDev > 0)
                {
                    //Item.ItemVendidos.Add(eiv);
                    //Item.Total += eiv.SubTotal;
                    SetStock(eiv.Producto.Codigo, eiv.CantidadDev + eiv.CantidadNor, false);
                }
            }

        }
        public bool IsEntregaValida()
        {
            bool rt = true;
            if (this.Item == null)
                rt = false;
            if (this.Item.Cliente == null)
                rt = false;
            foreach (EntregaItemVendido eiv in iVendidos)
            {
                if (!eiv.IsValid)
                {
                    rt = false;
                    break;
                }
            }
            return rt;
        }

        public void TotalRefresh()
        {
            if (this.Item != null && this.iVendidos != null)
            {
                if (this.iVendidos.Count > 0)
                {
                    int _c = this.iVendidos.Count((EntregaItemVendido arg) => arg.IsValid == false);
                    if (_c.Equals(0))
                    {
                        bool mayo = false;
                        if (Item.Cliente != null)
                        {
                            mayo = Item.Cliente.Mayorista;
                        }
                        Item.SubTotal = 0;
                        Item.Total = 0;
                        foreach (EntregaItemVendido e in this.iVendidos)
                        {
                            if (!e.Devolucion && e.CantidadNor > 0)
                            {
                                if (mayo)
                                {
                                    Item.SubTotal += e.Producto.PrecioMayorista * e.CantidadNor;
                                    Item.Total += e.Producto.PrecioMayorista * e.CantidadNor;
                                }
                                else if (e.Oferta)
                                {
                                    Item.SubTotal += e.Producto.PrecioOferta * e.CantidadNor;
                                    Item.Total += e.Producto.PrecioOferta * e.CantidadNor;
                                }
                                else
                                {
                                    Item.SubTotal += (e.Producto.PrecioUnitario * e.CantidadNor);
                                    Item.Total += (e.Producto.PrecioUnitario - (e.Producto.PrecioUnitario * (e.Porcentaje / 100))) * e.CantidadNor;
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
