using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Programmeringsoppgave
{
    public partial class DrawForm : Form
    {
        //Image paramters.
        private const float origoX = 120;
        private const float origoY = 40;
        //Update delay in ms.
        private const int delay = 500;

        private readonly string _fileName;
        private readonly List<IShape> _shapes = new List<IShape>();
        private readonly Timer _timer = new Timer();
        private int _drawCount = 0;
        private RectangleF _boundingBox = new RectangleF();

        public DrawForm(string fileName)
        {
            _fileName = fileName;
            InitializeComponent();
        }

        private void ReadFile()
        {
            try
            {
                using (var fs = File.OpenRead(_fileName))
                using (var r = new StreamReader(fs))
                {
                    string line;
                    var lineno = 0;
                    while ((line = r.ReadLine()) != null)
                    {
                        lineno++;
                        var words = line.Split(';');
                        if (words.Length != 8)
                        {
                            MessageBox.Show($"Error on line {lineno}");
                            continue;
                        }
                        var type = words[0];
                        try
                        {
                            switch (type)
                            {
                                case "line":
                                    var lineShape = LineShape.Parse(new ArraySegment<string>(words,1,7));
                                    _shapes.Add(lineShape);
                                    break;

                                case "rectangle":
                                    var rectangleShape = RectangleShape.Parse(new ArraySegment<string>(words, 1, 7));
                                    _shapes.Add(rectangleShape);
                                    break;

                                case "ellipse":
                                    var ellipseShape = EllipseShape.Parse(new ArraySegment<string>(words, 1, 7));
                                    _shapes.Add(ellipseShape);
                                    break;

                                default:
                                    MessageBox.Show($"Error on line {lineno}, unknown type {type}");
                                    break;
                            }
                        }
                        catch(Exception e)
                        {
                            MessageBox.Show($"Error on line {lineno}, error: {e}");
                        }
                    }
                    // Find minimum bounding rectangle by adding 2 and 2 primitives.
                    var first = true;
                    foreach(var shape in _shapes)
                    {
                        if (first)
                        {
                            first = false;
                            _boundingBox = shape.BoundingBox;
                            continue;
                        }

                        var shapeBox = shape.BoundingBox;
                        var xmin = Math.Min(_boundingBox.X, shapeBox.X);
                        var ymin = Math.Min(_boundingBox.Y, shapeBox.Y);
                        var xmax = Math.Max(_boundingBox.Right , shapeBox.Right);
                        var ymax = Math.Max(_boundingBox.Bottom, shapeBox.Bottom);
                        _boundingBox = new RectangleF(new PointF(xmin, ymin), new SizeF(xmax - xmin, ymax - ymin));
                    }
                    _shapes.Add(new RectangleShape(_boundingBox, Color.Black));
                    // Render periodically.
                    StartTimer();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message, "Error");
            }
        }

        private void DrawAxis(Graphics g)
        {
            using (Pen pen = new Pen(Color.Black, 0.5f))
            {
                // x-axis.
                g.DrawLine(pen, 0, -renderPanel.Height, 0, renderPanel.Height);
                // y-axis.
                g.DrawLine(pen, -renderPanel.Width, 0, renderPanel.Width, 0);
            }
        }

        private void DrawForm_Activated(object sender, EventArgs e)
        {
            TopMost = true;
        }

        private void RenderPanel_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.Clear(Color.CornflowerBlue);
            e.Graphics.PageUnit = GraphicsUnit.Millimeter;
            e.Graphics.TranslateTransform(origoX, origoY);
            DrawAxis(e.Graphics);
            foreach (var shape in _shapes.Take(_drawCount))
            {
                shape.Draw(e.Graphics);
            }

            if (_drawCount == _shapes.Count)
            {
                MessageBox.Show($"Minimum bounding box coordinates: ({_boundingBox.Bottom},{_boundingBox.Left}), ({_boundingBox.Top},{_boundingBox.Left}), " +
                                $"({_boundingBox.Top},{_boundingBox.Right}), ({_boundingBox.Bottom},{_boundingBox.Right})", "Solution");
            }
        }

        private void DrawForm_Load(object sender, EventArgs e)
        {
            ReadFile();
        }

        private void StartTimer()
        {
            _timer.Interval = delay;
            _timer.Tick += (_s, _e) =>
            {
                _drawCount++;
                renderPanel.Refresh();
                if(_drawCount >= _shapes.Count)
                {
                    _timer.Stop();
                }
            };
            _timer.Start();
        }
    }
}
