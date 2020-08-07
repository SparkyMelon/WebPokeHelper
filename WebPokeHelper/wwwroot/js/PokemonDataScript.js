$(document).ready(function () {
    var pokemonChosen = "";

    $("#pokemonListDropdown > .dropdown-item").click(function () {
        pokemonChosen = $(this).html();

        $("#pokemonListText").val(pokemonChosen);

        var pokemonUrl = window.location.href;
        pokemonUrl = pokemonUrl.split("PokemonData");
        pokemonUrl = pokemonUrl[0] + "PokemonData/?pokemonName=" + pokemonChosen;
        window.location.href = pokemonUrl;
    });

    $("#pokemonListText").on("keyup focus", function () {
        $("#pokemonListDropdown > .dropdown-item").each(function () {
            var currDropdownPokemon = $(this).text().toLowerCase();
            var searchStr = $("#pokemonListText").val().toLowerCase();

            if (!currDropdownPokemon.includes(searchStr)) {
                $(this).hide();
            }
            else {
                $(this).show();
            }
        });
    });

    $(".extraMoveInfo").hide();

    $(".pokemonMoveCaret").click(function () {
        if ($(this).prop("src").includes("caret-bottom.svg")) {
            $(this).parent().parent().parent().find(".extraMoveInfo").show();
            $(this).prop("src", $(this).prop("src").replace("bottom", "top"));
        }
        else {
            $(this).parent().parent().parent().find(".extraMoveInfo").hide();
            $(this).prop("src", $(this).prop("src").replace("top", "bottom"));
        }
    });
});

if (!String.prototype.includes) {
    String.prototype.includes = function () {
        'use strict';
        return String.prototype.indexOf.apply(this, arguments) !== -1;
    };
}