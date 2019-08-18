using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PaganOnline.DataManager.API.Models;

namespace PaganOnline.DataManager.API.Controllers
{
  [ApiController, Route("api/[controller]/{skillID}")]
  public class SkillEffectController : Controller
  {
    private readonly DataManagerContext _context;
    public SkillEffectController(DataManagerContext context)
    {
      _context = context;
    }
    
    [HttpGet]
    public async Task<ActionResult<IEnumerable<SkillEffect>>> Index(int skillID)
    {
      var skills = await _context.GetAllSkillEffects(skillID);
      return Ok(skills.ToArray());
    }

    [HttpGet, Route("{level}/{sequence}")]
    public async Task<ActionResult<Skill>> GetSkillEffect(int skillID, sbyte level, sbyte sequence)
    {
      var skill = await _context.GetSkillEffect(skillID, level, sequence);

      if (skill == null)
        return NotFound($"{skillID}/{level}/{sequence}");
      return Ok(skill);
    }

    [HttpPost]
    public async Task<ActionResult<(string SkillID, sbyte Level, sbyte Sequence)>> CreateSkillEffect(int skillID, SkillEffect skillEffect)
    {
      if (skillEffect != null)
        skillEffect.SkillID = skillID;
      
      if (!SkillEffect.Validate(skillEffect))
        return BadRequest(skillEffect);

      var result = await _context.CreateSkillEffect(skillEffect);

      if (result.SkillID < 1 ||
          result.Level < 1 ||
          result.Sequence < 1)
        return Conflict($"{skillEffect.SkillID}/{skillEffect.Level}/{skillEffect.Sequence}");
      return Created($"skilleffect/{result.SkillID}/{result.Level}/{result.Sequence}", $"skill/{result.SkillID}/{result.Level}/{result.Sequence}");
    }

    [HttpPut, Route("{level}/{sequence}")]
    public async Task<ActionResult> UpdateSkillEffect(int skillID, sbyte level, sbyte sequence, SkillEffect skillEffect)
    {
      if (!SkillEffect.Validate(skillEffect))
        return BadRequest(skillEffect);

      var result = await _context.UpdateSkillEffect(skillID, level, sequence, skillEffect);
      
      if (result.Conflict)
        return Conflict($"{skillEffect.SkillID}/{skillEffect.Level}/{skillEffect.Sequence}");
      if (result.SkillID < 1 ||
          result.Level < 1 ||
          result.Sequence < 1)
        return NotFound($"{skillEffect.SkillID}/{skillEffect.Level}/{skillEffect.Sequence}");
      if (skillID != result.SkillID ||
          level != result.Level ||
          sequence != result.Sequence)
        return Redirect($"skilleffect/{skillEffect.SkillID}/{skillEffect.Level}/{skillEffect.Sequence}");
      return NoContent();
    }
    
    [HttpDelete, Route("{level}/{sequence}")]
    public async Task<ActionResult> DeleteSkill(int skillID, sbyte level, sbyte sequence)
    {
      var result = await _context.DeleteSkillEffect(skillID, level, sequence);

      if (result.Conflict)
        return Conflict($"{skillID}/{level}/{sequence}");
      if (!result.Success)
        return NotFound($"{skillID}/{level}/{sequence}");
      return NoContent();
    }
  }
}