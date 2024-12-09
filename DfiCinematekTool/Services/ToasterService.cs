using Radzen;

namespace DfiCinematekTool.Services
{
	public class ToasterService
	{
		private readonly NotificationService _notificationService;

		public ToasterService(NotificationService notificationService)
		{
			_notificationService = notificationService;
		}

		public void OnSuccess(string summary, string detail, int duration)
		{
			_notificationService.Notify(
				new NotificationMessage
				{
					Severity = NotificationSeverity.Success,
					Summary = summary,
					Detail = detail,
					Duration = duration,
					Style = "opacity:85%"
				}
			);
		}

		public void OnError(string summary, string detail, int duration)
		{
			_notificationService.Notify(
				new NotificationMessage
				{
					Severity = NotificationSeverity.Error,
					Summary = summary,
					Detail = detail,
					Duration = duration,
					Style = "opacity:85%"
				}
			);
		}

		public void OnWarning(string summary, string detail, int duration)
		{
			_notificationService.Notify(
				new NotificationMessage
				{
					Severity = NotificationSeverity.Warning,
					Summary = summary,
					Detail = detail,
					Duration = duration,
					Style = "opacity:85%"
				}
			);
		}
	}
}
