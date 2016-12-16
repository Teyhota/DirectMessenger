using System;
using System.Data;
using System.Xml;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Threading;
using System.Windows;
using Rocket.API;
using Rocket.Unturned;
using Rocket.Unturned.Commands;
using Rocket.Unturned.Player;
using Rocket.Unturned.Chat;
using SDG.Unturned;
using UnityEngine;

namespace Command
{
    public class RCommand : IRocketCommand
    {

        public AllowedCaller AllowedCaller
        {
            get { return AllowedCaller.Both; }
        }

        public void Execute(IRocketPlayer caller, string[] command)
        {
            UnturnedPlayer cPlayer = (UnturnedPlayer)caller;
            UnturnedPlayer target;
            if (command.Length >= 1)
            {
                if (cPlayer.CSteamID == PM.Plugin.Instance.lastReceived[cPlayer.CSteamID])
                {
                    int cLength = command.Length;
                    string messageText = command[0].ToString();
                    target = UnturnedPlayer.FromCSteamID(PM.Plugin.Instance.lastReceived[cPlayer.CSteamID]);

                    Rocket.Unturned.Chat.UnturnedChat.Say(cPlayer, "TO: " + target.CharacterName + " >> " + messageText, Color.cyan);
                    Rocket.Unturned.Chat.UnturnedChat.Say(target, "FROM: " + cPlayer.CharacterName + " >> " + messageText, Color.clear);
                    Rocket.Unturned.Chat.UnturnedChat.Say(target, "Reply with /r <message>", Color.gray);
                }
                else if (cPlayer.CSteamID != PM.Plugin.Instance.lastReceived[cPlayer.CSteamID])
                {
                    Rocket.Unturned.Chat.UnturnedChat.Say(cPlayer, "Sorry, you haven't received a DM!", Color.red);
                }
            }
            else if (command.Length == 1)
            {
                Rocket.Unturned.Chat.UnturnedChat.Say(cPlayer, "Use /r <message>", Color.gray);
            }
            else if (command.Length > 1)
            {
                if (cPlayer.CSteamID != PM.Plugin.Instance.lastReceived[cPlayer.CSteamID])
                {
                    int cLength = command.Length;
                    string messageText = "";
                    for (int i = 0; i < cLength; i++)
                    {
                        messageText = messageText + command[i].ToString();
                    }
                    target = UnturnedPlayer.FromCSteamID(PM.Plugin.Instance.lastReceived[cPlayer.CSteamID]);

                    Rocket.Unturned.Chat.UnturnedChat.Say(cPlayer, "TO: " + target.CharacterName + " >> " + messageText, Color.cyan);
                    Rocket.Unturned.Chat.UnturnedChat.Say(target, "FROM: " + cPlayer.CharacterName + " >> " + messageText, Color.clear);
                    Rocket.Unturned.Chat.UnturnedChat.Say(target, "Reply with /r <message>", Color.gray);
                }
                else if (cPlayer.CSteamID == PM.Plugin.Instance.lastReceived[cPlayer.CSteamID])
                {
                    Rocket.Unturned.Chat.UnturnedChat.Say(cPlayer, "You haven't received a DM!", Color.red);
                }
            }
        }

        public bool AllowFromConsole
        {
            get
            {
                return false;
            }
        }

        public bool RunFromConsole
        {
            get { return false; }
        }

        public string Name
        {
            get { return "r"; }
        }

        public List<string> Aliases
        {
            get { return new List<string> { "reply" }; }
        }

        public string Syntax
        {
            get
            {
                return "<message>";
            }
        }

        public string Help
        {
            get { return "Reply to your latest DM!"; }
        }

        public List<string> Permissions
        {
            get
            {
                return new List<string>() {
                    "directmsngr.reply"
                };
            }
        }

    }
}