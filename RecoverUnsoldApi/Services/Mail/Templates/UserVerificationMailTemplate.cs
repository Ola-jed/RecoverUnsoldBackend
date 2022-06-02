namespace RecoverUnsoldApi.Services.Mail.Templates;

public static class UserVerificationMailTemplate
{
    public const string Text =
        "RecoverUnsold : Vérification de compte\nVeuillez utiliser le code suivant afin de vérifier votre compte\n{token}Merci.\nRecoverUnsold\nCopyright © 2022 RecoverUnsold";
    
}