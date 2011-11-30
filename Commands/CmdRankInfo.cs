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
using System.IO;

namespace MCForge
{
    class CmdRankInfo : Command
    {
        public override string name { get { return "rankinfo"; } }
        public override string shortcut { get { return "ri"; } }
        public override string type { get { return "mod"; } }
        public override bool museumUsable { get { return true; } }
        public override LevelPermission defaultRank { get { return LevelPermission.AdvBuilder; } }
        public CmdRankInfo() { }

        public override void Use(Player p, string message)
        {
            if (message == "") { message = p.name.ToString(); }
            if (Player.Find(message) != null) { message = Player.Find(message).name.ToString(); }
            string alltext = File.ReadAllText("text/rankinfo.txt").ToLower();
            if (alltext.ToLower().Contains(message.ToLower()) == false)
            {
                Player.SendMessage(p, "&cPlayer &a" + message + "&c has not been ranked yet!");
            //    Player.SendMessage(p, alltext);
                return;
            }
            


            foreach (string line3 in File.ReadAllLines("text/rankinfo.txt"))
            {
                if (line3.Split(' ')[0].ToLower().Contains(message.ToLower()))
                {
                    string newrank = line3.Split(' ')[7];
                    string oldrank = line3.Split(' ')[8];
                    string assigner = line3.Split(' ')[1];
                    string whoinfo = line3.Split(' ')[0];
                    Group newrankcolor = Group.Find(newrank);
                    Group oldrankcolor = Group.Find(oldrank);
                    int minutes = Convert.ToInt32(line3.Split(' ')[2]);
                    int hours = Convert.ToInt32(line3.Split(' ')[3]);
                    int days = Convert.ToInt32(line3.Split(' ')[4]);
                    int months = Convert.ToInt32(line3.Split(' ')[5]);
                    int years = Convert.ToInt32(line3.Split(' ')[6]);
                    DateTime ExpireDate = new DateTime(years, months, days, hours, minutes, 0);
                    Player.SendMessage(p, "&1Rank Information of " + whoinfo);
                    Player.SendMessage(p, "&aNew rank: " + newrankcolor.color + newrank);
                    Player.SendMessage(p, "&aOld Rank: " + oldrankcolor.color + oldrank);
                    Player.SendMessage(p, "&aDate of assignment: " + ExpireDate.ToString());
                    Player.SendMessage(p, "&aRanked by: " + assigner);
                }
            }


        }
        public override void Help(Player p)
        {
            Player.SendMessage(p, "/rankinfo - Returns the information available about someones ranking");
        }
    }
}
