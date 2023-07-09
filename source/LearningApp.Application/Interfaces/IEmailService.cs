namespace LearningApp.Application.Interfaces
{
    public interface IEmailService
    { 
        Task SendAccountVerificationEmail(string userEmail);
    }
}
