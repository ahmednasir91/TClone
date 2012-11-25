using System;
using System.Linq;
using System.Web.Security;
using CodeFirstMembershipSharp;
using TwitterClone.Context;
using TwitterClone.Entities;

namespace TwitterClone.Membership
{
    public class CodeFirstMembershipProvider : MembershipProvider
    {

        #region Properties

        public override string ApplicationName
        {
            get
            {
                return GetType().Assembly.GetName().Name;
            }
            set
            {
                if (value == null) throw new ArgumentNullException("value");
                ApplicationName = GetType().Assembly.GetName().Name;
            }
        }

        public override int MaxInvalidPasswordAttempts
        {
            get { return 5; }
        }

        public override int MinRequiredNonAlphanumericCharacters
        {
            get { return 0; }
        }

        public override int MinRequiredPasswordLength
        {
            get { return 6; }
        }

        public override int PasswordAttemptWindow
        {
            get { return 0; }
        }

        public override MembershipPasswordFormat PasswordFormat
        {
            get { return MembershipPasswordFormat.Hashed; }
        }

        public override string PasswordStrengthRegularExpression
        {
            get { return String.Empty; }
        }

        public override bool RequiresUniqueEmail
        {
            get { return true; }
        }

        #endregion

        #region Functions

        public override MembershipUser CreateUser(string username, string password, string email, string passwordQuestion, string passwordAnswer, bool isApproved, object providerUserKey, out MembershipCreateStatus status)
        {
            if (string.IsNullOrEmpty(username))
            {
                status = MembershipCreateStatus.InvalidUserName;
                return null;
            }
            if (string.IsNullOrEmpty(password))
            {
                status = MembershipCreateStatus.InvalidPassword;
                return null;
            }
            if (string.IsNullOrEmpty(email))
            {
                status = MembershipCreateStatus.InvalidEmail;
                return null;
            }

            string HashedPassword = Crypto.HashPassword(password);
            if (HashedPassword.Length > 128)
            {
                status = MembershipCreateStatus.InvalidPassword;
                return null;
            }

            using (DataContext Context = new DataContext())
            {
                if (Context.Users.Any(Usr => Usr.Username == username))
                {
                    status = MembershipCreateStatus.DuplicateUserName;
                    return null;
                }

                if (Context.Users.Any(Usr => Usr.Email == email))
                {
                    status = MembershipCreateStatus.DuplicateEmail;
                    return null;
                }

                User NewUser = new User
                {
                    Username = username,
                    Password = HashedPassword,
                    Email = email,
                    FullName = passwordQuestion,
                };

                Context.Users.Add(NewUser);
                Context.SaveChanges();
                status = MembershipCreateStatus.Success;
                return new MembershipUser(System.Web.Security.Membership.Provider.Name, NewUser.Username, NewUser.Email, null, null, null, true, false, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now);
            }
        }

        public override bool ValidateUser(string username, string password)
        {
            if (string.IsNullOrEmpty(username))
            {
                return false;
            }
            if (string.IsNullOrEmpty(password))
            {
                return false;
            }
            using (DataContext Context = new DataContext())
            {
                var User = Context.Users.FirstOrDefault(Usr => Usr.Username == username);
                if (User == null)
                {
                    return false;
                }
                var HashedPassword = User.Password;
                var VerificationSucceeded = (HashedPassword != null && Crypto.VerifyHashedPassword(HashedPassword, password));
               
                Context.SaveChanges();
                return VerificationSucceeded;
            }
        }

        public override MembershipUser GetUser(string username, bool userIsOnline)
        {
            if (string.IsNullOrEmpty(username))
            {
                return null;
            }
            using (DataContext Context = new DataContext())
            {
                var User = Context.Users.FirstOrDefault(Usr => Usr.Username == username);
                if (User == null)
                {
                    return null;
                }
                if (userIsOnline)
                {
                    Context.SaveChanges();
                }
                return new MembershipUser(System.Web.Security.Membership.Provider.Name, User.Username, null,
                                          User.Email, null, null, true, false,
                                          DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now);
            }
        }

        public override MembershipUser GetUser(object providerUserKey, bool userIsOnline)
        {
            if (providerUserKey is Guid) { }
            else
            {
                return null;
            }

            using (DataContext Context = new DataContext())
            {
                var User = Context.Users.Find(providerUserKey);
                if (User == null)
                {
                    return null;
                }
                if (userIsOnline)
                {
                    Context.SaveChanges();
                }
                return new MembershipUser(System.Web.Security.Membership.Provider.Name, User.Username, null,
                                          User.Email, null, null, true, false,
                                          DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now);
            }
        }

        public override bool ChangePassword(string username, string oldPassword, string newPassword)
        {
            if (string.IsNullOrEmpty(username))
            {
                return false;
            }
            if (string.IsNullOrEmpty(oldPassword))
            {
                return false;
            }
            if (string.IsNullOrEmpty(newPassword))
            {
                return false;
            }
            using (DataContext Context = new DataContext())
            {
                var User = Context.Users.FirstOrDefault(Usr => Usr.Username == username);
                if (User == null)
                {
                    return false;
                }
                var NewHashedPassword = Crypto.HashPassword(newPassword);
                if (NewHashedPassword.Length > 128)
                {
                    return false;
                }
                User.Password = NewHashedPassword;
                Context.SaveChanges();
                return true;
            }
        }

        public override bool UnlockUser(string userName)
        {
            using (DataContext Context = new DataContext())
            {
                var User = Context.Users.FirstOrDefault(Usr => Usr.Username == userName);
                if (User == null)
                {
                    return false;
                }
                Context.SaveChanges();
                return true;
            }
        }

       

        public override bool DeleteUser(string username, bool deleteAllRelatedData)
        {
            if (string.IsNullOrEmpty(username))
            {
                return false;
            }
            using (DataContext Context = new DataContext())
            {
                var User = Context.Users.FirstOrDefault(Usr => Usr.Username == username);
                if (User == null)
                {
                    return false;
                }
                Context.Users.Remove(User);
                Context.SaveChanges();
                return true;
            }
        }

        public override string GetUserNameByEmail(string email)
        {
            using (DataContext Context = new DataContext())
            {
                var User = Context.Users.FirstOrDefault(Usr => Usr.Email == email);
                return User != null ? User.Username : string.Empty;
            }
        }

        public override MembershipUserCollection FindUsersByEmail(string emailToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            MembershipUserCollection MembershipUsers = new MembershipUserCollection();
            using (DataContext Context = new DataContext())
            {
                totalRecords = Context.Users.Count(Usr => Usr.Email == emailToMatch);
                var Users = Context.Users.Where(Usr => Usr.Email == emailToMatch).OrderBy(Usrn => Usrn.Username).Skip(pageIndex * pageSize).Take(pageSize);
                foreach (var user in Users)
                {
                    MembershipUsers.Add(new MembershipUser(System.Web.Security.Membership.Provider.Name, user.Username, null, user.Email, null, null, true, false, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now));
                }
            }
            return MembershipUsers;
        }

        public override int GetNumberOfUsersOnline()
        {
            return 0;
        }

        public override MembershipUserCollection FindUsersByName(string usernameToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            MembershipUserCollection MembershipUsers = new MembershipUserCollection();
            using (DataContext Context = new DataContext())
            {
                totalRecords = Context.Users.Count(Usr => Usr.Username == usernameToMatch);
                var Users = Context.Users.Where(Usr => Usr.Username == usernameToMatch).OrderBy(Usrn => Usrn.Username).Skip(pageIndex * pageSize).Take(pageSize);
                foreach (User user in Users)
                {
                    MembershipUsers.Add(new MembershipUser(System.Web.Security.Membership.Provider.Name, user.Username, null, user.Email, null, null, true, false, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now));
                }
            }
            return MembershipUsers;
        }

        public override MembershipUserCollection GetAllUsers(int pageIndex, int pageSize, out int totalRecords)
        {
            MembershipUserCollection MembershipUsers = new MembershipUserCollection();
            using (DataContext Context = new DataContext())
            {
                totalRecords = Context.Users.Count();
                IQueryable<User> Users = Context.Users.OrderBy(Usrn => Usrn.Username).Skip(pageIndex * pageSize).Take(pageSize);
                foreach (User user in Users)
                {
                    MembershipUsers.Add(new MembershipUser(System.Web.Security.Membership.Provider.Name, user.Username, null, user.Email, null, null, true, false, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now));
                }
            }
            return MembershipUsers;
        }

        #endregion

        #region Not Supported

        //CodeFirstMembershipProvider does not support password retrieval scenarios.
        public override bool EnablePasswordRetrieval
        {
            get { return false; }
        }
        public override string GetPassword(string username, string answer)
        {
            throw new NotSupportedException("Consider using methods from WebSecurity module.");
        }

        //CodeFirstMembershipProvider does not support password reset scenarios.
        public override bool EnablePasswordReset
        {
            get { return false; }
        }
        public override string ResetPassword(string username, string answer)
        {
            throw new NotSupportedException("Consider using methods from WebSecurity module.");
        }

        //CodeFirstMembershipProvider does not support question and answer scenarios.
        public override bool RequiresQuestionAndAnswer
        {
            get { return false; }
        }
        public override bool ChangePasswordQuestionAndAnswer(string username, string password, string newPasswordQuestion, string newPasswordAnswer)
        {
            throw new NotSupportedException("Consider using methods from WebSecurity module.");
        }

        //CodeFirstMembershipProvider does not support UpdateUser because this method is useless.
        public override void UpdateUser(MembershipUser user)
        {
            throw new NotSupportedException();
        }

        #endregion
    }
}