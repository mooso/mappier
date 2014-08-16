using Mappier;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scratchpad
{
	class Program
	{
		static void Main(string[] args)
		{
			ShowContinentalUSA();
		}

		static void ShowContinentalUSA()
		{
			var usa = USA.Create2013USA();
			var image = usa.DrawContinentalUSA();
			var filePath = Path.Combine(GetScratchDir(), "usa_continental.gif");
			image.Save(filePath, ImageFormat.Gif);
			Process.Start(filePath);
		}

		static string GetScratchDir()
		{
			var appData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
			var myAppData = Path.Combine(appData, "Mappier");
			if (!Directory.Exists(myAppData))
			{
				Directory.CreateDirectory(myAppData);
			}
			return myAppData;
		}
	}
}
