using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Xml.Linq;

namespace BoxRender
{
    class SchemaBuilder
    {
        private Rectangle parentRectangle;
        private readonly XDocument xdoc;
        private Pen pen;
        private Graphics graph;
        private Bitmap bmp;
        readonly XElement firstElement;
        private readonly string nameOfXMLFile;

        const string rootElement = "folding";

        public SchemaBuilder(string XmlDocName)
        {
            nameOfXMLFile = XmlDocName;
            bmp = new Bitmap(4500, 3000);
            graph = Graphics.FromImage(bmp);
            pen = new Pen(Color.Black);
            xdoc = XDocument.Load(XmlDocName);
            firstElement = xdoc.Elements(rootElement).First();

            parentRectangle = new Rectangle
            {
                X = float.Parse(firstElement.Attribute("rootX").Value),
                Y = float.Parse(firstElement.Attribute("rootY").Value),
                IsRoot = true
            };
        }

        public void Run()
        {
            var FirstItem = xdoc.Element(rootElement).Element("panels").Elements("item").First();
            parentRectangle.Height = float.Parse(FirstItem.Attribute("panelHeight").Value);
            parentRectangle.Width = float.Parse(FirstItem.Attribute("panelWidth").Value);

            graph.DrawRectangle(pen, parentRectangle.X, parentRectangle.Y, parentRectangle.Width, parentRectangle.Height);
            IEnumerable<XElement> attachedElements = FirstItem.Elements("attachedPanels");

            DrawScan(attachedElements.First(), ref parentRectangle, ref bmp, ref graph, ref pen);

            bmp.Save($"{nameOfXMLFile}.png", ImageFormat.Png);
            System.Console.WriteLine($"{nameOfXMLFile} is ready.");
        }

        private void DrawScan(XElement attachedElement, ref Rectangle parentRectangle, ref Bitmap bmp, ref Graphics graph, ref Pen pen)
        {
            IEnumerable<XElement> attachedItems = attachedElement.Elements("item");

            foreach (XElement attachedItem in attachedItems)
            {
                var childRectangle = CreateRectangle(attachedItem);
                CalcRectangle(ref childRectangle, ref parentRectangle);
                graph.DrawRectangle(pen, childRectangle.X + childRectangle.HingeOffset, childRectangle.Y, childRectangle.Width, childRectangle.Height);
                if (attachedItem.Element("attachedPanels") != null)
                {
                    DrawScan(attachedItem.Element("attachedPanels"), ref childRectangle, ref bmp, ref graph, ref pen);
                }
            }
        }
        private int CalcAttachedToSide(Rectangle parentRectangle, Rectangle childRectangle)
        {
            int side = childRectangle.AttachedToSide;
            if (!parentRectangle.IsRoot)
            {
                int[] numbers = { 0, 1, 2, 3, 0 };

                switch (side)
                {
                    case 1:
                        side = numbers[parentRectangle.AttachedToSide - 1];
                        break;
                    case 2:
                        side = numbers[parentRectangle.AttachedToSide];
                        break;
                    case 3:
                        side = numbers[parentRectangle.AttachedToSide + 1];
                        break;
                    default:
                        throw new ArgumentException("BADD!");
                }
            }
            childRectangle.AttachedToSide = side;
            return side;
        }
        private void RotateRectangle(ref Rectangle childRectangle)
        {
            float RectangleWidth = childRectangle.Width;
            float RectangleHeight = childRectangle.Height;
            childRectangle.Width = RectangleHeight;
            childRectangle.Height = RectangleWidth;
        }
        private int CalcRectngleCoords(ref Rectangle childRectangle, ref int side, ref Rectangle parentRectangle)
        {
            float rootWidth = parentRectangle.Width;
            float rootHeight = parentRectangle.Height;
            childRectangle.X = parentRectangle.X;
            childRectangle.Y = parentRectangle.Y;
            switch (side)
            {
                case 0:
                    childRectangle.X += (rootWidth - childRectangle.Width) / 2;
                    childRectangle.Y += rootHeight;
                    break;
                case 1:
                    childRectangle.X += rootWidth;
                    childRectangle.Y += (rootHeight - childRectangle.Height) / 2;
                    break;
                case 2:
                    childRectangle.X += (rootWidth - childRectangle.Width) / 2;
                    childRectangle.Y -= childRectangle.Height;
                    break;
                case 3:
                    childRectangle.X -= childRectangle.Width;
                    childRectangle.Y += (rootHeight - childRectangle.Height) / 2;
                    break;
                default:
                    throw new ArgumentException("The rectangle has only 4 sides!");
            }

            return side;
        }

        private void CalcRectangle(ref Rectangle childRectangle, ref Rectangle parentRectangle)
        {
            int side = CalcAttachedToSide(parentRectangle, childRectangle);

            if (side % 2 == 1)
            {
                RotateRectangle(ref childRectangle);
            }

            side = CalcRectngleCoords(ref childRectangle, ref side, ref parentRectangle);
        }

        private Rectangle CreateRectangle(XElement element)
        {
            Rectangle rectangle = new Rectangle
            {
                Height = float.Parse(element.Attribute("panelHeight").Value),
                Width = float.Parse(element.Attribute("panelWidth").Value),
                AttachedToSide = int.Parse(element.Attribute("attachedToSide").Value),
                HingeOffset = float.Parse(element.Attribute("hingeOffset").Value)
            };
            return rectangle;
        }
    }
}