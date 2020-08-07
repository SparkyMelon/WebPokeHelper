var pokemonInBattle = [{ code: "00", data: null }, { code: "10", data: null }, { code: "01", data: null }, { code: "11", data: null }];
//var attackSimMove = null;
//var attackSimMoveId = null;
//var attackSimDefenderId = null;

$(document).ready(function () {
    InitializeEvents();
});

function InitializeEvents() {
    //Dropdown functionality
    $("#pokemonListText_00, #pokemonListText_01, #pokemonListText_10, #pokemonListText_11").on("keyup focus", function () {
        var dropdownCode = $(this).prop("id").split("_")[1];
        $("#pokemonListDropdown_" + dropdownCode + " > div > .dropdown-item").each(function () {
            var currDropdownPokemon = $(this).text().toLowerCase();
            var dropdownCode = $(this).parent().parent().prop("id").split("_")[1];
            var searchStr = $("#pokemonListText_" + dropdownCode).val().toLowerCase();

            if (!currDropdownPokemon.includes(searchStr)) {
                $(this).hide();
            }
            else {
                $(this).show();
            }
        });
    });

    //Pokemon selected
    $("#pokemonListDropdown_00 > div > .dropdown-item, #pokemonListDropdown_01 > div > .dropdown-item, #pokemonListDropdown_10 > div > .dropdown-item, #pokemonListDropdown_11 > div > .dropdown-item").click(function () {
        var pokemonChosen = $(this).html();
        var code = $(this).parent().parent().prop("id").split("_")[1];

        //$("#pokemonListText_" + code).val(pokemonChosen);
        $("#pokemonListText_" + code).val("");

        //Find the pokemon data from the model with the chosen pokemon name
        var pokemonData = null;

        for (var i = 0; i < model.length; i++) {
            if (model[i].name == pokemonChosen) {
                pokemonData = model[i];
                break;
            }
        }

        if (pokemonData == null) {
            alert("Pokemon not found");
        }
        else {
            SetPokemonInBattle(code, pokemonData);

            $("#pokemonName_" + code).html(pokemonData.name);

            //Image
            var imageTemplate = $("#pokemonImgTemplate").html();
            var imageHtml = imageTemplate.replace("[ID]", pokemonData.nationalDexNo);
            $("#pokemonImage_" + code).html(imageHtml);

            //Types
            var typeTemplate = $("#pokemonTypeTemplate").html();
            var typesHtml = "";
            for (var i = 0; i < pokemonData.types.length; i++) {
                var template = typeTemplate;
                template = template.replace("[TYPE]", pokemonData.types[i].name);
                typesHtml += template;
            }
            $("#pokemonTypes_" + code).html(typesHtml);

            //Stats
            //$("#pokemonStatHP_" + code).html(pokemonData.hp);
            //$("#pokemonStatATK_" + code).html(pokemonData.atk);
            //$("#pokemonStatDEF_" + code).html(pokemonData.def);
            //$("#pokemonStatSPATK_" + code).html(pokemonData.spAtk);
            //$("#pokemonStatSPDEF_" + code).html(pokemonData.spDef);
            //$("#pokemonStatSPD_" + code).html(pokemonData.spd);

            //Abilities
            var abilityTemplate = $("#pokemonAbilityTemplate").html();
            var abilitiesHtml = "";
            for (var i = 0; i < pokemonData.abilities.length; i++) {
                var template = abilityTemplate;
                template = template.replace("[ABILITY NAME]", pokemonData.abilities[i].name);
                template = template.replace("[ABILITY DESC]", pokemonData.abilities[i].description);
                abilitiesHtml += template;
            }
            $("#pokemonAbilities_" + code).html(abilitiesHtml);

            //Set the border colour
            $("#pokemonAbilities_" + code).find(".borderTeamColour_xy").removeClass("borderTeamColour_xy").addClass("borderTeamColour_" + code);

            //Moves
            var moveTemplate = $("#pokemonMoveTemplate").html();
            var movesHtml = "";
            for (var i = 0; i < pokemonData.moves.length; i++) {
                var template = moveTemplate;
                template = template.replace("[NAME]", pokemonData.moves[i].name);
                template = template.replace("[TYPE]", pokemonData.moves[i].type);
                template = template.replace("[CATEGORY]", pokemonData.moves[i].category);
                template = template.replace("[PP]", pokemonData.moves[i].powerPoints);
                template = template.replace("[POWER]", pokemonData.moves[i].basePower);
                template = template.replace("[ACCURACY]", pokemonData.moves[i].accuracy);
                template = template.replace("[EFFECT]", pokemonData.moves[i].effect + " - " + pokemonData.moves[i].secondaryEffect);
                template = template.replace("[EFFECT RATE]", pokemonData.moves[i].effectRate);
                template = template.replace("[CRIT RATE]", pokemonData.moves[i].baseCritRate);
                template = template.replace("[PRIORITY]", pokemonData.moves[i].speedPriority);
                template = template.replace("[TARGET]", pokemonData.moves[i].target);
                template = template.replace("[MAX MOVE]", pokemonData.moves[i].maxMove);
                template = template.replace("[MAX MOVE POWER]", pokemonData.moves[i].maxMovePower);
                movesHtml += template;
            }
            $("#pokemonMoves_" + code).html(movesHtml);

            //Set the border colour
            $("#pokemonMoves_" + code).find(".borderTeamColour_xy").removeClass("borderTeamColour_xy").addClass("borderTeamColour_" + code);

            //Hide all extra move details
            $("#pokemon_" + code).find(".pokemonMoveCaret").parent().parent().parent().parent().find(".moveExtrasContainer").hide();
            $("#pokemon_" + code).find(".pokemonMoveCaret").click(function () {
                var $container = $(this).parent().parent().parent().parent().find(".moveExtrasContainer");
                var originalContainerH = $container.height();
                CaretToggle($(this), $container);
            });

            //Adjust the moves container height to fill the container
            AdjustHeightOnMovesContainer(code);

            UpdateExtraMoveMasterButton();

            UpdateModifiedStats(code);

            //Expand all containers
            if ($("#pokemonStatsCaret_" + code).prop("src").includes("caret-bottom.svg")) $("#pokemonStatsCaret_" + code).click();
            if ($("#pokemonAbilitiesCaret_" + code).prop("src").includes("caret-bottom.svg")) $("#pokemonAbilitiesCaret_" + code).click();
            if ($("#pokemonMovesCaret_" + code).prop("src").includes("caret-bottom.svg")) $("#pokemonMovesCaret_" + code).click();
        }
    });

    //Caret events
    $("#pokemonStats_00, #pokemonStats_01, #pokemonStats_10, #pokemonStats_11").hide();
    $("#pokemonStatsCaret_00, #pokemonStatsCaret_01, #pokemonStatsCaret_10, #pokemonStatsCaret_11").click(function () {
        var code = $(this).prop("id").split("_")[1];
        CaretToggle($(this), $("#pokemonStats_" + code));
        AdjustHeightOnMovesContainer(code)
    });

    $("#pokemonAbilities_00, #pokemonAbilities_01, #pokemonAbilities_10, #pokemonAbilities_11").hide();
    $("#pokemonAbilitiesCaret_00, #pokemonAbilitiesCaret_01, #pokemonAbilitiesCaret_10, #pokemonAbilitiesCaret_11").click(function () {
        var code = $(this).prop("id").split("_")[1];
        CaretToggle($(this), $("#pokemonAbilities_" + code));
        AdjustHeightOnMovesContainer(code)
    });

    $("#pokemonMoves_00, #pokemonMoves_01, #pokemonMoves_10, #pokemonMoves_11").hide();
    $("#pokemonMovesHeaders_00, #pokemonMovesHeaders_01, #pokemonMovesHeaders_10, #pokemonMovesHeaders_11").hide();
    $("#pokemonMovesCaret_00, #pokemonMovesCaret_01, #pokemonMovesCaret_10, #pokemonMovesCaret_11").click(function () {
        var code = $(this).parent().parent().parent().parent().prop("id").split("_")[1];
        CaretToggle($(this), $("#pokemonMoves_" + code));

        if ($(this).prop("src").includes("caret-bottom.svg")) {
            $("#pokemonMovesHeaders_" + code).hide();
        } else {
            $("#pokemonMovesHeaders_" + code).show();
        }

        AdjustHeightOnMovesContainer(code);
    });

    //Moves scrolling fix
    //$("body").addClass("bodyScroll");
    //$(".movesContainer").hover(function () {
    //    //Hover in
    //    $("body").removeClass("bodyScroll").addClass("bodyNoScroll");
    //}, function () {
    //    //Hover out
    //    $("body").removeClass("bodyNoScroll").addClass("bodyScroll");
    //});

    //Add the middle panel templates
    var topMiddlePanelTemplate = $("#topMiddlePanelTemplate").html();
    $("#middlePanel_0").html(topMiddlePanelTemplate);

    //Master button events
    UpdateMasterButton($("#statsColMasterBtn"), $("#statsExpMasterBtn"), $(".pokemonStatsCaret"));
    UpdateMasterButton($("#abilitiesColMasterBtn"), $("#abilitiesExpMasterBtn"), $(".pokemonAbilitiesCaret"));
    UpdateMasterButton($("#movesColMasterBtn"), $("#movesExpMasterBtn"), $(".pokemonMovesCaret"));

    //Limit checkboxes on Stats to the limits of the game
    $(".evCheckbox").change(function () {
        var code = $(this).prop("id").split("_")[1];
        var limit = 2;
        var current = 0;
        var checkboxes = [$("#pokemonStatHPEV_" + code), $("#pokemonStatATKEV_" + code), $("#pokemonStatDEFEV_" + code), $("#pokemonStatSPATKEV_" + code), $("#pokemonStatSPDEFEV_" + code), $("#pokemonStatSPDEV_" + code)];

        for (var i = 0; i < checkboxes.length; i++) {
            if (checkboxes[i].is(":checked")) {
                current++;
            }
        }

        if (current > limit) {
            $(this).prop("checked", false);
        }

        UpdateModifiedStats(code);
    });

    $(".natureCheckbox").change(function () {
        var code = $(this).prop("id").split("_")[1];
        var limit = 1;
        var current = 0;
        var checkboxes = [$("#pokemonStatHPNature_" + code), $("#pokemonStatATKNature_" + code), $("#pokemonStatDEFNature_" + code), $("#pokemonStatSPATKNature_" + code), $("#pokemonStatSPDEFNature_" + code), $("#pokemonStatSPDNature_" + code)];

        for (var i = 0; i < checkboxes.length; i++) {
            if (checkboxes[i].is(":checked")) {
                current++;
            }
        }

        if (current > limit) {
            $(this).prop("checked", false);
        }

        UpdateModifiedStats(code);
    });

    //Attack Simulator events
    $("#attackSimBtn").click(function () {
        var attackSimMove = null;
        var attackSimMoveId = null;
        var attackSimDefenderId = null;

        $("#statsExpMasterBtn").click();
        $("#abilitiesColMasterBtn").click();
        $("#movesExpMasterBtn").click();
        $("#extraMoveColMasterBtn").click();

        //$("#attackSimTxt").prop("placeholder", "Click on the move you want to attack with");
        $("#attackSimTxt").html("Click on the move you want to attack with");

        //Turn all moves into an event
        $(".moveName").addClass("selectableTxt");
        $(".moveName").click(function () {
            attackSimMove = $(this).html();
            attackSimMoveId = $(this).parent().parent().parent().parent().prop("id").split("_")[1];

            $(".moveName").removeClass("selectableTxt");
            $(".moveName").off("click");

            //$("#attackSimTxt").prop("placeholder", "Click on the Pokemon you want to attack");
            $("#attackSimTxt").html("Click on the Pokemon you want to attack");

            $(".pokemonName").addClass("selectableTxt");
            $(".pokemonName").click(function () {
                attackSimDefenderId = $(this).prop("id").split("_")[1];

                $(".pokemonName").removeClass("selectableTxt");
                $(".pokemonName").off("click");

                $("#attackSimTxt").html(AttackSimulation(attackSimMoveId, attackSimMove, attackSimDefenderId));
            });
        });
    });
}

function CaretToggle($caret, $container) {
    if ($caret.prop("src").includes("caret-bottom.svg")) {
        $container.show();
        $caret.prop("src", $caret.prop("src").replace("bottom", "top"));
    } else {
        $container.hide();
        $caret.prop("src", $caret.prop("src").replace("top", "bottom"));
    }
}

function UpdateExtraMoveMasterButton() {
    UpdateMasterButton($("#extraMoveColMasterBtn"), $("#extraMoveExpMasterBtn"), $(".pokemonMoveCaret"));
}

function UpdateMasterButton($col, $exp, $carets) {
    $col.click(function () {
        $carets.each(function () {
            if ($(this).prop("src").includes("caret-top.svg")) {
                $(this).click();
            }
        });
    });

    $exp.click(function () {
        $carets.each(function () {
            if ($(this).prop("src").includes("caret-bottom.svg")) {
                $(this).click();
            }
        });
    });
}

function AdjustHeightOnMovesContainer(code) {
    var currHeight = $("#pokemon_" + code).height();
    var moveContainerHeight = $("#pokemonMoves_" + code).height();
    currHeight -= moveContainerHeight;
    $("#pokemonMoves_" + code).height(390 - currHeight);
}

function UpdateModifiedStats(code) {
    $("#pokemonStatHPMod_" + code).html(CalculateHPStat(GetPokemonInBattle(code).hp, $("#pokemonStatHPEV_" + code).is(":checked")));
    $("#pokemonStatATKMod_" + code).html(CalculateStat(GetPokemonInBattle(code).atk, $("#pokemonStatATKEV_" + code).is(":checked"), $("#pokemonStatATKNature_" + code).is(":checked")));
    $("#pokemonStatDEFMod_" + code).html(CalculateStat(GetPokemonInBattle(code).def, $("#pokemonStatDEFEV_" + code).is(":checked"), $("#pokemonStatDEFNature_" + code).is(":checked")));
    $("#pokemonStatSPATKMod_" + code).html(CalculateStat(GetPokemonInBattle(code).spAtk, $("#pokemonStatSPATKEV_" + code).is(":checked"), $("#pokemonStatSPATKNature_" + code).is(":checked")));
    $("#pokemonStatSPDEFMod_" + code).html(CalculateStat(GetPokemonInBattle(code).spDef, $("#pokemonStatSPDEFEV_" + code).is(":checked"), $("#pokemonStatSPDEFNature_" + code).is(":checked")));
    $("#pokemonStatSPDMod_" + code).html(CalculateStat(GetPokemonInBattle(code).spd, $("#pokemonStatSPDEV_" + code).is(":checked"), $("#pokemonStatSPDNature_" + code).is(":checked")));

    UpdateTurnOrder();
}

function UpdateTurnOrder() {
    var pokemonOrder = [
        { pokemon: $("#pokemonImage_00").html(), side: "L", speed: $("#pokemonStatSPDMod_00").html() },
        { pokemon: $("#pokemonImage_10").html(), side: "L", speed: $("#pokemonStatSPDMod_10").html() },
        { pokemon: $("#pokemonImage_01").html(), side: "R", speed: $("#pokemonStatSPDMod_01").html() },
        { pokemon: $("#pokemonImage_11").html(), side: "R", speed: $("#pokemonStatSPDMod_11").html() }];

    pokemonOrder.sort(Compare);
    
    for (var i = pokemonOrder.length - 1; i >= 0; i--) {
        if (isNaN(pokemonOrder[i].speed)) pokemonOrder.splice(i, 1);
    }

    $("#turnOrder1, #turnOrder2, #turnOrder3, #turnOrder4").removeClass("blueBorder").removeClass("redBorder").html("");

    for (var i = 0; i < pokemonOrder.length; i++) {
        $("#turnOrder" + i).html(pokemonOrder[i].pokemon);

        if (pokemonOrder[i].side == "L") {
            $("#turnOrder" + i).removeClass("redBorder");
            $("#turnOrder" + i).addClass("blueBorder");
        }
        else if (pokemonOrder[i].side == "R") {
            $("#turnOrder" + i).removeClass("blueBorder");
            $("#turnOrder" + i).addClass("redBorder");
        }$
    }
}

function CalculateHPStat(baseHP, evCheck) {
    var ev = 0;

    if (evCheck) ev = 63;

    var tmp = ((2 * baseHP + 31 + ev + 100) * 50) / 100 + 10;
    return Math.trunc(((2 * baseHP + 31 + ev + 100) * 50) / 100 + 10);
}

function CalculateStat(baseStat, evCheck, natureCheck) {
    var ev = 0;
    var nature = 1;

    if (evCheck) ev = 63;
    if (natureCheck) nature = 1.1;

    return Math.trunc((((2 * baseStat + 31 + ev) * 50) / 100 + 5) * nature);
}

function SetPokemonInBattle(code, data) {
    for (var i = 0; i < pokemonInBattle.length; i++) {
        if (pokemonInBattle[i].code == code) {
            pokemonInBattle[i].data = data;
        }
    }
}

function GetPokemonInBattle(code) {
    for (var i = 0; i < pokemonInBattle.length; i++) {
        if (pokemonInBattle[i].code == code) {
            return pokemonInBattle[i].data;
        }
    }

    return null;
}

function GetMoveFromPokemon(pokemon, moveName) {
    for (var i = 0; i < pokemon.moves.length; i++) {
        if (pokemon.moves[i].name == moveName) {
            return pokemon.moves[i];
        }
    }

    return null;
}

function IsMoveSTAB(pokemon, move) {
    for (var i = 0; i < pokemon.types.length; i++) {
        if (pokemon.types[i].name.toLowerCase() == move.type) {
            return true;
        }
    }

    return false;
}

function GetMoveEffectiveness(defender, move) {
    var typeOrder = ["normal", "fire", "water", "grass", "electric", "ice", "fighting", "poison", "ground", "flying", "psychic", "bug", "rock", "ghost", "dragon", "dark", "steel", "fairy"];
    var typeMatrix = [
        [1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0.5, 0, 1, 1, 0.5, 1], //Normal
        [1, 0.5, 0.5, 2, 1, 2, 1, 1, 1, 1, 1, 2, 0.5, 1, 0.5, 2, 0], //Fire
        [1, 2, 0.5, 0.5, 1, 1, 1, 1, 2, 1, 1, 1, 2, 1, 0.5, 1, 1, 1], //Water
        [1, 0.5, 2, 0.5, 1, 1, 1, 0.5, 2, 0.5, 1, 0.5, 2, 1, 0.5, 1, 0.5, 1], //Grass
        [1, 1, 2, 0.5, 0.5, 1, 1, 1, 0, 2, 1, 1, 1, 1, 0.5, 1, 1, 1], //Electric
        [1, 0.5, 0.5, 2, 1, 0.5, 1, 1, 2, 2, 1, 1, 1, 1, 2, 1, 0.5, 1], //Ice
        [2, 1, 1, 1, 1, 2, 1, 0.5, 1, 0.5, 0.5, 0.5, 2, 0, 1, 2, 2, 0.5], //Fighting
        [1, 1, 1, 2, 1, 1, 1, 0.5, 0.5, 1, 1, 1, 0.5, 0.5, 1, 1, 0, 2], //Poison
        [1, 2, 1, 0.5, 2, 1, 1, 2, 1, 0, 1, 0.5, 2, 1, 1, 1, 2, 1], //Ground
        [1, 1, 1, 2, 0.5, 1, 2, 1, 1, 1, 1, 2, 0.5, 1, 1, 1, 0.5, 1], //Flying
        [1, 1, 1, 1, 1, 1, 2, 2, 1, 1, 0.5, 1, 1, 1, 1, 0, 0.5, 1], //Psychic
        [1, 0.5, 1, 2, 1, 1, 0.5, 0.5, 1, 0.5, 2, 1, 1, 0.5, 1, 2, 0.5, 0.5], //Bug
        [1, 2, 1, 1, 1, 2, 0.5, 1, 0.5, 2, 1, 2, 1, 1, 1, 1, 0.5, 1], //Rock
        [0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 2, 1, 1, 2, 1, 0.5, 1, 1], //Ghost
        [1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 2, 1, 0.5, 0], //Dragon
        [1, 1, 1, 1, 1, 1, 0.5, 1, 1, 1, 2, 1, 1, 2, 1, 0.5, 1, 0.5], //Dark
        [1, 0.5, 0.5, 1, 0.5, 2, 1, 1, 1, 1, 1, 1, 2, 1, 1, 1, 0.5, 2], //Steel
        [1, 0.5, 1, 1, 1, 1, 2, 0.5, 1, 1, 1, 1, 1, 1, 2, 2, 0.5, 1] //Fairy
    ];

    var multiplier = 1;
    var attackTypeIndex;
    for (var i = 0; i < typeOrder.length; i++) {
        if (move.type == typeOrder[i]) {
            attackTypeIndex = i;
        }
    }

    for (var i = 0; i < defender.types.length; i++) {
        var defenseTypeIndex;
        for (var j = 0; j < typeOrder.length; j++) {
            if (defender.types[i].name.toLowerCase() == typeOrder[j]) {
                defenseTypeIndex = j;
            }
        }

        multiplier = multiplier * typeMatrix[attackTypeIndex][defenseTypeIndex];
    }

    return multiplier;
}

function AttackSimulation(attackerId, moveName, defenderId) {
    //var result = "<span class=\"[ATTACKER_COLOUR_CLASS]\">[ATTACKER_NAME]'s [MOVE]</span> does <b>[MIN_DAMAGE]% - [MAX_DAMAGE]%</b> damage " +
    //    "to <span class=\"[DEFENDER_COLOUR_CLASS]\">[DEFENDER_NAME]</span> with a <b>[CRIT_RATE]%</b> chance to crit for <b>[MIN_DAMAGE_CRIT]% - [MAX_DAMAGE_CRIT]%</b> damage." +
    //    "</br>" +
    //    "<span class=\"[ATTACKER_COLOUR_CLASS]\">[MOVE]'s</span> effect is [MOVE_EFFECT] with a rate of <b>[MOVE_EFFECT_RATE]</b>.";
    var result = $("#attackSimResultTemplate").html();
    var attacker = GetPokemonInBattle(attackerId);
    var defender = GetPokemonInBattle(defenderId);
    
    result = result.replace("[ATTACKER_NAME]", attacker.name);
    result = result.replace("[DEFENDER_NAME]", defender.name);
    result = result.replace("[MOVE]", moveName);
    result = result.replace("[MOVE]", moveName);

    if (attackerId == "00" || attackerId == "10") {
        result = result.replace("[ATTACKER_COLOUR_CLASS]", "blueText");
        result = result.replace("[ATTACKER_COLOUR_CLASS]", "blueText");
    } else if (attackerId == "01" || attackerId == "11") {
        result = result.replace("[ATTACKER_COLOUR_CLASS]", "redText");
        result = result.replace("[ATTACKER_COLOUR_CLASS]", "redText");
    }

    if (defenderId == "00" || defenderId == "10") {
        result = result.replace("[DEFENDER_COLOUR_CLASS]", "blueText");
    } else if (defenderId == "01" || defenderId == "11") {
        result = result.replace("[DEFENDER_COLOUR_CLASS]", "redText");
    }

    //Damage calculation (When finished, compare to other calculators!!)
    var move = GetMoveFromPokemon(attacker, moveName);
    var attackerStat, defenderStat; //Either their Atk/Def or Sp.Atk/Sp.Def

    if (move.category == "physical") {
        attackerStat = $("#pokemonStatATKMod_" + attackerId).html();
        defenderStat = $("#pokemonStatDEFMod_" + defenderId).html();
    } else if (move.category == "special") {
        attackerStat = $("#pokemonStatSPATKMod_" + attackerId).html();
        defenderStat = $("#pokemonStatSPDEFMod_" + defenderId).html();
    } else {
        return "This move is invalid.";
    }

    var damageNoMod = ((2 * 50 / 5 + 2) * move.basePower * (attackerStat / defenderStat) / 50 + 2);
    var randomMin = 0.85;
    var randomMax = 1;
    var crit = 1.5;
    var stab = (IsMoveSTAB(attacker, move) ? 1.5 : 1);
    var type = GetMoveEffectiveness(defender, move);

    var minDamage = damageNoMod * randomMin * stab * type;
    var maxDamage = damageNoMod * randomMax * stab * type;

    minDamage = Math.trunc(minDamage);
    maxDamage = Math.trunc(maxDamage);

    result = result.replace("[CRIT_RATE]", move.baseCritRate);
    result = result.replace("[ACCURACY]", move.accuracy);


    var defenderHP = CalculateHPStat(defender.hp, $("#pokemonStatHPEV_" + defenderId).is(":checked"));

    var minDamagePercent = Math.trunc((minDamage / defenderHP) * 100);
    var maxDamagePercent = Math.trunc((maxDamage / defenderHP) * 100);

    result = result.replace("[MIN_DAMAGE]", minDamagePercent);
    result = result.replace("[MAX_DAMAGE]", maxDamagePercent);
    result = result.replace("[MIN_DAMAGE_CRIT]", Math.trunc(minDamagePercent * 1.5));
    result = result.replace("[MAX_DAMAGE_CRIT]", Math.trunc(maxDamagePercent * 1.5));

    result = result.replace("[MOVE_EFFECT]", move.secondaryEffect);
    result = result.replace("[MOVE_EFFECT_RATE]", move.effectRate);

    return result;
}

function Compare(a, b) {
    if (parseInt(a.speed) < parseInt(b.speed)) {
        return 1;
    }
    if (parseInt(a.speed) > parseInt(b.speed)) {
        return -1;
    }
    return 0;
}

if (!String.prototype.includes) {
    String.prototype.includes = function () {
        'use strict';
        return String.prototype.indexOf.apply(this, arguments) !== -1;
    };
}

if (!Math.trunc) {
    Math.trunc = function (v) {
        return v < 0 ? Math.ceil(v) : Math.floor(v);
    };
}