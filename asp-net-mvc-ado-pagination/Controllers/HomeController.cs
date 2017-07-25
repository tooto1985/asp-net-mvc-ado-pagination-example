using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using asp_net_mvc_ado_pagination.Models;
using System.Data;
using System.Text;

namespace asp_net_mvc_ado_pagination.Controllers
{
    public class HomeController : Controller
    {
        private DatabaseModel dm = new DatabaseModel();

        public ActionResult Index()
        {
            DataTable Data = dm.Query("select * from products");
            ViewBag.Data = Data;
            return View();
        }

        public ActionResult Filter(string q = "")
        {
            StringBuilder sb = new StringBuilder();
            DataTable Data = dm.Query(sb.AppendFormat("select * from products where ProductName like '%{0}%'", q).ToString());
            ViewBag.Data = Data;
            ViewBag.q = q;
            return View("Index");
        }

        public ActionResult Pagination(int size = 10, int num = 0)
        {
            int Total = (int)dm.Query("select count(*) from products").Rows[0][0];
            StringBuilder sb = new StringBuilder();
            DataTable Data = dm.Query(sb.AppendFormat("select top {0} * from products where productID not in (select top {1} productID from products)", size, num).ToString());
            ViewBag.Data = Data;
            ViewBag.Size = size;
            ViewBag.Num = num;
            ViewBag.Total = Total;
            return View("Index");
        }

        public ActionResult Sorting(string orderby = "ProductID")
        {
            StringBuilder sb = new StringBuilder();
            DataTable Data = dm.Query(sb.AppendFormat("select * from products order by {0}", orderby).ToString());
            ViewBag.Data = Data;
            return View("Index");
        }

        public ActionResult Mixing(string q = "", int size = 10, int num = 0, string orderby = "ProductID")
        {
            StringBuilder where = new StringBuilder();
            if (!string.IsNullOrEmpty(q))
            {
                where.AppendFormat(" and ProductName like '%{0}%'", q);
            }

            StringBuilder order = new StringBuilder();
            if (orderby != "ProductID")
            {
                order.AppendFormat(" order by {0}", orderby);
            }
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("select top {0} * from products where productID not in (select top {1} productID from products where 1=1{2}{3}){2}{3}", size, num, where, order);



            int Total = (int)dm.Query("select count(*) from products where 1=1" + where).Rows[0][0];
            DataTable Data = dm.Query(sb.ToString());

            ViewBag.q = q;
            ViewBag.Data = Data;
            ViewBag.Size = size;
            ViewBag.Num = num;
            ViewBag.Total = Total;
            ViewBag.OrderBy = orderby;


            return View("Index");
        }
    }
}