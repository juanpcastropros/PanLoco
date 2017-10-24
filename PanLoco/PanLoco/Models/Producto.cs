namespace PanLoco.Models
{
    public class Producto : BaseDataObject
    {
        string codigo = string.Empty;
        public string Codigo
        {
            get { return codigo; }
            set { SetProperty(ref codigo, value); }
        }
        string descripcion = string.Empty;
        public string Descripcion
        {
            get { return descripcion; }
            set { SetProperty(ref descripcion, value); }
        }

        string nombre = string.Empty;
        public string Nombre
        {
            get { return nombre; }
            set { SetProperty(ref nombre, value); }
        }
        double precioUnitario= 0;
        public double PrecioUnitario
        {
            get { return precioUnitario; }
            set { SetProperty(ref precioUnitario, value); }
        }
        double costoUnitario = 0;
        public double CostoUnitario
        {
            get { return costoUnitario; }
            set { SetProperty(ref costoUnitario, value); }
        }
        int stock = 0;
        public int Stock
        {
            get { return stock; }
            set { SetProperty(ref stock, value); }
        }
        private double precioMayorista;

        public double PrecioMayorista
        {
            get { return precioMayorista; }
            set { precioMayorista = value; }
        }
        private double precioOferta;

        public double PrecioOferta
        {
            get { return precioOferta; }
            set { precioOferta = value; }
        }



    }
}
