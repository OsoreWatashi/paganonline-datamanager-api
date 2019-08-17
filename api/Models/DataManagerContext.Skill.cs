using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace PaganOnline.DataManager.API.Models
{
  public partial class DataManagerContext
  {
    public async Task<IEnumerable<Skill>> GetAllSkills(string characterTechnicalName)
    {
      var skills = new List<Skill>(64);
      using (MySqlConnection connection = GetConnection())
      {
        await connection.OpenAsync();
        var command = connection.CreateCommand();
        command.CommandText = "SELECT ID, ParentID, CharacterTechnicalName, TechnicalName, DisplayName, Type, Description, LevelRequirement, MinimumPoints, MaximumPoints FROM Skills WHERE CharacterTechnicalName = @CharacterTechnicalName";
        command.Parameters.AddWithValue("@CharacterTechnicalName", characterTechnicalName);
        
        using (var reader = await command.ExecuteReaderAsync())
          while (await reader.ReadAsync())
            skills.Add(new Skill((int) reader["ID"], reader["ParentID"] as int?, (string) reader["CharacterTechnicalName"], (string) reader["TechnicalName"], (string) reader["DisplayName"], SkillType.Ability, (string) reader["Description"], (sbyte) reader["LevelRequirement"], (sbyte) reader["MinimumPoints"], (sbyte) reader["MaximumPoints"]));
      }

      return skills;
    }

    public async Task<Skill> GetSkillByID(int id)
    {
      var skill = (Skill) null;
      using (MySqlConnection connection = GetConnection())
      {
        await connection.OpenAsync();
        var command = connection.CreateCommand();
        command.CommandText = "SELECT ID, ParentID, CharacterTechnicalName, TechnicalName, DisplayName, Type, Description, LevelRequirement, MinimumPoints, MaximumPoints FROM Skills WHERE ID = @ID";
        command.Parameters.AddWithValue("@ID", id);
        
        using (var reader = await command.ExecuteReaderAsync())
          if (await reader.ReadAsync())
            skill = new Skill((int) reader["ID"], reader["ParentID"] as int?, (string) reader["CharacterTechnicalName"], (string) reader["TechnicalName"], (string) reader["DisplayName"], SkillType.Ability, (string) reader["Description"], (sbyte) reader["LevelRequirement"], (sbyte) reader["MinimumPoints"], (sbyte) reader["MaximumPoints"]);
      }

      return skill;
    }

    public async Task<Skill> GetSkillByTechnicalName(string characterTechnicalName, string technicalName)
    {
      var skill = (Skill) null;
      using (MySqlConnection connection = GetConnection())
      {
        await connection.OpenAsync();
        var command = connection.CreateCommand();
        command.CommandText = "SELECT ID, ParentID, CharacterTechnicalName, TechnicalName, DisplayName, Type, Description, LevelRequirement, MinimumPoints, MaximumPoints FROM Skills WHERE CharacterTechnicalName = @CharacterTechnicalName AND TechnicalName = @TechnicalName";
        command.Parameters.AddWithValue("@CharacterTechnicalName", characterTechnicalName);
        command.Parameters.AddWithValue("@TechnicalName", technicalName);
        
        using (var reader = await command.ExecuteReaderAsync())
          if (await reader.ReadAsync())
            skill = new Skill((int) reader["ID"], reader["ParentID"] as int?, (string) reader["CharacterTechnicalName"], (string) reader["TechnicalName"], (string) reader["DisplayName"], SkillType.Ability, (string) reader["Description"], (sbyte) reader["LevelRequirement"], (sbyte) reader["MinimumPoints"], (sbyte) reader["MaximumPoints"]);
      }

      return skill;
    }

    public async Task<(string CharacterTechnicalName, string TechnicalName)> CreateSkill(Skill skill)
    {
      var success = false;
      using (MySqlConnection connection = GetConnection())
      {
        await connection.OpenAsync();
        var command = connection.CreateCommand();
        command.CommandText = "INSERT INTO Skills (CharacterTechnicalName, TechnicalName, DisplayName, Type, Description, LevelRequirement, MinimumPoints, MaximumPoints) VALUES (@CharacterTechnicalName, @TechnicalName, @DisplayName, @Type, @Description, @LevelRequirement, @MinimumPoints, @MaximumPoints)";
        command.Parameters.AddWithValue("@CharacterTechnicalName", skill.CharacterTechnicalName);
        command.Parameters.AddWithValue("@TechnicalName", skill.TechnicalName);
        command.Parameters.AddWithValue("@DisplayName", skill.DisplayName);
        command.Parameters.AddWithValue("@Type", skill.Type.ToString());
        command.Parameters.AddWithValue("@Description", skill.Description);
        command.Parameters.AddWithValue("@LevelRequirement", skill.LevelRequirement);
        command.Parameters.AddWithValue("@MinimumPoints", skill.MinimumPoints);
        command.Parameters.AddWithValue("@MaximumPoints", skill.MaximumPoints);

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

      return success ? (skill.CharacterTechnicalName, skill.TechnicalName) : (null, null);
    }

    public async Task<(bool Conflict, string CharacterTechnicalName, string TechnicalName)> UpdateSkill(string characterTechnicalName, string technicalName, Skill skill)
    {
      var success = false;
      var conflict = false;
      using (MySqlConnection connection = GetConnection())
      {
        await connection.OpenAsync();
        var command = connection.CreateCommand();
        command.CommandText = "UPDATE Skills SET ParentID = @ParentID, CharacterTechnicalName = @CharacterTechnicalName, TechnicalName = @TechnicalName, DisplayName = @DisplayName, Type = @Type, Description = @Description, LevelRequirement = @LevelRequirement, MinimumPoints = @MinimumPoints, MaximumPoints = @MaximumPoints WHERE CharacterTechnicalName = @CharacterTechnicalNameFilter AND TechnicalName = @TechnicalNameFilter";
        command.Parameters.AddWithValue("@ParentID", skill.ParentID);
        command.Parameters.AddWithValue("@CharacterTechnicalName", skill.CharacterTechnicalName);
        command.Parameters.AddWithValue("@TechnicalName", skill.TechnicalName);
        command.Parameters.AddWithValue("@DisplayName", skill.DisplayName);
        command.Parameters.AddWithValue("@Type", skill.Type.ToString());
        command.Parameters.AddWithValue("@Description", skill.Description);
        command.Parameters.AddWithValue("@LevelRequirement", skill.LevelRequirement);
        command.Parameters.AddWithValue("@MinimumPoints", skill.MinimumPoints);
        command.Parameters.AddWithValue("@MaximumPoints", skill.MaximumPoints);
        command.Parameters.AddWithValue("@CharacterTechnicalNameFilter", characterTechnicalName);
        command.Parameters.AddWithValue("@TechnicalNameFilter", technicalName);

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
        ? (conflict, skill.CharacterTechnicalName, skill.TechnicalName)
        : (conflict, null, null);
    }

    public async Task<(bool Conflict, bool Success)> DeleteSkill(string characterTechnicalName, string technicalName)
    {
      var success = false;
      var conflict = false;
      using (MySqlConnection connection = GetConnection())
      {
        await connection.OpenAsync();
        var command = connection.CreateCommand();
        command.CommandText = "DELETE FROM Skills  WHERE CharacterTechnicalName = @CharacterTechnicalName AND TechnicalName = @TechnicalName";
        command.Parameters.AddWithValue("@CharacterTechnicalName", characterTechnicalName);
        command.Parameters.AddWithValue("@TechnicalName", technicalName);

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