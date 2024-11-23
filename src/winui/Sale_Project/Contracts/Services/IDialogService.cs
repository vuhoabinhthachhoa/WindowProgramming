using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sale_Project.Contracts.Services;
public interface IDialogService
{
    Task ShowErrorAsync(string title, string message);
    Task ShowWarningAsync(string title, string message);
    Task ShowSuccessAsync(string title, string message);
    Task<bool> ShowConfirmAsync(string title, string message);
}
