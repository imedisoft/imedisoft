using CodeBase;
using Imedisoft.Data;
using Imedisoft.Data.Models;
using OpenDentBusiness;
using OpenDentBusiness.FileIO;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;

namespace OpenDentBusiness
{
    public class ImageStore
	{
		/// <summary>
		/// Throw exceptions. 
		/// Returns patient's AtoZ folder if local AtoZ used, blank if database is used, or Cloud AtoZ path if CloudStorage.IsCloudStorage. 
		/// Will validate that folder exists. Will create folder if needed. 
		/// It will set the pat.ImageFolder if pat.ImageFolder is blank.
		/// </summary>
		public static string GetPatientFolder(Patient patient, string AtoZpath)
		{
            var oldPatient = patient.Copy();

			// Creates new folder for patient if none present
			if (string.IsNullOrEmpty(patient.ImageFolder))
			{
				patient.ImageFolder = GetImageFolderName(patient);
			}

            string result = ODFileUtils.CombinePaths(AtoZpath,
				patient.ImageFolder.Substring(0, 1).ToUpper(),
				patient.ImageFolder);

            try
			{
				if (string.IsNullOrEmpty(AtoZpath))
				{
					// If AtoZpath parameter was null or empty string and DataStorageType is LocalAtoZ, don't create a directory since retVal would then be
					// considered a relative path. Example: If AtoZpath is null, retVal will be like "P\PatientAustin1" after ODFileUtils.CombinePaths.
					// CreateDirectory treats this as a relative path and the full path would be "C:\Program Files (x86)\Open Dental\P\PatientAustin1".
					throw new ApplicationException("AtoZpath was null or empty");
				}
				if (!Directory.Exists(result))
				{
					Directory.CreateDirectory(result);
				}
			}
			catch (Exception ex)
			{
				throw new ApplicationException("Error.  Could not create folder for patient: " + result, ex);
			}


			if (string.IsNullOrEmpty(oldPatient.ImageFolder))
			{
				Patients.Update(patient, oldPatient);
			}

			return result;
		}

		///<summary>Returns the name of the ImageFolder. Removes any non letter to the patient's name.</summary>
		public static string GetImageFolderName(Patient pat)
		{
			string name = pat.LName + pat.FName;
			string folder = "";
			for (int i = 0; i < name.Length; i++)
			{
				if (Char.IsLetter(name, i))
				{
					folder += name.Substring(i, 1);
				}
			}
			folder += pat.PatNum.ToString();//ensures unique name
			return folder;
		}

		private static string GetFolder(string folderName)
		{
			string result = ODFileUtils.CombinePaths(FileAtoZ.GetPreferredAtoZpath(), folderName);

			if (!Directory.Exists(result))
			{
				Directory.CreateDirectory(result);
			}

			return result;
		}

		/// <summary>
		/// Will create folder if needed. Will validate that folder exists.
		/// </summary>
		public static string GetEobFolder() 
			=> GetFolder("EOBs");

		/// <summary>
		/// Will create folder if needed. Will validate that folder exists.
		/// </summary>
		public static string GetAmdFolder() 
			=> GetFolder("Amendments");

		/// <summary>
		/// Gets the folder name where provider images are stored. Will create folder if needed.
		/// </summary>
		public static string GetProviderImagesFolder() 
			=> GetFolder("ProviderImages");

		/// <summary>
		/// Typically returns something similar to \\SERVER\OpenDentImages\EmailImages.
		/// This is the location of the email html template images.
		/// The images are stored in this central location in order to make them reusable on multiple email messages.
		/// These images are not patient specific, therefore are in a different location than the email attachments.
		/// For location of patient attachments, see EmailAttaches.GetAttachPath().
		/// </summary>
		public static string GetEmailImagePath() 
			=> GetFolder("EmailImages");

		/// <summary>
		/// When the Image module is opened, this loads newly added files.
		/// </summary>
		public static void AddMissingFilesToDatabase(Patient patient)
		{
			string patientFolder = GetPatientFolder(patient, FileAtoZ.GetPreferredAtoZpath());

			DirectoryInfo directoryInfo = new DirectoryInfo(patientFolder);

			var files = new List<string>(
				directoryInfo.GetFiles()
					.Where(fileInfo => !fileInfo.Attributes.HasFlag(FileAttributes.Hidden))
					.Select(fileInfo => fileInfo.FullName));

			Documents.InsertMissing(patient, files);
		}

		public static string GetHashString(Document document)
		{
			byte[] filebytes = new byte[1];

			var textbytes = string.IsNullOrEmpty(document.Note) ? 
				Encoding.UTF8.GetBytes(document.Note) : new byte[0];
			

			int fileLength = filebytes.Length;
			byte[] buffer = new byte[textbytes.Length + filebytes.Length];

			Array.Copy(filebytes, 0, buffer, 0, fileLength);
			Array.Copy(textbytes, 0, buffer, fileLength, textbytes.Length);

			return "";
			// TODO: return Encoding.ASCII.GetString(ODCrypt.MD5.Hash(buffer));
		}

		public static Bitmap[] OpenImages(IEnumerable<Document> documents, string patientFolder)
		{
			var bitmaps = new List<Bitmap>();

			foreach (var document in documents)
			{
				if (document == null)
				{
					bitmaps.Add(null);
				}
				else
				{
					bitmaps.Add(OpenImage(document, patientFolder));
				}
			}

			return bitmaps.ToArray();
		}

		public static Bitmap OpenImage(Document doc, string patientFolder)
		{
			string fileName = ODFileUtils.CombinePaths(patientFolder, doc.FileName);

			if (HasImageExtension(fileName))
			{
				try
				{
					return new Bitmap(fileName);
				}
				catch
				{
				}
			}

			return null;
		}

		private static Bitmap[] OpenImages(string folder, string fileName)
		{
			Bitmap[] values = new Bitmap[1] { null };

			fileName = ODFileUtils.CombinePaths(folder, fileName);
			if (HasImageExtension(fileName))
			{
				if (File.Exists(fileName))
				{
					try
					{
						values[0] = new Bitmap(fileName);
					}
					catch (Exception ex)
					{
						throw new ApplicationException("File found but could not be opened: " + fileName, ex);
					}
				}
				else
				{
					throw new ApplicationException("File not found: " + fileName);
				}
			}

			return values;
		}

		public static Bitmap[] OpenImagesEob(EobAttach eob) 
			=> OpenImages(GetEobFolder(), eob.FileName);

		public static Bitmap[] OpenImagesAmd(EhrAmendment amd) 
			=> OpenImages(GetAmdFolder(), amd.FileName);

		public static Document Import(string pathImportFrom, long docCategory, Patient pat)
		{
			string patFolder = GetPatientFolder(pat, FileAtoZ.GetPreferredAtoZpath());
			
			Document doc = new Document();
			//Document.Insert will use this extension when naming:
			if (Path.GetExtension(pathImportFrom) == "")
			{//If the file has no extension
				try
				{
					Bitmap bmp = new Bitmap(pathImportFrom);//check to see if file is an image and add .jpg extension
					doc.FileName = ".jpg";
				}
				catch
				{
					//catch the error and do nothing. Default the file to .txt to prevent errors.
					doc.FileName = ".txt";
				}
			}
			else
			{
				doc.FileName = Path.GetExtension(pathImportFrom);
			}

			doc.DateCreated = File.GetLastWriteTime(pathImportFrom);
			doc.PatNum = pat.PatNum;

			if (HasImageExtension(doc.FileName))
			{
				doc.ImgType = ImageType.Photo;
			}
			else
			{
				doc.ImgType = ImageType.Document;
			}

			doc.DocCategory = docCategory;
			doc = Documents.InsertAndGet(doc, pat);//this assigns a filename and saves to db

			try
			{
				SaveDocument(doc, pathImportFrom, patFolder);//Makes log entry
			}
			catch (Exception ex)
			{
				Documents.Delete(doc);
				throw ex;
			}

			return doc;
		}

		/// <summary>
		/// Saves to AtoZ folder, Cloud, or to db.
		/// Saves image as a jpg.
		/// Compression will differ depending on imageType.
		/// </summary>
		public static Document Import(Bitmap image, long docCategory, ImageType imageType, Patient patient, string mimeType = "image/jpeg")
		{
			string patientFolder = GetPatientFolder(patient, FileAtoZ.GetPreferredAtoZpath());

            var document = new Document
            {
                ImgType = imageType,
                FileName = GetImageFileExtensionByMimeType(mimeType),
                DateCreated = DateTime.Now,
                PatNum = patient.PatNum,
                DocCategory = docCategory
            };

            Documents.Insert(document, patient);
			document = Documents.GetByNum(document.DocNum);

            long qualityL;
            if (imageType == ImageType.Radiograph || imageType == ImageType.Photo)
			{
				qualityL = 100;
			}
			else
			{
				qualityL = ComputerPrefs.LocalComputer.ScanDocQuality;
			}

			ImageCodecInfo myImageCodecInfo;
			ImageCodecInfo[] encoders;
			encoders = ImageCodecInfo.GetImageEncoders();
			myImageCodecInfo = null;
			for (int j = 0; j < encoders.Length; j++)
			{
				if (encoders[j].MimeType == mimeType)
				{
					myImageCodecInfo = encoders[j];
				}
			}
			EncoderParameters myEncoderParameters = new EncoderParameters(1);
			EncoderParameter myEncoderParameter = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, qualityL);
			myEncoderParameters.Param[0] = myEncoderParameter;

			try
			{
				SaveDocument(document, image, myImageCodecInfo, myEncoderParameters, patientFolder);//Makes log entry
			}
			catch
			{
				Documents.Delete(document);
				throw;
			}
			return document;
		}

		/// <summary>
		/// Returns the file extension for the passed in mime type.
		/// </summary>
		private static string GetImageFileExtensionByMimeType(string mimeType)
		{
			switch (mimeType)
			{
				case "image/png": return ".png";
			}

			return ".jpg";
		}

		public static Document ImportForm(string form, long documentCategory, Patient patient)
		{
			string patientFolder = GetPatientFolder(patient, FileAtoZ.GetPreferredAtoZpath());
			string sourceFileName = ODFileUtils.CombinePaths(FileAtoZ.GetPreferredAtoZpath(), "Forms", form);

			if (!FileAtoZ.Exists(sourceFileName))
			{
				throw new Exception("Could not find file: " + sourceFileName);
			}

            Document document = new Document
            {
                FileName = Path.GetExtension(sourceFileName),
                DateCreated = DateTime.Now,
                DocCategory = documentCategory,
                PatNum = patient.PatNum,
                ImgType = ImageType.Document
            };

            Documents.Insert(document, patient);

			document = Documents.GetByNum(document.DocNum);

			try
			{
				SaveDocument(document, sourceFileName, patientFolder);
			}
			catch
			{
				Documents.Delete(document);
				throw;
			}

			return document;
		}

		/// <summary>
		/// Always saves as bmp.  So the 'paste to mount' logic needs to be changed to prevent conversion to bmp.
		/// </summary>
		public static Document ImportImageToMount(Bitmap image, short rotationAngle, long mountItemNum, long docCategory, Patient patient)
		{
			string patientFolder = GetPatientFolder(patient, FileAtoZ.GetPreferredAtoZpath());

            var document = new Document
            {
                MountItemNum = mountItemNum,
                DegreesRotated = rotationAngle,
                ImgType = ImageType.Radiograph,
                FileName = ".bmp",
                DateCreated = DateTime.Now,
                PatNum = patient.PatNum,
                DocCategory = docCategory,
                WindowingMin = PrefC.GetInt(PreferenceName.ImageWindowingMin),
                WindowingMax = PrefC.GetInt(PreferenceName.ImageWindowingMax)
            };

            Documents.Insert(document, patient);

			document = Documents.GetByNum(document.DocNum);
			try
			{
				SaveDocument(document, image, patientFolder);
			}
			catch
			{
				Documents.Delete(document);

				throw;
			}

			return document;
		}

		/// <summary>
		/// Saves to either AtoZ folder or to db.
		/// Saves image as a jpg.
		/// Compression will be according to user setting.
		/// </summary>
		public static EobAttach ImportEobAttach(Bitmap image, long claimPaymentNum)
		{
            EobAttach eob = new EobAttach
            {
                FileName = ".jpg",
                DateTCreated = DateTime.Now,
                ClaimPaymentNum = claimPaymentNum
            };

            EobAttaches.Insert(eob);

			eob = EobAttaches.GetOne(eob.EobAttachNum);

			ImageCodecInfo imageCodecInfo = null;
			foreach (var encoder in ImageCodecInfo.GetImageEncoders())
			{
				if (encoder.MimeType == "image/jpeg")
				{
					imageCodecInfo = encoder;

					break;
				}
			}

			var encoderParams = new EncoderParameters(1);
			encoderParams.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, ComputerPrefs.LocalComputer.ScanDocQuality);

			try
			{
				SaveEobAttach(eob, image, imageCodecInfo, encoderParams);
			}
			catch
			{
				EobAttaches.Delete(eob.EobAttachNum);
				throw;
			}

			return eob;
		}

		public static EobAttach ImportEobAttach(string pathImportFrom, long claimPaymentNum)
		{
			EobAttach eob = new EobAttach();
			if (Path.GetExtension(pathImportFrom) == "")
			{//If the file has no extension
				eob.FileName = ".jpg";
			}
			else
			{
				eob.FileName = Path.GetExtension(pathImportFrom);
			}

			eob.DateTCreated = File.GetLastWriteTime(pathImportFrom);
			eob.ClaimPaymentNum = claimPaymentNum;
			EobAttaches.Insert(eob);//creates filename and saves to db
			eob = EobAttaches.GetOne(eob.EobAttachNum);

			try
			{
				SaveEobAttach(eob, pathImportFrom);
			}
			catch
			{
				EobAttaches.Delete(eob.EobAttachNum);
				throw;
			}

			return eob;
		}

		public static EhrAmendment ImportAmdAttach(Bitmap image, EhrAmendment amd)
		{
			amd.FileName = DateTime.Now.ToString("yyyyMMdd_HHmmss_") + amd.EhrAmendmentNum + ".jpg";
			amd.DateTAppend = DateTime.Now;
			EhrAmendments.Update(amd);

			amd = EhrAmendments.GetOne(amd.EhrAmendmentNum);

			ImageCodecInfo imageCodecInfo = null;
			foreach (var encoder in ImageCodecInfo.GetImageEncoders())
            {
				if (encoder.MimeType == "image/jpeg")
				{
					imageCodecInfo = encoder;

					break;
				}
			}

			var encoderParams = new EncoderParameters(1);
			encoderParams.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, ComputerPrefs.LocalComputer.ScanDocQuality);

			SaveAmdAttach(amd, image, imageCodecInfo, encoderParams);

			return amd;
		}

		public static EhrAmendment ImportAmdAttach(string importFileName, EhrAmendment amd)
		{
			string amdFilename = amd.FileName;

			amd.DateTAppend = DateTime.Now;
			amd.FileName = DateTime.Now.ToString("yyyyMMdd_HHmmss_") + amd.EhrAmendmentNum + Path.GetExtension(importFileName);
			if (string.IsNullOrEmpty(Path.GetExtension(importFileName)))
			{
				amd.FileName += ".jpg";
			}

			SaveAmdAttach(amd, importFileName);

			EhrAmendments.Update(amd);

			CleanAmdAttach(amdFilename);

			return amd;
		}

		/// <summary>
		/// Save a Document to another location on the disk (outside of Open Dental).
		/// </summary>
		public static void Export(string exportFileName, Document document, Patient patient)
		{
			string sourceFileName = FileAtoZ.CombinePaths(GetPatientFolder(patient, FileAtoZ.GetPreferredAtoZpath()), document.FileName);

			FileAtoZ.Copy(sourceFileName, exportFileName);
		}

		/// <summary>
		/// Save an Eob to another location on the disk (outside of Open Dental).
		/// </summary>
		public static void ExportEobAttach(string exportPath, EobAttach eob)
		{
			string sourceFileName = ODFileUtils.CombinePaths(GetEobFolder(), eob.FileName);

			FileAtoZ.Copy(sourceFileName, exportPath);
		}

		/// <summary>
		/// Save an EHR amendment to another location on the disk (outside of Open Dental).
		/// </summary>
		public static void ExportAmdAttach(string exportPath, EhrAmendment amd)
		{
			string sourceFileName = ODFileUtils.CombinePaths(GetAmdFolder(), amd.FileName);

			FileAtoZ.Copy(sourceFileName, exportPath);
		}

		/// <summary>
		/// If using AtoZ folder, then patFolder must be fully qualified and valid.  
		/// If not using AtoZ folder, this uploads to Cloud or fills the doc.RawBase64 which must then be updated to db.  
		/// The image format can be bmp, jpg, etc, but this overload does not allow specifying jpg compression quality.
		/// </summary>
		private static void SaveDocument(Document document, Bitmap image, string patientFolder)
		{
			string destFileName = ODFileUtils.CombinePaths(patientFolder, document.FileName);

			image.Save(destFileName);

			LogDocument("Document Created: ", Permissions.ImageEdit, document, DateTime.MinValue);
		}

		public static void SaveDocument(Document document, Bitmap image, ImageCodecInfo codec, EncoderParameters encoderParameters, string patientFolder)
		{
			using (var bitmap = new Bitmap(image))
			{
				bitmap.Save(ODFileUtils.CombinePaths(patientFolder, document.FileName), codec, encoderParameters);
			}

			LogDocument("Document Created: ", Permissions.ImageEdit, document, DateTime.MinValue);
		}

		public static void SaveDocument(Document document, string sourceFileName, string patientFolder)
		{
			File.Copy(sourceFileName, ODFileUtils.CombinePaths(patientFolder, document.FileName));

			LogDocument("Document Created: ", Permissions.ImageEdit, document, DateTime.MinValue);
		}

		public static void SaveEobAttach(EobAttach eob, Bitmap image, ImageCodecInfo encoder, EncoderParameters encoderParams) 
			=> image.Save(ODFileUtils.CombinePaths(GetEobFolder(), eob.FileName), encoder, encoderParams);

		public static void SaveAmdAttach(EhrAmendment amd, Bitmap image, ImageCodecInfo encoder, EncoderParameters encoderParams) 
			=> image.Save(ODFileUtils.CombinePaths(GetAmdFolder(), amd.FileName), encoder, encoderParams);

		public static void SaveEobAttach(EobAttach eob, string sourceFileName) 
			=> File.Copy(sourceFileName, ODFileUtils.CombinePaths(GetEobFolder(), eob.FileName));

		public static void SaveAmdAttach(EhrAmendment amd, string sourceFileName) 
			=> File.Copy(sourceFileName, ODFileUtils.CombinePaths(GetAmdFolder(), amd.FileName));

		/// <summary>
		/// For each of the documents in the list, deletes row from db and image from AtoZ folder if needed.
		/// Throws exception if the file cannot be deleted.
		/// Surround in try/catch.
		/// </summary>
		/// <exception cref="Exception">If a document could not be deleted.</exception>
		public static void DeleteDocuments(IEnumerable<Document> documents, string patientFolder)
		{
			foreach (var document in documents)
            {
				if (document == null) continue;

				var sheets = Sheets.GetForDocument(document.DocNum);
				if (sheets.Count > 0)
                {
					var errorMessage = "Cannot delete image, it is referenced by sheets with the following dates:";
					foreach (var sheet in sheets)
                    {
						errorMessage += "\r\n" + sheet.DateTimeSheet.ToShortDateString();
					}

					throw new Exception(errorMessage);
                }

				var path = ODFileUtils.CombinePaths(patientFolder, document.FileName);
                try
                {
					if (File.Exists(path))
                    {
						File.Delete(path);

						LogDocument("Document Deleted: ", Permissions.ImageDelete, document, document.DateTStamp);
					}
                }
				catch
				{
					throw new Exception(
						"Could not delete file. It may be in use by another program, flagged as read-only, or you might not have sufficient permissions.");
				}

				Documents.Delete(document);
			}
		}

		/// <summary>
		/// Also handles deletion of db object.
		/// </summary>
		public static void DeleteEobAttach(EobAttach eob)
		{
			string path = ODFileUtils.CombinePaths(GetEobFolder(), eob.FileName);

			if (!TryDeleteFile(path))
			{
				return;
			}

			EobAttaches.Delete(eob.EobAttachNum);
		}

		/// <summary>
		/// Also handles deletion of db object.
		/// </summary>
		public static void DeleteAmdAttach(EhrAmendment amendment)
		{
			string path = ODFileUtils.CombinePaths(GetAmdFolder(), amendment.FileName);

			if (!TryDeleteFile(path, error => MessageBox.Show("Delete was unsuccessful.The file may be in use.")))
            {
				return;
            }

			amendment.DateTAppend = DateTime.MinValue;
			amendment.FileName = "";
			amendment.RawBase64 = "";
			EhrAmendments.Update(amendment);
		}

		public static bool TryDeleteFile(string path, Action<string> exceptionHandler = null)
		{
			try
			{
				if (File.Exists(path))
				{
					File.Delete(path);
				}

				return true;
			}
			catch (Exception exception)
			{
				exceptionHandler?.Invoke(exception.Message);
			}

			return false;
		}

		/// <summary>
		/// Cleans up unreferenced Amendments
		/// </summary>
		public static void CleanAmdAttach(string amdFileName) 
			=> TryDeleteFile(ODFileUtils.CombinePaths(GetAmdFolder(), amdFileName));

		public static void DeleteThumbnailImage(Document document, string patientFolder) 
			=> TryDeleteFile(ODFileUtils.CombinePaths(patientFolder, "Thumbnails", document.FileName));

		public static string GetExtension(Document document) 
			=> Path.GetExtension(document.FileName).ToLower();

		public static string GetFilePath(Document document, string patientFolder) 
			=> FileAtoZ.CombinePaths(patientFolder, document.FileName);

		/// <summary>
		/// Returns true if the given filename contains a supported file image extension.
		/// </summary>
		public static bool HasImageExtension(string fileName)
		{
			string ext = Path.GetExtension(fileName).ToLower();

			return 
				ext == ".jpg" || ext == ".jpeg" || ext == ".png" || ext == ".bmp" || 
				ext == ".tif" || ext == ".tiff" || ext == ".gif" || ext == ".emf" || 
				ext == ".ico" || ext == ".exif" || ext == ".wmf" || ext == ".tig" ||
				ext == ".tga";
		}

		/// <summary>
		/// Makes log entry for documents.
		/// Supply beginning text, permission, document, and the DateTStamp that the document was previously last edited.
		/// </summary>
		public static void LogDocument(string prefix, Permissions perm, Document document, DateTime secDatePrevious)
		{
			string message = prefix + document.FileName;

			if (document.Description != "")
			{
				string descriptDoc = document.Description;
				if (descriptDoc.Length > 50)
				{
					descriptDoc = descriptDoc.Substring(0, 50);
				}

				message += " with description " + descriptDoc;
			}

			var documentCategory = Definitions.GetDef(DefinitionCategory.ImageCats, document.DocCategory);

			message += " with category " + documentCategory.Name;

			SecurityLogs.MakeLogEntry(perm, document.PatNum, message, document.DocNum, secDatePrevious);
		}
	}
}
