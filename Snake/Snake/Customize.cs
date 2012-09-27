using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Xna.Framework;

namespace Snake
{
    public partial class Customize : Form
    {
        public Customize()
        {
            InitializeComponent();

            iName.Text = SnakeManager.Instance().LocalName;
        }

        private void iName_TextChanged(object sender, EventArgs e)
        {
            SnakeManager.Instance().LocalName = iName.Text;
        }

        private void iOkay_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void iColor_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch(iColor.Items[iColor.SelectedIndex].ToString())
            {
                case "Red":
                    SnakeManager.Instance().LocalCustomTint = Color.Red;
                    break;
                case "Blue":
                    SnakeManager.Instance().LocalCustomTint = Color.Blue;
                    break;

                case "Green":
                    SnakeManager.Instance().LocalCustomTint = Color.Green;
                    break;

                case "Yellow":
                    SnakeManager.Instance().LocalCustomTint = Color.Yellow;
                    break;

                case "Orange":
                    SnakeManager.Instance().LocalCustomTint = Color.Orange;
                    break;

                case "Purple":
                    SnakeManager.Instance().LocalCustomTint = Color.Purple;
                    break;

                case "Navy":
                    SnakeManager.Instance().LocalCustomTint = Color.Navy;
                    break;
                case "Pink":
                    SnakeManager.Instance().LocalCustomTint = Color.Pink;
                    break;
                case "LimeGreen":
                    SnakeManager.Instance().LocalCustomTint = Color.LimeGreen;
                    break;
                case "Gold":
                    SnakeManager.Instance().LocalCustomTint = Color.Gold;
                    break;
                case "Black":
                    SnakeManager.Instance().LocalCustomTint = Color.Black;
                    break;
                case "AliceBlue":
                    SnakeManager.Instance().LocalCustomTint = Color.AliceBlue;
                    break;


            }
        }
    }
}
