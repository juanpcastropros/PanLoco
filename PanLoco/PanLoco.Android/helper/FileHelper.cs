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
using PanLoco.Helpers;
using Xamarin.Forms;
using PanLoco.Droid.Helper;
using PanLoco.Models;


[assembly: Dependency(typeof(FileHelper))]
namespace PanLoco.Droid.Helper
{
    public class FileHelper : IFileHelper
    {

        public string GetLocalFilePath(string filename)
        {
            string path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            return System.IO.Path.Combine(path, filename);
        }
        public void DeleteFiles()
        {
            try
            {
                string path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments);
                string[] f = System.IO.Directory.GetFiles(path);
                List<string> values = new List<string>();
                foreach (string e in f)
                {
                    if(!e.Contains("SQLite"))
                        System.IO.File.Delete(e);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string SaveEntrega(List<EntregaItemVendido> itemVendidos, int id)
        {
            try
            {
                string path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments);
                string f = System.IO.Path.Combine(path, id.ToString() + ".txt");
                List<string> values = new List<string>();
                foreach (EntregaItemVendido e in itemVendidos)
                {
                    //IDentrega||ProductoCodigo||ProductoDescripcion||PrecioUnitario||PrecioOferta||cantidadNormal||CantidadDevolucion||Oferta||devolucion
                    values.Add(id.ToString()+"|"+e.Producto.Codigo.ToString() + "|" + e.Producto.Nombre + "|" +
                        e.PrecioUnitario.ToString("#.##") + "|" + e.PrecioOferta.ToString("#.##") + "|" + e.CantidadNor.ToString()
                        + "|" + e.CantidadDev.ToString() + "|" + e.Oferta.ToString()+"|"+e.Devolucion);
                }
                System.IO.File.AppendAllLines(f, values);
                return f;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<EntregaItemVendido> ReadEntrega( int id)
        {
            try
            {
                List<EntregaItemVendido> rt = new List<EntregaItemVendido>();
                string path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments);
                string f = System.IO.Path.Combine(path, id.ToString() + ".txt");
                string[] lines = System.IO.File.ReadAllLines(f);
                foreach (string e in lines)
                {
                    string[] values = e.Split("||".ToCharArray());
                    EntregaItemVendido t = new EntregaItemVendido();
                    t.Producto = new Producto();
                    t.EntregaId = int.Parse(values[0]);
                    t.Producto.Codigo = values[1];
                    t.Producto.Nombre = values[2];
                    t.PrecioUnitario = double.Parse("0"+values[3]);
                    t.PrecioOferta = double.Parse("0" + values[4]);
                    t.CantidadNor = int.Parse("0" + values[5]);
                    t.CantidadDev = int.Parse("0" + values[6]);
                    //t.Devolucion = t.CantidadDev > 0;
                    t.Oferta = bool.Parse(values[7]);
                    t.Devolucion= bool.Parse(values[8]);
                    rt.Add(t);
                    //IDentrega||ProductoID||ProductoDescripcion||PrecioUnitario||PrecioOferta||cantidadNormal||CantidadDevolucion
                }
                return rt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}

