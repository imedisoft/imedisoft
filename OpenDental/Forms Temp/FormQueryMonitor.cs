using CodeBase;
using Imedisoft.Data;
using Imedisoft.Properties;
using OpenDental;
using OpenDental.UI;
using OpenDentBusiness;
using OpenDentBusiness.IO;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace Imedisoft.Forms
{
    public partial class FormQueryMonitor : FormBase
	{
		private readonly ConcurrentQueue<QueryInfo> queriesQueue = new ConcurrentQueue<QueryInfo>();
		private readonly Dictionary<Guid, QueryInfo> queriesDictionary = new Dictionary<Guid, QueryInfo>();
		private bool isMonitoring = false;
		private QueryInfo selectedQuery = null;

		public FormQueryMonitor() => InitializeComponent();

		private void FormQueryMonitor_Load(object sender, EventArgs e)
		{
			queryGrid.BeginUpdate();
			queryGrid.Columns.Clear();
			queryGrid.Columns.Add(new GridColumn(Translation.Common.Command, 100) { IsWidthDynamic = true });
			queryGrid.Columns.Add(new GridColumn(Translation.Common.Start, 125, HorizontalAlignment.Center, GridSortingStrategy.DateParse));
			queryGrid.Columns.Add(new GridColumn(Translation.Common.Elapsed, 100, HorizontalAlignment.Center));
			queryGrid.EndUpdate();
		}

		private void TimerProcessQueue_Tick(object sender, EventArgs e)
		{
			if (queriesQueue.Count == 0) return;

			var queries = new List<QueryInfo>();
			while (queriesQueue.Count != 0)
			{
				if (!queriesQueue.TryDequeue(out var query))
				{
					break;
				}

				queries.Add(query);
			}

			AddQueriesToGrid(queries.ToArray());
		}

		private void QueryGrid_CellClick(object sender, ODGridClickEventArgs e)
		{
			startTextBox.Clear();
			stopTextBox.Clear();
			elapsedTextBox.Clear();
			commandTextBox.Clear();

            if (!(queryGrid.Rows[e.Row].Tag is QueryInfo query))
            {
                return;
            }

            try
            {
				selectedQuery = query;

				startTextBox.Text = query.StartTime.ToString();
				stopTextBox.Text = query.StartTime.ToString();
				elapsedTextBox.Text = query.Elapsed.ToString("G");
				commandTextBox.Text = query.Command;
			}
            catch { }

			copyButton.Enabled = selectedQuery != null;
		}

		private void AddQueriesToGrid(params QueryInfo[] queries)
		{
			if (queries.IsNullOrEmpty()) return;

			foreach (QueryInfo query in queries)
			{
				queriesDictionary[query.GUID] = query;
			}

			// Arbitrarily limit the number of rows showing in the grid.
			// The top of the grid is the oldest query so take the last X items.
			// Use TakeLast(X) instead of a loop because a bunch of queries could have been queued (e.g. pasting schedules can queue thousands).
			var displayQueries = queriesDictionary.Values.TakeLast(500);
			queryGrid.BeginUpdate();
			queryGrid.Rows.Clear();

			foreach (var query in displayQueries)
			{
				var row = new GridRow(query.Command.Trim(), query.StartTime.ToString(), (query.Elapsed == TimeSpan.MinValue) ? "" : query.Elapsed.ToString("G"))
				{
					Tag = query
				};

				queryGrid.Rows.Add(row);
			}

			queryGrid.EndUpdate();
			queryGrid.ScrollToIndex(queryGrid.Rows.Count - 1);
		}

		private void Start()
        {
			if (isMonitoring) return;

			timerProcessQueue.Start();
			QueryMonitorEvent.Fired += DbMonitorEvent_Fired;
			QueryMonitor.IsMonitoring = true;

			toggleButton.Text = "&Stop";
			toggleButton.Image = Resources.IconMediaStop;

			logButton.Enabled = false;

			isMonitoring = true;
		}

		private void Stop()
        {
			if (isMonitoring)
            {
				timerProcessQueue.Stop();
				QueryMonitorEvent.Fired -= DbMonitorEvent_Fired;
				QueryMonitor.IsMonitoring = false;

				toggleButton.Text = "&Start";
				toggleButton.Image = Resources.IconMediaPlay;

				logButton.Enabled = true;

				isMonitoring = false;
			}
        }

		private void ToggleButton_Click(object sender, EventArgs e)
		{
			if (isMonitoring) Stop();
            else
            {
				Start();
            }
		}

		private void DbMonitorEvent_Fired(ODEventArgs e)
		{
			if (e.EventType != EventCategory.QueryMonitor || !(e.Tag is QueryInfo))
			{
				return;
			}

			queriesQueue.Enqueue(e.Tag as QueryInfo);
		}

		private void LogButton_Click(object sender, EventArgs e)
		{
			if (isMonitoring)
			{
				ShowInfo(Translation.Common.StopMonitoringQueriesBeforeCreatingLog);
				return;
			}

			if (queriesDictionary.Count == 0)
			{
				ShowInfo(Translation.Common.NoQueriesInTheQueryFeedToLog);
				return;
			}

			if (Prompt($"Log all queries to a file? Total query count: {queriesDictionary.Count:N0}") == DialogResult.No)
            {
				return;
            }

			string logPath;
			try
			{
				// Create the query monitor log folder in the AtoZ image path.
				var path = Storage.CombinePaths(Storage.GetRootPath(), "QueryMonitorLogs");
				if (!Storage.DirectoryExists(path))
                {
					Storage.CreateDirectory(path);
                }

				// Get a unique file name within the log folder.
				logPath = Storage.CombinePaths(path, $"QueryMonitorLog_{DateTime.Now:yyyyMMdd_HHmmss}.txt");
				while (Storage.FileExists(logPath))
				{
					logPath = Storage.CombinePaths(path, $"QueryMonitorLog_{DateTime.Now:yyyyMMdd_HHmmss}.txt");
					Thread.Sleep(100);
				}

				// Dump the entire query history into the log file.
				Storage.WriteAllText(logPath,
					$"Query Monitor Log - {DateTime.Now}, OD User: {Security.CurrentUser.UserName}, Computer: {Environment.MachineName}\r\n" +
					$"{string.Join("\r\n", queriesDictionary.Values.Select(x => x.ToString()))}");
			}
			catch (ODException exception)
			{
				ShowError(exception.Message);

				return;
			}
			catch (Exception exception)
			{
				ShowError(string.Format(
					Translation.Common.ErrorCreatingLogFile, exception.Message));

				return;
			}

			if (Prompt(Translation.Common.LogFileCreatedWouldYouLikeToOpenFile) == DialogResult.Yes)
			{
				try
				{
					Storage.RunRelative(logPath);
				}
				catch (Exception ex)
				{
					ShowError(string.Format(
						Translation.Common.ErrorOpeningLogFile, ex.Message));
				}
			}
		}

		private void CopyButton_Click(object sender, EventArgs e)
		{
			if (selectedQuery == null)
			{
				ShowInfo(Translation.Common.SelectRowFromQueryFeed);

				return;
			}

			try
			{
				ODClipboard.Text = selectedQuery.ToString();

				ShowInfo(Translation.Common.Copied);
			}
			catch
			{
				ShowError(Translation.Common.CouldNotCopyContentsToClipboard);
			}
		}

		private void CloseButton_Click(object sender, EventArgs e)
		{
			DialogResult = DialogResult.Cancel;

			Close();
		}

		private void FormQueryMonitor_FormClosing(object sender, FormClosingEventArgs e)
		{
			ODException.SwallowAnyException(() => { QueryMonitorEvent.Fired -= DbMonitorEvent_Fired; });
		}

        private void AlwaysOnTopCheckBox_CheckedChanged(object sender, EventArgs e)
        {
			TopMost = alwaysOnTopCheckBox.Checked;
        }
    }
}
