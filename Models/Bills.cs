using System;
using System.Collections.Generic;
using System.Data.Entity;


/*
 Add-Migration AddDescriptionPriceToProduct
 Update-Database
     
     */
namespace billsSite.Models
{

    public class ElectrisityNumber
    {
        public int id { get; set; }
        public int conterNumber { get; set; }
        public DateTime month { get; set; }
        public double summRub { get; set; }
        public double tariff { get; set; }
        public bool isPaid { get; set; }
    }
    public class WaterBill
    {
        public int id { get; set; }
        public DateTime month { get; set; }
        public double summRub { get; set; }
        public int coldWaterNum { get; set; }
        public int hotWaterNum { get; set; }
        public bool isPaid { get; set; }
    }

    public class RentBill
    {
        public int id { get; set; }
        public DateTime month { get; set; }
        public bool isPaid { get; set; }
    }

    public class Payment
    {
        public int id { get; set; }
        public DateTime month { get; set; }
        public double summRub { get; set; }
        public int debt { get; set; }
    }

    public class Meeting
    {
        public int id { get; set; }
        public DateTime month { get; set; }
    }

    public class Bills
    {
        public List<ElectrisityNumber> ElectrisityNumbers { get; set; }
        public List<WaterBill> WaterBills { get; set; }
        public List<RentBill> RentBills { get; set; }
        public List<Payment> Payments { get; set; }
        public List<Meeting> Meetings { get; set; }
    }

    public class BillsContext : DbContext
    {
        public DbSet<ElectrisityNumber> ElectrisityNumbers { get; set; }
        public DbSet<WaterBill> WaterBills { get; set; }
        public DbSet<RentBill> RentBills { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Meeting> Meetings { get; set; }
    }
}