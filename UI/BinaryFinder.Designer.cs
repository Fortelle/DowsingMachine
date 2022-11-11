namespace PBT.DowsingMachine.UI
{
    partial class BinaryFinder
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
            this.optStep = new System.Windows.Forms.RadioButton();
            this.optKMP = new System.Windows.Forms.RadioButton();
            this.optBMH = new System.Windows.Forms.RadioButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.nudMaxStep = new System.Windows.Forms.NumericUpDown();
            this.nudMinStep = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.radioButton3 = new System.Windows.Forms.RadioButton();
            this.radioButton2 = new System.Windows.Forms.RadioButton();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.button1 = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudMaxStep)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudMinStep)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // optStep
            // 
            this.optStep.AutoSize = true;
            this.optStep.Checked = true;
            this.optStep.Location = new System.Drawing.Point(22, 36);
            this.optStep.Name = "optStep";
            this.optStep.Size = new System.Drawing.Size(63, 24);
            this.optStep.TabIndex = 0;
            this.optStep.TabStop = true;
            this.optStep.Text = "Steps";
            this.optStep.UseVisualStyleBackColor = true;
            // 
            // optKMP
            // 
            this.optKMP.AutoSize = true;
            this.optKMP.Location = new System.Drawing.Point(22, 146);
            this.optKMP.Name = "optKMP";
            this.optKMP.Size = new System.Drawing.Size(212, 24);
            this.optKMP.TabIndex = 1;
            this.optKMP.Text = "Knuth-Morris-Pratt method";
            this.optKMP.UseVisualStyleBackColor = true;
            // 
            // optBMH
            // 
            this.optBMH.AutoSize = true;
            this.optBMH.Location = new System.Drawing.Point(22, 186);
            this.optBMH.Name = "optBMH";
            this.optBMH.Size = new System.Drawing.Size(241, 24);
            this.optBMH.TabIndex = 2;
            this.optBMH.Text = "Boyer-Moore-Horspool method";
            this.optBMH.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.nudMaxStep);
            this.groupBox1.Controls.Add(this.nudMinStep);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.optStep);
            this.groupBox1.Controls.Add(this.optKMP);
            this.groupBox1.Controls.Add(this.optBMH);
            this.groupBox1.Location = new System.Drawing.Point(337, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(278, 291);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Method";
            // 
            // nudMaxStep
            // 
            this.nudMaxStep.Location = new System.Drawing.Point(88, 100);
            this.nudMaxStep.Name = "nudMaxStep";
            this.nudMaxStep.Size = new System.Drawing.Size(120, 25);
            this.nudMaxStep.TabIndex = 6;
            this.nudMaxStep.Value = new decimal(new int[] {
            8,
            0,
            0,
            0});
            // 
            // nudMinStep
            // 
            this.nudMinStep.Location = new System.Drawing.Point(88, 69);
            this.nudMinStep.Name = "nudMinStep";
            this.nudMinStep.Size = new System.Drawing.Size(120, 25);
            this.nudMinStep.TabIndex = 5;
            this.nudMinStep.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(35, 102);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(38, 20);
            this.label2.TabIndex = 4;
            this.label2.Text = "Max";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(35, 71);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(36, 20);
            this.label1.TabIndex = 3;
            this.label1.Text = "Min";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(16, 24);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(174, 250);
            this.textBox1.TabIndex = 4;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.radioButton3);
            this.groupBox2.Controls.Add(this.radioButton2);
            this.groupBox2.Controls.Add(this.radioButton1);
            this.groupBox2.Controls.Add(this.textBox1);
            this.groupBox2.Location = new System.Drawing.Point(12, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(319, 291);
            this.groupBox2.TabIndex = 5;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "groupBox2";
            // 
            // radioButton3
            // 
            this.radioButton3.AutoSize = true;
            this.radioButton3.Location = new System.Drawing.Point(196, 96);
            this.radioButton3.Name = "radioButton3";
            this.radioButton3.Size = new System.Drawing.Size(45, 24);
            this.radioButton3.TabIndex = 7;
            this.radioButton3.TabStop = true;
            this.radioButton3.Text = "int";
            this.radioButton3.UseVisualStyleBackColor = true;
            // 
            // radioButton2
            // 
            this.radioButton2.AutoSize = true;
            this.radioButton2.Location = new System.Drawing.Point(196, 66);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new System.Drawing.Size(61, 24);
            this.radioButton2.TabIndex = 6;
            this.radioButton2.TabStop = true;
            this.radioButton2.Text = "short";
            this.radioButton2.UseVisualStyleBackColor = true;
            // 
            // radioButton1
            // 
            this.radioButton1.AutoSize = true;
            this.radioButton1.Location = new System.Drawing.Point(197, 36);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(56, 24);
            this.radioButton1.TabIndex = 5;
            this.radioButton1.TabStop = true;
            this.radioButton1.Text = "byte";
            this.radioButton1.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(540, 409);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 6;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // BinaryFinder
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(627, 444);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "BinaryFinder";
            this.Text = "BinaryFinder";
            this.Load += new System.EventHandler(this.BinaryFinder_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudMaxStep)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudMinStep)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private RadioButton optStep;
        private RadioButton optKMP;
        private RadioButton optBMH;
        private GroupBox groupBox1;
        private NumericUpDown nudMaxStep;
        private NumericUpDown nudMinStep;
        private Label label2;
        private Label label1;
        private TextBox textBox1;
        private GroupBox groupBox2;
        private RadioButton radioButton3;
        private RadioButton radioButton2;
        private RadioButton radioButton1;
        private Button button1;
    }
}