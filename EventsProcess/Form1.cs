using EventsProcess.Objects;

namespace EventsProcess
{
    public partial class Form1 : Form
    {
        Player player;
        List<BaseObject> objects = new();
        Marker marker;
        GreenObject greenObject;
        GreenObject secondGreenObject;
        int countOfOverlaps = 0;
        public Form1()
        {
            InitializeComponent();
            player = new Player(pictureBox.Width / 2, pictureBox.Height / 2, 0);

            SpawnGreenObject(greenObject);
            SpawnGreenObject(secondGreenObject);

            player.OnOverlap += (p, obj) =>
            {
                txtLog.Text = $"[{DateTime.Now:HH:mm:ss:ff}] Игрок пересекся с {obj}\n" + txtLog.Text;
            };

            player.OnMarkerOverlap += (m) =>
            {
                objects.Remove(m);
                marker = null;
            };
            objects.Add(player);

            player.OnGreenObjectOverlap += (m) =>
            {
                objects.Remove(m);
                greenObject = null;
                UpdateCountOfOverlaps();
                SpawnGreenObject(m);
            };
        }

        private void pictureBox_Paint(object sender, PaintEventArgs e)
        {
            var g = e.Graphics;
            g.Clear(Color.White);

            UpdatePlayer();

            foreach (var obj in objects.ToList())
            {
                if (obj != player && player.Overlaps(obj, g))
                {
                    player.Overlap(obj);
                    obj.Overlap(player);
                }
            }

            foreach (var obj in objects)
            {
                g.Transform = obj.GetTransform();
                obj.Render(g);
            }
        }

        private void UpdateCountOfOverlaps()
        {
            countOfOverlaps++;
            label1.Text = $"Очки: {countOfOverlaps}";
        }

        private void DeleteGreenObject(GreenObject greenObject)
        {

        }

        private void SpawnGreenObject(GreenObject greenObject)
        {
            greenObject = new GreenObject(0, 0, 0);
            objects.Add(greenObject);

            Random random = new Random();
            int randomX = random.Next(0, pictureBox.Width);
            int randomY = random.Next(0, pictureBox.Height);

            greenObject.X = randomX;
            greenObject.Y = randomY;
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

            greenObject.GetLength(player.X, player.Y);
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