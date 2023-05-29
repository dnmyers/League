using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using League.Data;
using League.Models;

namespace League.Pages.Players
{
    public class PlayerModel : PageModel
    {
        private readonly LeagueContext _context;

        public Player Player { get; set; }

        public double feet { get; set; }
        public double inches { get; set; }

        public PlayerModel(LeagueContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if(string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            Player = await _context.Players.FirstOrDefaultAsync(p => p.PlayerId == id);

            // Height feet
            feet = Math.Floor((double)Player.Height / 12);

            // Height inches
            inches = (double)Player.Height % 12;

            return Page();
        }
    }
}
