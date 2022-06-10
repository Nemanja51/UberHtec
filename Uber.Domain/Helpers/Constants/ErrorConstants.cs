namespace Uber.Domain.Helpers.Constants
{
    public static class ErrorConstants
    {
        public static string IncorectCredentials = "Credentials are incorrect";
        public static string ModelStateNotValid = "Model state not valid!";
        public static string UserAlreadyExist = "User credentials already exist!";
        public static string RoleRequired = "When registrating new user, please write ROLE!";
        public static string ExistingRoles = "Role must be driver, passanger or admin!";
        public static string RequerdFieldsForDriver = "License plate and Vehicle brand are mandatory fields!";
        public static string ErrorWhileRegistrating = "Error while registrating!";
        public static string DriverStateError = "Only driver can change availability!";
    }
}
