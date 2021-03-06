﻿using System;
using System.Windows.Forms;
using DarkUI.Forms;
using ModioX.Models.Database;
using ModioX.Templates;

namespace ModioX.Forms.Windows
{
    public partial class RequestModsWindow : DarkForm
    {
        public RequestModsWindow()
        {
            InitializeComponent();
        }

        private void RequestModsWindow_Load(object sender, EventArgs e)
        {
            foreach (var category in MainWindow.Database.Categories.Categories)
            {
                if (category.CategoryType != CategoryType.Favorite)
                {
                    _ = ComboBoxCategoryTitle.Items.Add(category.Title);
                }
            }
        }

        private void ComboBoxCategoryTitle_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ComboBoxCategoryTitle.SelectedIndex != -1)
            {
                var categoryTitle = ComboBoxCategoryTitle.GetItemText(ComboBoxCategoryTitle.SelectedItem);

                TextBoxGameRegions.Enabled = MainWindow.Database.Categories.GetCategoryByTitle(categoryTitle).CategoryType == CategoryType.Game;
            }
        }

        private void ButtonSubmitModsDetails_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TextBoxName.Text))
            {
                _ = DarkMessageBox.Show(this, "You have not included a mod name.", "Missing Fields",
                    MessageBoxIcon.Exclamation);
                return;
            }

            if (string.IsNullOrWhiteSpace(TextBoxModType.Text))
            {
                _ = DarkMessageBox.Show(this, "You have not included a mod type.", "Missing Fields",
                    MessageBoxIcon.Exclamation);
                return;
            }

            if (string.IsNullOrWhiteSpace(TextBoxAuthor.Text))
            {
                _ = DarkMessageBox.Show(this, "You have not included the author/creator for this mod.",
                    "Missing Fields", MessageBoxIcon.Exclamation);
                return;
            }

            if (string.IsNullOrWhiteSpace(TextBoxDescription.Text))
            {
                _ = DarkMessageBox.Show(this,
                    "You have not included a description. Please enter any information you know about the mods, such as features or important notes.",
                    "Missing Fields", MessageBoxIcon.Exclamation);
                return;
            }

            if (string.IsNullOrWhiteSpace(TextBoxLinks.Text))
            {
                _ = DarkMessageBox.Show(this,
                    "You have not included any links, this will help to find the mods so they can be added.",
                    "No Links", MessageBoxIcon.Exclamation);
                return;
            }

            _ = DarkMessageBox.Show(this,
                "You will be re-directed to the GitHub Issues tracking page for ModioX. All the information you have provided will be auto-filled for you. Create or login with your GitHub account and click the 'Submit' button to open the mod request. It will be added for you as soon as we're able to find it.",
                "Opening GitHub Issues", MessageBoxButtons.OK, MessageBoxIcon.Information);

            GitHubTemplates.OpenRequestTemplate(TextBoxName.Text, TextBoxModType.Text,
                ComboBoxCategoryTitle.GetItemText(ComboBoxCategoryTitle.SelectedItem), TextBoxAuthor.Text,
                TextBoxVersion.Text, TextBoxSystemType.Text, TextBoxDescription.Text, TextBoxLinks.Text);
            Close();
        }
    }
}