﻿namespace EmployeeApi.Models;

public class Employee
{
    public int Id { get; set; }
    public string Name { get; set; }
    public DateOnly DateOfBirth { get; set; }
}
