using System;
using System.Collections.Generic;
using System.Drawing;

namespace Programmeringsoppgave
{
    class LineShape : IShape
    {
        readonly PointF _point1;
        readonly PointF _point2;
        readonly Color _color;

        public RectangleF BoundingBox
        {
            get
            {
                var xmin = Math.Min(_point1.X, _point2.X);
                var ymin = Math.Min(_point1.Y, _point2.Y);
                var xmax = Math.Max(_point1.X, _point2.X);
                var ymax = Math.Max(_point1.Y, _point2.Y);
                return new RectangleF(new PointF(xmin, ymin), new SizeF(xmax - xmin, ymax - ymin)); 
            }
        }

        public LineShape(PointF point1, PointF point2, Color color)
        {
            _point1 = point1;
            _point2 = point2;
            _color = color;
        }

        public void Draw(Graphics g)
        {
            using (Pen pen = new Pen(_color, 0.5f))
            {
                g.DrawLine(pen, _point1, _point2);
            }
        }

        public static LineShape Parse(IList<string> points)
        {
            var linePoint1 = new PointF(
                float.Parse(points[0]),
                float.Parse(points[1])
            );
            var linePoint2 = new PointF(
                float.Parse(points[2]),
                float.Parse(points[3])
            );
            var lineColor = Color.FromArgb(
                byte.Parse(points[4]),
                byte.Parse(points[5]),
                byte.Parse(points[6])
            );
            return new LineShape(linePoint1, linePoint2, lineColor);
        }
    }
}
