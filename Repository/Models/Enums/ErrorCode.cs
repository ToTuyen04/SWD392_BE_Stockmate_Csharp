namespace Repository.Models.Enums
{
    public enum ErrorCode
    {
        UNCATEGORIZED_EXCEPTION = 9999,
        USER_EXIST = 1001,
        EMAIL_EXIST = 1002,
        USERNAME_INVALID = 1003,
        EMAIL_INVALID = 1004,
        PASSWORD_INVALID = 1005,
        EMAIL_NOT_EXIST = 1006,
        USER_NOT_EXIST = 1007,
        UNAUTHENTICATED = 1008,
        ROLE_NOT_FOUND = 1009,
        PRODUCT_CODE_EXIST = 1010,
        INVALID_STATUS = 1011,
        FULLNAME_REQUIRED = 1012,
        WAREHOUSE_NOT_FOUND = 1013,
        PRODUCT_NOT_FOUND = 1014,
        STOCK_CHECK_PRODUCTS_NOT_FOUND = 1015,
        UNKNOWN_ERROR = 1016,
        STOCK_CHECK_NOTE_NOT_FOUND = 1017,
        USER_CODE_EXIST = 1018,
        TRANSACTION_NOT_FOUND = 1022,
        TRANSACTION_CANNOT_BE_MODIFIED = 1023,
        TRANSACTION_CANNOT_BE_FINALIZED = 1024,
        INSUFFICIENT_STOCK = 1025,
        WAREHOUSE_REQUIRED = 1026,
        NOTE_ITEMS_NOT_FOUND = 1027,
        CAN_NOT_SYSTEM = 1028,
        NOT_ENOUGH_QUANTITY = 1029,
        INVALID_SOURCE_TYPE = 1030,
        INVALID_TRANSACTION_TYPE = 1031,
        STOCK_CHECK_NOTE_INVALID = 1032,
        STOCK_CHECK_CANNOT_BE_MODIFIED = 1033,
        STOCK_CHECK_CANNOT_BE_FINALIZED = 1034,
        CATEGORY_CODE_EXIST = 1035,
        CATEGORY_NOT_FOUND = 1036,
        PRODUCT_TYPE_CODE_EXIST = 1037,
        PRODUCT_TYPE_NOT_FOUND = 1038,
        UNAUTHORIZED_ACTION = 1039,
        INVALID_OPERATION = 1040
    }

    public static class ErrorCodeExtensions
    {
        public static string GetMessage(this ErrorCode errorCode)
        {
            return errorCode switch
            {
                ErrorCode.UNCATEGORIZED_EXCEPTION => "Uncategorized exception",
                ErrorCode.USER_EXIST => "User existed",
                ErrorCode.EMAIL_EXIST => "Email existed",
                ErrorCode.USERNAME_INVALID => "Username must be at least 3 characters",
                ErrorCode.EMAIL_INVALID => "Email must be end with .@gmail.com",
                ErrorCode.PASSWORD_INVALID => "Password must be at least 8 characters",
                ErrorCode.EMAIL_NOT_EXIST => "User is not exist",
                ErrorCode.USER_NOT_EXIST => "User not found",
                ErrorCode.UNAUTHENTICATED => "Unauthenticated",
                ErrorCode.ROLE_NOT_FOUND => "Role not found",
                ErrorCode.PRODUCT_CODE_EXIST => "Product is exist",
                ErrorCode.INVALID_STATUS => "Invalid status",
                ErrorCode.FULLNAME_REQUIRED => "Fullname must not blank",
                ErrorCode.WAREHOUSE_NOT_FOUND => "Warehouse not found",
                ErrorCode.PRODUCT_NOT_FOUND => "Product not found",
                ErrorCode.STOCK_CHECK_PRODUCTS_NOT_FOUND => "Stock record not found for this product",
                ErrorCode.UNKNOWN_ERROR => "Unknown error",
                ErrorCode.STOCK_CHECK_NOTE_NOT_FOUND => "Stock check note not found",
                ErrorCode.USER_CODE_EXIST => "User code is exist",
                ErrorCode.TRANSACTION_NOT_FOUND => "Stock exchange note not found",
                ErrorCode.TRANSACTION_CANNOT_BE_MODIFIED => "Stock exchange note can not be modified",
                ErrorCode.TRANSACTION_CANNOT_BE_FINALIZED => "Stock exchange note can not be finalized",
                ErrorCode.INSUFFICIENT_STOCK => "Not enough stock",
                ErrorCode.WAREHOUSE_REQUIRED => "Warehouse is required",
                ErrorCode.NOTE_ITEMS_NOT_FOUND => "Note item not found",
                ErrorCode.CAN_NOT_SYSTEM => "SYSTEM only used for TRANSFER",
                ErrorCode.NOT_ENOUGH_QUANTITY => "Not enough quantity",
                ErrorCode.INVALID_SOURCE_TYPE => "Source type is incorrect",
                ErrorCode.INVALID_TRANSACTION_TYPE => "Transaction type are: IMPORT, EXPORT, TRANSFER",
                ErrorCode.STOCK_CHECK_NOTE_INVALID => "Stock check note is invalid",
                ErrorCode.STOCK_CHECK_CANNOT_BE_MODIFIED => "Stock check note cannot be modified",
                ErrorCode.STOCK_CHECK_CANNOT_BE_FINALIZED => "Stock check note cannot be finalized",
                ErrorCode.CATEGORY_CODE_EXIST => "Category code is exist",
                ErrorCode.CATEGORY_NOT_FOUND => "Category code is not found",
                ErrorCode.PRODUCT_TYPE_CODE_EXIST => "Product type is exist",
                ErrorCode.PRODUCT_TYPE_NOT_FOUND => "Product type is not found",
                ErrorCode.UNAUTHORIZED_ACTION => "You are not authorized to perform this action",
                ErrorCode.INVALID_OPERATION => "Invalid operation",
                _ => "Unknown error"
            };
        }
    }
}
