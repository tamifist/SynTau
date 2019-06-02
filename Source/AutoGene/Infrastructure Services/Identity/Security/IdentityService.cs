using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using Data.Contracts;
using Data.Contracts.Entities.Identity;
using Infrastructure.Contracts.Security;
using Shared.Framework.Dependency;
using Shared.Framework.Exceptions;
using Shared.Framework.Security;

namespace Infrastructure.Identity.Security
{
    public class IdentityService : IIdentityService, IScopedDependency
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IPasswordHashService passwordHashService;

        public IdentityService(IUnitOfWork unitOfWork, IPasswordHashService passwordHashService)
        {
            this.unitOfWork = unitOfWork;
            this.passwordHashService = passwordHashService;
        }

        public bool CreateAccount(string firstName, string lastName, string email, string password)
        {
            if (string.IsNullOrWhiteSpace(firstName) || string.IsNullOrWhiteSpace(lastName) ||
                string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                throw new ArgumentNullException();
            }

            User existingUser = unitOfWork.GetAll<User>().SingleOrDefault(u => u.Email.Equals(email));

            if (existingUser != null)
            {
                throw new UserDuplicateException();
            }

            User user = new User();
            user.Email = email;
            user.FirstName = firstName;
            user.LastName = lastName;

            user.Password = password;
            SetPasswordHash(user);

            Role guestRole = unitOfWork.GetAll<Role>().SingleOrDefault(x => x.Name.Equals("Guest"));

            user.AddRole(guestRole);

            unitOfWork.InsertOrUpdate(user);
            unitOfWork.Commit();

            return true;
        }

        /// <summary>
        /// Validates user password.
        /// </summary>
        /// <param name="email">User name from database.</param>
        /// <param name="password">Password for validate.</param>
        /// <returns>
        /// Returns UserInfo with user validation result.
        /// </returns>
        public UserInfo ValidateUserCredentials(string email, string password)
        {
            bool userCredentialsIsValid;

            IList<User> users = unitOfWork.GetAll<User>().Where(u => u.Email.Equals(email)).ToList();

            if (!users.Any())
            {
                userCredentialsIsValid = false;
            }
            else if (users.Count > 1)
            {
                throw new UserDuplicateException();
            }
            else
            {
                User user = users.Single();
                userCredentialsIsValid = ValidateUserCredentials(password, user) && user.Roles != null && user.Roles.Any();
            }

            var userInfo = new UserInfo();
            if (userCredentialsIsValid)
            {
                userInfo.Email = email;
                User userEntity = users.Single();
                if (userEntity != null)
                {
                    userInfo.UserId = userEntity.Id;
                    userInfo.FirstName = userEntity.FirstName;
                    userInfo.LastName = userEntity.LastName;
                    LoadRoles(userInfo, userEntity.Roles);
                }
            }
            else
            {
                throw new UserNotSignedUpException();
            }

            return userInfo;
        }

        /// <summary>
        /// Changes the user password.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <param name="password">The password.</param>
        /// <param name="newPassword">The new password.</param>
        /// <returns>
        ///   <c>true</c> if the password was changed successfully; <c>false</c> otherwize.
        /// </returns>
        public bool ChangeUserPassword(string userName, string password, string newPassword)
        {
            User user = unitOfWork.GetAll<User>().SingleOrDefault(u => u.Email.Equals(userName));

            if (user == null)
            {
                return false;
            }

            if (!ValidateUserCredentials(password, user))
            {
                return false;
            }

            user.Password = newPassword;
            SetPasswordHash(user);

            unitOfWork.InsertOrUpdate(user);
            unitOfWork.Commit();

            return true;
        }

        private bool ValidateUserCredentials(string password, User user)
        {
            if (string.IsNullOrWhiteSpace(password))
            {
                return false;
            }

            return passwordHashService.ValidatePassword(password, user.Password, user.PasswordSalt);
        }

        public string GenerateNewSecretKey()
        {
            using (var rng = new RNGCryptoServiceProvider())
            {
                var buff = new byte[64];
                rng.GetBytes(buff);
                return Convert.ToBase64String(buff);
            }
        }

        private void SetPasswordHash(User user)
        {
            string passwordSalt = GenerateNewSecretKey();
            string passwordHash = passwordHashService.CreatePasswordHash(user.Password, passwordSalt);

            user.Password = passwordHash;
            user.PasswordSalt = passwordSalt;
        }

        private void LoadRoles(UserInfo userInfo, IEnumerable<Role> roles)
        {
            if (!roles.Any())
            {
                return;
            }

            userInfo.Roles = new List<RoleInfo>();
            foreach (Role role in roles)
            {
                var roleInfo = new RoleInfo();
                roleInfo.Name = role.Name;

                userInfo.Roles.Add(roleInfo);
            }
        }
    }
}