namespace InterpretadorSQL
{
    partial class frmMain
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
            this.btnSolicitacaoSQL = new System.Windows.Forms.Button();
            this.txtSql = new System.Windows.Forms.TextBox();
            this.txtResultado = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtPosFixa = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txtIteracoes = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnSolicitacaoSQL
            // 
            this.btnSolicitacaoSQL.Location = new System.Drawing.Point(231, 162);
            this.btnSolicitacaoSQL.Name = "btnSolicitacaoSQL";
            this.btnSolicitacaoSQL.Size = new System.Drawing.Size(153, 23);
            this.btnSolicitacaoSQL.TabIndex = 3;
            this.btnSolicitacaoSQL.Text = "Solicitação SQL";
            this.btnSolicitacaoSQL.UseVisualStyleBackColor = true;
            this.btnSolicitacaoSQL.Click += new System.EventHandler(this.btnSolicitacaoSQL_Click);
            // 
            // txtSql
            // 
            this.txtSql.Location = new System.Drawing.Point(13, 85);
            this.txtSql.Multiline = true;
            this.txtSql.Name = "txtSql";
            this.txtSql.Size = new System.Drawing.Size(371, 71);
            this.txtSql.TabIndex = 4;
            this.txtSql.Text = "select 5 + ((7 / 6 + 7 + (sin(2*2-8 - cos(2) - 5 * 3 + 6 /4) * 3 + 6 - tan(34)) *" +
    " 2 + 5 - 6) - 5) + 3 * 2";
            // 
            // txtResultado
            // 
            this.txtResultado.Location = new System.Drawing.Point(12, 218);
            this.txtResultado.Name = "txtResultado";
            this.txtResultado.Size = new System.Drawing.Size(174, 20);
            this.txtResultado.TabIndex = 5;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 202);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(58, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "Resultado:";
            // 
            // txtPosFixa
            // 
            this.txtPosFixa.Location = new System.Drawing.Point(12, 30);
            this.txtPosFixa.Name = "txtPosFixa";
            this.txtPosFixa.ReadOnly = true;
            this.txtPosFixa.Size = new System.Drawing.Size(371, 20);
            this.txtPosFixa.TabIndex = 7;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 14);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(50, 13);
            this.label2.TabIndex = 8;
            this.label2.Text = "Pós Fixa:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 69);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(79, 13);
            this.label3.TabIndex = 9;
            this.label3.Text = "Comando SQL:";
            // 
            // txtIteracoes
            // 
            this.txtIteracoes.Location = new System.Drawing.Point(319, 62);
            this.txtIteracoes.Name = "txtIteracoes";
            this.txtIteracoes.Size = new System.Drawing.Size(64, 20);
            this.txtIteracoes.TabIndex = 10;
            this.txtIteracoes.Text = "1000";
            this.txtIteracoes.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(219, 65);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(64, 13);
            this.label4.TabIndex = 11;
            this.label4.Text = "Repetições:";
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(396, 424);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtIteracoes);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtPosFixa);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtResultado);
            this.Controls.Add(this.txtSql);
            this.Controls.Add(this.btnSolicitacaoSQL);
            this.Name = "frmMain";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button btnSolicitacaoSQL;
        private System.Windows.Forms.TextBox txtSql;
        private System.Windows.Forms.TextBox txtResultado;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtPosFixa;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtIteracoes;
        private System.Windows.Forms.Label label4;
    }
}

