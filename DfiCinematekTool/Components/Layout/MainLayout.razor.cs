namespace DfiCinematekTool.Components.Layout
{
	public partial class MainLayout
	{
		private bool sidebarExpanded = true;

		public void SidebarToggleClick()
		{
			sidebarExpanded = !sidebarExpanded;
		}
	}
}
