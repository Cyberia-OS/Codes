namespace AsyncPractice
{
    partial class Form1
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.txtNumbers = new System.Windows.Forms.TextBox();
            this.txtLetters = new System.Windows.Forms.TextBox();
            this.txtSymbols = new System.Windows.Forms.TextBox();
            this.cmbNumPrio = new System.Windows.Forms.ComboBox();
            this.cmbLetPrio = new System.Windows.Forms.ComboBox();
            this.cmbSymPrio = new System.Windows.Forms.ComboBox();
            this.btnStart = new System.Windows.Forms.Button();
            this.btnStop = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // txtNumbers
            // 
            this.txtNumbers.Location = new System.Drawing.Point(161, 99);
            this.txtNumbers.Multiline = true;
            this.txtNumbers.Name = "txtNumbers";
            this.txtNumbers.Size = new System.Drawing.Size(100, 47);
            this.txtNumbers.TabIndex = 0;
            this.txtNumbers.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // txtLetters
            // 
            this.txtLetters.Location = new System.Drawing.Point(329, 99);
            this.txtLetters.Multiline = true;
            this.txtLetters.Name = "txtLetters";
            this.txtLetters.Size = new System.Drawing.Size(100, 47);
            this.txtLetters.TabIndex = 1;
            this.txtLetters.TextChanged += new System.EventHandler(this.textBox2_TextChanged);
            // 
            // txtSymbols
            // 
            this.txtSymbols.Location = new System.Drawing.Point(502, 99);
            this.txtSymbols.Multiline = true;
            this.txtSymbols.Name = "txtSymbols";
            this.txtSymbols.Size = new System.Drawing.Size(100, 47);
            this.txtSymbols.TabIndex = 2;
            // 
            // cmbNumPrio
            // 
            this.cmbNumPrio.FormattingEnabled = true;
            this.cmbNumPrio.Location = new System.Drawing.Point(150, 72);
            this.cmbNumPrio.Name = "cmbNumPrio";
            this.cmbNumPrio.Size = new System.Drawing.Size(121, 21);
            this.cmbNumPrio.TabIndex = 3;
            this.cmbNumPrio.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // cmbLetPrio
            // 
            this.cmbLetPrio.FormattingEnabled = true;
            this.cmbLetPrio.Location = new System.Drawing.Point(320, 72);
            this.cmbLetPrio.Name = "cmbLetPrio";
            this.cmbLetPrio.Size = new System.Drawing.Size(121, 21);
            this.cmbLetPrio.TabIndex = 4;
            // 
            // cmbSymPrio
            // 
            this.cmbSymPrio.FormattingEnabled = true;
            this.cmbSymPrio.Location = new System.Drawing.Point(491, 72);
            this.cmbSymPrio.Name = "cmbSymPrio";
            this.cmbSymPrio.Size = new System.Drawing.Size(121, 21);
            this.cmbSymPrio.TabIndex = 5;
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(270, 188);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(100, 60);
            this.btnStart.TabIndex = 6;
            this.btnStart.Text = "Старт";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // btnStop
            // 
            this.btnStop.Location = new System.Drawing.Point(401, 188);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(100, 60);
            this.btnStop.TabIndex = 7;
            this.btnStop.Text = "Стоп";
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.Click += new System.EventHandler(this.button2_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.btnStop);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.cmbSymPrio);
            this.Controls.Add(this.cmbLetPrio);
            this.Controls.Add(this.cmbNumPrio);
            this.Controls.Add(this.txtSymbols);
            this.Controls.Add(this.txtLetters);
            this.Controls.Add(this.txtNumbers);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtNumbers;
        private System.Windows.Forms.TextBox txtLetters;
        private System.Windows.Forms.TextBox txtSymbols;
        private System.Windows.Forms.ComboBox cmbNumPrio;
        private System.Windows.Forms.ComboBox cmbLetPrio;
        private System.Windows.Forms.ComboBox cmbSymPrio;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Button btnStop;
    }
}

