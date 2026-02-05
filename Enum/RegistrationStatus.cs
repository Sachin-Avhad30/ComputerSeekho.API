namespace ComputerSeekho.API.Enums
{
    /// <summary>
    /// Represents the current status of a student's registration and payment
    /// </summary>
    public enum RegistrationStatus
    {
        /// <summary>
        /// Student has only inquired, not yet registered
        /// Use this for enquiry tracking
        /// </summary>
        Enquired = 0,

        /// <summary>
        /// Student has registered but hasn't made any payment yet
        /// IMPORTANT: This is the default status when a new student record is created
        /// This solves the "ghost student" problem - we know they registered but didn't pay
        /// </summary>
        PaymentPending = 1,

        /// <summary>
        /// Student has made at least one payment (can be partial)
        /// They are now an active student in the system
        /// Change to this status after the FIRST payment is received
        /// </summary>
        Active = 2,

        /// <summary>
        /// Student has completed full payment
        /// Optional: Use this if you want to differentiate between active and fully paid
        /// </summary>
        FullyPaid = 3,

        /// <summary>
        /// Student has dropped out or stopped attending
        /// But their record and payment history is preserved
        /// </summary>
        Inactive = 4,

        /// <summary>
        /// Student has successfully completed the course
        /// </summary>
        Completed = 5,

        /// <summary>
        /// Student registration was cancelled before any payment
        /// </summary>
        Cancelled = 6
    }
}