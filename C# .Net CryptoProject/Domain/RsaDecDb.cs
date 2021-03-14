using Microsoft.AspNetCore.Identity;

namespace Domain
{
    public class RsaDecDb
    {
        public int RsaDecDbId { get; set; }
        public ulong n { get; set; } // more than zero, not null
        public ulong d { get; set; } // same
        public string Plaintext { get; set; }
        public string Ciphertext { get; set; } // not null, Base64
        
        
        public string UserId { get; set; }
        public IdentityUser User { get; set; }
    }
}