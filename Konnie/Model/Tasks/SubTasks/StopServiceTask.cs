namespace Konnie.Model.Tasks.SubTasks
{
	public class StopServiceTask : ISubTask
	{
		public string Name { get; set; }
		public string Type => nameof(StopServiceTask);
		public string ServiceName { get; set; }

		public bool CanRun(IFilesHistory history)
		{
			return true;
		}

		public void Run()
		{
			
		}
	}
}