using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Diagnostics;

namespace WebPokeHelper.Controllers
{
    public class HomeController : Controller
    {
        private SqlConnection sqlCon = new SqlConnection("Server=SPARKY-PC\\SQLEXPRESS;Database=PokeData;Integrated Security=True");

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Details(string pokemonName)
        {
            sqlCon.Open();

            Models.Pokemon pokemon = new Models.Pokemon();
            SqlCommand command = new SqlCommand("SELECT * FROM POKEMON WHERE NAME = @name", sqlCon);
            command.Parameters.Add(new SqlParameter("@name", pokemonName));

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
                }
            }

            //Types
            List<Models.Type> types = new List<Models.Type>();
            SqlCommand typesCommand = new SqlCommand(@"SELECT TYPES.TYPE FROM POKEMON_TYPES INNER JOIN TYPES ON POKEMON_TYPES.TYPES_ID = TYPES.TYPES_ID 
                    WHERE POKEMON_TYPES.POKEMON_ID = @pokemonPk", sqlCon);
            typesCommand.Parameters.Add(new SqlParameter("@pokemonPk", pokemon.pokemonID));

            using (SqlDataReader drTypes = typesCommand.ExecuteReader())
            {
                while (drTypes.Read())
                {
                    Models.Type type = new Models.Type();
                    type.TypeID = (int)drTypes["TYPE_ID"];
                    type.name = (string)drTypes["TYPE"];

                    types.Add(type);
                }
            }

            pokemon.types = types;

            //Abilities
            List<Models.Ability> abilities = new List<Models.Ability>();
            SqlCommand abilitiesCommand = new SqlCommand(@"SELECT ABILITIES.NAME FROM POKEMON_ABILITIES INNER JOIN ABILITIES ON POKEMON_ABILITIES.ABILITIES_ID = ABILITIES.ABILITIES_ID
                    WHERE POKEMON_ABILITIES.POKEMON_ID = @pokemonPk", sqlCon);
            abilitiesCommand.Parameters.Add(new SqlParameter("@pokemonPk", pokemon.pokemonID));

            using (SqlDataReader drAbilities = abilitiesCommand.ExecuteReader())
            {
                while (drAbilities.Read())
                {
                    Models.Ability ability = new Models.Ability();
                    ability.abilityID = (int)drAbilities["ABILITY_ID"];
                    ability.name = (string)drAbilities["NAME"];
                    ability.description = (string)drAbilities["DESCRIPTION"];

                    abilities.Add(ability);
                }
            }

            pokemon.abilities = abilities;

            //Moves
            List<Models.Move> moves = new List<Models.Move>();
            SqlCommand movesCommand = new SqlCommand(@"SELECT MOVES.NAME FROM POKEMON_MOVES INNER JOIN MOVES ON POKEMON_MOVES.MOVES_ID = MOVES.MOVES_ID
                    WHERE POKEMON_MOVES.POKEMON_ID = @pokemonPK", sqlCon);
            movesCommand.Parameters.Add(new SqlParameter("@pokemonPK", pokemon.pokemonID));

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

            sqlCon.Close();

            return View(pokemon);
        }
    }
}