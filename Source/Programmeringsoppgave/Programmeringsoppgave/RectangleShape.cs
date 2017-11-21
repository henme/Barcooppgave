using System.Collections.Generic;
using System.Drawing;

namespace Programmeringsoppgave
{
    class RectangleShape : IShape
    {
        readonly RectangleF _rect;
        readonly Color _color;
        
        public RectangleF BoundingBox
        {
            get
            {
                return _rect;
            }
        }

        public RectangleShape(RectangleF rect, Color color)
        {
            _rect = rect;
            _color = color;
        }

        public void Draw(Graphics g)
        {
            using (Pen pen = new Pen(_color, 0.5f))
            {
                g.DrawRectangles(pen, new[] { _rect });
            }
        }

        internal static RectangleShape Parse(IList<string> parameters)
        {
            var rectPoint = new PointF(
                float.Parse(parameters[0]),
                float.Parse(parameters[1])
            );
            var width = float.Parse(parameters[2]);
            var height = float.Parse(parameters[3]);
            var rectColor = Color.FromArgb(
                byte.Parse(parameters[4]),
                byte.Parse(parameters[5]),
                byte.Parse(parameters[6])
            );
            return new RectangleShape(new RectangleF(rectPoint.X, rectPoint.Y, width, height), rectColor);
        }
    }
}
