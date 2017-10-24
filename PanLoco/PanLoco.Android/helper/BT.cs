using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Bluetooth;
using Java.IO;
using Java.Util;
using System.Collections.ObjectModel;
using PanLoco.Helpers;
using Xamarin.Forms;
using PanLoco.Droid.Helper;
using Java.Lang;
using PanLoco.Models;

[assembly: Dependency(typeof(clsBluetooth))]
namespace PanLoco.Droid.Helper
{
    public class clsBluetooth : IBluetooth
    {
        private BluetoothAdapter adapter = BluetoothAdapter.DefaultAdapter;
        private BluetoothSocket socket = null;
        private BufferedWriter outReader = null;
        private BluetoothDevice device = null;

        byte[] readBuffer;
        int readBufferPosition;
        volatile bool stopWorker;
        private BufferedWriter mmInputStream = null;
        public void Imprimir(string pStrNomBluetooth, int intSleepTime, Entrega entrega, Perfil perfil)
        {
            try
            {
                string pStrTextoImprimir = string.Empty;
                string bt_printer = (from d in adapter.BondedDevices
                                     where d.Name == pStrNomBluetooth
                                     select d).FirstOrDefault().ToString();

                device = adapter.GetRemoteDevice(bt_printer);

                UUID applicationUUID = UUID.FromString("00001101-0000-1000-8000-00805f9b34fb");


                //socket.InputStream.Write()
                socket = device.CreateRfcommSocketToServiceRecord(applicationUUID);

                socket.Connect();

                //socket.InputStream.Write(bytes,0, bytes.Length);
                //socket.InputStream.WriteByte(Byte.Parse("h"));
                outReader = new BufferedWriter(new OutputStreamWriter(socket.OutputStream));

                printLine(perfil.FullName);
                printLine(perfil.Calle);
                printLine(perfil.Localidad);
                if (entrega != null)
                    printLine(perfil.Cuil+"          " + entrega.Fecha.ToShortDateString());
                //printLine("d123456789v123456789t123456789cu23456789c1234567");
                printLine("Item    Descripcion                       Neto");
                printLine("  Cant    Precio                          Neto");

                IList<EntregaItemVendido> nor = new List<EntregaItemVendido>();
                IList<EntregaItemVendido> of = new List<EntregaItemVendido>();
                IList<EntregaItemVendido> dev = new List<EntregaItemVendido>();
                System.Text.StringBuilder st = new System.Text.StringBuilder();
                if (entrega != null)
                {
                    if (entrega.ItemVendidos.Count > 0)
                    {
                        foreach (EntregaItemVendido i in entrega.ItemVendidos)
                        {
                            if (i.Oferta && i.CantidadNor > 0)
                                of.Add(i);
                            if (i.Devolucion || i.CantidadDev > 0)
                                dev.Add(i);
                            if (!i.Oferta && i.CantidadNor > 0)
                                nor.Add(i);
                        }
                    }
                }
                if (entrega.ClienteMayorista)
                { PrintMayorista(nor,of,dev, entrega); }
                else
                { PrintMinorista(nor, of, dev, entrega); }

                //outReader.WriteAsync(pStrTextoImprimir);
                //cabecera
              
                //

                //Body
                
                
                if (entrega != null)
                {
                    System.Text.StringBuilder lb = new System.Text.StringBuilder();
                    string lbb = "";
                    int u = 0;
                    lb = lb.Clear();
                    lb.Append("   TOTAL");
                    lbb = "$ " + entrega.Total.ToString("#.##");
                    u = 47 - (lb.Length + lbb.Length);
                    lb.Insert(lb.Length, " ", u);
                    lb.Append(lbb);
                    printLine(lb.ToString());

                    //printLine("  TOTAL                                  $" +
                    //    entrega.Total.ToString("#.##"));
                }
                printLine("        ESTE TICKET NO TIENE");
                printLine("           VALIDEZ FISCAL");
                printLine("");
                printLine("");
            }
            catch (System.Exception ex)
            {

                throw ex;
            }
            finally
            {
                if (socket != null)
                {
                    outReader.Close();
                    socket.Close();
                    socket.Dispose();
                }

            }
        }
        private void PrintMinorista(IList<EntregaItemVendido> nor,
        IList<EntregaItemVendido> of ,
        IList<EntregaItemVendido> dev, Entrega ent)
        {
            double subt = 0;
            if (nor.Count > 0)
            {
                printLine("");
                printLine(" Productos");
                printLine("");
            }
            System.Text.StringBuilder lb = new System.Text.StringBuilder();
            string lbb = "";
            int u = 0;
            foreach (EntregaItemVendido i in nor)
            {
                //f.Insert(0," ",5)
                lb = lb.Clear();
                lb.Append(i.Producto.Codigo + "  " + i.Producto.Nombre);
                lbb = "$ "+(i.CantidadNor * i.PrecioUnitario).ToString("#.##");
                u = 47 - (lb.Length + lbb.Length);
                lb.Insert(lb.Length, " ", u);
                lb.Append(lbb);
                printLine(lb.ToString());
                //printLine("d123456789v123456789t123456789cu23456789c1234567");
                //printLine(i.Producto.Codigo + "  " + i.Producto.Descripcion);
                printLine("  " + i.CantidadNor.ToString("##") + " X   $"
                    + i.PrecioUnitario.ToString("#.##"));
                
                subt += (i.CantidadNor * i.PrecioUnitario);
            }
            if (nor.Count() > 0)
            {
                printLine("");
                lb = lb.Clear();
                lb.Append("Sub Total Prod");
                lbb = "$ " + subt.ToString("##.##");
                u = 47 - (lb.Length + lbb.Length);
                lb.Insert(lb.Length, " ", u);
                lb.Append(lbb);
                printLine(lb.ToString());
                //printLine("Sub Total Prod $ " + subt.ToString("##.##") + "");
                if (ent.ClienteDescuento > 0)
                {
                    lb = lb.Clear();
                    lb.Append("Descuento " + ent.ClienteDescuento.ToString() + "%");
                    lbb = "$ -" + (subt * (ent.ClienteDescuento / 100)).ToString("##.##");
                    u = 47 - (lb.Length + lbb.Length);
                    lb.Insert(lb.Length, " ", u);
                    lb.Append(lbb);
                    printLine(lb.ToString());
                    //printLine("Descuento " + ent.ClienteDescuento.ToString() + "% - $ " + (subt * (ent.ClienteDescuento / 100)).ToString("##.##"));
                    subt -= (subt * (ent.ClienteDescuento / 100));
                    lb = lb.Clear();
                    lb.Append("Sub Total con Desc");
                    lbb = "$ " + subt.ToString("##.##");
                    u = 47 - (lb.Length + lbb.Length);
                    lb.Insert(lb.Length, " ", u);
                    lb.Append(lbb);
                    printLine(lb.ToString());
                    //printLine("Sub Total con Desc $ " + subt.ToString("##.##") );
                }
            }
            if (of.Count > 0)
            {
                printLine("");
                printLine("Ofertas");
                printLine("");
            }
            subt = 0;
            foreach (EntregaItemVendido i in of)
            {
                lb = lb.Clear();
                lb.Append(i.Producto.Codigo + "  " + i.Producto.Nombre);
                lbb = "$ " + (i.CantidadNor * i.PrecioOferta).ToString("#.##");
                u = 47 - (lb.Length + lbb.Length);
                lb.Insert(lb.Length, " ", u);
                lb.Append(lbb);
                printLine(lb.ToString());
                //printLine( i.Producto.Codigo + " $" + (i.CantidadNor * i.PrecioMayorista) );
                printLine(i.CantidadNor + " x $" + i.PrecioOferta);
                subt += (i.CantidadNor * i.PrecioOferta);
            }
            if (of.Count > 0)
            {
                lb = lb.Clear();
                lb.Append("Sub Total Ofertas ");
                lbb = "$ " + subt.ToString("##.##");
                u = 47 - (lb.Length + lbb.Length);
                lb.Insert(lb.Length, " ", u);
                lb.Append(lbb);
                printLine(lb.ToString());
                //printLine("Sub Total Ofertas $ " + subt.ToString("##.##"));
            }

            if (dev.Count() > 0)
            {
                printLine("");
                printLine("Devoluciones");
                printLine("");
            }
            foreach (EntregaItemVendido i in dev)
            {
                lb = lb.Clear();
                lb.Append(i.Producto.Codigo + "  " + i.Producto.Nombre);
                lbb = "$ 0" ;
                u = 47 - (lb.Length + lbb.Length);
                lb.Insert(lb.Length, " ", u);
                lb.Append(lbb);
                printLine(lb.ToString());
                //printLine("" + i.Producto.Codigo + "$ 0");
                printLine( i.CantidadDev + " x $ 0");
            }
        }

        private void PrintMayorista(IList<EntregaItemVendido> nor,
        IList<EntregaItemVendido> of,
        IList<EntregaItemVendido> dev, Entrega ent)
        {
            double subt = 0;
            if (nor.Count > 0)
            {
                printLine("");
                printLine(" Productos");
                printLine("");
            }
            System.Text.StringBuilder lb = new System.Text.StringBuilder();
            string lbb = "";
            int u = 0;
            foreach (EntregaItemVendido i in nor)
            {
                //f.Insert(0," ",5)
                lb = lb.Clear();
                lb.Append(i.Producto.Codigo + "  " + i.Producto.Nombre);
                lbb = "$ " + (i.CantidadNor * i.PrecioMayorista).ToString("#.##");
                u = 47 - (lb.Length + lbb.Length);
                lb.Insert(lb.Length, " ", u);
                lb.Append(lbb);
                printLine(lb.ToString());
                //printLine("d123456789v123456789t123456789cu23456789c1234567");
                //printLine(i.Producto.Codigo + "  " + i.Producto.Descripcion);
                printLine("  " + i.CantidadNor.ToString("##") + " X   $"
                    + i.PrecioMayorista.ToString("#.##"));

                subt += (i.CantidadNor * i.PrecioMayorista);
            }
            if (nor.Count() > 0)
            {
                printLine("");
                lb = lb.Clear();
                lb.Append("Sub Total Prod");
                lbb = "$ " + subt.ToString("##.##");
                u = 47 - (lb.Length + lbb.Length);
                lb.Insert(lb.Length, " ", u);
                lb.Append(lbb);
                printLine(lb.ToString());
                //printLine("Sub Total Prod $ " + subt.ToString("##.##") + "");
                if (ent.ClienteDescuento > 0)
                {
                    lb = lb.Clear();
                    lb.Append("Descuento " + ent.ClienteDescuento.ToString() + "%");
                    lbb = "$ -" + (subt * (ent.ClienteDescuento / 100)).ToString("##.##");
                    u = 47 - (lb.Length + lbb.Length);
                    lb.Insert(lb.Length, " ", u);
                    lb.Append(lbb);
                    printLine(lb.ToString());
                    //printLine("Descuento " + ent.ClienteDescuento.ToString() + "% - $ " + (subt * (ent.ClienteDescuento / 100)).ToString("##.##"));
                    subt -= (subt * (ent.ClienteDescuento / 100));
                    lb = lb.Clear();
                    lb.Append("Sub Total con Desc");
                    lbb = "$ " + subt.ToString("##.##");
                    u = 47 - (lb.Length + lbb.Length);
                    lb.Insert(lb.Length, " ", u);
                    lb.Append(lbb);
                    printLine(lb.ToString());
                    //printLine("Sub Total con Desc $ " + subt.ToString("##.##") );
                }
            }
            if (of.Count > 0)
            {
                printLine("");
                printLine("Ofertas");
                printLine("");
            }
            subt = 0;
            foreach (EntregaItemVendido i in of)
            {
                lb = lb.Clear();
                lb.Append(i.Producto.Codigo + "  " + i.Producto.Nombre);
                lbb = "$ " + (i.CantidadNor * i.PrecioMayorista).ToString("#.##");
                u = 47 - (lb.Length + lbb.Length);
                lb.Insert(lb.Length, " ", u);
                lb.Append(lbb);
                printLine(lb.ToString());
                //printLine( i.Producto.Codigo + " $" + (i.CantidadNor * i.PrecioMayorista) );
                printLine(i.CantidadNor + " x $" + i.PrecioMayorista);
                subt += (i.CantidadNor * i.PrecioOferta);
            }
            if (of.Count > 0)
            {
                lb = lb.Clear();
                lb.Append("Sub Total Ofertas ");
                lbb = "$ " + subt.ToString("##.##");
                u = 47 - (lb.Length + lbb.Length);
                lb.Insert(lb.Length, " ", u);
                lb.Append(lbb);
                printLine(lb.ToString());
                //printLine("Sub Total Ofertas $ " + subt.ToString("##.##"));
            }

            if (dev.Count() > 0)
            {
                printLine("");
                printLine("Devoluciones");
                printLine("");
            }
            foreach (EntregaItemVendido i in dev)
            {
                lb = lb.Clear();
                lb.Append(i.Producto.Codigo + "  " + i.Producto.Nombre);
                lbb = "$ 0";
                u = 47 - (lb.Length + lbb.Length);
                lb.Insert(lb.Length, " ", u);
                lb.Append(lbb);
                printLine(lb.ToString());
                //printLine("" + i.Producto.Codigo + "$ 0");
                printLine(i.CantidadDev + " x $ 0");
            }
        }
        private void printLine(string pStrTextoImprimir)
        {
            byte[] bytes = null;
            bytes = new byte[pStrTextoImprimir.Length * sizeof(char)];
            System.Buffer.BlockCopy(pStrTextoImprimir.ToCharArray(), 0, bytes, 0, bytes.Length);
            outReader.Write(pStrTextoImprimir + "\n");
        }
        public ObservableCollection<string> PairedDevices()
        {
            ObservableCollection<string> devices = new ObservableCollection<string>();

            foreach (var bd in adapter.BondedDevices)
                devices.Add(bd.Name);

            return devices;
        }


        public void beginListenForData()
        {
            try
            {
                Handler handler = new Handler();

                // this is the ASCII code for a newline character
                byte delimiter = 10;

                stopWorker = false;
                readBufferPosition = 0;
                readBuffer = new byte[1024];

                //Thread workerThread = new Thread(new Runnable()
            }
            catch (System.Exception ex)
            {

            }
        }
    }
}