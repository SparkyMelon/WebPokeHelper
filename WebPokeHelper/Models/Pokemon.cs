using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebPokeHelper.Models
{
    public class Pokemon
    {
        public int pokemonID { get; set; }
        public string name { get; set; }
        public int nationalDexNo { get; set; }
        public int galarDexNo { get; set; }
        public int hp { get; set; }
        public int atk { get; set; }
        public int def { get; set; }
        public int spAtk { get; set; }
        public int spDef { get; set; }
        public int spd { get; set; }
        public string description { get; set; }
        public List<Type> types { get; set; }
        public List<Ability> abilities { get; set; }
        public List<Move> moves { get; set; }

    }

    public class Type
    {
        public int TypeID { get; set; }
        public string name { get; set; }
    }

    public class Ability
    {
        public int abilityID { get; set; }
        public string name { get; set; }
        public string description { get; set; }
    }

    public class Move
    {
        public int moveID { get; set; }
        public string name { get; set; }
        public string type { get; set; }
        public string category { get; set; }
        public int powerPoints { get; set; }
        public int basePower { get; set; }
        public int accuracy { get; set; }
        public string effect { get; set; }
        public string secondaryEffect { get; set; }
        public int effectRate { get; set; }
        public string maxMove { get; set; }
        public int maxMovePower { get; set; }
        public float baseCritRate { get; set; }
        public int speedPriority { get; set; }
        public string target { get; set; }
    }
}
