using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PanLoco.Models
{
    public class EntregaItemVendido : BaseDataObject
    {
        Producto producto = new Producto();
        [SQLite.Ignore]
        public Producto Producto
        {
            get { return producto; }
            set { SetProperty(ref producto, value); }
        }
        public int ProductoID
        {
            get { return Producto.Id; }
            set {
               Producto =  App.ProductoDB.GetItem(value);
            }
        }
        double precioUnitario = 0;
        public double PrecioUnitario
        {
            get { return precioUnitario; }
            set { SetProperty(ref precioUnitario, value); }
        }
        double precioOferta = 0;
        public double PrecioOferta
        {
            get { return precioOferta; }
            set { SetProperty(ref precioOferta, value); }
        }
        double precioMayorista= 0;
        public double PrecioMayorista
        {
            get { return precioMayorista; }
            set { SetProperty(ref precioMayorista, value); }
        }
        int entregaId = 0;
        public int EntregaId
        {
            get { return entregaId; }
            set { SetProperty(ref entregaId, value); }
        }
        double costoUnitario = 0;
        public double CostoUnitario
        {
            get { return costoUnitario; }
            set { SetProperty(ref costoUnitario, value); }
        }
        int cantidad = 0;
        public int Cantidad
        {
            get { return cantidad; }
            set { SetProperty(ref cantidad, value); }
        }
        private double subTotal = 0;
        public double SubTotal
        {
            get { return subTotal; }
            set { SetProperty(ref subTotal, value); }
        }
        public string Texto
        { get { return string.Concat(this.Producto.Codigo, "    ", this.Producto.Nombre); } }
        public string Descripcion
        { get { return string.Concat(this.Producto.Codigo," - " , this.Producto.Nombre,"(",this.producto.Stock,")"); } }

        private bool devolucion=false;

        public bool Devolucion 
        {
            get { return devolucion; }
            set { devolucion = value; }
        }
        private bool oferta=false;

        public bool Oferta
        {
            get { return oferta; }
            set { oferta = value; }
        }
        private string tipo;

        public string Tipo
        {
            get
            {
                if (Devolucion)
                {
                    return "DEV";
                }
                if (Oferta)
                {
                    return "OF";
                }
                return "NO";
            }
            set { tipo = value; }
        }

        private int cantidadNor;

        public int CantidadNor
        {
            get { return cantidadNor; }
            set { cantidadNor = value; }
        }
        private double porcentaje;

        public double Porcentaje
        {
            get { return porcentaje; }
            set { porcentaje = value; }
        }
        private int cantidadDev;

        public int CantidadDev
        {
            get { return cantidadDev; }
            set { cantidadDev = value; }
        }
        private bool _isValid=true;

        public bool IsValid
        {
            get { return _isValid; }
            set { _isValid = value; }
        }
        private int _descuento;



    }
}
