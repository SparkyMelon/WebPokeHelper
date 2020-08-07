using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebPokeHelper
{
    public class Pokemon
    {
        public string name { get; set; }
        public int nationalDexNo { get; set; }
        public int galarDexNo { get; set; }
        public int hp { get; set; }
        public int atk { get; set; }
        public int def { get; set; }
        public int spAtk { get; set; }
        public int spDef { get; set; }
        public int spd { get; set; }
        public List<string> abilities { get; set; }
        public List<string> types { get; set; }
        public List<string> moves { get; set; }
        public string description { get; set; }

        public Pokemon()
        { 
            
        }
    }

    //public class Pokemon
    //{
    //    public int id { get; set; }
    //    public string name { get; set; }
    //    public int stage { get; set; }
    //    public string galarDexNo { get; set; }
    //    public List<int> baseStats { get; set; }
    //    public List<string> abilities { get; set; }
    //    public List<string> types { get; set; }
    //    public List<string> levelUpMoves { get; set; }
    //    public List<string> eggMoves { get; set; }
    //    public List<int> tms { get; set; }
    //    public List<int> trs { get; set; }
    //    public string description { get; set; }

    //    public Pokemon()
    //    {

    //    }

    //    public Pokemon(PokemonJson pokemonJson)
    //    {
    //        id = pokemonJson.id;
    //        name = pokemonJson.name;
    //        galarDexNo = pokemonJson.galar_dex;
    //        baseStats = pokemonJson.base_stats;
    //        abilities = pokemonJson.abilities;
    //        types = pokemonJson.types;


    //        //Old way - make a class for moves to store all data and get that data from here: https://www.serebii.net/attackdex-swsh/
    //        //levelUpMoves = pokemonJson.level_up_moves;
    //        levelUpMoves = new List<string>();
    //        foreach (List<object> move in pokemonJson.level_up_moves)
    //            levelUpMoves.Add(move[1].ToString());

    //        //eggMoves = pokemonJson.egg_moves;
    //        eggMoves = new List<string>();
    //        foreach (object move in pokemonJson.egg_moves)
    //            eggMoves.Add(move.ToString());

    //        tms = new List<int>();
    //        foreach (object tm in pokemonJson.tms)
    //            tms.Add(Convert.ToInt32(tm.ToString()));

    //        trs = new List<int>();
    //        foreach (object tr in pokemonJson.trs)
    //            trs.Add(Convert.ToInt32(tr.ToString()));

    //        description = pokemonJson.description;
    //    }
    //}

    public class Pokemons
    {
        private List<Pokemon> pokemonList;

        public Pokemons(List<Pokemon> pokemonList)
        {
            this.pokemonList = pokemonList;
        }

        public List<Pokemon> GetPokemonList()
        {
            return pokemonList;
        }

        public Pokemon GetPokemon(string name)
        {
            return pokemonList.First(pokemon => pokemon.name == name);
        }
    }

    public class Move
    {
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
        public string record { get; set; }

        public Move()
        {

        }

        public Move(string name,
            string type,
            string category,
            int powerPoints,
            int basePower,
            int accuracy,
            string effect,
            string secondaryEffect,
            int effectRate,
            string maxMove,
            int maxMovePower,
            float baseCritRate,
            int speedPriority,
            string target,
            string record)
        {
            this.name = name;
            this.type = type;
            this.category = category;
            this.powerPoints = powerPoints;
            this.basePower = basePower;
            this.accuracy = accuracy;
            this.effect = effect;
            this.secondaryEffect = secondaryEffect;
            this.effectRate = effectRate;
            this.maxMove = maxMove;
            this.maxMovePower = maxMovePower;
            this.baseCritRate = baseCritRate;
            this.speedPriority = speedPriority;
            this.target = target;
            this.record = record;
        }
    }

    public class Moves
    {
        private List<Move> moveList;

        public Moves(List<Move> moveList)
        {
            this.moveList = moveList;
        }

        public List<Move> GetMoveList()
        {
            return moveList;
        }
    }

    public class Ability
    {
        public string name { get; set; }
        public string description { get; set; }
    }

    public class Abilities
    {
        private List<Ability> abilityList;

        public Abilities()
        {

        }

        public Abilities(List<Ability> abilityList)
        {
            this.abilityList = abilityList;
        }

        public Ability GetAbility(string name)
        {
            return abilityList.First(ability => ability.name == name);
        }

        public List<Ability> GetAbilityList()
        {
            return abilityList;
        }
    }

    public class PokemonJson
    {
        public int id { get; set; }
        public string name { get; set; }
        public int stage { get; set; }
        public string galar_dex { get; set; }
        public List<int> base_stats { get; set; }
        public List<int> ev_yield { get; set; }
        public List<string> abilities { get; set; }
        public List<string> types { get; set; }
        public List<object> items { get; set; }
        public string exp_group { get; set; }
        public List<string> egg_groups { get; set; }
        public int hatch_cycles { get; set; }
        public double height { get; set; }
        public double weight { get; set; }
        public string color { get; set; }
        public List<List<object>> level_up_moves { get; set; }
        public List<object> egg_moves { get; set; }
        public List<object> tms { get; set; }
        public List<object> trs { get; set; }
        public List<object> evolutions { get; set; }
        public string description { get; set; }
    }

    //public class MoveJson
    //{
    //    public string name { get; set; }
    //    public string type { get; set; }
    //    public string category { get; set; }
    //    public int power_points { get; set; }
    //    public int base_power { get; set; }
    //    public int accuracy { get; set; }
    //    public string effect { get; set; }
    //    public string secondary_effect { get; set; }
    //    public int effect_rate { get; set; }
    //    public string max_move { get; set; }
    //    public int max_move_power { get; set; }
    //    public float base_critical_hit_rate { get; set; }
    //    public int speed_priority { get; set; }
    //    public string target { get; set; }
    //    public string record { get; set; }

    //    //There's move detail that could be added here (like if the move counts as physical contact, or if it's a biting move etc)
    //}
}
