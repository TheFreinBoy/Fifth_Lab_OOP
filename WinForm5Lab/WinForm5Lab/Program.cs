using System;
using System.Drawing;
using System.Windows.Forms;

namespace FiguresDrawing
{
    public abstract class Figure
    {
        public int CenterX { get; set; }
        public int CenterY { get; set; }
        public abstract void DrawBlack(Graphics g);
        public void MoveRight(int step)
        {
            CenterX += step;
        }
    }
    public class Circle : Figure
    {
        public int Radius { get; set; }
        public override void DrawBlack(Graphics g)
        {
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            g.DrawEllipse(Pens.Black, CenterX - Radius, CenterY - Radius, Radius * 2, Radius * 2);
        }
    }

    public class Square : Figure
    {
        public int SideLength { get; set; }
        public override void DrawBlack(Graphics g)
        {
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            g.DrawRectangle(Pens.Black, CenterX - SideLength / 2, CenterY - SideLength / 2, SideLength, SideLength);
        }
    }
    public class Rhomb : Figure
    {
        public int HorDiagLen { get; set; }
        public int VertDiagLen { get; set; }

        public override void DrawBlack(Graphics g)
        {
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            Point[] points =
            {
                new Point(CenterX, CenterY - VertDiagLen / 2),
                new Point(CenterX + HorDiagLen / 2, CenterY),
                new Point(CenterX, CenterY + VertDiagLen / 2),
                new Point(CenterX - HorDiagLen / 2, CenterY)
            };
            g.DrawPolygon(Pens.Black, points);

        }
    }

    public class MainForm : Form
    {       
        private Timer timer;
        private Figure currentFigure;
        private int stepsLeft;

        public MainForm()
        {
            DoubleBuffered = true;
            this.Text = "Малювання фігур";
            this.Size = new Size(800, 600);
            this.BackColor = Color.White;
            timer = new Timer();
            timer.Interval = 50; 
            timer.Tick += Timer_Tick;

            var moveCircleButton = new Button { Text = "Move Circle", Top = 10, Left = 10 };
            var moveSquareButton = new Button { Text = "Move Square", Top = 40, Left = 10 };
            var moveRhombButton = new Button { Text = "Move Rhomb", Top = 70, Left = 10 };

            moveCircleButton.Click += MoveCircleButton_Click;
            moveSquareButton.Click += MoveSquareButton_Click;
            moveRhombButton.Click += MoveRhombButton_Click;


            this.Controls.Add(moveCircleButton);
            this.Controls.Add(moveSquareButton);
            this.Controls.Add(moveRhombButton);
        }
        private void MoveCircleButton_Click(object sender, EventArgs e)
        {
            StartMoving(new Circle { CenterX = 50, CenterY = 200, Radius = 50 });
        }

        private void MoveSquareButton_Click(object sender, EventArgs e)
        {
            StartMoving(new Square { CenterX = 50, CenterY = 300, SideLength = 80 });
        }

        private void MoveRhombButton_Click(object sender, EventArgs e)
        {
            StartMoving(new Rhomb { CenterX = 50, CenterY = 400, HorDiagLen = 120, VertDiagLen = 80 });
        }

        private void StartMoving(Figure figure)
        {
            currentFigure = figure;
            stepsLeft = 160; 
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (stepsLeft > 0 && currentFigure != null)
            {
                currentFigure.MoveRight(5); 
                this.Invalidate();
                stepsLeft--;
            }
            else
            {
                timer.Stop();
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            if (currentFigure != null)
            {
                currentFigure.DrawBlack(e.Graphics);
            }
        }
    }

    // Точка входу
    public static class Program
    {
        [STAThread]
        public static void Main()
        {

            
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }
}
