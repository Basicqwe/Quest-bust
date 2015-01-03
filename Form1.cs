using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace WindowsFormsApplication3
{
    public partial class MainForm : Form
    {
        private int[,] mas = new int[4, 4];
        DateTime lastUpdateTime;
        long updateInterval = 100; //milliseconds

        public MainForm()
        {
            InitializeComponent();
            updateInterval *= 10000; //Переводим в 100 наносекундные промежутки
        }

        private void buttonCalc_Click(object sender, EventArgs e)
        {
            for(int i = 0; i < 4; ++i)
                for (int j = 0; j < 4; ++j)
                {
                    mas[i, j] = 1;
                }
            mas[0, 3] = 5;
            mas[1, 2] = 2;
            mas[2, 1] = 6;
            mas[3, 0] = 4;

            
            ShowResult();
            lastUpdateTime = DateTime.Now;

            buttonCalc.Enabled = false;
            progressBar1.Style = ProgressBarStyle.Marquee;

            backgroundWorker1.RunWorkerAsync();
        }

        private void ShowResult()
        {
            labelX11.Text = mas[0, 0].ToString();
            labelX13.Text = mas[0, 1].ToString();
            labelX15.Text = mas[0, 2].ToString();
            labelX17.Text = mas[0, 3].ToString();
            labelX31.Text = mas[1, 0].ToString();
            labelX33.Text = mas[1, 1].ToString();
            labelX35.Text = mas[1, 2].ToString();
            labelX37.Text = mas[1, 3].ToString();
            labelX51.Text = mas[2, 0].ToString();
            labelX53.Text = mas[2, 1].ToString();
            labelX55.Text = mas[2, 2].ToString();
            labelX57.Text = mas[2, 3].ToString();
            labelX71.Text = mas[3, 0].ToString();
            labelX73.Text = mas[3, 1].ToString();
            labelX75.Text = mas[3, 2].ToString();
            labelX77.Text = mas[3, 3].ToString();
        }

        private void Check(BackgroundWorker worker)
        {
            if (DateTime.Now.ToFileTime() - lastUpdateTime.ToFileTime() > updateInterval)
            {
                lastUpdateTime = DateTime.Now;
                worker.ReportProgress(0);
            }
        }

        private void Calc(BackgroundWorker worker, DoWorkEventArgs e)
        {
            bool result = false;

            for (mas[0, 0] = 1; mas[0, 0] <= 10; ++mas[0, 0])
                for (mas[0, 1] = 1; mas[0, 1] <= 10; ++mas[0, 1])
                    for (mas[0, 2] = 1; mas[0, 2] <= 10; ++mas[0, 2])
                    {
                        Check(worker);
                        if (mas[0, 0] + mas[0, 1] / mas[0, 2] != 5 || mas[0, 1] % mas[0, 2] != 0) // 1
                        {
                            continue;
                        }

                        for (mas[1, 0] = 1; mas[1, 0] <= 10; ++mas[1, 0])
                        {
                            for (mas[1, 1] = 1; mas[1, 1] <= 10; ++mas[1, 1])
                                for (mas[1, 3] = 1; mas[1, 3] <= 10; ++mas[1, 3])
                                {
                                    Check(worker);
                                    if (mas[1, 0] + mas[1, 1] / 2 != mas[1, 3] || mas[1, 1] % 2 != 0) // 2
                                    {
                                        continue;
                                    }

                                    for (mas[2, 0] = 1; mas[2, 0] <= 10; ++mas[2, 0])
                                        for (mas[2, 2] = 1; mas[2, 2] <= 10; ++mas[2, 2])
                                        {
                                            Check(worker);
                                            if (mas[2, 2] != 1 && mas[2, 2] != 2 && mas[2, 2] != 3 && mas[2, 2] != 6) 
                                            {
                                                continue;
                                            }

                                            for (mas[2, 3] = 1; mas[2, 3] <= 10; ++mas[2, 3])
                                            {
                                                Check(worker);
                                                if (mas[2, 0] + 6 / mas[2, 2] != mas[2, 3] || 6 % mas[2, 2] != 0) // 3
                                                {
                                                    continue;
                                                }

                                                for (mas[3, 1] = 1; mas[3, 1] <= 10; ++mas[3, 1])
                                                    for (mas[3, 2] = 1; mas[3, 2] <= 10; ++mas[3, 2])
                                                        for (mas[3, 3] = 1; mas[3, 3] <= 10; ++mas[3, 3])
                                                        {
                                                            Check(worker);

                                                            if (4 + mas[3, 1] / mas[3, 2] != mas[3, 3] || mas[3, 1] % mas[3, 2] != 0) // 4
                                                            {
                                                                continue;
                                                            }

                                                            if (mas[0, 0] + mas[1, 0] / mas[2, 0] != 4 || mas[1, 0] % mas[2, 0] != 0)
                                                            {
                                                                continue;
                                                            }
                                                            if (mas[0, 1] * mas[1, 1] / 6 != mas[3, 1] || (mas[0, 1] * mas[1, 1]) % 6 != 0)
                                                            {
                                                                continue;
                                                            }
                                                            if (mas[0, 2]  + 2 - mas[2, 2] != mas[3, 2])
                                                            {
                                                                continue;
                                                            }

                                                            if (5 - mas[1, 3] + mas[2, 3] != mas[3, 3])
                                                            {
                                                                continue;
                                                            }

                                                            goto exit;
                                                        }
                                            }
                                        }
                                }
                        }
                    }

        exit:
            result = true;
            worker.ReportProgress(0);
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            Calc(worker, e);
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            progressBar1.Visible = false;
            buttonCalc.Enabled = true;
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            ShowResult();
        }
    }
}
