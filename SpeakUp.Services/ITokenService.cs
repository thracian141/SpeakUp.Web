using SpeakUp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpeakUp.Services {
    public interface ITokenService {
        Task<string> GenerateToken(ApplicationUser user);
    }
}
