namespace BulkyBook.Utility
{
    public static class StaticDetails
    {
        public const string ROLE_USER_INDIVIDUAL = "Individual";
        public const string ROLE_USER_COMPANY = "Company";
        public const string ROLE_ADMIN = "Admin";
        public const string ROLE_EMPLOYEE = "Employee";

        public const string ORDERSTATUS_PENDING = "Pending";
        public const string ORDERSTATUS_APPROVED = "Approved";
        public const string ORDERSTATUS_INPROCESS = "Processing";
        public const string ORDERSTATUS_SHIPPED = "Shipped";
        public const string ORDERSTATUS_CANCELLED = "Cancelled";
        public const string ORDERSTATUS_REFUNDED = "Refunded";

        public const string PAYMENTSTATUS_PENDING = "Pending";
        public const string PAYMENTSTATUS_APPROVED = "Approved";
        public const string PAYMENTSTATUS_DELAYED = "ApprovedForDelayedPayment";
        public const string PAYMENTSTATUS_REJECTED = "Rejected";

        public const string SessionCart = "SessionShoppingCart";
    }
}
