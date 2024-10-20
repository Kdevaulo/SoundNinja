using System.Linq;
using System.Text.RegularExpressions;

namespace Kdevaulo.SoundNinja.Utils
{
    public static class RegexUtils
    {
        public static string GetSoundName(string path)
        {
            return Regex.Split(path, @"\\").Last();
        }
    }
}