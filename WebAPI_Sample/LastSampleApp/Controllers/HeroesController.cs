using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using LastSampleApp.Models;

namespace LastSampleApp.Controllers
{
    public class HeroesController : ApiController
    {
        public IQueryable<GAMA_BANK> Get()
        {
            List<GAMA_BANK> heroes;
            using (var context = new JBADBEntities())
            {
                heroes = context.GAMA_BANK.ToList();
            }
            //List<T>をIQueryable<T>に変換する。
            return heroes.AsQueryable();

        }
        public IQueryable<GAMA_BANK> Get(string bankcd)
        {
            List<GAMA_BANK> heroes;

            using (var context = new JBADBEntities())
            {
                heroes = context.GAMA_BANK.Where(n => n.BANK_CD == bankcd).ToList();
                if (heroes == null)
                {
                    throw new HttpResponseException(HttpStatusCode.NotFound);
                }
            }
            //List<T>をIQueryable<T>に変換する。
            return heroes.AsQueryable();
        }
    }
}
