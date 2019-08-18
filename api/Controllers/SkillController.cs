using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PaganOnline.DataManager.API.Models;

namespace PaganOnline.DataManager.API.Controllers
{
  [ApiController, Route("api/[controller]/{characterTechnicalName}")]
  public class SkillController : Controller
  {
    private readonly DataManagerContext _context;
    public SkillController(DataManagerContext context)
    {
      _context = context;
    }
    
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Skill>>> Index(string characterTechnicalName)
    {
      var skills = await _context.GetAllSkills(characterTechnicalName);
      return Ok(skills.ToArray());
    }

    [HttpGet, Route("{technicalName}")]
    public async Task<ActionResult<Skill>> GetSkill(string characterTechnicalName, string technicalName)
    {
      var skill = await _context.GetSkillByTechnicalName(characterTechnicalName, technicalName);

      if (skill == null)
        return NotFound($"{characterTechnicalName}/{technicalName}");
      return Ok(skill);
    }

    [HttpPost]
    public async Task<ActionResult<(string CharacterTechnicalName, string TechnicalName)>> CreateSkill(string characterTechnicalName, Skill skill)
    {
      if (skill != null)
        skill.CharacterTechnicalName = characterTechnicalName;
      
      if (!Skill.Validate(skill))
        return BadRequest(skill);
      if (skill.ParentID != null)
      {
        var parentSkill = await _context.GetSkillByID(skill.ParentID.Value);
        if (parentSkill == null || !String.Equals(parentSkill.CharacterTechnicalName, characterTechnicalName))
          return BadRequest(skill);
      }

      var result = await _context.CreateSkill(skill);

      if (String.IsNullOrWhiteSpace(result.CharacterTechnicalName) ||
          String.IsNullOrWhiteSpace(result.TechnicalName))
        return Conflict($"{skill.CharacterTechnicalName}/{skill.TechnicalName}");
      return Created($"skill/{result.CharacterTechnicalName}/{result.TechnicalName}", $"{result.CharacterTechnicalName}/{result.TechnicalName}");
    }

    [HttpPut, Route("{technicalName}")]
    public async Task<ActionResult> UpdateSkill(string characterTechnicalName, string technicalName, Skill skill)
    {
      if (!Skill.Validate(skill))
        return BadRequest(skill);

      var result = await _context.UpdateSkill(characterTechnicalName, technicalName, skill);
      
      if (result.Conflict)
        return Conflict($"{skill.CharacterTechnicalName}/{skill.TechnicalName}");
      if (String.IsNullOrWhiteSpace(result.CharacterTechnicalName) ||
          String.IsNullOrWhiteSpace(result.TechnicalName))
        return NotFound($"{skill.CharacterTechnicalName}/{skill.TechnicalName}");
      if (!String.Equals(characterTechnicalName, result.CharacterTechnicalName) ||
          !String.Equals(technicalName, result.TechnicalName))
        return Redirect($"skill/{skill.CharacterTechnicalName}/{skill.TechnicalName}");
      return NoContent();
    }
    
    [HttpDelete, Route("{technicalName}")]
    public async Task<ActionResult> DeleteSkill(string characterTechnicalName, string technicalName)
    {
      var result = await _context.DeleteSkill(characterTechnicalName, technicalName);

      if (result.Conflict)
        return Conflict($"{characterTechnicalName}/{technicalName}");
      if (!result.Success)
        return NotFound($"{characterTechnicalName}/{technicalName}");
      return NoContent();
    }
  }
}