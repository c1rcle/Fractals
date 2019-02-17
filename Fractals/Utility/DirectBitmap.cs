using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace Fractals.Utility
{
    public class DirectBitmap : IDisposable
    {
        public Bitmap Bitmap { get; }
        
        private int[] Bits { get; }
        
        private bool Disposed { get; set; }
        
        private int Width { get; }

        private GCHandle BitsHandle { get; }

        public DirectBitmap(int width, int height)
        {
            Width = width;
            Bits = new int[width * height];
            BitsHandle = GCHandle.Alloc(Bits, GCHandleType.Pinned);
            Bitmap = new Bitmap(width, height, width * 4, PixelFormat.Format32bppPArgb, BitsHandle.AddrOfPinnedObject());
        }

        public void SetPixel(int x, int y, Color color)
        {
            var index = x + (y * Width);
            var col = color.ToArgb();
            Bits[index] = col;
        }

        public void Dispose()
        {
            if (Disposed) return;
            Disposed = true;
            Bitmap.Dispose();
            BitsHandle.Free();
        }
    }
}