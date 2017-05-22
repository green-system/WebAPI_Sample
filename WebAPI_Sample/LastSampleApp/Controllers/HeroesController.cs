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
        private static readonly log4net.ILog logger = 
            log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public IQueryable<GAMA_BANK> Get()
        {
            List<GAMA_BANK> heroes;
            heroes = null;
            try
            {
                using (var context = new JBADBEntities())
                {
                    heroes = context.GAMA_BANK.ToList();
                    if (heroes == null)
                    {
                        throw new HttpResponseException(HttpStatusCode.NotFound);
                    }
                }
                //ログ出力
                logger.Info("BANK_CD=All");
                //List<T>をIQueryable<T>に変換する。
                return heroes.AsQueryable();
            }
            catch (System.UnauthorizedAccessException ex)
            {
                //ログ出力
                logger.Error("error_code=401 " + ex.Message);
                return null;
            }
            catch (System.NotImplementedException ex)
            {
                //ログ出力
                logger.Error("error_code=501 " + ex.Message);
                return null;
            }
            catch (HttpResponseException ex)
            {
                //ログ出力
                logger.Error(ex.Response + " " + ex.Message);
                return heroes.AsQueryable();
            }
        }

        public IQueryable<GAMA_BANK> Get(string bankcd)
        {
            List<GAMA_BANK> heroes;
            heroes = null;
            try
            {
                using (var context = new JBADBEntities())
                {
                    heroes = context.GAMA_BANK.Where(n => n.BANK_CD == bankcd).ToList();
                    if (heroes == null)
                    {
                        throw new HttpResponseException(HttpStatusCode.NotFound);
                    }
                }
                //ログ出力
                logger.Info("BANK_CD=" + bankcd);
                //List<T>をIQueryable<T>に変換する。
                return heroes.AsQueryable();
            }
            catch (System.UnauthorizedAccessException ex)
            {
                //ログ出力
                logger.Error("error_code=401 " + ex.Message);
                return heroes.AsQueryable();
            }
            catch (System.NotImplementedException ex)
            {
                //ログ出力
                logger.Error("error_code=501 " + ex.Message);
                return heroes.AsQueryable();
            }
            catch (HttpResponseException ex)
            {
                //ログ出力
                logger.Error(ex.Response + " " + ex.Message);
                return heroes.AsQueryable();
            }
        }
    }
}
