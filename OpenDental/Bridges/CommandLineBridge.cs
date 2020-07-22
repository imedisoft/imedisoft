using CodeBase;
using OpenDentBusiness;
using System;
using System.Diagnostics;

namespace Imedisoft.Bridges
{
    public abstract class CommandLineBridge : IBridge
	{
		protected void ShowError(string errorMessage)
			=> ODMessageBox.Show(errorMessage);

		/// <summary>
		/// Sends the details of the specified patient to the external program.
		/// </summary>
		/// <param name="program">The program configuration.</param>
		/// <param name="patient">The patient details.</param>
		public void Send(Program program, Patient patient)
		{
			var path = Programs.GetProgramPath(program);

			if (patient == null)
			{
				try
				{
					Process.Start(path);
				}
				catch (Exception exception)
				{
					ShowError(exception.Message);
				}

				return;
			}

			var arguments = GetCommandLineArguments(program, patient);
			try
			{
				Process.Start(path, program.CommandLine + " " + arguments);
			}
			catch (Exception exception)
			{
				ShowError(exception.Message);
			}
		}

		/// <summary>
		/// Generates the command line arguments to pass to the external program.
		/// </summary>
		/// <param name="program">The program configuration.</param>
		/// <param name="patient">The patient details.</param>
		/// <returns>The command line arguments.</returns>
		protected virtual string GetCommandLineArguments(Program program, Patient patient)
		{
			return "";
		}

		/// <summary>
		/// Performs cleanup. Does nothing for command line bridges, as data is passed through command line arguments.
		/// </summary>
		public void Cleanup()
		{
		}
	}
}
