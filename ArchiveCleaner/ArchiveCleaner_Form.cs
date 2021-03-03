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
using System.IO;
using System.Linq;
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
                //clear the Listbox
                FilesListBox.Items.Clear();
                StatusLabel.Text = "";
                //search for files
                SearchFilesQueue();
            }
            else
            {
                MessageBox.Show("Bitte zuerst ein Verzeichnis und ein Datum wählen!");
                return;
            }
        }
        /// <summary>
        /// starts the search for files which fullfill the search criteria
        /// </summary>
        private void SearchFiles_oldway()
        {
            if (PathTextBox.Text.Length > 0 && DateTextBox.Text.Length > 0)
            {
                //date and path are filled
                Console.WriteLine("Timestep: " + DateTime.Now);
                //recursive search for files 
                string[] allFiles = Directory.GetFiles(PathTextBox.Text, "*.*", System.IO.SearchOption.AllDirectories);

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
                Console.WriteLine("Timestep: " + DateTime.Now);
                MessageBox.Show("Es gibt " + filesToDelete.Count + " Dateien, die den Suchparametern entsprechen!");
            }
            else
            {
                MessageBox.Show("Bitte zuerst ein Verzeichnis und ein Datum wählen!");
                return;
            }
        }


        /// <summary>
        /// starts the search for files which fullfill the search criteria
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SearchFiles()
        {
            if (PathTextBox.Text.Length > 0 && DateTextBox.Text.Length > 0)
            {
                DateTime searchDate = DateTime.Parse(DateTextBox.Text);
                //date and path are filled
                try
                {
                    Console.WriteLine("Timestep: " + DateTime.Now);
                    int fCount = Directory.EnumerateFiles(PathTextBox.Text, "*", System.IO.SearchOption.AllDirectories).Count();
                    Console.WriteLine("Timestep: " + DateTime.Now);
                    MessageBox.Show("Es müssen " + fCount + " Verzeichnisse durchsucht werden! Das kann lange dauern (vor allem bei Netzwerklaufwerken!");

                    //search for files 
                    Console.WriteLine("Timestep: " + DateTime.Now);
                    var allFiles = from file in
                        Directory.EnumerateFiles(@PathTextBox.Text, "*", System.IO.SearchOption.AllDirectories)
                                   where ((File.GetCreationTime(file)) < searchDate)
                                   select file;
                    foreach (var file in allFiles)
                    {
                        //Console.WriteLine("{0}", file);
                        FilesListBox.Items.Add(file);
                    }
                }
                catch (UnauthorizedAccessException UAEx)
                {
                    Console.WriteLine(UAEx.Message);
                }
                catch (PathTooLongException PathEx)
                {
                    Console.WriteLine(PathEx.Message);
                }

                Console.WriteLine("Timestep: " + DateTime.Now);

                MessageBox.Show(String.Format("Es gibt {0} Dateien, die den Suchparametern entsprechen.", FilesListBox.Items.Count));
            }
            else
            {
                MessageBox.Show("Bitte zuerst ein Verzeichnis und ein Datum wählen!");
                return;
            }
        }

        private void SearchFilesQueue()
        {
            if (PathTextBox.Text.Length > 0 && DateTextBox.Text.Length > 0)
            {
                StatusLabel.Text = "Zähle Dateien... Das kann sehr lange dauern...";
                StatusLabel.Update();

                //get the list of files. This can be run for a long! time..
                IEnumerable<string> files = EnumerateFiles(PathTextBox.Text);

                DateTime fileCreationDate;
                DateTime searchDate = DateTime.Parse(DateTextBox.Text);
                int counter = 0;
                int lastTick = 0;
                int amount = files.Count<string>();
                foreach (var file in files)
                {
                    counter++;
                    //Console.WriteLine(file);
                    //fileCreationDate = File.GetCreationTime(file);
                    FileSystemInfo fsi = new FileInfo(file);
                    fileCreationDate = fsi.CreationTime;
                    // System.IO.DirectoryInfo.EnumerateFiles(file) ;//.Crea;
                    if (fileCreationDate < searchDate)
                    {
                        if (FilesListBox.Items.Count == lastTick)
                        {
                            //Console.WriteLine(lastTick+" Dateien gefunden...");
                            StatusLabel.Text = counter + " / "+ amount + " Dateien durchsucht und "+ lastTick + " alte Dateien gefunden";
                            StatusLabel.Update();
                            FilesListBox.Update();
                            lastTick += 100;
                        }
                        
                        FilesListBox.Items.Add(file);
                    }
                }
                StatusLabel.Text = FilesListBox.Items.Count + " Dateien gefunden";
                StatusLabel.Update();
                MessageBox.Show("Es gibt " + FilesListBox.Items.Count + " Dateien, die den Suchparametern entsprechen!");
                StatusLabel1.Text = "";
                StatusLabel1.Update();

            }
        }

        internal IEnumerable<string> EnumeratePaths(string root)
        {
            if (root == null)
                throw new ArgumentNullException("root");
            if (!Directory.Exists(root))
                throw new ArgumentException("Invalid root path", "root");

            if (root.Length > 3)
                root = Path.GetDirectoryName(root + "\\");

            Queue<string> queue = new Queue<string>();
            queue.Enqueue(root);

            while (queue.Count > 0)
            {
                string curr = queue.Dequeue();
                bool failed = false;
                StatusLabel1.Text = "bearbeite Verzeichnis: " + curr;
                StatusLabel1.Update();
                try
                {
                    foreach (var path in Directory.GetDirectories(curr))
                    {
                        queue.Enqueue(path);
                    }
                }
                catch
                {
                    failed = true;
                }
                if (!failed)
                    yield return curr;
            }
        }

        internal IEnumerable<string> EnumerateFiles(string root)
        {
            var paths = EnumeratePaths(root);
            foreach (var nxt in paths)
            {
                foreach (var filename in Directory.GetFiles(nxt)) //EnumerateFiles(nxt))
                    yield return filename;
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
                if (MessageBox.Show("Wollen Sie wirklich " + filecount + " Datei(en) und Verzeichnisse löschen?", "Warnung", MessageBoxButtons.YesNo, MessageBoxIcon.Stop).Equals(DialogResult.Yes))
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
                Console.Write("Lösche Datei " + filename + " ... ");
                //deleting without trash bin
                //File.Delete(filename);    

                //deleting with trash bin
                FileSystem.DeleteFile(filename, UIOption.AllDialogs, RecycleOption.SendToRecycleBin);

                Console.WriteLine("fertig!");
                //lösche EIntrag aus der Listbox
                FilesListBox.Items.Remove(filename);
            }
            catch (Exception ex)
            {
                //error deleting the file
                Console.WriteLine("Datei konnte nicht gelöscht werden [" + ex.Message + "]");
            }

            //delete the folder if it is empty
            Console.Write(String.Format("Verzeichnis {0} ", folder));
            if (IsEmpty(folder))
            {
                try
                {
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
