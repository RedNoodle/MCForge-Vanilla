/*
	Copyright 2010 MCSharp team (Modified for use with MCZall/MCLawl/MCForge)
	
	Dual-licensed under the	Educational Community License, Version 2.0 and
	the GNU General Public License, Version 3 (the "Licenses"); you may
	not use this file except in compliance with the Licenses. You may
	obtain a copy of the Licenses at
	
	http://www.opensource.org/licenses/ecl2.php
	http://www.gnu.org/licenses/gpl-3.0.html
	
	Unless required by applicable law or agreed to in writing,
	software distributed under the Licenses are distributed on an "AS IS"
	BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express
	or implied. See the Licenses for the specific language governing
	permissions and limitations under the Licenses.
*/
using System;
using System.Collections.Generic;
using System.Text;

namespace MCForge
{
    class CmdPlayers : Command
    {

        public override string name { get { return "players"; } }
        public override string shortcut { get { return "who"; } }
        public override string type { get { return "information"; } }
        public override bool museumUsable { get { return true; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Guest; } }
        public CmdPlayers() { }

        struct groups { public Group group; public List<string> players; }
        public override void Use(Player p, string message)
        {
            try
            {
                List<groups> playerList = new List<groups>();

                foreach (Group grp in Group.GroupList)
                {
                    if (grp.name != "nobody")
                    {
                        if (String.IsNullOrEmpty(message) || !Group.Exists(message))
                        {
                            groups groups;
                            groups.group = grp;
                            groups.players = new List<string>();
                            playerList.Add(groups);
                        }
                        else
                        {
                            Group grp2 = Group.Find(message);
                            if (grp2 != null && grp == grp2)
                            {
                                groups groups;
                                groups.group = grp;
                                groups.players = new List<string>();
                                playerList.Add(groups);
                            }
                        }
                    }
                }

                string devs = "";
                int totalPlayers = 0;
                foreach (Player pl in Player.players)
                {
                    if (!pl.hidden || p == null || p.group.Permission >= pl.group.Permission || Server.devs.Contains(p.name.ToLower()))
                    {
                        if (String.IsNullOrEmpty(message) || !Group.Exists(message) || Group.Find(message) == pl.group)
                        {
                            totalPlayers++;
                            string foundName = pl.name;

                            if (Server.afkset.Contains(pl.name))
                            {
                                foundName = pl.name + "-afk";
                            }

                            if (Server.devs.Contains(pl.name.ToLower()))
                            {
                                if (pl.voice)
                                    devs += " " + "&f+" + Server.DefaultColor + foundName + " (" + pl.level.name + "),";
                                else
                                    devs += " " + foundName + " (" + pl.level.name + "),";
                            }

                            if (pl.voice)
                                playerList.Find(grp => grp.group == pl.group).players.Add("&f+" + Server.DefaultColor + foundName + " (" + pl.level.name + ")");
                            else
                                playerList.Find(grp => grp.group == pl.group).players.Add(foundName + " (" + pl.level.name + ")");
                        }
                        else
                        {
                            Player.SendMessage(p, "There are no players of that rank online.");
                            return;
                        }
                    }
                }
                Player.SendMessage(p, "There are " + totalPlayers + " players online.");
                if (devs.Length > 0)
                {
                    Player.SendMessage(p, ":&9Developers:" + Server.DefaultColor + devs.Trim(','));
                }

                for (int i = playerList.Count - 1; i >= 0; i--)
                {
                    groups groups = playerList[i];
                    if (groups.players.Count > 0 || Server.showEmptyRanks)
                    {
                        string appendString = "";
                        foreach (string player in groups.players)
                            appendString += ", " + player;

                        if (appendString != "")
                            appendString = appendString.Remove(0, 2);
                        appendString = ":" + groups.group.color + getPlural(groups.group.trueName) + ": " + appendString;

                        Player.SendMessage(p, appendString);
                    }
                }
            }
            catch (Exception e) { Server.ErrorLog(e); }
        }

        public string getPlural(string groupName)
        {
            try
            {
                string last2 = groupName.Substring(groupName.Length - 2).ToLower();
                if ((last2 != "ed" || groupName.Length <= 3) && last2[1] != 's')
                {
                    return groupName + "s";
                }
                return groupName;
            }
            catch
            {
                return groupName;
            }
        }

        public override void Help(Player p)
        {
            Player.SendMessage(p, "/players [rank] - Shows name and general rank of all players");
        }
    }
}
