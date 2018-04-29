using System;
using System.Linq;
using System.Web.Mvc;
using billsSite.Models;
using System.Web.Script.Serialization;
using System.Web.Security;

namespace billsSite.Controllers
{

    public class HomeController : Controller
    {
        BillsContext db = new BillsContext();
        // GET: Home
        private string wrap(string s)
        {
           return  "{\"data\":" + s + "}";
        }

        public string SendData()
        {
            var rent = "\"rent\":" + new JavaScriptSerializer().Serialize(db.RentBills.OrderByDescending(s => s.month).ToList()) + "";
            var elect = "\"elect\":" + new JavaScriptSerializer().Serialize(db.ElectrisityNumbers.OrderByDescending(s => s.month).ToList()) + "";
            var meet = "\"meet\":" + new JavaScriptSerializer().Serialize(db.Meetings.OrderByDescending(s => s.month).ToList()) + "";
            var pay = "\"pay\":" + new JavaScriptSerializer().Serialize(db.Payments.OrderByDescending(s => s.month).ToList()) + "";
            var water = "\"water\":" + new JavaScriptSerializer().Serialize(db.WaterBills.OrderByDescending(s => s.month).ToList()) + "";
            var a = "{\"data\":{" + rent + "," + elect + "," + meet + "," + pay + "," + water + "}}";
            return a;
        }

        private  void clearDB()
        {

            db.WaterBills.RemoveRange(db.WaterBills);
            db.ElectrisityNumbers.RemoveRange(db.ElectrisityNumbers);
            db.RentBills.RemoveRange(db.RentBills);
            db.Meetings.RemoveRange(db.Meetings);
            db.Payments.RemoveRange(db.Payments);

            db.SaveChanges();
        }

        [HttpPost]
        public ActionResult CreateWaterBill(WaterBill wBill)
        {
            wBill.isPaid = false;
            db.WaterBills.Add(wBill);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult CreatePayment(Payment payment)
        {
            db.Payments.Add(payment);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult CreateMeeting(Meeting meet)
        {
            db.Meetings.Add(meet);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult CreateRentBill(RentBill rBill)
        {
            rBill.isPaid = false;
            db.RentBills.Add(rBill);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult CreateElectricBill(ElectrisityNumber eBill)
        {
            eBill.isPaid = false;
            db.ElectrisityNumbers.Add(eBill);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public string NewMeet(Meeting meet)
        {
            db.Meetings.Add(meet);
            db.SaveChanges();
            var meetJson = new JavaScriptSerializer().Serialize(db.Meetings.OrderByDescending(s => s.month).ToList()) ;
            return wrap(meetJson);
        }
        [HttpPost]
        public string NewEBill(ElectrisityNumber elect)
        {
            if(elect.month.Year< 2000)
                elect.month = DateTime.Now;
            if (db.ElectrisityNumbers.Any())
            {
                var lastElBil = db.ElectrisityNumbers.OrderByDescending(s => s.month).FirstOrDefault();
                if (elect.tariff < 1)
                    elect.tariff = lastElBil.tariff;
                elect.summRub = (-lastElBil.conterNumber + elect.conterNumber)*elect.tariff;
            }
            else if (elect.tariff < 1)
            {
                elect.tariff = 1;
                elect.summRub = 0;
            }

            db.ElectrisityNumbers.Add(elect);
            db.SaveChanges();
            var meetJson = new JavaScriptSerializer().Serialize(db.ElectrisityNumbers.OrderByDescending(s => s.month).ToList());
            return wrap(meetJson);
        }
        [HttpPost]
        public string NewRBill(RentBill rent)
        {
            db.RentBills.Add(rent);
            db.SaveChanges();
            var meetJson = new JavaScriptSerializer().Serialize(db.RentBills.OrderByDescending(s => s.month).ToList());
            return wrap(meetJson);
        }
        [HttpPost]
        public string NewWBill(WaterBill water)
        {
            db.WaterBills.Add(water);
            db.SaveChanges();
            var meetJson = new JavaScriptSerializer().Serialize(db.WaterBills.OrderByDescending(s => s.month).ToList());
            return wrap(meetJson);
        }
        [HttpPost]
        public string NewPay(Payment pay)
        {
            db.Payments.Add(pay);
            db.SaveChanges();
            var meetJson = new JavaScriptSerializer().Serialize(db.Payments.OrderByDescending(s => s.month).ToList());
            return wrap(meetJson);
        }
        [HttpPost]
        public string WaterBillPaid(int id)
        {
            var bill = db.WaterBills.Where(b=>b.id == id).FirstOrDefault();
            bill.isPaid = !bill.isPaid;
            db.SaveChanges();
            var water =new JavaScriptSerializer().Serialize(db.WaterBills.OrderByDescending(s => s.month).ToList());
            var a = "{\"data\":" + water+ "}";
            return a;
        }

        [HttpPost]
        public string ElectricBillPaid(int id)
        {
            var bill = db.ElectrisityNumbers.Where(b => b.id == id).FirstOrDefault();
            bill.isPaid = !bill.isPaid;
            db.SaveChanges();
            var elect = new JavaScriptSerializer().Serialize(db.ElectrisityNumbers.OrderByDescending(s => s.month).ToList());
            var a = "{\"data\":" + elect + "}";
            return a;
        }

        [HttpPost]
        public string RentBillPaid(int id)
        {
            var bill = db.RentBills.Where(b => b.id == id).FirstOrDefault();
            bill.isPaid = !bill.isPaid;
            db.SaveChanges();
            var rent = new JavaScriptSerializer().Serialize(db.RentBills.OrderByDescending(s => s.month).ToList());
            var a = "{\"data\":" + rent + "}";
            return a;
        }

        private void InitDb()
        {
            WaterBill wBill = new WaterBill();
            wBill.coldWaterNum = 56;
            wBill.hotWaterNum = 37;
            wBill.isPaid = false;
            wBill.summRub = 34;
            wBill.month = DateTime.Now;
            if(db.WaterBills.ToList().Capacity == 0)
                db.WaterBills.Add(wBill);

            ElectrisityNumber eBill = new ElectrisityNumber();
            eBill.conterNumber = 986;
            eBill.isPaid = false;
            eBill.summRub = 354;
            eBill.month = DateTime.Now;
            if (db.ElectrisityNumbers.ToList().Capacity == 0)
                db.ElectrisityNumbers.Add(eBill);

            RentBill rBill = new RentBill();
            rBill.isPaid = false;
            rBill.month = DateTime.Now;
            if (db.RentBills.ToList().Capacity == 0)
                db.RentBills.Add(rBill);

            Meeting meeting = new Meeting();
            meeting.month = DateTime.Now;
            if (db.Meetings.ToList().Capacity == 0)
                db.Meetings.Add(meeting);

            Payment payment = new Payment();
            payment.month = DateTime.Now;
            payment.summRub = 4;
            db.Payments.Add(payment);
            if (db.Payments.ToList().Capacity == 0)
                db.SaveChanges();
        }

        public ActionResult Index()
        {
          //  InitDb();
            
            Bills bills = new Bills();
            bills.WaterBills = db.WaterBills.OrderByDescending(s => s.month).ToList();
            bills.ElectrisityNumbers = db.ElectrisityNumbers.OrderByDescending(s => s.month).ToList();
            bills.RentBills = db.RentBills.OrderByDescending(s => s.month).ToList();
            bills.Meetings = db.Meetings.OrderByDescending(s => s.month).ToList();
            bills.Payments = db.Payments.OrderByDescending(s => s.month).ToList();
            return View(bills);
        }

        [HttpPost]
        public ActionResult Login(string login, string password)
        {
            //TODO: move password to db
            bool isLoginPass = (login == "admin") && (password == "some_pass1");
            if (isLoginPass)
            {
                FormsAuthentication.SetAuthCookie(login.ToLower(), true);
                return Redirect(Url.Action("Index", "Home"));
            }
            return RedirectToAction("Index");
        }

        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }

    }
}