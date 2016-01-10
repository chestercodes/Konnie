using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using Konnie.Model.File;
using Konnie.Model.FilesHistory;
using Konnie.Runner;
using Konnie.Runner.Logging;
using Microsoft.Web.XmlTransform;

namespace Konnie.Model.Tasks.SubTasks
{
	public class TransformTask : ISubTask
	{
		private readonly Func<string, Stream> _getFileStream;

		public TransformTask(Func<string, Stream> getFileStream = null)
		{
			_getFileStream = getFileStream ?? (fp => new FileStream(fp, FileMode.Open));
		}

		public string Name { get; set; }
		public ILogger Logger { get; set; }
		public string Type => nameof(TransformTask);

		public bool NeedToRun(IFilesHistory history)
		{
			return true;
		}

		public string InputFile { get; set; }
		public string OutputFile { get; set; }
		public List<string> TransformFiles { get; set; }

		public void Run(IFileSystemHandler fileSystemHandler, KVariableSets variableSets)
		{
			Logger.Verbose($"Starting task '{Name}'");

			if (TransformFiles.Count == 0)
			{
				throw new InvalidProgramException("Need to specify Transform file paths.");
			}
			
			CheckFilesExist(fileSystemHandler);

			var input = new XmlDocument();
			var stringReader = new StringReader(fileSystemHandler.ReadAllText(InputFile));
			input.Load(stringReader);

			var logger = new XmlTransformationLogger();
			foreach (var transformFile in TransformFiles)
			{
				Logger.Verbose($"Applying transform '{transformFile}'");
				var stream = _getFileStream(transformFile);
				var transform = new XmlTransformation(stream, logger);
				transform.Apply(input);
			}

			Logger.Verbose($"Writing to file '{OutputFile}'.");
			using (var stringWriter = new StringWriter())
			{
				var xmlWriterSettings = new XmlWriterSettings
				{
					Indent = true
				};
				using (var xmlTextWriter = XmlWriter.Create(stringWriter, xmlWriterSettings))
				{
					input.WriteTo(xmlTextWriter);
					xmlTextWriter.Flush();
					var xmlString = stringWriter.GetStringBuilder().ToString();
					fileSystemHandler.WriteAllText(xmlString, OutputFile);
				}
			}

			Logger.Verbose("Applied all of the transforms");
		}

		private void CheckFilesExist(IFileSystemHandler fileSystemHandler)
		{
			AssertFileExists(OutputFile, fileSystemHandler);
			AssertFileExists(InputFile, fileSystemHandler);
			foreach (var transformFile in TransformFiles)
			{
				AssertFileExists(transformFile, fileSystemHandler);
			}
		}

		private void AssertFileExists(string file, IFileSystemHandler fileSystemHandler)
		{
			if (fileSystemHandler.Exists(file) == false)
			{
				Logger.Terse($"File '{file}' doesn't exist, can't continue");
				throw new FileDoesntExist(file);
			}
		}
	}
}