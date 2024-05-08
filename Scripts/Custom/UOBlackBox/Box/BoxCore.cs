using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Net;
using System.Security.Principal;
using System.Text.RegularExpressions;

using Server.Commands;
using Server.Mobiles;

namespace Server.Services.UOBlackBox
{
    public static class BoxCore
    {
        public static AccessLevel StaffAccess = AccessLevel.VIP; // Access > than Level

        public static bool ShowInfo = false;

        private const int VersionNum = 0;

        public const int DevVersion = 16;

        public static string CurrentVersion = $"1.0.{VersionNum}.{DevVersion}";

        public static bool NeedsUpdate = false;

        public const string Url = @"https://www.uoopenai.com/resources";

        public static BoxTimer BoxTime = new BoxTimer();

        public static void Initialize()
        {
            EventSink.ItemCreated += EventSink_ItemCreated;

            EventSink.ItemDeleted += EventSink_ItemDeleted;

            EventSink.OnPropertyChanged += EventSink_OnPropertyChanged;

            EventSink.Login += EventSink_Login;

            EventSink.Logout += EventSink_Logout;

            EventSink.Shutdown += EventSink_Shutdown;
        }

        private static void EventSink_ItemCreated(ItemCreatedEventArgs e)
        {
            if (e.Item is BoxStatic b)
            {
                LogConsole(ConsoleColor.DarkYellow, $"{b.Staff} Added => {b.ItemID}{b.Location}");

                if (!b.IsUndo)
                    UndoManager.AddCommand(b.Staff, $"Delete:{b.X}:{b.Y}:{b.Z}:{b.Map}");
            }
        }

        private static void EventSink_ItemDeleted(ItemDeletedEventArgs e)
        {
            if (e.Item is BoxStatic b)
            {
                LogConsole(ConsoleColor.DarkYellow, $"{b.Staff} Deleted => {b.ItemID}{b.Location}{b.Hue}");

                if (!b.IsUndo)
                    UndoManager.AddCommand(b.Staff, $"Add:{b.ItemID}:{b.X}:{b.Y}:{b.Z}:{b.Map}:{b.Hue}");
            }
        }

        private static void EventSink_OnPropertyChanged(OnPropertyChangedEventArgs e)
        {
            if (e.Property.Name != "Name" && e.OldValue != e.NewValue)
            {
                if (e.Instance is BoxStatic b && b.Location != new Point3D(0, 0, 0))
                {
                    LogConsole(ConsoleColor.DarkYellow, $"{b.Staff} Altered => {b.ItemID}{b.Location}-[{e.Property.Name}] = {e.OldValue} > {e.NewValue}");

                    if (!b.IsUndo)
                        UndoManager.AddCommand(b.Staff, $"Alter:{b.ItemID}:{b.X}:{b.Y}:{b.Z}:{b.Map}:{e.Property.Name}:{e.OldValue}");
                }
            }
        }

        private static void EventSink_Login(LoginEventArgs e)
        {
            if (e.Mobile.AccessLevel > StaffAccess)
            {
                if (!HasBox(e.Mobile))
                {
                    e.Mobile.Backpack.AddItem(new BlackBox(e.Mobile));
                }
            }
        }

        private static bool HasBox(Mobile staff)
        {
            if (staff.Backpack.FindItemByType(typeof(BlackBox)) != null)
            {
                return true;
            }

            var boxes = World.Items.Values.Where(b => b is BlackBox).ToList();

            if (boxes?.Count > 0)
            {
                foreach (var box in boxes)
                {
                    if (box is BlackBox bb && bb.Staff == staff)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private static void EventSink_Logout(LogoutEventArgs e)
        {
            if (e.Mobile is PlayerMobile pm && pm.AccessLevel > StaffAccess)
            {
                if (pm.Backpack.FindItemByType(typeof(BlackBox)) is BlackBox box)
                {
                    box.Session?.EndBox();
                }
            }
        }

        private static void EventSink_Shutdown(ShutdownEventArgs e)
        {
            var players = World.Mobiles.Values.ToList().FindAll(m => m is PlayerMobile);

            for (int i = 0; i < players?.Count; i++)
            {
                if (players[i].AccessLevel > StaffAccess && players[i].Backpack.FindItemByType(typeof(BlackBox)) is BlackBox box)
                {
                    box.Session?.EndBox();
                }
            }

            ArtCore.SaveStaticArtInfo();
        }

        public static void LogConsole(ConsoleColor color, string message, bool hasName = true)
        {
            var systemColor = color == ConsoleColor.DarkCyan || color == ConsoleColor.Red || color == ConsoleColor.DarkRed;

            if (ShowInfo || systemColor || message.Length == LogoDesign.Banner.Length)
            {
                Console.ForegroundColor = color;

                if (hasName)
                {
                    Console.WriteLine($"UO Black Box [{message}]");
                }
                else
                {
                    Console.WriteLine(message);
                }

                Console.ResetColor();
            }
        }

        public static bool IsUserAdministrator()
        {
            try
            {
                WindowsIdentity user = WindowsIdentity.GetCurrent();

                WindowsPrincipal principal = new WindowsPrincipal(user);

                return principal.IsInRole(WindowsBuiltInRole.Administrator);
            }
            catch
            {
                return false;
            }
        }

        private static readonly StringBuilder NameBuilder = new StringBuilder();

        private static string[] words;

        private static char firstChar;

        private static string restOfWord;

        public static string CapitalizeWords(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return input;
            }

            NameBuilder.Clear();

            if (input.Contains("_"))
            {
                input.Replace('_', ' ');
            }

            words = input.Split(' ');

            for (var i = 0; i < words.Length; i++)
            {
                if (words[i].Length > 0)
                {
                    firstChar = char.ToUpper(words[i][0]);

                    restOfWord = words[i].Substring(1);

                    NameBuilder.Append($"{firstChar}{restOfWord} ");
                }
            }

            return NameBuilder.ToString().TrimEnd();
        }

        public static void RunBBCommand(PlayerMobile pm, string cmd)
        {
            LogConsole(ConsoleColor.DarkGray, $"{pm.Name} => {cmd}");

            CommandSystem.Handle(pm, $"{CommandSystem.Prefix}{cmd}");
        }

        public static void CheckVersion()
        {
            try
            {
                WebRequest request = WebRequest.Create(Url);

                WebResponse response = request.GetResponse();

                if (response is HttpWebResponse httpWebResponse && httpWebResponse.StatusCode == HttpStatusCode.OK)
                {
                    string html;

                    using (Stream responseStream = response.GetResponseStream())

                    using (StreamReader reader = new StreamReader(responseStream))
                    {
                        html = reader.ReadToEnd();
                    }

                    Match match = Regex.Match(html, @"""version"":\s*""(?<value>.*?)""");

                    if (match.Success)
                    {
                        var version = match.Groups["value"].Value;

                        if (CurrentVersion != version)
                        {
                            NeedsUpdate = true;

                            LogConsole(ConsoleColor.Red, $"Version : {CurrentVersion} Outdated => Requires Update!");
                        }
                        else
                        {
                            NeedsUpdate = false;

                            LogConsole(ConsoleColor.DarkCyan, $"Version : {version}");
                        }
                    }
                }
            }
            catch
            {
                NeedsUpdate = false;

                LogConsole(ConsoleColor.DarkCyan, $"Version : {CurrentVersion} (Not Verified)");
            }
        }

        public static void CheckVersionAlt()
        {
            try
            {
                WebRequest request = WebRequest.Create(Url);

                WebResponse response = request.GetResponse();

                if (response is HttpWebResponse httpWebResponse && httpWebResponse.StatusCode == HttpStatusCode.OK)
                {
                    string html;

                    using (Stream responseStream = response.GetResponseStream())

                    using (StreamReader reader = new StreamReader(responseStream))
                    {
                        html = reader.ReadToEnd();
                    }

                    Match match = Regex.Match(html, @"UOBlackBox:Version:(?<value>\d+\.\d+\.\d+\.\d+)");

                    if (match.Success)
                    {
                        var version = match.Groups["value"].Value;

                        if (CurrentVersion != version)
                        {
                            NeedsUpdate = true;

                            LogConsole(ConsoleColor.Red, $"Current Version: {CurrentVersion}, Latest Version: {version} - Outdated => Requires Update!");
                        }
                        else
                        {
                            NeedsUpdate = false;

                            LogConsole(ConsoleColor.DarkCyan, $"Version : {version}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                NeedsUpdate = false;

                LogConsole(ConsoleColor.DarkCyan, $"Error occurred while checking version: {ex.Message}");
            }
        }
    }
}
