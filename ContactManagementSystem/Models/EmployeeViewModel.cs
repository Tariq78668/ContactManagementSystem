using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ContactManagementSystem.Models
{
    public class EmployeeViewModel
    {
        public int EmpID { get; set; }
        public string EmpName { get; set; }
        public string EmpAddress { get; set; }
        public string EmpContact { get; set; }
        public int DepartmentID { get; set; }
        public string EmpImage { get; set; }
        public Nullable<bool> IsDeleted { get; set; }

        public string DepartmentName { get; set; }
    }
}