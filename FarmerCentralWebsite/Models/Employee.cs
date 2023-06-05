using System;
using System.Collections.Generic;

namespace FarmerCentralWebsite.Models;

public partial class Employee
{
    public string EmployeeEmail { get; set; } = null!;

    public string Password { get; set; } = null!;
}
