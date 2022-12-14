using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace EZLabel.Scripts.AnnotationData
{
    public record AnnoData
    {
        /// <summary>
        /// 这个注释数据的点的位置 (x, y)
        /// </summary>
        public Point[] points;

        /// <summary>
        /// 这个注释数据的类
        /// </summary>
        public int clas;

        /// <summary>
        /// 这个注释数据的标签
        /// </summary>
        public string label;
    }
}
