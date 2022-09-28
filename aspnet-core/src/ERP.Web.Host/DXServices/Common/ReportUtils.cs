﻿using Microsoft.AspNetCore.Hosting;
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
        public static string NumberToWords(Int64 number)