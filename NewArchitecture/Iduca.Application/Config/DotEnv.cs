namespace Iduca.Application.Config;

public static class DotEnv
{
    public static void Load(string filePath = "../.env")
    {
        if (!File.Exists(filePath)) return;

        foreach (var line in File.ReadAllLines(filePath))
        {
            var trimmedLine = line.Trim();
            if (string.IsNullOrWhiteSpace(trimmedLine) || trimmedLine.StartsWith('#'))
                continue;

            var parts = trimmedLine.Split('=', 2);
            if (parts.Length != 2)
                continue;

            var key = parts[0].Trim();
            var value = parts[1].Trim().Trim('"');

            if (!string.IsNullOrEmpty(key) && !string.IsNullOrEmpty(value))
            {
                Environment.SetEnvironmentVariable(key, value);
            }
        }
    }

    public static string Get(string varName)
        => Environment.GetEnvironmentVariable(varName) ?? "";
}