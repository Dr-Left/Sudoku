using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sudoku
{
    public partial class Form1 : Form
    {
        TextBox[] textBoxes= new TextBox[100];
        public Form1()
        {
            InitializeComponent();
            int Top = 10,Left=10;
            for (int i = 0; i < 9; i++){
                if (i % 3 == 0)
                        Top += 10;
                for (int j = 0; j < 9; j++)
                {
                    textBoxes[i * 9 + j] = new TextBox();
                    textBoxes[i * 9 + j].Top = Top;
                    if (j % 3 == 0)
                        Left += 10;
                    textBoxes[i * 9 + j].MaxLength = 1;
                    textBoxes[i * 9 + j].Left = Left;
                    textBoxes[i * 9 + j].Font = new Font("微软雅黑", 18, textBoxes[i * 9 + j].Font.Style | FontStyle.Bold);
                    textBoxes[i * 9 + j].Height = textBoxes[i * 9 + j].Width = 40;
                    textBoxes[i * 9 + j].TextAlign = HorizontalAlignment.Center;
                    textBoxes[i * 9 + j].KeyPress += new KeyPressEventHandler(textBoxes_KeyPress);
                    this.panel2.Controls.Add(textBoxes[i * 9 + j]);
                    Left += 40;
                }
                Top += 40;
                Left = 10;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void textBoxes_KeyPress(object sender, KeyPressEventArgs e)
        {
            TextBox textBox = (sender as TextBox);
            if(e.KeyChar == 37)
            {
                SelectNextControl(textBox, false, true, true, true);
                return;
            }
            if (e.KeyChar == 13 || e.KeyChar == 32)
            {
                if(textBox.TextLength > 0 &&(!(textBox.Text[0]>='0' && textBox.Text[0]<='9')))
                    textBox.Clear();
            }
            if (e.KeyChar == 8)
            {
                if(textBox.TextLength>0 &&!(textBox.Text[0]>='0' && textBox.Text[0]<='9'))
                    textBox.Clear();
                SelectNextControl(textBox, false, true, true, true);
                return;
            }
            SelectNextControl(textBox, true, true, true, true);
        }
        public int[,] num = new int[10,10];
        public bool sign = new bool();
        public bool Check(int n, int key)
        {
            /* 判断n所在横列是否合法 */
            for (int i = 0; i < 9; i++)
            {
                /* j为n竖坐标 */
                int j = n / 9;
                if (num[j,i] == key) return false;
            }

            /* 判断n所在竖列是否合法 */
            for (int i = 0; i < 9; i++)
            {
                /* j为n横坐标 */
                int j = n % 9;
                if (num[i,j] == key) return false;
            }

            /* x为n所在的小九宫格左顶点竖坐标 */
            int x = n / 9 / 3 * 3;

            /* y为n所在的小九宫格左顶点横坐标 */
            int y = n % 9 / 3 * 3;

            /* 判断n所在的小九宫格是否合法 */
            for (int i = x; i < x + 3; i++)
            {
                for (int j = y; j < y + 3; j++)
                {
                    if (num[i,j] == key) return false;
                }
            }

            /* 全部合法，返回正确 */
            return true;
        }

        /* 深搜构造数独 */
        public int DFS(int n)
        {
            /* 所有的都符合，退出递归 */
            if (n > 80)
            {
                sign = true;
                return 0;
            }
            /* 当前位不为空时跳过 */
            if (num[n / 9,n % 9] != 0)
            {
                DFS(n + 1);
            }
            else
            {
                /* 否则对当前位进行枚举测试 */
                for (int i = 1; i <= 9; i++)
                {
                    /* 满足条件时填入数字 */
                    if (Check(n, i) == true)
                    {
                        num[n / 9,n % 9] = i;
                        /* 继续搜索 */
                        DFS(n + 1);
                        /* 返回时如果构造成功，则直接退出 */
                        if (sign == true) return 0;
                        /* 如果构造不成功，还原当前位 */
                        num[n / 9,n % 9] = 0;
                    }
                }
            }
            return 0;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            
            for(int i = 0; i < 9; i++)
            {
                for(int j = 0; j < 9; j++)
                {
                    if (textBoxes[i * 9 + j].TextLength == 0 || textBoxes[i * 9 + j].Text[0] == ' ')
                        num[i, j] = 0;
                    else
                        num[i, j] = Convert.ToInt32(textBoxes[i * 9 + j].Text);
                }
            }
            DFS(0);
            if (sign == false)
                MessageBox.Show("No Solution");
            else
            for(int i = 0; i < 9; i++)
            {
                for(int j = 0; j < 9; j++)
                {
                    textBoxes[i * 9 + j].Text = Convert.ToString(num[i, j]);
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            for(int i = 0; i < 9; i++)
            {
                for(int j = 0; j < 9; j++)
                {
                    textBoxes[i * 9 + j].Clear();
                    num[i, j] = 0;
                }
            }
            sign = false;
        }
    }
}
