using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Sale_Project.Helpers;
public static class FileHelper
{
    /// <summary>
    /// Retrieves the full file path for a given JSON file located in the "StaticData" directory of the Sale_Project.
    /// </summary>
    /// <param name="fileName">The name of the JSON file to retrieve the path for.</param>
    /// <returns>
    /// The full file path to the specified JSON file.
    /// </returns>
    /// <exception cref="InvalidOperationException">
    /// Thrown if the path to the "Sale_Project" directory cannot be found.
    /// </exception>
    public static string GetJsonFilePath(string fileName)
    {
        var fullPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

        var index = fullPath.IndexOf(@"Sale_Project");

        if (index != -1)
        {
            var basePath = fullPath.Substring(0, index);

            return Path.Combine(basePath, @"Sale_Project\StaticData\", fileName);
        }
        else
        {
            throw new InvalidOperationException("Invalid path");
        }
    }
}
