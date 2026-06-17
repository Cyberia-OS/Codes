namespace Task2Anketa
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.labelCity = new System.Windows.Forms.Label();
            this.comboBoxCity = new System.Windows.Forms.ComboBox();
            this.labelAge = new System.Windows.Forms.Label();
            this.numericUpDownAge = new System.Windows.Forms.NumericUpDown();
            this.labelHobby = new System.Windows.Forms.Label();
            this.checkedListBoxHobby = new System.Windows.Forms.CheckedListBox();
            this.btnSubmit = new System.Windows.Forms.Button();
            this.textBoxResult = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownAge)).BeginInit();
            this.SuspendLayout();

            this.labelCity.AutoSize = true;
            this.labelCity.Location = new System.Drawing.Point(20, 20);
            this.labelCity.Name = "labelCity";
            this.labelCity.Size = new System.Drawing.Size(35, 13);
            this.labelCity.TabIndex = 0;
            this.labelCity.Text = "Город:";

            this.comboBoxCity.Location = new System.Drawing.Point(120, 17);
            this.comboBoxCity.Name = "comboBoxCity";
            this.comboBoxCity.Size = new System.Drawing.Size(180, 21);
            this.comboBoxCity.TabIndex = 1;

            this.labelAge.AutoSize = true;
            this.labelAge.Location = new System.Drawing.Point(20, 55);
            this.labelAge.Name = "labelAge";
            this.labelAge.Size = new System.Drawing.Size(45, 13);
            this.labelAge.TabIndex = 2;
            this.labelAge.Text = "Возраст:";

            this.numericUpDownAge.Location = new System.Drawing.Point(120, 53);
            this.numericUpDownAge.Name = "numericUpDownAge";
            this.numericUpDownAge.Size = new System.Drawing.Size(80, 20);
            this.numericUpDownAge.TabIndex = 3;

            this.labelHobby.AutoSize = true;
            this.labelHobby.Location = new System.Drawing.Point(20, 90);
            this.labelHobby.Name = "labelHobby";
            this.labelHobby.Size = new System.Drawing.Size(60, 13);
            this.labelHobby.TabIndex = 4;
            this.labelHobby.Text = "Увлечения:";

            this.checkedListBoxHobby.FormattingEnabled = true;
            this.checkedListBoxHobby.Location = new System.Drawing.Point(120, 90);
            this.checkedListBoxHobby.Name = "checkedListBoxHobby";
            this.checkedListBoxHobby.Size = new System.Drawing.Size(180, 95);
            this.checkedListBoxHobby.TabIndex = 5;

            this.btnSubmit.Location = new System.Drawing.Point(20, 200);
            this.btnSubmit.Name = "btnSubmit";
            this.btnSubmit.Size = new System.Drawing.Size(280, 30);
            this.btnSubmit.TabIndex = 6;
            this.btnSubmit.Text = "Отправить анкету";
            this.btnSubmit.UseVisualStyleBackColor = true;
            this.btnSubmit.Click += new System.EventHandler(this.btnSubmit_Click);

            this.textBoxResult.Location = new System.Drawing.Point(20, 240);
            this.textBoxResult.Multiline = true;
            this.textBoxResult.Name = "textBoxResult";
            this.textBoxResult.ReadOnly = true;
            this.textBoxResult.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxResult.Size = new System.Drawing.Size(280, 140);
            this.textBoxResult.TabIndex = 7;

            this.ClientSize = new System.Drawing.Size(320, 400);
            this.Controls.Add(this.labelCity);
            this.Controls.Add(this.comboBoxCity);
            this.Controls.Add(this.labelAge);
            this.Controls.Add(this.numericUpDownAge);
            this.Controls.Add(this.labelHobby);
            this.Controls.Add(this.checkedListBoxHobby);
            this.Controls.Add(this.btnSubmit);
            this.Controls.Add(this.textBoxResult);
            this.Name = "Form1";
            this.Text = "Анкета";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownAge)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.Label labelCity;
        private System.Windows.Forms.ComboBox comboBoxCity;
        private System.Windows.Forms.Label labelAge;
        private System.Windows.Forms.NumericUpDown numericUpDownAge;
        private System.Windows.Forms.Label labelHobby;
        private System.Windows.Forms.CheckedListBox checkedListBoxHobby;
        private System.Windows.Forms.Button btnSubmit;
        private System.Windows.Forms.TextBox textBoxResult;
    }
}
