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
            var dt = new DataTable();
            dt = GetEmployeeList();
            report.DataSources.Add(new ReportDataSource("dsEmployee", dt));
            var parameters = new[] { new ReportParameter("param1", "RDLC Sub-Report Example") };
            report.ReportPath = $"{this._webHostEnviorment.WebRootPath}\\Reports\\rptEmployee.rdlc";
            report.SetParameters(parameters);

            //For Sub Type =======>>>
            report.SubreportProcessing += new SubreportProcessingEventHandler(subReportProcessing);

            var pdf = report.Render(renderFromat);
            return File(pdf, mimetype, "report." + extension);
        }

        void subReportProcessing(object sender,SubreportProcessingEventArgs e)
        {
            int empId = int.Parse(e.Parameters["EmpId"].Values[0].ToString());
            var dt2 = GetEmployeeDetailsList().Select("EmpId=" + empId);
            ReportDataSource ds = new ReportDataSource("dsEmployeeDetails",dt2);
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
    }
}
