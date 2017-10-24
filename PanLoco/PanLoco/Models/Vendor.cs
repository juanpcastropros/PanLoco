using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PanLoco.Models
{
    public class Vendor : BaseDataObject
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

    }
}
