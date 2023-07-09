using WebApi.Entities;

namespace WebApi.DTOs
{
    public class PlayerDTO
    {
        public string Name { get; set; }
        public string Position { get; set; }
        public List<PlayerSkillDTO> PlayerSkills { get; set; }
    }
}
