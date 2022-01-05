using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DEMONEW.Models;
using PagedList;

namespace DEMONEW.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            MenuProvider menu = new MenuProvider();
            Session["menushow"] = menu.MainHoziontal();
            DEMODataContext context = new DEMODataContext();
            var pages = from p in context.PageItems.OrderBy(p => p.ID_P) where (p.ID_P == 49) select p;
            ViewBag.pages = pages;
            var homes = from p in context.PageItems.OrderBy(p => p.ID_P) where (p.ID_P == 1) select p;
            ViewBag.homes = homes;
            var sptas = from p in context.PageItems.OrderBy(p => p.ID_P) where (p.ID_P == 15) select p;
            ViewBag.sptas = sptas;
            var nnas = from p in context.PageItems.OrderBy(p => p.ID_P) where (p.ID_P == 16) select p;
            ViewBag.nnas = nnas;
            var nntqs = from p in context.PageItems.OrderBy(p => p.ID_P) where (p.ID_P == 17) select p;
            ViewBag.nntqs = nntqs;
            var page = from p in context.PageItems.OrderByDescending(p => p.ID_P).Take(4) select p;
            ViewBag.page = page;
            return View();
        }
        public ActionResult News(int? id)
        {
            DEMODataContext context = new DEMODataContext();
            var pages = from p in context.PageItems.OrderByDescending(p => p.ID_P) select p;
            int pagesize = 10;
            int pageindex = id ?? 1;
            ViewBag.page = pages;
            return View(pages.ToPagedList(pageindex, pagesize));
        }
        public ActionResult NewsNews(int? id, int? mn)
        {
            DEMODataContext context = new DEMODataContext();
            if (mn != null) Session.Add("mn", mn);

            var pages = from p in context.PageItems.OrderByDescending(p => p.ID_P)
                        where (p.ID_MN.ToString() == Session["mn"].ToString())
                        select p;
            int pagesize = 10;
            int pageindex = id ?? 1;
            return View(pages.ToPagedList(pageindex, pagesize));
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult FindNews(int? id, string key)
        {
            if (!string.IsNullOrEmpty(key))
            {
                ViewBag.key = key;
                DEMODataContext context = new DEMODataContext();
                var pages = from p in context.PageItems
                            .Where(p => p.Title.Contains(key) || p.Contents.Contains(key))
                            .OrderBy(p => p.ID_P)
                            select p;
                int pagesize = 10;
                int pageindex = id ?? 1;
                return View(pages.ToPagedList(pageindex, pagesize));
            }
            else
                return View();
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult FindNewsAll(int? id, string keyall)
        {
            if (!string.IsNullOrEmpty(keyall))
            {
                ViewBag.key = keyall;
                DEMODataContext context = new DEMODataContext();
                var pages = from p in context.PageItems
                            .Where(p => p.Title.Contains(keyall) || p.Contents.Contains(keyall))
                            .OrderBy(p => p.ID_P)
                            select p;
                int pagesize = 10;
                int pageindex = id ?? 1;
                return View(pages.ToPagedList(pageindex, pagesize));
            }
            else
                return View();
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult ViewNewsShow(int id)
        {
            DEMODataContext context = new DEMODataContext();
            var page = context.PageItems.Single(p => p.ID_P == id);
     
            var links = from p in context.PageItems.OrderByDescending(p => p.ID_P).Take(10) select p;
            ViewBag.links = links;
            return View(page);

        }
    }
}