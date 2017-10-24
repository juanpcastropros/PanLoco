using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PanLoco.Models
{
    public class EntregaMock: BaseDataObject
    {
        public List<string> Clientes
        { get {
                List<string> temp = new List<string>();
                var list = App.ClienteDB.GetItemsNotDoneAsync().Result;

                foreach (Cliente c in list)
                    temp.Add(string.Concat("[",c.Id,"] ", c.NombreDeFantasia));
                return temp; } }
    }
}
