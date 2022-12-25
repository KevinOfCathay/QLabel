using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace QLabel.Scripts.Projects
{
    public record ImageFileData
    {
        public ImageSource source;
        public string filename { get; set; } = string.Empty;
        public string path { get; set; } = string.Empty;
        public double width { get; set; }
        public double height { get; set; }
        public long size { get; set; }
    }
}