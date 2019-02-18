using System.Drawing;
using System.IO;
using System.Windows.Media.Imaging;

namespace Swtor.Dps.StatOptimizer.Utils
{
	public static class BitmapUtils
	{
		public static BitmapImage ToImageSource(this Image image)
		{
			using (var ms = new MemoryStream())
			{
				image.Save(ms, image.RawFormat);
				ms.Seek(0, SeekOrigin.Begin);
				var bi = new BitmapImage();
				bi.BeginInit();
				bi.CacheOption = BitmapCacheOption.OnLoad;
				bi.StreamSource = ms;
				bi.EndInit();
				return bi;
			}
		}
	}
}
