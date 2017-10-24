using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PanLoco.Views.Acordeon
{

    public class MainPageAMenuItem
    {
        public MainPageAMenuItem()
        {
            TargetType = typeof(MainPageADetail);
        }
        public int Id { get; set; }
        public string Title { get; set; }

        public Type TargetType { get; set; }
    }
}
