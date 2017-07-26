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

        public ActionResult Filter(string ProductName = "", string SupplierID = "", string CategoryID = "", string QuantityPerUnit = "", string UnitPrice = "")
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("select * from products where 1=1");
            if (!string.IsNullOrEmpty(ProductName)) {
                sb.AppendFormat(" and ProductName like '{0}'", ProductName);
            }
            if (!string.IsNullOrEmpty(SupplierID))
            {
                sb.AppendFormat(" and SupplierID like '{0}'", SupplierID);
            }
            if (!string.IsNullOrEmpty(CategoryID))
            {
                sb.AppendFormat(" and CategoryID like '{0}'", CategoryID);
            }
            if (!string.IsNullOrEmpty(QuantityPerUnit))
            {
                sb.AppendFormat(" and QuantityPerUnit like '{0}'", QuantityPerUnit);
            }
            if (!string.IsNullOrEmpty(UnitPrice))
            {
                sb.AppendFormat(" and UnitPrice like '{0}'", UnitPrice);
            }
            DataTable Data = dm.Query(sb.ToString());
            ViewBag.Data = Data;
            ViewBag.ProductName = ProductName;
            ViewBag.SupplierID = SupplierID;
            ViewBag.CategoryID = CategoryID;
            ViewBag.QuantityPerUnit = QuantityPerUnit;
            ViewBag.UnitPrice = UnitPrice;
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

        public ActionResult Mixing(string ProductName = "", string SupplierID = "", string CategoryID = "", string QuantityPerUnit = "", string UnitPrice = "", int size = 10, int num = 0, string orderby = "ProductID")
        {
            StringBuilder where = new StringBuilder();
            if (!string.IsNullOrEmpty(ProductName))
            {
                where.AppendFormat(" and ProductName like '%{0}%'", ProductName);
            }
            if (!string.IsNullOrEmpty(SupplierID))
            {
                where.AppendFormat(" and SupplierID like '{0}'", SupplierID);
            }
            if (!string.IsNullOrEmpty(CategoryID))
            {
                where.AppendFormat(" and CategoryID like '{0}'", CategoryID);
            }
            if (!string.IsNullOrEmpty(QuantityPerUnit))
            {
                where.AppendFormat(" and QuantityPerUnit like '{0}'", QuantityPerUnit);
            }
            if (!string.IsNullOrEmpty(UnitPrice))
            {
                where.AppendFormat(" and UnitPrice like '{0}'", UnitPrice);
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

            ViewBag.ProductName = ProductName;
            ViewBag.SupplierID = SupplierID;
            ViewBag.CategoryID = CategoryID;
            ViewBag.QuantityPerUnit = QuantityPerUnit;
            ViewBag.UnitPrice = UnitPrice;
            ViewBag.Data = Data;
            ViewBag.Size = size;
            ViewBag.Num = num;
            ViewBag.Total = Total;
            ViewBag.OrderBy = orderby;


            return View("Index");
        }
    }
}