namespace SmallNet.DebugSession
{
    partial class DebugForm<T, H> where T:ClientModel where H:HostModel<T>
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
            this.components = new System.ComponentModel.Container();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.commandOptionsBox = new System.Windows.Forms.ComboBox();
            this.executeButton = new System.Windows.Forms.Button();
            this.consoleBox = new System.Windows.Forms.TextBox();
            this.versionLabel = new System.Windows.Forms.Label();
            this.logBox = new System.Windows.Forms.TextBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.commandTab = new System.Windows.Forms.TabPage();
            this.logTab = new System.Windows.Forms.TabPage();
            this.label7 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.paramBox = new System.Windows.Forms.TextBox();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.label6 = new System.Windows.Forms.Label();
            this.ipText = new System.Windows.Forms.Label();
            this.serverOn = new System.Windows.Forms.CheckBox();
            this.clientOn = new System.Windows.Forms.CheckBox();
            this.tabControl1.SuspendLayout();
            this.commandTab.SuspendLayout();
            this.logTab.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(17, 85);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(128, 17);
            this.label1.TabIndex = 0;
            this.label1.Text = "Select Command";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(12, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(142, 36);
            this.label2.TabIndex = 1;
            this.label2.Text = "SmallNet";
            // 
            // commandOptionsBox
            // 
            this.commandOptionsBox.FormattingEnabled = true;
            this.commandOptionsBox.Location = new System.Drawing.Point(20, 105);
            this.commandOptionsBox.Name = "commandOptionsBox";
            this.commandOptionsBox.Size = new System.Drawing.Size(239, 24);
            this.commandOptionsBox.TabIndex = 2;
            this.commandOptionsBox.SelectedValueChanged += new System.EventHandler(this.commandOptionsBox_SelectedValueChanged);
            // 
            // executeButton
            // 
            this.executeButton.Location = new System.Drawing.Point(524, 100);
            this.executeButton.Name = "executeButton";
            this.executeButton.Size = new System.Drawing.Size(107, 33);
            this.executeButton.TabIndex = 3;
            this.executeButton.Text = "execute";
            this.executeButton.UseVisualStyleBackColor = true;
            this.executeButton.Click += new System.EventHandler(this.executeButton_Click);
            // 
            // consoleBox
            // 
            this.consoleBox.BackColor = System.Drawing.Color.White;
            this.consoleBox.Font = new System.Drawing.Font("Consolas", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.consoleBox.Location = new System.Drawing.Point(6, 6);
            this.consoleBox.Multiline = true;
            this.consoleBox.Name = "consoleBox";
            this.consoleBox.ReadOnly = true;
            this.consoleBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.consoleBox.Size = new System.Drawing.Size(597, 323);
            this.consoleBox.TabIndex = 4;
            // 
            // versionLabel
            // 
            this.versionLabel.AutoSize = true;
            this.versionLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.versionLabel.Location = new System.Drawing.Point(597, 9);
            this.versionLabel.Name = "versionLabel";
            this.versionLabel.Size = new System.Drawing.Size(34, 13);
            this.versionLabel.TabIndex = 6;
            this.versionLabel.Text = "v0.01";
            // 
            // logBox
            // 
            this.logBox.BackColor = System.Drawing.Color.White;
            this.logBox.Font = new System.Drawing.Font("Consolas", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.logBox.Location = new System.Drawing.Point(6, 6);
            this.logBox.Multiline = true;
            this.logBox.Name = "logBox";
            this.logBox.ReadOnly = true;
            this.logBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.logBox.Size = new System.Drawing.Size(597, 332);
            this.logBox.TabIndex = 8;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.commandTab);
            this.tabControl1.Controls.Add(this.logTab);
            this.tabControl1.Location = new System.Drawing.Point(18, 133);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(617, 373);
            this.tabControl1.TabIndex = 9;
            // 
            // commandTab
            // 
            this.commandTab.Controls.Add(this.consoleBox);
            this.commandTab.Location = new System.Drawing.Point(4, 25);
            this.commandTab.Name = "commandTab";
            this.commandTab.Padding = new System.Windows.Forms.Padding(3);
            this.commandTab.Size = new System.Drawing.Size(609, 344);
            this.commandTab.TabIndex = 0;
            this.commandTab.Text = "Output";
            this.commandTab.UseVisualStyleBackColor = true;
            // 
            // logTab
            // 
            this.logTab.Controls.Add(this.logBox);
            this.logTab.Location = new System.Drawing.Point(4, 25);
            this.logTab.Name = "logTab";
            this.logTab.Padding = new System.Windows.Forms.Padding(3);
            this.logTab.Size = new System.Drawing.Size(609, 344);
            this.logTab.TabIndex = 1;
            this.logTab.Text = "Log";
            this.logTab.UseVisualStyleBackColor = true;
            this.logTab.Click += new System.EventHandler(this.logTab_Click);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 4.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(409, 130);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(109, 7);
            this.label7.TabIndex = 8;
            this.label7.Text = "separate arguments with spaces";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(262, 85);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(81, 17);
            this.label5.TabIndex = 7;
            this.label5.Text = "Parameters";
            // 
            // paramBox
            // 
            this.paramBox.Location = new System.Drawing.Point(265, 105);
            this.paramBox.Name = "paramBox";
            this.paramBox.Size = new System.Drawing.Size(253, 22);
            this.paramBox.TabIndex = 6;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(19, 54);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(66, 17);
            this.label6.TabIndex = 10;
            this.label6.Text = "Local IP: ";
            // 
            // ipText
            // 
            this.ipText.AutoSize = true;
            this.ipText.Location = new System.Drawing.Point(78, 54);
            this.ipText.Name = "ipText";
            this.ipText.Size = new System.Drawing.Size(68, 17);
            this.ipText.TabIndex = 11;
            this.ipText.Text = "127.0.0.1";
            // 
            // serverOn
            // 
            this.serverOn.AutoSize = true;
            this.serverOn.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.serverOn.Enabled = false;
            this.serverOn.Location = new System.Drawing.Point(561, 50);
            this.serverOn.Name = "serverOn";
            this.serverOn.Size = new System.Drawing.Size(70, 21);
            this.serverOn.TabIndex = 12;
            this.serverOn.Text = "server";
            this.serverOn.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.serverOn.UseVisualStyleBackColor = true;
            // 
            // clientOn
            // 
            this.clientOn.AutoSize = true;
            this.clientOn.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.clientOn.Enabled = false;
            this.clientOn.Location = new System.Drawing.Point(568, 73);
            this.clientOn.Name = "clientOn";
            this.clientOn.Size = new System.Drawing.Size(63, 21);
            this.clientOn.TabIndex = 13;
            this.clientOn.Text = "client";
            this.clientOn.UseVisualStyleBackColor = true;
            // 
            // DebugForm
            // 
            this.AcceptButton = this.executeButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(641, 527);
            this.Controls.Add(this.clientOn);
            this.Controls.Add(this.serverOn);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.ipText);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.paramBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.commandOptionsBox);
            this.Controls.Add(this.versionLabel);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.executeButton);
            this.Name = "DebugForm";
            this.Text = "SmallNet Debug";
            this.Load += new System.EventHandler(this.DebugForm_Load);
            this.tabControl1.ResumeLayout(false);
            this.commandTab.ResumeLayout(false);
            this.commandTab.PerformLayout();
            this.logTab.ResumeLayout(false);
            this.logTab.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox commandOptionsBox;
        private System.Windows.Forms.Button executeButton;
        private System.Windows.Forms.TextBox consoleBox;
        private System.Windows.Forms.Label versionLabel;
        private System.Windows.Forms.TextBox logBox;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage commandTab;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox paramBox;
        private System.Windows.Forms.TabPage logTab;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label ipText;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.CheckBox serverOn;
        private System.Windows.Forms.CheckBox clientOn;
    }
}