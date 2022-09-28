using System.ComponentModel.DataAnnotations;

namespace ERP.Authorization.Users.Dto
{
    public class ChangeUserLanguageDto
    {
        [Required]
        public string LanguageName { get; set; }
    }
}
