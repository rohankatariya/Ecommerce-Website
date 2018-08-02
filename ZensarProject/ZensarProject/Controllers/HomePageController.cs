using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZensarProject.Models;
using ZensarProject.DAL;
using System.Data.SqlClient;
using System.Data;
namespace ZensarProject.Controllers
{
    public class HomePageController : Controller
    {
        SqlConnection con =new SqlConnection(@"Data Source=DESKTOP-8U502A3\SQLSERVER;Initial Catalog=ZensarProject;Integrated Security=True;MultipleActiveResultSets=True;Application Name=EntityFramework");
        DataSet ds;
        SqlDataAdapter da;
        SqlCommandBuilder cb;
        public DataSet GetData(string a)
        {
            string str = "select * from "+a;
            da = new SqlDataAdapter(str, con);
            da.MissingSchemaAction = MissingSchemaAction.AddWithKey;
            ds = new DataSet();
            da.Fill(ds, a);
            return ds;
        }
        public DataSet Get(string a,int i)
        {
            string str = "select * from " + a+" where Category_Id="+i;
            da = new SqlDataAdapter(str, con);
            da.MissingSchemaAction = MissingSchemaAction.AddWithKey;
            ds = new DataSet();
            da.Fill(ds, a);
            return ds;
        }
        // GET: HomePage
        public ActionResult Index()
        {       
            return View();
        }
        public ActionResult LogIn()
        {
            
            return View();
        }
        [HttpPost]
        public ActionResult LogIn(Registration cd)
        {
            ds = GetData("Registration");
          

            DataRow dr = ds.Tables["Registration"].Rows.Find(cd.Email);
            ViewBag.m = dr["Email"];
            ViewBag.n = dr["Password"];
            if (dr != null)
            {
                if (dr["Password"].ToString()==cd.Password)
                    return RedirectToAction("Home_Appliance");
                else
                    return RedirectToAction("/HomePage/LogIn");
            }
            else
            {
                return RedirectToAction("/LogIn");
            }
        }
        public ActionResult Sign_up()
        {
            List<string> li = new List<string>();
            li.Add("Select State");
            li.Add("Maharashtra");
            li.Add("Gujrat");
            li.Add("TamilNadu");
            li.Add("Karnatak");
            li.Add("Goa");
            li.Add("Bangal");
            li.Add("AndhraPradesh");
            ViewData["state"] = new SelectList(li);
            return View();
        }
        [HttpPost]
        public ActionResult Sign_up(Registration re, FormCollection fc)
        {
            List<string> li = new List<string>();
            li.Add("Select State");
            li.Add("Maharashtra");
            li.Add("Gujrat");
            li.Add("TamilNadu");
            li.Add("Karnatak");
            li.Add("Goa");
            li.Add("Bangal");
            li.Add("AndhraPradesh");
            ViewData["state"] = new SelectList(li);

            ds = GetData("Registration");
                if (re.Password == re.ConfirmPassword)
                {
                try
                {
                    DataRow dr = ds.Tables["Registration"].NewRow();
                    dr["UserName"] = re.UserName;
                    dr["FName"] = re.FName;
                    dr["LName"] = re.LName;
                    dr["Password"] = re.Password;
                    dr["Email"] = re.Email;
                    dr["Gender"] = fc["r1"];
                    dr["State"] = fc["state"];

                    ds.Tables["Registration"].Rows.Add(dr);
                    cb = new SqlCommandBuilder(da);
                    int res = da.Update(ds.Tables["Registration"]);
                    if (res > 0)
                    {
                        return RedirectToAction("Home_Appliance");
                    }
                    else
                    {
                        ViewBag.adduser = "User Not added due to some internal issues pleas try again";
                        return View();
                    }
                }
                catch (InvalidOperationException i)
                {
                    ViewBag.adduser = "Sorry Something Went wrong...You entered invalid details";
                    return View("");
                }
            }

            else
            {
                ViewBag.conf = "Password donot Match";
                return View();
            }
        

}
        public ActionResult Mobile()
        {
            int b = 5;
            Disp(b);
            return View("Disp");
        }
        [HttpPost]
        public ActionResult Mobile(int id)
        {
            List<Product> li;
            if (Session["id"] != null)
            {
                li = (List<Product>)Session["id"];
            }
            else
            {
                li = new List<Product>();
            }
            Product imp=new Product();
            ds = GetData("Product");
            DataRow dr=ds.Tables["Product"].Rows.Find(id);
            foreach (DataRow item in dr.Table.Rows)
            {
                li.Add(new Product { Product_Id = Convert.ToInt32(item[1]), Product_Name = item[2].ToString(), Product_Price = Convert.ToInt32(item[3]), image = item[4].ToString() });
            }
            li.Add(imp);
            Session["id"] = li;
            return View();
        }

        public ActionResult Home_Appliance()
        {
            int b=1;
            Disp(b);
         return View("Disp");
        }
        [HttpPost]
        public ActionResult Home_Appliance(int id)
        {

            int b = 1;
            Disp(b);
            return View("Disp");
        }
        public ActionResult Furniture()
        {
            int b = 3;
            Disp(b);
            return View("Disp");
        }
        public ActionResult Electronics()
        {
            int b = 2;
            Disp(b);
            return View("Disp");
        }
        public ActionResult Mens_Wear()
        {
            int b = 4;
            Disp(b);
            return View("Disp");
        }
        public ActionResult Woman_Wear()
        {
            int b = 6;
            Disp(b);
            return View("Disp");
        }
        public void Disp(int n)
        {
            ds = Get("Product",n);
            DataView dv = ds.Tables["Product"].DefaultView;
            dv.RowFilter = "Category_Id=" + n;
            List<Product> prodlst = new List<Product>();
            foreach (DataRow item in dv.Table.Rows)
            {
                prodlst.Add(new Product { Product_Id=Convert.ToInt32(item[1]), Product_Name=item[2].ToString(),Product_Price=Convert.ToInt32( item[3]),image=item[4].ToString()});
            }
            ViewBag.prodlist = prodlst;           
        }
        public ActionResult Admin_Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Admin_Login(Registration dt)
        {
            if (dt.Email=="Admin" && dt.Password=="Admin")
            {
                
                return View("Admin_Add_Item");
            }
            else
            {
                return View("Admin_Login");
            }
        }
        public ActionResult Admin_Add_Item()
        {

            return View();
        }
        [HttpPost]
        public ActionResult Admin_Add_Item(Product p)
        {
            ds = GetData("Product");
           
                try
                {
                    DataRow dr = ds.Tables["Product"].NewRow();
                    dr["Category_Id"] = p.Category_Id;
                    dr["Product_Name"] = p.Product_Name;
                    dr["Product_Price"] = p.Product_Price;
                    dr["image"] = p.image;
                    ds.Tables["Product"].Rows.Add(dr);
                    cb = new SqlCommandBuilder(da);
                    int res = da.Update(ds.Tables["Product"]);
                    if (res > 0)
                    {
                        return RedirectToAction("Home_Appliance");
                    }
                    else
                    {
                        ViewBag.adduser = "User Not added due to some internal issues pleas try again";
                        return View();
                    }
                }
                catch (InvalidOperationException i)
                {
                    ViewBag.adduser = "Sorry Something Went wrong...You entered invalid details";
                    return View();
                }
        }
        public ActionResult Cart(int id)
        {
            return RedirectToAction("Home_Appliance");
        }
        public ActionResult Payment()
        {
            return View();
        }
        
    }
}