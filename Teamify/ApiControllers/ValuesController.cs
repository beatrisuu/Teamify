﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using DataTables.AspNet.Core;
using Teamify.Models.People;
using Teamify.Models.Sport;

namespace Teamify.ApiControllers
{
    [RoutePrefix("api/Values")]
    public class ValuesController : BaseApiController
    {
        [Route("GetValues/{someId}")]
        [HttpGet]
        public IHttpActionResult GetValues(int someId)
        {
            if (someId == 0) return BadRequest();
            return Ok(someId);
        }

        [Route("PostValue/{someId}")]
        [HttpPost]
        public IHttpActionResult PostValues([FromUri]int someId, [FromBody]object someObj)
        {
            return Ok(someObj);
        }

        [Route("GetSports")]
        [HttpGet]
        public IHttpActionResult GetSports()
        {
            var sports = Db.Sports.Select(x => new SportModel
            {
                Name = x.Name,
                Description = x.Description,
                Rules = x.Rules,
                SportId = x.SportId
            }).ToList();

            return Ok(sports);
        }

        [Route("GetPeople")]
        [HttpGet]
        public IHttpActionResult GetPeople(IDataTablesRequest objRequest)
        {
            var list = Db.UserProfiles.Select(x => new UserModel()
            {
                Name = x.LastName + x.FirstName,
                Bio = x.Bio,
                Rating = x.Rating

            }).ToList();

            return Ok(list);
        }
    }
}