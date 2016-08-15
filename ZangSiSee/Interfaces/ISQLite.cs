using SQLite;

namespace ZangSiSee.Interfaces
{
    public interface ISQLite
    {
        SQLiteConnection GetConnection(string filename);
    }
}
