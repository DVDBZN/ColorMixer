using System;
using System.Drawing;
using System.Windows.Forms;

namespace ColorMixer
{
    public partial class ColorMixer : Form
    {
        bool locked = false;
        int tempR1 = 0;
        int tempR2 = 0;
        int tempG1 = 0;
        int tempG2 = 0;
        int tempB1 = 0;
        int tempB2 = 0;
        private Control activeControl;

        public ColorMixer()
        {
            InitializeComponent();
            //Slider values before change
            tempR1 = red1.Value;
            tempR2 = red2.Value;
            tempG1 = green1.Value;
            tempG2 = green2.Value;
            tempB1 = blue1.Value;
            tempB2 = blue2.Value;
        }

        private void red1_ValueChanged(object sender, EventArgs e)
        {
            //When slider is moved, display change in corresponding label

            //Slider value after change
            int temp2 = red1.Value;
            redLabel1.Text = red1.Value.ToString();
            //Update color
            updateRGB();

            //If mixedColor is locked, change both sliders without changing the mixedColor
            //Active slider changes other slider, otherwise a deadlock occurs 
            if (locked && activeControl == red1)
            {
                //If change is negative
                if (temp2 < tempR1)
                {
                    //Change other slider by same value, but positive
                    red2.Value += tempR1 - temp2;
                }
                else
                {
                    //Change other slider by same value, but negative
                    red2.Value -= temp2 - tempR1;
                }
            }

            //Set slider value before change to after change for next time
            tempR1 = temp2;
        }

        //Same is done for each slider
        private void green1_ValueChanged(object sender, EventArgs e)
        {
            int temp2 = green1.Value;
            greenLabel1.Text = green1.Value.ToString();
            updateRGB();

            if (locked && activeControl == green1)
            {
                if (temp2 < tempG1)
                {
                    green2.Value += tempG1 - temp2;
                }
                else
                {
                    green2.Value -= temp2 - tempG1;
                }
            }

            tempG1 = temp2;
        }

        private void blue1_ValueChanged(object sender, EventArgs e)
        {
            int temp2 = blue1.Value;
            blueLabel1.Text = blue1.Value.ToString();
            updateRGB();

            if (locked && activeControl == blue1)
            {
                if (temp2 < tempB1)
                {
                    blue2.Value += tempB1 - temp2;
                }
                else
                {
                    blue2.Value -= temp2 - tempB1;
                }
            }

            tempB1 = temp2;
        }

        private void red2_ValueChanged(object sender, EventArgs e)
        {
            int temp2 = red2.Value;
            redLabel2.Text = red2.Value.ToString();
            updateRGB();

            if (locked && activeControl == red2)
            {
                if (temp2 < tempR2)
                {
                    red1.Value += tempR2 - temp2;
                }
                else
                {
                    red1.Value -= temp2 - tempR2;
                }
            }

            tempR2 = temp2;
        }

        private void green2_ValueChanged(object sender, EventArgs e)
        {
            int temp2 = green2.Value;
            greenLabel2.Text = green2.Value.ToString();
            updateRGB();

            if (locked && activeControl == green2)
            {
                if (temp2 < tempG2)
                {
                    green1.Value += tempG2 - temp2;
                }
                else
                {
                    green1.Value -= temp2 - tempG2;
                }
            }

            tempG2 = temp2;
        }

        private void blue2_ValueChanged(object sender, EventArgs e)
        {
            int temp2 = blue2.Value;
            blueLabel2.Text = blue2.Value.ToString();
            updateRGB();

            if (locked && activeControl == blue2)
            {
                if (temp2 < tempB2)
                {
                    blue1.Value += tempB2 - temp2;
                }
                else
                {
                    blue1.Value -= temp2 - tempB2;
                }
            }

            tempB2 = temp2;
        }

        private void updateRGB()
        {
            //Update values in RGB textboxes
            rgbTextBox1.Text = red1.Value.ToString() + "," + green1.Value.ToString() + "," + blue1.Value.ToString();
            rgbTextBox2.Text = red2.Value.ToString() + "," + green2.Value.ToString() + "," + blue2.Value.ToString();

            //Update MixedColor textbox
            mixedColorRGB.Text = ((red1.Value + red2.Value) / 2).ToString() + "," + ((green1.Value + green2.Value) / 2).ToString() + "," + ((blue1.Value + blue2.Value) / 2).ToString();
        }

        private void rgbTextBox1_TextChanged(object sender, EventArgs e)
        {
            //When text is changed (either through slider moving it, or manual input), update color
            colorShow1.BackColor = Color.FromArgb(red1.Value, green1.Value, blue1.Value);
        }

        private void rgbTextBox2_TextChanged(object sender, EventArgs e)
        {
            //Same as above
            colorShow2.BackColor = Color.FromArgb(red2.Value, green2.Value, blue2.Value);
        }

        private void mixedColorRGB_TextChanged(object sender, EventArgs e)
        {
            //Same as above
            mixedColor.BackColor = Color.FromArgb((red1.Value + red2.Value) / 2, (green1.Value + green2.Value) / 2, (blue1.Value + blue2.Value) / 2);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int[] targetRGB, firstRGB, secondRGB = { 0, 0, 0 };

            //Update button for when RGB is manually changed
            try
            {
                targetRGB = Array.ConvertAll(mixedColorRGB.Text.Split(','), int.Parse);
                firstRGB = Array.ConvertAll(rgbTextBox1.Text.Split(','), int.Parse);
                secondRGB = Array.ConvertAll(rgbTextBox2.Text.Split(','), int.Parse);
            }
            catch
            {
                MessageBox.Show("Values must be numerical values between 0 and 255.");
                return;
            }

            //If Update button other color is pressed, ignore
            if (firstRGB[0] == red1.Value && firstRGB[1] == green1.Value && firstRGB[2] == blue1.Value)
                return;

            if (locked)
            {
                //Update other color
                try
                {
                    red2.Value = secondRGB[0] + (red1.Value - firstRGB[0]);
                    green2.Value = secondRGB[1] + (green1.Value - firstRGB[1]);
                    blue2.Value = secondRGB[2] + (blue1.Value - firstRGB[2]);

                    red1.Value = firstRGB[0];
                    green1.Value = firstRGB[1];
                    blue1.Value = firstRGB[2];
                }
                catch
                {
                    MessageBox.Show("Values must be numerical values between 0 and 255 and account for constraints.");
                }
            }

            else
            {
                //Update mixed color
                try
                {
                    red1.Value = firstRGB[0];
                    green1.Value = firstRGB[1];
                    blue1.Value = firstRGB[2];

                    mixedColorRGB.Text = ((red1.Value + red2.Value) / 2).ToString() + "," + ((green1.Value + green2.Value) / 2).ToString() + "," + ((blue1.Value + blue2.Value) / 2).ToString();
                }
                catch
                {
                    MessageBox.Show("Values must be numerical values between 0 and 255.");
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            int[] targetRGB, firstRGB, secondRGB = { 0, 0, 0 };

            try
            {
                //Same as above
                targetRGB = Array.ConvertAll(mixedColorRGB.Text.Split(','), int.Parse);
                firstRGB = Array.ConvertAll(rgbTextBox1.Text.Split(','), int.Parse);
                secondRGB = Array.ConvertAll(rgbTextBox2.Text.Split(','), int.Parse);
            }
            catch
            {
                MessageBox.Show("Values must be numerical values between 0 and 255.");
                return;
            }

            //If Update button other color is pressed, ignore
            if (secondRGB[0] == red2.Value && secondRGB[1] == green2.Value && secondRGB[2] == blue2.Value)
                return;

            if (locked)
            {
                //Update other color
                try
                {
                    red1.Value = firstRGB[0] + (red2.Value - secondRGB[0]);
                    green1.Value = firstRGB[1] + (green2.Value - secondRGB[1]);
                    blue1.Value = firstRGB[2] + (blue2.Value - secondRGB[2]);

                    red2.Value = secondRGB[0];
                    green2.Value = secondRGB[1];
                    blue2.Value = secondRGB[2];
                }
                catch
                {
                    MessageBox.Show("Values must be numerical values between 0 and 255 and account for constraints.");
                }
            }

            else
            {
                //Update mixed color
                try
                {
                    red2.Value = secondRGB[0];
                    green2.Value = secondRGB[1];
                    blue2.Value = secondRGB[2];

                    mixedColorRGB.Text = ((red1.Value + red2.Value) / 2).ToString() + "," + ((green1.Value + green2.Value) / 2).ToString() + "," + ((blue1.Value + blue2.Value) / 2).ToString();
                }
                catch
                {
                    MessageBox.Show("Values must be numerical values between 0 and 255.");
                }
            }
        }

        private void update_Click(object sender, EventArgs e)
        {
            //Similar to above
            try
            {
                int[] rgb = Array.ConvertAll(mixedColorRGB.Text.Split(','), int.Parse);
                mixedColor.BackColor = Color.FromArgb(rgb[0], rgb[1], rgb[2]);

                //Since the average of the two colors creates the mixed color,
                //It is possible for a wide range of colors to equal it
                //Rather than finding a two values that average to the mixed value,
                //Just change to slider values to equal the mixed color values
                red1.Value = rgb[0];
                red2.Value = rgb[0];
                green1.Value = rgb[1];
                green2.Value = rgb[1];
                blue1.Value = rgb[2];
                blue2.Value = rgb[2];

                //Prevent certain errors from occuring
                unlockTrackBars();

                //If it was already locked, lock after unlocking
                if (locked == true)
                    lockTrackBars();
            }

            catch
            {
                MessageBox.Show("Values must be numerical values between 0 and 255.");
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            if (locked == false)
            {
                //Change image
                lockPicture.Image = Properties.Resources.Locked;
                //Switch locked variable
                locked = true;
                //Make mixedColor textbox ReadOnly
                mixedColorRGB.ReadOnly = true;

                //Call lock method
                lockTrackBars();
            }

            else
            {
                lockPicture.Image = Properties.Resources.Unlocked;
                locked = false;
                mixedColorRGB.ReadOnly = false;

                unlockTrackBars();
            }
        }

        private void lockTrackBars()
        {
            //Slider max and min values are manipulated to prevent forcing other values into negative or over 255
            if (red1.Value + red2.Value <= 255)
            {
                red1.Maximum = red1.Value + red2.Value;
                red2.Maximum = red1.Value + red2.Value;
            }

            else if (red1.Value + red2.Value > 255)
            {
                red1.Minimum = (red1.Value + red2.Value) - 255;
                red2.Minimum = (red1.Value + red2.Value) - 255;
            }

            if (green1.Value + green2.Value <= 255)
            {
                green1.Maximum = green1.Value + green2.Value;
                green2.Maximum = green1.Value + green2.Value;
            }

            else if (green1.Value + green2.Value > 255)
            {
                green1.Minimum = (green1.Value + green2.Value) - 255;
                green2.Minimum = (green1.Value + green2.Value) - 255;
            }

            if (blue1.Value + blue2.Value <= 255)
            {
                blue1.Maximum = blue1.Value + blue2.Value;
                blue2.Maximum = blue1.Value + blue2.Value;
            }

            else if (blue1.Value + blue2.Value > 255)
            {
                blue1.Minimum = (blue1.Value + blue2.Value) - 255;
                blue2.Minimum = (blue1.Value + blue2.Value) - 255;
            }
        }

        private void unlockTrackBars()
        {
            //Reset max and min values to default
            red1.Maximum = 255;
            red1.Minimum = 0;
            red2.Maximum = 255;
            red2.Minimum = 0;

            green1.Maximum = 255;
            green1.Minimum = 0;
            green2.Maximum = 255;
            green2.Minimum = 0;

            blue1.Maximum = 255;
            blue1.Minimum = 0;
            blue2.Maximum = 255;
            blue2.Minimum = 0;
        }

        private void red1_MouseEnter(object sender, EventArgs e)
        {
            //If mixedColor is locked, active slider is chosen to control other slider
            activeControl = red1;
        }

        private void red2_MouseEnter(object sender, EventArgs e)
        {
            activeControl = red2;
        }

        private void green1_MouseEnter(object sender, EventArgs e)
        {
            activeControl = green1;
        }

        private void green2_MouseEnter(object sender, EventArgs e)
        {
            activeControl = green2;
        }

        private void blue1_MouseEnter(object sender, EventArgs e)
        {
            activeControl = blue1;
        }

        private void blue2_MouseEnter(object sender, EventArgs e)
        {
            activeControl = blue2;
        }

        //Reset activeControl to prevent last active slider from changing other slider during mixedColor update
        private void MouseExit(object sender, EventArgs e)
        {
            activeControl = mixedColor;
        }
    }
}