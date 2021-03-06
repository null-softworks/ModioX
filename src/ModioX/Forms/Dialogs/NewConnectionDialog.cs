﻿using System;
using System.Net;
using System.Windows.Forms;
using DarkUI.Forms;
using ModioX.Forms.Windows;
using ModioX.Models.Resources;

namespace ModioX.Forms.Dialogs
{
    public partial class NewConnectionDialog : DarkForm
    {
        public NewConnectionDialog()
        {
            InitializeComponent();
        }

        public ConsoleProfile ConsoleProfile { get; set; } = new ConsoleProfile();

        public bool IsEditingProfile { get; set; } = false;

        private void ConsolesWindow_Load(object sender, EventArgs e)
        {
            TextBoxConnectionName.Text = ConsoleProfile.Name;
            TextBoxConsoleAddress.Text = ConsoleProfile.Address;
            TextBoxConsolePort.Text = ConsoleProfile.Port.ToString();

            LabelUserPass.Text = ConsoleProfile.UseDefaultCredentials
                ? "Default"
                : ConsoleProfile.Username + " / " + ConsoleProfile.Password;

            TextBoxConnectionName.SelectionStart = 0;
        }

        private static bool ProfileExists(string name)
        {
            foreach (var console in MainWindow.Settings.ConsoleProfiles)
            {
                if (console.Name.Equals(name))
                {
                    return true;
                }
            }

            return false;
        }

        private void ButtonChangeCredentials_Click(object sender, EventArgs e)
        {
            using (var consoleCredentials = new LoginDialog())
            {
                var setCredentials = consoleCredentials.ShowDialog(this);

                if (setCredentials == DialogResult.OK)
                {
                    LabelUserPass.Text = ConsoleProfile.Username + @" / " + ConsoleProfile.Password;

                    ConsoleProfile.Username = consoleCredentials.TextBoxUsername.Text;
                    ConsoleProfile.Password = consoleCredentials.TextBoxPassword.Text;
                    ConsoleProfile.UseDefaultCredentials = false;
                }
                else if (setCredentials == DialogResult.Abort)
                {
                    LabelUserPass.Text = @"Default";

                    ConsoleProfile.Username = "";
                    ConsoleProfile.Password = "";
                    ConsoleProfile.UseDefaultCredentials = true;
                }
            }
        }

        private void ButtonOK_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TextBoxConnectionName.Text))
            {
                _ = DarkMessageBox.Show(this, @"You must enter a connection name.", "Error", MessageBoxIcon.Error);
                return;
            }

            if (string.IsNullOrWhiteSpace(TextBoxConsoleAddress.Text))
            {
                _ = DarkMessageBox.Show(this, @"You must enter an IP Address.", "Error", MessageBoxIcon.Error);
                return;
            }

            if (string.IsNullOrWhiteSpace(TextBoxConsolePort.Text))
            {
                _ = DarkMessageBox.Show(this, @"You must enter a port value. The default value is 21.", "Error",
                    MessageBoxIcon.Error);
                return;
            }


            var isAddressValid = IPAddress.TryParse(TextBoxConsoleAddress.Text, out var address);
            var isPortValid = int.TryParse(TextBoxConsolePort.Text, out var port);

            if (isAddressValid)
            {
                if (isPortValid)
                {
                    if (IsEditingProfile)
                    {
                        ConsoleProfile.Name = TextBoxConnectionName.Text;
                        ConsoleProfile.Address = address.ToString();
                        ConsoleProfile.Port = port;
                        Close();
                    }
                    else
                    {
                        if (ProfileExists(TextBoxConnectionName.Text))
                        {
                            _ = DarkMessageBox.Show(this, @"A console with this connection name already exists.",
                                "Name Already Exists", MessageBoxIcon.Error);
                        }
                        else
                        {
                            ConsoleProfile.Name = TextBoxConnectionName.Text;
                            ConsoleProfile.Address = address.ToString();
                            ConsoleProfile.Port = port;
                            Close();
                        }
                    }
                }
                else
                {
                    _ = DarkMessageBox.Show(this, @"Port isn't an integer value.", "Invalid Port",
                        MessageBoxIcon.Error);
                }
            }
            else
            {
                _ = DarkMessageBox.Show(this, @"IP Address isn't the correct format.", "Invalid IP Address",
                    MessageBoxIcon.Error);
            }
        }

        private void ButtonCancel_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}