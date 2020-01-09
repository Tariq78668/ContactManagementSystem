using ContactManagementSystem.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace ContactManagementSystem.Controllers
{
    public class HomeController : Controller
    {
        CMSEntities cmsEntities = new CMSEntities();

        [Authorize]
        public ActionResult Index()
        {
            List<tblDepartment> DeptList = cmsEntities.tblDepartments.ToList();
            ViewBag.ListOfDepartment = new SelectList(DeptList, "DepartmentID", "DepartmentName");
            return View();
        }

        public JsonResult GetEmployeeList()
        {

            List<EmployeeViewModel> StuList = cmsEntities.tblEmployees.Where(x => x.IsDeleted == false).Select(x => new EmployeeViewModel
            {
                EmpID = x.EmpID,
                EmpName = x.EmpName,
                EmpAddress = x.EmpAddress,
                EmpContact = x.EmpContact,
                EmpImage = x.EmpImage,
                DepartmentName = x.tblDepartment.DepartmentName
            }).ToList();

            return Json(StuList, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetEmployeeById(int EmployeeId)
        {
            tblEmployee model = cmsEntities.tblEmployees.Where(x => x.EmpID == EmployeeId).SingleOrDefault();
            string value = string.Empty;
            value = JsonConvert.SerializeObject(model, Formatting.Indented, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
            return Json(value, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SaveDataInDatabase(EmployeeViewModel model)
        {
            var result = false;
            try
            {
                if (model.EmpID > 0)
                {
                    tblEmployee Stu = cmsEntities.tblEmployees.SingleOrDefault(x => x.IsDeleted == false && x.EmpID == model.EmpID);
                    Stu.EmpName = model.EmpName;
                    Stu.EmpAddress = model.EmpAddress;
                    Stu.EmpContact = model.EmpContact;
                    Stu.EmpImage = model.EmpImage.Replace(" ","+");
                    Stu.DepartmentID = model.DepartmentID;
                    cmsEntities.SaveChanges();
                    result = true;
                }
                else
                {
                    tblEmployee Stu = new tblEmployee();
                    Stu.EmpName = model.EmpName;
                    Stu.EmpAddress = model.EmpAddress;
                    Stu.EmpContact = model.EmpContact;
                    Stu.EmpImage = model.EmpImage.Replace(" ", "+"); ;
                    Stu.DepartmentID = model.DepartmentID;
                    Stu.IsDeleted = false;
                    cmsEntities.tblEmployees.Add(Stu);
                    cmsEntities.SaveChanges();
                    result = true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public JsonResult DeleteStudentRecord(int EmployeeId)
        {
            bool result = false;
            tblEmployee Stu = cmsEntities.tblEmployees.SingleOrDefault(x => x.IsDeleted == false && x.EmpID == EmployeeId);
            if (Stu != null)
            {
                Stu.IsDeleted = true;
                cmsEntities.SaveChanges();
                result = true;
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

    }
}