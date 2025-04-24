using System.Reflection.Metadata;
using GDWeave.Godot;
using GDWeave.Godot.Variants;
using GDWeave.Modding;

namespace macrottie.exceptions.Patches;

public class PlayerPatcher : IScriptMod
{
    public bool ShouldRun(string path) => path == "res://Scenes/Singletons/SteamNetwork.gdc";

    public IEnumerable<Token> Modify(string path, IEnumerable<Token> tokens)
    {
        var lobbyFullWaiter = new MultiTokenWaiter([
            t => t is IdentifierToken { Name: "LOBBY_FULL" },
            t => t.Type is TokenType.Newline,
            t => t.Type is TokenType.Identifier,
            t => t.Type is TokenType.OpAssign,
            t => t.Type is TokenType.Constant
        ]);

        foreach (var token in tokens)
        {
            if (lobbyFullWaiter.Check(token))
            {
                yield return token;
                yield return new Token(TokenType.Newline, 2);
                yield return new Token(TokenType.CfIf);
                yield return new IdentifierToken("Steam");
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("getFriendRelationship");
                yield return new Token(TokenType.ParenthesisOpen);
                yield return new IdentifierToken("steam_id");
                yield return new Token(TokenType.ParenthesisClose);
                yield return new Token(TokenType.OpEqual);
                yield return new ConstantToken(new IntVariant(3));
                yield return new Token(TokenType.Colon);
                yield return new IdentifierToken("valid_join");
                yield return new Token(TokenType.OpAssign);
                yield return new ConstantToken(new BoolVariant(true));
            }
            else
            {
                yield return token;
            }

            
        }
    }
}