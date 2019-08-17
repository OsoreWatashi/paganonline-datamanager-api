using System;

namespace PaganOnline.DataManager.API.Models
{
  public class Character
  {
    public string TechnicalName { get; }
    public string DisplayName { get; }

    public Character(string technicalName, string displayName)
    {
      TechnicalName = technicalName;
      DisplayName = displayName;
    }

    public static bool Validate(Character character)
    {
      if (character == null)
        return false;

      if (String.IsNullOrWhiteSpace(character.TechnicalName) ||
          String.IsNullOrWhiteSpace(character.DisplayName))
        return false;

      return true;
    }
  }
}