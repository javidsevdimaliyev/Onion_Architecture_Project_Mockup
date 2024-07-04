using Application.Shared.Resources;
using SolutionName.Application.Attributes;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SolutionName.Application.Enums;

public enum HttpResponseStatus
{
    #region System exceptions [0 - 1000]
    [Description("Successfully executed.")]
    Success = 200,
    [Description("An error occurred.")]
    Exception = 500,

    [Description("Validation error occurred.")]
    ValidationError = 1003,

    [Description("Unable to process the request.")]
    Failure = 600,

    [Description("Session has expired. Please log in again.")]
    Unauthorized = 3131,

    [Description("Session has expired. Please log in again.:ExpiredRefreshToken")]
    ExpiredRefreshToken = 3132,

    [Description("Session has expired. Please log in again.:NotFoundRefreshToken")]
    NotFoundRefreshToken = 3133,

    [Description("RemoteIpAddress in JWT is different from the client's RemoteIpAddress.:DifferentRemoteIP")]
    DifferentRemoteIP = 3134,

    [Description("You do not have permission to perform this operation.")]
    Permission = 403,
    [Description("URL not found.")] NotFound = 404,

    [Description("No content available at this URL.")]
    NoContent = 204,
    [Description("Please try again.")] TryAgain = 0,

    [Description("No results found matching the selected search parameters.")]
    NoContentBySearchParams = 405,


    [Description("Request was cancelled - Client Closed Request.")]
    OperationCancelled = 499,

    #endregion

    #region Custom exceptions [1000 - 20000]

    [Description("Permission has been denied.")]
    PermissionDenied = 1000,

    [Description("An error occurred due to missing data.")]
    BindError = 1001,

    [Description("User not found.")]
    UserNotFound = 1993,

    [Description("User information is incorrect.")]
    InvalidPassword = 1994,

    [Description("This user is already logged in.")]
    UserAlreadyLoggedIn = 1995,

    [Description("Invalid Access Token!.")]
    InvalidAccessToken = 2001,

    [Description("Filter field is not valid.")]
    FilterFieldNotValid = 2002,

    [Description("Parameter is missing")]
    MissingParameter = 2003,
    [Description("Domain User not found.")]
    DomainUserNotFound = 2004,
    #endregion

    #region Database Exception [20000-30000]

    [LocalizedDescription("DbException", typeof(SharedResources))]
    DbException = 20000,

    [LocalizedDescription("UniqeKeyException", typeof(SharedResources))]
    UniqeKeyException = 20001,

    [LocalizedDescription("DuplicateKeyException", typeof(SharedResources))]
    DuplicateKeyException = 20002,

    [LocalizedDescription("ForeignKeyException", typeof(SharedResources))]
    ForeignKeyException = 20003,

    [LocalizedDescription("ReferfenceConstraintException", typeof(SharedResources))]
    ReferfenceConstraintException = 20004,
    #endregion

    #region Global exceptions [30000 - 40000]

    [Description("Error in search filters.")]
    LoadResultError = 30000,

    [Description("File not saved.")]
    FileNotSave = 30001,

    [Description("File not found.")]
    FileNotFound = 30002,

    [Description("Error occurred while reading the file.")]
    FileReadError = 30003,

    [Description("At least one item must be selected.")]
    ElementNotSelected = 30004,

    [Description("Data cannot be empty.")]
    ListIsNull = 30005,

    [Description("Data cannot be empty.")]
    StringIsNull = 30006,

    [Description("Data cannot be empty.")]
    ObjectIsNull = 30007,

    [Description("Data cannot be empty.")]
    NumberIsNull = 30008,

    [Description("Data cannot be empty.")]
    ImageIsNull = 30009,

    [Description("Data cannot be empty.")]
    ElementInCollectionIsNull = 30010,

    [Description("No items found in the collection.")]
    ItemsNotFound = 30011,

    [Description("Date cannot be empty.")]
    DateIsNull = 30012,

    [Description("Field cannot be empty.")]
    FieldIsNull = 30013,

    [Description("Document not found.")]
    DocumentNotFound = 30014,

    [Description("Field is not filled correctly.")]
    FieldIsIncorrect = 30015,

    [Description("No data found for the given key.")]
    ValueNotFoundByKey = 30016,

    [Description("Participants not found.")]
    PersonsIsEmpty = 30017,

    [Description("Property information not found.")]
    PropertyInformationIsEmpty = 30018,

    [Description("Printing id not selected.")]
    PrintingIdEmpty = 30019,

    [Description("Minimum 1 day expiration date must be selected.")]
    ExpireDateMustOneDay = 30020,

    [Description("Property right not found.")]
    PropertyInformationProprietaryRightIsEmpty = 30021,

    [Description("Persons not added.")]
    PersonsIdIsEmpty = 30022,

    [Description("Directory not found! - (Directory)")]
    InvalidDirectory = 30023,

    [Description("Invalid document signature found!")]
    NotValidSignature = 30024,

    [Description("Error occurred while reading the file.")]
    FileNameReadError = 30025,

    [Description("No signatures found for the sent document.")]
    DocumentSignaturesNotFound = 30026,

    [Description("No 'Excel' files found for the sent document.")]
    DocumentExcelsNotFound = 30027,

    [Description("Unable to parse the total amount.")]
    UnableToParseTotalAmount = 30028,

    [Description("The 'Total' values in the Excel documents are different.")]
    TheTotalValuesInTheExcelDocumentsAreDifferent = 30029,

    [Description("Document type for payment not found.")]
    DocumentTypeNotFound = 30030,

    [Description("Header row not found. Column names are incorrect.")]
    ExcelHeaderRowNotFound = 30031,

    [Description("Setting not found.")]
    GeneralSettingNotFound = 30032,

    [Description("From-to dates must be entered.")]
    FromToDatesRequired = 30033,

    [Description("PropertyIsNull")]
    PropertyIsNull = 30033,

    #endregion
}