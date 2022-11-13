using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventsProcess.Objects
{
    class GreenObject : BaseObject
    {
        public Action<GreenObject> TimeOut;
        public int time = 100;
        public GreenObject(float x, float y, float angle) : base(x, y, angle)
        {
        }
        public override void Render(Graphics g)
        {
            g.FillEllipse(new SolidBrush(Color.DarkSeaGreen), -10, -10, 20, 20);

            g.DrawString(
            $"{UpdateTime()}",
             new Font("Verdana", 10),
             new SolidBrush(Color.Green),
             10, 10
             );
        }

        public override GraphicsPath GetGraphicsPath()
        {
            var path = base.GetGraphicsPath();
            path.AddEllipse(-10, -10, 20, 20);
            return path;
        }

        public int UpdateTime()
        {
            time -= 1;
            return time;
        }

        public override void Overlap(BaseObject obj)
        {
            if (obj is GreenObject)
            {
                TimeOut(obj as GreenObject);
            }
        }
    }
}
