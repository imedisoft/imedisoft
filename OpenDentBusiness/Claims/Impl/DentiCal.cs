using CodeBase;
using OpenDentBusiness;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using Tamir.SharpSsh.jsch;

namespace Imedisoft.Claims.Impl
{
    public class DentiCal : ClaimBridge
	{
		public DentiCal() : base("Denti-Cal")
		{
		}

        protected override bool OnSend(Clearinghouse clearingHouse, long batchNumber, List<ClaimSendQueueItem> queueItems, EnumClaimMedType medType)
        {
			// called from FormClaimReports and Eclaims.cs. clinic-level clearinghouse passed in.
			// Before this function is called, the X12 file for the current batch has already been generated in
			// the clearinghouse export folder. The export folder will also contain batch files which have failed
			// to upload from previous attempts and we must attempt to upload these older batch files again if
			// there are any.
			// Step 1: Retrieve reports regarding the existing pending claim statuses.
			// Step 2: Send new claims in a new batch.
			IODProgressExtended progress = new ODProgressExtendedNull();

			bool success = true;
            
            progress.UpdateProgress("Contacting web server", "reports", "17%", 17);
			if (progress.IsPauseOrCancel())
			{
				progress.UpdateProgress("Canceled by user.");
				return false;
			}

            ChannelSftp ch;
            Channel channel;
			Session session;
			JSch jsch = new JSch();

			// Connect to the Denti-Cal SFTP server.
			try
            {
                var remoteHost = "sftp.mft.oxisaas.com";
                var remotePort = 2222;

                if (!string.IsNullOrEmpty(clearingHouse.ClientProgram))
                {
                    var p = clearingHouse.ClientProgram.IndexOf(':');
                    if (p == -1)
                    {
                        remoteHost = clearingHouse.ClientProgram;
                    }
                    else
                    {
                        remoteHost = clearingHouse.ClientProgram.Substring(0, p);
                        if (!int.TryParse(clearingHouse.ClientProgram.Substring(p + 1), out remotePort))
                        {
                            remotePort = 2222;
                        }
                    }
                }

                session = jsch.getSession(clearingHouse.LoginID, remoteHost);
                session.setPassword(clearingHouse.Password);
                session.setConfig(new Hashtable
                {
                    { "StrictHostKeyChecking", "no" }
                });
                session.setPort(remotePort);
                session.connect();

                channel = session.openChannel("sftp");
                channel.connect();

                ch = (ChannelSftp)channel;
            }
            catch (Exception exception)
            {
                ErrorMessage = "Connection Failed: " + exception.Message;

                return false;
            }

            progress.UpdateProgress("Web server contact successful.");
			try
			{
				string homeDir = "/"; // new production home root dir
									  // At this point we are connected to the Denti-Cal SFTP server.
				if (batchNumber == 0)
				{ 
					// Retrieve reports.
					progress.UpdateProgress("Downloading reports", "reports", "33%", 33);
					if (progress.IsPauseOrCancel())
					{
						progress.UpdateProgress("Canceled by user.");
						return false;
					}

					if (!Directory.Exists(clearingHouse.ResponsePath))
					{
						progress.UpdateProgress("Clearinghouse response path is invalid.");

						return false;
					}

					// Only retrieving reports so do not send new claims.
					// Although the documentation we received from Denti-Cal says that the folder name should start "OXi", that was not the case for a customer
					// that we connected to and Barbara Castelli from Denti-Cal informed us that the folder name should start with "dcaprod".
					string retrievePath = homeDir + "dcaprod_" + clearingHouse.LoginID + "_out/";
					Tamir.SharpSsh.java.util.Vector fileList;
					try
					{
						fileList = ch.ls(retrievePath);
					}
					catch
					{
						// Try again with the path as described in the documentation.
						retrievePath = homeDir + "OXi_" + clearingHouse.LoginID + "_out/";
						fileList = ch.ls(retrievePath);
					}

					for (int i = 0; i < fileList.Count; i++)
					{
						int percent = (i / fileList.Count) * 100;

						// We re-use the bar again for importing later, hence the tag.
						progress.UpdateProgress("Getting file:" + i + " / " + fileList.Count, "import", percent + "%", percent);
						if (progress.IsPauseOrCancel())
						{
							progress.UpdateProgress("Canceled by user.");
							return false;

						}

						string listItem = fileList[i].ToString().Trim();
						if (listItem[0] == 'd')
						{
							continue; // Skip directories and focus on files.
						}

						Match fileNameMatch = Regex.Match(listItem, ".*\\s+(.*)$");
						string getFileName = fileNameMatch.Result("$1");
						string getFilePath = retrievePath + getFileName;
						string exportFilePath = ODFileUtils.CombinePaths(clearingHouse.ResponsePath, getFileName);

						try
						{
							using var fileStream = ch.get(getFilePath);
							using var exportFileStream = File.Open(exportFilePath, FileMode.Create, FileAccess.Write);//Creates or overwrites.

							byte[] dataBytes = new byte[4096];
							int numBytes = fileStream.Read(dataBytes, 0, dataBytes.Length);
							while (numBytes > 0)
							{
								exportFileStream.Write(dataBytes, 0, numBytes);
								numBytes = fileStream.Read(dataBytes, 0, dataBytes.Length);
							}
							float overallpercent = 33 + (i / fileList.Count) * 17;//33 is starting point. 17 is the amount of bar space we have before our next major spot (50%)
							progress.UpdateProgress("Getting files", "reports", overallpercent + "%", (int)overallpercent);
							if (progress.IsPauseOrCancel())
							{
								progress.UpdateProgress("Canceled by user.");
								return false;
							}
						}
						catch
						{
							success = false;
						}
						finally
						{
							progress.UpdateProgress("", "import", "");//Clear import bar for now
						}

						if (success)
						{
							// Removed the processed report from the Denti-Cal SFTP so it does not get processed again in the future.
							try
							{
								ch.rm(getFilePath);
								progress.UpdateProgress("Reports downloaded successfully.");
							}
							catch
							{
							}
						}
					}
				}
				else
				{
					// Send batch of claims.
					progress.UpdateProgress("Sending batch of claims", "reports", "33%", 33);
					if (progress.IsPauseOrCancel())
					{
						progress.UpdateProgress("Canceled by user.");
						return false;
					}

					if (!Directory.Exists(clearingHouse.ExportPath))
					{
						throw new Exception("Clearinghouse export path is invalid.");
					}

					string[] files = Directory.GetFiles(clearingHouse.ExportPath);

					// Try to find a folder that starts with "dcaprod" or "OXi".
					string uploadPath = homeDir + "dcaprod_" + clearingHouse.LoginID + "_in/";
					Tamir.SharpSsh.java.util.Vector fileList;
					try
					{
						fileList = ch.ls(uploadPath);
					}
					catch
					{

						// Try again with the path as described in the documentation.
						uploadPath = homeDir + "OXi_" + clearingHouse.LoginID + "_in/";
						fileList = ch.ls(uploadPath);
					}

					// We have successfully found the folder where we need to put the files.
					for (int i = 0; i < files.Length; i++)
					{
						float overallpercent = 33 + (i / files.Length) * 17; // 33 is starting point. 17 is the amount of bar space we have before our next major spot (50%)
						progress.UpdateProgress("Sending claims", "reports", overallpercent + "%", (int)overallpercent);
						if (progress.IsPauseOrCancel())
						{
							progress.UpdateProgress("Canceled by user.");
							return false;
						}

						// First upload the batch file to a temporary file name. 
						// Denti-Cal does not process file names unless they start with the Login ID.
						// Uploading to a temporary file and then renaming the file allows us to avoid partial file uploads if there is connection loss.
						string tempRemoteFilePath = uploadPath + "temp_" + Path.GetFileName(files[i]);
						ch.put(files[i], tempRemoteFilePath);
						
						// Denti-Cal requires the file name to start with the Login ID followed by a period and end with a .txt extension.
						// The middle part of the file name can be anything.
						string remoteFilePath = uploadPath + Path.GetFileName(files[i]);
						ch.rename(tempRemoteFilePath, remoteFilePath);
						File.Delete(files[i]);
					}

					progress.UpdateProgress("Claims sent successfully.");
				}
			}
			catch (Exception exception)
			{
				success = false;

				ErrorMessage += exception.Message;
			}
			finally
			{
				progress.UpdateProgress("Closing connection", "reports", "50%", 50);

				// Disconnect from the Denti-Cal SFTP server.
				channel.disconnect();
				ch.disconnect();

				session.disconnect();
			}

			return success;
		}
	}
}
