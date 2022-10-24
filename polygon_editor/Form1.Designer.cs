
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
            this.radioButton1 = new System.Windows.Forms.RadioButton();
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
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1573, 931);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // Canvas
            // 
            this.Canvas.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Canvas.Location = new System.Drawing.Point(3, 3);
            this.Canvas.Name = "Canvas";
            this.Canvas.Size = new System.Drawing.Size(1173, 925);
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
            this.groupBox1.Location = new System.Drawing.Point(1182, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(388, 925);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Choose tool";
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 1;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel2.Controls.Add(this.tableLayoutPanel3, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.tableLayoutPanel4, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.tableLayoutPanel5, 0, 2);
            this.tableLayoutPanel2.Controls.Add(this.tableLayoutPanel6, 0, 3);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 35);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 4;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(382, 887);
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
            this.tableLayoutPanel3.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 1;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(376, 215);
            this.tableLayoutPanel3.TabIndex = 2;
            // 
            // MODIFY
            // 
            this.MODIFY.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MODIFY.Location = new System.Drawing.Point(191, 3);
            this.MODIFY.Name = "MODIFY";
            this.MODIFY.Size = new System.Drawing.Size(182, 209);
            this.MODIFY.TabIndex = 1;
            this.MODIFY.Text = "modyfikuj wielokąt";
            this.MODIFY.UseVisualStyleBackColor = true;
            this.MODIFY.Click += new System.EventHandler(this.MODIFY_Click);
            // 
            // CREATE
            // 
            this.CREATE.Dock = System.Windows.Forms.DockStyle.Fill;
            this.CREATE.Location = new System.Drawing.Point(3, 3);
            this.CREATE.Name = "CREATE";
            this.CREATE.Size = new System.Drawing.Size(182, 209);
            this.CREATE.TabIndex = 0;
            this.CREATE.Text = "stwórz wielokąt";
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
            this.tableLayoutPanel4.Location = new System.Drawing.Point(3, 224);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RowCount = 1;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel4.Size = new System.Drawing.Size(376, 215);
            this.tableLayoutPanel4.TabIndex = 3;
            // 
            // MIDDLE_INSERT
            // 
            this.MIDDLE_INSERT.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MIDDLE_INSERT.Location = new System.Drawing.Point(3, 3);
            this.MIDDLE_INSERT.Name = "MIDDLE_INSERT";
            this.MIDDLE_INSERT.Size = new System.Drawing.Size(182, 209);
            this.MIDDLE_INSERT.TabIndex = 0;
            this.MIDDLE_INSERT.Text = "dodaj wierzchołek w środku";
            this.MIDDLE_INSERT.UseVisualStyleBackColor = true;
            this.MIDDLE_INSERT.Click += new System.EventHandler(this.MIDDLE_INSERT_Click);
            // 
            // SET_LENGTH
            // 
            this.SET_LENGTH.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SET_LENGTH.Location = new System.Drawing.Point(191, 3);
            this.SET_LENGTH.Name = "SET_LENGTH";
            this.SET_LENGTH.Size = new System.Drawing.Size(182, 209);
            this.SET_LENGTH.TabIndex = 1;
            this.SET_LENGTH.Text = "zmień rozmiar krawędzi";
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
            this.tableLayoutPanel5.Location = new System.Drawing.Point(3, 445);
            this.tableLayoutPanel5.Name = "tableLayoutPanel5";
            this.tableLayoutPanel5.RowCount = 1;
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel5.Size = new System.Drawing.Size(376, 215);
            this.tableLayoutPanel5.TabIndex = 4;
            // 
            // ADD_REL
            // 
            this.ADD_REL.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ADD_REL.Location = new System.Drawing.Point(3, 3);
            this.ADD_REL.Name = "ADD_REL";
            this.ADD_REL.Size = new System.Drawing.Size(182, 209);
            this.ADD_REL.TabIndex = 0;
            this.ADD_REL.Text = "dodaj relacje";
            this.ADD_REL.UseVisualStyleBackColor = true;
            this.ADD_REL.Click += new System.EventHandler(this.ADD_REL_Click);
            // 
            // REMOVE_REL
            // 
            this.REMOVE_REL.Dock = System.Windows.Forms.DockStyle.Fill;
            this.REMOVE_REL.Location = new System.Drawing.Point(191, 3);
            this.REMOVE_REL.Name = "REMOVE_REL";
            this.REMOVE_REL.Size = new System.Drawing.Size(182, 209);
            this.REMOVE_REL.TabIndex = 1;
            this.REMOVE_REL.Text = "usuń relacje";
            this.REMOVE_REL.UseVisualStyleBackColor = true;
            this.REMOVE_REL.Click += new System.EventHandler(this.REMOVE_REL_Click);
            // 
            // tableLayoutPanel6
            // 
            this.tableLayoutPanel6.ColumnCount = 1;
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel6.Controls.Add(this.CLEAR, 0, 0);
            this.tableLayoutPanel6.Controls.Add(this.SCENE, 0, 1);
            this.tableLayoutPanel6.Controls.Add(this.radioButton1, 0, 2);
            this.tableLayoutPanel6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel6.Location = new System.Drawing.Point(3, 666);
            this.tableLayoutPanel6.Name = "tableLayoutPanel6";
            this.tableLayoutPanel6.RowCount = 3;
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel6.Size = new System.Drawing.Size(376, 218);
            this.tableLayoutPanel6.TabIndex = 5;
            // 
            // CLEAR
            // 
            this.CLEAR.Dock = System.Windows.Forms.DockStyle.Fill;
            this.CLEAR.Location = new System.Drawing.Point(3, 3);
            this.CLEAR.Name = "CLEAR";
            this.CLEAR.Size = new System.Drawing.Size(370, 66);
            this.CLEAR.TabIndex = 0;
            this.CLEAR.Text = "wyczyść canve";
            this.CLEAR.UseVisualStyleBackColor = true;
            this.CLEAR.Click += new System.EventHandler(this.CLEAR_Click);
            // 
            // SCENE
            // 
            this.SCENE.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SCENE.Location = new System.Drawing.Point(3, 75);
            this.SCENE.Name = "SCENE";
            this.SCENE.Size = new System.Drawing.Size(370, 66);
            this.SCENE.TabIndex = 1;
            this.SCENE.Text = "predefiniowana scena";
            this.SCENE.UseVisualStyleBackColor = true;
            this.SCENE.Click += new System.EventHandler(this.SCENE_Click);
            // 
            // radioButton1
            // 
            this.radioButton1.AutoSize = true;
            this.radioButton1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.radioButton1.Location = new System.Drawing.Point(3, 147);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(370, 68);
            this.radioButton1.TabIndex = 2;
            this.radioButton1.TabStop = true;
            this.radioButton1.Text = "radioButton1";
            this.radioButton1.UseVisualStyleBackColor = true;
            // 
            // polygon_editor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(13F, 32F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1574, 929);
            this.Controls.Add(this.tableLayoutPanel1);
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
        private System.Windows.Forms.RadioButton radioButton1;
    }
}

