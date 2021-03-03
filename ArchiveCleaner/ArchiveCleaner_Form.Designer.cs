
namespace ArchiveCleaner
{
    partial class ArchiveCleaner_Form
    {
        /// <summary>
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Windows Form-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ArchiveCleaner_Form));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.SearchButton = new System.Windows.Forms.Button();
            this.SelectPathButton = new System.Windows.Forms.Button();
            this.PathTextBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.DateTextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.FilesListBox = new System.Windows.Forms.ListBox();
            this.DeleteAllButton = new System.Windows.Forms.Button();
            this.DeleteSelectedButton = new System.Windows.Forms.Button();
            this.CloseProgramButton = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.linkLabel2 = new System.Windows.Forms.LinkLabel();
            this.StatusLabel = new System.Windows.Forms.Label();
            this.StatusLabel1 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Controls.Add(this.SearchButton);
            this.groupBox1.Controls.Add(this.SelectPathButton);
            this.groupBox1.Controls.Add(this.PathTextBox);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.DateTextBox);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // SearchButton
            // 
            resources.ApplyResources(this.SearchButton, "SearchButton");
            this.SearchButton.Name = "SearchButton";
            this.SearchButton.UseVisualStyleBackColor = true;
            this.SearchButton.Click += new System.EventHandler(this.SearchButton_Click);
            // 
            // SelectPathButton
            // 
            resources.ApplyResources(this.SelectPathButton, "SelectPathButton");
            this.SelectPathButton.Name = "SelectPathButton";
            this.SelectPathButton.UseVisualStyleBackColor = true;
            this.SelectPathButton.Click += new System.EventHandler(this.SelectPathButton_Click);
            // 
            // PathTextBox
            // 
            resources.ApplyResources(this.PathTextBox, "PathTextBox");
            this.PathTextBox.Name = "PathTextBox";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // DateTextBox
            // 
            resources.ApplyResources(this.DateTextBox, "DateTextBox");
            this.DateTextBox.Name = "DateTextBox";
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // FilesListBox
            // 
            resources.ApplyResources(this.FilesListBox, "FilesListBox");
            this.FilesListBox.FormattingEnabled = true;
            this.FilesListBox.Name = "FilesListBox";
            this.FilesListBox.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            // 
            // DeleteAllButton
            // 
            resources.ApplyResources(this.DeleteAllButton, "DeleteAllButton");
            this.DeleteAllButton.Name = "DeleteAllButton";
            this.DeleteAllButton.UseVisualStyleBackColor = true;
            this.DeleteAllButton.Click += new System.EventHandler(this.DeleteAllButton_Click);
            // 
            // DeleteSelectedButton
            // 
            resources.ApplyResources(this.DeleteSelectedButton, "DeleteSelectedButton");
            this.DeleteSelectedButton.Name = "DeleteSelectedButton";
            this.DeleteSelectedButton.UseVisualStyleBackColor = true;
            this.DeleteSelectedButton.Click += new System.EventHandler(this.DeleteSelectedButton_Click);
            // 
            // CloseProgramButton
            // 
            resources.ApplyResources(this.CloseProgramButton, "CloseProgramButton");
            this.CloseProgramButton.Name = "CloseProgramButton";
            this.CloseProgramButton.UseVisualStyleBackColor = true;
            this.CloseProgramButton.Click += new System.EventHandler(this.CloseProgramButton_Click);
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // linkLabel1
            // 
            resources.ApplyResources(this.linkLabel1, "linkLabel1");
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.TabStop = true;
            this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
            // 
            // linkLabel2
            // 
            resources.ApplyResources(this.linkLabel2, "linkLabel2");
            this.linkLabel2.Name = "linkLabel2";
            this.linkLabel2.TabStop = true;
            this.linkLabel2.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel2_LinkClicked);
            // 
            // StatusLabel
            // 
            resources.ApplyResources(this.StatusLabel, "StatusLabel");
            this.StatusLabel.Name = "StatusLabel";
            // 
            // StatusLabel1
            // 
            resources.ApplyResources(this.StatusLabel1, "StatusLabel1");
            this.StatusLabel1.Name = "StatusLabel1";
            // 
            // ArchiveCleaner_Form
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.StatusLabel1);
            this.Controls.Add(this.StatusLabel);
            this.Controls.Add(this.linkLabel2);
            this.Controls.Add(this.linkLabel1);
            this.Controls.Add(this.CloseProgramButton);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.DeleteSelectedButton);
            this.Controls.Add(this.DeleteAllButton);
            this.Controls.Add(this.FilesListBox);
            this.Controls.Add(this.groupBox1);
            this.Name = "ArchiveCleaner_Form";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button SearchButton;
        private System.Windows.Forms.Button SelectPathButton;
        private System.Windows.Forms.TextBox PathTextBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox DateTextBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ListBox FilesListBox;
        private System.Windows.Forms.Button DeleteAllButton;
        private System.Windows.Forms.Button DeleteSelectedButton;
        private System.Windows.Forms.Button CloseProgramButton;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.LinkLabel linkLabel1;
        private System.Windows.Forms.LinkLabel linkLabel2;
        private System.Windows.Forms.Label StatusLabel1;
        internal System.Windows.Forms.Label StatusLabel;
    }
}

