using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ERP.Web.DXServices.Common
{
    public static class ReportUtils
    {
        public static IHostingEnvironment _hostingEnvironment { get; set; }
        public static byte[] GetImage(int TenantId)
        {
            string str = ConfigurationManager.AppSettings["ConnectionString"];
            Byte[] image = null;
            using (SqlConnection cn = new SqlConnection(str))
            {

                SqlCommand cmd = null;

                cmd = new SqlCommand("select Bytes from AppDocs inner join AppBinaryObjects on AppDocs.FileID = AppBinaryObjects.Id and AppDocs.TenantID = AppDocs.TenantID where appdocs.TenantId = " + TenantId + " and APPID = 3", cn);
                cn.Open();
                using (SqlDataReader rdr = cmd.ExecuteReader())
                {
                    while (rdr.Read())
                    {
                        image = (byte[])(rdr["Bytes"]);
                    }
                }

                if (image == null)
                {
                    var imgPath = _hostingEnvironment.WebRootPath + "\\Common\\Images\\No-Image.png";
                    using (Image noImage = Image.FromFile(imgPath))
                    {
                        using (MemoryStream m = new MemoryStream())
                        {
                            noImage.Save(m, noImage.RawFormat);
                            image = m.ToArray();

                        }
                    }
                }
            }
            return image;
        }
        public static string NumberToWords(Int64 number)        {            if (number == 0)                return "Zero";            if (number < 0)                return "Minus " + NumberToWords(Math.Abs(number));            string words = "";            if ((number / 1000000) > 0)            {                words += NumberToWords(number / 1000000) + " Million ";                number %= 1000000;            }            if ((number / 1000) > 0)            {                words += NumberToWords(number / 1000) + " Thousand ";                number %= 1000;            }            if ((number / 100) > 0)            {                words += NumberToWords(number / 100) + " Hundred ";                number %= 100;            }            if (number > 0)            {                if (words != "")                    words += "";                var unitsMap = new[] { "Zero", "One", "Two", "Three", "Four", "Five", "Six", "Seven", "Eight", "Nine", "Ten", "Eleven", "Twelve", "Thirteen", "Fourteen", "Fifteen", "Sixteen", "Seventeen", "Eighteen", "Nineteen" };                var tensMap = new[] { "zero", "Ten", "Twenty", "Thirty", "Forty", "Fifty", "Sixty", "Seventy", "Eighty", "Ninety" };                if (number < 20)                    words += unitsMap[number];                else                {                    words += tensMap[number / 10];                    if ((number % 10) > 0)                        words += "-" + unitsMap[number % 10];                }            }            return words;        }    }}
