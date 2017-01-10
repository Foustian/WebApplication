using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IQMedia.Web.Logic;
using IQMedia.Web.Logic.Base;
using IQMedia.Model;

namespace IQMedia.WebApplication.Controllers
{
    public class _HomeController : Controller
    {
        //
        // GET: /Home/

        public ActionResult Index()
        {
            CustomerLogic customerLogic = (CustomerLogic)LogicFactory.GetLogic(LogicType.Customer);
            IEnumerable<_CustomerModel> customerList = customerLogic.GetCustomerList();

            return View(customerList);
        }

    }
}
