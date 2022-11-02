namespace WSBLearn.Application.Requests
{
    public class UpdateUserRequest
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string EmailAddress { get; set; } = string.Empty;
        public int RoleId { get; set; }
    }
}
