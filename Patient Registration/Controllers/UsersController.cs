using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Patient_Registration.Data;
using Patient_Registration.Models;
using Patient_Registration.ViewModels;

namespace Patient_Registration.Controllers
{
    public class UsersController : Controller
    {
        private readonly RegisterDb _context;
       
        public UsersController(RegisterDb context)
        {
            _context = context;
           
        }

        public ActionResult Dashboard()
        {
            var sessionValue = HttpContext.Session.GetString("loggeduser");
            if (sessionValue != "none")
            {
                return View();
            }
            return View("Signin");
        }
        public ActionResult Signin()
        {
            return View();
            
        }
        public ActionResult Logout()
        {
            HttpContext.Session.SetString("loggeduser", "none");
            return View("Signin");
        }

        [HttpPost]
        public ActionResult Signin(Call_Users ur)
        {

            var user = _context.Call_Users.FirstOrDefault(x => x.UserName == ur.UserName && x.Password == ur.Password);
            if (user != null)
            {
                //HttpContext.Session.SetString("logged", "true");
                // HttpContext.Session.SetString("userName", ur.UserName);
                //Session["logged"] = "true";
                //Session["userName"] = usr.UserName;
                /*LoginViewModel logData = new LoginViewModel();
                logData.usersdata = user;*/

                HttpContext.Session.SetString("loggeduser", user.UserName);

                // return View("Dashboard", user);
                return RedirectToAction("Create", "OurPatients");
            }
            /* var usr = (from b in db.tbl_UserDetails
                        where b.UserName.Equals(objUsr.UserName)
                        && b.Password.Equals(objUsr.Password)
                        select b).FirstOrDefault();

             if (usr != null)
             {
                // Session["logged"] = "true";
                //Session["userName"] = usr.UserName;
                 return View("Dashboard", usr);

             }*/
            ViewBag.Error = "Invalid username or password";
            return View();
        }

        // GET: Users
        public IActionResult ChangePassword()
        {
            var sessionValue = HttpContext.Session.GetString("loggeduser");
            if ((sessionValue == "none") || (sessionValue == null))
            {
                return RedirectToAction("Signin", "Users");
            }
            else
            {
                return View();
            }
            /*var username = HttpContext.Session.GetString("loggeduser");
            if (!string.IsNullOrEmpty(username))
            {
                var user = _context.Call_Users.FirstOrDefault(x => x.UserName == username);

            }*/

        }

        [HttpPost]
        public IActionResult ChangePassword(string? oldpwd, string? newpwd,string? confirmnewpwd)
        {
            var username = HttpContext.Session.GetString("loggeduser");
            
            if (!string.IsNullOrEmpty(username))
            {
                 var usr = _context.Call_Users.FirstOrDefault(x => x.UserName == username);
                if(usr != null)
                {
                    if(usr.Password == oldpwd)
                    {
                        if (!string.IsNullOrEmpty(newpwd))
                        {
                            if (newpwd.Equals(confirmnewpwd))
                            {
                                usr.Password = newpwd;
                                _context.Update(usr);
                                _context.SaveChanges();
                                HttpContext.Session.SetString("loggeduser", "none");
                                return View("Signin");
                            }
                            else
                            {
                                ViewBag.Error = "New password and Confirm password are mismatch";
                            }
                        }
                    }
                    else
                    {
                        ViewBag.Error = "Invalid previous password";
                    }
                }
            }
            return View();
        }

        // GET: Users
        public async Task<IActionResult> Index()
        {
            return View(await _context.Call_Users.ToListAsync());
        }

        // GET: Users/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var call_Users = await _context.Call_Users
                .FirstOrDefaultAsync(m => m.UserId == id);
            if (call_Users == null)
            {
                return NotFound();
            }

            return View(call_Users);
        }

        // GET: Users/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserId,UserName,Password,Active,Type")] Call_Users call_Users)
        {
            if (ModelState.IsValid)
            {
                _context.Add(call_Users);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(call_Users);
        }

        // GET: Users/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var call_Users = await _context.Call_Users.FindAsync(id);
            if (call_Users == null)
            {
                return NotFound();
            }
            return View(call_Users);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("UserId,UserName,Password,Active,Type")] Call_Users call_Users)
        {
            if (id != call_Users.UserId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(call_Users);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!Call_UsersExists(call_Users.UserId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(call_Users);
        }

        // GET: Users/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var call_Users = await _context.Call_Users
                .FirstOrDefaultAsync(m => m.UserId == id);
            if (call_Users == null)
            {
                return NotFound();
            }

            return View(call_Users);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var call_Users = await _context.Call_Users.FindAsync(id);
            if (call_Users != null)
            {
                _context.Call_Users.Remove(call_Users);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool Call_UsersExists(int id)
        {
            return _context.Call_Users.Any(e => e.UserId == id);
        }
    }
}
