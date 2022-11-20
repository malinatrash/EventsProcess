﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventsProcess.Objects
{
    internal class RedObject : BaseObject
    {
        public Action<GreenObject>? TimeOut;
        public int x1 = -10;
        public int x2 = 20;
        public int y1 = -10;
        public int y2 = 20;
        public RedObject(float x, float y, float angle) : base(x, y, angle)
        {

        }

        public override void Render(Graphics g)
        {
            SolidBrush fillColor = new SolidBrush(Color.IndianRed);
            g.FillEllipse(fillColor, x1, y1, x2, y2);
        }

        public override GraphicsPath GetGraphicsPath()
        {
            var path = base.GetGraphicsPath();
            path.AddEllipse(x1, y1, x2, y2);
            return path;
        }

        public void UpdateSize()
        {
            x1 -= 1; x2 += 2; y1 -= 1; y2 += 2;
        }

        public void ResetSize()
        {
            x1 = -10;
            x2 = 20;
            y1 = -10;
            y2 = 20;
        }
    }
}
