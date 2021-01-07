using CSCAssignment.Task4.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace CSCAssignment.Task4.Controller
{
    public class TalentController : ApiController
    {
        static readonly TalentRepository repository = new TalentRepository();

        [EnableCors(origins: "*", headers: "*", methods: "*")]
        [Route("api/t4/talents")]
        public IEnumerable<Talent> GetAllTalents()
        {
            return repository.GetAll();
        }

        [Route("api/t4/talents/{id:int}")]
        public Talent GetTalent(int id)
        {
            Talent item = repository.Get(id);
            if (item == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            return item;
        }


    }
}
