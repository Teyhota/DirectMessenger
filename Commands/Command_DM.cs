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

			if (cLength == 2)
			{
                Rocket.Unturned.Chat.UnturnedChat.Say(cPlayer, "Use /dm <player> <message>", Color.gray);
                target = UnturnedPlayer.FromName(command[1].ToString());

                if (command[0].ToString().ToLower() != "set")
                {
                    target = UnturnedPlayer.FromName(command[0].ToString());
                    if (target != null)
                    {
                        //send a message to an unset recipient, command[1] is message
                        string messageText = command[1].ToString();

                        Rocket.Unturned.Chat.UnturnedChat.Say(cPlayer, "TO: " + target.CharacterName + " >> " + messageText, Color.cyan);
                        Rocket.Unturned.Chat.UnturnedChat.Say(target, "FROM: " + cPlayer.CharacterName + " >> " + messageText, Color.clear);
                        Rocket.Unturned.Chat.UnturnedChat.Say(target, "Reply with /r <message>", Color.gray);
                        PM.Plugin.Instance.lastReceived[target.CSteamID] = cPlayer.CSteamID;
                    }
                    else if (target == null)
                    {
                        Rocket.Unturned.Chat.UnturnedChat.Say(cPlayer, "Cannot find player!", Color.red);
                    }
                }
			}
            else if (command.Length < 1)
            {
                Rocket.Unturned.Chat.UnturnedChat.Say(cPlayer, "Use /dm <player> <message>", Color.gray);
                target = UnturnedPlayer.FromName(command[1].ToString());

                if (command[0].ToString().ToLower() != "set")
                {
                    target = UnturnedPlayer.FromName(command[0].ToString());
                    if (target != null)
                    {
                        //send a message to an unset recipient, command[1] is message
                        string messageText = command[1].ToString();

                        Rocket.Unturned.Chat.UnturnedChat.Say(cPlayer, "TO: " + target.CharacterName + " >> " + messageText, Color.cyan);
                        Rocket.Unturned.Chat.UnturnedChat.Say(target, "FROM: " + cPlayer.CharacterName + " >> " + messageText, Color.clear);
                        Rocket.Unturned.Chat.UnturnedChat.Say(target, "Reply with /r <message>", Color.gray);
                        PM.Plugin.Instance.lastReceived[target.CSteamID] = cPlayer.CSteamID;
                    }
                    else if (target == null)
                    {
                        Rocket.Unturned.Chat.UnturnedChat.Say(cPlayer, "Cannot find player!", Color.red);
                    }
                }
			}
            else if (command.Length < 1)
            {
            
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
			get { return "dm"; }
		}
		public List<string> Aliases
        {
            get
            {
                return new List<string>()
                {

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
			get { return "Send a direct private message to another player!"; }
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