using System;
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
    private readonly DataManagerContext _context;
    public CharacterController(DataManagerContext context)
    {
      _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Character>>> Index()
    {
      var characters = await _context.GetAllCharacters();
      return Ok(characters.ToArray());
    }

    [HttpGet, Route("{technicalName}")]
    public async Task<ActionResult<Character>> GetCharacter(string technicalName)
    {
      var character = await _context.GetCharacterByTechnicalName(technicalName);

      if (character == null)
        return NotFound(technicalName);
      return Ok(character);
    }

    [HttpPost]
    public async Task<ActionResult<string>> CreateCharacter(Character character)
    {
      if (!Character.Validate(character))
        return BadRequest(character);

      var technicalName = await _context.CreateCharacter(character);

      if (String.IsNullOrWhiteSpace(technicalName))
        return Conflict(character.TechnicalName);
      return Created($"/character/{technicalName}", technicalName);
    }

    [HttpPut, Route("{technicalName}")]
    public async Task<ActionResult> UpdateCharacter(string technicalName, Character character)
    {
      if (!Character.Validate(character))
        return BadRequest(character);

      var result = await _context.UpdateCharacter(technicalName, character);

      if (result.Conflict)
        return Conflict(character.TechnicalName);
      if (String.IsNullOrWhiteSpace(result.TechnicalName))
        return NotFound(technicalName);
      if (!String.Equals(technicalName, result.TechnicalName))
        return Redirect($"/character/{result.TechnicalName}");
      return NoContent();
    }

    [HttpDelete, Route("{technicalName}")]
    public async Task<ActionResult> DeleteCharacter(string technicalName)
    {
      var result = await _context.DeleteCharacter(technicalName);

      if (result.Conflict)
        return Conflict(technicalName);
      if (!result.Success)
        return NotFound(technicalName);
      return NoContent();
    }
  }
}