using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;
using System.Text.Json;

namespace WebPokeHelper
{
    public sealed class DataManager
    {
        private static DataManager instance = null;
        private static readonly object padlock = new object();

        public Pokemons Pokemon;
        public Moves Moves;
        public Abilities Abilities;

        public DataManager()
        {
            PopulateDataFromJSON();
        }

        public void PopulateDataFromJSON()
        {
            //Pokemon
            using (StreamReader sr = new StreamReader(Directory.GetCurrentDirectory() + @"/Resources/Pokemon.json"))
            {
                Pokemon = new Pokemons(JsonSerializer.Deserialize<List<Pokemon>>(sr.ReadToEnd()));
            }

            //Moves
            using (StreamReader sr = new StreamReader(Directory.GetCurrentDirectory() + @"/Resources/Moves.json"))
            {
                Moves = new Moves(JsonSerializer.Deserialize<List<Move>>(sr.ReadToEnd()));
            }

            //Abilities (No longer needed, this data is now stored in the Pokemon itself)
            using (StreamReader sr = new StreamReader(Directory.GetCurrentDirectory() + @"/Resources/Abilities.json"))
            {
                Abilities = new Abilities(JsonSerializer.Deserialize<List<Ability>>(sr.ReadToEnd()));
            }
        }

        public static DataManager Instance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new DataManager();
                    }

                    return instance;
                }
            }
        }
    }
}
