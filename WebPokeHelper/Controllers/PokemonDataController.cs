using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Diagnostics;

namespace WebPokeHelper.Controllers
{
    public class PokemonDataController : Controller
    {
        private SqlConnection sqlCon = new SqlConnection("Server=SPARKY-PC\\SQLEXPRESS;Database=PokeData;Integrated Security=True");

        public IActionResult Index(string pokemonName = "")
        {
            sqlCon.Open();

            PokemonDataModel model = new PokemonDataModel();
            List<int> allPokemonIDs = new List<int>();
            List<Models.Pokemon> pokemonList = new List<Models.Pokemon>();

            SqlCommand allPokemonCommand = new SqlCommand("SELECT POKEMON_ID FROM POKEMON", sqlCon);

            using (SqlDataReader dr = allPokemonCommand.ExecuteReader())
            {
                while (dr.Read())
                {
                    allPokemonIDs.Add((int)dr["POKEMON_ID"]);
                }
            }

            for (int i = 0; i < allPokemonIDs.Count; i++)
            {
                Models.Pokemon pokemon = new Models.Pokemon();
                SqlCommand command = new SqlCommand("SELECT * FROM POKEMON WHERE POKEMON_ID = @pk", sqlCon);
                command.Parameters.Add(new SqlParameter("@pk", allPokemonIDs[i]));

                using (SqlDataReader dr = command.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        pokemon.pokemonID = (int)dr["POKEMON_ID"];
                        pokemon.name = (string)dr["NAME"];
                        pokemon.nationalDexNo = (int)dr["NATIONAL_DEX_NO"];
                        pokemon.galarDexNo = (int)dr["GALAR_DEX_NO"];
                        pokemon.hp = (int)dr["HP"];
                        pokemon.atk = (int)dr["ATK"];
                        pokemon.def = (int)dr["DEF"];
                        pokemon.spAtk = (int)dr["SP_ATK"];
                        pokemon.spDef = (int)dr["SP_DEF"];
                        pokemon.spd = (int)dr["SPD"];
                        pokemon.description = (string)dr["DESCRIPTION"];

                        //Calculate lvl 50 stats from base stats
                        pokemon.hp = ((2 * pokemon.hp + 31 + 63 + 100) * 50) / 100 + 10;
                        pokemon.atk = Convert.ToInt32(((((2 * pokemon.atk + 31 + 63) * 50) / 100 + 5) * 1.1f));
                        pokemon.def = Convert.ToInt32(((((2 * pokemon.def + 31 + 63) * 50) / 100 + 5) * 1.1f));
                        pokemon.spAtk = Convert.ToInt32(((((2 * pokemon.spAtk + 31 + 63) * 50) / 100 + 5) * 1.1f));
                        pokemon.spDef = Convert.ToInt32(((((2 * pokemon.spDef + 31 + 63) * 50) / 100 + 5) * 1.1f));
                        pokemon.spd = Convert.ToInt32(((((2 * pokemon.spd + 31 + 63) * 50) / 100 + 5) * 1.1f));
                    }
                }

                //Types
                List<Models.Type> types = new List<Models.Type>();
                SqlCommand typesCommand = new SqlCommand(@"SELECT * FROM POKEMON_TYPES INNER JOIN TYPES ON POKEMON_TYPES.TYPES_ID = TYPES.TYPES_ID 
                    WHERE POKEMON_TYPES.POKEMON_ID = @pokemonPk", sqlCon);
                typesCommand.Parameters.Add(new SqlParameter("@pokemonPk", allPokemonIDs[i]));

                using (SqlDataReader drTypes = typesCommand.ExecuteReader())
                {
                    while (drTypes.Read())
                    {
                        Models.Type type = new Models.Type();
                        type.TypeID = (int)drTypes["TYPES_ID"];
                        type.name = (string)drTypes["TYPE"];

                        types.Add(type);
                    }
                }

                pokemon.types = types;

                //Abilities
                List<Models.Ability> abilities = new List<Models.Ability>();
                SqlCommand abilitiesCommand = new SqlCommand(@"SELECT * FROM POKEMON_ABILITIES INNER JOIN ABILITIES ON POKEMON_ABILITIES.ABILITIES_ID = ABILITIES.ABILITIES_ID
                    WHERE POKEMON_ABILITIES.POKEMON_ID = @pokemonPk", sqlCon);
                abilitiesCommand.Parameters.Add(new SqlParameter("@pokemonPk", allPokemonIDs[i]));

                using (SqlDataReader drAbilities = abilitiesCommand.ExecuteReader())
                {
                    while (drAbilities.Read())
                    {
                        Models.Ability ability = new Models.Ability();
                        ability.abilityID = (int)drAbilities["ABILITIES_ID"];
                        ability.name = (string)drAbilities["NAME"];
                        ability.description = (string)drAbilities["DESCRIPTION"];

                        abilities.Add(ability);
                    }
                }

                pokemon.abilities = abilities;

                //Moves
                List<Models.Move> moves = new List<Models.Move>();
                SqlCommand movesCommand = new SqlCommand(@"SELECT * FROM POKEMON_MOVES INNER JOIN MOVES ON POKEMON_MOVES.MOVES_ID = MOVES.MOVES_ID
                    WHERE POKEMON_MOVES.POKEMON_ID = @pokemonPK", sqlCon);
                movesCommand.Parameters.Add(new SqlParameter("@pokemonPK", allPokemonIDs[i]));

                using (SqlDataReader drMoves = movesCommand.ExecuteReader())
                {
                    while (drMoves.Read())
                    {
                        Models.Move move = new Models.Move();
                        move.moveID = (int)drMoves["MOVES_ID"];
                        move.name = (string)drMoves["NAME"];
                        move.type = (string)drMoves["TYPE"];
                        move.category = (string)drMoves["CATEGORY"];
                        move.powerPoints = (int)drMoves["POWER_POINTS"];
                        move.basePower = (int)drMoves["BASE_POWER"];
                        move.accuracy = (int)drMoves["ACCURACY"];
                        move.effect = (string)drMoves["EFFECT"];
                        move.secondaryEffect = (string)drMoves["SECONDARY_EFFECT"];
                        move.effectRate = (int)drMoves["EFFECT_RATE"];
                        move.maxMove = (string)drMoves["MAX_MOVE"];
                        move.maxMovePower = (int)drMoves["MAX_MOVE_POWER"];
                        move.baseCritRate = (float)drMoves["BASE_CRIT_RATE"];
                        move.speedPriority = (int)drMoves["SPEED_PRIORITY"];
                        move.target = (string)drMoves["TARGET"];

                        moves.Add(move);
                    }
                }

                pokemon.moves = moves;

                if (pokemon.name == pokemonName)
                {
                    model.pokemonChosen = pokemon;
                }

                pokemonList.Add(pokemon);
            }

            sqlCon.Close();

            model.pokemonList = pokemonList;
            model.pokemonName = pokemonName;

            return View(model);
        }
    }

    public class PokemonDataModel
    {
        public List<Models.Pokemon> pokemonList { get; set; }
        public string pokemonName { get; set; }
        public Models.Pokemon pokemonChosen { get; set; }
    }
}