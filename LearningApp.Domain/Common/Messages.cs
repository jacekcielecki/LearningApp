namespace LearningApp.Domain.Common
{
    public static class Messages
    {
        public const string InvalidLevel = "Given level is invalid";
        public const string InvalidGainedExperience = "Specified gained experience need to be between 0 and 20";
        public const string QuizNotStarted = "User with given id has not yet started any quiz in this category";
        public const string GenericErrorMessage = "Something went wrong";
        public const string AuthorizationFailed = "Invalid email or password";
        public const string InvalidPassword = "Invalid Password";
        public const string AccessProtectedResource = "Action forbidden, resource is protected";
        public static string InvalidId(string entityName) => $"{entityName} with given id not found.";
    }

    public static class ValidationMessages
    {
        public const string InvalidUrl = "Field is not empty and not a valid fully-qualified http, https or ftp URL";
        public const string CorrectAnswerOutOfRange = "CorrectAnswer must be either a, b, c or d";
        public const string LevelOutOfRange = "Level must be either 1, 2 or 3";
        public static string AnswerRequired(char answer) => $"Answer '{answer}' need to be specified when set as the CorrectAnswer";
        public static string UsernameTaken(string username) => $"User with \"{username}\" username already exists";
        public static string EmailAddressTaken(string email) => $"User with \"{email}\" email address already exists";
    }

    public static class BlobStorageMessages
    {
        public const string InvalidImageExtension = "Provided image must have \".jpg\", \".png\" or \".svg\" file extension";
        public const string InvalidContainer = "Container with given name does not exist";
        public static string FileUploadedSuccessfully(string fileName) => $"File {fileName} has been successfully uploaded";
        public static string FileDeletedSuccessfully(string fileName) => $"File: {fileName} has been successfully deleted.";
        public static string FileNameTaken(string fileName, string containerName) => $"File with name {fileName} already exists in container. Set another name to store the file in the container: '{containerName}.'";
        public static string UnhandledException(string stackTrace, string message) => $"Unhandled Exception. ID: {stackTrace} - Message: {message}";
        public static string FileNotFound(string fileName) => $"File {fileName} was not found.";
    }
}
