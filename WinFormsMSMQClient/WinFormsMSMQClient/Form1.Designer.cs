namespace WinFormsMSMQClient
{
    partial class Form1
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
            btnSendMessage = new Button();
            txtBxMessage = new TextBox();
            listBoxMessages = new ListBox();
            txtBxLabel = new TextBox();
            lblMsmqLabel = new Label();
            lblMsmqMessage = new Label();
            SuspendLayout();
            // 
            // btnSendMessage
            // 
            btnSendMessage.Location = new Point(559, 160);
            btnSendMessage.Name = "btnSendMessage";
            btnSendMessage.Size = new Size(97, 80);
            btnSendMessage.TabIndex = 0;
            btnSendMessage.Text = "Send Message";
            btnSendMessage.UseVisualStyleBackColor = true;
            btnSendMessage.Click += btnSendMessage_Click;
            // 
            // txtBxMessage
            // 
            txtBxMessage.Location = new Point(106, 217);
            txtBxMessage.Name = "txtBxMessage";
            txtBxMessage.Size = new Size(436, 23);
            txtBxMessage.TabIndex = 1;
            // 
            // listBoxMessages
            // 
            listBoxMessages.FormattingEnabled = true;
            listBoxMessages.ItemHeight = 15;
            listBoxMessages.Location = new Point(106, 251);
            listBoxMessages.Name = "listBoxMessages";
            listBoxMessages.Size = new Size(436, 94);
            listBoxMessages.TabIndex = 2;
            // 
            // txtBxLabel
            // 
            txtBxLabel.Location = new Point(106, 160);
            txtBxLabel.Name = "txtBxLabel";
            txtBxLabel.Size = new Size(436, 23);
            txtBxLabel.TabIndex = 3;
            // 
            // lblMsmqLabel
            // 
            lblMsmqLabel.AutoSize = true;
            lblMsmqLabel.Location = new Point(106, 142);
            lblMsmqLabel.Name = "lblMsmqLabel";
            lblMsmqLabel.Size = new Size(75, 15);
            lblMsmqLabel.TabIndex = 4;
            lblMsmqLabel.Text = "MSMQ Label";
            // 
            // lblMsmqMessage
            // 
            lblMsmqMessage.AutoSize = true;
            lblMsmqMessage.Location = new Point(106, 199);
            lblMsmqMessage.Name = "lblMsmqMessage";
            lblMsmqMessage.Size = new Size(93, 15);
            lblMsmqMessage.TabIndex = 5;
            lblMsmqMessage.Text = "MSMQ Message";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(lblMsmqMessage);
            Controls.Add(lblMsmqLabel);
            Controls.Add(txtBxLabel);
            Controls.Add(listBoxMessages);
            Controls.Add(txtBxMessage);
            Controls.Add(btnSendMessage);
            Name = "Form1";
            Text = "Form1";
            FormClosing += Form1_FormClosing;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button btnSendMessage;
        private TextBox txtBxMessage;
        private ListBox listBoxMessages;
        private TextBox txtBxLabel;
        private Label lblMsmqLabel;
        private Label lblMsmqMessage;
    }
}
