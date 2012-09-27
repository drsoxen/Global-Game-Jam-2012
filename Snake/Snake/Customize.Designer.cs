namespace Snake
{
    partial class Customize
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
            this.iOkay = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.iName = new System.Windows.Forms.TextBox();
            this.labels = new System.Windows.Forms.Label();
            this.iColor = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // iOkay
            // 
            this.iOkay.Location = new System.Drawing.Point(151, 112);
            this.iOkay.Name = "iOkay";
            this.iOkay.Size = new System.Drawing.Size(75, 26);
            this.iOkay.TabIndex = 0;
            this.iOkay.Text = "Okay";
            this.iOkay.UseVisualStyleBackColor = true;
            this.iOkay.Click += new System.EventHandler(this.iOkay_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(33, 41);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(61, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Your name:";
            // 
            // iName
            // 
            this.iName.Location = new System.Drawing.Point(100, 41);
            this.iName.Name = "iName";
            this.iName.Size = new System.Drawing.Size(100, 20);
            this.iName.TabIndex = 2;
            this.iName.TextChanged += new System.EventHandler(this.iName_TextChanged);
            // 
            // labels
            // 
            this.labels.AutoSize = true;
            this.labels.Location = new System.Drawing.Point(36, 73);
            this.labels.Name = "labels";
            this.labels.Size = new System.Drawing.Size(53, 13);
            this.labels.TabIndex = 3;
            this.labels.Text = "Head tint:";
            // 
            // iColor
            // 
            this.iColor.FormattingEnabled = true;
            this.iColor.Items.AddRange(new object[] {
            "Normal",
            "Red",
            "Blue",
            "Green",
            "Yellow",
            "Black",
            "Purple",
            "Orange",
            "LimeGreen",
            "Pink",
            "Navy",
            "Gold",
            "AliceBlue"});
            this.iColor.Location = new System.Drawing.Point(105, 73);
            this.iColor.Name = "iColor";
            this.iColor.Size = new System.Drawing.Size(121, 21);
            this.iColor.TabIndex = 4;
            this.iColor.SelectedIndexChanged += new System.EventHandler(this.iColor_SelectedIndexChanged);
            // 
            // Customize
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(277, 156);
            this.Controls.Add(this.iColor);
            this.Controls.Add(this.labels);
            this.Controls.Add(this.iName);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.iOkay);
            this.Name = "Customize";
            this.Text = "Customize";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button iOkay;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox iName;
        private System.Windows.Forms.Label labels;
        private System.Windows.Forms.ComboBox iColor;
    }
}