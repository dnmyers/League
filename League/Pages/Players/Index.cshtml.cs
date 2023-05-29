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

        // Get a list of all players
        public List<Player> Players { get; set; }

        // SelectList for the teams, populated from all Team IDs
        public SelectList TeamIDs { get; set; }

        [BindProperty(SupportsGet = true)]
        public string SelectedTeam { get; set; }

        // SelectList for the positions, populated with distinct positions
        public SelectList Positions { get; set; }

        [BindProperty(SupportsGet = true)]
        public string SelectedPosition { get; set; }

        // Search for Name
        [BindProperty(SupportsGet = true)]
        public string SearchString { get; set; }

        // Sort By
        [BindProperty(SupportsGet = true)]
        public string SortBy { get; set; } = "Name";

        // Sort By SelectList
        public SelectList SortByList { get; set; }

        public async Task OnGetAsync()
        {
            //Players = await _context.Players.ToListAsync();

            var players = from p in _context.Players
                          select p;

            // ------------------
            // SelectedTeam
            // ------------------

            // Get TeamIds from Players table
            IQueryable<string> teamIDQuery = (from p in _context.Players
                                              orderby p.TeamId
                                              select p.TeamId).Distinct();

            TeamIDs = new SelectList(await teamIDQuery.ToListAsync());

            // Filter players by SelectedTeam if not "All"
            if (!string.IsNullOrEmpty(SelectedTeam) && SelectedTeam != "All")
            {
                players = from p in players
                          where p.TeamId == SelectedTeam
                          select p;
                    //players.Where(p => p.TeamId == SelectedTeam);
            }

            // ------------------
            // SelectedPosition
            // ------------------

            // Get Positions from Players table
            IQueryable<string> positionsQuery = (from p in _context.Players
                                                orderby p.Position
                                                select p.Position).Distinct();

            Positions = new SelectList(await positionsQuery.ToListAsync());

            // Filter players by position if not "All"
            if (!string.IsNullOrEmpty(SelectedPosition) && SelectedPosition != "All")
            {
                players = from p in players
                          where p.Position == SelectedPosition
                          select p;                    
                    //players.Where(p => p.Position == SelectedPosition);
            }

            // -----------------
            // SearchString
            // -----------------

            // If SearchString isn't null or empty, use it to filter players
            if (!string.IsNullOrEmpty(SearchString))
            {
                players = from p in players
                          where p.Name.ToLower().Contains(SearchString.ToLower())
                          select p;                    
                    //players.Where(p => p.Name.ToLower().Contains(SearchString.ToLower()));
            }

            // ------------------
            // SortBy
            // ------------------

            // Populate SortByList with SelectListItems
            SortByList = new SelectList(new List<SelectListItem>
            {
                new SelectListItem{Text = "Team", Value = "Team" },
                new SelectListItem{Text = "Number", Value = "Number" },
                new SelectListItem{ Text = "Name", Value = "Name" },
                new SelectListItem{ Text = "Position", Value = "Position" },
                new SelectListItem{ Text = "Experience", Value = "Experience" },
                new SelectListItem{ Text = "College", Value = "College" }
            }, "Value", "Text");

            // Order players by SortBy value
            switch (SortBy)
            {
                case "Team":
                    players = players.OrderBy(p => p.TeamId);
                    break;
                case "Number":
                    players = players.OrderBy(p => p.Number);
                    break;
                case "Name":
                    players = players.OrderBy(p => p.Name);
                    break;
                case "Position":
                    players = players.OrderBy(p => p.Position);
                    break;
                case "Experience":
                    players = players.OrderBy(p => p.Experience);
                    break;
                case "College":
                    players = players.OrderBy(p => p.College);
                    break;
                case null:
                default:
                    break;
            }

            // ------------------
            // Populate Players
            // ------------------

            Players = await players.ToListAsync();
        }
    }
}
