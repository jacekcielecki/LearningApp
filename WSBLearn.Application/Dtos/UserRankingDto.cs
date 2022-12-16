namespace WSBLearn.Application.Dtos
{
    public class UserRankingDto
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string EmailAddress { get; set; }
        public string ProfilePictureUrl { get; set; }
        public int ExperiencePoints { get; set; }
        public int Level { get; set; }
    }
}
