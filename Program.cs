using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace PortableWIfiCalc
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("随身WiFi切卡密码计算器 v0,1");
            Console.WriteLine("Made by th7");
            Console.WriteLine();

            while (true)
            {
                Console.WriteLine("请选择要计算的设备：");
                Console.WriteLine("1. 新迅 MFX32");
                Console.WriteLine("2. 铁盒 CPE");
                Console.WriteLine("3. 果迷新版");
                Console.WriteLine("4. 退出");
                Console.WriteLine();

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        CalculateXinxun();
                        break;
                    case "2":
                        CalculateCPE();
                        break;
                    case "3":
                        CalculateGuomiNewVersion();
                        break;
                    case "4":
                        return;
                    default:
                        Console.WriteLine("无效的选择，请重试。");
                        break;
                }

                Console.WriteLine();
            }
        }

        static void CalculateXinxun()
        {
            Console.Write("请输入IMEI: ");
            string input = Console.ReadLine();

            if (input.Length < 6)
            {
                Console.WriteLine("请输入完整的IMEI!");
                return;
            }

            string lastSixChars = input.Substring(input.Length - 6);
            char[] swapped = lastSixChars.ToCharArray();
            (swapped[2], swapped[3]) = (swapped[3], swapped[2]);
            string modifiedString = new string(swapped);

            if (!int.TryParse(modifiedString, out int number))
            {
                Console.WriteLine("无法转换为数字");
                return;
            }

            int result = (number + 329) * 5;
            Console.WriteLine($"计算结果: {result}");
        }

        static void CalculateCPE()
        {
            Console.Write("请输入设备的MAC地址: ");
            string macInput = Console.ReadLine();

            string simctr_pass = macInput.ToLower();
            simctr_pass = Regex.Replace(simctr_pass, @"[:\-]+", "");
            simctr_pass = "cpe" + simctr_pass[8..];

            Console.WriteLine($"计算结果: {simctr_pass}");
        }

        static string CalculateMD5(string input)
        {
            using (MD5 md5 = MD5.Create())
            {
                byte[] inputBytes = Encoding.ASCII.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("x2"));
                }
                return sb.ToString();
            }
        }

        static void CalculateGuomiNewVersion()
        {
            Console.Write("输入IMEI: ");
            string imei = Console.ReadLine();

            Console.WriteLine("adb命令: adb shell getprop ro.sim.ccsn");
            Console.Write("输入CCSN: ");
            string ccsn = Console.ReadLine();

            string combinedString = imei.Substring(Math.Max(0, imei.Length - 8)) + ccsn;
            string md5Hash = CalculateMD5(combinedString);
            string result = md5Hash.Substring(8, 16);
            string password = result.Substring(result.Length - 8);

            Console.WriteLine($"计算结果: {password}");
        }
    }
}