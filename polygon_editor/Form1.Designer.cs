
namespace polygon_editor
{
    partial class polygon_editor
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.Canvas = new System.Windows.Forms.PictureBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.MODIFY = new System.Windows.Forms.Button();
            this.CREATE = new System.Windows.Forms.Button();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.MIDDLE_INSERT = new System.Windows.Forms.Button();
            this.SET_LENGTH = new System.Windows.Forms.Button();
            this.tableLayoutPanel5 = new System.Windows.Forms.TableLayoutPanel();
            this.ADD_REL = new System.Windows.Forms.Button();
            this.REMOVE_REL = new System.Windows.Forms.Button();
            this.tableLayoutPanel6 = new System.Windows.Forms.TableLayoutPanel();
            this.CLEAR = new System.Windows.Forms.Button();
            this.SCENE = new System.Windows.Forms.Button();
            this.BRESENHAM = new System.Windows.Forms.RadioButton();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Canvas)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            this.tableLayoutPanel5.SuspendLayout();
            this.tableLayoutPanel6.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 75F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.Controls.Add(this.Canvas, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.groupBox1, 1, 0);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1578, 946);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // Canvas
            // 
            this.Canvas.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Canvas.Location = new System.Drawing.Point(2, 2);
            this.Canvas.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Canvas.Name = "Canvas";
            this.Canvas.Size = new System.Drawing.Size(1179, 942);
            this.Canvas.TabIndex = 0;
            this.Canvas.TabStop = false;
            this.Canvas.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Canvas_MouseDown);
            this.Canvas.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Canvas_MouseMove);
            this.Canvas.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Canvas_MouseUp);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.tableLayoutPanel2);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(1185, 2);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.groupBox1.Size = new System.Drawing.Size(391, 942);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Choose one:";
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 1;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 15F));
            this.tableLayoutPanel2.Controls.Add(this.tableLayoutPanel3, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.tableLayoutPanel4, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.tableLayoutPanel5, 0, 2);
            this.tableLayoutPanel2.Controls.Add(this.tableLayoutPanel6, 0, 3);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(2, 26);
            this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 4;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(387, 914);
            this.tableLayoutPanel2.TabIndex = 2;
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 2;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.Controls.Add(this.MODIFY, 1, 0);
            this.tableLayoutPanel3.Controls.Add(this.CREATE, 0, 0);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(2, 2);
            this.tableLayoutPanel3.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 1;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(383, 224);
            this.tableLayoutPanel3.TabIndex = 2;
            // 
            // MODIFY
            // 
            this.MODIFY.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MODIFY.Location = new System.Drawing.Point(193, 2);
            this.MODIFY.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.MODIFY.Name = "MODIFY";
            this.MODIFY.Size = new System.Drawing.Size(188, 220);
            this.MODIFY.TabIndex = 1;
            this.MODIFY.Text = "Modify existing polygon";
            this.MODIFY.UseVisualStyleBackColor = true;
            this.MODIFY.Click += new System.EventHandler(this.MODIFY_Click);
            // 
            // CREATE
            // 
            this.CREATE.Dock = System.Windows.Forms.DockStyle.Fill;
            this.CREATE.Location = new System.Drawing.Point(2, 2);
            this.CREATE.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.CREATE.Name = "CREATE";
            this.CREATE.Size = new System.Drawing.Size(187, 220);
            this.CREATE.TabIndex = 0;
            this.CREATE.Text = "Create new polygon";
            this.CREATE.UseVisualStyleBackColor = true;
            this.CREATE.Click += new System.EventHandler(this.CREATE_Click);
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.ColumnCount = 2;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel4.Controls.Add(this.MIDDLE_INSERT, 0, 0);
            this.tableLayoutPanel4.Controls.Add(this.SET_LENGTH, 1, 0);
            this.tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel4.Location = new System.Drawing.Point(2, 230);
            this.tableLayoutPanel4.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RowCount = 1;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel4.Size = new System.Drawing.Size(383, 224);
            this.tableLayoutPanel4.TabIndex = 3;
            // 
            // MIDDLE_INSERT
            // 
            this.MIDDLE_INSERT.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MIDDLE_INSERT.Location = new System.Drawing.Point(2, 2);
            this.MIDDLE_INSERT.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.MIDDLE_INSERT.Name = "MIDDLE_INSERT";
            this.MIDDLE_INSERT.Size = new System.Drawing.Size(187, 220);
            this.MIDDLE_INSERT.TabIndex = 0;
            this.MIDDLE_INSERT.Text = "Insert node in the middle of the edge";
            this.MIDDLE_INSERT.UseVisualStyleBackColor = true;
            this.MIDDLE_INSERT.Click += new System.EventHandler(this.MIDDLE_INSERT_Click);
            // 
            // SET_LENGTH
            // 
            this.SET_LENGTH.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SET_LENGTH.Location = new System.Drawing.Point(193, 2);
            this.SET_LENGTH.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.SET_LENGTH.Name = "SET_LENGTH";
            this.SET_LENGTH.Size = new System.Drawing.Size(188, 220);
            this.SET_LENGTH.TabIndex = 1;
            this.SET_LENGTH.Text = "Set length of an edge";
            this.SET_LENGTH.UseVisualStyleBackColor = true;
            this.SET_LENGTH.Click += new System.EventHandler(this.SET_LENGTH_Click);
            // 
            // tableLayoutPanel5
            // 
            this.tableLayoutPanel5.ColumnCount = 2;
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel5.Controls.Add(this.ADD_REL, 0, 0);
            this.tableLayoutPanel5.Controls.Add(this.REMOVE_REL, 1, 0);
            this.tableLayoutPanel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel5.Location = new System.Drawing.Point(2, 458);
            this.tableLayoutPanel5.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.tableLayoutPanel5.Name = "tableLayoutPanel5";
            this.tableLayoutPanel5.RowCount = 1;
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel5.Size = new System.Drawing.Size(383, 224);
            this.tableLayoutPanel5.TabIndex = 4;
            // 
            // ADD_REL
            // 
            this.ADD_REL.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ADD_REL.Location = new System.Drawing.Point(2, 2);
            this.ADD_REL.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.ADD_REL.Name = "ADD_REL";
            this.ADD_REL.Size = new System.Drawing.Size(187, 220);
            this.ADD_REL.TabIndex = 0;
            this.ADD_REL.Text = "Add new perpendicular relation";
            this.ADD_REL.UseVisualStyleBackColor = true;
            this.ADD_REL.Click += new System.EventHandler(this.ADD_REL_Click);
            // 
            // REMOVE_REL
            // 
            this.REMOVE_REL.Dock = System.Windows.Forms.DockStyle.Fill;
            this.REMOVE_REL.Location = new System.Drawing.Point(193, 2);
            this.REMOVE_REL.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.REMOVE_REL.Name = "REMOVE_REL";
            this.REMOVE_REL.Size = new System.Drawing.Size(188, 220);
            this.REMOVE_REL.TabIndex = 1;
            this.REMOVE_REL.Text = "Remove perpendicular relation";
            this.REMOVE_REL.UseVisualStyleBackColor = true;
            this.REMOVE_REL.Click += new System.EventHandler(this.REMOVE_REL_Click);
            // 
            // tableLayoutPanel6
            // 
            this.tableLayoutPanel6.ColumnCount = 1;
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel6.Controls.Add(this.CLEAR, 0, 0);
            this.tableLayoutPanel6.Controls.Add(this.SCENE, 0, 1);
            this.tableLayoutPanel6.Controls.Add(this.BRESENHAM, 0, 2);
            this.tableLayoutPanel6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel6.Location = new System.Drawing.Point(2, 686);
            this.tableLayoutPanel6.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.tableLayoutPanel6.Name = "tableLayoutPanel6";
            this.tableLayoutPanel6.RowCount = 3;
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel6.Size = new System.Drawing.Size(383, 226);
            this.tableLayoutPanel6.TabIndex = 5;
            // 
            // CLEAR
            // 
            this.CLEAR.Dock = System.Windows.Forms.DockStyle.Fill;
            this.CLEAR.Location = new System.Drawing.Point(2, 2);
            this.CLEAR.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.CLEAR.Name = "CLEAR";
            this.CLEAR.Size = new System.Drawing.Size(379, 71);
            this.CLEAR.TabIndex = 0;
            this.CLEAR.Text = "Clear canva";
            this.CLEAR.UseVisualStyleBackColor = true;
            this.CLEAR.Click += new System.EventHandler(this.CLEAR_Click);
            // 
            // SCENE
            // 
            this.SCENE.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SCENE.Location = new System.Drawing.Point(2, 77);
            this.SCENE.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.SCENE.Name = "SCENE";
            this.SCENE.Size = new System.Drawing.Size(379, 71);
            this.SCENE.TabIndex = 1;
            this.SCENE.Text = "Show predefined scene";
            this.SCENE.UseVisualStyleBackColor = true;
            this.SCENE.Click += new System.EventHandler(this.SCENE_Click);
            // 
            // BRESENHAM
            // 
            this.BRESENHAM.AutoSize = true;
            this.BRESENHAM.Dock = System.Windows.Forms.DockStyle.Fill;
            this.BRESENHAM.Location = new System.Drawing.Point(2, 152);
            this.BRESENHAM.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.BRESENHAM.Name = "BRESENHAM";
            this.BRESENHAM.Size = new System.Drawing.Size(379, 72);
            this.BRESENHAM.TabIndex = 2;
            this.BRESENHAM.TabStop = true;
            this.BRESENHAM.Text = "Bresenham\'s algorithm";
            this.BRESENHAM.UseVisualStyleBackColor = true;
            this.BRESENHAM.CheckedChanged += new System.EventHandler(this.BRESENHAM_CheckedChanged);
            this.BRESENHAM.Click += new System.EventHandler(this.BRESENHAM_Click);
            // 
            // polygon_editor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1578, 944);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Name = "polygon_editor";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Form1";
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.Canvas)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel4.ResumeLayout(false);
            this.tableLayoutPanel5.ResumeLayout(false);
            this.tableLayoutPanel6.ResumeLayout(false);
            this.tableLayoutPanel6.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.PictureBox Canvas;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button MODIFY;
        private System.Windows.Forms.Button CREATE;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel5;
        private System.Windows.Forms.Button MIDDLE_INSERT;
        private System.Windows.Forms.Button SET_LENGTH;
        private System.Windows.Forms.Button ADD_REL;
        private System.Windows.Forms.Button REMOVE_REL;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel6;
        private System.Windows.Forms.Button CLEAR;
        private System.Windows.Forms.Button SCENE;
        private System.Windows.Forms.RadioButton BRESENHAM;
    }
}

