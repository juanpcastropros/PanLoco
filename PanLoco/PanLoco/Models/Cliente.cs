using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PanLoco.Models
{
    public class Cliente: BaseDataObject
    {
        string nombreDeFantasia = string.Empty;
        public string NombreDeFantasia
        {
            get { return nombreDeFantasia; }
            set { SetProperty(ref nombreDeFantasia, value); }
        }

        string nombreCompleto = string.Empty;
        public string NombreCompleto
        {
            get { return nombreCompleto; }
            set { SetProperty(ref nombreCompleto, value); }
        }
        string telefono = string.Empty;
        public string Telefono
        {
            get { return telefono; }
            set { SetProperty(ref telefono, value); }
        }

        string horario = string.Empty;
        public string Horario
        {
            get { return horario; }
            set { SetProperty(ref horario, value); }
        }
        double descuento = 0;
        public double Descuento
        {
            get { return descuento; }
            set { SetProperty(ref descuento, value); }
        }
        private bool mayorista=false;

        public bool Mayorista
        {
            get { return mayorista; }
            set { mayorista = value; }
        }


    }
}
