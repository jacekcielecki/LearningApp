namespace WSBLearn.Application.Constants
{
    public static class Messages
    {
        public const string DataAccessError = "Unable to access database";
        public const string InvalidId = "{0} with given id does not exist";
    }

    public static class CrudMessages
    {
        public const string CreateEntitySuccess = "{0} has been successfully created";
        public const string CreateEntityFailure = "Unable to create new {0} entity";

        public const string UpdateEntitySuccess = "{0} has been successfully updated";
        public const string UpdateEntityFailure = "Unable to update {0} entity";

        public const string DeleteEntitySuccess = "{0} has been successfully deleted";
        public const string DeleteEntityFailure = "Unable to delete {0} entity";
    }
}