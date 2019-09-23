using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using Business.Identity.Contracts.Exceptions;
using Business.Identity.Contracts.Services;
using Business.Identity.Contracts.ViewModels;
using Data.Common.Contracts;
using Data.Common.Contracts.Entities;
using Microsoft.EntityFrameworkCore;
using Shared.Enum;
using Shared.Framework.Dependency;
using Shared.Framework.Security;
using Shared.Framework.Utilities;

namespace Business.Identity.Services.Services
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

        public bool CreateAccount(CreateAccountViewModel createAccountViewModel)
        {
            if (string.IsNullOrWhiteSpace(createAccountViewModel.FirstName) || string.IsNullOrWhiteSpace(createAccountViewModel.LastName) ||
                string.IsNullOrWhiteSpace(createAccountViewModel.Email) || string.IsNullOrWhiteSpace(createAccountViewModel.Password) || 
                string.IsNullOrWhiteSpace(createAccountViewModel.ConfirmPassword))
            {
                throw new ArgumentNullException();
            }

            if (createAccountViewModel.Password != createAccountViewModel.ConfirmPassword)
            {
                throw new ConfirmPasswordException();
            }

            if (PasswordStrengthUtility.CheckStrength(createAccountViewModel.Password) != PasswordScore.Strong)
            {
                throw new PasswordStrengthException();
            }

            User existingUser = unitOfWork.GetAll<User>().SingleOrDefault(u => u.Email.Equals(createAccountViewModel.Email));

            if (existingUser != null)
            {
                throw new UserDuplicateException();
            }

            User user = new User();
            user.Email = createAccountViewModel.Email;
            user.FirstName = createAccountViewModel.FirstName;
            user.LastName = createAccountViewModel.LastName;
            //user.Organization = createAccountViewModel.Organization;
            //user.LabGroup = createAccountViewModel.LabGroup;
            //user.Country = (CountryEnum)createAccountViewModel.Country.Value;

            user.Password = createAccountViewModel.Password;
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
            User user = unitOfWork.GetAll<User>()
                .Include(x => x.UserRoles)
                    .ThenInclude(x => x.Role)
                .SingleOrDefault(u => u.Email.Equals(email));

            if (user == null)
            {
                throw new UserNotFoundException();
            }

            bool isUserPasswordValid = ValidateUserPassword(password, user) && user.UserRoles != null && user.UserRoles.Any();

            if (!isUserPasswordValid)
            {
                throw new IncorrectPasswordException();
            }

            var userInfo = new UserInfo();
            userInfo.Email = email;
            userInfo.UserId = user.Id;
            userInfo.FirstName = user.FirstName;
            userInfo.LastName = user.LastName;
            SetRoles(userInfo, user.UserRoles);

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

            if (!ValidateUserPassword(password, user))
            {
                return false;
            }

            user.Password = newPassword;
            SetPasswordHash(user);

            unitOfWork.InsertOrUpdate(user);
            unitOfWork.Commit();

            return true;
        }

        private bool ValidateUserPassword(string password, User user)
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

        private void SetRoles(UserInfo userInfo, IEnumerable<UserRole> roles)
        {
            if (!roles.Any())
            {
                return;
            }

            userInfo.Roles = new List<RoleInfo>();
            foreach (UserRole role in roles)
            {
                var roleInfo = new RoleInfo();
                roleInfo.Name = role.Role.Name;

                userInfo.Roles.Add(roleInfo);
            }
        }
    }
}