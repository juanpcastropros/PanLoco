using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PanLoco.Models
{
    public class Perfil: BaseDataObject
    {
        public string FullName { get; set; }
        public string Cuil { get; set; }
        public string Calle { get; set; }
        public string Localidad { get; set; }
    }
}
