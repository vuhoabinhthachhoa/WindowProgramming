using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sale_Project.Core.Models;

namespace Sale_Project.Contracts.Services;
public interface IBranchService
{
    // Add a new employee to the system
    Task<Branch> CreateBranch(Branch branch);

    // Mark an employee as unemployed (inactive)
    Task<bool> InactiveBranch(string branchName);

    // Update an existing employee's details
    Task<Branch> UpdateBranch(Branch branch, string newBranchName);

    // Optionally, add a method to list all employees
    Task<IEnumerable<Branch>> GetAllBranches();
}
