
namespace polygon_editor
{
    partial class Form3
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
            this.APPLY = new System.Windows.Forms.Button();
            this.CANCEL = new System.Windows.Forms.Button();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            this.SuspendLayout();
            // 
            // APPLY
            // 
            this.APPLY.Location = new System.Drawing.Point(12, 71);
            this.APPLY.Name = "APPLY";
            this.APPLY.Size = new System.Drawing.Size(150, 46);
            this.APPLY.TabIndex = 1;
            this.APPLY.Text = "Apply";
            this.APPLY.UseVisualStyleBackColor = true;
            this.APPLY.Click += new System.EventHandler(this.APPLY_Click);
            // 
            // CANCEL
            // 
            this.CANCEL.Location = new System.Drawing.Point(162, 71);
            this.CANCEL.Name = "CANCEL";
            this.CANCEL.Size = new System.Drawing.Size(150, 46);
            this.CANCEL.TabIndex = 2;
            this.CANCEL.Text = "Cancel";
            this.CANCEL.UseVisualStyleBackColor = true;
            this.CANCEL.Click += new System.EventHandler(this.CANCEL_Click);
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.Location = new System.Drawing.Point(45, 12);
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new System.Drawing.Size(240, 39);
            this.numericUpDown1.TabIndex = 3;
            this.numericUpDown1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // Form3
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(13F, 32F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.ClientSize = new System.Drawing.Size(324, 129);
            this.Controls.Add(this.numericUpDown1);
            this.Controls.Add(this.CANCEL);
            this.Controls.Add(this.APPLY);
            this.Name = "Form3";
            this.Text = "Form3";
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button APPLY;
        private System.Windows.Forms.Button CANCEL;
        private System.Windows.Forms.NumericUpDown numericUpDown1;
    }
}