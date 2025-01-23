namespace YGate.String.Operations
{
    public static class Modify
    {
        public static string TurkishCharPrufy(this string str)
        {
            str = str.Replace('ş', 's');
            str = str.Replace('Ş', 'S');
            str = str.Replace('İ', 'I');
            str = str.Replace('ı', 'i');
            str = str.Replace('ğ', 'g');
            str = str.Replace('Ğ', 'G');
            str = str.Replace('ç', 'c');
            str = str.Replace('Ç', 'C');
            str = str.Replace('ö', 'o');
            str = str.Replace('Ö', 'O');
            str = str.Replace(' ', '_');
            str = str.Replace('Ü', 'U');
            str = str.Replace('ü', 'u');
            str = str.Replace('/', ' ');
            return str;
        }

        public static string Salt(string data)
        {
            return $"_*_SaltB3yB!{data}0_1s_Z0r";
        }
    }
}
