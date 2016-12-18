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

//     THIS
//        COMMAND
//           IS
//             OBSOLETE!!
//                   ...
//                      USE
//                        "/dm set"
//                              INSTEAD!!

namespace Command
{
    public class QMCommand : IRocketCommand
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

            //command is exactly equal to 2 characters "qm"
            if (command.Length == 2)
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
                    Rocket.Unturned.Chat.UnturnedChat.Say(cPlayer, "Player does not exist or is offline!", Color.red);
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
                        Rocket.Unturned.Chat.UnturnedChat.Say(cPlayer, "Player does not exist or is offline!", Color.yellow);
                    }
                }
            }


            else if (command.Length > 2)
            {
                target = UnturnedPlayer.FromName(command[0].ToString());
                if (target == null)
                {
                    //send multi messages to the set recipient
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
                    }
                    else
                    {
                        Rocket.Unturned.Chat.UnturnedChat.Say(cPlayer, "Sorry, you don't have a player set!", Color.red);
                    }
                }
            }
            //if cmd is less than OR equal to 2 characters
            else if (cLength <= 2)
            {

                //displays this msg...
                Rocket.Unturned.Chat.UnturnedChat.Say(cPlayer, "Use /qm set <player>, then use /qm <message>", Color.gray);
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
            get { return "qm"; }
        }
        public List<string> Aliases
        {
            get
            {
                return new List<string>()
                {
                    "im"
                };
            }
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
            get { return "Send a quick private message to a set player!"; }
        }
        public List<string> Permissions
        {
            get
            {
                return new List<string>()
                {
                    "directmsngr.qm"
                };
            }
        }
    }
}