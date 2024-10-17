namespace MYPM.Common;
public static class GenerateOrderSerial
{
    public static string GetSL(int serial)
    {
        var now = DateTime.Now;
        string dayPart = now.Day.ToString("D2"); 
        string monthPart = now.ToString("MMM").ToUpper(); 
        string yearPart = now.ToString("yy");
        string serialPart = serial.ToString("D2"); 

        return $"{yearPart}_{monthPart}_{dayPart}_{serialPart}";
    }
}
