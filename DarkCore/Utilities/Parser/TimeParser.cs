using System;
using System.Text.RegularExpressions;

namespace DarkCore.Utilities.Parser
{
    public class TimeParser
    {
        public static TimeSpan ParseTimeSpan(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                throw new ArgumentException("Input cannot be null or empty.");

            var regex = new Regex(@"(\d+)([smhd])", RegexOptions.IgnoreCase);
            var matches = regex.Matches(input);

            if (matches.Count == 0)
                throw new FormatException("Invalid time format.");

            var total = TimeSpan.Zero;
            foreach (Match match in matches)
            {
                var value = int.Parse(match.Groups[1].Value);
                var unit = match.Groups[2].Value.ToLower();

                switch (unit)
                {
                    case "s":
                        total += TimeSpan.FromSeconds(value);
                        break;
                    case "m":
                        total += TimeSpan.FromMinutes(value);
                        break;
                    case "h":
                        total += TimeSpan.FromHours(value);
                        break;
                    case "d":
                        total += TimeSpan.FromDays(value);
                        break;
                    default:
                        throw new FormatException($"Unknown time unit: {unit}");
                }
            }

            return total;
        }
    }
}