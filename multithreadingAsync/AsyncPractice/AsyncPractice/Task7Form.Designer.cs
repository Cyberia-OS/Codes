namespace AsyncPractice
{
    partial class Task7Form
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

        #region Код, автоматически созданный конструктором форм Windows

        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.txtBase = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtPower = new System.Windows.Forms.TextBox();
            this.btnCalc = new System.Windows.Forms.Button();
            this.lblResult = new System.Windows.Forms.Label();
            this.SuspendLayout();

            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(30, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(87, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Введите число:";

            this.txtBase.Location = new System.Drawing.Point(30, 45);
            this.txtBase.Name = "txtBase";
            this.txtBase.Size = new System.Drawing.Size(150, 20);
            this.txtBase.TabIndex = 1;

            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(210, 25);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(96, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Введите степень:";

            this.txtPower.Location = new System.Drawing.Point(210, 45);
            this.txtPower.Name = "txtPower";
            this.txtPower.Size = new System.Drawing.Size(150, 20);
            this.txtPower.TabIndex = 3;

            this.btnCalc.Location = new System.Drawing.Point(30, 85);
            this.btnCalc.Name = "btnCalc";
            this.btnCalc.Size = new System.Drawing.Size(330, 35);
            this.btnCalc.TabIndex = 4;
            this.btnCalc.Text = "Возвести в степень";
            this.btnCalc.UseVisualStyleBackColor = true;
            this.btnCalc.Click += new System.EventHandler(this.btnCalc_Click);

            this.lblResult.AutoSize = true;
            this.lblResult.Location = new System.Drawing.Point(30, 140);
            this.lblResult.Name = "lblResult";
            this.lblResult.Size = new System.Drawing.Size(89, 17);
            this.lblResult.TabIndex = 5;
            this.lblResult.Text = "Результат: ";

            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(400, 190);
            this.Controls.Add(this.lblResult);
            this.Controls.Add(this.btnCalc);
            this.Controls.Add(this.txtPower);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtBase);
            this.Controls.Add(this.label1);
            this.Name = "Task7Form";
            this.Text = "Задание 7: Степень числа";
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtBase;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtPower;
        private System.Windows.Forms.Button btnCalc;
        private System.Windows.Forms.Label lblResult;
    }
}
