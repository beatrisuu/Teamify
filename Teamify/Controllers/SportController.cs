﻿using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Teamify.DL;
using Teamify.DL.Entities;
using Teamify.Helpers;
using Teamify.Models.Sport;

namespace Teamify.Controllers
{
    [Authorize]
    public class SportController : BaseController
    {
        public SportController(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public ActionResult CreateSport(int id)
        {
            var requestModel = DbContext.AddSportRequests.FirstOrDefault(x => x.AddSportRequestId == id);

            if (requestModel == null)
                return HttpNotFound();

            var createModel = new CreateSportModel
            {
                AddSportRequestId = id,
                RequestSportName = requestModel.SportName,
                RequestSportDescription = requestModel.SportDescription,
                RequestSportRules = requestModel.SportRules
            };
            return View("Create", createModel);
        }

        [HttpPost]
        public ActionResult Create(CreateSportModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var createSport = new Sport
                    {
                        Name = model.NewSportName,
                        Description = model.NewSportDescription,
                        Rules = model.NewSportRules
                    };
                    createSport.AddAudit(CurrentUser);
                    DbContext.Sports.Add(createSport);

                    var updateSport = DbContext.AddSportRequests.FirstOrDefault(x => x.AddSportRequestId == model.AddSportRequestId);
                    if (updateSport != null)
                    {
                        updateSport.RequestStatus = AddSportRequestStatus.Accepted;
                        DbContext.AddSportRequests.AddOrUpdate(updateSport);
                    }

                    DbContext.SaveChanges();
                }
                catch (Exception e)
                {
                    ModelState.AddModelError("", "Something went wrong. Please try again");
                    return RedirectToAction("Create", model.AddSportRequestId);
                }
                return RedirectToAction("Index", "Home");
            }
            return RedirectToAction("Create", model.AddSportRequestId);
        }

        public ActionResult Details(int id)
        {
            var dbmodel = DbContext.Sports.FirstOrDefault(x => x.SportId == id);

            if (dbmodel == null)
                return HttpNotFound();

            var model = new SportModel
            {
                Name = dbmodel.Name,
                Description = dbmodel.Description,
                Rules = dbmodel.Rules
            };

            return View("Details", model);
        }

        [AllowAnonymous]
        public ActionResult SportsList()
        {
            var model = DbContext.Sports.Select(x => new SportModel
            {
                Name = x.Name,
                SportId = x.SportId

            }).ToList();

            return PartialView("_SportsListPartial", model);
        }
    }
}