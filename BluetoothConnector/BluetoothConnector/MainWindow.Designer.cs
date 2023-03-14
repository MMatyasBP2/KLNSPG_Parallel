namespace BluetoothConnector
{
    partial class MainWindow
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainWindow));
            this.MainTitle = new System.Windows.Forms.Label();
            this.SearchButton = new System.Windows.Forms.Button();
            this.ConnectButton = new System.Windows.Forms.Button();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.DevicesBox = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // MainTitle
            // 
            this.MainTitle.AutoSize = true;
            this.MainTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 19.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.MainTitle.Location = new System.Drawing.Point(420, 9);
            this.MainTitle.Name = "MainTitle";
            this.MainTitle.Size = new System.Drawing.Size(314, 38);
            this.MainTitle.TabIndex = 0;
            this.MainTitle.Text = "Bluetooth Connector";
            this.MainTitle.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // SearchButton
            // 
            this.SearchButton.Location = new System.Drawing.Point(249, 126);
            this.SearchButton.Name = "SearchButton";
            this.SearchButton.Size = new System.Drawing.Size(107, 37);
            this.SearchButton.TabIndex = 1;
            this.SearchButton.Text = "Search";
            this.SearchButton.UseVisualStyleBackColor = true;
            this.SearchButton.Click += new System.EventHandler(this.SearchButton_Click);
            // 
            // ConnectButton
            // 
            this.ConnectButton.Location = new System.Drawing.Point(249, 459);
            this.ConnectButton.Name = "ConnectButton";
            this.ConnectButton.Size = new System.Drawing.Size(107, 38);
            this.ConnectButton.TabIndex = 2;
            this.ConnectButton.Text = "Connect";
            this.ConnectButton.UseVisualStyleBackColor = true;
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(381, 474);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(407, 23);
            this.progressBar1.TabIndex = 3;
            // 
            // DevicesBox
            // 
            this.DevicesBox.FormattingEnabled = true;
            this.DevicesBox.Location = new System.Drawing.Point(381, 126);
            this.DevicesBox.Name = "DevicesBox";
            this.DevicesBox.Size = new System.Drawing.Size(407, 24);
            this.DevicesBox.TabIndex = 4;
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1132, 644);
            this.Controls.Add(this.DevicesBox);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.ConnectButton);
            this.Controls.Add(this.SearchButton);
            this.Controls.Add(this.MainTitle);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainWindow";
            this.Text = "BluetoothConnector";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label MainTitle;
        private System.Windows.Forms.Button SearchButton;
        private System.Windows.Forms.Button ConnectButton;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.ComboBox DevicesBox;
    }
}

