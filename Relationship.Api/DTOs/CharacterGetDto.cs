namespace Relationship.Api.DTOs
{
    public record struct CharacterGetDto(string Name, BackpackCreateDto Backpack,
        List<WeaponCreateDto> Weapons, List<FactionCreateDto> Factions);
}
