namespace GoogleMusicTest
{
    partial class GoogleTest
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnGetPlaylists = new System.Windows.Forms.Button();
            this.btnFetchSongs = new System.Windows.Forms.Button();
            this.btnCreatePl = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.btnLogin = new System.Windows.Forms.Button();
            this.tbPass = new System.Windows.Forms.TextBox();
            this.tbEmail = new System.Windows.Forms.TextBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnSongURL = new System.Windows.Forms.Button();
            this.panel3 = new System.Windows.Forms.Panel();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.lvSongs = new System.Windows.Forms.ListView();
            this.lbPlaylists = new System.Windows.Forms.ListBox();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnSongURL);
            this.panel1.Controls.Add(this.btnGetPlaylists);
            this.panel1.Controls.Add(this.btnFetchSongs);
            this.panel1.Controls.Add(this.btnCreatePl);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.btnLogin);
            this.panel1.Controls.Add(this.tbPass);
            this.panel1.Controls.Add(this.tbEmail);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(745, 98);
            this.panel1.TabIndex = 0;
            // 
            // btnGetPlaylists
            // 
            this.btnGetPlaylists.Location = new System.Drawing.Point(349, 33);
            this.btnGetPlaylists.Name = "btnGetPlaylists";
            this.btnGetPlaylists.Size = new System.Drawing.Size(132, 23);
            this.btnGetPlaylists.TabIndex = 7;
            this.btnGetPlaylists.Text = "Fetch Playlists";
            this.btnGetPlaylists.UseVisualStyleBackColor = true;
            this.btnGetPlaylists.Click += new System.EventHandler(this.btnGetPlaylists_Click);
            // 
            // btnFetchSongs
            // 
            this.btnFetchSongs.Location = new System.Drawing.Point(349, 3);
            this.btnFetchSongs.Name = "btnFetchSongs";
            this.btnFetchSongs.Size = new System.Drawing.Size(132, 23);
            this.btnFetchSongs.TabIndex = 6;
            this.btnFetchSongs.Text = "Fetch Tracks";
            this.btnFetchSongs.UseVisualStyleBackColor = true;
            this.btnFetchSongs.Click += new System.EventHandler(this.btnFetchSongs_Click);
            // 
            // btnCreatePl
            // 
            this.btnCreatePl.Location = new System.Drawing.Point(349, 64);
            this.btnCreatePl.Name = "btnCreatePl";
            this.btnCreatePl.Size = new System.Drawing.Size(132, 23);
            this.btnCreatePl.TabIndex = 5;
            this.btnCreatePl.Text = "Create playlist \"Testing\"";
            this.btnCreatePl.UseVisualStyleBackColor = true;
            this.btnCreatePl.Click += new System.EventHandler(this.btnCreatePl_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(22, 41);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(56, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Password:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(43, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Email:";
            // 
            // btnLogin
            // 
            this.btnLogin.Location = new System.Drawing.Point(213, 64);
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.Size = new System.Drawing.Size(75, 23);
            this.btnLogin.TabIndex = 2;
            this.btnLogin.Text = "Login";
            this.btnLogin.UseVisualStyleBackColor = true;
            this.btnLogin.Click += new System.EventHandler(this.btnLogin_Click);
            // 
            // tbPass
            // 
            this.tbPass.Location = new System.Drawing.Point(84, 38);
            this.tbPass.Name = "tbPass";
            this.tbPass.Size = new System.Drawing.Size(204, 20);
            this.tbPass.TabIndex = 1;
            this.tbPass.UseSystemPasswordChar = true;
            // 
            // tbEmail
            // 
            this.tbEmail.Location = new System.Drawing.Point(84, 12);
            this.tbEmail.Name = "tbEmail";
            this.tbEmail.Size = new System.Drawing.Size(204, 20);
            this.tbEmail.TabIndex = 0;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.lvSongs);
            this.panel2.Controls.Add(this.panel3);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 98);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(745, 483);
            this.panel2.TabIndex = 1;
            // 
            // btnSongURL
            // 
            this.btnSongURL.Location = new System.Drawing.Point(510, 3);
            this.btnSongURL.Name = "btnSongURL";
            this.btnSongURL.Size = new System.Drawing.Size(138, 23);
            this.btnSongURL.TabIndex = 8;
            this.btnSongURL.Text = "Get Selected Song URL";
            this.btnSongURL.UseVisualStyleBackColor = true;
            this.btnSongURL.Click += new System.EventHandler(this.btnSongURL_Click);
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.lbPlaylists);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(121, 483);
            this.panel3.TabIndex = 0;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "ID";
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Title";
            this.columnHeader2.Width = 96;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Artist";
            this.columnHeader3.Width = 114;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "Album";
            this.columnHeader4.Width = 142;
            // 
            // columnHeader5
            // 
            this.columnHeader5.Text = "gid";
            // 
            // lvSongs
            // 
            this.lvSongs.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader4,
            this.columnHeader5});
            this.lvSongs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvSongs.Location = new System.Drawing.Point(121, 0);
            this.lvSongs.Name = "lvSongs";
            this.lvSongs.Size = new System.Drawing.Size(624, 483);
            this.lvSongs.TabIndex = 2;
            this.lvSongs.UseCompatibleStateImageBehavior = false;
            this.lvSongs.View = System.Windows.Forms.View.Details;
            // 
            // lbPlaylists
            // 
            this.lbPlaylists.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbPlaylists.FormattingEnabled = true;
            this.lbPlaylists.Location = new System.Drawing.Point(0, 0);
            this.lbPlaylists.Name = "lbPlaylists";
            this.lbPlaylists.Size = new System.Drawing.Size(121, 483);
            this.lbPlaylists.TabIndex = 4;
            // 
            // GoogleTest
            // 
            this.AcceptButton = this.btnLogin;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(745, 581);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "GoogleTest";
            this.Text = "GoogleMusicAPI.NET Test";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnLogin;
        private System.Windows.Forms.TextBox tbPass;
        private System.Windows.Forms.TextBox tbEmail;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnCreatePl;
        private System.Windows.Forms.Button btnFetchSongs;
        private System.Windows.Forms.Button btnGetPlaylists;
        private System.Windows.Forms.Button btnSongURL;
        private System.Windows.Forms.ListView lvSongs;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.ColumnHeader columnHeader5;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.ListBox lbPlaylists;

    }
}

