using System;

namespace PaganOnline.DataManager.API.Models
{
  public class SkillEffect
  {
    public int SkillID { get; set; }
    public sbyte Level { get; }
    public sbyte Sequence { get; }
    public string Description { get; }

    public SkillEffect(int skillId, sbyte level, sbyte sequence, string description)
    {
      SkillID = skillId;
      Level = level;
      Sequence = sequence;
      Description = description;
    }

    public static bool Validate(SkillEffect skillEffect)
    {
      if (skillEffect == null)
        return false;

      if (skillEffect.SkillID < 0 ||
          skillEffect.Level < 1 ||
          skillEffect.Sequence < 1)
        return false;

      if (String.IsNullOrWhiteSpace(skillEffect.Description))
        return false;

      return true;
    }
  }
}