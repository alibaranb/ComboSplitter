using System.IO;
using System.Text;
using System.Text.RegularExpressions;

const Int32 BufferSize = 128;
using (var fileStream = File.OpenRead("combolist.txt"))
using (var streamReader = new StreamReader(fileStream, Encoding.UTF8, true, BufferSize))
{
    string line;
    Regex r = new Regex(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$");
    List<string> emailPass = new List<string>();
    List<string> userPass = new List<string>();

    while ((line = streamReader.ReadLine()) != null)
    {
        String[] splitted = line.Split(":");
        if (line.Contains(' ') || splitted[1].Length > 64)
        {
            continue;
        }
        else
        {
            if (r.IsMatch(splitted[0]))
            {
                emailPass.Add(line);
            }
            else
            {
                userPass.Add(line);
            }
        }
        
    }
    File.WriteAllLines("user_pass.txt", userPass);
    File.WriteAllLines("email_pass.txt", emailPass);

    Console.WriteLine(emailPass.Count + " email:pass saved");
    Console.WriteLine(userPass.Count + " user:pass saved");
    Console.ReadLine();
}