namespace WSBLearn.Application.Constants
{
    public static class Messages
    {
        public const string DataAccessError = "Unable to access database";
        public const string InvalidId = "{0} with given id does not exist";
        public const string GenericErrorMessage = "Something went wrong";
        public const string ExistingSubentity = "{0} contains {1}. Delete {1} firts to proceed.";
    }

    public static class CrudMessages
    {
        public const string CreateEntitySuccess = "{0} has been successfully created. Id: {1}";
        public const string CreateEntityFailure = "Unable to create new {0}";

        public const string UpdateEntitySuccess = "{0} has been successfully updated";
        public const string UpdateEntityFailure = "Unable to update {0}";

        public const string DeleteEntitySuccess = "{0} has been successfully deleted";
        public const string DeleteEntityFailure = "Unable to delete {0}";
    }
}