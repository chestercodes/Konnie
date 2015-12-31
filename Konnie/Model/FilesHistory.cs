using System;
using System.Collections.Generic;
using System.IO.Abstractions;
using Newtonsoft.Json;

namespace Konnie.Model
{
	/// <summary>
	///     IFileHistory objects are task specific, it doesn't make sense for them to know about other tasks.
	///     They only care about konnie and input files (xml files and transforms etc)`.
	///     They know how to check to see if a file is different (newer or older) to the last time it was seen and return true
	///     or false
	///     They also know how to update the history if a file is changed for whatever reason
	///     They can commit changes to the file history, this should be done when the task is finishing.
	/// </summary>
	public interface IFilesHistory
	{
		bool FileIsDifferent(string absoluteFilePath, DateTime lastModified);
		void UpdateHistory(string absoluteFilePath, DateTime lastModified);
		void CommitChanges();
	}

	public class FilesHistory : IFilesHistory
	{
		private readonly IFileSystem _fs;
		private readonly string _historyJsonFilePath;
		private readonly bool _runWithoutHistory;
		private HistoryFile _historyFile = new HistoryFile();
		private string _taskBeingPerformed;

		public FilesHistory(string historyJsonFilePath, string taskBeingPerformed, IFileSystem fs = null)
		{
			_fs = fs ?? new FileSystem();
			_historyJsonFilePath = historyJsonFilePath;
			_runWithoutHistory = string.IsNullOrEmpty(_historyJsonFilePath);
			_taskBeingPerformed = taskBeingPerformed;

			if (string.IsNullOrEmpty(taskBeingPerformed))
			{
				throw new InvalidProgramException("Need to specify a task.");
			}

			if (_runWithoutHistory || _fs.File.Exists(historyJsonFilePath) == false)
			{
				return;
			}
			
			var jsonText = _fs.File.ReadAllText(historyJsonFilePath);
			_historyFile = JsonConvert.DeserializeObject<HistoryFile>(jsonText);
		}

		public bool FileIsDifferent(string absoluteFilePath, DateTime lastModified)
		{
			if (_runWithoutHistory)
			{
				return true;
			}

			return true;
		}

		public void UpdateHistory(string absoluteFilePath, DateTime lastModified)
		{
			if (_runWithoutHistory)
			{
			}
		}

		public void CommitChanges()
		{
			if (_runWithoutHistory)
			{
			}

			// save file to wherever.
		}
	}

	internal class HistoryFile : Dictionary<string, FileModifiedDateByAbsoluteFilePath>
	{
	}

	internal class FileModifiedDateByAbsoluteFilePath : Dictionary<string, DateTime>
	{
	}
}