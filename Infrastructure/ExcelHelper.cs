using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;

namespace Infrastructure
{
    public class ExcelHelper
    {
        private IHostingEnvironment _hostingEnvironment;
        public static string ExcelContentType
        {
            get
            {
                return "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            }
        }
        public ExcelHelper(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        private ExcelHelper() { }

        /// <summary>
        /// Excel文件 Content-Type
        /// </summary>
        private const string Excel = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

        /// <summary>
        /// Excel导出
        /// </summary>
        /// <param name="keyValuePairs">字典表【名称，数据】</param>
        /// <param name="sWebRootFolder">网站根文件夹</param>
        /// <param name="tuple">item1:The virtual path of the file to be returned.|item2:The Content-Type of the file</param>
        public static string Export(Dictionary<string, DataTable> keyValuePairs, out Tuple<string, string> tuple)
        {
            ExcelHelper excle = new ExcelHelper();

            string sWebRootFolder = @"wwwroot\ExportData\";
            if (string.IsNullOrWhiteSpace(sWebRootFolder))
                tuple = Tuple.Create("", Excel);
            string sFileName = $"{DateTime.Now.ToString("yyyyMMddHHmmssfff")}-{Guid.NewGuid()}.xlsx";
            FileInfo file = new FileInfo(Path.Combine(sWebRootFolder, sFileName));
            using (ExcelPackage package = new ExcelPackage(file))
            {
                foreach (var item in keyValuePairs)
                {
                    string worksheetTitle = item.Key; //表名称
                    var dt = item.Value; //数据表

                    // 添加worksheet
                    ExcelWorksheet worksheet = package.Workbook.Worksheets.Add(worksheetTitle);

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        for (int j = 0; j < dt.Columns.Count; j++)
                        {
                            if (i == 0)
                            {
                                //添加表头
                                worksheet.Cells[i + 1, j + 1].Value = dt.Columns[j].ColumnName;
                                worksheet.Cells[i + 1, j + 1].Style.Font.Bold = true;
                                //添加值
                                worksheet.Cells[i + 2, j + 1].Value = dt.Rows[i][j].ToString();
                            }
                            else
                            {
                                //添加值
                                worksheet.Cells[i + 2, j + 1].Value = dt.Rows[i][j].ToString();
                            }
                        }
                    }
                }
                package.Save();
            }
            tuple = Tuple.Create(sFileName, Excel);
            return sFileName;
        }

        private Tuple<string, string> GetTuple()
        {
            //string sWebRootFolder = @"wwwroot\";
            string sWebRootFolder = _hostingEnvironment.WebRootPath;
            string sFileName = $"{DateTime.Now.ToString("yyyyMMddHHmmssfff")}-{Guid.NewGuid()}.xlsx";
            return Tuple.Create(sWebRootFolder, sFileName);
        }
        public List<T> ConvertToModel<T>(IFormFile excelfile, Dictionary<int, string> ExcelMap = null) where T : new()
        {
            string sWebRootFolder = GetTuple().Item1;
            string sFileName = GetTuple().Item2;
            FileInfo file = new FileInfo(Path.Combine(sWebRootFolder, sFileName));
            string filePath = Path.Combine(sWebRootFolder, sFileName);

            // 定义集合    
            List<T> ts = new List<T>();
            string tempName = "";
            DataTable dt = new DataTable();
            string fileExt = Path.GetExtension(filePath).ToLower();       
            using (FileStream fs = new FileStream(file.ToString(), FileMode.Create))
            {
                excelfile.CopyTo(fs);
                fs.Flush();
            }

            int No = 0;
            string sErrorMsg = "";
            bool tempFlag = false;
            using (ExcelPackage package = new ExcelPackage(file))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets[1];
                int rowCount = worksheet.Dimension.Rows;
                int ColCount = worksheet.Dimension.Columns;
                bool bHeaderRow = false;
                for (int j = 1; j <= ColCount; j++)
                {
                    dt.Columns.Add(worksheet.Cells[1, j].Value.ToString());
                }
                for (int row = 1; row <= rowCount; row++)
                {
                    DataRow dr = dt.NewRow();
                    for (int col = 1; col <= ColCount; col++)
                    {
                        if (bHeaderRow)
                        {
                            dr[col - 1] = worksheet.Cells[row, col].Value.ToString();
                        }
                        else if (worksheet.Cells[row, col].Value == null)
                        {
                            dr[col - 1] = null;
                        }
                        else
                        {
                            dr[col - 1] = worksheet.Cells[row, col].Value.ToString();
                        }
                    }
                    dt.Rows.Add(dr);
                }
                dt.Rows.RemoveAt(0);
                if (ExcelMap != null)
                {
                    foreach (var item in ExcelMap)
                    {
                        int key = item.Key;
                        string value = item.Value;
                        dt.Columns[key - 1].ColumnName = value;
                    }
                }
                T t = new T();
                // 获得此模型的公共属性      
                PropertyInfo[] propertys = t.GetType().GetProperties();
                string propertysName = "";

                foreach (PropertyInfo pi in propertys)
                {
                    propertysName = pi.Name;
                    Type p = pi.PropertyType;
                    if (dt.Columns.Contains(propertysName))
                    {
                        tempFlag = true;
                    }
                }
            }

            try
            {
                File.Delete(filePath);
            }
            catch (Exception)
            {
            }

            try
            {
                if (tempFlag)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        No = i;
                        T t = new T();
                        object value = null;
                        // 获得此模型的公共属性      
                        PropertyInfo[] propertys = t.GetType().GetProperties();

                        foreach (PropertyInfo pi in propertys)
                        {
                            tempName = pi.Name;
                            Type p = pi.PropertyType;
                            if (dt.Columns.Contains(tempName))
                            {
                                // 判断此属性是否有Setter      
                                if (!pi.CanWrite) continue;
                                Type[] Type = p.GetGenericArguments();
                                string row = dt.Rows[i][tempName].ToString().Trim();
                                if (row != "")
                                {
                                    if (Type.Count() > 0)
                                    {
                                        value = Convert.ChangeType(row, Type[0]);
                                    }
                                    else
                                    {
                                        value = Convert.ChangeType(row, p);
                                    }
                                    if (value != DBNull.Value)
                                    {
                                        pi.SetValue(t, value, null);
                                    }
                                }
                                else
                                {
                                    if (dt.Rows[0][tempName].ToString().EndsWith("(必填项)"))
                                    {
                                        sErrorMsg += (sErrorMsg.Equals(string.Empty) ? "导入失败<br>" : "") + "第" + (i + 2) + "行:字段:[" + dt.Rows[0][tempName].ToString().Replace("(必填项)", "") + "] 为必填项<br>";
                                    }
                                }
                            }
                        }

                        ts.Add(t);
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Log(ex.ToString());
                throw new Exception("EXCEL数据加载错误: 第" + (No + 2) + "行" + " 字段:[" + tempName + "] 数据不正确");
            }


            if (ts.Count == 0)
            {
                if (!tempFlag)
                {
                    throw new Exception("EXCEL数据加载错误, 请使用下载模板功能来生成数据模板!");
                }

                if (dt.Rows.Count == 1)
                {
                    throw new Exception("EXCEL数据加载错误, 请确认EXCEL中是否有填充数据!");
                }
            }

            if (!sErrorMsg.Equals(string.Empty))
            {
                throw new Exception(sErrorMsg);
            }

            file.Delete();
            return ts;
        }

        public static byte[] ExportByEPPlus(DataTable sourceTable/*, string strFileName*/)
        {
            byte[] reslut = null;
            using (ExcelPackage pck = new ExcelPackage())
            {
                //Create the worksheet
                string sheetName = string.IsNullOrEmpty(sourceTable.TableName) ? "Sheet1" : sourceTable.TableName;
                ExcelWorksheet ws = pck.Workbook.Worksheets.Add(sheetName);

                //Load the datatable into the sheet, starting from cell A1. Print the column names on row 1
                ws.Cells["A1"].LoadFromDataTable(sourceTable, true);

                //Format the row
                ExcelBorderStyle borderStyle = ExcelBorderStyle.Thin;
                Color borderColor = Color.FromArgb(155, 155, 155);

                using (ExcelRange rng = ws.Cells[1, 1, sourceTable.Rows.Count + 1, sourceTable.Columns.Count])
                {
                    rng.Style.Font.Name = "宋体";
                    rng.Style.Font.Size = 10;
                    rng.Style.Fill.PatternType = ExcelFillStyle.Solid;                      //Set Pattern for the background to Solid
                    rng.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(255, 255, 255));

                    rng.Style.Border.Top.Style = borderStyle;
                    rng.Style.Border.Top.Color.SetColor(borderColor);

                    rng.Style.Border.Bottom.Style = borderStyle;
                    rng.Style.Border.Bottom.Color.SetColor(borderColor);

                    rng.Style.Border.Right.Style = borderStyle;
                    rng.Style.Border.Right.Color.SetColor(borderColor);
                }

                //Format the header row
                using (ExcelRange rng = ws.Cells[1, 1, 1, sourceTable.Columns.Count])
                {
                    rng.Style.Font.Bold = true;
                    rng.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    rng.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(234, 241, 246));  //Set color to dark blue
                    rng.Style.Font.Color.SetColor(Color.FromArgb(51, 51, 51));
                }

                ////Write it back to the client
                //HttpContext.Current.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                // HttpContext.Current.Response.AddHeader("content-disposition", string.Format("attachment;  filename={0}.xlsx", HttpUtility.UrlEncode(strFileName, Encoding.UTF8)));
                // HttpContext.Current.Response.ContentEncoding = Encoding.UTF8;

                reslut = pck.GetAsByteArray();
                return reslut;
            }
        }



    }
}
