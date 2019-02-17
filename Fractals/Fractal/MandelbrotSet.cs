using System;
using System.Diagnostics;
using System.Drawing;
using System.Numerics;
using System.Threading;
using System.Threading.Tasks;
using Fractals.Listeners;
using Fractals.Utility;

namespace Fractals.Fractal
{
    public class MandelbrotSet : IFractal
    {
        public IListener Listener { private get; set; }
        
        private const float XMinVal = -2.0f;

        private const float XMaxVal = 1.0f;

        private const float YMinVal = -1.0f;

        private const float YMaxVal = 1.0f;

        private readonly int iterations;

        private readonly int offsetX;

        private readonly int offsetY;

        private readonly float zoom;

        public MandelbrotSet(int iterations, int offsetX, int offsetY, float zoom)
        {
            this.iterations = iterations;
            this.offsetX = offsetX;
            this.offsetY = offsetY;
            this.zoom = zoom;
        }

        public void GetFractal(int width, int height)
        {
            var watch = Stopwatch.StartNew();
            var bitmap = new DirectBitmap(width, height);
            var progress = 0;
            Parallel.For(0, width, x =>
            {
                for (var y = 0; y < height; y++)
                {
                    double rel = (XMinVal + ((XMaxVal - XMinVal) / width) * (x + offsetX)) / zoom;
                    double ima = (YMinVal + ((YMaxVal - YMinVal) / height) * (y + offsetY)) / zoom;
                    var complexC = new Complex(rel, ima);
                    var complexZ = new Complex(0, 0);
                    var iteration = 0;
                    while (iteration < iterations && Complex.Abs(complexZ) <= 2)
                    {
                        var complexNew = Complex.Pow(complexZ, 2);
                        complexNew = Complex.Add(complexNew, complexC);
                        complexZ = complexNew;
                        iteration++;
                    }

                    if (iteration < iterations)
                    {
                        var smoothVal = iteration + 1 - Math.Log(Math
                                            .Log(Complex.Abs(complexZ))) / Math.Log(2);
                        var color = ColorConversion.GetColor(smoothVal);
                        bitmap.SetPixel(x, y, color);
                    }
                    else bitmap.SetPixel(x, y, Color.Black);
                }
                
                Interlocked.Increment(ref progress);
                if (progress % (width / 10) != 0) return;
                var percentage = (float) progress / (width - 1) * 100;
                Listener.OnProgressChanged((int) percentage);
            });
            watch.Stop();
            Listener.OnTaskComplete(bitmap, watch.ElapsedMilliseconds);
        }
    }
}
