using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OlimoXamarinForms.Services
{
    public interface IFirebaseAuthenticator
    {
        Task<string> LoginWithEmailPassword(string email, string password);
        Task<string> SignUpWithEmailPassword(string email, string password, string name, bool isDriver);
        Task SendPasswordResetEmail(string email);
        void SignOut();
    }
}
