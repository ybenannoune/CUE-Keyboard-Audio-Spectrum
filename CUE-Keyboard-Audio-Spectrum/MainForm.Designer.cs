namespace CUE_Keyboard_Audio_Spectrum
{
    partial class MainForm
    {
        /// <summary>
        /// Variable nécessaire au concepteur.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Nettoyage des ressources utilisées.
        /// </summary>
        /// <param name="disposing">true si les ressources managées doivent être supprimées ; sinon, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Code généré par le Concepteur Windows Form

        /// <summary>
        /// Méthode requise pour la prise en charge du concepteur - ne modifiez pas
        /// le contenu de cette méthode avec l'éditeur de code.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.comboBox_deviceList = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.button_Start = new System.Windows.Forms.Button();
            this.chart_Spectrum = new System.Windows.Forms.DataVisualization.Charting.Chart();
            ((System.ComponentModel.ISupportInitialize)(this.chart_Spectrum)).BeginInit();
            this.SuspendLayout();
            // 
            // comboBox_deviceList
            // 
            this.comboBox_deviceList.FormattingEnabled = true;
            this.comboBox_deviceList.Location = new System.Drawing.Point(99, 12);
            this.comboBox_deviceList.Name = "comboBox_deviceList";
            this.comboBox_deviceList.Size = new System.Drawing.Size(219, 21);
            this.comboBox_deviceList.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(81, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Target Device :";
            // 
            // button_Start
            // 
            this.button_Start.Location = new System.Drawing.Point(324, 10);
            this.button_Start.Name = "button_Start";
            this.button_Start.Size = new System.Drawing.Size(75, 23);
            this.button_Start.TabIndex = 2;
            this.button_Start.Text = "Start";
            this.button_Start.UseVisualStyleBackColor = true;
            this.button_Start.Click += new System.EventHandler(this.button_Start_Click);
            // 
            // chart_Spectrum
            // 
            chartArea1.Name = "ChartArea1";
            this.chart_Spectrum.ChartAreas.Add(chartArea1);
            this.chart_Spectrum.Location = new System.Drawing.Point(12, 39);
            this.chart_Spectrum.Name = "chart_Spectrum";
            series1.ChartArea = "ChartArea1";
            series1.Name = "Series1";
            this.chart_Spectrum.Series.Add(series1);
            this.chart_Spectrum.Size = new System.Drawing.Size(406, 191);
            this.chart_Spectrum.TabIndex = 3;
            this.chart_Spectrum.Text = "chart1";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(430, 242);
            this.Controls.Add(this.chart_Spectrum);
            this.Controls.Add(this.button_Start);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.comboBox_deviceList);
            this.Name = "MainForm";
            this.Text = "CUE-Keyboard-Audio-Spectrum";
            ((System.ComponentModel.ISupportInitialize)(this.chart_Spectrum)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox comboBox_deviceList;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button_Start;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart_Spectrum;
    }
}

