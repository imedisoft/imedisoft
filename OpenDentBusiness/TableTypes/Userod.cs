using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using Imedisoft.Data.Annotations;

namespace OpenDentBusiness
{
    /// <summary>
    /// (User OD since "user" is a reserved word)
    /// Users are a completely separate entity from Providers and Employees even though they can be linked.
    /// A usernumber can never be changed, ensuring a permanent way to record database entries and leave an audit trail.
    /// A user can be a provider, employee, or neither.
    /// </summary>
    [Table]
    public class Userod : TableBase
    {
        [PrimaryKey]
        [Column(Name = "UserNum")]
        public long Id;

        public string UserName;

        ///<summary>The password details in a "HashType$Salt$Hash" format, separating the different fields by '$'.
        ///This is NOT the actual password but the encoded password hash.
        ///If the contents of this variable are not in the aforementioned format, it is assumed to be a legacy password hash (MD5).</summary>
        public string Password;

        ///<summary>Deprecated. Use UserGroupAttaches to link Userods to UserGroups.</summary>
        public long UserGroupNum;

        ///<summary>FK to employee.EmployeeNum. Cannot be used if provnum is used. Used for timecards to block access by other users.</summary>
        public long EmployeeNum;

        /// <summary>
        /// FK to clinic.ClinicNum.
        /// Default clinic for this user.
        /// It causes new patients to default to this clinic when entered by this user.  
        /// If 0, then user has no default clinic or default clinic is HQ if clinics are enabled.
        /// </summary> 		
        public long ClinicNum;

        /// <summary>
        /// FK to provider.ProvNum.
        /// Cannot be used if EmployeeNum is used.
        /// It is possible to have multiple userods attached to a single provider.
        /// </summary>
        public long ProvNum;

        /// <summary>
        /// Set true to hide user from login list.
        /// </summary>
        public bool IsHidden;

        ///<summary>FK to tasklist.TaskListNum.  0 if no inbox setup yet.  It is assumed that the TaskList is in the main trunk, but this is not strictly enforced.  User can't delete an attached TaskList, but they could move it.</summary>
        public long TaskListInBox;

        /// <summary> Defaults to 3 (regular user) unless specified. Helps populates the Anesthetist, Surgeon, Assistant and Circulator dropdowns properly on FormAnestheticRecord/// </summary>
        public int AnesthProvType;

        ///<summary>If set to true, the BlockSubsc button will start out pressed for this user.</summary>
        public bool DefaultHidePopups;

        ///<summary>Gets set to true if strong passwords are turned on, and this user changes their password to a strong password.  We don't store actual passwords, so this flag is the only way to tell.</summary>
        public bool PasswordIsStrong;

        ///<summary>When true, prevents user from having access to clinics that are not in the corresponding userclinic table.
        ///Many places throughout the program will optionally remove the 'All' option from this user when true.</summary>
        public bool ClinicIsRestricted;

        ///<summary>If set to true, the BlockInbox button will start out pressed for this user.</summary>
        public bool InboxHidePopups;

        ///<summary>FK to userod.UserNum.  The user num within the Central Manager database.  Only editable via CEMT.  Can change when CEMT syncs.</summary>
        public long UserNumCEMT;

        ///<summary>The date and time of the most recent log in failure for this user.  Set to MinValue after user logs in successfully.</summary>
        public DateTime DateTFail;

        ///<summary>The number of times this user has failed to log into their account.  Set to 0 after user logs in successfully.</summary>
        public byte FailedAttempts;

        /// <summary>The username for the ActiveDirectory user to link the account to.</summary>
        public string DomainUser;

        ///<summary>Boolean.  If true, the user's password needs to be reset on next login.</summary>
        public bool IsPasswordResetRequired;

        ///<summary>A hashed pin that is used for mobile web validation on eClipboard. Not used in OD proper.</summary>
        public string MobileWebPin;

        ///<summary>The number of attempts the mobile web pin has failed. Reset on successful attempt.</summary>
        public byte MobileWebPinFailedAttempts;

        ///<summary>Minimum date if last login date and time is unknown.
        ///Otherwise contians the last date and time this user successfully logged in.</summary>
        public DateTime DateTLastLogin;

        ///<summary>The getter will return a struct created from the database-ready password which is stored in the Password field.
        /// The setter will manipulate the Password variable to the string representation of this PasswordContainer object.</summary>
        public PasswordContainer LoginDetails
        {
            get => Authentication.DecodePass(Password);
            set => Password = value.ToString();
        }

        /// <summary>
        /// The password hash, not the actual password. 
        /// If no password has been entered, then this will be blank.
        /// </summary>
        public string PasswordHash => LoginDetails.Hash;

        ///<summary>All valid users should NOT set this value to anything other than None otherwise permission checking will act unexpectedly.
        ///Programmatically set this value from the init method of the corresponding eService.  Helps prevent unhandled exceptions.
        ///Custom property only meant to be used via eServices.  Not a column in db.  Not to be used in middle tier environment.</summary>
        [XmlIgnore]
        public EServiceTypes EServiceType { get; set; }

        public Userod Copy() 
            => (Userod)MemberwiseClone();

        public override string ToString() 
            => UserName;

        public bool IsInUserGroup(long userGroupNum) 
            => Userods.IsInUserGroup(Id, userGroupNum);

        /// <summary>
        /// Gets all of the usergroups attached to this user.
        /// </summary>
        public List<UserGroup> GetGroups(bool includeCEMT = false) => UserGroups.GetForUser(Id, includeCEMT).ToList();
    }

    public enum EServiceTypes
    {
        /// <summmary>
        /// Not an eService user. 
        /// All valid users should be this type otherwise permission checking will act differently.
        /// </summmary>
        None,

        EConnector,
        Broadcaster,
        BroadcastMonitor,
        ServiceMainHQ,
    }
}
