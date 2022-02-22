using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using crudf.Models;
using MySql.Data.MySqlClient;

using Microsoft.Extensions.Configuration;


//function page[d]
namespace crudf.Controllers
{
    public class HomeController : Controller
    {
        private IConfiguration Configuration;

        public HomeController(IConfiguration _configuration)
        {
            Configuration = _configuration;
        }
        public IActionResult Index()

            //function for index page and fetch from db[d]
        {
            List<Customer> Customers = new List<Customer>();
            string connString = this.Configuration.GetConnectionString("DefaultConnection");

            using (MySqlConnection con = new MySqlConnection(connString))

            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand("select * from customer", con);
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Customer Customer = new Customer();
                    Customer.CustomerId = Convert.ToInt32(reader["CustomerId"]);
                    Customer.FullName = reader["FullName"].ToString();
                    Customer.CusCode = reader["CusCode"].ToString();
                    Customer.Location = reader["Location"].ToString();

                    Customers.Add(Customer);


                }
                reader.Close();

            }
            //view karan customer return karanva
                return View(Customers);
        }


        //delete function
        
        public  ActionResult Delete(int id)

        {
            string connString = this.Configuration.GetConnectionString("DefaultConnection");
            using (MySqlConnection con = new MySqlConnection(connString))

            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand("DELETE FROM customer Where CustomerId =" + id, con);
                MySqlDataReader reader = cmd.ExecuteReader();

                reader.Close();


            }

            return RedirectToAction("Index");
        }
            
        //create function
        public IActionResult Create()
        {
            return View();

        }

        [HttpPost]
        public IActionResult Create(int CustomerId, string FullName, string CusCode, string Location)
        {
            //connection eka hadanva
            string connString = this.Configuration.GetConnectionString("DefaultConnection");
            using (MySqlConnection con = new MySqlConnection(connString))

            {
                con.Open();
                var command = "insert into customer(FullName,CusCode,Location) values ('"+ FullName +"' ,'"+CusCode+"','"+Location+"')";
                MySqlCommand cmd = new MySqlCommand(command, con);
                MySqlDataReader reader = cmd.ExecuteReader();
                //reader kiyan ekn aragen eka ececute kranva query eken ena data tika ececute karal eliyata denva 
                reader.Close();


            }


            return View();
        }

        //edit 

        public IActionResult Edit(int id)
        {
            Customer Customer = new Customer();
            string connString = this.Configuration.GetConnectionString("DefaultConnection");
            using (MySqlConnection con = new MySqlConnection(connString))


            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand("select * from customer where CustomerId =" +id, con);
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    //extract data...fetch
                    Customer.CustomerId = Convert.ToInt32(reader["CustomerId"]);
                    Customer.FullName = reader["FullName"].ToString();
                    Customer.CusCode = reader["CusCode"].ToString();
                    Customer.Location = reader["Location"].ToString();
                    


                }
                reader.Close();
            }
                return View(Customer);
            
        }

        [HttpPost]
        public IActionResult Edit(int CustomerId, string FullName, string CusCode, string Location)

        {
            string connString = this.Configuration.GetConnectionString("DefaultConnection");
            using (MySqlConnection con = new MySqlConnection(connString))

            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand("UPDATE customer SET FullName = '"+FullName+"', CusCode = '"+CusCode+"' , Location = '"+Location+"' WHERE CustomerId =" +CustomerId, con);
                MySqlDataReader reader = cmd.ExecuteReader();

                reader.Close();


            }

            return RedirectToAction("Index");
        }


        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
