namespace Project.Utility;

internal class Utils
{
    public static string GetEnumString(Type enumClass, int intValue) => Enum.GetName(enumClass, intValue);

}
