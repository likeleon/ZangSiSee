using SQLite;
using System;
using System.IO;
using Xamarin.Forms;
using ZangSiSee.Interfaces;

[assembly: Dependency(typeof(SQLiteAndroid))]

public class SQLiteAndroid : ISQLite
{
    public SQLiteConnection GetConnection(string name)
    {
        var directory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
        return new SQLiteConnection(Path.Combine(directory, name));
    }
}