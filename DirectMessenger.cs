using Rocket.API;
using Rocket.Core.Commands;
using Rocket.Core.Logging;
using Rocket.Core.Plugins;
using Rocket.Unturned.Chat;
using Rocket.Unturned.Player;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Teyhota.DirectMessenger
{
    public class MessageTargets
    {
        public ulong MessageTo;
        public ulong ReplyTo;
        public MessageTargets()
        {
            MessageTo = 0;
            ReplyTo = 0;
        }
    }

    public class DirectMessenger : RocketPlugin<DirectMessengerConfig>
    {
        Dictionary<ulong, MessageTargets> PlayerList;

            protected override void Load()
        {
            Rocket.Core.Logging.Logger.Log("Loading plugin...", ConsoleColor.DarkGreen);
            Rocket.Core.Logging.Logger.LogWarning("Plugin by: Teyhota");
            Rocket.Core.Logging.Logger.LogWarning("Plugin Version: 1.1.0.0 beta");
            Rocket.Core.Logging.Logger.LogWarning("For Unturned Version: 3.17.10.1");
            Rocket.Core.Logging.Logger.Log("...DirectMessenger has been loaded!", ConsoleColor.DarkGreen);
            Rocket.Core.Logging.Logger.Log("Go to TRP4Unturned.tk for support!", ConsoleColor.DarkRed);
            PlayerList = new Dictionary<ulong, MessageTargets>();
        }
        protected override void Unload()
        {
            Rocket.Core.Logging.Logger.Log("Unloading plugin...", ConsoleColor.DarkRed);
            Rocket.Core.Logging.Logger.Log("...DirectMessenger has been unloaded!", ConsoleColor.DarkRed);
            base.Unload();
        }

        // (Command, Help string, Syntax, Allowed Caller Console/Both/Player

        [RocketCommand("dm", "DM a player", "/dm <name> <message>", AllowedCaller.Player)]
        public void ExecuteWhisper(IRocketPlayer caller, string[] para)
        {
            String Message = "";
            UnturnedPlayer plyr = (UnturnedPlayer)caller;
            if (para.Length == 0) { Logger.Log("DM Para too small!"); return; } // Display Help
            UnturnedPlayer Target = UnturnedPlayer.FromName(para[0]);
            if (Target == null)
            {
                if (PlayerList.ContainsKey(plyr.CSteamID.m_SteamID))
                {
                    Target = UnturnedPlayer.FromCSteamID(new Steamworks.CSteamID(PlayerList[plyr.CSteamID.m_SteamID].MessageTo));
                    if (Target == null)
                        Target = UnturnedPlayer.FromCSteamID(new Steamworks.CSteamID(PlayerList[plyr.CSteamID.m_SteamID].ReplyTo));
                }
                if (Target == null) { Logger.Log("No Target!"); return; }// Display Help
                foreach (var s in para)
                {
                    Message = Message + s;
                }
            }
            else
            {
                int i = 0;
                foreach (var s in para)
                {
                    i++;
                    if (i == 1) continue;
                    Message = Message + " " + s;
                }
            }
            UnturnedChat.Say(Target, String.Format("From:{0} {1}", plyr.CharacterName, Message), UnityEngine.Color.magenta);
            UnturnedChat.Say(plyr, String.Format("To:{0} {1}", Target.DisplayName, Message), UnityEngine.Color.magenta);

            SetPlayerData(plyr, Target);
        }

        private void SetPlayerData(UnturnedPlayer plyr, UnturnedPlayer Target)
        {
            if (!PlayerList.ContainsKey(Target.CSteamID.m_SteamID))
                PlayerList[Target.CSteamID.m_SteamID] = new MessageTargets();
            if (!PlayerList.ContainsKey(plyr.CSteamID.m_SteamID))
                PlayerList[plyr.CSteamID.m_SteamID] = new MessageTargets();
            PlayerList[Target.CSteamID.m_SteamID].ReplyTo = plyr.CSteamID.m_SteamID;
            PlayerList[plyr.CSteamID.m_SteamID].MessageTo = Target.CSteamID.m_SteamID;
        }
        [RocketCommand("r", "Replies to a player", "/r <message>", AllowedCaller.Player)]
        public void ExecuteReply(IRocketPlayer caller, string[] para)
        {
            UnturnedPlayer plyr = (UnturnedPlayer)caller;
            UnturnedPlayer Target = null;
            String Message = "";
            if (!PlayerList.ContainsKey(plyr.CSteamID.m_SteamID)) { Logger.Log("Sorry, you haven't received a DM!"); return; } // No record available.. (no Reply or Message Target) Display Help
            if (para.Length < 1) { Logger.Log("Reply Para too small!"); return; } // Display Help
            if (PlayerList[plyr.CSteamID.m_SteamID].MessageTo != 0)
                Target = UnturnedPlayer.FromCSteamID(new Steamworks.CSteamID(PlayerList[plyr.CSteamID.m_SteamID].MessageTo));
            if (PlayerList[plyr.CSteamID.m_SteamID].ReplyTo != 0)
                Target = UnturnedPlayer.FromCSteamID(new Steamworks.CSteamID(PlayerList[plyr.CSteamID.m_SteamID].ReplyTo));
            if (Target == null) { Logger.Log("An Unexpected Error Has Occured!"); return; }// Some unexpected Error
            foreach (var s in para)
            {
                Message = Message + " " + s;
            }
            UnturnedChat.Say(Target, String.Format("From:{0} {1}", plyr.DisplayName, Message), UnityEngine.Color.magenta);
            UnturnedChat.Say(plyr, String.Format("To:{0} {1}", Target.DisplayName, Message), UnityEngine.Color.magenta);
            SetPlayerData(plyr, Target);
        }
    }
}