using DfiCinematekTool.Domain.Enums;

namespace DfiCinematekTool.Mapper
{
	public static class ScreenExtension
	{
		public static Screen ConvertScreenEnumToScreen(this ScreenEnum screenEnum)
		{
			return new Screen
			{
				ScreenType = screenEnum.ToString(),
				Value = screenEnum.ToString()
			};
		}
	}

	public class Screen
	{
		public string ScreenType { get; set; } = string.Empty;
		public string Value { get; set; } = string.Empty;
	}
}
