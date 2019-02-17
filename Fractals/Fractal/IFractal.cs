using Fractals.Listeners;

namespace Fractals.Fractal
{
    public interface IFractal
    {
        IListener Listener { set; }
        
        void GetFractal(int width, int height);
    }
}
