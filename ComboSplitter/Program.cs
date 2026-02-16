using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections.Generic;

const int BufferSize = 128;

using (var fileStream = File.OpenRead("combolist.txt"))
using (var streamReader = new StreamReader(fileStream, Encoding.UTF8, true, BufferSize))
{
    string? line;
    Regex emailRegex = new Regex(@"^[\w\-\.]+@([\w\-]+\.)+[\w\-]{2,4}$");

    List<string> emailPass = new List<string>();
    List<string> userPass = new List<string>();

    while ((line = streamReader.ReadLine()) != null)
    {
        if (string.IsNullOrWhiteSpace(line))
            continue;

        // Split only into 2 parts (safer than normal Split)
        string[] splitted = line.Split(':', 2);

        if (splitted.Length < 2)
            continue;

        string username = splitted[0].Trim();
        string passwordPart = splitted[1].Trim();

        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(passwordPart))
            continue;

        if (line.Contains("UNKNOWN"))
            continue;

        // Get only the part before first space
        int spaceIndex = passwordPart.IndexOf(' ');
        string password = spaceIndex > -1
            ? passwordPart.Substring(0, spaceIndex)
            : passwordPart;

        if (password.Length > 64)
            continue;

        string cleanLine = $"{username}:{password}";

        if (emailRegex.IsMatch(username))
        {
            emailPass.Add(cleanLine);
        }
        else
        {
            userPass.Add(cleanLine);
        }
    }

    File.WriteAllLines("user_pass.txt", userPass);
    File.WriteAllLines("email_pass.txt", emailPass);

    Console.WriteLine($"{emailPass.Count} email:pass saved");
    Console.WriteLine($"{userPass.Count} user:pass saved");
}

Console.WriteLine("Done.");
Console.ReadLine();
