using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PaganOnline.DataManager.API.Models;

namespace PaganOnline.DataManager.API.Controllers
{
  [ApiController, Route("api/[controller]")]
  public class CharacterController : Controller
  {
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Character>>> Index()
    {
      var context = (DataManagerContext) HttpContext.RequestServices.GetService(typeof(DataManagerContext));
      var characters = await context.GetAllCharacters();
      return characters.ToArray();
    }

    [HttpGet, Route("{technicalName}")]
    public async Task<ActionResult<Character>> GetCharacter(string technicalName)
    {
      var context = (DataManagerContext) HttpContext.RequestServices.GetService(typeof(DataManagerContext));
      var character = await context.GetCharacterByTechnicalName(technicalName);
      return character;
    }

    [HttpPost]
    public async Task<ActionResult<string>> CreateCharacter(Character character)
    {
      var context = (DataManagerContext) HttpContext.RequestServices.GetService(typeof(DataManagerContext));
      var technicalName = await context.CreateCharacter(character);
      return technicalName;
    }

    [HttpPut, Route("{technicalName}")]
    public async Task<ActionResult<string>> UpdateCharacter(string technicalName, Character character)
    {
      var context = (DataManagerContext) HttpContext.RequestServices.GetService(typeof(DataManagerContext));
      var updatedTechnicalName = await context.UpdateCharacter(technicalName, character);
      return updatedTechnicalName;
    }

    [HttpDelete, Route("{technicalName}")]
    public async Task<ActionResult<bool>> DeleteChaarcter(string technicalName)
    {
      var context = (DataManagerContext) HttpContext.RequestServices.GetService(typeof(DataManagerContext));
      var deleted = await context.DeleteCharacter(technicalName);
      return deleted;
    }
  }
}