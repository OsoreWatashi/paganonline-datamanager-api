using System;

namespace PaganOnline.DataManager.API.Models
{
  public class Skill
  {
    public int ID { get; }
    public int? ParentID { get; }
    public string CharacterTechnicalName { get; set; }
    public string TechnicalName { get; }
    public string DisplayName { get; }
    public SkillType Type { get; }
    public string Description { get; }
    public sbyte LevelRequirement { get; }
    public sbyte MinimumPoints { get; }
    public sbyte MaximumPoints { get; }

    public Skill(int id, int? parentId, string characterTechnicalName, string technicalName, string displayName, SkillType type, string description, sbyte levelRequirement, sbyte minimumPoints, sbyte maximumPoints)
    {
      ID = id;
      ParentID = parentId;
      CharacterTechnicalName = characterTechnicalName;
      TechnicalName = technicalName;
      DisplayName = displayName;
      Type = type;
      Description = description;
      LevelRequirement = levelRequirement;
      MinimumPoints = minimumPoints;
      MaximumPoints = maximumPoints;
    }

    public static bool Validate(Skill skill)
    {
      if (skill == null)
        return false;

      if (skill.ID < 0 ||
          (skill.ParentID != null && skill.ParentID < 0))
        return false;

      if (String.IsNullOrWhiteSpace(skill.CharacterTechnicalName) ||
          String.IsNullOrWhiteSpace(skill.TechnicalName) ||
          String.IsNullOrWhiteSpace(skill.DisplayName) ||
          String.IsNullOrWhiteSpace(skill.Description))
        return false;

      if (skill.Type == SkillType.__ERROR)
        return false;

      if (skill.LevelRequirement < 1 ||
          skill.MinimumPoints < 1 ||
          skill.MaximumPoints < 1 ||
          skill.MinimumPoints > skill.MaximumPoints)
        return false;

      return true;
    }
  }

  public enum SkillType
  {
    __ERROR,
    
    Ability,
    Major,
    Minor
  }
}