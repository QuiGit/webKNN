using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DEMONEW.Models;
namespace DEMONEW.Controllers
{
    public class MenuProvider
    {
        public string MainHoziontal()
        {
            DEMODataContext context = new DEMODataContext();
            var menus = from m in context.Menus.Where(m => m.Parent == 0) select m;
            if (menus != null)
            {
                string listMenu = "<ul class='menu-ngang'>";
                foreach (Menus m in (menus as IEnumerable<Menus>))
                {
                    listMenu = listMenu + "<li><a href='" + m.UrlLink + "'>" + m.Lable + " <i class='fas fa-caret-down'></i></a>";
                    listMenu = listMenu + Main1Hoziontal(Convert.ToInt64(m.ID_MN)) + "</li>";
                }
                return listMenu + "</ul>";
            }
            else
            {
                return "";
            }
        }
        protected string Main1Hoziontal(long id)
        {
            DEMODataContext context = new DEMODataContext();
            var menus = from m in context.Menus.Where(m => m.Parent == id) select m;

            if (menus != null)
            {
                string listMenu = "";

                foreach (Menus m in (menus as IEnumerable<Menus>))
                {
                    listMenu = listMenu + "<li><a href=http://localhost:49470/Home/NewsNews/1?mn=" + m.ID_MN + ">" + m.Lable + "</a></li>";
                }
                return (listMenu.Length == 0) ? listMenu : "<ul class='menu-con'>" + listMenu + "</ul>";
            }
            else
            {
                return "";
            }
        }


    }
}