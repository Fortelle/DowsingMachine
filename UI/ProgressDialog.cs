using PBT.DowsingMachine.Structures;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PBT.DowsingMachine.UI
{
    public partial class ProgressDialog : Form
    {
        private bool AutoProcess { get; set; }

        private int count;
        public int Count
        {
            get
            {
                return count;
            }

            set
            {
                count = value;
                progressBar1.Maximum = count;
                progressBar1.Style = count == 0
                    ? ProgressBarStyle.Marquee
                    : ProgressBarStyle.Continuous;
            }
        }

        public int _counter;

        readonly WorkProcesser Work;

        private DateTime BeginTime;
        private CancellationTokenSource CTS;

        public ProgressDialog()
        {
            InitializeComponent();

            Count = 0;
            progressBar1.Value = 0;

            lblStatus.Text = string.Empty;
            lblProgress.Text = count == 0
                ? "0"
                : $"0/{count} (0%)";
        }

        public ProgressDialog(WorkProcesser workProcesser2, bool autoProcess) : this()
        {
            Work = workProcesser2;
            Count = workProcesser2.Size;
            AutoProcess = autoProcess;

            btnStart.Visible = !autoProcess;
        }

        private void ProgressDialog_Load(object sender, EventArgs e)
        {
        }

        public void Begin()
        {
            BeginTime = DateTime.Now;
            _counter = 0;
            UpdateUI();
            timer1.Enabled = true;

            if (Work.Queue is IEnumerable<Func<Task>>)
            {
                Task.Run(DoWorkAsync);
            }
            else
            {
                backgroundWorker1.RunWorkerAsync();
            }
        }

        private async void DoWorkAsync()
        {
            var queue = Work.Queue.Cast<Func<Task>>().Select(x => x());
            await Task.WhenAll(queue);
            Invoke(End);
        }

        public void UpdateUI()
        {
            if (count == 0)
            {
                lblProgress.Text = $"{_counter}";
            }
            else
            {
                lblProgress.Text = $"{_counter}/{Count} ({(double)_counter / Count:0%})";
                progressBar1.Value = _counter;
            }

            lblTimes.Text = $"{DateTime.Now - BeginTime:c}";
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            UpdateUI();
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            Begin();
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            CTS = new();
            
            DoWorkParallel();
        }

        private void DoWorkParallel()
        {
            var po = new ParallelOptions()
            {
                CancellationToken = CTS.Token,
                MaxDegreeOfParallelism = 8,
            };

            try
            {
                Parallel.ForEach(Work.Queue.Cast<object>(), po, (item, loopState) =>
                {
                    Interlocked.Increment(ref _counter);
                    backgroundWorker1.ReportProgress(0);
                });
            }
            catch (OperationCanceledException)
            {

            }
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            CTS.Dispose();

            End();
        }

        private void End()
        {
            timer1.Enabled = false;


            if (AutoProcess)
            {
                Application.DoEvents();
                Close();
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            CTS?.Cancel();
        }

        private void ProgressDialog_Shown(object sender, EventArgs e)
        {
            if (AutoProcess)
            {
                Application.DoEvents();
                Begin();
            }
        }
    }
}
