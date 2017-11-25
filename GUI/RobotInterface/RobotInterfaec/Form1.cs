using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Ports;
using System.Drawing.Drawing2D;

namespace RobotInterface
{
    public partial class RobotGUI : Form
    {
        //Compass property
        private const int COMPASS_INTERVAL = 500;

        //Slider properties
        private const int DEFAULT_SLIDER_VALUE = 0;
        private const int MIN_SLIDER_VALUE     = -100;
        private const int MAX_SLIDER_VALUE     = 100;
        private const int STEPSIZE             = 20;

        //This property holds the value of the compass position
        //The value ranges between 90 and 360
        private int compass_direction = 0;

        //Initialise the boolean properties 
        //isCOMavail is either set true or false if the COM port is available
        private bool isCOMavail = false;
        //isCOMavail is either set true or false
        private bool isAutoMode = false;

        //Create a new object of SerialPort
        SerialPort COMPort;

       /**
        * RobotGUI
        *
        * This function is the contructor of the class RobotGUI
        * The function initialises the COM port connection, sets the compass
        * on the default direction (north) and attaches an on recievehandler
        * to the COM port.
        *
        * @param none
        * @return none
        */
        public RobotGUI()
        {
            InitializeComponent();

            //init the compass direction
            pb_compass.Image = RotateImage(RobotInterfaec.Properties.Resources.Compass, 360);

            //change the layout of the checkbox to the layout of a button
            cbx_mode.Appearance =  System.Windows.Forms.Appearance.Button;

            var availPorts = SerialPort.GetPortNames();
            foreach (var com_port in availPorts)
            {
                cbx_com_ports.Items.Add(com_port);
            }
        }

       /**
        * RotateImage
        *
        * This function is a eventhandler which only executes if it notices incoming data
        * The function reads compass data and changes the position of the images based on the incoming data
        *
        * @access private
        * @param object sender (standard)
        * @param SerialDataReceivedEventArgs e (standard)
        * @return void
        */
        private void DataReceivedHandler(object sender, SerialDataReceivedEventArgs e)
        {
            //Check if the robot is in autonomous mode
            if (isAutoMode)
            {   
                //Create a asynchronous delay/thread
                var t = Task.Run(async delegate{await Task.Delay(30); return 42;});

                //Start delay
                t.Wait();
                
                //Create object based on the sender and convert it to a SerialPort object
                SerialPort serialPort_obj = (SerialPort)sender;

                //Read the incoming values of the COM port
                string compassPos = serialPort_obj.ReadExisting();

                //Declare a temporary variable for parsing
                int tmpInt;
                
                //Check if the string can be converted to a int and if the val contains only numbers (int)
                // M100 returns false 100 returns true
                bool isNum = int.TryParse(compassPos, out tmpInt);

                //Check if the len of the int is larger than of equal to 2 e.g. 90 or 180 or 360
                //And if there is a number
                if (compassPos.Length >= 2 && isNum)
                {
                    //Convert the string to a int
                    compass_direction = Convert.ToInt16(compassPos);

                    //Change the position of the image
                    pb_compass.Image = RotateImage(RobotInterfaec.Properties.Resources.Compass, compass_direction);
                }
            }
        }

       /**
        * RotateImage
        *
        * This function is responsible for rotating the compass image (bitmap)
        *
        * @access private
        * @param Image img contains the bitmap image
        * @param float rotationAngle contains position of the compass 90 to 360
        * @return Image (new bitmap)
        */
        private static Image RotateImage(Image img, float rotationAngle)
        {   
            //Create a new bitmap based on the current bitmap height and width
            Bitmap temp_bmp = new Bitmap(img.Width, img.Height);

            //Create a graphic of the temp_bmp
            //The Graphic is editable the bitmap isn't
            Graphics gfx = Graphics.FromImage(temp_bmp);
            gfx.TranslateTransform((float)temp_bmp.Width / 2, (float)temp_bmp.Height / 2);
            gfx.RotateTransform(rotationAngle);
            gfx.TranslateTransform(-(float)temp_bmp.Width / 2, -(float)temp_bmp.Height / 2);
            gfx.InterpolationMode = InterpolationMode.HighQualityBicubic;
            gfx.DrawImage(img, new Point(0, 0));
            gfx.Dispose();

            return temp_bmp;
        }

       /**
        * RobotGUI_KeyUp
        *
        * This function is triggered when there is a keyUp event is recognized
        * on the GUI. The function allows the user to modify the values of the
        * sliders with the keys W, A, S, D and Q
        * 
        * @access private
        * @param object sender (standard)
        * @param KeyEventArgs e holds the value of the key events
        * @return void
        */
        private void RobotGUI_KeyUp(object sender, KeyEventArgs e)
        {
            /* 
             * Check if the COM port is available and 
             * if the robot is not in autonomous mode
             */
            if (isCOMavail && !isAutoMode)
            {
                //Initialize the variable for the new val of the slider
                int new_slider_value   = 0;
               
                string direction_text = "no direction";

                //Switch with as argument e.KeyCode with holds values as Keys.W and Keys.Q
                switch (e.KeyCode)
                {   
                    //If W is pressed on the keyboard
                    case Keys.W:
                        
                        //Set the direction value to vooruit
                        direction_text  = "forward";

                        //Increment the current value of the master slider with STEPSIZE (10).
                        new_slider_value = tbar_master.Value + STEPSIZE;
                        
                        //Check boundaries of the new value
                        if (new_slider_value >= MIN_SLIDER_VALUE && new_slider_value <= DEFAULT_SLIDER_VALUE)
                        {
                            //Modify all sliders to higher value
                            modifySliderValue(new_slider_value, "M");
                            sendCommand("M" + new_slider_value * -1);

                        }//Check boundaries of the new value
                        else if (new_slider_value >= DEFAULT_SLIDER_VALUE && new_slider_value <= MAX_SLIDER_VALUE)
                        {
                            //Modify all sliders to higher value
                            modifySliderValue(new_slider_value, "M");

                            sendCommand("M" + new_slider_value);
                        }
                    break;
                    //If A is pressed on the keyboard
                    case Keys.A:

                        direction_text  = "left";

                        //Increment the current value of the left slider with STEPSIZE (10).
                        new_slider_value = tbar_left_motor.Value + STEPSIZE;

                        //Check boundaries of the new value
                        if (new_slider_value <= MAX_SLIDER_VALUE)
                        {
                            //Modify left slider to higher value
                            modifySliderValue(new_slider_value, "L");

                            sendCommand("L" + new_slider_value);
                        }
                    break;
                    //If S is pressed on the keyboard
                    case Keys.S:

                        direction_text = "backward";

                        //Decrement the current value of all sliders with STEPSIZE (10).
                        new_slider_value = tbar_master.Value - STEPSIZE;

                        //Check boundaries of the new value
                        if (new_slider_value >= DEFAULT_SLIDER_VALUE && new_slider_value <= MAX_SLIDER_VALUE)
                        {
                            //Modify all sliders to lower value
                            modifySliderValue(new_slider_value, "B");
                      
                            sendCommand("B" + new_slider_value);

                        }//Check boundaries of the new value
                        else if (new_slider_value >= MIN_SLIDER_VALUE && new_slider_value <= DEFAULT_SLIDER_VALUE)
                        {
                            //Modify all sliders to lower value
                            modifySliderValue(new_slider_value, "B");
                            
                            sendCommand("B" + new_slider_value * -1);

                        }
                    break;
                    //If D is pressed on the keyboard
                    case Keys.D:

                        direction_text = "right";
                        new_slider_value = tbar_right_motor.Value + STEPSIZE;
                        
                        //Check boundaries of the new value
                        if (new_slider_value <= MAX_SLIDER_VALUE)
                        {
                            //Modify the right slider
                            modifySliderValue(new_slider_value, "R");
                            
                            //Send the Right - alias R - command including the speed as percentage e.g. R50.
                            sendCommand("R" + new_slider_value);
                        }
                    break;
                    //If Q is pressed on the keyboard
                    case Keys.Q:

                        //change the value of the direction_text to stop
                        direction_text = "stop";
                        
                        //Send the QUIT - alias Q - command through the COM port
                        sendCommand("Q00");

                        //Reset the sliders to their default value (0)
                        resetSliders();
                    break;
                }
                
                //Set the text on the GUI
                lbl_direction.Text = direction_text;

            }
        }

        /**
         * modifySliderValue
         *
         * This function modifies the slides in the GUI and the labels
         * 
         * @access private
         * @param int slider_value contains the slider value which ranges from -100 to 100
         * @param string direction contains values such as M (forward), B(ackwards), L(eft) R(ight)
         * @return void
         */
        private void modifySliderValue(int slider_value, string direction)
        {
            if (direction == "M" || direction == "B")
            {
                tbar_master.Value      = slider_value;
                tbar_right_motor.Value = slider_value;
                tbar_left_motor.Value  = slider_value;

                lbl_right_motor.Text = tbar_right_motor.Value.ToString();
                lbl_left_motor.Text  = tbar_left_motor.Value.ToString();
                lbl_master_data.Text = tbar_master.Value.ToString();
            }
            else if(direction == "L")
            {
                tbar_left_motor.Value = slider_value;
                lbl_left_motor.Text   = tbar_left_motor.Value.ToString();
            }
            else if(direction == "R")
            {
                tbar_right_motor.Value = slider_value;
                lbl_right_motor.Text   = tbar_right_motor.Value.ToString();
            }
        }

       /**
        * resetSliders
        *
        * This function resets the sliders to the initial value of 0 (zero)
        * 
        * @access private
        * @param none
        * @return void
        */
        private void resetSliders()
        {
            lbl_right_motor.Text = DEFAULT_SLIDER_VALUE.ToString();
            lbl_master_data.Text = DEFAULT_SLIDER_VALUE.ToString();
            lbl_left_motor.Text  = DEFAULT_SLIDER_VALUE.ToString();

            tbar_right_motor.Value = DEFAULT_SLIDER_VALUE;
            tbar_left_motor.Value  = DEFAULT_SLIDER_VALUE;
            tbar_master.Value      = DEFAULT_SLIDER_VALUE;
        }

       /**
        * sendCommand
        *
        * This function writes data through the COM port 
        * 
        * @access private
        * @param string data contains values as M100 or Q or B50 etc etc
        * @return void
        */
        private void sendCommand(string data)
        {
            //Check if the COM port is available
            if (isCOMavail)
            {
                //Write data to the COM port
                COMPort.Write(data);

                //Write a \n needed for terminating ISR function (USART) on the AVR code
                COMPort.Write("\n");
            }
        }

        /**
         * setGUIlayout
         *
         * This function writes data through the COM port 
         * 
         * @access private
         * @param bool state can be either true or false
         * @return void
         */
        private void setGUIlayout(bool state)
        {
            if (state)
            {
                cbx_mode.Text = "manual";

                tbar_left_motor.Visible  = true;
                tbar_right_motor.Visible = true;
                tbar_master.Visible      = true;
                lbl_left.Visible         = true;
                lbl_right.Visible        = true;
                lbl_master.Visible       = true;
                lbl_master_data.Visible  = true;
                lbl_right_motor.Visible  = true;
                lbl_left_motor.Visible   = true;

                isAutoMode               = false;
            }
            else
            {
                lbl_direction.Text = "";
                cbx_mode.Text      = "autonomous";

                tbar_left_motor.Visible  = false;
                tbar_right_motor.Visible = false;
                tbar_master.Visible      = false;
                lbl_left.Visible         = false;
                lbl_right.Visible        = false;
                lbl_master.Visible       = false;
                lbl_master_data.Visible  = false;
                lbl_right_motor.Visible  = false;
                lbl_left_motor.Visible   = false;
                
                isAutoMode               = true;
            }
        }
        private void cbx_mode_CheckedChanged(object sender, EventArgs e)
        {
            if (cbx_mode.Text == "manual")
            {
                //Disable components of the layout
                setGUIlayout(false);
                //Send the command autonomous - alias A - through the COM port
                sendCommand("A");
            }
            else
            {
                //Re-enable the components of the layout
                setGUIlayout(true);
            }
        }
        private void tbar_master_MouseUp(object sender, MouseEventArgs e)
        {
            sendCommand("M" + tbar_master.Value.ToString());
        }
        private void tbar_left_motor_MouseUp(object sender, MouseEventArgs e)
        {
            sendCommand("L" + tbar_left_motor.Value.ToString());
        }
        private void tbar_right_motor_MouseUp(object sender, MouseEventArgs e)
        {
            sendCommand("R" + tbar_right_motor.Value.ToString());
        }
        private void tbar_left_motor_Scroll(object sender, EventArgs e)
        {
            lbl_left_motor.Text = "" + tbar_left_motor.Value;
        }
        private void tbar_right_motor_Scroll(object sender, EventArgs e)
        {
            lbl_right_motor.Text = "" + tbar_right_motor.Value;
        }
        private void tbar_master_Scroll(object sender, EventArgs e)
        {
            tbar_right_motor.Value = tbar_master.Value;
            tbar_left_motor.Value = tbar_master.Value;
            lbl_right_motor.Text = tbar_right_motor.Value.ToString();
            lbl_left_motor.Text = tbar_left_motor.Value.ToString();
            lbl_master_data.Text = tbar_master.Value.ToString();
        }


        private void cbx_com_ports_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                COMPort = new SerialPort(cbx_com_ports.SelectedItem.ToString());

                //Open the COM port
                COMPort.Open();

                lbl_con_state.Text = (COMPort.IsOpen) ? "Connection made" : "Connection failed";

                //Attach an eventhandler (data recieved to the COM port)
                COMPort.DataReceived += new SerialDataReceivedEventHandler(DataReceivedHandler);

                //Set the value of isCOMavail to true because the COM port is avaiable
                isCOMavail = true;

            }
            catch (Exception ex)
            {
                //Set the value of isCOMavail to false because the COM port wasn't available
                isCOMavail = false;

                //Write the error message to the console
                Console.Write(ex.Message);
            }
        }
    }
}
