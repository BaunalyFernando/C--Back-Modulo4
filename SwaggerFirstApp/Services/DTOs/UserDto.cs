using SwaggerFirstApp.Helpers;

namespace SwaggerFirstApp.Services.DTOs
{
    public class UserDto
    {
        public string Name { get; set; }
        public RoleEnum Role { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

        public int CredentialId { get; set; }

        public string Username { get; set; }
        public string Password { get; set; }
    }
}
