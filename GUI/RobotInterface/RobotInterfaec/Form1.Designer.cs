namespace RobotInterface
{
    partial class RobotGUI
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lbl_direction = new System.Windows.Forms.Label();
            this.pb_compass = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.lbl_compass_direction = new System.Windows.Forms.Label();
            this.tbar_right_motor = new System.Windows.Forms.TrackBar();
            this.lbl_right_motor = new System.Windows.Forms.Label();
            this.lbl_left_motor = new System.Windows.Forms.Label();
            this.tbar_left_motor = new System.Windows.Forms.TrackBar();
            this.tbar_master = new System.Windows.Forms.TrackBar();
            this.lbl_master = new System.Windows.Forms.Label();
            this.lbl_left = new System.Windows.Forms.Label();
            this.lbl_right = new System.Windows.Forms.Label();
            this.cbx_mode = new System.Windows.Forms.CheckBox();
            this.lbl_master_data = new System.Windows.Forms.Label();
            this.cbx_com_ports = new System.Windows.Forms.ComboBox();
            this.lbl_con_state = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pb_compass)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbar_right_motor)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbar_left_motor)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbar_master)).BeginInit();
            this.SuspendLayout();
            // 
            // lbl_direction
            // 
            this.lbl_direction.AutoSize = true;
            this.lbl_direction.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_direction.Location = new System.Drawing.Point(390, 167);
            this.lbl_direction.Name = "lbl_direction";
            this.lbl_direction.Size = new System.Drawing.Size(0, 36);
            this.lbl_direction.TabIndex = 6;
            // 
            // pb_compass
            // 
            this.pb_compass.Location = new System.Drawing.Point(67, 76);
            this.pb_compass.Name = "pb_compass";
            this.pb_compass.Size = new System.Drawing.Size(168, 152);
            this.pb_compass.TabIndex = 7;
            this.pb_compass.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(135, 33);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(31, 29);
            this.label1.TabIndex = 8;
            this.label1.Text = "N";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(248, 135);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(32, 29);
            this.label2.TabIndex = 9;
            this.label2.Text = "O";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(138, 245);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(27, 29);
            this.label3.TabIndex = 10;
            this.label3.Text = "Z";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(21, 136);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(35, 29);
            this.label4.TabIndex = 11;
            this.label4.Text = "W";
            // 
            // lbl_compass_direction
            // 
            this.lbl_compass_direction.AutoSize = true;
            this.lbl_compass_direction.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_compass_direction.Location = new System.Drawing.Point(123, 319);
            this.lbl_compass_direction.Name = "lbl_compass_direction";
            this.lbl_compass_direction.Size = new System.Drawing.Size(0, 29);
            this.lbl_compass_direction.TabIndex = 12;
            // 
            // tbar_right_motor
            // 
            this.tbar_right_motor.LargeChange = 1;
            this.tbar_right_motor.Location = new System.Drawing.Point(836, 87);
            this.tbar_right_motor.Maximum = 100;
            this.tbar_right_motor.Minimum = -100;
            this.tbar_right_motor.Name = "tbar_right_motor";
            this.tbar_right_motor.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.tbar_right_motor.Size = new System.Drawing.Size(56, 254);
            this.tbar_right_motor.TabIndex = 13;
            this.tbar_right_motor.Scroll += new System.EventHandler(this.tbar_right_motor_Scroll);
            this.tbar_right_motor.MouseUp += new System.Windows.Forms.MouseEventHandler(this.tbar_right_motor_MouseUp);
            // 
            // lbl_right_motor
            // 
            this.lbl_right_motor.AutoSize = true;
            this.lbl_right_motor.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_right_motor.Location = new System.Drawing.Point(841, 63);
            this.lbl_right_motor.Name = "lbl_right_motor";
            this.lbl_right_motor.Size = new System.Drawing.Size(0, 20);
            this.lbl_right_motor.TabIndex = 14;
            // 
            // lbl_left_motor
            // 
            this.lbl_left_motor.AutoSize = true;
            this.lbl_left_motor.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_left_motor.Location = new System.Drawing.Point(774, 63);
            this.lbl_left_motor.Name = "lbl_left_motor";
            this.lbl_left_motor.Size = new System.Drawing.Size(0, 20);
            this.lbl_left_motor.TabIndex = 16;
            // 
            // tbar_left_motor
            // 
            this.tbar_left_motor.LargeChange = 1;
            this.tbar_left_motor.Location = new System.Drawing.Point(769, 87);
            this.tbar_left_motor.Maximum = 100;
            this.tbar_left_motor.Minimum = -100;
            this.tbar_left_motor.Name = "tbar_left_motor";
            this.tbar_left_motor.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.tbar_left_motor.Size = new System.Drawing.Size(56, 254);
            this.tbar_left_motor.TabIndex = 15;
            this.tbar_left_motor.Scroll += new System.EventHandler(this.tbar_left_motor_Scroll);
            this.tbar_left_motor.MouseUp += new System.Windows.Forms.MouseEventHandler(this.tbar_left_motor_MouseUp);
            // 
            // tbar_master
            // 
            this.tbar_master.LargeChange = 1;
            this.tbar_master.Location = new System.Drawing.Point(666, 87);
            this.tbar_master.Maximum = 100;
            this.tbar_master.Minimum = -100;
            this.tbar_master.Name = "tbar_master";
            this.tbar_master.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.tbar_master.Size = new System.Drawing.Size(56, 254);
            this.tbar_master.TabIndex = 17;
            this.tbar_master.Scroll += new System.EventHandler(this.tbar_master_Scroll);
            this.tbar_master.MouseUp += new System.Windows.Forms.MouseEventHandler(this.tbar_master_MouseUp);
            // 
            // lbl_master
            // 
            this.lbl_master.AutoSize = true;
            this.lbl_master.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_master.Location = new System.Drawing.Point(642, 344);
            this.lbl_master.Name = "lbl_master";
            this.lbl_master.Size = new System.Drawing.Size(86, 29);
            this.lbl_master.TabIndex = 18;
            this.lbl_master.Text = "master";
            // 
            // lbl_left
            // 
            this.lbl_left.AutoSize = true;
            this.lbl_left.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_left.Location = new System.Drawing.Point(775, 350);
            this.lbl_left.Name = "lbl_left";
            this.lbl_left.Size = new System.Drawing.Size(19, 20);
            this.lbl_left.TabIndex = 19;
            this.lbl_left.Text = "L";
            // 
            // lbl_right
            // 
            this.lbl_right.AutoSize = true;
            this.lbl_right.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_right.Location = new System.Drawing.Point(843, 350);
            this.lbl_right.Name = "lbl_right";
            this.lbl_right.Size = new System.Drawing.Size(21, 20);
            this.lbl_right.TabIndex = 20;
            this.lbl_right.Text = "R";
            // 
            // cbx_mode
            // 
            this.cbx_mode.AutoSize = true;
            this.cbx_mode.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbx_mode.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbx_mode.Location = new System.Drawing.Point(396, 280);
            this.cbx_mode.Name = "cbx_mode";
            this.cbx_mode.Size = new System.Drawing.Size(109, 33);
            this.cbx_mode.TabIndex = 21;
            this.cbx_mode.Text = "manual";
            this.cbx_mode.UseVisualStyleBackColor = true;
            this.cbx_mode.CheckedChanged += new System.EventHandler(this.cbx_mode_CheckedChanged);
            // 
            // lbl_master_data
            // 
            this.lbl_master_data.AutoSize = true;
            this.lbl_master_data.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_master_data.Location = new System.Drawing.Point(675, 64);
            this.lbl_master_data.Name = "lbl_master_data";
            this.lbl_master_data.Size = new System.Drawing.Size(0, 20);
            this.lbl_master_data.TabIndex = 22;
            // 
            // cbx_com_ports
            // 
            this.cbx_com_ports.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbx_com_ports.FormattingEnabled = true;
            this.cbx_com_ports.Location = new System.Drawing.Point(12, 375);
            this.cbx_com_ports.Name = "cbx_com_ports";
            this.cbx_com_ports.Size = new System.Drawing.Size(120, 24);
            this.cbx_com_ports.TabIndex = 23;
            this.cbx_com_ports.SelectedIndexChanged += new System.EventHandler(this.cbx_com_ports_SelectedIndexChanged);
            // 
            // lbl_con_state
            // 
            this.lbl_con_state.AutoSize = true;
            this.lbl_con_state.Location = new System.Drawing.Point(13, 352);
            this.lbl_con_state.Name = "lbl_con_state";
            this.lbl_con_state.Size = new System.Drawing.Size(98, 17);
            this.lbl_con_state.TabIndex = 25;
            this.lbl_con_state.Text = "Select a COM:";
            // 
            // RobotGUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(914, 411);
            this.Controls.Add(this.lbl_con_state);
            this.Controls.Add(this.cbx_com_ports);
            this.Controls.Add(this.lbl_master_data);
            this.Controls.Add(this.cbx_mode);
            this.Controls.Add(this.lbl_right);
            this.Controls.Add(this.lbl_left);
            this.Controls.Add(this.lbl_master);
            this.Controls.Add(this.tbar_master);
            this.Controls.Add(this.lbl_left_motor);
            this.Controls.Add(this.tbar_left_motor);
            this.Controls.Add(this.lbl_right_motor);
            this.Controls.Add(this.tbar_right_motor);
            this.Controls.Add(this.lbl_compass_direction);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pb_compass);
            this.Controls.Add(this.lbl_direction);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.KeyPreview = true;
            this.Name = "RobotGUI";
            this.Text = "RobotGUI";
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.RobotGUI_KeyUp);
            ((System.ComponentModel.ISupportInitialize)(this.pb_compass)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbar_right_motor)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbar_left_motor)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbar_master)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label lbl_direction;
        private System.Windows.Forms.PictureBox pb_compass;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label lbl_compass_direction;
        private System.Windows.Forms.TrackBar tbar_right_motor;
        private System.Windows.Forms.Label lbl_right_motor;
        private System.Windows.Forms.Label lbl_left_motor;
        private System.Windows.Forms.TrackBar tbar_left_motor;
        private System.Windows.Forms.TrackBar tbar_master;
        private System.Windows.Forms.Label lbl_master;
        private System.Windows.Forms.Label lbl_left;
        private System.Windows.Forms.Label lbl_right;
        private System.Windows.Forms.CheckBox cbx_mode;
        private System.Windows.Forms.Label lbl_master_data;
        private System.Windows.Forms.ComboBox cbx_com_ports;
        private System.Windows.Forms.Label lbl_con_state;
    }
}

