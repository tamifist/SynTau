﻿using System.Security.Cryptography;
using System.Text;
using Infrastructure.Identity.Contracts.Services;
using Shared.Framework.Dependency;

namespace Infrastructure.Identity.Services
{
    public class PasswordHashService : IPasswordHashService, IScopedDependency
    {
        private readonly HashAlgorithm hashAlgorithm;

        public PasswordHashService(HashAlgorithm hashAlgorithm)
        {
            this.hashAlgorithm = hashAlgorithm;
        }

        /// <summary>
        /// Validates user password.
        /// </summary>
        /// <param name="password">Plain text user password.</param>
        /// <param name="passwordHash">Hashed user password from database.</param>
        /// <param name="passwordSalt">User password salt from database.</param>
        /// <returns>Returns true if password is valid.</returns>
        public bool ValidatePassword(string password, string passwordHash, string passwordSalt)
        {
            byte[] passwordWithSalt = Encoding.Unicode.GetBytes(string.Concat(password, passwordSalt));
            var passwordHashForCompare = Encoding.Unicode.GetString(hashAlgorithm.ComputeHash(passwordWithSalt));
            return passwordHashForCompare.Equals(passwordHash);
        }

        /// <summary>
        /// Creates user password hash.
        /// </summary>
        /// <param name="password">Plain text user password.</param>
        /// <param name="passwordSalt">Autogenerated password salt.</param>
        /// <returns>Returns hashed password.</returns>
        public string CreatePasswordHash(string password, string passwordSalt)
        {
            byte[] passwordWithSalt = Encoding.Unicode.GetBytes(string.Concat(password, passwordSalt));
            var passwordHash = hashAlgorithm.ComputeHash(passwordWithSalt);
            return Encoding.Unicode.GetString(passwordHash);
        }
    }
}