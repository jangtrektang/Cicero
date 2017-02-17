using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cicero.Core.Helpers.Membership
{
    public class PasswordHelper
    {
        public string HashPassword(string password)
        {
            var bytes = new UTF8Encoding().GetBytes(password);
            byte[] hashBytes;
            using (var algorithm = new System.Security.Cryptography.SHA512Managed())
            {
                hashBytes = algorithm.ComputeHash(bytes);
            }
            return Convert.ToBase64String(hashBytes);
        }
    }
}
