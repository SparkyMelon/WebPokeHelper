using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Diagnostics;

namespace WebPokeHelper
{
    public sealed class DbData
    {
        private static DbData instance = null;
        private static readonly object padlock = new object();

        private SqlConnection sqlCon;

        public DbData()
        {
            //Trusted_Conn‌ection=True;Multiple‌​ActiveResultSets=tru‌​e;
            sqlCon = new SqlConnection("Server=SPARKY-PC\\SQLEXPRESS;Database=PokeData;Integrated Security=True");
        }

        public void SQLQuery(string sql, List<SqlParameter> sqlParams = null)
        {
            sqlCon.Open();

            SqlCommand command;
            command = new SqlCommand(sql, sqlCon);
            
            if (sqlParams != null)
            {
                foreach (SqlParameter sqlParam in sqlParams)
                {
                    command.Parameters.Add(sqlParam);
                }
            }

            try
            {
                command.ExecuteReader();
            }
            catch (Exception ex)
            {
                Debugger.Break();
            }

            sqlCon.Close();
        }

        public object SQLQueryGetSingleResult(string sql, List<SqlParameter> sqlParams = null)
        {
            sqlCon.Open();

            object result = null;
            SqlCommand command;
            command = new SqlCommand(sql, sqlCon);

            if (sqlParams != null)
            {
                foreach (SqlParameter sqlParam in sqlParams)
                {
                    command.Parameters.Add(sqlParam);
                }
            }

            try
            {
                using (SqlDataReader dr = command.ExecuteReader())
                {
                    if (dr.Read())
                        result = dr.GetValue(0);
                    else
                        Debugger.Break();
                }
            }
            catch (Exception ex)
            {
                Debugger.Break();
            }

            sqlCon.Close();

            return result;
        }

        public void CreateTablesForDB()
        {
            SQLQuery("DROP TABLE POKEMON_TYPES");
            SQLQuery("DROP TABLE POKEMON_MOVES");
            SQLQuery("DROP TABLE POKEMON_ABILITIES");
            SQLQuery("DROP TABLE POKEMON");
            SQLQuery("DROP TABLE TYPES");
            SQLQuery("DROP TABLE MOVES");
            SQLQuery("DROP TABLE ABILITIES");


            //Pokemon
            string sql = @"
                CREATE TABLE POKEMON (
                    POKEMON_ID int NOT NULL PRIMARY KEY,
                    NAME varchar(25) NOT NULL,
                    NATIONAL_DEX_NO int NOT NULL,
                    GALAR_DEX_NO int NOT NULL,
                    HP int NOT NULL,
                    ATK int NOT NULL,
                    DEF int NOT NULL,
                    SP_ATK int NOT NULL,
                    SP_DEF int NOT NULL,
                    SPD int NOT NULL,
                    DESCRIPTION varchar(255) NOT NULL
                )
            ";
            SQLQuery(sql);

            //Types
            sql = @"
                CREATE TABLE TYPES (
                    TYPES_ID int NOT NULL PRIMARY KEY,
                    TYPE VARCHAR(30) NOT NULL
                )
            ";
            SQLQuery(sql);

            sql = @"
                CREATE TABLE POKEMON_TYPES (
                    POKEMON_TYPES_ID int NOT NULL PRIMARY KEY,
                    POKEMON_ID INT NOT NULL FOREIGN KEY REFERENCES POKEMON(POKEMON_ID),
                    TYPES_ID INT NOT NULL FOREIGN KEY REFERENCES TYPES(TYPES_ID)
                )
            ";
            SQLQuery(sql);

            //Abilities
            sql = @"
                CREATE TABLE ABILITIES (
                    ABILITIES_ID int NOT NULL PRIMARY KEY,
                    NAME VARCHAR(30) NOT NULL,
                    DESCRIPTION VARCHAR(255) NOT NULL
                )
            ";
            SQLQuery(sql);

            sql = @"
                CREATE TABLE POKEMON_ABILITIES (
                    POKEMON_ABILITIES_ID int NOT NULL PRIMARY KEY,
                    POKEMON_ID INT NOT NULL FOREIGN KEY REFERENCES POKEMON(POKEMON_ID),
                    ABILITIES_ID INT NOT NULL FOREIGN KEY REFERENCES ABILITIES(ABILITIES_ID)
                )
            ";
            SQLQuery(sql);

            //Moves
            sql = @"
                CREATE TABLE MOVES (
                    MOVES_ID int NOT NULL PRIMARY KEY,
                    NAME VARCHAR(50) NOT NULL,
                    TYPE VARCHAR(20) NOT NULL,
                    CATEGORY VARCHAR(15) NOT NULL,
                    POWER_POINTS INT NOT NULL,
                    BASE_POWER INT NOT NULL,
                    ACCURACY INT NOT NULL,
                    EFFECT VARCHAR(255) NOT NULL,
                    SECONDARY_EFFECT VARCHAR(500) NOT NULL,
                    EFFECT_RATE INT NOT NULL,
                    MAX_MOVE VARCHAR(25),
                    MAX_MOVE_POWER INT NOT NULL,
                    BASE_CRIT_RATE REAL NOT NULL,
                    SPEED_PRIORITY INT NOT NULL,
                    TARGET VARCHAR(35) NOT NULL
                )
            ";
            SQLQuery(sql);

            sql = @"
                CREATE TABLE POKEMON_MOVES (
                    POKEMON_MOVES_ID INT NOT NULL PRIMARY KEY,
                    POKEMON_ID INT NOT NULL FOREIGN KEY REFERENCES POKEMON(POKEMON_ID),
                    MOVES_ID INT NOT NULL FOREIGN KEY REFERENCES MOVES(MOVES_ID)
                )
            ";
            SQLQuery(sql);
        }

        public void LoadAllJSONDataToDB()
        {
            //Clear tables first
            SQLQuery("DELETE FROM POKEMON_TYPES");
            SQLQuery("DELETE FROM POKEMON_MOVES");
            SQLQuery("DELETE FROM POKEMON_ABILITIES");
            SQLQuery("DELETE FROM POKEMON");
            SQLQuery("DELETE FROM TYPES");
            SQLQuery("DELETE FROM MOVES");
            SQLQuery("DELETE FROM ABILITIES");
            SQLQuery("DELETE FROM POKEMON_ABILITIES");

            //Populate tables
            //Types
            List<string> types = new List<string>()
            {
                "Normal",
                "Fire",
                "Water",
                "Grass",
                "Electric",
                "Ice",
                "Fighting",
                "Poison",
                "Ground",
                "Flying",
                "Psychic",
                "Bug",
                "Rock",
                "Ghost",
                "Dragon",
                "Dark",
                "Steel",
                "Fairy"
            };

            string sqlTypes = "INSERT INTO TYPES (TYPES_ID, TYPE) VALUES ";

            for (int i = 0; i < types.Count; i++)
            {
                sqlTypes += "(" + (i + 1) + ", '" + types[i] + "'),";
            }

            sqlTypes = sqlTypes.TrimEnd(',');

            SQLQuery(sqlTypes);

            //Abilities
            List<Ability> abilities = DataManager.Instance.Abilities.GetAbilityList();

            for (int i = 0; i < abilities.Count; i++)
            {
                List<SqlParameter> abilityParams = new List<SqlParameter>();
                abilityParams.Add(new SqlParameter("@id", i + 1));
                abilityParams.Add(new SqlParameter("@name", abilities[i].name));
                abilityParams.Add(new SqlParameter("@desc", abilities[i].description));

                string sqlAbilities = @"INSERT INTO ABILITIES (ABILITIES_ID, NAME, DESCRIPTION) VALUES
                    (@id, @name, @desc)";

                SQLQuery(sqlAbilities, abilityParams);
            }

            //Moves
            List<Move> moves = DataManager.Instance.Moves.GetMoveList();

            for (int i = 0; i < moves.Count; i++)
            {
                List<SqlParameter> moveParams = new List<SqlParameter>();
                moveParams.Add(new SqlParameter("@id", i + 1));
                moveParams.Add(new SqlParameter("@name", moves[i].name));
                moveParams.Add(new SqlParameter("@type", moves[i].type));
                moveParams.Add(new SqlParameter("@cat", moves[i].category));
                moveParams.Add(new SqlParameter("@pp", moves[i].powerPoints));
                moveParams.Add(new SqlParameter("@power", moves[i].basePower));
                moveParams.Add(new SqlParameter("@acc", moves[i].accuracy));
                moveParams.Add(new SqlParameter("@ef", moves[i].effect));
                moveParams.Add(new SqlParameter("@secEf", moves[i].secondaryEffect));
                moveParams.Add(new SqlParameter("@efRate", moves[i].effectRate));
                moveParams.Add(new SqlParameter("@maxM", moves[i].maxMove));
                moveParams.Add(new SqlParameter("@maxMPow", moves[i].maxMovePower));
                moveParams.Add(new SqlParameter("@baseCR", moves[i].baseCritRate));
                moveParams.Add(new SqlParameter("@spdPri", moves[i].speedPriority));
                moveParams.Add(new SqlParameter("@tar", moves[i].target));


                string sqlMoves = @"INSERT INTO MOVES (MOVES_ID, NAME, TYPE, CATEGORY, POWER_POINTS, BASE_POWER, 
                    ACCURACY, EFFECT, SECONDARY_EFFECT, EFFECT_RATE, MAX_MOVE, MAX_MOVE_POWER, BASE_CRIT_RATE, 
                    SPEED_PRIORITY, TARGET) VALUES (@id, @name, @type, @cat, @pp, @power, @acc, @ef, @secEf, @efRate, @maxM, @maxMPow, @baseCR, @spdPri, @tar)";

                SQLQuery(sqlMoves, moveParams);
            }

            //Pokemon & the connecting tables
            List<Pokemon> pokemons = DataManager.Instance.Pokemon.GetPokemonList();

            int pokemonTypesId = 1;
            int pokemonAbilitiesId = 1;
            int pokemonMovesId = 1;

            for (int i = 0; i < pokemons.Count; i++)
            {
                int pokemonId = i + 1;

                List<SqlParameter> pokemonParams = new List<SqlParameter>();
                pokemonParams.Add(new SqlParameter("@id", pokemonId));
                pokemonParams.Add(new SqlParameter("@name", pokemons[i].name));
                pokemonParams.Add(new SqlParameter("@natDexNo", pokemons[i].nationalDexNo));
                pokemonParams.Add(new SqlParameter("@galarDexNo", pokemons[i].galarDexNo));
                pokemonParams.Add(new SqlParameter("@hp", pokemons[i].hp));
                pokemonParams.Add(new SqlParameter("@atk", pokemons[i].atk));
                pokemonParams.Add(new SqlParameter("@def", pokemons[i].def));
                pokemonParams.Add(new SqlParameter("@spAtk", pokemons[i].spAtk));
                pokemonParams.Add(new SqlParameter("@spDef", pokemons[i].spDef));
                pokemonParams.Add(new SqlParameter("@spd", pokemons[i].spd));
                pokemonParams.Add(new SqlParameter("@desc", pokemons[i].description));

                string sqlPokemon = @"INSERT INTO POKEMON (POKEMON_ID, NAME, NATIONAL_DEX_NO, GALAR_DEX_NO, HP, ATK, DEF, SP_ATK, SP_DEF, SPD, DESCRIPTION) VALUES (
                    @id, @name, @natDexNo, @galarDexNo, @hp, @atk, @def, @spAtk, @spDef, @spd, @desc)";

                SQLQuery(sqlPokemon, pokemonParams);

                for (int j = 0; j < pokemons[i].types.Count; j++)
                {
                    List<SqlParameter> typeFindParams = new List<SqlParameter>();
                    typeFindParams.Add(new SqlParameter("@type", pokemons[i].types[j]));

                    int typeId = int.Parse(SQLQueryGetSingleResult("SELECT TYPES_ID FROM TYPES WHERE TYPE = @type", typeFindParams).ToString());

                    List<SqlParameter> pokemonTypeParams = new List<SqlParameter>();
                    pokemonTypeParams.Add(new SqlParameter("@pokeTypesId", pokemonTypesId++));
                    pokemonTypeParams.Add(new SqlParameter("@pokeId", pokemonId));
                    pokemonTypeParams.Add(new SqlParameter("@typesId", typeId));

                    string sqlPokemonTypes = @"INSERT INTO POKEMON_TYPES (POKEMON_TYPES_ID, POKEMON_ID, TYPES_ID) VALUES (
                        @pokeTypesId, @pokeId, @typesId)";

                    SQLQuery(sqlPokemonTypes, pokemonTypeParams);
                }

                for (int k = 0; k < pokemons[i].abilities.Count; k++)
                {
                    List<SqlParameter> abilityFindParams = new List<SqlParameter>();
                    abilityFindParams.Add(new SqlParameter("@name", pokemons[i].abilities[k]));

                    int abilityId = int.Parse(SQLQueryGetSingleResult("SELECT ABILITIES_ID FROM ABILITIES WHERE NAME = @name", abilityFindParams).ToString());

                    List<SqlParameter> pokemonAbilityParams = new List<SqlParameter>();
                    pokemonAbilityParams.Add(new SqlParameter("@pokeAbilId", pokemonAbilitiesId++));
                    pokemonAbilityParams.Add(new SqlParameter("@pokeId", pokemonId));
                    pokemonAbilityParams.Add(new SqlParameter("@abilId", abilityId));

                    string sqlPokemonAbilities = @"INSERT INTO POKEMON_ABILITIES (POKEMON_ABILITIES_ID, POKEMON_ID, ABILITIES_ID) VALUES (
                        @pokeAbilId, @pokeId, @abilId)";

                    SQLQuery(sqlPokemonAbilities, pokemonAbilityParams);
                }

                for (int l = 0; l < pokemons[i].moves.Count; l++)
                {
                    List<SqlParameter> moveFindParams = new List<SqlParameter>();
                    moveFindParams.Add(new SqlParameter("@name", pokemons[i].moves[l]));

                    int moveId = int.Parse(SQLQueryGetSingleResult("SELECT MOVES_ID FROM MOVES WHERE NAME = @name", moveFindParams).ToString());

                    List<SqlParameter> pokemonMoveParams = new List<SqlParameter>();
                    pokemonMoveParams.Add(new SqlParameter("@pokeMoveId", pokemonMovesId++));
                    pokemonMoveParams.Add(new SqlParameter("@pokeId", pokemonId));
                    pokemonMoveParams.Add(new SqlParameter("@moveId", moveId));

                    string sqlPokemonMoves = @"INSERT INTO POKEMON_MOVES (POKEMON_MOVES_ID, POKEMON_ID, MOVES_ID) VALUES (
                        @pokeMoveId, @pokeId, @moveId)";

                    SQLQuery(sqlPokemonMoves, pokemonMoveParams);
                }
            }
        }

        public Pokemon GetPokemonFromDB(string name)
        {
            Pokemon pokemon = new Pokemon();
            int pokemonPK = -1;

            try
            {
                sqlCon.Open();

                SqlCommand command = new SqlCommand("SELECT * FROM POKEMON WHERE NAME = @name", sqlCon);
                command.Parameters.Add(new SqlParameter("@name", name));

                using (SqlDataReader dr = command.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        pokemonPK = (int)dr["POKEMON_ID"];
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

                //sqlCon.Close();
                //sqlCon.Open();

                //Types
                List<string> types = new List<string>();
                SqlCommand typesCommand = new SqlCommand(@"SELECT TYPES.TYPE FROM POKEMON_TYPES INNER JOIN TYPES ON POKEMON_TYPES.TYPES_ID = TYPES.TYPES_ID 
                    WHERE POKEMON_TYPES.POKEMON_ID = @pokemonPk", sqlCon);
                typesCommand.Parameters.Add(new SqlParameter("@pokemonPk", pokemonPK));

                using (SqlDataReader drTypes = typesCommand.ExecuteReader())
                {
                    while (drTypes.Read())
                    {
                        types.Add((string)drTypes["TYPE"]);
                    }
                }

                pokemon.types = types;

                //Abilities
                List<string> abilities = new List<string>();
                SqlCommand abilitiesCommand = new SqlCommand(@"SELECT ABILITIES.NAME FROM POKEMON_ABILITIES INNER JOIN ABILITIES ON POKEMON_ABILITIES.ABILITIES_ID = ABILITIES.ABILITIES_ID
                    WHERE POKEMON_ABILITIES.POKEMON_ID = @pokemonPk", sqlCon);
                abilitiesCommand.Parameters.Add(new SqlParameter("@pokemonPk", pokemonPK));

                using (SqlDataReader drAbilities = abilitiesCommand.ExecuteReader())
                {
                    while (drAbilities.Read())
                    {
                        abilities.Add((string)drAbilities["NAME"]);
                    }
                }

                pokemon.abilities = abilities;

                //Moves
                List<string> moves = new List<string>();
                SqlCommand movesCommand = new SqlCommand(@"SELECT MOVES.NAME FROM POKEMON_MOVES INNER JOIN MOVES ON POKEMON_MOVES.MOVES_ID = MOVES.MOVES_ID
                    WHERE POKEMON_MOVES.POKEMON_ID = @pokemonPK", sqlCon);
                movesCommand.Parameters.Add(new SqlParameter("@pokemonPK", pokemonPK));

                using (SqlDataReader drMoves = movesCommand.ExecuteReader())
                {
                    while (drMoves.Read())
                    {
                        moves.Add((string)drMoves["NAME"]);
                    }
                }

                pokemon.moves = moves;

                sqlCon.Close();
            }
            catch (Exception ex)
            {
                Debugger.Break();
            }

            return pokemon;
        }

        public static DbData Instance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new DbData();
                    }

                    return instance;
                }
            }
        }
    }
}
