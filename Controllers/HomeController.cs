using System.Diagnostics;
using FirstWebsite.Data;
using FirstWebsite.Models;
using Microsoft.AspNetCore.Mvc;

namespace FirstWebsite.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> SendEmail(ContactForm model)
        {
            if (ModelState.IsValid)
            {
                var msg = new ContactMessage
                {
                    Name = model.Name,
                    Email = model.Email,
                    Message = model.Message
                };

                _context.ContactMessages.Add(msg);
                await _context.SaveChangesAsync();

                ViewBag.Message = "Message sent successfully!";
            }
            return View("Contact");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteMessage(int id)
        {
            if (HttpContext.Session.GetString("IsAdmin") != "true")
            {
                return Unauthorized();
            }

            var message = await _context.ContactMessages.FindAsync(id);
            if (message != null)
            {
                _context.ContactMessages.Remove(message);
                await _context.SaveChangesAsync();
            }
            return Redirect("/Home/Contact/Admin");
        }

        // Show the Login Page
        [HttpGet]
        [Route("Home/Contact/Login")]
        public IActionResult Login() => View();

        // Process the Login
        [HttpPost]
        [Route("Home/Contact/Login")]
        public IActionResult Login(string password)
        {
            if (password == "Ncx020308") // Set your own password here!
            {
                HttpContext.Session.SetString("IsAdmin", "true");
                return RedirectToAction("Admin");
            }
            ViewBag.Error = "Incorrect Password";
            return View();
        }

        // 1. Centralized list of ALL projects with IDs
        private List<Project> GetProjects() => new List<Project>
        {
            new Project {
                Id = "period-app",
                Title = "Period Application Design",
                Tool = "Figma",
                ImageUrl = "/images/main/Figmaperiod.png",
                Description = "Designed the user interface for a women-focused mobile application.",
                Challenge = "Unsure how to make the linked page be fluent.",
                Solution = "Studied similar applications from YouTube and Rednote."
            },
            new Project {
                Id = "cos-design",
                Title = "Website & Logo Design",
                Tool = "Wix, Adobe Illustrator",
                ImageUrl = "/images/main/COS.png",
                Description = "Built a company website and created the brand logo for C.O.S.",
                Challenge = "Limited layout flexibility when using Wix templates.",
                Solution = "Customized sections using advanced editor mode and adjusted grids manually."
            },
            new Project {
                Id = "edu-game",
                Title = "Educational Game Design",
                Tool = "Unity, C#",
                ImageUrl = "/images/main/education.png",
                Description = "Interface for child-friendly learning activities.",
                Challenge = "Sensitivity of the touch screen issue.",
                Solution = "Studied from youtube to enhance the touch screen sensitivity."
            },
            new Project {
                Id = "3d-game",
                Title = "3D Level Game Design",
                Tool = "Unity, C#",
                ImageUrl = "/images/main/3DGame.png",
                Description = "Level gameplay involving collection and time challenges.",
                Challenge = "Collision between enemy and character, characther between the reward.",
                Solution = "Studied from youtube to solve the collision problem."
            },
            new Project {
                Id = "internship",
                Title = "Internship Project",
                Tool = "Canva, Adobe Creative Suite",
                ImageUrl = "/images/main/RAS.png",
                Description = "Designed poster, edit video, photography and videography.",
                Challenge = "Handle multiple projects in dealine.",
                Solution = "Manage time properly to finish all the projects."
            },
            new Project {
                Id = "achievement",
                Title = "Individual Achievement",
                Tool = "Unity, Adobe Creative Suite",
                ImageUrl = "/images/main/Adobe.png",
                Description = "My journey in technical design, spanning self-initiated learning in 3D modeling, competitive school challenges, and national-level accolades.",
                Challenge = "Learn to master complex 3D workflows in Blender and AI-driven design tools.",
                Solution = "Adopted a disciplined self-learning roadmap, utilizing my own time to explore 3D animation and product visualization in Blender."
            }
        };

        // 2. Index now calls the helper so the IDs exist
        public IActionResult Index()
        {
            var projects = GetProjects();
            return View(projects);
        }

        // 3. Details will now find the project because the ID exists
        public IActionResult Details(string id)
        {
            var projectList = GetProjects(); // Your existing method that returns the list
            var project = projectList.FirstOrDefault(p => p.Id == id);

            if (project == null) return NotFound();

            // Find the index of the current project
            int currentIndex = projectList.IndexOf(project);

            // Get the next project (loop back to start if it's the last one)
            var nextProject = projectList[(currentIndex + 1) % projectList.Count];

            // Pass the next project ID to the view using ViewBag
            ViewBag.NextProjectId = nextProject.Id;
            ViewBag.NextProjectTitle = nextProject.Title;

            return View(project);
        }

        // This forces the URL to be: yoursite.com/Home/Contact/Admin
        [Route("Home/Contact/Admin")]
        public IActionResult Admin()
        {
            // Check if the user is "Logged In"
            if (HttpContext.Session.GetString("IsAdmin") != "true")
            {
                return RedirectToAction("Login");
            }

            var messages = _context.ContactMessages.OrderByDescending(m => m.SubmittedAt).ToList();
            return View(messages);
        }

        [Route("Home/Error/{statusCode}")]
        public IActionResult Error(int statusCode)
        {
            if (statusCode == 404)
            {
                return View("NotFound");
            }
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public IActionResult About() => View();
        public IActionResult Contact() => View();
        public IActionResult Privacy() => View();

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

    }
}
