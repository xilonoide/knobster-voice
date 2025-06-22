namespace KnobsterVoiceEncoder
{
    partial class ScanForm
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ScanForm));
			this.btnOK = new System.Windows.Forms.Button();
			this.btnClear = new System.Windows.Forms.Button();
			this.lblLeftControl = new System.Windows.Forms.Label();
			this.lblLeftAlt = new System.Windows.Forms.Label();
			this.lblLeftShift = new System.Windows.Forms.Label();
			this.txtKeys = new System.Windows.Forms.TextBox();
			this.lblRightControl = new System.Windows.Forms.Label();
			this.lblRightAlt = new System.Windows.Forms.Label();
			this.lblRightShift = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// btnOK
			// 
			this.btnOK.Location = new System.Drawing.Point(299, 51);
			this.btnOK.Name = "btnOK";
			this.btnOK.Size = new System.Drawing.Size(100, 23);
			this.btnOK.TabIndex = 0;
			this.btnOK.TabStop = false;
			this.btnOK.Text = "OK";
			this.btnOK.UseVisualStyleBackColor = true;
			this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
			// 
			// btnClear
			// 
			this.btnClear.Location = new System.Drawing.Point(299, 80);
			this.btnClear.Name = "btnClear";
			this.btnClear.Size = new System.Drawing.Size(100, 23);
			this.btnClear.TabIndex = 0;
			this.btnClear.TabStop = false;
			this.btnClear.Text = "Clear";
			this.btnClear.UseVisualStyleBackColor = true;
			this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
			// 
			// lblLeftControl
			// 
			this.lblLeftControl.AutoSize = true;
			this.lblLeftControl.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblLeftControl.Location = new System.Drawing.Point(12, 23);
			this.lblLeftControl.Name = "lblLeftControl";
			this.lblLeftControl.Size = new System.Drawing.Size(107, 20);
			this.lblLeftControl.TabIndex = 1;
			this.lblLeftControl.Text = "[L-CONTROL]";
			// 
			// lblLeftAlt
			// 
			this.lblLeftAlt.AutoSize = true;
			this.lblLeftAlt.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblLeftAlt.Location = new System.Drawing.Point(128, 23);
			this.lblLeftAlt.Name = "lblLeftAlt";
			this.lblLeftAlt.Size = new System.Drawing.Size(60, 20);
			this.lblLeftAlt.TabIndex = 1;
			this.lblLeftAlt.Text = "[L-ALT]";
			// 
			// lblLeftShift
			// 
			this.lblLeftShift.AutoSize = true;
			this.lblLeftShift.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblLeftShift.Location = new System.Drawing.Point(194, 23);
			this.lblLeftShift.Name = "lblLeftShift";
			this.lblLeftShift.Size = new System.Drawing.Size(78, 20);
			this.lblLeftShift.TabIndex = 1;
			this.lblLeftShift.Text = "[L-SHIFT]";
			// 
			// txtKeys
			// 
			this.txtKeys.Location = new System.Drawing.Point(299, 25);
			this.txtKeys.Name = "txtKeys";
			this.txtKeys.Size = new System.Drawing.Size(100, 20);
			this.txtKeys.TabIndex = 1;
			this.txtKeys.TabStop = false;
			// 
			// lblRightControl
			// 
			this.lblRightControl.AutoSize = true;
			this.lblRightControl.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblRightControl.Location = new System.Drawing.Point(12, 66);
			this.lblRightControl.Name = "lblRightControl";
			this.lblRightControl.Size = new System.Drawing.Size(110, 20);
			this.lblRightControl.TabIndex = 1;
			this.lblRightControl.Text = "[R-CONTROL]";
			// 
			// lblRightAlt
			// 
			this.lblRightAlt.AutoSize = true;
			this.lblRightAlt.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblRightAlt.Location = new System.Drawing.Point(128, 66);
			this.lblRightAlt.Name = "lblRightAlt";
			this.lblRightAlt.Size = new System.Drawing.Size(63, 20);
			this.lblRightAlt.TabIndex = 1;
			this.lblRightAlt.Text = "[R-ALT]";
			// 
			// lblRightShift
			// 
			this.lblRightShift.AutoSize = true;
			this.lblRightShift.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblRightShift.Location = new System.Drawing.Point(194, 66);
			this.lblRightShift.Name = "lblRightShift";
			this.lblRightShift.Size = new System.Drawing.Size(81, 20);
			this.lblRightShift.TabIndex = 1;
			this.lblRightShift.Text = "[R-SHIFT]";
			// 
			// ScanForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(416, 117);
			this.Controls.Add(this.txtKeys);
			this.Controls.Add(this.btnClear);
			this.Controls.Add(this.lblRightShift);
			this.Controls.Add(this.lblLeftShift);
			this.Controls.Add(this.lblRightAlt);
			this.Controls.Add(this.lblLeftAlt);
			this.Controls.Add(this.lblRightControl);
			this.Controls.Add(this.lblLeftControl);
			this.Controls.Add(this.btnOK);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.KeyPreview = true;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "ScanForm";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Key scanner";
			this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ScanForm_KeyDown);
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.Label lblLeftControl;
        private System.Windows.Forms.Label lblLeftAlt;
        private System.Windows.Forms.Label lblLeftShift;
        private System.Windows.Forms.TextBox txtKeys;
        private System.Windows.Forms.Label lblRightControl;
        private System.Windows.Forms.Label lblRightAlt;
        private System.Windows.Forms.Label lblRightShift;
    }
}