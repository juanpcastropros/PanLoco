using PanLoco.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace PanLoco.Helpers
{
    public interface IBluetooth
    {
        ObservableCollection<string> PairedDevices();

        void Imprimir(string pStrNomBluetooth, int intSleepTime, Entrega pStrTextoImprimir, Perfil perfil);
    }
}
