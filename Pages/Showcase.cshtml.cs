using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace ComplexWeb.Pages
{
    public class ShowcaseModel : PageModel
    {
        private readonly ILogger<PrivacyModel> _logger;

        // Brings connection status from AppConnect Class forward to use in Razor Page
        public bool p_isConnected { get{ return AppConnect.p_hasConnected; } }

        public ShowcaseModel(ILogger<PrivacyModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
            System.Diagnostics.Debug.WriteLine( "GET USED" );
        }

        public void OnPost()
        {
            System.Diagnostics.Debug.WriteLine( "POST USED" );
        }

        public void OnPostSounder( string l_soundName )
        {

        }
    }
}
