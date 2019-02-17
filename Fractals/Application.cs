using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using Fractals.Fractal;
using Fractals.Listeners;
using Fractals.Utility;
using Mono.Options;

namespace Fractals
{
    public class Application : IListener
    {
        private bool isHelp;
        
        public void StartApp(IReadOnlyCollection<string> args)
        {
            int width = 900, height = 600;
            var iterations = 20;
            int offsetX = 0, offsetY = 0;
            var zoom = 1.0f;

            var options = new OptionSet
            {
                {"w=", x => width = int.Parse(x)},
                {"h=", x => height = int.Parse(x)},
                {"iter=", x => iterations = int.Parse(x)},
                {"ofX=", x => offsetX = int.Parse(x)},
                {"ofY=", x => offsetY = int.Parse(x)},
                {"z=", x => zoom = float.Parse(x)},
                {"help", x => PrintHelp()}
            };
            List<string> argsList;
            try
            {
                argsList = options.Parse(args);
            }
            catch (FormatException)
            {
                Console.WriteLine("Wrong parameter types!");
                return;
            }
            
            if (argsList.Count > 0 || args.Count < 1) Console.WriteLine("Using default values. Start with -help to get more information.");
            if (isHelp) return;
            IFractal fractal = new MandelbrotSet(iterations, offsetX, offsetY, zoom);
            fractal.Listener = this;
            fractal.GetFractal(width, height);
        }
        
        private void PrintHelp()
        {
            Console.WriteLine("Available parameters are:");
            Console.WriteLine("'-w=value' - sets width of output image");
            Console.WriteLine("'-h=value' - sets height of output image");
            Console.WriteLine("'-iter=value' - sets iteration count of the algorithm");
            Console.WriteLine("'-ofX=value' - sets x-axis offset of output image");
            Console.WriteLine("'-ofY=value' - sets y-axis offset of output image");
            Console.WriteLine("'-z=value' - sets zoom of output image");
            Console.WriteLine("Default values are set as follows:");
            Console.WriteLine("w=900, h=600, iter=20, ofX=0, ofY=0, z=1");
            isHelp = true;
        }

        public void OnProgressChanged(int percentage)
        {
            Console.SetCursorPosition(0, Console.CursorTop);
            Console.Write("Progress: " + percentage + "%");
        }

        public void OnTaskComplete(DirectBitmap bitmap, long elapsed)
        {
            Console.SetCursorPosition(0, Console.CursorTop);
            Console.WriteLine("Task completed. Execution time: " + (float) elapsed / 1000 + "s");
            bitmap.Bitmap.Save("image.png", ImageFormat.Png);
            bitmap.Dispose();
        }
    }
}