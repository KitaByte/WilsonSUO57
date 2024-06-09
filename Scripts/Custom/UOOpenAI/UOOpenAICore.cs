using System;
using System.IO;
using System.Linq;
using Server.Mobiles;
using System.Diagnostics;
using System.Collections.Generic;

namespace Server.Custom.UOOpenAI
{
    internal static class UOOpenAICore
    {
        private static readonly bool Debug_AI = false;

        private static readonly string PromptPath = Path.Combine(Directory.GetCurrentDirectory(), "AIPrompt");

        private static readonly string ResponsePath = Path.Combine(Directory.GetCurrentDirectory(), "AIResponse");

        private static readonly Dictionary<string, DateTime> lastReadTimes = new Dictionary<string, DateTime>();

        private static FileSystemWatcher _ResponseWatcher;

        public static void Initialize()
        {
            EventSink.Speech += EventSink_Speech;

            EventSink.Login += EventSink_Login;

            EventSink.AfterWorldSave += EventSink_AfterWorldSave;

            EventSink.Shutdown += EventSink_Shutdown;

            EventSink.Crashed += EventSink_Crashed;

            InitializeResponseWatcher();
        }

        private static void EventSink_Login(LoginEventArgs e)
        {
            if (AI_Server == null || AI_Server.HasExited)
            {
                if (e.Mobile.AccessLevel < AccessLevel.Administrator)
                {
                    StartUOOpenAIServer();
                }
            }
        }

        private static void EventSink_AfterWorldSave(AfterWorldSaveEventArgs e)
        {
            if (!World.Mobiles.Values.Any(m => m is PlayerMobile && m.Map != Map.Internal))
            {
                if (AI_Server != null && !AI_Server.HasExited)
                {
                    AI_Server.Kill();
                    AI_Server.WaitForExit();
                }
            }
        }

        private static void EventSink_Shutdown(ShutdownEventArgs e)
        {
            if (AI_Server != null && !AI_Server.HasExited)
            {
                AI_Server.Kill();
                AI_Server.WaitForExit();
            }
        }

        private static void EventSink_Crashed(CrashedEventArgs e)
        {
            if (AI_Server != null && !AI_Server.HasExited)
            {
                AI_Server.Kill();
                AI_Server.WaitForExit();
            }
        }

        private static Process AI_Server;

        private static void StartUOOpenAIServer()
        {
            try
            {
                AI_Server = Process.Start(new ProcessStartInfo
                {
                    FileName = Path.Combine(Directory.GetCurrentDirectory(), "UOOpenAIServer.exe"),
                    UseShellExecute = true,
                    WindowStyle = ProcessWindowStyle.Minimized
                });

                Console.ForegroundColor = ConsoleColor.DarkCyan;

                Console.WriteLine("UOOpenAI Server: Started!");

                Console.ForegroundColor = Debug_AI ? ConsoleColor.Green : ConsoleColor.Gray;

                Console.WriteLine($"UOOpenAI Server: Debug Enabled = {Debug_AI}");
            }
            catch (Exception ex)
            {
                if (Debug_AI)
                {
                    Console.ForegroundColor = ConsoleColor.DarkRed;

                    Console.WriteLine($"UOAI: Failed to start UOOpenAI Server: {ex.Message}");
                }
            }

            Console.ResetColor();
        }

        private static void EventSink_Speech(SpeechEventArgs e)
        {
            if (e.Mobile is PlayerMobile pm)
            {
                List<Mobile> vendors = pm.GetMobilesInRange(3)?.Where(m => m is BaseVendor)?.ToList();

                if (vendors != null && vendors.Count > 0)
                {
                    var vendor = vendors[0];

                    if (vendor is BaseVendor bv)
                    {
                        bv.Direction = bv.GetDirectionTo(e.Mobile);

                        bv.Emote(RandomResponse());

                        ProcessAIPrompt(pm, bv, e.Speech);

                        if (Debug_AI)
                        {
                            Console.ForegroundColor = ConsoleColor.DarkGreen;

                            Console.WriteLine($"UOAI: Sent Prompt for {pm.Name} and {bv.Name}: {e.Speech}");

                            Console.ResetColor();
                        }
                    }
                }
            }
        }

        private static void ProcessAIPrompt(PlayerMobile player, BaseVendor vendor, string speech)
        {
            if (!Directory.Exists(PromptPath))
            {
                Directory.CreateDirectory(PromptPath);
            }

            var prompt = player.IsStaff() ? $"STAFF:{speech}" : $"{vendor.GetType().Name}:{speech}";

            try
            {
                File.WriteAllText(Path.Combine(PromptPath, $"{player.Name}_{vendor.Name}.txt"), prompt);
            }
            catch
            {
                vendor.Emote(RandomResponse());
            }
        }

        private static void InitializeResponseWatcher()
        {
            if (!Directory.Exists(ResponsePath))
            {
                Directory.CreateDirectory(ResponsePath);
            }

            _ResponseWatcher = new FileSystemWatcher(ResponsePath)
            {
                NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.FileName,
                Filter = "*.txt"
            };

            _ResponseWatcher.Changed += ResponseWatcher_Changed;

            _ResponseWatcher.Created += ResponseWatcher_Changed;

            _ResponseWatcher.EnableRaisingEvents = true;
        }

        private static void ResponseWatcher_Changed(object sender, FileSystemEventArgs e)
        {
            var now = DateTime.UtcNow;

            if (lastReadTimes.TryGetValue(e.FullPath, out var lastReadTime))
            {
                if ((now - lastReadTime).TotalMilliseconds < 500)
                {
                    if (Debug_AI)
                    {
                        Console.ForegroundColor = ConsoleColor.DarkYellow;

                        Console.WriteLine($"UOAI: Skipped duplicate file change event for {e.Name}");
                    }

                    return;
                }
            }

            lastReadTimes[e.FullPath] = now;

            int retries = 5;

            while (retries > 0)
            {
                try
                {
                    string contents = File.ReadAllText(e.FullPath);

                    ProcessFileContents(e, contents);

                    break;
                }
                catch (IOException)
                {
                    retries--;

                    if (retries == 0)
                    {
                        if (Debug_AI)
                        {
                            Console.ForegroundColor = ConsoleColor.DarkRed;

                            Console.WriteLine($"UOAI: Failed to read {e.FullPath} after multiple attempts.");
                        }
                    }
                    else
                    {
                        if (Debug_AI)
                        {
                            Console.ForegroundColor = ConsoleColor.DarkYellow;

                            Console.WriteLine($"UOAI: Retrying read of {e.FullPath}. Attempts left: {retries}");
                        }

                        System.Threading.Thread.Sleep(200);
                    }
                }
                catch (Exception ex)
                {
                    if (Debug_AI)
                    {
                        Console.ForegroundColor = ConsoleColor.DarkRed;

                        Console.WriteLine($"UOAI: An unexpected error occurred while processing {e.FullPath}: {ex.Message}");
                    }

                    break; 
                }
            }

            Console.ResetColor();
        }

        private static void ProcessFileContents(FileSystemEventArgs e, string contents)
        {
            string[] nameParts = Path.GetFileNameWithoutExtension(e.FullPath)?.Split('_');

            if (nameParts?.Length == 2)
            {
                string playerName = nameParts[0];

                string vendorName = nameParts[1];

                Mobile player = World.Mobiles.Values.FirstOrDefault(m => m is PlayerMobile && m.Name == playerName);

                if (player != null && player is PlayerMobile pm)
                {
                    Mobile vendor = pm.GetMobilesInRange(6)?.FirstOrDefault(m => m is BaseVendor && m.Name == vendorName);

                    if (vendor != null && vendor is BaseVendor bv)
                    {
                        ProcessAIResponse(pm, bv, contents);
                    }
                }
            }
            else
            {
                if (Debug_AI)
                {
                    Console.ForegroundColor = ConsoleColor.DarkRed;

                    Console.WriteLine($"UOAI: Unexpected file name format for {e.FullPath}");

                    Console.ResetColor();
                }
            }
        }

        private static void ProcessAIResponse(PlayerMobile player, BaseVendor vendor, string response)
        {
            vendor.SetDirection(vendor.GetDirectionTo(player));

            vendor.SayTo(player, false, response);

            if (Debug_AI)
            {
                Console.ForegroundColor = ConsoleColor.DarkGreen;

                Console.WriteLine($"UOAI: Response for {player.Name} and {vendor.Name}: {response}");

                Console.ResetColor();
            }
        }

        private static string RandomResponse()
        {
            return Utility.RandomList
            (
                "thinks",
                "thinking",
                "ponders",
                "pondering",
                "contemplates",
                "wonders",
                "reflects",
                "muses",
                "meditates",
                "speculates",
                "scrutinizes",
                "deliberates",
                "ruminates",
                "considers",
                "examines",
                "broods",
                "evaluates",
                "weighs",
                "reflects",
                "questions",
                "mulls over",
                "chews over",
                "cogitates",
                "daydreams",
                "fantasizes",
                "woolgathers",
                "introspects",
                "rummages",
                "riffs on",
                "puzzles over",
                "reviews",
                "turns over",
                "stews over",
                "wrestles with",
                "mulls",
                "revisits",
                "rethinks",
                "reconsiders",
                "reexamines",
                "second-guesses",
                "chews on",
                "grapples with",
                "digests",
                "masticates",
                "ratiocinates",
                "ponders",
                "troubles over"
            );
        }
    }
}

