﻿using DarkUI.Forms;
using ModioX.Models.Resources;
using System;
using System.IO;
using System.Windows.Forms;

namespace ModioX.Windows
{
    public partial class EditBackupForm : DarkForm
    {
        public BackupFile BackupFile { get; set; } = new BackupFile();

        public EditBackupForm()
        {
            InitializeComponent();
        }

        private void EditBackupForm_Load(object sender, EventArgs e)
        {
            TextBoxName.Text = BackupFile.Name;
            TextBoxFileName.Text = BackupFile.FileName;
            TextBoxGameId.Text = BackupFile.CategoryId;
            TextBoxLocalPath.Text = BackupFile.LocalPath;
            TextBoxConsolePath.Text = BackupFile.InstallPath;
        }

        private void TextBoxConsolePath_TextChanged(object sender, EventArgs e)
        {
            TextBoxFileName.Text = Path.GetFileName(TextBoxConsolePath.Text);
        }

        private void ButtonBackupSave_Click(object sender, EventArgs e)
        {
            if (BackupFile == CreateBackup())
            {
                return;
            }

            if (string.IsNullOrWhiteSpace(TextBoxName.Text))
            {
                DarkMessageBox.Show(this, "You must include a name for the game file backup..", "Empty Name", MessageBoxIcon.Exclamation);
                return;
            }

            if (string.IsNullOrWhiteSpace(TextBoxLocalPath.Text) || TextBoxLocalPath.Text.IndexOfAny(Path.GetInvalidPathChars()) != -1)
            {
                DarkMessageBox.Show(this, "You must include a local file path for the game file backup.", "Empty Local Path", MessageBoxIcon.Exclamation);
                return;
            }

            if (string.IsNullOrWhiteSpace(TextBoxConsolePath.Text))
            {
                DarkMessageBox.Show(this, "You must include a console path for for the game file backup. This is where the file will be restored at on the console.", "Empty Console Path", MessageBoxIcon.Exclamation);
                return;
            }

            MainForm.SettingsData.BackupFiles.Remove(BackupFile);
            MainForm.SettingsData.BackupFiles.Add(CreateBackup());

            Close();
        }

        private BackupFile CreateBackup()
        {
            BackupFile backupFile = new BackupFile()
            {
                Name = TextBoxName.Text,
                CategoryId = TextBoxGameId.Text,
                FileName = Path.GetFileName(TextBoxConsolePath.Text),
                LocalPath = TextBoxLocalPath.Text,
                InstallPath = TextBoxConsolePath.Text,
            };

            return backupFile;
        }

        private void ButtonLocalPath_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog() { CheckFileExists = true, Multiselect = false })
            {
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    TextBoxLocalPath.Text = openFileDialog.FileName;
                }
            }
        }
    }
}