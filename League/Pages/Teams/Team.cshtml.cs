using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using League.Data;
using League.Models;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.ValueGeneration;

namespace League.Pages.Teams
{
    public class TeamModel : PageModel
    {
        // Inject the Entity Framework context
        private readonly LeagueContext _context;

        public TeamModel(LeagueContext context)
        {
            _context = context;
        }

        // Load a single team with TeamId matching id and include the related division and players        
        public Team Team { get; set; }

        public async Task OnGetAsync(string id)
        {
            Team = await _context.Teams
                .Include(t => t.Players)
                .Include(t => t.Division)
                .AsNoTracking()
                .FirstOrDefaultAsync(t => t.TeamId == id);
        }
    }
}
