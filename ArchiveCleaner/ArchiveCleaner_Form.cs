/* Copyright (c) 2021 Sven Mathing
 * Licensed under the EUPL, Version 1.2 or as soon they will be approved by
 * the European Commission - subsequent versions of the EUPL (the "Licence"); You may not use this work except
 * in compliance with the Licence. You may obtain a copy of the Licence at:
 * http://joinup.ec.europa.eu/software/page/eupl Unless required by applicable law or agreed to in writing,
 * software distributed under the Licence is distributed on an "AS IS" basis, WITHOUT WARRANTIES OR CONDITIONS
 * OF ANY KIND, either express or implied. See the Licence for the specific language governing permissions and
 * limitations under the Licence.
 */
using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ArchiveCleaner
{
    public partial class ArchiveCleaner_Form : Form
    {
        public ArchiveCleaner_Form()
        {
            InitializeComponent();

            //default timespan is 10 years back
            DateTextBox.Text = new DateTime((DateTime.Today.Year) - 10, 1, 1).ToShortDateString();
        }

        /// <summary>
        /// closes the program
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CloseProgramButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// helps selecting a valid path by showing a folder browsing dialog
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SelectPathButton_Click(object sender, EventArgs e)
        {
            
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            DialogResult result = dialog.ShowDialog();
            if (result.Equals(DialogResult.OK))
            {
                //MessageBox.Show("verzeichnis:" + dialog.FileName);
                PathTextBox.Text = dialog.SelectedPath;
            }
        }

        /// <summary>
        /// starts the search for files which fullfill the search criteria
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SearchButton_Click(object sender, EventArgs e)
        {
            if (PathTextBox.Text.Length > 0 && DateTextBox.Text.Length > 0)
            {
                //date and path are filled

                //recursive search for files 
                string[] allFiles = Directory.GetFiles(PathTextBox.Text, "*.*", System.IO.SearchOption.AllDirectories);
                //MessageBox.Show("There are " + allFiles.Length + " files..."+ allFiles[0]+ " " + File.GetCreationTime(allFiles[0]));

                //now go through the list and find the one older than the date
                List<string> filesToDelete = new List<string>();
                DateTime fileCreationDate;
                DateTime searchDate = DateTime.Parse(DateTextBox.Text);
                FilesListBox.Items.Clear();
                foreach (string file in allFiles)
                {
                    //get Fileproperties
                    fileCreationDate = File.GetCreationTime(file);
                    if (fileCreationDate < searchDate)
                    {
                        filesToDelete.Add(file);
                        FilesListBox.Items.Add(file);
                    }
                }
                MessageBox.Show("Es gibt " + filesToDelete.Count + " Dateien, die den Suchparametern entsprechen!");
            }
            else
            {
                MessageBox.Show("Bitte zuerst ein Verzeichnis und ein Datum wählen!");
                return;
            }
        }

        /// <summary>
        /// deletes all files in the Listbox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DeleteAllButton_Click(object sender, EventArgs e)
        {
            List<string> filesToDelete;
            filesToDelete = FilesListBox.Items.OfType<string>().ToList();
            int filecount = FilesListBox.Items.Count;
            if (filesToDelete.Count > 0)
            {
                if (MessageBox.Show("Wollen Sie wirklich "+filecount+" Datei(en) und Verzeichnisse löschen?", "Warnung", MessageBoxButtons.YesNo, MessageBoxIcon.Stop).Equals(DialogResult.Yes))
                {
                    //YES...delete the files
                    if (MessageBox.Show("Wollen Sie wirklich löschen? " + Environment.NewLine
                        + "Nach 'Ja' werden ALLE DIESE DATEIEN GELÖSCHT UND KÖNNEN NUR ÜBER DEN PAPIERKORB WIEDERHERGESTELLT WERDEN!", "Warnung", MessageBoxButtons.YesNo, MessageBoxIcon.Stop).Equals(DialogResult.Yes))
                    {
                        foreach (string file in filesToDelete)
                        {
                            DeleteFileAndFolder(file);
                        }
                    }
                }
                else
                {
                    //Do not delete the files...
                    MessageBox.Show("Dateien wurden NICHT gelöscht!", "Entwarnung", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    return;
                }
            }
        }

        /// <summary>
        /// deletes all selected files from the listbox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DeleteSelectedButton_Click(object sender, EventArgs e)
        {
            List<string> filesToDelete;
            filesToDelete = FilesListBox.SelectedItems.OfType<string>().ToList();

            //check if there are some selected files
            int filecount = FilesListBox.SelectedItems.Count;
            if (filecount > 0)
            {
                if (MessageBox.Show("Wollen Sie wirklich " + filecount + " Datei(en) und zugehörige leere Verzeichnisse löschen?", "Warnung", MessageBoxButtons.YesNo, MessageBoxIcon.Stop).Equals(DialogResult.Yes))
                {
                    //YES...delete the files
                    if (MessageBox.Show("Wollen Sie wirklich löschen? " + Environment.NewLine
                        + "Nach 'Ja' werden ALLE DIESE DATEIEN GELÖSCHT UND KÖNNEN NUR ÜBER DEN PAPIERKORB WIEDERHERGESTELLT WERDEN!", "Warnung", MessageBoxButtons.YesNo, MessageBoxIcon.Stop).Equals(DialogResult.Yes))
                    {
                        foreach (string file in filesToDelete)
                        {
                            DeleteFileAndFolder(file);
                        }
                    }
                }
                else
                {
                    //Do not delete the files...
                    MessageBox.Show("Dateien wurden NICHT gelöscht!", "Entwarnung", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    return;
                }
            }
        }

        /// <summary>
        /// deletes one file and maybe its folder (if it is empty)
        /// The files/folder are deleted using the trash bin for recovery
        /// </summary>
        /// <param name="filename">the file with complete path to delete</param>
        private void DeleteFileAndFolder(string filename)
        {            
            string folder = System.IO.Path.GetDirectoryName(filename);

            //try to delete the file and then the folder
            try
            {
                Console.Write("Lösche Datei " + filename+ " ... ");
                //deleting without trash bin
                //File.Delete(filename);    

                //deleting with trash bin
                FileSystem.DeleteFile(filename, UIOption.AllDialogs, RecycleOption.SendToRecycleBin);

                Console.WriteLine("fertig!");
                //lösche EIntrag aus der Listbox
                FilesListBox.Items.Remove(filename);
            }catch (Exception ex)
            {
                //error deleting the file
                Console.WriteLine("Datei konnte nicht gelöscht werden ["+ex.Message+"]");
            }
            
            //delete the folder if it is empty
            Console.Write(String.Format("Verzeichnis {0} ", folder));
            if (IsEmpty(folder))
            {
                try {
                    Console.Write("ist leer und wird gelöscht... ");
                    //deleting without trash bin
                    //Directory.Delete(folder); 

                    //deleting with trash bin
                    FileSystem.DeleteDirectory(folder, UIOption.AllDialogs, RecycleOption.SendToRecycleBin);

                    Console.WriteLine("fertig!");
                }
                catch (Exception ex)
                {
                    //error deleting the file
                    Console.WriteLine("Verzeichnis konnte nicht gelöscht werden [" + ex.Message + "]");
                }
            }
            else
            {
                Console.WriteLine("ist noch nicht leer und wird nicht gelöscht!");
            }
        }

        /// <summary>
        /// checks if an folder is empty
        /// </summary>
        /// <param name="folderPath">the folder to check</param>
        /// <returns></returns>
        private static bool IsEmpty(string folderPath)
        {
            bool allSubFoldersEmpty = true;
            //check every subfolder
            foreach (var subFolder in Directory.EnumerateDirectories(folderPath))
            {
                if (IsEmpty(subFolder))
                {
                    //Console.Write($"[Empty Subfolder: {subFolder}] ");
                }
                else
                {
                    allSubFoldersEmpty = false;
                }
            }

            if (allSubFoldersEmpty && !HasFiles(folderPath))
            {
                return true;
            }
            return false;

        }

        /// <summary>
        /// checks if in a folder are some files
        /// </summary>
        /// <param name="folderPath">the folder to chekc</param>
        /// <returns>true if files are found</returns>
        private static bool HasFiles(string folderPath)
        {
            return Directory.EnumerateFiles(folderPath).Any();
        }

        /// <summary>
        /// opens the link for the linklabel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            // Specify that the link was visited.
            this.linkLabel1.LinkVisited = true;

            // Navigate to a URL.
            System.Diagnostics.Process.Start("https://joinup.ec.europa.eu/collection/eupl/eupl-text-eupl-12");
        }

        /// <summary>
        /// opens the link for the linklabel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            // Specify that the link was visited.
            this.linkLabel1.LinkVisited = true;

            // Navigate to a URL.
            System.Diagnostics.Process.Start("https://github.com/SvenMathing/ArchiveCleaner");
        }
    }
}
