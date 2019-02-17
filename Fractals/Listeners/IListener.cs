using Fractals.Utility;

namespace Fractals.Listeners
{
    public interface IListener
    {
        void OnProgressChanged(int percentage);
        
        void OnTaskComplete(DirectBitmap bitmap, long elapsed);
    }
}