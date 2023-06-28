using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Relationship.Api.Data;
using Relationship.Api.DTOs;
using Relationship.Api.Models;

namespace Relationship.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PersonController : ControllerBase
    {
        private readonly ILogger<PersonController> _logger;
        private readonly DataContext _context;

        public PersonController(ILogger<PersonController> logger, DataContext context)
        {
            _logger = logger;
            _context = context;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CharacterGetDto>> GetCharacterById(int id)
        {
            var character = await _context.Characters
                .Include(c => c.Backpack)
                .Include(c => c.Weapons)
                .Include(c => c.Factions)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (character == null)
                return NotFound("Person not found");

            var result = new CharacterGetDto(character.Name,
                new BackpackCreateDto(character.Backpack.Description),
                new List<WeaponCreateDto>(character.Weapons.Select(w => new WeaponCreateDto(w.Name)).ToList()),
                new List<FactionCreateDto>(character.Factions.Select(w => new FactionCreateDto(w.Name)).ToList()));

            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<List<CharacterGetDto>>> CreateCharacter(CharacterCreateDto request)
        {
            var newCharacter = new Character
            {
                Name = request.Name
            };

            var backpack = new Backpack { Description = request.Backpack.Description, Character = newCharacter };
            var weapons = request.Weapons.Select(w => new Weapon { Name = w.Name, Character = newCharacter }).ToList();
            var factions = request.Factions.Select(f => new Faction { Name = f.Name, Characters = new List<Character> { newCharacter }}).ToList();

            newCharacter.Backpack = backpack;
            newCharacter.Weapons = weapons;
            newCharacter.Factions = factions;

            _context.Characters.Add(newCharacter);
            await _context.SaveChangesAsync();

            return Ok(await _context.Characters
                .Include(c => c.Backpack)
                .Include(c => c.Weapons)
                .Include(c => c.Factions)
                .ToListAsync());
        }
    }
}