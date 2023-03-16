﻿namespace StatisticalApp
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
            this.MainTitle = new System.Windows.Forms.Label();
            this.StartButton = new System.Windows.Forms.Button();
            this.SampleBox = new System.Windows.Forms.RichTextBox();
            this.StopButton = new System.Windows.Forms.Button();
            this.StatBox = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // MainTitle
            // 
            this.MainTitle.AutoSize = true;
            this.MainTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.MainTitle.Location = new System.Drawing.Point(323, 9);
            this.MainTitle.Name = "MainTitle";
            this.MainTitle.Size = new System.Drawing.Size(400, 46);
            this.MainTitle.TabIndex = 0;
            this.MainTitle.Text = "Statistical Application";
            // 
            // StartButton
            // 
            this.StartButton.Location = new System.Drawing.Point(84, 113);
            this.StartButton.Name = "StartButton";
            this.StartButton.Size = new System.Drawing.Size(108, 47);
            this.StartButton.TabIndex = 1;
            this.StartButton.Text = "Sample Start";
            this.StartButton.UseVisualStyleBackColor = true;
            this.StartButton.Click += new System.EventHandler(this.StartButton_Click);
            // 
            // SampleBox
            // 
            this.SampleBox.Location = new System.Drawing.Point(84, 189);
            this.SampleBox.Name = "SampleBox";
            this.SampleBox.Size = new System.Drawing.Size(420, 388);
            this.SampleBox.TabIndex = 2;
            this.SampleBox.Text = "";
            // 
            // StopButton
            // 
            this.StopButton.Location = new System.Drawing.Point(250, 113);
            this.StopButton.Name = "StopButton";
            this.StopButton.Size = new System.Drawing.Size(67, 47);
            this.StopButton.TabIndex = 5;
            this.StopButton.Text = "STOP";
            this.StopButton.UseVisualStyleBackColor = true;
            this.StopButton.Click += new System.EventHandler(this.StopButton_Click);
            // 
            // StatBox
            // 
            this.StatBox.Location = new System.Drawing.Point(569, 189);
            this.StatBox.Name = "StatBox";
            this.StatBox.Size = new System.Drawing.Size(342, 213);
            this.StatBox.TabIndex = 6;
            this.StatBox.Text = "";
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1068, 628);
            this.Controls.Add(this.StatBox);
            this.Controls.Add(this.StopButton);
            this.Controls.Add(this.SampleBox);
            this.Controls.Add(this.StartButton);
            this.Controls.Add(this.MainTitle);
            this.Name = "MainWindow";
            this.Text = "MainWindow";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label MainTitle;
        private System.Windows.Forms.Button StartButton;
        private System.Windows.Forms.RichTextBox SampleBox;
        private System.Windows.Forms.Button StopButton;
        private System.Windows.Forms.RichTextBox StatBox;
    }
}
