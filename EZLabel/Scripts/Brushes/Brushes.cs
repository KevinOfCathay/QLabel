using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

public class Brushes {
	public static Brush brush_dot_stroke_solid = new SolidColorBrush((Color) ColorConverter.ConvertFromString("#FF6A20D4"));
	public static Brush brush_dot_stroke_transparent = new SolidColorBrush((Color) ColorConverter.ConvertFromString("#556A20D4"));

	public static Brush brush_dot_fill_solid = new SolidColorBrush((Color) ColorConverter.ConvertFromString("#FFB5DCFF"));
	public static Brush brush_dot_fill_transparent = new SolidColorBrush((Color) ColorConverter.ConvertFromString("#55B5DCFF"));

	public static Brush brush_label_text_foreground_solid = new SolidColorBrush((Color) ColorConverter.ConvertFromString("#FF3B7E61"));
	public static Brush brush_label_text_foreground_transparent = new SolidColorBrush((Color) ColorConverter.ConvertFromString("#553B7E61"));

	public static Brush brush_label_text_background_solid = new SolidColorBrush((Color) ColorConverter.ConvertFromString("#FFEBFBFF"));
	public static Brush brush_label_text_background_transparent = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0));
}
