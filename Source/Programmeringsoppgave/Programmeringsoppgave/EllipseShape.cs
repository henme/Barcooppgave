using System.Collections.Generic;
using System.Drawing;

namespace Programmeringsoppgave
{
    class EllipseShape : IShape
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

        public EllipseShape(RectangleF rect, Color ellipseColor)
        {
            _rect = rect;
            _color = ellipseColor;
        }

        public void Draw(Graphics g)
        {
            using (Pen pen = new Pen(_color, 0.5f))
            {
                g.DrawEllipse(pen,_rect);
            }
        }

        internal static EllipseShape Parse(IList<string> parameters)
        {
            var topLeftPoint = new PointF(
                float.Parse(parameters[0]) - (float.Parse(parameters[2]) /2),
                float.Parse(parameters[1]) - (float.Parse(parameters[3]) / 2)
            );
            var width = float.Parse(parameters[2]);
            var height = float.Parse(parameters[3]);
            var color = Color.FromArgb(
                byte.Parse(parameters[4]),
                byte.Parse(parameters[5]),
                byte.Parse(parameters[6])
            );
            return new EllipseShape(new RectangleF(topLeftPoint.X, topLeftPoint.Y, width, height), color);
        }
    }
}
