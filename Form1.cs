using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication7
{
    public partial class Form1 : Form
    {
        bool lost = false;
        static int fieldSize = 20;
        int cellSize = 20;
        int margin = -5;
        int bombs = 50;
        int[,] field = new int[fieldSize, fieldSize];

        public Form1()
        {
            InitializeComponent();
            int x = 20;
            int y = 20;
            for (int i = 0; i < fieldSize; i++)
            {
                for (int k = 0; k < fieldSize; k++)
                {
                    x += margin + fieldSize;
                    CreateField(x, y);
                }
                x = 20;
                y += margin + fieldSize;
            }
            plantBombs();

        }

        private void CreateField(int x, int y)
        {
            Button newButton = new Button();
            this.Controls.Add(newButton);
            newButton.Location = new Point(x, y);
            newButton.Size = new Size(fieldSize, fieldSize);
            newButton.MouseUp += new MouseEventHandler(FieldClick);
            newButton.Tag = 0;
        }

        private void FieldClick(object sender, MouseEventArgs e)
        {
            try
            {
                if (e.Button == MouseButtons.Right)
                {
                    if ((sender as Button).BackColor != Color.Red) (sender as Button).BackColor = Color.Red;
                    else (sender as Button).BackColor = default(Color);
                }
                else
                {
                    if (Convert.ToInt32((sender as Button).Tag) < 0) GameOver();
                    ShowCell((sender as Button).TabIndex);
                }
            } catch (Exception ex) { }
            if (checkWin()) Win();
        }

        private void plantBombs()
        {
            Random rand = new Random();
            for (int i = 0; i < bombs; i++)
            {
                int index = rand.Next(0, fieldSize * fieldSize);
                Button button = (getElementByTabIndex(index) as Button);
                if (Convert.ToInt32(button.Tag) < 0)
                {
                    i--;
                    continue;
                }
                button.Tag = -9999;

                button.Tag = Convert.ToInt32(button.Tag) + 1;

                if ((index + 1) % fieldSize != 0)
                {
                    try { (getElementByTabIndex(index + 1) as Button).Tag = Convert.ToInt32((getElementByTabIndex(index + 1) as Button).Tag) + 1; } catch (Exception e) { }
                    try { (getElementByTabIndex(index + 1 + fieldSize) as Button).Tag = Convert.ToInt32((getElementByTabIndex(index + 1 + fieldSize) as Button).Tag) + 1; } catch (Exception e) { }
                    try { (getElementByTabIndex(index + 1 - fieldSize) as Button).Tag = Convert.ToInt32((getElementByTabIndex(index + 1 - fieldSize) as Button).Tag) + 1; } catch (Exception e) { }
                }

                if (index < fieldSize * (fieldSize - 1)) try { (getElementByTabIndex(index + fieldSize) as Button).Tag = Convert.ToInt32((getElementByTabIndex(index + fieldSize) as Button).Tag) + 1; } catch (Exception e) { }
                if (index > fieldSize) try { (getElementByTabIndex(index - fieldSize) as Button).Tag = Convert.ToInt32((getElementByTabIndex(index - fieldSize) as Button).Tag) + 1; } catch (Exception e) { }

                if ((index) % fieldSize != 0)
                {
                    try { (getElementByTabIndex(index - 1) as Button).Tag = Convert.ToInt32((getElementByTabIndex(index - 1) as Button).Tag) + 1; } catch (Exception e) { }
                    try { (getElementByTabIndex(index - 1 + fieldSize) as Button).Tag = Convert.ToInt32((getElementByTabIndex(index - 1 + fieldSize) as Button).Tag) + 1; ; } catch (Exception e) { }
                    try { (getElementByTabIndex(index - 1 - fieldSize) as Button).Tag = Convert.ToInt32((getElementByTabIndex(index - 1 - fieldSize) as Button).Tag) + 1; } catch (Exception e) { }
                }
            }

        }

        private void ShowField()
        {
            int index = 0;
            for (int i = 0; i < fieldSize; i++)
            {
                for (int k = 0; k < fieldSize; k++)
                {
                    try
                    {
                        int value = Convert.ToInt32((getElementByTabIndex(index) as Button).Tag);
                        (getElementByTabIndex(index) as Button).Text = (getElementByTabIndex(index) as Button).Tag.ToString();
                        if (value < 0) (getElementByTabIndex(index) as Button).BackColor = Color.LightPink;
                    } catch(Exception e) { }
                    index++;
                }
            }
        }

        private void ShowCell(int index)
        {
            try
            {
                Button button = (getElementByTabIndex(index) as Button);
                button.Text = button.Tag.ToString();

                int number = Convert.ToInt32(button.Tag);
                if (number == 1) button.ForeColor = Color.Green;
                if (number == 2) button.ForeColor = Color.Purple;
                if (number == 3) button.ForeColor = Color.Red;
                if (number == 4) button.ForeColor = Color.DarkRed;
                if (number == 5) button.ForeColor = Color.Cyan;

                if (Convert.ToInt32(button.Tag) == 0)
                {
                    button.Tag = "";
                    button.Text = "";
                    button.BackColor = Color.Gray;
                    button.FlatStyle = FlatStyle.Flat;
                    button.FlatAppearance.BorderColor = Color.Gray;


                    if ((index + 1) % fieldSize != 0)
                    {
                        ShowCell(index + 1);
                        ShowCell(index + 1 + fieldSize);
                        ShowCell(index + 1 - fieldSize);
                    }

                    if (index < fieldSize * (fieldSize - 1)) ShowCell(index + fieldSize);
                    if (index > fieldSize) ShowCell(index - fieldSize);

                    if ((index) % fieldSize != 0)
                    {
                        ShowCell(index - 1);
                        ShowCell(index - 1 + fieldSize);
                        ShowCell(index - 1 - fieldSize);
                    }
                    
                }
            } catch (Exception e) { }
        }
        
        private void GameOver()
        {
            lost = true;
            ShowField();
        }

        private void Win()
        {
            string message = "Holy smokes! Looks like you won!";
            string caption = "Win";
            MessageBox.Show(message, caption);
        }

        private bool checkWin()
        {
            if (lost) return false;
            for (int i = 0; i < fieldSize * fieldSize; i++)
            {
                object element = getElementByTabIndex(i);
                try
                {
                    if ((element as Button).Text == "" && Convert.ToInt32((element as Button).Tag) >= 0) return false;
                } catch (Exception e) { }
            }
            return true;
        }

        private object getElementByTabIndex(int index)
        {
            foreach (Control control in Controls)
            {
                if (control.TabIndex == index)
                {
                    return control;
                }
            }
            return new object { };
        }

    }
}
