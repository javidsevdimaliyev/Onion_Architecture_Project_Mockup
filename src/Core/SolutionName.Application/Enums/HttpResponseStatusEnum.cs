using Application.Shared.Resources;
using SolutionName.Application.Attributes;

namespace SolutionName.Application.Enums
{
    public enum HttpResponseStatusEnum
    {
        Default = 0,

        #region System exceptions [0 - 1000]

        //[LocalizedDescription("Success", typeof(SharedResources))]
        //Success = 200,

        [LocalizedDescription("Exception", typeof(SharedResources))]
        Exception = 500,

        [LocalizedDescription("Failure", typeof(SharedResources))]
        Failure = 600,

        [LocalizedDescription("Permission", typeof(SharedResources))]
        Permission = 403,

        [LocalizedDescription("NotFound", typeof(SharedResources))]
        NotFound = 404,

        [LocalizedDescription("NoContent", typeof(SharedResources))]
        NoContent = 204,

        [LocalizedDescription("OperationCancelled", typeof(SharedResources))]
        OperationCancelled = 410,

        [LocalizedDescription("Unauthorized", typeof(SharedResources))]
        Unauthorized = 999,

        #endregion

        #region Custom exceptions [1000 - 3000]

        [LocalizedDescription("PermissionDenied", typeof(SharedResources))]
        PermissionDenied = 1000,

        [LocalizedDescription("BindError", typeof(SharedResources))]
        BindError = 1001,

        [LocalizedDescription("ValidationError", typeof(SharedResources))]
        ValidationError = 1003,



        [LocalizedDescription("ObjectNotFound", typeof(SharedResources))]
        ObjectNotFound = 1992,

        [LocalizedDescription("UserNotFound", typeof(SharedResources))]
        UserNotFound = 1993,

        [LocalizedDescription("InvalidPassword", typeof(SharedResources))]
        InvalidPassword = 1994,

        [LocalizedDescription("InvalidPassword", typeof(SharedResources))]
        InvalidOTPCode = 1995,

        [LocalizedDescription("UserAlreadyLoggedIn", typeof(SharedResources))]
        UserAlreadyLoggedIn = 1996,

        [LocalizedDescription("UserCertificateIsInvalid", typeof(SharedResources))]
        UserCertificateIsInvalid = 1997,

        [LocalizedDescription("Locked", typeof(SharedResources))]
        Locked = 1998,

        [LocalizedDescription("RefreshTokenExpired", typeof(SharedResources))]
        RefreshTokenExpired = 1999,

        [LocalizedDescription("RefreshTokenNotFound", typeof(SharedResources))]
        RefreshTokenNotFound = 2000,

        [LocalizedDescription("InvalidAccessToken", typeof(SharedResources))]
        InvalidAccessToken = 2001,

        [LocalizedDescription("RefreshTokenIsUsed", typeof(SharedResources))]
        RefreshTokenIsUsed = 2002,

        [LocalizedDescription("RefreshTokenIsRevoked", typeof(SharedResources))]
        RefreshTokenIsRevoked = 2003,

        [LocalizedDescription("PasswordIsUsed", typeof(SharedResources))]
        PasswordIsUsed = 2004,

        [LocalizedDescription("InvalidAsanLoginToken", typeof(SharedResources))]
        InvalidAsanLoginToken = 2005,

        [LocalizedDescription("DomainUserNotFound", typeof(SharedResources))]
        DomainUserNotFound = 2006,

        [LocalizedDescription("EmailNotConfirmed", typeof(SharedResources))]
        EmailNotConfirmed = 2007,

        [LocalizedDescription("PhoneNumberNotConfirmed", typeof(SharedResources))]
        PhoneNumberNotConfirmed = 2008,

        [LocalizedDescription("RequireResetPassword", typeof(SharedResources))]
        RequireResetPassword = 2009,

        [LocalizedDescription("TwoFactorAuthenticationNotEnabled", typeof(SharedResources))]
        TwoFactorAuthenticationNotEnabled = 2010,

        [LocalizedDescription("RequireTwoFactor", typeof(SharedResources))]
        RequireTwoFactor = 2011,

        [LocalizedDescription("InvalidToken", typeof(SharedResources))]
        InvalidToken = 2012,

        [LocalizedDescription("ResetPasswordDoesNotRequired", typeof(SharedResources))]
        ResetPasswordDoesNotRequired = 2013,

        [LocalizedDescription("AsanSignVerificateAuthenticateExpire", typeof(SharedResources))]
        AsanSignVerificateAuthenticateExpire = 2014,

        [LocalizedDescription("AccountNotConfirmed", typeof(SharedResources))]
        AccountNotConfirmed = 2015,

        [LocalizedDescription("CreatedAccountSuccessfullyPleaseWaitForConfirmation", typeof(SharedResources))]
        CreatedAccountSuccessfullyPleaseWaitForConfirmation = 2016,

        [LocalizedDescription("InvalidDirectory", typeof(SharedResources))]
        InvalidDirectory = 2013,
        #endregion

        #region Database Exception [3000-5000]

        [LocalizedDescription("DbException", typeof(SharedResources))]
        DbException = 3000,

        [LocalizedDescription("UniqeKeyException", typeof(SharedResources))]
        UniqeKeyException = 3001,

        [LocalizedDescription("DuplicateKeyException", typeof(SharedResources))]
        DuplicateKeyException = 3002,

        [LocalizedDescription("ForeignKeyException", typeof(SharedResources))]
        ForeignKeyException = 3003,

        [LocalizedDescription("ReferfenceConstraintException", typeof(SharedResources))]
        ReferfenceConstraintException = 3004,
        #endregion

        #region Global exceptions [30000 - 40000]

        [LocalizedDescription("LoadResultError", typeof(SharedResources))]
        LoadResultError = 30000,

        [LocalizedDescription("FileNotSave", typeof(SharedResources))]
        FileNotSave = 30001,

        [LocalizedDescription("FileNotFound", typeof(SharedResources))]
        FileNotFound = 30002,

        [LocalizedDescription("FileReadError", typeof(SharedResources))]
        FileReadError = 30003,

        [LocalizedDescription("NotValid", typeof(SharedResources))]
        NotValid = 30004,

        [LocalizedDescription("PropertyIsNull", typeof(SharedResources))]
        PropertyIsNull = 30005,

        [LocalizedDescription("PropertyAlreadyExists", typeof(SharedResources))]
        PropertyAlreadyExists = 30006,

        [LocalizedDescription("IdNotExists", typeof(SharedResources))]
        IdNotExists = 30007,

        [LocalizedDescription("RowAlreadyExists", typeof(SharedResources))]
        RowAlreadyExists = 30008,

        [LocalizedDescription("FileContentIsEmpty", typeof(SharedResources))]
        FileContentIsEmpty = 30009,

        [LocalizedDescription("NoPermission", typeof(SharedResources))]
        NoPermission = 30010,

        [LocalizedDescription("IncorrectSyntax", typeof(SharedResources))]
        IncorrectSyntax = 30011,

        [LocalizedDescription("MoreColumnCount", typeof(SharedResources))]
        MoreColumnCount = 30012,

        [LocalizedDescription("MissingParameter", typeof(SharedResources))]
        MissingParameter = 30013,

        [LocalizedDescription("FilterFieldNotValid", typeof(SharedResources))]
        FilterFieldNotValid = 30014,
        [LocalizedDescription("FilterDateFieldNotValid", typeof(SharedResources))]
        FilterDateFieldNotValid = 30015,

        [LocalizedDescription("UserMustBeAddedByDirector", typeof(SharedResources))]
        UserMustBeAddedByDirector = 30016,

        #endregion

    }
}
