using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace WinForGIFSicle
{
    public partial class Form1 : Form
    {
        private BackgroundWorker bgWorker;
        private List<string> fileList = new List<string>();
        private int leftIndex = -1;
        private int rightIndex = -1;
        private volatile bool previewRunning;

        private Image origImage;
        private MemoryStream origStream;
        private FrameDimension origDim;
        private int origFrameCount;
        private int origFrame;

        private Image newImage;
        private MemoryStream newStream;
        private FrameDimension newDim;
        private int newFrameCount;
        private int newFrame;

        private string currentOrigPath;
        private string currentNewPath;

        private class CompressionSettings
        {
            public string OptLevel;
            public int Scale;
            public int Lossy;
            public int Colors;
            public bool NoComments;
            public bool NoExtensions;
            public bool Interlace;
            public int LoopCount;
            public int Delay;
            public string Dispose;
            public bool SaveToOriginalDir;
            public bool SaveToDesktop;
            public bool SaveToCustomDir;
            public string CustomDir;
            public bool UseOrigName;
            public bool UseTimeStamp;
            public bool UseCustomName;
            public string CustomName;
            public bool ReplaceName;  // true=完全改名, false=增加名字
        }

        private class ProgressData
        {
            public string FileName;
            public string OrigSizeStr;
            public string NewSizeStr;
            public string Ratio;
            public string Status;
            public string OrigImagePath;
            public string NewImagePath;
        }

        private string PreviewTempFile
        {
            get { return Path.Combine(Path.GetTempPath(), "__gifsicle_preview__.gif"); }
        }

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            cmbOptLevel.SelectedIndex = 2;
            cmbDispose.SelectedIndex = 0;
            dgvResults.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvResults.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            bgWorker = new BackgroundWorker();
            bgWorker.WorkerReportsProgress = true;
            bgWorker.DoWork += BgWorker_DoWork;
            bgWorker.ProgressChanged += BgWorker_ProgressChanged;
            bgWorker.RunWorkerCompleted += BgWorker_RunWorkerCompleted;

            ShowSimple();
        }

        #region Simple / Advanced
        private void btnSimple_Click(object sender, EventArgs e) { ShowSimple(); }
        private void btnAdvanced_Click(object sender, EventArgs e) { ShowAdvanced(); }

        private void ShowSimple()
        {
            lblScale.Visible = true; numScale.Visible = true; btnResetScale.Visible = true; lblScalePct.Visible = true;
            lblLossy.Visible = true; numLossy.Visible = true; btnResetLossy.Visible = true;
            lblColors.Visible = true; numColors.Visible = true; btnResetColors.Visible = true; lblColorsHint.Visible = true;
            chkNoComments.Visible = true; chkNoExtensions.Visible = true;
            lblOptLevel.Visible = false; cmbOptLevel.Visible = false;
            chkInterlace.Visible = false; chkNoWarnings.Visible = false;
            lblLoopCount.Visible = false; numLoopCount.Visible = false; lblLoopHint.Visible = false;
            SetAdvancedExtraVisible(false);
        }

        private void ShowAdvanced()
        {
            lblOptLevel.Visible = true; cmbOptLevel.Visible = true;
            chkInterlace.Visible = true; chkNoWarnings.Visible = true;
            lblLoopCount.Visible = true; numLoopCount.Visible = true; lblLoopHint.Visible = true;
            SetAdvancedExtraVisible(true);
            lblScale.Visible = false; numScale.Visible = false; btnResetScale.Visible = false; lblScalePct.Visible = false;
            lblLossy.Visible = false; numLossy.Visible = false; btnResetLossy.Visible = false;
            lblColors.Visible = false; numColors.Visible = false; btnResetColors.Visible = false; lblColorsHint.Visible = false;
            chkNoComments.Visible = false; chkNoExtensions.Visible = false;
        }

        private void SetAdvancedExtraVisible(bool v)
        {
            lblCatFrame.Visible = v; lblAdvDelay.Visible = v; numDelay.Visible = v; lblAdvCs.Visible = v;
            lblAdvDispose.Visible = v; cmbDispose.Visible = v;
        }
        #endregion

        #region Reset
        private void btnResetScale_Click(object sender, EventArgs e) { numScale.Value = 100; }
        private void btnResetLossy_Click(object sender, EventArgs e) { numLossy.Value = 0; }
        private void btnResetColors_Click(object sender, EventArgs e) { numColors.Value = 256; }

        private void btnResetAll_Click(object sender, EventArgs e)
        {
            cmbOptLevel.SelectedIndex = 2; numScale.Value = 100; numLossy.Value = 0; numColors.Value = 256;
            chkNoComments.Checked = false; chkNoExtensions.Checked = false;
            chkInterlace.Checked = false; chkNoWarnings.Checked = false; numLoopCount.Value = 0;
            numDelay.Value = 0; cmbDispose.SelectedIndex = 0;
        }
        #endregion

        #region Real-time Preview
        private void SettingChanged(object sender, EventArgs e)
        {
            if (leftIndex < 0 || bgWorker.IsBusy) return;
            previewDebounceTimer.Stop();
            previewDebounceTimer.Start();
        }

        private void previewDebounceTimer_Tick(object sender, EventArgs e)
        {
            previewDebounceTimer.Stop();
            StartPreviewCompression();
        }

        private void StartPreviewCompression()
        {
            if (leftIndex < 0 || leftIndex >= fileList.Count) return;
            if (previewRunning || bgWorker.IsBusy) return;
            string srcFile = fileList[leftIndex];
            if (string.IsNullOrEmpty(srcFile) || !File.Exists(srcFile)) return;

            previewRunning = true;
            var settings = CaptureSettings();
            string tempFile = PreviewTempFile;

            ThreadPool.QueueUserWorkItem(_ =>
            {
                try
                {
                    string args = BuildArgs(srcFile, tempFile, settings);
                    ProcessStartInfo psi = new ProcessStartInfo();
                    psi.FileName = "gifsicle.exe";
                    psi.Arguments = args;
                    psi.UseShellExecute = false;
                    psi.RedirectStandardError = true;
                    psi.CreateNoWindow = true;

                    using (Process proc = Process.Start(psi))
                    {
                        proc.WaitForExit();
                    }

                    if (File.Exists(tempFile))
                    {
                        long origSize = new FileInfo(srcFile).Length;
                        long newSize = new FileInfo(tempFile).Length;
                        string origSizeStr = FormatFileSize(origSize);
                        string newSizeStr = FormatFileSize(newSize);
                        string ratio = origSize > 0 ? ((1.0 - (double)newSize / origSize) * 100.0).ToString("0.0") + "%" : "";
                        string fileName = Path.GetFileName(srcFile);

                        this.Invoke((MethodInvoker)delegate
                        {
                            try
                            {
                                SetNewImage(tempFile);
                                currentNewPath = tempFile;
                                ShowNewAnimated();
                                lblNewInfo.Text = TruncateFileName(fileName) + "  |  " + newSizeStr + "  |  节省 " + ratio;
                            }
                            catch { }
                            previewRunning = false;
                        });
                    }
                    else
                    {
                        this.Invoke((MethodInvoker)delegate
                        {
                            SetNewImage(null);
                            currentNewPath = null;
                            if (picCompressed.Image != null) { picCompressed.Image.Dispose(); picCompressed.Image = null; }
                            lblNewInfo.Text = "预览压缩失败";
                            previewRunning = false;
                        });
                    }
                }
                catch
                {
                    this.Invoke((MethodInvoker)delegate
                    {
                        SetNewImage(null);
                        currentNewPath = null;
                        if (picCompressed.Image != null) { picCompressed.Image.Dispose(); picCompressed.Image = null; }
                        lblNewInfo.Text = "预览压缩失败";
                        previewRunning = false;
                    });
                }
            });
        }
        #endregion

        #region GIF Animation
        private void SetOrigImage(string path)
        {
            if (origImage != null) { origImage.Dispose(); origImage = null; }
            if (origStream != null) { origStream.Dispose(); origStream = null; }
            origFrameCount = 0; origFrame = 0;
            if (string.IsNullOrEmpty(path) || !File.Exists(path)) return;
            byte[] data = File.ReadAllBytes(path);
            origStream = new MemoryStream(data);
            origImage = Image.FromStream(origStream);
            if (origImage.FrameDimensionsList.Length > 0)
            {
                origDim = new FrameDimension(origImage.FrameDimensionsList[0]);
                origFrameCount = origImage.GetFrameCount(origDim);
            }
        }

        private void SetNewImage(string path)
        {
            if (newImage != null) { newImage.Dispose(); newImage = null; }
            if (newStream != null) { newStream.Dispose(); newStream = null; }
            newFrameCount = 0; newFrame = 0;
            if (string.IsNullOrEmpty(path) || !File.Exists(path)) return;
            byte[] data = File.ReadAllBytes(path);
            newStream = new MemoryStream(data);
            newImage = Image.FromStream(newStream);
            if (newImage.FrameDimensionsList.Length > 0)
            {
                newDim = new FrameDimension(newImage.FrameDimensionsList[0]);
                newFrameCount = newImage.GetFrameCount(newDim);
            }
        }

        private void ShowOrigAnimated()
        {
            if (origImage == null) { picOriginal.Image = null; return; }
            if (origFrameCount <= 1) { picOriginal.Image = new Bitmap(origImage); return; }
            origFrame = 0;
            origImage.SelectActiveFrame(origDim, 0);
            picOriginal.Image = new Bitmap(origImage);
            RefreshAnimTimer();
        }

        private void ShowNewAnimated()
        {
            if (newImage == null) { picCompressed.Image = null; return; }
            if (newFrameCount <= 1) { picCompressed.Image = new Bitmap(newImage); return; }
            newFrame = 0;
            newImage.SelectActiveFrame(newDim, 0);
            picCompressed.Image = new Bitmap(newImage);
            RefreshAnimTimer();
        }

        private void RefreshAnimTimer()
        {
            bool need = (origImage != null && origFrameCount > 1 && chkPreviewOrig.Checked) ||
                        (newImage != null && newFrameCount > 1 && chkPreviewNew.Checked);
            if (need && !animTimer.Enabled) animTimer.Start();
            else if (!need && animTimer.Enabled) animTimer.Stop();
        }

        private void StopAnimation() { if (animTimer.Enabled) animTimer.Stop(); }

        private void animTimer_Tick(object sender, EventArgs e)
        {
            bool any = false;
            if (origImage != null && origFrameCount > 1 && chkPreviewOrig.Checked)
            {
                origFrame = (origFrame + 1) % origFrameCount;
                origImage.SelectActiveFrame(origDim, origFrame);
                Image old = picOriginal.Image;
                picOriginal.Image = new Bitmap(origImage);
                if (old != null) old.Dispose();
                any = true;
            }
            if (newImage != null && newFrameCount > 1 && chkPreviewNew.Checked)
            {
                newFrame = (newFrame + 1) % newFrameCount;
                newImage.SelectActiveFrame(newDim, newFrame);
                Image old = picCompressed.Image;
                picCompressed.Image = new Bitmap(newImage);
                if (old != null) old.Dispose();
                any = true;
            }
            if (!any) animTimer.Stop();
        }

        private void chkPreviewOrig_CheckedChanged(object sender, EventArgs e)
        {
            if (chkPreviewOrig.Checked) ShowOrigAnimated();
            else if (origImage != null) { origImage.SelectActiveFrame(origDim, 0); picOriginal.Image = new Bitmap(origImage); }
            RefreshAnimTimer();
        }

        private void chkPreviewNew_CheckedChanged(object sender, EventArgs e)
        {
            if (chkPreviewNew.Checked) ShowNewAnimated();
            else if (newImage != null) { newImage.SelectActiveFrame(newDim, 0); picCompressed.Image = new Bitmap(newImage); }
            RefreshAnimTimer();
        }
        #endregion

        #region File List & Preview
        private void RebuildFileList()
        {
            string[] lines = txtFilePath.Text.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            fileList = lines.Where(f => !string.IsNullOrWhiteSpace(f)).Select(f => f.Trim()).ToList();
            leftIndex = fileList.Count > 0 ? 0 : -1;
            rightIndex = leftIndex;
            UpdateNavButtons();
            if (leftIndex >= 0) ShowFilePreview(leftIndex);
            else ClearPreview();
        }

        private void UpdateNavButtons()
        {
            bool hasFiles = fileList.Count > 1;
            btnPrevLeft.Enabled = hasFiles && leftIndex > 0;
            btnNextLeft.Enabled = hasFiles && leftIndex < fileList.Count - 1;
            lblLeftIndex.Text = (fileList.Count > 0 && leftIndex >= 0) ? string.Format("{0}/{1}", leftIndex + 1, fileList.Count) : "0/0";

            btnPrevRight.Enabled = hasFiles && rightIndex > 0;
            btnNextRight.Enabled = hasFiles && rightIndex < fileList.Count - 1;
            lblRightIndex.Text = (fileList.Count > 0 && rightIndex >= 0) ? string.Format("{0}/{1}", rightIndex + 1, fileList.Count) : "0/0";
        }

        private void ShowFilePreview(int index)
        {
            if (index < 0 || index >= fileList.Count) return;
            string filePath = fileList[index];
            if (!File.Exists(filePath)) return;
            try
            {
                SetOrigImage(filePath);
                currentOrigPath = filePath;
                SetNewImage(null);
                currentNewPath = null;
                ShowOrigAnimated();
                if (picCompressed.Image != null) { picCompressed.Image.Dispose(); picCompressed.Image = null; }
                lblOrigInfo.Text = TruncateFileName(Path.GetFileName(filePath)) + "  |  " + FormatFileSize(new FileInfo(filePath).Length);
                lblNewInfo.Text = "调整选项预览压缩效果...";
                previewDebounceTimer.Stop();
                previewDebounceTimer.Start();
            }
            catch { }
        }

        private void ClearPreview()
        {
            SetOrigImage(null); SetNewImage(null);
            currentOrigPath = null;
            currentNewPath = null;
            if (picOriginal.Image != null) { picOriginal.Image.Dispose(); picOriginal.Image = null; }
            if (picCompressed.Image != null) { picCompressed.Image.Dispose(); picCompressed.Image = null; }
            lblOrigInfo.Text = "请选择文件..."; lblNewInfo.Text = "等待压缩...";
            lblLeftIndex.Text = "0/0"; lblRightIndex.Text = "0/0";
        }

        private void btnPrevLeft_Click(object sender, EventArgs e)
        {
            if (leftIndex > 0) { leftIndex--; if (chkSync.Checked) rightIndex = leftIndex; ShowFilePreview(leftIndex); UpdateNavButtons(); }
        }

        private void btnNextLeft_Click(object sender, EventArgs e)
        {
            if (leftIndex < fileList.Count - 1) { leftIndex++; if (chkSync.Checked) rightIndex = leftIndex; ShowFilePreview(leftIndex); UpdateNavButtons(); }
        }

        private void btnPrevRight_Click(object sender, EventArgs e)
        {
            if (rightIndex > 0) { rightIndex--; if (chkSync.Checked) { leftIndex = rightIndex; ShowFilePreview(leftIndex); } UpdateNavButtons(); }
        }

        private void btnNextRight_Click(object sender, EventArgs e)
        {
            if (rightIndex < fileList.Count - 1) { rightIndex++; if (chkSync.Checked) { leftIndex = rightIndex; ShowFilePreview(leftIndex); } UpdateNavButtons(); }
        }
        #endregion

        #region DragDrop & Select
        private void Form1_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] paths = e.Data.GetData(DataFormats.FileDrop) as string[];
                if (paths == null || paths.Length == 0) return;
                StringBuilder sb = new StringBuilder();
                foreach (var p in paths) sb.AppendLine(p);
                txtFilePath.Text = sb.ToString().Trim();
                e.Effect = DragDropEffects.Link;
                RebuildFileList();
            }
        }

        private void Form1_DragEnter(object sender, DragEventArgs e)
        { e.Effect = e.Data.GetDataPresent(DataFormats.FileDrop) ? DragDropEffects.Link : DragDropEffects.None; }

        private void btnSelectFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "GIF文件|*.gif|所有文件|*.*";
            ofd.RestoreDirectory = true;
            ofd.Multiselect = true;
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                StringBuilder sb = new StringBuilder();
                foreach (var f in ofd.FileNames) sb.AppendLine(f);
                txtFilePath.Text = sb.ToString().Trim();
                RebuildFileList();
            }
        }

        private void btnClearTxt_Click(object sender, EventArgs e)
        {
            txtFilePath.Clear(); dgvResults.Rows.Clear();
            fileList.Clear(); leftIndex = -1; rightIndex = -1;
            UpdateNavButtons(); ClearPreview(); lblStatus.Text = "就绪";
        }

        private void btnBrowseDir_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            if (fbd.ShowDialog() == DialogResult.OK) { txtCustomDir.Text = fbd.SelectedPath; if (!radCustomDir.Checked) radCustomDir.Checked = true; }
        }

        private void radCustomName_CheckedChanged(object sender, EventArgs e) 
        { 
            txtCustomName.Enabled = radCustomName.Checked; 
            radReplaceName.Visible = radCustomName.Checked;
            radAppendName.Visible = radCustomName.Checked;
        }
        private void radCustomDir_CheckedChanged(object sender, EventArgs e) { txtCustomDir.Enabled = radCustomDir.Checked; }
        #endregion

        #region Build Command
        private CompressionSettings CaptureSettings()
        {
            return new CompressionSettings
            {
                OptLevel = cmbOptLevel.SelectedItem != null ? cmbOptLevel.SelectedItem.ToString() : "O3",
                Scale = (int)numScale.Value, Lossy = (int)numLossy.Value, Colors = (int)numColors.Value,
                NoComments = chkNoComments.Checked, NoExtensions = chkNoExtensions.Checked,
                Interlace = chkInterlace.Checked, LoopCount = (int)numLoopCount.Value,
                Delay = (int)numDelay.Value,
                Dispose = cmbDispose.SelectedItem != null ? cmbDispose.SelectedItem.ToString() : "不设置",
                SaveToOriginalDir = radOriginalDir.Checked, SaveToDesktop = radDesktop.Checked,
                SaveToCustomDir = radCustomDir.Checked, CustomDir = txtCustomDir.Text.Trim(),
                UseOrigName = radOrigName.Checked, UseTimeStamp = radTimeStamp.Checked,
                UseCustomName = radCustomName.Checked, CustomName = txtCustomName.Text.Trim(),
                ReplaceName = radReplaceName.Checked
            };
        }

        private string BuildOutputPath(string filePath, CompressionSettings s)
        {
            string dir = s.SaveToDesktop ? Environment.GetFolderPath(Environment.SpecialFolder.Desktop)
                        : s.SaveToCustomDir ? s.CustomDir : Path.GetDirectoryName(filePath);
            string name = Path.GetFileNameWithoutExtension(filePath);
            if (s.UseTimeStamp) name += "_" + DateTime.Now.ToString("yyyyMMddHHmmssfff");
            else if (s.UseCustomName && !string.IsNullOrEmpty(s.CustomName))
            {
                if (s.ReplaceName) name = s.CustomName;
                else name = name + s.CustomName;
            }
            return Path.Combine(dir, name + ".gif");
        }

        private string BuildArgs(string filePath, string outputPath, CompressionSettings s)
        {
            StringBuilder a = new StringBuilder();
            a.Append("-" + s.OptLevel);
            if (s.Scale != 100) a.AppendFormat(" --scale {0:0.##}", (double)s.Scale / 100.0);
            if (s.Lossy > 0) a.AppendFormat(" --lossy={0}", s.Lossy);
            if (s.Colors < 256 && s.Colors >= 2) a.AppendFormat(" --colors={0}", s.Colors);
            if (s.NoComments) a.Append(" --no-comments");
            if (s.NoExtensions) a.Append(" --no-extensions");
            if (s.Interlace) a.Append(" --interlace");
            if (s.LoopCount > 0) a.AppendFormat(" --loopcount={0}", s.LoopCount);
            if (s.Delay > 0) a.AppendFormat(" --delay={0}", s.Delay);
            if (s.Dispose == "none") a.Append(" --dispose=none");
            else if (s.Dispose == "background") a.Append(" --dispose=background");
            else if (s.Dispose == "previous") a.Append(" --dispose=previous");
            a.AppendFormat(" {0} -o {1}", Quote(filePath), Quote(outputPath));
            return a.ToString();
        }

        private string Quote(string s) { return s.IndexOf(' ') >= 0 ? "\"" + s + "\"" : s; }
        #endregion

        private void btnCompress_Click(object sender, EventArgs e)
        {
            RebuildFileList();
            if (fileList.Count == 0) { MessageBox.Show("请先选择GIF文件！"); return; }
            var s = CaptureSettings();
            if (s.SaveToCustomDir && string.IsNullOrEmpty(s.CustomDir)) { MessageBox.Show("请选择自选保存目录！"); return; }
            previewDebounceTimer.Stop();
            btnCompress.Enabled = false; btnCompress.Text = "压缩中...";
            dgvResults.Rows.Clear(); lblStatus.Text = "正在压缩...";
            bgWorker.RunWorkerAsync(new object[] { fileList.ToArray(), s });
        }

        private void BgWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            object[] args = (object[])e.Argument;
            string[] files = (string[])args[0];
            CompressionSettings s = (CompressionSettings)args[1];
            int total = files.Length;
            for (int i = 0; i < total; i++)
            {
                string filePath = files[i];
                if (string.IsNullOrEmpty(filePath)) continue;
                string fileName = Path.GetFileName(filePath);
                string outputPath = BuildOutputPath(filePath, s);
                long origSize = 0;
                try { if (File.Exists(filePath)) origSize = new FileInfo(filePath).Length; } catch { }
                bool success = false; string error = "";
                try
                {
                    ProcessStartInfo psi = new ProcessStartInfo();
                    psi.FileName = "gifsicle.exe";
                    psi.Arguments = BuildArgs(filePath, outputPath, s);
                    psi.UseShellExecute = false; psi.RedirectStandardError = true; psi.CreateNoWindow = true;
                    using (Process proc = Process.Start(psi)) { string stderr = proc.StandardError.ReadToEnd(); proc.WaitForExit(); if (proc.ExitCode != 0) error = stderr.Trim(); else success = true; }
                }
                catch (Exception ex) { error = ex.Message; }
                long newSize = 0;
                try { if (success && File.Exists(outputPath)) newSize = new FileInfo(outputPath).Length; } catch { }
                string origSizeStr = FormatFileSize(origSize);
                string newSizeStr = success ? FormatFileSize(newSize) : "-";
                string ratio = ""; if (success && origSize > 0) { double r = (1.0 - (double)newSize / origSize) * 100.0; ratio = r.ToString("0.0") + "%"; }
                string status = success ? "完成" : (!string.IsNullOrEmpty(error) ? "失败: " + error.Substring(0, Math.Min(error.Length, 60)) : "失败");
                bgWorker.ReportProgress((int)((i + 1) * 100.0 / total),
                    new ProgressData { FileName = fileName, OrigSizeStr = origSizeStr, NewSizeStr = newSizeStr, Ratio = ratio, Status = status, OrigImagePath = filePath, NewImagePath = success ? outputPath : null });
            }
        }

        private void BgWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            ProgressData pd = (ProgressData)e.UserState;
            dgvResults.Rows.Add(pd.FileName, pd.OrigSizeStr, pd.NewSizeStr, pd.Ratio, pd.Status);
            dgvResults.FirstDisplayedScrollingRowIndex = dgvResults.Rows.Count - 1;
            lblStatus.Text = string.Format("压缩进度: {0}%", e.ProgressPercentage);
            try
            {
                SetOrigImage(pd.OrigImagePath); currentOrigPath = pd.OrigImagePath; ShowOrigAnimated();
                lblOrigInfo.Text = TruncateFileName(pd.FileName) + "  |  " + pd.OrigSizeStr;
                SetNewImage(pd.NewImagePath); currentNewPath = pd.NewImagePath; ShowNewAnimated();
                if (pd.NewImagePath != null) lblNewInfo.Text = TruncateFileName(Path.GetFileName(pd.NewImagePath)) + "  |  " + pd.NewSizeStr + "  |  节省 " + pd.Ratio;
                else lblNewInfo.Text = "压缩失败";
            }
            catch { }
        }

        private void BgWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            btnCompress.Enabled = true; btnCompress.Text = "开始压缩";
            int total = dgvResults.Rows.Count, success = 0;
            foreach (DataGridViewRow row in dgvResults.Rows)
                if (row.Cells["colStatus"].Value != null && row.Cells["colStatus"].Value.ToString() == "完成") success++;
            lblStatus.Text = string.Format("压缩完成！共 {0} 个文件，成功 {1} 个", total, success);
            if (leftIndex >= 0 && leftIndex < fileList.Count) ShowFilePreview(leftIndex);
        }

        private string FormatFileSize(long bytes)
        {
            if (bytes < 1024) return bytes + " B";
            if (bytes < 1048576) return (bytes / 1024.0).ToString("0.0") + " KB";
            return (bytes / 1048576.0).ToString("0.0") + " MB";
        }

        /// <summary>
        /// 截断过长的文件名，保留前8个字符和后6个字符，中间用…代替
        /// </summary>
        private string TruncateFileName(string name, int head = 8, int tail = 6)
        {
            if (string.IsNullOrEmpty(name)) return name;
            if (name.Length <= head + tail + 1) return name;
            return name.Substring(0, head) + "…" + name.Substring(name.Length - tail);
        }

        #region Zoom Preview
        private void picOriginal_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(currentOrigPath) || !File.Exists(currentOrigPath)) return;
            ShowZoomForm(currentOrigPath, "原图预览");
        }

        private void picCompressed_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(currentNewPath) || !File.Exists(currentNewPath)) return;
            ShowZoomForm(currentNewPath, "压缩后预览");
        }

        private void ShowZoomForm(string imagePath, string title)
        {
            if (string.IsNullOrEmpty(imagePath) || !File.Exists(imagePath)) return;

            Image img = null;
            MemoryStream ms = null;
            try
            {
                byte[] data = File.ReadAllBytes(imagePath);
                ms = new MemoryStream(data);
                img = Image.FromStream(ms);
            }
            catch { return; }

            FrameDimension dim = null;
            int frameCount = 0;
            if (img.FrameDimensionsList.Length > 0)
            {
                dim = new FrameDimension(img.FrameDimensionsList[0]);
                frameCount = img.GetFrameCount(dim);
            }

            Form zf = new Form();
            zf.Text = title;
            zf.StartPosition = FormStartPosition.CenterScreen;
            zf.WindowState = FormWindowState.Maximized;
            zf.BackColor = Color.FromArgb(30, 30, 30);
            zf.KeyPreview = true;

            float zoom = 1.0f;
            bool dragging = false;
            Point dragStart = Point.Empty;
            Point picStart = Point.Empty;

            Panel scrollPanel = new Panel();
            scrollPanel.Dock = DockStyle.Fill;
            scrollPanel.AutoScroll = true;
            scrollPanel.BackColor = Color.FromArgb(30, 30, 30);

            PictureBox pb = new PictureBox();
            pb.SizeMode = PictureBoxSizeMode.AutoSize;
            pb.BackColor = Color.FromArgb(30, 30, 30);
            pb.Cursor = Cursors.Hand;
            scrollPanel.Controls.Add(pb);

            Label lblHint = new Label();
            lblHint.Text = "滚轮缩放 | 左键拖动 | 右键复位 | ESC关闭";
            lblHint.Dock = DockStyle.Bottom;
            lblHint.ForeColor = Color.White;
            lblHint.BackColor = Color.FromArgb(50, 50, 50);
            lblHint.TextAlign = ContentAlignment.MiddleCenter;
            lblHint.Height = 30;
            lblHint.Font = new Font("Microsoft YaHei", 9F);

            zf.Controls.Add(scrollPanel);
            zf.Controls.Add(lblHint);

            Bitmap currentBmp = null;
            int origW = img.Width;
            int origH = img.Height;
            int frame = 0;

            Action updateZoom = () =>
            {
                if (currentBmp != null) { currentBmp.Dispose(); currentBmp = null; }
                int w = Math.Max(1, (int)(origW * zoom));
                int h = Math.Max(1, (int)(origH * zoom));
                currentBmp = new Bitmap(w, h);
                using (Graphics g = Graphics.FromImage(currentBmp))
                {
                    g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                    g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
                    g.DrawImage(img, 0, 0, w, h);
                }
                pb.Image = currentBmp;
                if (w <= scrollPanel.ClientSize.Width && h <= scrollPanel.ClientSize.Height)
                {
                    pb.Location = new Point((scrollPanel.ClientSize.Width - w) / 2, (scrollPanel.ClientSize.Height - h) / 2);
                }
                else
                {
                    pb.Location = new Point(0, 0);
                }
            };

            System.Windows.Forms.Timer zoomAnimTimer = new System.Windows.Forms.Timer();
            zoomAnimTimer.Interval = 80;
            zoomAnimTimer.Tick += (s, ev) =>
            {
                if (frameCount > 1)
                {
                    frame = (frame + 1) % frameCount;
                    img.SelectActiveFrame(dim, frame);
                    updateZoom();
                }
            };
            if (frameCount > 1) zoomAnimTimer.Start();

            pb.MouseDown += (s, ev) =>
            {
                if (ev.Button == MouseButtons.Left)
                {
                    dragging = true;
                    dragStart = Control.MousePosition;
                    picStart = pb.Location;
                    pb.Cursor = Cursors.SizeAll;
                }
                else if (ev.Button == MouseButtons.Right)
                {
                    zoom = 1.0f;
                    updateZoom();
                }
            };

            pb.MouseMove += (s, ev) =>
            {
                if (dragging)
                {
                    Point now = Control.MousePosition;
                    pb.Location = new Point(picStart.X + now.X - dragStart.X, picStart.Y + now.Y - dragStart.Y);
                }
            };

            pb.MouseUp += (s, ev) => { dragging = false; pb.Cursor = Cursors.Hand; };

            pb.DoubleClick += (s, ev) => zf.Close();

            scrollPanel.MouseWheel += (s, ev) =>
            {
                if (ev.Delta > 0) zoom = Math.Min(zoom * 1.15f, 20.0f);
                else zoom = Math.Max(zoom / 1.15f, 0.05f);
                updateZoom();
                ((HandledMouseEventArgs)ev).Handled = true;
            };
            pb.MouseWheel += (s, ev) =>
            {
                if (ev.Delta > 0) zoom = Math.Min(zoom * 1.15f, 20.0f);
                else zoom = Math.Max(zoom / 1.15f, 0.05f);
                updateZoom();
                ((HandledMouseEventArgs)ev).Handled = true;
            };

            zf.KeyDown += (s, ev) => { if (ev.KeyCode == Keys.Escape) zf.Close(); };

            zf.Shown += (s, ev) =>
            {
                int maxW = scrollPanel.ClientSize.Width;
                int maxH = scrollPanel.ClientSize.Height;
                if (origW > maxW || origH > maxH)
                {
                    float ratioW = (float)maxW / origW;
                    float ratioH = (float)maxH / origH;
                    zoom = Math.Min(ratioW, ratioH) * 0.9f;
                    if (zoom < 0.05f) zoom = 0.05f;
                }
                updateZoom();
            };
            zf.Resize += (s, ev) => updateZoom();

            zf.FormClosing += (s, ev) =>
            {
                zoomAnimTimer.Stop();
                if (currentBmp != null) { currentBmp.Dispose(); currentBmp = null; }
                if (img != null) { img.Dispose(); img = null; }
                if (ms != null) { ms.Dispose(); ms = null; }
            };

            zf.Show(this);
        }
        #endregion
    }
}
