using System.Linq;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using Client_Customer.Models;
using Client_Customer.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Client_Customer.Pages
{
    /// <summary>
    /// Model class for Login, responsible for handling all get/post requests directly from the User.
    /// </summary>
    public class LoginModel : PageModel
    {
        private OrderService orderService;
        private string targetUrlRegisterClient;
        [BindProperty]
        public Client Client { get; set; }

        /// <summary>
        /// Handles any unspecified OnGet requests from user and signs him out.
        /// </summary>
        public void OnGet()
        {
            Task.Run(() => HttpContext.SignOutAsync("Cookies"));
            Task.Run(() => HttpContext.SignOutAsync("oidc"));
        }
        /// <summary>
        /// Handles any unspecified OnPot requests from user.
        /// </summary>
        public void OnPost()
        {
            targetUrlRegisterClient = "http://localhost:8080/server_war_exploded/root/api/registerclient";
            orderService = new OrderService();
            Task<string> response = orderService.PostRegisterClientAsync(Client, targetUrlRegisterClient);
        }
    }
}