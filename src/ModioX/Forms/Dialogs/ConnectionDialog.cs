﻿using System;
using System.Windows.Forms;
using DarkUI.Controls;
using DarkUI.Forms;
using ModioX.Extensions;
using ModioX.Forms.Windows;
using ModioX.Models.Resources;

namespace ModioX.Forms.Dialogs
{
    public partial class ConnectionDialog : DarkForm
    {
        public ConnectionDialog()
        {
            InitializeComponent();
        }

        public ConsoleProfile ConsoleProfile { get; private set; }

        private void ConnectConsole_Load(object sender, EventArgs e)
        {
            LoadConsoles();
        }

        private void LoadConsoles()
        {
            ListViewConsoleProfiles.Items.Clear();

            foreach (var console in MainWindow.Settings.ConsoleProfiles)
            {
                ListViewConsoleProfiles.Items.Add(new DarkListItem(console.ToString()));
            }

            _ = ListViewConsoleProfiles.Focus();
        }

        private void ListViewConsoleProfiles_SelectedIndicesChanged(object sender, EventArgs e)
        {
            ButtonEdit.Enabled = ListViewConsoleProfiles.SelectedIndices.Count > 0;
            ButtonDelete.Enabled = ListViewConsoleProfiles.SelectedIndices.Count > 0;
            ButtonConnect.Enabled = ListViewConsoleProfiles.SelectedIndices.Count > 0;

            if (ListViewConsoleProfiles.SelectedIndices.Count > 0)
            {
                _ = ButtonConnect.Focus();
            }
        }

        private void ButtonConnect_Click(object sender, EventArgs e)
        {
            var selectedItem = ListViewConsoleProfiles.Items[ListViewConsoleProfiles.SelectedIndices[0]];
            ConsoleProfile = MainWindow.Settings.ConsoleProfiles[ListViewConsoleProfiles.Items.IndexOf(selectedItem)];
            Close();
        }

        private void ButtonDelete_Click(object sender, EventArgs e)
        {
            if (MainWindow.Settings.ConsoleProfiles.Count == 1)
            {
                _ = DarkMessageBox.Show(this, "You can't delete this because there must be at least one console.",
                    "Can't Delete Console", MessageBoxIcon.Warning);
            }
            else
            {
                if (DarkMessageBox.Show(this, "Do you really want to delete the selected item?", "Delete Console",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes)
                {
                    var selectedItem = ListViewConsoleProfiles.Items[ListViewConsoleProfiles.SelectedIndices[0]];
                    MainWindow.Settings.ConsoleProfiles.RemoveAt(
                        ListViewConsoleProfiles.Items.IndexOf(selectedItem));
                    LoadConsoles();
                }
            }
        }

        private void ButtonEdit_Click(object sender, EventArgs e)
        {
            var selectedItem = ListViewConsoleProfiles.Items[ListViewConsoleProfiles.SelectedIndices[0]];
            var selectedIndex = ListViewConsoleProfiles.Items.IndexOf(selectedItem);
            var oldConsoleProfile = MainWindow.Settings.ConsoleProfiles[selectedIndex];

            var newConsoleProfile = DialogExtensions.ShowNewConnectionWindow(this, oldConsoleProfile, true);

            if (newConsoleProfile != null)
            {
                oldConsoleProfile = newConsoleProfile;
            }

            LoadConsoles();
        }
    }
}