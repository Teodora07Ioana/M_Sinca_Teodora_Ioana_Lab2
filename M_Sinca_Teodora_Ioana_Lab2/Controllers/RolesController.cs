using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using M_Sinca_Teodora_Ioana_Lab2.Models;

namespace M_Sinca_Teodora_Ioana_Lab2.Controllers
{
    public class RolesController : Controller
    {
        private RoleManager<IdentityRole> roleManager;
        private UserManager<IdentityUser> userManager;

        public RolesController(RoleManager<IdentityRole> roleMgr, UserManager<IdentityUser> userMrg)
        {
            roleManager = roleMgr;
            userManager = userMrg;
        }
        public ViewResult Index() => View(roleManager.Roles);
        private void Errors(IdentityResult result)
        {
            foreach (IdentityError error in result.Errors)
                ModelState.AddModelError("", error.Description);
        }

        public IActionResult Create() => View();
        [HttpPost]
        public async Task<IActionResult> Create([Required] string name)
        {
            if (ModelState.IsValid)
            {
                IdentityResult result = await roleManager.CreateAsync(new
               IdentityRole(name));
                if (result.Succeeded)
                    return RedirectToAction("Index");
                else
                    Errors(result);
            }
            return View(name);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            IdentityRole role = await roleManager.FindByIdAsync(id);
            if (role != null)
            {
                IdentityResult result = await
               roleManager.DeleteAsync(role);
                if (result.Succeeded)
                    return RedirectToAction("Index");
                else
                    Errors(result);
            }
            else
                ModelState.AddModelError("", "No role found");
            return View("Index", roleManager.Roles);
        }

        public async Task<IActionResult> Update(string id)
        {
            Console.WriteLine("Update method was called."); ////////
            IdentityRole role = await roleManager.FindByIdAsync(id);
            List<IdentityUser> members = new List<IdentityUser>();
            List<IdentityUser> nonMembers = new List<IdentityUser>();
            foreach (IdentityUser user in userManager.Users)
            {
                var list = await userManager.IsInRoleAsync(user, role.Name)
               ? members : nonMembers;
                list.Add(user);
            }
            return View(new RoleEdit
            {
                Role = role,
                Members = members,
                NonMembers = nonMembers
            });
        }

        /*[HttpPost]
        public async Task<IActionResult> Update(RoleModification model)
        {
            if (ModelState.IsValid)
            {
                IdentityResult result;

                foreach (string userId in model.AddIds ?? Array.Empty<string>())
                {
                    IdentityUser user = await userManager.FindByIdAsync(userId);
                    if (user != null)
                    {
                        result = await userManager.AddToRoleAsync(user, model.RoleName);
                        if (!result.Succeeded)
                            Errors(result);
                    }
                }

                *//* foreach (string userId in model.DeleteIds ?? Array.Empty<string>())
                 {
                     IdentityUser user = await userManager.FindByIdAsync(userId);

                     {
                         result = await userManager.RemoveFromRoleAsync(user, model.RoleName);
                         if (!result.Succeeded)
                             Errors(result);
                     }
                 }*//*
                if (model.DeleteIds != null) // Verifică dacă DeleteIds nu este null
                {
                    foreach (string userId in model.DeleteIds)
                    {
                        IdentityUser user = await userManager.FindByIdAsync(userId);
                        if (user != null)
                        {
                            result = await userManager.RemoveFromRoleAsync(user, model.RoleName);
                            if (!result.Succeeded)
                                Errors(result);
                        }
                    }
                }

                return RedirectToAction(nameof(Index));
            }
            return await Update(model.RoleId);*/

        [HttpPost]
        public async Task<IActionResult> Update(RoleModification model)
        {
            if (ModelState.IsValid) // Verifică dacă modelul este valid
            {
                IdentityResult result;

                // **1. Procesare pentru AddIds**
                foreach (string userId in model.AddIds ?? Array.Empty<string>()) // Verifică dacă AddIds este null
                {
                    IdentityUser user = await userManager.FindByIdAsync(userId);
                    if (user != null)
                    {
                        result = await userManager.AddToRoleAsync(user, model.RoleName);
                        if (!result.Succeeded)
                            Errors(result);
                    }
                }

                // **2. Procesare pentru DeleteIds**
                if (model.DeleteIds != null) // Verifică explicit dacă DeleteIds este null
                {
                    foreach (string userId in model.DeleteIds)
                    {
                        IdentityUser user = await userManager.FindByIdAsync(userId);
                        if (user != null)
                        {
                            result = await userManager.RemoveFromRoleAsync(user, model.RoleName);
                            if (!result.Succeeded)
                                Errors(result);
                        }
                    }
                }

                return RedirectToAction(nameof(Index));
            }

            // Dacă validarea modelului eșuează, reîncarcă pagina cu datele curente
            return await Update(model.RoleId);
        }



        /*if (ModelState.IsValid)
            return RedirectToAction(nameof(Index));
        else
            return await Update(model.RoleId);*/
    }
}
