using System.Collections.Generic;
using Konnie.Model.FilesHistory;

namespace Konnie.Model.Tasks.SubTasks
{
	public class SubstitutionTask : ISubTaskThatUsesVariableSets
	{
		public string Name { get; set; }
		public string Type => nameof(SubstitutionTask);

		public bool NeedToRun(IFilesHistory history)
		{
			return true;
		}

		public void Run()
		{
		}

		public List<string> SubstitutionVariableSets { get; set; }
	}
}