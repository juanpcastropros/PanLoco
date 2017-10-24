using PanLoco.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PanLoco.Helpers
{
    public interface IFileHelper
    {
        string GetLocalFilePath(string filename);
        string SaveEntrega(List<EntregaItemVendido> itemVendidos, int id);
        List<EntregaItemVendido> ReadEntrega(int ig);
        void DeleteFiles();
    }
}
