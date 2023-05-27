using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using League.Models;
using League.Data;
using SQLitePCL;
using Microsoft.AspNetCore.Http;

namespace League.Pages.Teams
{
    public class IndexModel : PageModel
    {
        // Inject the Entity Framework context
        private readonly LeagueContext _context;

        public IndexModel(LeagueContext context)
        {
            _context = context;
        }

        // Load all leagues, conferences, divisions, and teams
        public List<League.Models.League> Leagues { get; set; }
        public List<Conference> Conferences { get; set; }
        public List<Division> Divisions { get; set; }
        public List<Team> Teams { get; set; }

        // Allow selectiong of a favorite team
        [BindProperty(SupportsGet = true)]
        public string FavoriteTeam { get; set; }

        // Dropdown list for favorite team selectiong
        public SelectList AllTeams { get; set; }

        public async Task OnGetAsync()
        {
            // load all records for all 3 tables
            Conferences = await _context.Conferences.ToListAsync();
            Divisions = await _context.Divisions.ToListAsync();
            Teams = await _context.Teams.ToListAsync();

            // Get Teams for SelectList
            IQueryable<string> teamQuery = from t in _context.Teams
                                           orderby t.TeamId
                                           select t.TeamId;

            // Populate SelectList
            AllTeams = new SelectList(await teamQuery.ToListAsync());

            // If a favorite team exists, manage the cookie
            if(FavoriteTeam != null)
            {
                HttpContext.Session.SetString("_Favorite", FavoriteTeam);
            } else
            {
                FavoriteTeam = HttpContext.Session.GetString("_Favorite");      
            }
        }

        // Get all divisions for a conference and sort them
        public List<Division> GetConferenceDivisions(string ConferenceId) 
        {
            return Divisions.Where(d => d.ConferenceId.Equals(ConferenceId)).OrderBy(d => d.Name).ToList();
        }

        // Get all teams for a division and sort them
        public List<Team> GetDivisionTeams(string DivisionId)
        {
            return Teams.Where(t => t.DivisionId.Equals(DivisionId)).OrderBy(t => t.Loss).ToList();
        }
    }
}
