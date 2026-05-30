using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace CopyWallpapers
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        //private void button1_Click(object sender, EventArgs e)
        //{

        //}
        //private void Download_Click(object sender, EventArgs e)
        //{

        //}


        private void btnOpenTargetPath_Click(object sender, EventArgs e)
        {
            try
            {
                using (var fbd = new FolderBrowserDialog())
                {
                    fbd.Description = "Zielordner für Wallpaper auswählen";
                    fbd.RootFolder = Environment.SpecialFolder.MyComputer;
                    fbd.ShowNewFolderButton = true;

                    DialogResult dr = fbd.ShowDialog();
                    if (dr == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                    {
                        txtTargetPath.Text = fbd.SelectedPath;
                    }
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show("Fehler: " + ex.Message, "Fehler");
            }
        }

        
        private async void btnDownload_Click(object sender, EventArgs e)
        {
            string ablage = txtTargetPath.Text;
            if (string.IsNullOrWhiteSpace(ablage) || !Directory.Exists(ablage))
            {
                MessageBox.Show("Bitte wählen Sie einen gültigen Zielordner.", "Info");
                return;
            }

            string temp = Path.Combine(ablage, "temp_SAB_" + Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(temp);

            btnDownload.Enabled = false;
            btnOpenTargetPath.Enabled = false;
            progressBar.Value = 0;
            lblStatus.Text = "Starte Kopiervorgang...";

            try
            {
                string item = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Packages", "Microsoft.Windows.ContentDeliveryManager_cw5n1h2txyewy", "LocalState", "Assets");
                var files = new DirectoryInfo(item).GetFiles().Where(f => f.Length > 2048).ToArray();

                if (files.Length == 0)
                {
                    MessageBox.Show("Keine Kandidaten gefunden.", "Info");
                    return;
                }

                int total = files.Length;
                int processed = 0;
                int copied = 0;

                var progress = new Progress<int>(percent =>
                {
                    try
                    {
                        progressBar.Value = percent;
                        lblStatus.Text = $"Verarbeitet: {percent}% ({processed}/{total})";
                    }
                    catch { }
                });

                await Task.Run(() =>
                {
                    foreach (var file in files)
                    {
                        processed++;

                        string targetFile = Path.Combine(ablage, Path.ChangeExtension(file.Name, "jpg"));
                        if (File.Exists(targetFile))
                        {
                            // already exists
                            int p = (int)(processed * 100.0 / total);
                            ((IProgress<int>)progress).Report(p);
                            continue;
                        }

                        // Load image into memory to avoid locking the source file
                        try
                        {
                            byte[] data = File.ReadAllBytes(file.FullName);
                            using (var ms = new System.IO.MemoryStream(data))
                            using (var img = System.Drawing.Image.FromStream(ms))
                            {
                                if (img.Width >= 1520 && img.Height >= 980)
                                {
                                    string tempTarget = Path.Combine(temp, file.Name);
                                    File.WriteAllBytes(tempTarget, data);
                                    copied++;
                                }
                            }
                        }
                        catch
                        {
                            // ignore invalid image files
                        }

                        int percent = (int)(processed * 100.0 / total);
                        ((IProgress<int>)progress).Report(percent);
                    }
                });

                // move and rename
                foreach (var f in new DirectoryInfo(temp).GetFiles())
                {
                    string newName = Path.ChangeExtension(f.Name, "jpg");
                    string dest = Path.Combine(ablage, newName);
                    if (File.Exists(dest)) File.Delete(dest);
                    f.MoveTo(dest);
                }

                lblStatus.Text = "Fertig.";
                progressBar.Value = 100;

                MessageBox.Show($"Kopiervorgang abgeschlossen. {copied} neue Bilder kopiert.", "Erfolg");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Fehler: " + ex.Message, "Fehler");
            }
            finally
            {
                try
                {
                    if (Directory.Exists(temp)) Directory.Delete(temp, true);
                }
                catch { }

                btnDownload.Enabled = true;
                btnOpenTargetPath.Enabled = true;
            }
        }
    }
}


