using DEMONEW.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace DEMONEW.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult Index()
        {
            if (Session["username"].ToString() == "Login") return RedirectToAction("Login", "Admin");
            return View();
        }
        public ActionResult MenuList()
        {
            if (Session["username"].ToString() == "Login") return RedirectToAction("Login", "Admin");
            DEMODataContext context = new DEMODataContext();
            var parents = from p in context.Menus where (p.Parent == 0) select p;
            ViewBag.parents = parents;
            ViewBag.parent = parents.First().ID_MN;
            long id = ViewBag.parent;
            var menus = from m in context.Menus.OrderByDescending(m => m.ID_MN) where (m.Parent == id) select m;
            ViewBag.menus = menus;
            return View();
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult MenuList(FormCollection collection)
        {
           
            DEMODataContext context = new DEMODataContext();
            var parents = from p in context.Menus where (p.Parent == 0) select p;
            ViewBag.parents = parents;
            ViewBag.parent = collection.Get("ID_MN");
            long id = Convert.ToInt64(ViewBag.parent);
            var menus = from m in context.Menus.OrderBy(m => m.ID_MN) where (m.Parent == id) select m;
            ViewBag.menus = menus;
            return View();
        }
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult ViewMenu(int id)
        {
            DEMODataContext context = new DEMODataContext();
            var menu = context.Menus.Single(m => m.ID_MN == id);
            return View(menu);
        }
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult AddMenu()
        {
            if (Session["username"].ToString() == "Login") return RedirectToAction("Login", "Admin");
            DEMODataContext context = new DEMODataContext();
            var parents = from p in context.Menus where (p.Parent == 0) select p;
            ViewBag.parent = 0;
            ViewBag.idmnew = (context.Menus.Max(m => m.ID_MN) + 1).ToString();
            ViewBag.parents = parents;
            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public string AddMenu(FormCollection collection)
        {
            string error = "";
            string result = "";
            DEMODataContext context = new DEMODataContext();

            long id = 0;
            if (!string.IsNullOrEmpty(collection.Get("ID_MN")))
            {
                id = Convert.ToInt64(collection.Get("ID_MN"));
                if (context.Menus.SingleOrDefault(m => m.ID_MN == id) != null) error += "ID_MN đã tồn tại<br/>";
            }
            else
                error += "Chưa nhập ID_MN<br/>";
            string lable = "";
            if (!string.IsNullOrEmpty(collection.Get("Lable")))
                lable = collection.Get("Lable");
            else
                error += "Chưa nhập Lable<br/>";
            byte pos = 0;
            if (!string.IsNullOrEmpty(collection.Get("Pos")))
            {
                if (!byte.TryParse(collection.Get("Pos"), out pos)) error += "Pos phải là số<br/>";
            }
            else
                error += "Chưa nhập Pos<br/>";
            string url = "";
            if (!string.IsNullOrEmpty(collection.Get("UrlLink")))
                url = collection.Get("UrlLink");
            else
                error += "Chưa nhập UrlLink<br/>";
            long parent = 0;
            if (!string.IsNullOrEmpty(collection.Get("ID_Parent")))
                parent = Convert.ToInt64(collection.Get("ID_Parent"));
            else
                error += "Chưa chọn menu cha<br/>";
            if (!string.IsNullOrEmpty(error))
                result = error;
            else
            {
                Menus mn = new Menus();
                mn.ID_MN = id;
                mn.Lable = lable;
                mn.Pos = pos;
                mn.Parent = parent;
                mn.UrlLink = url;
                context.Menus.InsertOnSubmit(mn);
                context.SubmitChanges();
                result = "Lưu thành công. Bạn có thể thêm nữa!";
            }
            ViewBag.idmnew = (context.Menus.Max(m => m.ID_MN) + 1).ToString();
            var parents = from p in context.Menus where (p.Parent == 0) select p;
            ViewBag.parent = parent;
            ViewBag.parents = parents;
            return result;
        }
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult EditMenu(int id)
        {
            if (Session["username"].ToString() == "Login") return RedirectToAction("Login", "Admin");
            DEMODataContext context = new DEMODataContext();
            var menu = context.Menus.Single(m => m.ID_MN == id);
            ViewBag.parent = menu.Parent;
            var parents = from p in context.Menus where (p.Parent == 0) select p;
            ViewBag.parents = parents;
            return View(menu);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult EditMenu(FormCollection collection)
        {
            DEMODataContext context = new DEMODataContext();
            int id = Convert.ToInt32(collection.Get("ID_MN"));
            string lable = collection.Get("Lable");
            byte pos = Convert.ToByte(collection.Get("pos"));
            long parent = Convert.ToInt64(collection.Get("ID_Parent"));
            string url = collection.Get("UrlLink");
            Menus mn = context.Menus.Single(m => m.ID_MN == id);
            mn.Lable = lable;
            mn.Pos = pos;
            mn.Parent = parent;
            mn.UrlLink = url;
            context.SubmitChanges();
            return RedirectToAction("MenuList");
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult DeleteMenu(int id)
        {
            if (Session["username"].ToString() == "Login") return RedirectToAction("Login", "Admin");
            if (Session["permission"].ToString() == "1")
            {
                DEMODataContext context = new DEMODataContext();
                Menus menu = context.Menus.Single(m => m.ID_MN == id);
                context.Menus.DeleteOnSubmit(menu);
                context.SubmitChanges();
            }
            return RedirectToAction("MenuList");
        }
        public ActionResult PageList()
        {
            if (Session["username"].ToString() == "Login") return RedirectToAction("Login", "Admin");
            DEMODataContext context = new DEMODataContext();
            var pages = from p in context.PageItems.OrderByDescending(p => p.ID_P) select p;
            ViewBag.pages = pages;
            return View();
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult FindPage(int? id, string keyp)
        {
            if (!string.IsNullOrEmpty(keyp))
            {
                ViewBag.key = keyp;
                DEMODataContext context = new DEMODataContext();
                var pages = from p in context.PageItems
                            .Where(p => p.Title.Contains(keyp) || p.Contents.Contains(keyp))
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
        public ActionResult ViewPage(int id)
        {
            DEMODataContext context = new DEMODataContext();
            var page = context.PageItems.Single(p => p.ID_P == id);
            return View(page);
        }
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult AddPage()
        {
            if (Session["username"].ToString() == "Login") return RedirectToAction("Login", "Admin");
            ViewBag.Message = "Nhập thông tin";
            DEMODataContext context = new DEMODataContext();
            var parents = from p in context.Menus where (p.Parent != 0) select p;
            ViewBag.parent = parents.First().ID_MN;
            ViewBag.parent = 0;
            ViewBag.idpnew = (context.PageItems.Max(n => n.ID_P) + 1).ToString();
            ViewBag.parents = parents;
            return View();
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public string AddPage(FormCollection collection)
        {
            string error = "";
            string result = "";
            DEMODataContext context = new DEMODataContext();
            int id = 0;
            if (!string.IsNullOrEmpty(collection.Get("ID_P")))
            {
                id = Convert.ToInt32(collection.Get("ID_P"));
                if (context.PageItems.SingleOrDefault(p => p.ID_P == id) != null) error += "ID_P đã tồn tại<br/>";
            }
            else
                error += "Chưa nhập ID_P<br/>";

            long parent = 0;
            if (!string.IsNullOrEmpty(collection.Get("ID_Parent")))
                parent = Convert.ToInt64(collection.Get("ID_Parent"));
            else
                error += "Chưa chọn menu cha<br/>";
            string title = "";
            if (!string.IsNullOrEmpty(collection.Get("Title")))
            {
                title = collection.Get("Title");
            }
            else
                error += "Chưa nhập Title<br/>";
            string sumary = "";
            if (!string.IsNullOrEmpty(collection.Get("Sumary")))
                sumary = collection.Get("Sumary");
            else
                error += "Chưa nhập Sumary<br/>";

            if (!string.IsNullOrEmpty(error))
                result = error;
            else
            {
                PageItem page = new PageItem();
                page.ID_P = id;
                page.ID_MN = parent;
                page.Title = title;
                page.Sumary = sumary;
                context.PageItems.InsertOnSubmit(page);
                context.SubmitChanges();
                result = "Lưu thành công. Bạn có thể thêm nữa!";
            }
            ViewBag.idpnew = (context.PageItems.Max(n => n.ID_P) + 1).ToString();
            return result;
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult EditPage(int id)
        {
            if (Session["username"].ToString() == "Login") return RedirectToAction("Login", "Admin");
            DEMODataContext context = new DEMODataContext();
            var parents = from m in context.Menus where (m.Parent != 0) select m;
            ViewBag.parents = parents;
            var page = context.PageItems.Single(m => m.ID_P == id);
            ViewBag.parent = page.ID_MN;
            ViewBag.page = page;
            return View(page);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult EditPage(FormCollection collection, HttpPostedFileBase file)
        {
            DEMODataContext context = new DEMODataContext();
            int id = Convert.ToInt32(collection.Get("ID_P"));
            long parent = Convert.ToInt64(collection.Get("ID_Parent"));
            string title = collection.Get("Title");
            string sumary = collection.Get("Sumary");

            PageItem page = context.PageItems.Single(p => p.ID_P == id);
            page.ID_MN = parent;
            page.Title = title;
            page.Sumary = sumary;
            context.SubmitChanges();
            return RedirectToAction("PageList");
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult DeletePage(int id)
        {
            if (Session["username"].ToString() == "Login") return RedirectToAction("Login", "Admin");
            if (Session["permission"].ToString() == "1")
            {
                DEMODataContext context = new DEMODataContext();
                PageItem page = context.PageItems.Single(p => p.ID_P == id);
                context.PageItems.DeleteOnSubmit(page);
                context.SubmitChanges();
            }
            return RedirectToAction("PageList");
        }
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Uploads()
        {
            ViewBag.massage = "Chọn file cần upload";
            return View();
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Uploads(FormCollection collection, HttpPostedFileBase file, int id)
        {
            DEMODataContext context = new DEMODataContext();
            PageItem page = context.PageItems.Single(p => p.ID_P == id);
            try
            {
                if (Request.Files.Count > 0)
                {
                    if (file != null && file.ContentLength > 0)
                    {
                        string _FileName = id.ToString() + " " + Path.GetFileName(file.FileName);
                        if (!Path.GetExtension(_FileName).ToLower().Contains("jpg"))
                        {
                            ViewBag.Message = "Upload thất bại. File: jpg!";
                            return View();
                        }
                        string _path = Path.Combine(Server.MapPath("~/Uploads/Images"), _FileName);
                        file.SaveAs(_path);
                        page.Image = _FileName;
                        ViewBag.Message = "Upload thành công!";
                    }
                    context.SubmitChanges();
                }
            }
            catch
            {
                ViewBag.Message = "Uploads thất bại";
            }
            return RedirectToAction("PageList");
        }
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult ContentPage(int id)
        {
            DEMODataContext context = new DEMODataContext();
            var page = context.PageItems.Single(m => m.ID_P == id);
            return View(page);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        [ValidateInput(false)]
        public ActionResult ContentPage(FormCollection collection, int id)
        {
            DEMODataContext context = new DEMODataContext();
            string content = collection.Get("editor1");
            PageItem page = context.PageItems.Single(m => m.ID_P == id);
            page.Contents = content;
            context.SubmitChanges();
            return RedirectToAction("PageList");

        }

        public ActionResult AccList(string id)
        {
            if (Session["username"].ToString() == "Login") return RedirectToAction("Login", "Admin");
            DEMODataContext context = new DEMODataContext();
            var accs = from a in context.Accounts.OrderByDescending(a => a.Username) select a;
            ViewBag.accs = accs;
            return View();
        }
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult FindAcc(int? id, string keyacc)
        {
            if (!string.IsNullOrEmpty(keyacc))
            {
                ViewBag.key = keyacc;
                DEMODataContext context = new DEMODataContext();
                var pages = from p in context.Accounts
                            .Where(p => p.Fullname.Contains(keyacc) || p.Username.Contains(keyacc))
                            .OrderBy(p => p.Username)
                            select p;
                int pagesize = 10;
                int pageindex = id ?? 1;
                return View(pages.ToPagedList(pageindex, pagesize));
            }
            else
                return View();
        }
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult AddAcc()
        {
            if (Session["username"].ToString() == "Login") return RedirectToAction("Login", "Admin");
            ViewBag.Message = "Nhập thông tin";
            return View();
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public string AddAcc(FormCollection collection)
        {
            string error = "";
            string result = "";
            DEMODataContext context = new DEMODataContext();
            string username = "";
            if (!string.IsNullOrEmpty(collection.Get("Username")))
            {
                username = collection.Get("Username");
            }
            else
                error += "Chưa nhập username<br/>";
            string pass = "";
            if (!string.IsNullOrEmpty(collection.Get("Password")))
                pass = Security.MD5(collection.Get("Password"));
            else
                error += "Chưa nhập password<br/>";
            string fullname = "";
            if (!string.IsNullOrEmpty(collection.Get("Fullname")))
            {
                fullname = collection.Get("Fullname");
            }
            else
                error += "Chưa nhập fullname<br/>";
            byte per = 0;
            if (!string.IsNullOrEmpty(collection.Get("Permission")))
                per = Convert.ToByte(collection.Get("Permission"));
            else
                error += "Chưa nhập permission<br/>";
            string email= collection.Get("Email");
           
            if (!string.IsNullOrEmpty(error))
                result = error;
            else
            {
                Account ac = new Account();
                ac.Username = username;
                ac.Password = pass;
                ac.Fullname = fullname;
                ac.Permission = per;
                ac.Email = email;
                context.Accounts.InsertOnSubmit(ac);
                context.SubmitChanges();
                result = "Lưu thành công. Bạn có thể thêm nữa!";
            }
            ViewBag.users = (context.Accounts.Max(a => a.Username) == null);
            return result;
        }
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult EditAcc(string id)
        {
            if (Session["username"].ToString() == "Login") return RedirectToAction("Login", "Admin");
            DEMODataContext context = new DEMODataContext();
            var acc = context.Accounts.Single(a => a.Username == id);
            return View(acc);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult EditAcc(FormCollection collection, string id)
        {

            DEMODataContext context = new DEMODataContext();
            string username = collection.Get("Username");
            string fullname = collection.Get("Fullname");
            byte per = Convert.ToByte(collection.Get("Permission"));
            string email = collection.Get("Email");
            Account ac = context.Accounts.Single(a => a.Username == id);
            ac.Username = username;
            ac.Fullname = fullname;
            ac.Permission = per;
            ac.Email = email;

            context.SubmitChanges();

            return RedirectToAction("AccList");
        }
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult ChangePass()
        {
            if (Session["username"].ToString() == "Login") return RedirectToAction("Login", "Admin");
            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult ChangePass(FormCollection collection, string id)
        {
            DEMODataContext context = new DEMODataContext();
            Account acc = context.Accounts.Single(a => a.Username == id);

            acc.Password = Security.MD5(collection.Get("Password"));
            context.SubmitChanges();
            return RedirectToAction("AccList");
        }
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Reset(string id)
        {
            if (Session["username"].ToString() == "Login") return RedirectToAction("Login", "Admin");
            DEMODataContext context = new DEMODataContext();
            string pass = "12345";
            Account acc = context.Accounts.Single(a => a.Username == id);
            acc.Password = Security.MD5(pass);
            context.SubmitChanges();
            
            return RedirectToAction("AccList");
        }
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult LockAcc()
        {
            if (Session["username"].ToString() == "Login") return RedirectToAction("Login", "Admin");
            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult LockAcc(FormCollection collection, string id)
        {
            DEMODataContext context = new DEMODataContext();
            Account acc = context.Accounts.Single(a => a.Username == id);

            acc.Lock = Convert.ToBoolean(collection.Get("Lock"));
            context.SubmitChanges();
            return RedirectToAction("AccList");
        }
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Login()
        {
 
            return View();
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Login(FormCollection collection)
        {

            DEMODataContext context = new DEMODataContext();
            string us = collection.Get("Username");
            string p = Security.MD5(collection.Get("Password"));
            Account ac = context.Accounts.SingleOrDefault(a => a.Username == us && a.Password == p && a.Lock == false);
            if (ac != null)
            {
                Session["username"] = us;
                Session["permission"] = ac.Permission;
                ViewBag.message = "Đăng nhập thành công!";

                return RedirectToAction("MenuList", "Admin");
            }
            else
                ViewBag.message = "Đăng nhập thất bại!";
            return View();
        }
        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            Session.Abandon();
            Session.Clear();
            Session.RemoveAll();
            return RedirectToAction("Login", "Admin");
        }
    }

}