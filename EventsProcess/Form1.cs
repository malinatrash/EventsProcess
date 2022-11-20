using EventsProcess.Objects;
using System;
using System.Collections.Specialized;

namespace EventsProcess
{
    public partial class Form1 : Form
    {
        Player player;
        List<BaseObject> objects = new();
        Marker marker;
        RedObject redObject;
        int countOfOverlaps = 0;

        public Form1()
        {
            InitializeComponent();

            player = new Player(pictureBox.Width / 2, pictureBox.Height / 2, 0);

            player.OnOverlap += (p, obj) =>
            {
                txtLog.Text = $"[{DateTime.Now:HH:mm:ss}] Игрок пересекся с {obj}\n\n" + txtLog.Text;
            };

            player.OnMarkerOverlap += (m) =>
            {
                objects.Remove(m);
                marker = null;
            };

            player.OnGreenObjectOverlap += (m) =>
            {
                ResetGreenObject(m);
                UpdateCountOfOverlaps(true);
            };

            player.OnRedObjectOverlap += (m) =>
            {
                ResetRedObject(m); 
                UpdateCountOfOverlaps(false);
            };

            redObject = new RedObject(0, 0, 0);
            objects.Add(redObject);

            Random random = new Random();
            int randomX = random.Next(0, pictureBox.Width);
            int randomY = random.Next(0, pictureBox.Height);

            redObject.X = randomX;
            redObject.Y = randomY;

            objects.Add(player);
            objects.Add(CreateGreenObject());
            objects.Add(CreateGreenObject());

            //поиск кругов в списке объектов
            foreach (var obj in objects.ToList())
            {
                if (obj is GreenObject greenObject)
                {
                    greenObject.TimeOut += (c) =>
                    {
                        ResetGreenObject(c);
                    };
                }
            }
        }

        private void pictureBox_Paint(object sender, PaintEventArgs e)
        {
            var g = e.Graphics;
            g.Clear(Color.White);

            UpdatePlayer();
            redObject.UpdateSize();

            foreach (var obj in objects.ToList())
            {
                if (obj != player && player.Overlaps(obj, g))
                {
                    player.Overlap(obj);
                    obj.Overlap(player);
                }

                if (obj is GreenObject greenObject)
                {
                    greenObject.time -= 1;
                    greenObject.TimeOver();
                }
            }

            foreach (var obj in objects)
            {
                g.Transform = obj.GetTransform();
                obj.Render(g);
            }
        }

        private void UpdateCountOfOverlaps(bool value)
        {
            if (value == true)
            {
                countOfOverlaps++;
            } else
            {
                countOfOverlaps--;
            }
            label1.Text = $"Очки: {countOfOverlaps}";
        }

        private void ResetGreenObject(GreenObject greenObject)
        {
            Random random = new Random();
            int randomX = random.Next(0, pictureBox.Width);
            int randomY = random.Next(0, pictureBox.Height);

            greenObject.X = randomX;
            greenObject.Y = randomY;
            greenObject.time = 200;
        }

        private void ResetRedObject(RedObject redObject)
        {
            Random random = new Random();
            int randomX = random.Next(0, pictureBox.Width);
            int randomY = random.Next(0, pictureBox.Height);

            redObject.ResetSize();

            redObject.X = randomX;
            redObject.Y = randomY;
        }

        private GreenObject CreateGreenObject()
        {
            GreenObject greenObject = new GreenObject(0, 0, 0);
            objects.Add(greenObject);

            Random random = new Random();
            int randomX = random.Next(0, pictureBox.Width);
            int randomY = random.Next(0, pictureBox.Height);

            greenObject.X = randomX;
            greenObject.Y = randomY;
            return greenObject;
        }

        private void UpdatePlayer()
        {
            if (marker != null)
            {
                float dx = marker.X - player.X;
                float dy = marker.Y - player.Y;

                float length = MathF.Sqrt(dx * dx + dy * dy);
                dx /= length;
                dy /= length;

                player.vX += dx * 0.5f;
                player.vY += dy * 0.5f;

                player.Angle = 90 - MathF.Atan2(player.vX, player.vY) * 180 / MathF.PI;
            }
            player.vX += -player.vX * 0.1f;
            player.vY += -player.vY * 0.1f;

            player.X += player.vX;
            player.Y += player.vY;
        }
        
        private void timer1_Tick(object sender, EventArgs e)
        {
            pictureBox.Invalidate();
        }

        private void pictureBox_MouseClick(object sender, MouseEventArgs e)
        {
            if (marker == null)
            {
                marker = new Marker(0, 0, 0);
                objects.Add(marker);
            }

            marker.X = e.X;
            marker.Y = e.Y;
        }

        private void pictureBox_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}