using System;
using Microsoft.AspNetCore.Identity;

namespace Domain
{
    public class DiffeDB
    {
        public int Id { get; set; }
        public ulong g { get; set; }
        public ulong p { get; set; }
        public ulong a { get; set; }
        public ulong b { get; set; }
        public ulong HelKey { get; set; }

        public string UserId { get; set; }
        public IdentityUser User { get; set; }
    }
}