using ExcelDataReader;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication5.Models;

namespace WebApplication5.Controllers
{
    public class MyController : Controller
    {
        // GET: My
        public ActionResult Index()
        {
            DataTable dt = new DataTable();

            try
            {
                dt = (DataTable)Session["tmpdata"];
                Student_test st = new Student_test();
            }
            catch (Exception ex)
            {

            }

            return View(dt);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(HttpPostedFileBase upload)
        {

            if (ModelState.IsValid)
            {

                if (upload != null && upload.ContentLength > 0)
                {
                    Stream stream = upload.InputStream;

                    IExcelDataReader reader = null;

                    if (upload.FileName.EndsWith(".xls"))
                    {
                        reader = ExcelReaderFactory.CreateBinaryReader(stream);
                    }
                    else if (upload.FileName.EndsWith(".xlsx"))
                    {
                        reader = ExcelReaderFactory.CreateOpenXmlReader(stream);
                    }
                    else
                    {
                        ModelState.AddModelError("File", "This file format is not supported");
                        return View();
                    }
                    int fieldcount = reader.FieldCount;
                    int rowcount = reader.RowCount;


                    DataTable dt = new DataTable();
                    //dt.Columns.Add("UserName");
                    //dt.Columns.Add("Adddress");
                    DataRow row;


                    DataTable dt_ = new DataTable();
                    try
                    {

                        dt_ = reader.AsDataSet().Tables[0];

                        string ret = "";



                        for (int i = 0; i < dt_.Columns.Count; i++)
                        {
                            dt.Columns.Add(dt_.Rows[0][i].ToString());
                        }

                        int rowcounter = 0;
                        for (int row_ = 1; row_ < dt_.Rows.Count; row_++)
                        {
                            row = dt.NewRow();

                            for (int col = 0; col < dt_.Columns.Count; col++)
                            {
                                row[col] = dt_.Rows[row_][col].ToString();
                                rowcounter++;
                            }
                            dt.Rows.Add(row);
                        }

                        Student_test st = new Student_test();
                        using (var _dbContext = new ApplicationDbContext())
                        {
                            var dbContextTransaction = _dbContext.Database.BeginTransaction();
                            for (int i = 0; i < rowcounter; i++)
                            {
                                var insertStudent = new Student_test()
                                {
                                    ID = st.ID = int.Parse(dt.Rows[i][0].ToString()),
                                    LastName = st.LastName = dt.Rows[i][1].ToString()
                                };
                                _dbContext.Student_test.Add(insertStudent);
                                _dbContext.SaveChanges();
                            }
                            dbContextTransaction.Commit();
                        }
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError("File", "Unable to Upload file!");
                        return View();
                    }

                    DataSet result = new DataSet();//reader.AsDataSet();
                    result.Tables.Add(dt);

                    reader.Close();
                    reader.Dispose();
                    // return View();
                    //  return View(result.Tables[0]);

                    DataTable ddd = result.Tables[0];

                    Session["tmpdata"] = ddd;

                    return RedirectToAction("Index");

                }
                else
                {
                    ModelState.AddModelError("File", "Please Upload Your file");
                }
            }
            return View();
        }
    }
}