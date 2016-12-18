using System;
using System.Data;
using System.Xml;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;
using Rocket.API;
using Rocket.Unturned;
using Rocket.Unturned.Commands;
using Rocket.Unturned.Player;
using Rocket.Unturned.Chat;
using SDG.Unturned;
using UnityEngine;

namespace Command
{
	public class DMCommand : IRocketCommand
	{

        public AllowedCaller AllowedCaller
        {
            get { return AllowedCaller.Both; }
        }

        public void Execute(IRocketPlayer caller, string[] command)
        {
            int cLength = command.Length;
            UnturnedPlayer cPlayer = (UnturnedPlayer)caller;
            UnturnedPlayer target;

            if (command.Length < 1)
            {

            }
            else if (command.Length == 1)
            {
                if (command[0].ToString().ToLower() == "set")
                {
                    Rocket.Unturned.Chat.UnturnedChat.Say(cPlayer, "Use /dm set <player>", Color.grey);
                    Rocket.Unturned.Chat.UnturnedChat.Say(cPlayer, "Then, use /dm <message>", Color.grey);
                }
                if (command[0].ToString().ToLower() != "set")
                {
                    //send single message to set recipient, command[0] is message
                    string messageText = command[0].ToString().ToLower();
                    if (DM.Plugin.Instance.setPlayers.ContainsKey(cPlayer.CSteamID))
                    {
                        Steamworks.CSteamID setPlayerSID = DM.Plugin.Instance.setPlayers[cPlayer.CSteamID];
                        UnturnedPlayer setPlayer = UnturnedPlayer.FromCSteamID(setPlayerSID);

                        Rocket.Unturned.Chat.UnturnedChat.Say(cPlayer, "TO: " + setPlayer.CharacterName + " >> " + messageText, Color.cyan);
                        Rocket.Unturned.Chat.UnturnedChat.Say(setPlayer, "FROM: " + cPlayer.CharacterName + " >> " + messageText, Color.clear);
                        Rocket.Unturned.Chat.UnturnedChat.Say(setPlayer, "Reply with /r <message>", Color.gray);
                        DM.Plugin.Instance.lastReceived[setPlayerSID] = cPlayer.CSteamID;
                    }
                    else
                    {
                        Rocket.Unturned.Chat.UnturnedChat.Say(cPlayer, "You don't have a player set for quick DMs!", Color.red);
                    }
                }
            }
            else if (command.Length == 2)
            {
                target = UnturnedPlayer.FromName(command[1].ToString());
                if (command[0].ToString().ToLower() == "set" && target != null)
                {
                    //set recipient
                    if (DM.Plugin.Instance.setPlayers.ContainsKey(cPlayer.CSteamID))
                    {
                        DM.Plugin.Instance.setPlayers[cPlayer.CSteamID] = target.CSteamID;
                    }
                    else if (!DM.Plugin.Instance.setPlayers.ContainsKey(cPlayer.CSteamID))
                    {
                        DM.Plugin.Instance.setPlayers.Add(cPlayer.CSteamID, target.CSteamID);
                    }
                    Rocket.Unturned.Chat.UnturnedChat.Say(cPlayer, target.CharacterName + " has been set for quick DMs!", Color.grey);
                }
                else if (command[0].ToString().ToLower() == "set" && target == null)
                {
                    Rocket.Unturned.Chat.UnturnedChat.Say(cPlayer, "Player is offline!", Color.red);
                }
                else if (command[0].ToString().ToLower() != "set")
                {
                    target = UnturnedPlayer.FromName(command[0].ToString());
                    if (target != null)
                    {
                        //send single message to unset recipient, command[1] is message
                        string messageText = command[1].ToString();

                        Rocket.Unturned.Chat.UnturnedChat.Say(cPlayer, "TO: " + target.CharacterName + " >> " + messageText, Color.cyan);
                        Rocket.Unturned.Chat.UnturnedChat.Say(target, "FROM: " + cPlayer.CharacterName + " >> " + messageText, Color.clear);
                        Rocket.Unturned.Chat.UnturnedChat.Say(target, "Reply with /r <message>", Color.gray);
                        DM.Plugin.Instance.lastReceived[target.CSteamID] = cPlayer.CSteamID;
                    }
                    else if (target == null)
                    {
                        Rocket.Unturned.Chat.UnturnedChat.Say(cPlayer, "Player is offline!", Color.red);
                    }
                }
            }
            else if (command.Length > 2)
            {
                target = UnturnedPlayer.FromName(command[0].ToString());
                if (target == null)
                {
                    //send multi message to set recipient
                    string messageText = "";
                    for (int i = 0; i < cLength; i++)
                    {
                        messageText = messageText + command[i].ToString() + " ";
                    }
                    if (DM.Plugin.Instance.setPlayers.ContainsKey(cPlayer.CSteamID))
                    {
                        Steamworks.CSteamID setPlayerSID = DM.Plugin.Instance.setPlayers[cPlayer.CSteamID];
                        UnturnedPlayer setPlayer = UnturnedPlayer.FromCSteamID(setPlayerSID);

                        Rocket.Unturned.Chat.UnturnedChat.Say(cPlayer, "TO: " + setPlayer.CharacterName + " >> " + messageText, Color.cyan);
                        Rocket.Unturned.Chat.UnturnedChat.Say(setPlayer, "FROM: " + cPlayer.CharacterName + " >> " + messageText, Color.clear);
                        Rocket.Unturned.Chat.UnturnedChat.Say(setPlayer, "Reply with /r <message>", Color.gray);
                        DM.Plugin.Instance.lastReceived[setPlayerSID] = cPlayer.CSteamID;
                    }
                    else
                    {
                        Rocket.Unturned.Chat.UnturnedChat.Say(cPlayer, "You don't have a player set for quick DMs!", Color.red);
                    }
                }
                else if (target != null)
                {
                    //send multi message to unset recipient
                    string messageText = "";
                    for (int i = 1; i < cLength; i++)
                    {
                        messageText = messageText + command[i].ToString() + " ";
                    }
                    Rocket.Unturned.Chat.UnturnedChat.Say(cPlayer, "TO: " + target.CharacterName + " >> " + messageText, Color.cyan);
                    Rocket.Unturned.Chat.UnturnedChat.Say(target, "FROM: " + cPlayer.CharacterName + " >> " + messageText, Color.clear);
                    Rocket.Unturned.Chat.UnturnedChat.Say(target, "Reply with /r <message>", Color.gray);
                    DM.Plugin.Instance.lastReceived[target.CSteamID] = cPlayer.CSteamID;
                }
            }
        }

        //COMMAND CONTROLS
        public bool AllowFromConsole
		{
			get
			{
				return false;
			}
		}
		public bool RunFromConsole
		{
			get
            {
                return false;
            }
		}
		public string Name
		{
			get
            {
                return "dm";
            }
		}
		public List<string> Aliases
        {
            get
            {
                return new List<string>()
                {
                    "message"
                };
            }
        }
		public string Syntax
		{
			get
			{
				return "<player> <message>";
			}
		}
		public string Help
		{
			get
            {
                return "Send a direct private message to another player!";
            }
		}
		public List<string> Permissions
		{
			get
			{
				return new List<string>()
                {
                    "directmsngr.dm"
                };
			}
		}
    }

}