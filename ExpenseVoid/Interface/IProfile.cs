using ExpenseVoid.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ExpenseVoid.Interface
{
    public interface IProfile
    {
        Task SaveProfileAsync(Profile profile);
        Task RemoveProfileAsync(Profile profile);
        Task EditProfileAsync(Profile profile);
        Task<List<Profile>> LoadProfilesAsync();

    }
}
