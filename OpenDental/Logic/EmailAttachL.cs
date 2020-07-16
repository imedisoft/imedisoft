using OpenDentBusiness;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace OpenDental
{
    public class EmailAttachL
	{
		/// <summary>
		/// Allow the user to pick the files to be attached. The 'pat' argument can be null.
		/// If the user cancels at any step, the return value will be an empty list.
		/// </summary>
		public static List<EmailAttach> PickAttachments(Patient patient)
		{
            var fileNames = new List<string>();
            var attachments = new List<EmailAttach>();

            using (var openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Multiselect = true;

                var initialDirectory = "";
                if (patient != null)
                {
                    initialDirectory = ImageStore.GetPatientFolder(patient, OpenDentBusiness.FileIO.FileAtoZ.GetPreferredAtoZpath());
                }

                openFileDialog.InitialDirectory = initialDirectory;
                if (openFileDialog.ShowDialog() != DialogResult.OK)
                {
                    return attachments;
                }

                fileNames.AddRange(openFileDialog.FileNames.ToList());
            }

            try
			{
				foreach (var fileName in fileNames)
				{
					attachments.Add(
                        EmailAttaches.CreateAttach(
                            Path.GetFileName(fileName), 
                            File.ReadAllBytes(fileName)));
				}
			}
			catch (Exception exception)
			{
				MessageBox.Show(exception.Message);
			}

			return attachments;
		}
	}
}
