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
  }
}