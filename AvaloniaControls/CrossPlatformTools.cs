using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Platform.Storage;

namespace AvaloniaControls;

// ReSharper disable once ClassNeverInstantiated.Global
public class CrossPlatformTools
{
    /// <summary>
    /// Opens a directory in the OS file explorer
    /// </summary>
    /// <param name="path">The path to the file or directory</param>
    /// <param name="isFile">If this is a file rather than a folder</param>
    /// <returns>True if successful</returns>
    public static bool OpenDirectory(string path, bool isFile = false)
    {
        if (isFile)
        {
            path = new FileInfo(path).DirectoryName ?? "";
        }

        if (!Directory.Exists(path))
        {
            return false;
        }

        try
        {
            Process.Start(new ProcessStartInfo()
            {
                FileName = path,
                UseShellExecute = true,
                Verb = "open"
            });
        }
        catch (Exception)
        {
            return false;
        }

        return true;
    }

    /// <summary>
    /// Opens the provided URL in the OS default browser
    /// </summary>
    /// <param name="url">The url to open</param>
    /// <returns>True if successful</returns>
    public static bool OpenUrl(string url)
    {
        try
        {
            Process.Start(url);
        }
        catch
        {
            try
            {
                // hack because of this: https://github.com/dotnet/corefx/issues/10361
                if (OperatingSystem.IsWindows())
                {
                    url = url.Replace("&", "^&");
                    Process.Start(new ProcessStartInfo(url) { UseShellExecute = true });
                }
                else if (OperatingSystem.IsLinux())
                {
                    Process.Start("xdg-open", url);
                }
                else if (OperatingSystem.IsMacOS())
                {
                    Process.Start("open", url);
                }
                else
                {
                    return false;
                }

                return true;
            }
            catch
            {
                return false;
            }
            
        }

        return false;
    }

    /// <summary>
    /// Opens a file or folder dialog
    /// </summary>
    /// <param name="parentWindow">The parent window for the file dialog</param>
    /// <param name="type">The type of file or folder dialog</param>
    /// <param name="filter">The file filter</param>
    /// <param name="path">The initial path</param>
    /// <param name="title">The title of the dialog, if desired</param>
    /// <param name="warnOnOverwrite">If the save dialog should warn about overwrites</param>
    /// <param name="caseSensitiveFilter">If the filter pattern should be made to be case-sensitive</param>
    /// <returns></returns>
    public static async Task<IStorageItem?> OpenFileDialogAsync(Window parentWindow, FileInputControlType type, string? filter, string? path, string? title = null, bool warnOnOverwrite = true, bool caseSensitiveFilter = false)
    {
        if (!string.IsNullOrEmpty(path) && (File.Exists(path) || Directory.Exists(path)))
        {
            var attr = File.GetAttributes(path);
            if ((attr & FileAttributes.Directory) != FileAttributes.Directory)
            {
                var file = new FileInfo(path);
                path = file.DirectoryName;
            }
        }
        
        path ??= Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        
        var folder = await parentWindow.StorageProvider.TryGetFolderFromPathAsync(path);  
        
        if (type == FileInputControlType.OpenFile)
        {
            var files = await parentWindow.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
            {
                Title = title ?? "Select File",
                FileTypeFilter = ParseFilter(filter, caseSensitiveFilter),
                SuggestedStartLocation = folder,
            });
            
            if (!string.IsNullOrEmpty(files.FirstOrDefault()?.Path.LocalPath))
            {
                return files.First();
            }
        }
        else if (type == FileInputControlType.SaveFile)
        {
            var file = await parentWindow.StorageProvider.SaveFilePickerAsync(new FilePickerSaveOptions
            {
                Title = title ?? "Save File",
                FileTypeChoices = ParseFilter(filter, caseSensitiveFilter),
                ShowOverwritePrompt = warnOnOverwrite,
                SuggestedStartLocation = folder,
            });

            if (!string.IsNullOrEmpty(file?.Path.LocalPath))
            {
                return file;
            }
        }
        else if (type == FileInputControlType.Folder)
        {
            var folders = await parentWindow.StorageProvider.OpenFolderPickerAsync(new FolderPickerOpenOptions()
            {
                Title = title ?? "Select Folder",
                SuggestedStartLocation = folder,
            });
            
            if (!string.IsNullOrEmpty(folders.FirstOrDefault()?.Path.LocalPath))
            {
                return folders.First();
            }
        }

        return null;
    }
    
    private static List<FilePickerFileType>? ParseFilter(string? filter, bool caseSensitiveFilter = false)
    {
        if (string.IsNullOrEmpty(filter))
        {
            return null;
        }
        
        var toReturn = new List<FilePickerFileType>();

        if (filter.Contains(':'))
        {
            foreach (var filterPart in filter.Split(";"))
            {
                var filterParts = filterPart.Split(":");
                if (filterParts.Length != 2)
                {
                    throw new InvalidOperationException($"{filter} has an invalid filter: {filterPart}");
                }

                var patterns = filterParts[1].Split(",").Select(x => x.Trim()).ToArray();

                var text = filterParts[0].Trim();
                if (!text.Contains('('))
                {
                    text += " (" + string.Join(", ", patterns) + ")";
                }
                
                if (!OperatingSystem.IsWindows() && !caseSensitiveFilter)
                {
                    patterns = patterns.SelectMany(CasePermutation).ToArray();
                }
                
                toReturn.Add(new FilePickerFileType(text) { Patterns = patterns });
            }
        }
        else
        {
            var parts = filter.Split('|');

            if (parts.Length % 2 != 0)
            {
                throw new InvalidOperationException($"File filter {filter} is invalid");
            }
                
            for (var i = 0; i < parts.Length; i += 2)
            {
                var description = parts[i].Trim();
                var patterns = parts[i + 1].Split(";").Select(x => x.Trim()).ToList();

                if (!OperatingSystem.IsWindows() && !caseSensitiveFilter)
                {
                    patterns = patterns.SelectMany(CasePermutation).ToList();
                }
                
                toReturn.Add(new FilePickerFileType(description) { Patterns = patterns });
            }
        }
        

        return toReturn;
    }

    private static List<string> CasePermutation(string input)
    {
        var lower = input.ToLower().ToCharArray();
        var upper = input.ToUpper().ToCharArray();
        return GetIndexCombinations(lower, upper).Select(x => new string(x)).Distinct().ToList();
    }
    
    private static List<char[]> GetIndexCombinations(char[] a, char[] b)
    {
        if (a.Length != b.Length)
        {
            throw new ArgumentException("Arrays A and B must have the same length.");
        }

        var result = new List<char[]>();
        int n = a.Length;

        // Generate all combinations using a bitmask approach
        int totalCombinations = 1 << n; // 2^n combinations

        for (var mask = 0; mask < totalCombinations; mask++)
        {
            var combination = new char[n];
            for (var i = 0; i < n; i++)
            {
                // Use the bitmask to decide whether to pick from A or B
                combination[i] = (mask & (1 << i)) != 0 ? b[i] : a[i];
            }
            result.Add(combination);
        }

        return result;
    }
}