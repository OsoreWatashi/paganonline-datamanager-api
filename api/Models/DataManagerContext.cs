using MySql.Data.MySqlClient;

namespace PaganOnline.DataManager.API.Models
{
  public partial class DataManagerContext
  { 
    public string ConnectionString { get; }

    public DataManagerContext(string connectionString)
    {
      ConnectionString = connectionString;
    }

    private MySqlConnection GetConnection() => new MySqlConnection(ConnectionString);
  }
}