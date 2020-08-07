using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;
using System.Data.SqlClient;
using System.Data;
using System.IO;
using System.Diagnostics;
using System.Text.Json;

namespace WebPokeHelper
{
    public class Startup
    {
        public DataManager PokeData = DataManager.Instance;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

            //Testing area
            //GetGen8Images();
            //Task task = GetGen8Images();

            //DbData.Instance.CreateTablesForDB();
            //DbData.Instance.LoadAllJSONDataToDB();

            //Pokemon pokemon = DbData.Instance.GetPokemonFromDB("Bulbasaur");

            //foreach (Pokemon pokemon in PokeData.Pokemon.GetPokemonList())
            //{
            //    for (int i = 0; i < pokemon.abilities.Count; i++)
            //    {
            //        int colonIndex = pokemon.abilities[i].IndexOf(':');

            //        if (colonIndex == -1)
            //            Debugger.Break();

            //        pokemon.abilities[i] = pokemon.abilities[i].Substring(0, colonIndex);
            //    }
            //}

            //using (StreamWriter sw = new StreamWriter(Directory.GetCurrentDirectory() + @"/Resources/Pokemon_NEW.json"))
            //{
            //    sw.Write(JsonSerializer.Serialize(PokeData.Pokemon.GetPokemonList()));
            //}

            //Debugger.Break();
        }

        //public async Task LoadPokemonToNewJSON()
        //{
        //    //Loading pokemon data from json file test
        //    using (StreamReader reader = new StreamReader(Directory.GetCurrentDirectory() + @"/Resources/pokemon_old.json"))
        //    {
        //        string json = reader.ReadToEnd();

        //        List<PokemonJson> pokemonJsonList = new List<PokemonJson>();

        //        pokemonJsonList = JsonSerializer.Deserialize<List<PokemonJson>>(json);

        //        foreach (PokemonJson pokemonJson in pokemonJsonList)
        //            pokemons.Add(new Pokemon(pokemonJson));
        //    }

        //    Debugger.Break();

        //    using (StreamWriter writer = new StreamWriter(Directory.GetCurrentDirectory() + @"/Resources/Pokemon.json"))
        //    {
        //        writer.Write(JsonSerializer.Serialize(pokemons));
        //    }
        //    Debugger.Break();
        //}

        //public void MergeAllMovesTogetherJSON()
        //{
        //    List<PokemonNew> newPokemonList = new List<PokemonNew>();

        //    foreach (Pokemon pokemon in PokeData.Pokemon.GetFullPokemonList())
        //    {
        //        PokemonNew newPokemon = new PokemonNew()
        //        {
        //            name = pokemon.name,
        //            nationalDexNo = pokemon.id,
        //            baseStatHp = pokemon.baseStats[0],
        //            baseStatAtk = pokemon.baseStats[1],
        //            baseStatDef = pokemon.baseStats[2],
        //            baseStatSpAtk = pokemon.baseStats[3],
        //            baseStatSpDef = pokemon.baseStats[4],
        //            baseStatSpd = pokemon.baseStats[5],
        //            description = pokemon.description,
        //            abilities = pokemon.abilities,
        //            types = pokemon.types
        //        };

        //        int result;
        //        newPokemon.galarDexNo = Int32.TryParse(pokemon.galarDexNo, out result) ? result : -1;

        //        //Convert tms & trs to their move names
        //        List<string> namedTms = new List<string>();

        //        foreach (int tm in pokemon.tms)
        //        {
        //            foreach (Move move in PokeData.Moves.GetMoveList())
        //            {
        //                if ("TM" + tm == move.record)
        //                {
        //                    namedTms.Add(move.name);
        //                }
        //            }
        //        }

        //        List<string> namedTrs = new List<string>();

        //        foreach (int tr in pokemon.trs)
        //        {
        //            foreach (Move move in PokeData.Moves.GetMoveList())
        //            {
        //                if ("TR" + tr == move.record)
        //                {
        //                    namedTrs.Add(move.name);
        //                }
        //            }
        //        }

        //        List<string> fullMoveList = new List<string>();
        //        fullMoveList.AddRange(pokemon.levelUpMoves);
        //        fullMoveList.AddRange(namedTms);
        //        fullMoveList.AddRange(namedTrs);
        //        fullMoveList.AddRange(pokemon.eggMoves);

        //        newPokemon.moves = fullMoveList;

        //        newPokemonList.Add(newPokemon);
        //    }

        //    using (StreamWriter sw = new StreamWriter(Directory.GetCurrentDirectory() + @"/Resources/Pokemon_FullyFormatted.json"))
        //    {
        //        sw.Write(JsonSerializer.Serialize(newPokemonList));
        //    }
        //}

        public async Task GetGen8Images()
        {
            Task<string> htmlTask = HTTPGet.GetAsync("https://www.serebii.net/pokedex-swsh/");
            await htmlTask;

            string html = htmlTask.Result;
            html = GetDataFromHtml(html, "<form name=\"nav8\">", 0, "</form>", 0);
            html = html;

            string alola = @"
                '/pokedex-swsh/rowlet/'>722*
                '/pokedex-swsh/dartrix/'>723*
                '/pokedex-swsh/decidueye/'>724*
                '/pokedex-swsh/litten/'>725*
                '/pokedex-swsh/torracat/'>726*
                '/pokedex-swsh/incineroar/'>727*
                '/pokedex-swsh/popplio/'>728*
                '/pokedex-swsh/brionne/'>729*
                '/pokedex-swsh/primarina/'>730*
                '/pokedex-swsh/grubbin/'>736*
                '/pokedex-swsh/charjabug/'>737*
                '/pokedex-swsh/vikavolt/'>738*
                '/pokedex-swsh/cutiefly/'>742*
                '/pokedex-swsh/ribombee/'>743*
                '/pokedex-swsh/rockruff/'>744*
                '/pokedex-swsh/lycanroc/'>745*
                '/pokedex-swsh/wishiwashi/'>746*
                '/pokedex-swsh/mareanie/'>747*
                '/pokedex-swsh/toxapex/'>748*
                '/pokedex-swsh/mudbray/'>749*
                '/pokedex-swsh/mudsdale/'>750*
                '/pokedex-swsh/dewpider/'>751*
                '/pokedex-swsh/araquanid/'>752*
                '/pokedex-swsh/fomantis/'>753*
                '/pokedex-swsh/lurantis/'>754*
                '/pokedex-swsh/morelull/'>755*
                '/pokedex-swsh/shiinotic/'>756*
                '/pokedex-swsh/salandit/'>757*
                '/pokedex-swsh/salazzle/'>758*
                '/pokedex-swsh/stufful/'>759*
                '/pokedex-swsh/bewear/'>760*
                '/pokedex-swsh/bounsweet/'>761*
                '/pokedex-swsh/steenee/'>762*
                '/pokedex-swsh/tsareena/'>763*
                '/pokedex-swsh/comfey/'>764*
                '/pokedex-swsh/oranguru/'>765*
                '/pokedex-swsh/passimian/'>766*
                '/pokedex-swsh/wimpod/'>767*
                '/pokedex-swsh/golisopod/'>768*
                '/pokedex-swsh/sandygast/'>769*
                '/pokedex-swsh/palossand/'>770*
                '/pokedex-swsh/pyukumuku/'>771*
                '/pokedex-swsh/type:null/'>772*
                '/pokedex-swsh/silvally/'>773*
                '/pokedex-swsh/turtonator/'>776*
                '/pokedex-swsh/togedemaru/'>777*
                '/pokedex-swsh/mimikyu/'>778*
                '/pokedex-swsh/drampa/'>780*
                '/pokedex-swsh/dhelmise/'>781*
                '/pokedex-swsh/jangmo-o/'>782*
                '/pokedex-swsh/hakamo-o/'>783*
                '/pokedex-swsh/kommo-o/'>784*
                '/pokedex-swsh/cosmog/'>789*
                '/pokedex-swsh/cosmoem/'>790*
                '/pokedex-swsh/solgaleo/'>791*
                '/pokedex-swsh/lunala/'>792*
                '/pokedex-swsh/necrozma/'>800*
                '/pokedex-swsh/magearna/'>801*
                '/pokedex-swsh/marshadow/'>802*
                '/pokedex-swsh/zeraora/'>807*
                '/pokedex-swsh/meltan/'>808*
                '/pokedex-swsh/melmetal/'>809
                ";

            string galar = @"
                '/pokedex-swsh/grookey/'>810*
                '/pokedex-swsh/thwackey/'>811*
                '/pokedex-swsh/rillaboom/'>812*
                '/pokedex-swsh/scorbunny/'>813*
                '/pokedex-swsh/raboot/'>814*
                '/pokedex-swsh/cinderace/'>815*
                '/pokedex-swsh/sobble/'>816*
                '/pokedex-swsh/drizzile/'>817*
                '/pokedex-swsh/inteleon/'>818*
                '/pokedex-swsh/skwovet/'>819*
                '/pokedex-swsh/greedent/'>820*
                '/pokedex-swsh/rookidee/'>821*
                '/pokedex-swsh/corvisquire/'>822*
                '/pokedex-swsh/corviknight/'>823*
                '/pokedex-swsh/blipbug/'>824*
                '/pokedex-swsh/dottler/'>825*
                '/pokedex-swsh/orbeetle/'>826*
                '/pokedex-swsh/nickit/'>827*
                '/pokedex-swsh/thievul/'>828*
                '/pokedex-swsh/gossifleur/'>829*
                '/pokedex-swsh/eldegoss/'>830*
                '/pokedex-swsh/wooloo/'>831*
                '/pokedex-swsh/dubwool/'>832*
                '/pokedex-swsh/chewtle/'>833*
                '/pokedex-swsh/drednaw/'>834*
                '/pokedex-swsh/yamper/'>835*
                '/pokedex-swsh/boltund/'>836*
                '/pokedex-swsh/rolycoly/'>837*
                '/pokedex-swsh/carkol/'>838*
                '/pokedex-swsh/coalossal/'>839*
                '/pokedex-swsh/applin/'>840*
                '/pokedex-swsh/flapple/'>841*
                '/pokedex-swsh/appletun/'>842*
                '/pokedex-swsh/silicobra/'>843*
                '/pokedex-swsh/sandaconda/'>844*
                '/pokedex-swsh/cramorant/'>845*
                '/pokedex-swsh/arrokuda/'>846*
                '/pokedex-swsh/barraskewda/'>847*
                '/pokedex-swsh/toxel/'>848*
                '/pokedex-swsh/toxtricity/'>849*
                '/pokedex-swsh/sizzlipede/'>850*
                '/pokedex-swsh/centiskorch/'>851*
                '/pokedex-swsh/clobbopus/'>852*
                '/pokedex-swsh/grapploct/'>853*
                '/pokedex-swsh/sinistea/'>854*
                '/pokedex-swsh/polteageist/'>855*
                '/pokedex-swsh/hatenna/'>856*
                '/pokedex-swsh/hattrem/'>857*
                '/pokedex-swsh/hatterene/'>858*
                '/pokedex-swsh/impidimp/'>859*
                '/pokedex-swsh/morgrem/'>860*
                '/pokedex-swsh/grimmsnarl/'>861*
                '/pokedex-swsh/obstagoon/'>862*
                '/pokedex-swsh/perrserker/'>863*
                '/pokedex-swsh/cursola/'>864*
                '/pokedex-swsh/sirfetch'd/'>865*
                '/pokedex-swsh/mr.rime/'>866*
                '/pokedex-swsh/runerigus/'>867*
                '/pokedex-swsh/milcery/'>868*
                '/pokedex-swsh/alcremie/'>869*
                '/pokedex-swsh/falinks/'>870*
                '/pokedex-swsh/pincurchin/'>871*
                '/pokedex-swsh/snom/'>872*
                '/pokedex-swsh/frosmoth/'>873*
                '/pokedex-swsh/stonjourner/'>874*
                '/pokedex-swsh/eiscue/'>875*
                '/pokedex-swsh/indeedee/'>876*
                '/pokedex-swsh/morpeko/'>877*
                '/pokedex-swsh/cufant/'>878*
                '/pokedex-swsh/copperajah/'>879*
                '/pokedex-swsh/dracozolt/'>880*
                '/pokedex-swsh/arctozolt/'>881*
                '/pokedex-swsh/dracovish/'>882*
                '/pokedex-swsh/arctovish/'>883*
                '/pokedex-swsh/duraludon/'>884*
                '/pokedex-swsh/dreepy/'>885*
                '/pokedex-swsh/drakloak/'>886*
                '/pokedex-swsh/dragapult/'>887*
                '/pokedex-swsh/zacian/'>888*
                '/pokedex-swsh/zamazenta/'>889*
                '/pokedex-swsh/eternatus/'>890*
                '/pokedex-swsh/kubfu/'>891*
                '/pokedex-swsh/urshifu/'>892*
                '/pokedex-swsh/zarude/'>893
                ";

            List<string> urlsWithId = new List<string>();
            urlsWithId.AddRange(alola.Split("*"));
            urlsWithId.AddRange(galar.Split("*"));

            foreach(string urlWithId in urlsWithId)
            {
                string trimmedStr = urlWithId.Trim();
                string url = "https://www.serebii.net/swordshield/pokemon/" + trimmedStr.Substring(trimmedStr.Length - 3, 3) + ".png";

                HTTPGet.GetImage(url, "C:\\Users\\James\\Desktop\\New Pokemon Images\\" + trimmedStr.Substring(trimmedStr.Length - 3, 3) + ".png");
            }
        }

        public async Task LoadMovesToJSON()
        {
            //Get all moves into a json file
            //Get a list of all move links
            List<string> moveUrls = new List<string>();

            //Get raw html
            Task<string> physicalHtmlTask = HTTPGet.GetAsync("https://www.serebii.net/attackdex-swsh/physical.shtml");
            Task<string> specialHtmlTask = HTTPGet.GetAsync("https://www.serebii.net/attackdex-swsh/special.shtml");
            Task<string> otherHtmlTask = HTTPGet.GetAsync("https://www.serebii.net/attackdex-swsh/other.shtml");
            await physicalHtmlTask;
            await specialHtmlTask;
            await otherHtmlTask;

            string physicalHtml = physicalHtmlTask.Result;
            string specialHtml = specialHtmlTask.Result;
            string otherHtml = otherHtmlTask.Result;

            List<Move> moves = new List<Move>();

            //moveUrls.AddRange(GetMovesFromHTML(physicalHtml));
            //moveUrls.AddRange(GetMovesFromHTML(specialHtml));
            moveUrls.AddRange(GetMovesFromHTML(otherHtml));
            moveUrls.Sort();

            foreach (string moveUrl in moveUrls)
            {
                //TEST
                //Task<string> moveHtmlTask = HTTPGet.GetAsync("https://www.serebii.net/attackdex-swsh/maxairstream.shtml");
                Task<string> moveHtmlTask = HTTPGet.GetAsync("https://www.serebii.net" + moveUrl);
                await moveHtmlTask;

                string moveHtml = moveHtmlTask.Result;
                string recordStr = "";

                try
                {
                    //Get the TM or TR check early
                    if (moveHtml.Contains("By Technical Machine ("))
                        recordStr += GetDataFromHtml(moveHtml, "By Technical Machine (", 0, ")", 0);
                    if (moveHtml.Contains("By Technical Record ("))
                        recordStr += GetDataFromHtml(moveHtml, "By Technical Record (", 0, ")", 0);

                    if (moveHtml.IndexOf("<table class=" + '"' + "dextab" + '"' + ">") == -1)
                    {
                        moveHtml = RemoveDataFromHtml(moveHtml, "<table  class=" + '"' + "dextab" + '"' + ">", -23, "</table>", 7);
                    }
                    else
                    {
                        moveHtml = RemoveDataFromHtml(moveHtml, "<table class=" + '"' + "dextab" + '"' + ">", -22, "</table>", 7);
                    }

                    moveHtml = GetDataFromHtml(moveHtml, "<b>Attack Name</b>", 0, "Physical Contact", 0);
                } 
                catch (Exception ex)
                {
                    Debugger.Break();
                }
                
                
                try
                {
                    Move move;
                    string record = recordStr;

                    string name = GetDataFromHtml(moveHtml, "English</b>: </td><td>", 0, "</td></tr>", 0);

                    string type = GetDataFromHtml(moveHtml, "<a href=\"/attackdex-swsh/", 0, ".shtml", 0);
                    moveHtml = RemoveDataFromHtml(moveHtml, "<a href=\"/attackdex-swsh/", -25, ".shtml", 5);

                    string category = GetDataFromHtml(moveHtml, "<a href=\"/attackdex-swsh/", 0, ".shtml", 0);
                    moveHtml = RemoveDataFromHtml(moveHtml, "</td>", -5, "Accuracy", 0);

                    int powerPoints = 0;
                    int.TryParse(GetDataFromHtml(moveHtml, "<td class=\"cen\">", 0, "</td>", 0), out powerPoints);
                    moveHtml = RemoveDataFromHtml(moveHtml, "<td class=\"cen\">", -16, "</td>", 5);

                    int basePower = 0;
                    int.TryParse(GetDataFromHtml(moveHtml, "<td class=\"cen\">", 0, "</td>", 0), out basePower);
                    moveHtml = RemoveDataFromHtml(moveHtml, "<td class=\"cen\">", -16, "</td>", 5);

                    int accuracy = 0;
                    int.TryParse(GetDataFromHtml(moveHtml, "<td class=\"cen\">", 0, "</td>", 0), out accuracy);
                    moveHtml = RemoveDataFromHtml(moveHtml, "<td class=\"cen\">", -16, "</td>", 5);

                    string effect = GetDataFromHtml(moveHtml, "<td colspan=\"3\" class=\"fooinfo\">", 4, "</td>", -1);

                    string secondaryEffect = GetDataFromHtml(moveHtml, "<td colspan=\"2\" class=\"fooinfo\">", 4, "</td>", -1);

                    int effectRate = 0;
                    int.TryParse(GetDataFromHtml(moveHtml, "<td class=\"cen\">", 4, "</td>", -3), out effectRate);
                    moveHtml = RemoveDataFromHtml(moveHtml, "<td class=\"cen\">", -16, "</td>", 5);

                    string maxMove = "";
                    int maxMovePower = 1;

                    if (!name.Contains("G-Max"))
                    {
                        if (!(name[0] == 'M' && name[1] == 'a' && name[2] == 'x'))
                        {
                            if (category != "other")
                            {
                                maxMove = GetDataFromHtml(moveHtml, ".shtml", 5, "</u>", 0);

                                maxMovePower = 0;
                                int.TryParse(GetDataFromHtml(moveHtml, "<td class=\"cen\">", 0, "</td>", 0), out maxMovePower);
                                moveHtml = RemoveDataFromHtml(moveHtml, "<td class=\"cen\">", -16, "</td>", 5);
                            }
                        }
                    }

                    float baseCritRate = 0;
                    float.TryParse(GetDataFromHtml(moveHtml, "<td class=\"cen\">", 4, "</td>", -5), out baseCritRate);
                    moveHtml = RemoveDataFromHtml(moveHtml, "<td class=\"cen\">", -16, "</td>", 5);

                    int speedPriority = 0;
                    int.TryParse(GetDataFromHtml(moveHtml, "<td class=\"cen\">", 0, "</td>", 0), out speedPriority);
                    moveHtml = RemoveDataFromHtml(moveHtml, "<td class=\"cen\">", -16, "</td>", 5);

                    string target = GetDataFromHtml(moveHtml, "<td class=\"cen\">", 4, "</td>", 0);

                    move = new Move(name, type, category, powerPoints, basePower, accuracy, effect, secondaryEffect, effectRate, maxMove, maxMovePower, baseCritRate, speedPriority, target, record);

                    moves.Add(move);
                }
                catch (Exception ex)
                {
                    Debugger.Break();
                }
            }

            string json = JsonSerializer.Serialize(moves);

            using (StreamWriter sw = new StreamWriter(Directory.GetCurrentDirectory() + @"/Resources/Other Moves.json"))
            {
                sw.Write(json);
            }

            Debugger.Break();
        }

        public void ConvertAllMoveFilesToOne()
        {
            List<Move> moves = new List<Move>();

            using (StreamReader sr = new StreamReader(Directory.GetCurrentDirectory() + @"/Resources/Physical Moves.json"))
            {
                moves.AddRange(JsonSerializer.Deserialize<List<Move>>(sr.ReadToEnd()));
            }

            using (StreamReader sr = new StreamReader(Directory.GetCurrentDirectory() + @"/Resources/Special Moves.json"))
            {
                moves.AddRange(JsonSerializer.Deserialize<List<Move>>(sr.ReadToEnd()));
            }

            using (StreamReader sr = new StreamReader(Directory.GetCurrentDirectory() + @"/Resources/Other Moves.json"))
            {
                moves.AddRange(JsonSerializer.Deserialize<List<Move>>(sr.ReadToEnd()));
            }

            var orderedMoves = moves.OrderBy(m => m.name);

            string json = JsonSerializer.Serialize(orderedMoves);

            using (StreamWriter sw = new StreamWriter(Directory.GetCurrentDirectory() + @"/Resources/Moves.json"))
            {
                sw.Write(json);
            }

            Debugger.Break();
        }

        public async Task LoadAbilitiesToJSON()
        {
            Task<string> abilitiesHtmlTask = HTTPGet.GetAsync("https://bulbapedia.bulbagarden.net/wiki/Ability");
            await abilitiesHtmlTask;

            string html = abilitiesHtmlTask.Result;

            string abilitiesHtml = GetDataFromHtml(html, "List of Abilities</span></h2>", 0, "</table>", 0);

            List<Ability> abilityList = new List<Ability>();

            while (abilitiesHtml.IndexOf(" (Ability)\">") != -1)
            {
                Ability ability = new Ability();

                ability.name = GetDataFromHtml(abilitiesHtml, "(Ability)\">", 0, "</a>", 0);
                abilitiesHtml = RemoveDataFromHtml(abilitiesHtml, " (Ability)\">", -12, "</a>", 5);

                ability.description = GetDataFromHtml(abilitiesHtml, "<td class=\"l\"> ", 0, "</td>", -1);
                abilitiesHtml = RemoveDataFromHtml(abilitiesHtml, "<td class=\"l\"> ", -15, "</td>", 5);

                abilityList.Add(ability);

                //if (ability.name == "Zen Mode")
                //    Debugger.Break();
            }

            using (StreamWriter sw = new StreamWriter(Directory.GetCurrentDirectory() + @"/Resources/Abilities.json"))
            {
                sw.Write(JsonSerializer.Serialize(abilityList));
            }

            Debugger.Break();
        }

        public void AddAbilityDataToPokemonJSON()
        {
            foreach (Pokemon pokemon in PokeData.Pokemon.GetPokemonList())
            {
                pokemon.abilities = pokemon.abilities.Distinct().ToList();

                for (int i = 0; i < pokemon.abilities.Count; i++)
                {
                    pokemon.abilities[i] += ": " + PokeData.Abilities.GetAbility(pokemon.abilities[i]).description;
                }
            }

            using (StreamWriter sw = new StreamWriter(Directory.GetCurrentDirectory() + @"/Resources/pokemon_experimental.json"))
            {
                sw.Write(JsonSerializer.Serialize(PokeData.Pokemon.GetPokemonList()));
            }
        }

        private List<string> GetMovesFromHTML(string rawHtml)
        {
            List<string> result = new List<string>();

            rawHtml = GetDataFromHtml(rawHtml, "<main>", 0, "</main>", 0);
            rawHtml = GetDataFromHtml(rawHtml, "<table class=" + '"' + "dextable" + '"' + ">", 0, "</table>", 0);

            while (rawHtml.IndexOf("<a href=" + '"') != -1)
            {
                result.Add(GetDataFromHtml(rawHtml, "<a href=" + '"', 0, ">", -1));
                rawHtml = RemoveDataFromHtml(rawHtml, "<a href=" + '"', -9, ">", 0);
            }

            return result;
        }

        private string GetDataFromHtml(
            string rawHtml,
            string startIdentifier,
            int startOffset,
            string endIdentifier,
            int endOffset)
        {
            int startIndex = rawHtml.IndexOf(startIdentifier) + startIdentifier.Length + startOffset;
            int endIndex = rawHtml.IndexOf(endIdentifier, startIndex) + endOffset;
            string result = rawHtml.Substring(startIndex, endIndex - startIndex);

            return result;
        }

        private string RemoveDataFromHtml(
            string rawHtml,
            string startIdentifier,
            int startOffset,
            string endIdentifier,
            int endOffset)
        {
            int startIndex = rawHtml.IndexOf(startIdentifier) + startIdentifier.Length + startOffset;
            int endIndex = rawHtml.IndexOf(endIdentifier, startIndex) + endOffset;

            rawHtml = rawHtml.Remove(startIndex, endIndex - startIndex);

            return rawHtml;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages();

            //services.AddDbContext<DefaultDatabaseString>(options =>
            //    options.UseSqlServer(Configuration.GetConnectionString("DefaultDatabaseString")));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                //endpoints.MapRazorPages();

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}