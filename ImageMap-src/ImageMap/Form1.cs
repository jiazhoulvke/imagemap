using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace ImageMap
{
    public partial class Form1 : Form
    {
        bool MouseIsDown = false;
        Rectangle MouseRect = Rectangle.Empty;
        Point p1, p2;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Image img = Image.FromFile(Program.filename);
            this.BackgroundImage = img;
            this.Width = img.Width;
            this.Height = img.Height;
            this.Width = this.Width - this.ClientSize.Width + this.Width;
            this.Height = this.Height - this.ClientSize.Height + this.Height;
            Program.result = "<img src=\"" + Program.filename + "\" border=\"0\" usemap=\"#Map\" />";
            Program.result += "<map name=\"Map\" id=\"Map\">";
        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            MouseIsDown = true;
            DrawStart(e.Location);
            p1.X = e.X;
            p1.Y = e.Y;
        }

        private void Form1_MouseUp(object sender, MouseEventArgs e)
        {
            this.Capture = false;
            Cursor.Clip = Rectangle.Empty;
            MouseIsDown = false;
            p2.X = e.X;
            p2.Y = e.Y;

            if (p1.X == p2.X && p1.Y == p2.Y)
                return;

            string inputResult;
            inputResult = InputBox("链接属性:", "");

            if (inputResult != null)
            {
                string[] inputarray = inputResult.Split(new char[] { ',' });
                string target = "";
                if (inputarray[0].Length > 0)
                    target = " target=\"" + inputarray[0] + "\"";
                string link = string.Format("<area shape=\"rect\" coords=\"{0},{1},{2},{3}\" href=\"{4}\" {5} />", Min(p1.X, p2.X), Min(p1.Y, p2.Y), Max(p1.X, p2.X), Max(p1.Y, p2.Y), inputarray[1], target);
                Program.result += link;
                Program.linkNum += 1;
            }
            else
            {
                DrawRectangle();
            }
            MouseRect = Rectangle.Empty;
        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            if (MouseIsDown)
                ResizeToRectangle(e.Location);
        }

        private void ResizeToRectangle(Point p)
        {
            DrawRectangle();
            MouseRect.Width = p.X - MouseRect.Left;
            MouseRect.Height = p.Y - MouseRect.Top;
            DrawRectangle();
        }

        private void DrawRectangle()
        {
            Rectangle rect = this.RectangleToScreen(MouseRect);
            ControlPaint.DrawReversibleFrame(rect, Color.White, FrameStyle.Dashed);
        }

        private void DrawStart(Point StartPoint)
        {
            this.Capture = true;
            Cursor.Clip = this.RectangleToScreen(new Rectangle(0, 0, ClientSize.Width, ClientSize.Height));
            MouseRect = new Rectangle(StartPoint.X, StartPoint.Y, 0, 0);
        }

        private int Max(int a, int b)
        {
            if (a > b)
                return a;
            else
                return b;
        }

        private int Min(int a, int b)
        {
            if (a < b)
                return a;
            else
                return b;
        }

        private string InputBox(string Caption, string Default)
        {
            Form InputForm = new Form();
            InputForm.MinimizeBox = false;
            InputForm.MaximizeBox = false;
            InputForm.StartPosition = FormStartPosition.CenterScreen;
            InputForm.Width = 220;
            InputForm.Height = 130;
            InputForm.Text = Caption;

            ComboBox cb = new ComboBox();
            cb.Items.AddRange(new string[] { "_blank", "_top", "_self", "_parent" });
            cb.Parent = InputForm;
            cb.Left = 30;
            cb.Top = 10;
            cb.Width = 160;

            TextBox tb = new TextBox();
            tb.Left = 30;
            tb.Top = 40;
            tb.Width = 160;
            tb.Parent = InputForm;
            tb.Text = Default;
            tb.SelectAll();

            Button btnok = new Button();
            btnok.Left = 30;
            btnok.Top = 70;
            btnok.Parent = InputForm;
            btnok.Text = "确定";
            InputForm.AcceptButton = btnok;
            InputForm.CancelButton = btnok;

            btnok.DialogResult = DialogResult.OK;
            Button btncancal = new Button();
            btncancal.Left = 120;
            btncancal.Top = 70;
            btncancal.Parent = InputForm;
            btncancal.Text = "取消";
            btncancal.DialogResult = DialogResult.Cancel;
            try
            {
                if (InputForm.ShowDialog() == DialogResult.OK)
                {
                    return cb.Text + "," + tb.Text;
                }
                else
                {
                    return null;
                }
            }
            finally
            {
                InputForm.Dispose();
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Program.result += "</map>";
        }
    }
}
