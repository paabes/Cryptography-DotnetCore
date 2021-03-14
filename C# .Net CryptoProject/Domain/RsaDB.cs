using Microsoft.AspNetCore.Identity;

namespace Domain
{
    public class RsaDb
    {
        public int RsaDbId { get; set; }
        public ulong p { get; set; } // is prime, p*q < maxvalue, prime includes not 0
        public ulong q { get; set; } //
        public ulong n { get; set; }
        public ulong e { get; set; }
        public ulong d { get; set; }
        public string Plaintext { get; set; } // not null
        public string Ciphertext { get; set; }
        
        
        public string UserId { get; set; }
        public IdentityUser User { get; set; }
        
    }
}