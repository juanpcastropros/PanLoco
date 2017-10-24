using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PanLoco.Models
{
    public class Entrega: BaseDataObject
    {
        DateTime fecha = DateTime.Now;
        public DateTime Fecha
        {
            get { return fecha; }
            set { SetProperty(ref fecha, value); }
        }

        public double ClienteDescuento { get; set; }
        public bool ClienteMayorista { get; set; }
        public string ClienteNombre { get; set; }
        Cliente cliente = null;
        [SQLite.Ignore]
        public Cliente Cliente
        {
            get {
                if (clienteID > 0)
                    cliente = App.ClienteDB.GetItemAsync(clienteID).Result;
                return cliente; }
            set {
                cliente = value;
                if (cliente != null && cliente.Id > 0)
                    clienteID = cliente.Id;
            //    SetProperty(ref cliente, value);
            }
        }

        int clienteID = 0;
        public int ClienteID
        {
            get { return clienteID; }
            set {
                clienteID = value;
            }
        }
        double total = 0;
        public double Total
        {
            get { return total; }
            set { SetProperty(ref total, value); }
        }

        List<EntregaItemVendido> itemVendidos= new List<EntregaItemVendido>();
        [SQLite.Ignore]
        public List<EntregaItemVendido> ItemVendidos
        {
            get { return itemVendidos; }
            set { SetProperty(ref itemVendidos, value); }
        }

        public string ItemVendidosLista
        {
            get
            {
                string tmp = string.Empty;
                foreach (EntregaItemVendido eiv in itemVendidos)
                {
                    tmp= string.Concat(tmp.ToString(), eiv.ProductoID.ToString(), "|0");
                }
                return tmp;
            }
            set
            {
                ///TODO: Armar lista de items vendidos desde DB con los IDs
            }
        }
        public string Descripcion
        {
            get {
                string rt = "";
                if (clienteID >0)
                    rt = cliente.NombreCompleto;
                return rt;
            }
        }
        private bool esMayorista;

        public bool EsMayorista
        {
            get { return esMayorista; }
            set { esMayorista = value; }
        }
        double subTotal = 0;
        public double SubTotal
        {
            get { return subTotal; }
            set { SetProperty(ref subTotal, value); }
        }

    }

    
}
