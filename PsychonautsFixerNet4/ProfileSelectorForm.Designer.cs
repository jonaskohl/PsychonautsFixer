using System.Windows.Forms;

namespace PsychonautsFixer
{
    partial class ProfileSelectorForm
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.buttonProfile3 = new System.Windows.Forms.Button();
            this.buttonProfile2 = new System.Windows.Forms.Button();
            this.buttonProfile1 = new System.Windows.Forms.Button();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.continueButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.tableLayoutPanel1.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33334F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33334F));
            this.tableLayoutPanel1.Controls.Add(this.buttonProfile3, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.buttonProfile2, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.buttonProfile1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.flowLayoutPanel1, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(9, 9);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(315, 122);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // buttonProfile3
            // 
            this.buttonProfile3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonProfile3.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.buttonProfile3.Location = new System.Drawing.Point(212, 3);
            this.buttonProfile3.Name = "buttonProfile3";
            this.buttonProfile3.Size = new System.Drawing.Size(100, 87);
            this.buttonProfile3.TabIndex = 2;
            this.buttonProfile3.Tag = "profile 3";
            this.buttonProfile3.Text = "Profile &3";
            this.buttonProfile3.UseVisualStyleBackColor = true;
            this.buttonProfile3.Click += new System.EventHandler(this.ProfileButton_Click);
            // 
            // buttonProfile2
            // 
            this.buttonProfile2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonProfile2.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.buttonProfile2.Location = new System.Drawing.Point(107, 3);
            this.buttonProfile2.Name = "buttonProfile2";
            this.buttonProfile2.Size = new System.Drawing.Size(99, 87);
            this.buttonProfile2.TabIndex = 1;
            this.buttonProfile2.Tag = "profile 2";
            this.buttonProfile2.Text = "Profile &2";
            this.buttonProfile2.UseVisualStyleBackColor = true;
            this.buttonProfile2.Click += new System.EventHandler(this.ProfileButton_Click);
            // 
            // buttonProfile1
            // 
            this.buttonProfile1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonProfile1.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.buttonProfile1.Location = new System.Drawing.Point(3, 3);
            this.buttonProfile1.Name = "buttonProfile1";
            this.buttonProfile1.Size = new System.Drawing.Size(98, 87);
            this.buttonProfile1.TabIndex = 0;
            this.buttonProfile1.Tag = "profile 1";
            this.buttonProfile1.Text = "Profile &1";
            this.buttonProfile1.UseVisualStyleBackColor = true;
            this.buttonProfile1.Click += new System.EventHandler(this.ProfileButton_Click);
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.flowLayoutPanel1.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.flowLayoutPanel1, 3);
            this.flowLayoutPanel1.Controls.Add(this.continueButton);
            this.flowLayoutPanel1.Controls.Add(this.cancelButton);
            this.flowLayoutPanel1.Location = new System.Drawing.Point(73, 96);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(239, 23);
            this.flowLayoutPanel1.TabIndex = 3;
            this.flowLayoutPanel1.WrapContents = false;
            // 
            // continueButton
            // 
            this.continueButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.continueButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.continueButton.Location = new System.Drawing.Point(3, 0);
            this.continueButton.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.continueButton.Name = "continueButton";
            this.continueButton.Size = new System.Drawing.Size(158, 23);
            this.continueButton.TabIndex = 1;
            this.continueButton.Text = "C&ontinue without profile";
            this.continueButton.UseVisualStyleBackColor = true;
            // 
            // cancelButton
            // 
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.cancelButton.Location = new System.Drawing.Point(164, 0);
            this.cancelButton.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 0;
            this.cancelButton.Text = "&Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            // 
            // ProfileSelectorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(333, 140);
            this.ControlBox = false;
            this.Controls.Add(this.tableLayoutPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "ProfileSelectorForm";
            this.Padding = new System.Windows.Forms.Padding(9);
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Select profile";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private TableLayoutPanel tableLayoutPanel1;
        private Button buttonProfile1;
        private Button buttonProfile3;
        private Button buttonProfile2;
        private FlowLayoutPanel flowLayoutPanel1;
        private Button cancelButton;
        private Button continueButton;
    }
}