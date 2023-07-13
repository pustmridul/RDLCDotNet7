using GGCorporateSale.Report.ReportUtility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using rdlc.Contracts;
using System.ComponentModel;
using System.Data;
using System.Reflection;

namespace rdlc.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly ICompanyRepository _companyRepo;
        public ValuesController(ICompanyRepository companyRepository)
        {
                _companyRepo = companyRepository;
        }
        [HttpGet(Name = "GetRepot")]
        public  async Task<IActionResult> GetReport(string reportName="Report", string reportType = "PDF")
        {
            var companyList=await _companyRepo.GetCompanies();
            DataTable dataOrder = ConvertToDatatable(companyList);
            
            Dictionary<string, DataTable> data = new();
            Dictionary<string, string> param = new();
          
            data.Add("DataSet1", dataOrder);
            string fileDirPath = Assembly.GetExecutingAssembly().Location.Replace("rdlc.dll", string.Empty);
            string rdlcFilePath = string.Format("{0}Reports\\{1}.rdlc", fileDirPath, reportName);
          
            ReportDomain reportDomain = new(reportType, data, rdlcFilePath, param);
           // return  Ok(File(new ReportApplication().Load(reportDomain), reportDomain.mimeType, "Reports" + "." + reportType));
            return File(new ReportApplication().Load(reportDomain), reportDomain.mimeType, "Reports" + "." + reportType);

        }
        private static DataTable ConvertToDatatable<T>(List<T> data)
        {
            PropertyDescriptorCollection props = TypeDescriptor.GetProperties(typeof(T));
            DataTable table = new DataTable();
            for (int i = 0; i < props.Count; i++)
            {
                PropertyDescriptor prop = props[i];
                if (prop.PropertyType.IsGenericType && prop.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                    table.Columns.Add(prop.Name, prop.PropertyType.GetGenericArguments()[0]);
                else
                    table.Columns.Add(prop.Name, prop.PropertyType);
            }

            object[] values = new object[props.Count];
            foreach (T item in data)
            {
                for (int i = 0; i < values.Length; i++)
                {
                    values[i] = props[i].GetValue(item);
                }
                table.Rows.Add(values);
            }
            return table;
        }

        //public IActionResult CustomerLedgerDetailOverDueRpt(string CustomerId, string sDate, string eDate, bool effect = true, string inpurformat = "PDF"
        //{
        //    string UserReq = User.Identity.Name;
        //    SqlParameter[] commandParameters = new SqlParameter[3];
        //    commandParameters[0] = new SqlParameter("@CustomerId", CustomerId);
        //    if (effect == false)
        //    {
        //        commandParameters[1] = new SqlParameter("@sDate", null);
        //    }
        //    else
        //    {
        //        commandParameters[1] = new SqlParameter("@sDate", sDate);
        //    }
        //    if (effect == false)
        //    {
        //        commandParameters[2] = new SqlParameter("@eDate", null);
        //    }
        //    else
        //    {
        //        commandParameters[2] = new SqlParameter("@eDate", eDate);
        //    }
        //    DataTable dataOrder = icommonService.GetBySpWithParam("CS_SP_rptCustomerLedgerDetailOverDue", commandParameters);
        //    DataTable dataHeader = icommonService.GetDatatableBySQL("select * from dbo.ShopSetup where ShopID='" + shopId + "'";
        //    Dictionary<string, DataTable> data = new();
        //    Dictionary<string, string> param = new();
        //    param.Add("UserReq", UserReq);
        //    param.Add("sDate", sDate);
        //    param.Add("eDate", eDate);
        //    if ((!string.IsNullOrEmpty(CustomerId)) && (dataOrder.Rows.Count > 0))
        //    {
        //        param.Add("CutomerName", dataOrder.Rows[0]["CustomerName"].ToString());
        //    }
        //    else
        //    {
        //        param.Add("CutomerName", null);
        //    }
        //    data.Add("DataSet1", dataOrder);
        //    var path = $"{this.iWebHostEnvironment.ContentRootPath}\\Report\\rptCustomerLedgerDueDetail.rdlc";
        //    ReportDomain reportDomain = new(inpurformat, data, path, param);
        //    return Ok(File(new ReportApplication().Load(reportDomain), reportDomain.mimeType, "CustomerLedgerDetail" + "." + inpurformat));
        //}
    }
}
