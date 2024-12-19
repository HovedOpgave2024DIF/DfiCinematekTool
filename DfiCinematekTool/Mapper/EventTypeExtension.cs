using DfiCinematekTool.Domain.Enums;

namespace DfiCinematekTool.Mapper
{
	public static class EventTypeExtension
	{
		public static EventType ConvertEventTypeEnumToEventType(this EventTypeEnum eventTypeEnum)
		{
			return new EventType
			{
				Event = eventTypeEnum.ToString(),
				Value = eventTypeEnum.ToString(),
			};
		}
	}

	public class EventType
	{
		public string Event { get; set; } = string.Empty;
		public string Value { get; set; } = string.Empty;
	}
}
