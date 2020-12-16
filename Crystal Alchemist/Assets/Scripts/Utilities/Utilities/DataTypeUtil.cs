using System;
using Sirenix.OdinInspector;

[System.Serializable]
public class UDateTime
{
    [BoxGroup("Date")]
    [HorizontalGroup("Date/Group 1", LabelWidth = 35, MarginRight = 10, MaxWidth = 10, Width = 0)]
    public int day;
    [HorizontalGroup("Date/Group 1", LabelWidth = 40, MarginRight = 10, MaxWidth = 10, Width = 0)]
    public int month;
    [HorizontalGroup("Date/Group 1", LabelWidth = 40, MaxWidth = 10, Width = 0)]
    public int year;

    [HorizontalGroup("Date/Group 2", LabelWidth = 35, MarginRight = 10, MaxWidth = 10, Width = 0)]
    public int hour;
    [HorizontalGroup("Date/Group 2", LabelWidth = 40, MarginRight = 10, MaxWidth = 10, Width = 0)]
    public int minute;
    [HorizontalGroup("Date/Group 2", LabelWidth = 40, MaxWidth = 10, Width = 0)]
    public int second;

    public UDateTime(DateTime date)
    {
        this.year = date.Year;
        this.month = date.Month;
        this.day = date.Day;
        this.hour = date.Hour;
        this.minute = date.Minute;
        this.second = date.Second;
    }

    public UDateTime(string value)
    {
        try
        {
            this.year = Convert.ToInt32(value.Split(' ')[0].Split('.')[2]);
            this.month = Convert.ToInt32(value.Split(' ')[0].Split('.')[1]);
            this.day = Convert.ToInt32(value.Split(' ')[0].Split('.')[0]);
            this.hour = Convert.ToInt32(value.Split(' ')[1].Split('.')[0]);
            this.minute = Convert.ToInt32(value.Split(' ')[1].Split('.')[1]);
            this.second = Convert.ToInt32(value.Split(' ')[1].Split('.')[2]);
        }
        catch { }
    }

    public override string ToString()
    {
        return day+"."+month+"."+year +" "+ hour +":"+ minute + ":" + second;
    }

    public DateTime ToDateTime()
    {
        return new DateTime(year, month, day, hour, minute, second);
    }
}

[System.Serializable]
public class UTimeSpan
{
    [BoxGroup("Timespan")]
    [HorizontalGroup("Timespan/Group 1", LabelWidth = 30, MarginRight = 10, MaxWidth = 10, Width = 0)]
    public int days;
    [HorizontalGroup("Timespan/Group 1", LabelWidth = 30, MarginRight = 10, MaxWidth = 10, Width = 0)]
    public int hours;
    [HorizontalGroup("Timespan/Group 1", LabelWidth = 30, MarginRight = 10, MaxWidth = 10, Width = 0)]
    public int minutes;
    [HorizontalGroup("Timespan/Group 1", LabelWidth = 30, MaxWidth = 10, Width = 0)]
    public int seconds;

    public UTimeSpan(int hours, int minutes, int seconds)
    {
        this.hours = hours;
        this.minutes = minutes;
        this.minutes = minutes;
    }

    public UTimeSpan(string value)
    {
        try
        {
            this.days = Convert.ToInt32(value.Split(':')[0]);
            this.hours = Convert.ToInt32(value.Split(':')[1]);
            this.minutes = Convert.ToInt32(value.Split(':')[2]);
            this.seconds = Convert.ToInt32(value.Split(':')[3]);
        }
        catch { }
    }

    public override string ToString()
    {
        return days + ":" + hours + ":" + minutes + ":" + seconds;
    }

    public TimeSpan ToTimeSpan()
    {
        return new TimeSpan(days, hours, minutes, seconds);
    }
}

public class DataTypeUtil
{

}
