using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace PaganOnline.DataManager.API.Models
{
  public partial class DataManagerContext
  {
    public async Task<IEnumerable<SkillEffect>> GetAllSkillEffects(int skillID)
    {
      var skillEffects = new List<SkillEffect>(64);
      using (MySqlConnection connection = GetConnection())
      {
        await connection.OpenAsync();
        var command = connection.CreateCommand();
        command.CommandText = "SELECT SkillID, Level, Sequence, Description FROM SkillEffects WHERE SkillID = @SkillID";
        command.Parameters.AddWithValue("@SkillID", skillID);

        using (var reader = await command.ExecuteReaderAsync())
          while (await reader.ReadAsync())
            skillEffects.Add(new SkillEffect((int) reader["SkillID"], (sbyte) reader["Level"], (sbyte) reader["Sequence"], (string) reader["Description"]));
      }

      return skillEffects;
    }

    public async Task<SkillEffect> GetSkillEffect(int skillID, sbyte level, sbyte sequence)
    {
      var skillEffect = (SkillEffect) null;
      using (MySqlConnection connection = GetConnection())
      {
        await connection.OpenAsync();
        var command = connection.CreateCommand();
        command.CommandText = "SELECT SkillID, Level, Sequence, Description FROM SkillEffects WHERE SkillID = @SkillID AND Level = @Level AND Sequence = @Sequence";
        command.Parameters.AddWithValue("@SkillID", skillID);
        command.Parameters.AddWithValue("@Level", level);
        command.Parameters.AddWithValue("@Sequence", sequence);
        
        using (var reader = await command.ExecuteReaderAsync())
          if (await reader.ReadAsync())
            skillEffect = new SkillEffect((int) reader["SkillID"], (sbyte) reader["Level"], (sbyte) reader["Sequence"], (string) reader["Description"]);
      }

      return skillEffect;
    }

    public async Task<(int SkillID, sbyte Level, sbyte Sequence)> CreateSkillEffect(SkillEffect skillEffect)
    {
      var success = false;
      using (MySqlConnection connection = GetConnection())
      {
        await connection.OpenAsync();
        var command = connection.CreateCommand();
        command.CommandText = "INSERT INTO SkillEffects (SkillID, Level, Sequence, Description) VALUES (@SkillID, @Level, @Sequence, @Description)";
        command.Parameters.AddWithValue("@SkillID", skillEffect.SkillID);
        command.Parameters.AddWithValue("@Level", skillEffect.Level);
        command.Parameters.AddWithValue("@Sequence", skillEffect.Sequence);
        command.Parameters.AddWithValue("@Description", skillEffect.Description);

        try
        {
          var result = await command.ExecuteNonQueryAsync();
          success = result == 1;
        }
        catch (Exception exception)
        {
          Debug.WriteLine(exception);
        }
      }

      return success ? (skillEffect.SkillID, skillEffect.Level, skillEffect.Sequence) : (0, (sbyte) 0, (sbyte) 0);
    }

    public async Task<(bool Conflict, int SkillID, sbyte Level, sbyte Sequence)> UpdateSkillEffect(int skillID, sbyte level, sbyte Sequence, SkillEffect skillEffect)
    {
      var success = false;
      var conflict = false;
      using (MySqlConnection connection = GetConnection())
      {
        await connection.OpenAsync();
        var command = connection.CreateCommand();
        command.CommandText = "UPDATE SkillEffects SET SkillID = @SkillID, Level = @Level, Sequence = @Sequence, Description = @Description WHERE SkillID = @SkillIDFilter AND Level = @LevelFilter AND Sequence = @SequenceFilter";
        command.Parameters.AddWithValue("@SkillID", skillEffect.SkillID);
        command.Parameters.AddWithValue("@Level", skillEffect.Level);
        command.Parameters.AddWithValue("@Sequence", skillEffect.Sequence);
        command.Parameters.AddWithValue("@Description", skillEffect.Description);
        command.Parameters.AddWithValue("@SkillIDFilter", skillID);
        command.Parameters.AddWithValue("@LevelFilter", level);
        command.Parameters.AddWithValue("@SequenceFilter", Sequence);

        try
        {
          var result = await command.ExecuteNonQueryAsync();
          success = result == 1;
        }
        catch (Exception exception)
        {
          conflict = true;
          Debug.WriteLine(exception);
        }
      }

      return success
        ? (conflict, skillEffect.SkillID, skillEffect.Level, skillEffect.Sequence)
        : (conflict, 0, (sbyte) 0, (sbyte) 0);
    }

    public async Task<(bool Conflict, bool Success)> DeleteSkillEffect(int skillID, sbyte level, sbyte sequence)
    {
      var success = false;
      var conflict = false;
      using (MySqlConnection connection = GetConnection())
      {
        await connection.OpenAsync();
        var command = connection.CreateCommand();
        command.CommandText = "DELETE FROM SkillEffects  WHERE SkillID = @SkillID AND Level = @Level AND Sequence = @Sequence";
        command.Parameters.AddWithValue("@SkillID", skillID);
        command.Parameters.AddWithValue("@Level", level);
        command.Parameters.AddWithValue("@Sequence", sequence);

        try
        {
          var result = await command.ExecuteNonQueryAsync();
          success = result == 1;
        }
        catch (Exception exception)
        {
          conflict = true;
          Debug.WriteLine(exception);
        }
      }

      return (conflict, success);
    }
  }
}