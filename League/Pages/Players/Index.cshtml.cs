using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using League.Models;
using League.Data;

namespace League.Pages.Players
{
    public class IndexModel : PageModel
    {
        // Inject the Entity Framework context
        private readonly LeagueContext _context;

        public IndexModel(LeagueContext context)
        {
            _context = context;
        }

        // Load all leagues, conferences, divisions, teams, and players
        public List<Models.League> Leagues { get; set; }
        public List<Conference> Conferences { get; set; }
        public List<Division> Divisions { get; set; }
        public List<Team> Teams { get; set; }
        public List<Player> Players { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            Players = await _context.Players.ToListAsync();

            return Page();
        }
    }
}
