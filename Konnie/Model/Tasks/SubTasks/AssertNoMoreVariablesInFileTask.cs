using Konnie.Model.FilesHistory;
using Konnie.Runner;
using Konnie.Runner.Logging;

namespace Konnie.Model.Tasks.SubTasks
{
	public class AssertNoMoreVariablesInFileTask : ISubTask
	{
		public string Name { get; set; }
		public ILogger Logger { get; set; }
		public string Type => nameof(AssertNoMoreVariablesInFileTask);
		
		public bool NeedToRun(IFilesHistory history)
		{
			return true;
		}

		public void Run(FileSystemHandler fileSystemHandler)
		{
			
		}
	}
}