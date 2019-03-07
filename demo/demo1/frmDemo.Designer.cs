namespace SuperFast
{
    partial class frmDemo
    {
        /// <summary>
        /// Variável de designer necessária.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpar os recursos que estão sendo usados.
        /// </summary>
        /// <param name="disposing">true se for necessário descartar os recursos gerenciados; caso contrário, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código gerado pelo Windows Form Designer

        /// <summary>
        /// Método necessário para suporte ao Designer - não modifique 
        /// o conteúdo deste método com o editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.btnLoadData = new System.Windows.Forms.Button();
            this.tmr = new System.Windows.Forms.Timer(this.components);
            this.btnCreateTable = new System.Windows.Forms.Button();
            this.dgResultado = new System.Windows.Forms.DataGridView();
            this.btnSelect = new System.Windows.Forms.Button();
            this.txtSolicitar = new System.Windows.Forms.TextBox();
            this.btnSolicitar = new System.Windows.Forms.Button();
            this.btnMultitFileRead = new System.Windows.Forms.Button();
            this.btnSql1 = new System.Windows.Forms.Button();
            this.btnSql2 = new System.Windows.Forms.Button();
            this.btnSql3 = new System.Windows.Forms.Button();
            this.btnSql4 = new System.Windows.Forms.Button();
            this.btnSql5 = new System.Windows.Forms.Button();
            this.btnSql6 = new System.Windows.Forms.Button();
            this.lblStatus = new System.Windows.Forms.Label();
            this.txtPosfixa = new System.Windows.Forms.TextBox();
            this.btnCreateTableDisco = new System.Windows.Forms.Button();
            this.btnSql7 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgResultado)).BeginInit();
            this.SuspendLayout();
            // 
            // btnLoadData
            // 
            this.btnLoadData.Location = new System.Drawing.Point(482, 9);
            this.btnLoadData.Name = "btnLoadData";
            this.btnLoadData.Size = new System.Drawing.Size(130, 23);
            this.btnLoadData.TabIndex = 1;
            this.btnLoadData.Text = "Load Data from MySql";
            this.btnLoadData.UseVisualStyleBackColor = true;
            this.btnLoadData.Click += new System.EventHandler(this.btnLoadData_Click);
            // 
            // tmr
            // 
            this.tmr.Enabled = true;
            this.tmr.Interval = 1;
            this.tmr.Tick += new System.EventHandler(this.tmr_Tick);
            // 
            // btnCreateTable
            // 
            this.btnCreateTable.Location = new System.Drawing.Point(5, 41);
            this.btnCreateTable.Name = "btnCreateTable";
            this.btnCreateTable.Size = new System.Drawing.Size(160, 23);
            this.btnCreateTable.TabIndex = 3;
            this.btnCreateTable.Text = "CreateTable (Memória)";
            this.btnCreateTable.UseVisualStyleBackColor = true;
            this.btnCreateTable.Click += new System.EventHandler(this.btnCreateTable_Click);
            // 
            // dgResultado
            // 
            this.dgResultado.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgResultado.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.dgResultado.Location = new System.Drawing.Point(0, 438);
            this.dgResultado.Name = "dgResultado";
            this.dgResultado.Size = new System.Drawing.Size(778, 196);
            this.dgResultado.TabIndex = 4;
            // 
            // btnSelect
            // 
            this.btnSelect.Location = new System.Drawing.Point(5, 99);
            this.btnSelect.Name = "btnSelect";
            this.btnSelect.Size = new System.Drawing.Size(160, 23);
            this.btnSelect.TabIndex = 5;
            this.btnSelect.Text = "Select Codigo = 100000";
            this.btnSelect.UseVisualStyleBackColor = true;
            this.btnSelect.Click += new System.EventHandler(this.btnSelect_Click);
            // 
            // txtSolicitar
            // 
            this.txtSolicitar.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.txtSolicitar.Location = new System.Drawing.Point(0, 302);
            this.txtSolicitar.Multiline = true;
            this.txtSolicitar.Name = "txtSolicitar";
            this.txtSolicitar.Size = new System.Drawing.Size(778, 136);
            this.txtSolicitar.TabIndex = 6;
            // 
            // btnSolicitar
            // 
            this.btnSolicitar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSolicitar.Location = new System.Drawing.Point(642, 273);
            this.btnSolicitar.Name = "btnSolicitar";
            this.btnSolicitar.Size = new System.Drawing.Size(136, 23);
            this.btnSolicitar.TabIndex = 7;
            this.btnSolicitar.Text = "Solicitar";
            this.btnSolicitar.UseVisualStyleBackColor = true;
            this.btnSolicitar.Click += new System.EventHandler(this.btnSolicitar_Click);
            // 
            // btnMultitFileRead
            // 
            this.btnMultitFileRead.Location = new System.Drawing.Point(5, 12);
            this.btnMultitFileRead.Name = "btnMultitFileRead";
            this.btnMultitFileRead.Size = new System.Drawing.Size(160, 23);
            this.btnMultitFileRead.TabIndex = 11;
            this.btnMultitFileRead.Text = "Multithread file read";
            this.btnMultitFileRead.UseVisualStyleBackColor = true;
            this.btnMultitFileRead.Click += new System.EventHandler(this.btnMultitFileRead_Click);
            // 
            // btnSql1
            // 
            this.btnSql1.Location = new System.Drawing.Point(12, 273);
            this.btnSql1.Name = "btnSql1";
            this.btnSql1.Size = new System.Drawing.Size(47, 23);
            this.btnSql1.TabIndex = 13;
            this.btnSql1.Text = "SQL1";
            this.btnSql1.UseVisualStyleBackColor = true;
            this.btnSql1.Click += new System.EventHandler(this.btnSql1_Click);
            // 
            // btnSql2
            // 
            this.btnSql2.Location = new System.Drawing.Point(65, 273);
            this.btnSql2.Name = "btnSql2";
            this.btnSql2.Size = new System.Drawing.Size(47, 23);
            this.btnSql2.TabIndex = 14;
            this.btnSql2.Text = "SQL2";
            this.btnSql2.UseVisualStyleBackColor = true;
            this.btnSql2.Click += new System.EventHandler(this.btnSql2_Click);
            // 
            // btnSql3
            // 
            this.btnSql3.Location = new System.Drawing.Point(118, 273);
            this.btnSql3.Name = "btnSql3";
            this.btnSql3.Size = new System.Drawing.Size(47, 23);
            this.btnSql3.TabIndex = 15;
            this.btnSql3.Text = "SQL3";
            this.btnSql3.UseVisualStyleBackColor = true;
            this.btnSql3.Click += new System.EventHandler(this.btnSql3_Click);
            // 
            // btnSql4
            // 
            this.btnSql4.Location = new System.Drawing.Point(171, 273);
            this.btnSql4.Name = "btnSql4";
            this.btnSql4.Size = new System.Drawing.Size(47, 23);
            this.btnSql4.TabIndex = 16;
            this.btnSql4.Text = "SQL4";
            this.btnSql4.UseVisualStyleBackColor = true;
            this.btnSql4.Click += new System.EventHandler(this.btnSql4_Click);
            // 
            // btnSql5
            // 
            this.btnSql5.Location = new System.Drawing.Point(224, 273);
            this.btnSql5.Name = "btnSql5";
            this.btnSql5.Size = new System.Drawing.Size(47, 23);
            this.btnSql5.TabIndex = 17;
            this.btnSql5.Text = "SQL5";
            this.btnSql5.UseVisualStyleBackColor = true;
            this.btnSql5.Click += new System.EventHandler(this.btnSql5_Click);
            // 
            // btnSql6
            // 
            this.btnSql6.Location = new System.Drawing.Point(277, 273);
            this.btnSql6.Name = "btnSql6";
            this.btnSql6.Size = new System.Drawing.Size(47, 23);
            this.btnSql6.TabIndex = 18;
            this.btnSql6.Text = "SQL6";
            this.btnSql6.UseVisualStyleBackColor = true;
            this.btnSql6.Click += new System.EventHandler(this.btnSql6_Click);
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Location = new System.Drawing.Point(12, 211);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(37, 13);
            this.lblStatus.TabIndex = 19;
            this.lblStatus.Text = "Status";
            // 
            // txtPosfixa
            // 
            this.txtPosfixa.Location = new System.Drawing.Point(12, 129);
            this.txtPosfixa.Multiline = true;
            this.txtPosfixa.Name = "txtPosfixa";
            this.txtPosfixa.Size = new System.Drawing.Size(754, 79);
            this.txtPosfixa.TabIndex = 20;
            // 
            // btnCreateTableDisco
            // 
            this.btnCreateTableDisco.Location = new System.Drawing.Point(5, 70);
            this.btnCreateTableDisco.Name = "btnCreateTableDisco";
            this.btnCreateTableDisco.Size = new System.Drawing.Size(160, 23);
            this.btnCreateTableDisco.TabIndex = 21;
            this.btnCreateTableDisco.Text = "CreateTable (Disco)";
            this.btnCreateTableDisco.UseVisualStyleBackColor = true;
            this.btnCreateTableDisco.Click += new System.EventHandler(this.btnCreateTableDisco_Click);
            // 
            // btnSql7
            // 
            this.btnSql7.Location = new System.Drawing.Point(330, 273);
            this.btnSql7.Name = "btnSql7";
            this.btnSql7.Size = new System.Drawing.Size(47, 23);
            this.btnSql7.TabIndex = 18;
            this.btnSql7.Text = "SQL7";
            this.btnSql7.UseVisualStyleBackColor = true;
            this.btnSql7.Click += new System.EventHandler(this.btnSql7_Click);
            // 
            // frmAlencarDB
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(778, 634);
            this.Controls.Add(this.btnCreateTableDisco);
            this.Controls.Add(this.txtPosfixa);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.btnSql7);
            this.Controls.Add(this.btnSql6);
            this.Controls.Add(this.btnSql5);
            this.Controls.Add(this.btnSql4);
            this.Controls.Add(this.btnSql3);
            this.Controls.Add(this.btnSql2);
            this.Controls.Add(this.btnSql1);
            this.Controls.Add(this.btnMultitFileRead);
            this.Controls.Add(this.btnSolicitar);
            this.Controls.Add(this.txtSolicitar);
            this.Controls.Add(this.btnSelect);
            this.Controls.Add(this.dgResultado);
            this.Controls.Add(this.btnCreateTable);
            this.Controls.Add(this.btnLoadData);
            this.Name = "frmAlencarDB";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.frmPrincipal_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgResultado)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnLoadData;
        private System.Windows.Forms.Timer tmr;
        private System.Windows.Forms.Button btnCreateTable;
        private System.Windows.Forms.DataGridView dgResultado;
        private System.Windows.Forms.Button btnSelect;
        private System.Windows.Forms.TextBox txtSolicitar;
        private System.Windows.Forms.Button btnSolicitar;
        private System.Windows.Forms.Button btnMultitFileRead;
        private System.Windows.Forms.Button btnSql1;
        private System.Windows.Forms.Button btnSql2;
        private System.Windows.Forms.Button btnSql3;
        private System.Windows.Forms.Button btnSql4;
        private System.Windows.Forms.Button btnSql5;
        private System.Windows.Forms.Button btnSql6;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.TextBox txtPosfixa;
        private System.Windows.Forms.Button btnCreateTableDisco;
        private System.Windows.Forms.Button btnSql7;
    }
}

