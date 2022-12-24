using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

public class Brushes {
	public static Brush brush_dot_stroke_solid = new SolidColorBrush((Color) ColorConverter.ConvertFromString("#FF6A20D4"));
	public static Brush brush_dot_stroke_transparent = new SolidColorBrush((Color) ColorConverter.ConvertFromString("#666A20D4"));

	public static Brush brush_dot_fill_solid = new SolidColorBrush((Color) ColorConverter.ConvertFromString("#FFB5DCFF"));
	public static Brush brush_dot_fill_transparent = new SolidColorBrush((Color) ColorConverter.ConvertFromString("#66B5DCFF"));

	public static Brush brush_label_text_foreground_solid = new SolidColorBrush((Color) ColorConverter.ConvertFromString("#FF6FA46F"));
	public static Brush brush_label_text_foreground_transparent = new SolidColorBrush((Color) ColorConverter.ConvertFromString("#006FA46F"));

}
