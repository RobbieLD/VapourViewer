using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vv
{
    public class Settings
    {
        public string ImagePath { get; set; }
        public bool ShowHeader { get; set; }
        public bool ShowScrollbars { get; set; }
        public double WindowLeft { get; set; }
        public double WindowTop { get; set; }
        public double WindowHeight { get; set; }
        public double WindowWidth { get; set; }
        public double ScrollLeft { get; set; }
        public double ScrollTop { get; set; }
        public double ZoomLevel { get; set; }
        public Settings()
        {

        }
    }
}
