using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Reporting.NETCore;
using RdlReport.Models;

namespace RdlReport.Controllers
{
    public class HomeController : Controller
    {
        private List<EmployeeDemo> GetEmployeeDemoList = new List<EmployeeDemo>()
        {
                new EmployeeDemo() {ID = 1, Name = "Sam", Gender ="male", Email="sam@gmail.com"},
                new EmployeeDemo() {ID = 1, Name = "Ella",Gender ="female", Email="Ella@gmail.com" },
                new EmployeeDemo() {ID = 1, Name = "TG",Gender ="male", Email ="TG@gmail.com" },
                new EmployeeDemo() {ID = 1, Name = "Favor",Gender ="female", Email="Favor@gmail.com" },
                new EmployeeDemo() {ID = 2,Name = "Micheal",Gender ="male",Email ="Micheal@gmail.com" },
                new EmployeeDemo() {ID = 2, Name = "Joe",Gender ="male", Email ="Joe@gmail.com" },
                new EmployeeDemo() {ID = 2, Name="Maintain",Gender ="female",Email ="Maintain@gmail.com" },
                new EmployeeDemo() {ID = 3, Name = "Akeem",Gender ="male", Email ="Akeem@gmail.com" },
                new EmployeeDemo() {ID = 3, Name = "Boye",Gender ="male", Email ="Boye@gmail.com" },
                new EmployeeDemo() {ID = 4, Name ="Chioma",Gender ="female",Email ="Chioma@gmail.com" },
                new EmployeeDemo() {ID = 4, Name = "Ofure",Gender ="female", Email ="Ofure@gmail.com" },
                new EmployeeDemo() {ID = 4, Name = "Hart",Gender ="male",Email ="Hart@gmail.com" }
        };

        private List<Department> departments = new List<Department>()
        {
               new Department() {ID = 1, Name = "Applied Mathematics" },
               new Department() {ID = 2, Name = "Software" },
               new Department() {ID = 3, Name = "Machine Learning" },
               new Department() {ID = 4, Name = "Petroleum Engineering" },
        };

        private readonly ILogger<HomeController> _logger;
        private readonly IWebHostEnvironment _webHostEnviorment;

        public HomeController(ILogger<HomeController> logger, IWebHostEnvironment webHostEnviorment)
        {
            _logger = logger;
            _webHostEnviorment = webHostEnviorment;
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Print()
        {
            string renderFromat = "pdf";
            string mimetype = "application/pdf";
            string extension = "pdf";

            Dictionary<string, string> parameter = new Dictionary<string, string>();
            parameter.Add("ReportParameter1", "Well Come to RDLC Report");
            LocalReport localReport = new LocalReport();
            localReport.ReportPath= $"{this._webHostEnviorment.WebRootPath}\\Reports\\Report1.rdlc";
            var pdf = localReport.Render(renderFromat);
            return File(pdf, mimetype, "report." + extension);
        }

        public IActionResult PrintReport()
         {
            string renderFromat = "PDF";
            string extension = "pdf";
            string mimetype = "application/pdf";
            using var report = new LocalReport();
          //  var dt = new DataTable();
            var  dt =departments;
            report.DataSources.Add(new ReportDataSource("Department_DS", dt));
            var parameters = new[] { new ReportParameter("prm1", "RDLC Sub-Report Example") };
            report.ReportPath = $"{this._webHostEnviorment.WebRootPath}\\Reports\\rptDepartment.rdlc";
            report.SetParameters(parameters);

            //For Sub Type =======>>>
            report.SubreportProcessing += new SubreportProcessingEventHandler(subReportProcessing);

            var pdf = report.Render(renderFromat);
            return File(pdf, mimetype, "report." + extension);
        }

        void subReportProcessing(object sender,SubreportProcessingEventArgs e)
        {
            var ID = Convert.ToInt32(e.Parameters[0].Values[0]);
          //  var employeegroup = Employees.FindAll(x => x.ID == ID);

           int empId = Convert.ToInt32((e.Parameters["ID"].Values[0].ToString()));
            //var dt2 = GetEmployeeDetailsList().Select("EmpId=" + empId);

            var dt = GetEmployeeDemoList;

            var result = dt.FindAll(x => x.ID == empId);
            var res = result;
            ReportDataSource ds = new ReportDataSource("EmployeeDemo_DS", result);
            e.DataSources.Add(ds);
        }
        public DataTable GetEmployeeDetailsList()
        {
            var dt = new DataTable();
            dt.Columns.Add("EmpId");
            dt.Columns.Add("Details");
           
            DataRow row;
            for (int i = 101; i <= 120; i++)
            {
                for (int j = 0; j <=3; j++)
                {
                    row = dt.NewRow();
                    row["EmpId"] = i;
                    row["Details"] = "Chile =" + j;
                    dt.Rows.Add(row);
                }
               
            }
            return dt;
        }
        public DataTable GetEmployeeList()
        {
            var dt = new DataTable();
            dt.Columns.Add("EmpId");
            dt.Columns.Add("EmpName");
            dt.Columns.Add("Department");
            dt.Columns.Add("Designation");
            DataRow row;
            for (int i = 101; i < 120; i++)
            {
                row = dt.NewRow();
                row["EmpId"] = i;
                row["EmpName"] = "Mr.Robert"+i;
                row["Department"] ="Software";
                row["Designation"] ="Software Engnineer";
                dt.Rows.Add(row);
            }
            return dt;
        }

        public List<Employee> GetEmployee = new List<Employee>()
        {
                new Employee{EmpId=1,EmpName="Abir",Department="Software",Designation="Asso. Software Enginner"},
                new Employee{EmpId=2,EmpName="Nayeem",Department="Software",Designation="Software Enginner"},
                new Employee{EmpId=3,EmpName="Anik",Department="Marketing",Designation="Junior Excutive"},
                new Employee{EmpId=4,EmpName="Sabbir",Department="Marketing",Designation="Junior Excutive"},
                new Employee{EmpId=5,EmpName="Tania",Department="HR",Designation="Senior Excutive"}
        };


        public List<EmployeeDetails> GetEmployeeDetails = new List<EmployeeDetails>()
        {
                new EmployeeDetails{EmpId=1,Details="Personal Reason1"},
                new EmployeeDetails{EmpId=1,Details="Personal Reason1"},
                new EmployeeDetails{EmpId=1,Details="Personal Reason1"},
                new EmployeeDetails{EmpId=2,Details="Personal Reason2"},
                new EmployeeDetails{EmpId=2,Details="Personal Reason2"},
                new EmployeeDetails{EmpId=2,Details="Personal Reason2"},
                new EmployeeDetails{EmpId=3,Details="Personal Reason3"},
                new EmployeeDetails{EmpId=3,Details="Personal Reason3"},
                new EmployeeDetails{EmpId=3,Details="Personal Reason3"},
                new EmployeeDetails{EmpId=4,Details="Personal Reason4"},
                new EmployeeDetails{EmpId=4,Details="Personal Reason4"},
                new EmployeeDetails{EmpId=4,Details="Personal Reason4"},
                new EmployeeDetails{EmpId=5,Details="Personal Reason5"},
                new EmployeeDetails{EmpId=5,Details="Personal Reason5"},
                new EmployeeDetails{EmpId=5,Details="Personal Reason5"},
        };


        public IActionResult PrintView()
        {
            string renderFromat = "PDF";
            string extension = "pdf";
            string mimetype = "application/pdf";
            using var report = new LocalReport();
            //  var dt = new DataTable();
            var dt = GetEmployeeDemoList;
            report.DataSources.Add(new ReportDataSource("EmployeeDemo_DS", dt));
            var parameters = new[] { new ReportParameter("prm1", "RDLC Sub-Report Example") };
            report.ReportPath = $"{this._webHostEnviorment.WebRootPath}\\Reports\\rptEmployeeBase.rdlc";
            report.SetParameters(parameters);

            //For Sub Type =======>>>
            report.SubreportProcessing += new SubreportProcessingEventHandler(employeeSubReportProcessing);

            var pdf = report.Render(renderFromat);
            return File(pdf, mimetype, "report." + extension);
        }

        void employeeSubReportProcessing(object sender, SubreportProcessingEventArgs e)
        {
            int empId = Convert.ToInt32((e.Parameters["ID"].Values[0].ToString()));
            var dt = departments;
            var result = dt.FindAll(x => x.ID == empId);
            ReportDataSource ds = new ReportDataSource("Department_DS", result);
            e.DataSources.Add(ds);
        }


    }
}
