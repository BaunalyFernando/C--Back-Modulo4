using SwaggerFirstApp.Helpers;

namespace SwaggerFirstApp.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public RoleEnum Role { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

        public int CredentialId { get; set; }
        public Credential Credential { get; set; }

    }
}
