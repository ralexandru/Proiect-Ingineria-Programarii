namespace Proiect_IP_w_UI
{
    public partial class Form1 : Form
    {
        private int[,] maze = Utilities.GetMazeFromFile("maze.txt");
        private List<List<Position>> correctPaths;
        private int cellSize;

        public Form1()
        {
            InitializeComponent();
            pictureBox1.Paint += pictureBox1_Paint;

            // Se calculeaza dimensiunea celulei pe baza dimensiunii Form-ului
            CalculateCellSize();

            // PictureBox-ul in care se afiseaza labirintul va fi de dimensiunea labirintului * marimea unei celule
            pictureBox1.Size = new Size(maze.GetLength(1) * cellSize, maze.GetLength(0) * cellSize);

            // Marimea Form-ului va fi egala cu marimea PictureBox-ului
            this.ClientSize = pictureBox1.Size;

            
            this.Resize += Form1_Resize;

            // Se apeleaza metoda FindPath din clasa Utilities pentru rezolvarea labirintului
            Utilities utilities = new Utilities();
            correctPaths = utilities.FindPath();
        }

        private void CalculateCellSize()
        {
            // Calculeaza marimea maxima a celulei 
            int maxWidth = this.ClientSize.Width / maze.GetLength(1);
            int maxHeight = this.ClientSize.Height / maze.GetLength(0);

            // Ma asigur ca nu apar spatii albe
            cellSize = Math.Min(maxWidth, maxHeight);
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            // Se recalculeaza marimea celulelor cand se redimensioneaza formularul
            CalculateCellSize();

            // Se actualizeaza marimea PictureBox-ului pentru a reflecta marimea labirintului
            pictureBox1.Size = new Size(maze.GetLength(1) * cellSize, maze.GetLength(0) * cellSize);

            // Se centreaza PictureBox-ul in formular
            pictureBox1.Location = new Point((this.ClientSize.Width - pictureBox1.Width) / 2, (this.ClientSize.Height - pictureBox1.Height) / 2);

            // Se redeseneaza PictureBox-ul
            pictureBox1.Invalidate();
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            // Deseneaza labirintul
            for (int i = 0; i < maze.GetLength(0); i++)
            {
                for (int j = 0; j < maze.GetLength(1); j++)
                {
                    Brush brush = maze[i, j] == 1 ? Brushes.Black : Brushes.White;
                    g.FillRectangle(brush, j * cellSize, i * cellSize, cellSize, cellSize);
                    // Adauga un text ce indica pozitia curenta 
                    string text = i+","+j; 
                    Font font = new Font("Roboto", 5); 
                    Brush textBrush = Brushes.Red; 

                    // Pozitioneaza textul in mijlocul celulei
                    float x = j * cellSize + (cellSize - g.MeasureString(text, font).Width) / 2;
                    float y = i * cellSize + (cellSize - g.MeasureString(text, font).Height) / 2;

                    g.DrawString(text, font, textBrush, x, y);
                }
            }

            // Drumul corect este evidentiat printr-o culoare distincta
            Pen pathPen = new Pen(Color.Green, 9);
            foreach (List<Position> lista in correctPaths)
            {
                foreach (Position point in lista)
                {
                    g.DrawRectangle(pathPen, point.y * cellSize, point.x * cellSize, cellSize, cellSize);
                }
            }
        }
    }
}
