using System.Drawing;

namespace Programmeringsoppgave
{
    interface IShape
    {
        void Draw(Graphics g);
        RectangleF BoundingBox { get; }
    }
}
