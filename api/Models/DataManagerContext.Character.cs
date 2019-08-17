using System.Collections.Generic;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace PaganOnline.DataManager.API.Models
{
  public partial class DataManagerContext
  {
    public async Task<IEnumerable<Character>> GetAllCharacters()
    {
      var characters = new List<Character>(8);
      using (MySqlConnection connection = GetConnection())
      {
        await connection.OpenAsync();
        var command = connection.CreateCommand();
        command.CommandText = "SELECT TechnicalName, DisplayName FROM Characters";


        using (var reader = await command.ExecuteReaderAsync())
          while (await reader.ReadAsync())
            characters.Add(new Character((string) reader["TechnicalName"], (string) reader["DisplayName"]));
      }

      return characters;
    }

    public async Task<Character> GetCharacterByTechnicalName(string technicalName)
    {
      var character = (Character) null;
      using (MySqlConnection connection = GetConnection())
      {
        await connection.OpenAsync();
        var command = connection.CreateCommand();
        command.CommandText = "SELECT TechnicalName, DisplayName FROM Characters WHERE TechnicalName = @TechnicalName";
        command.Parameters.AddWithValue("@TechnicalName", technicalName);

        using (var reader = await command.ExecuteReaderAsync())
          if (await reader.ReadAsync())
            character = new Character((string) reader["TechnicalName"], (string) reader["DisplayName"]);
      }

      return character;
    }

    public async Task<string> CreateCharacter(Character character)
    {
      var success = false;
      using (MySqlConnection connection = GetConnection())
      {
        await connection.OpenAsync();
        var command = connection.CreateCommand();
        command.CommandText = "INSERT INTO Characters (TechnicalName, DisplayName) VALUES (@TechnicalName, @DisplayName)";
        command.Parameters.AddWithValue("@TechnicalName", character.TechnicalName);
        command.Parameters.AddWithValue("@DisplayName", character.DisplayName);
        
        var result = await command.ExecuteScalarAsync();
        success = true; // TODO: Properly read result
      }

      return success ? character.TechnicalName : null;
    }

    public async Task<string> UpdateCharacter(string technicalName, Character character)
    {
      var success = false;
      using (MySqlConnection connection = GetConnection())
      {
        await connection.OpenAsync();
        var command = connection.CreateCommand();
        command.CommandText = "UPDATE Characters SET TechnicalName = @TechnicalName, DisplayName = @DisplayName WHERE TechnicalName = @TechnicalNameFilter";
        command.Parameters.AddWithValue("@TechnicalName", character.TechnicalName);
        command.Parameters.AddWithValue("@DisplayName", character.DisplayName);
        command.Parameters.AddWithValue("@TechnicalNameFilter", technicalName);
        
        var result = await command.ExecuteScalarAsync();
        success = true; // TODO: Properly read result
      }

      return success ? character.TechnicalName : null;
    }

    public async Task<bool> DeleteCharacter(string technicalName)
    {
      var success = false;
      using (MySqlConnection connection = GetConnection())
      {
        await connection.OpenAsync();
        var command = connection.CreateCommand();
        command.CommandText = "DELETE FROM Characters  WHERE TechnicalName = @TechnicalName";
        command.Parameters.AddWithValue("@TechnicalName", technicalName);
        
        var result = await command.ExecuteScalarAsync();
        success = true; // TODO: Properly read result
      }

      return success;
    }
  }
}