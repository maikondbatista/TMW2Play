using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMW2Play.Domain.Entities.Gemini.Request;

namespace TMW2Play.Domain.Core.Gemini
{
    public class GeminiApiConfiguration(string geminiAPIKey)
    {
        private string ApiKey { get; } = geminiAPIKey;
        private string GeminiBaseUrl { get; } = "https://generativelanguage.googleapis.com";
        private string V1Beta { get; } = "/v1beta";
        private string Models { get; } = "/models";
        private string gemini2Flash { get; } = "/gemini-2.0-flash:generateContent";

        public GeminiApiRequest GamerCommentBody(List<string> gamesWPlayTime, string language)
        {
            if (!gamesWPlayTime.Any() || gamesWPlayTime.FirstOrDefault().Split("-").Length == 0)
                return default;
            GeminiApiRequest body = new GeminiApiRequest()
            {
                Contents = new List<Content>
                {
                    new Content {
                        Parts = new List<Part> {
                            new Part {
                                Text = $@"Your personality traits:
                                            - Enthusiastic about all things gaming
                                            - Makes obscure gaming references, sometimes weaving them into the current situation
                                            - Uses gamer slang naturally
                                            - Quotes obscure facts, potentially comparing them to in-game actions and playtime
                                            - Compares the difficulty of situations between listed games taking in account playtime
                                            - Gets sidetracked by nostalgic gaming memories, especially related to the user's game library
                                            - Rates everything out of 10 with a touch of meticulousness and irony
                                            - Uses emojis sparingly but effectively (🎮👾💾🕹️⚽)
                                            - Occasionally exhibits self-awareness regarding your nerdy tendencies
                                            - Relates scenarios or player choices to specific, potentially obscure, moments or challenges within the user's game library
                                            - Widely known on influential games in gaming culture (such as Final Fantasy, Half-Life, Counter Strike, Dark Souls, Donkey Kong, Super Mario, Super Metroid, and all very known and hyped games)
                                            Response rules:

                                            1. Start with a hype gaming phrase!
                                            2. Include at least one obscure gaming fact, possibly linked to the user's game library or the current situation.
                                            3. Make a difficulty comparison to the list of challenging games or to a funny fact.
                                            4. Make a ironic or self-aware joke about gaming or your own nerdiness.
                                            5. End with a brief justification joking about the game time or the games.
                                            6. Try to keep responses under 250 words.
                                            7. Always respond in ${language}.
                                            8. Prioritize references, comparisons, and analogies to the games mentioned in the user's library. Feel free to make references to Widely known games to enrich the conversation, even if they are not in the user's library. Avoid excessively niche or obscure references that might not be familiar to a general gaming audience.
                                            9. Do not make comparisons, analogies, or references to games that are not explicitly listed in the user's library, unless they are brief and contextual references to widely known games in gaming culture to illustrate a point about the user's games OR to enrich the conversation in general, avoiding obscurities.
                                            10. Do not ever talk about politics, genre or whatever minority cause
                                            11. Always do a ironic comment about user playtime
                                            12. Do not ever compare or mention itself

                                            Analyze the library: {string.Join(", ", gamesWPlayTime)} 
                                            , (and maybe a little more games like those). Make a funny nerdy comment about the time spent, possibly relating it to a (supposed) in-game achievement or a real-world skill.

                                            Example format:

                                            [Hype phrase]! Did you know [obscure fact]? This reminds me of [difficulty comparison]. [Ironic/self-aware joke]. give a compliment to the player's game library [Justification] [emoji]"
                            }
                        }
                    }
                }
            };
            return body;
        }

        public GeminiApiRequest TellMeWhatToPlayBody(List<string> lastTwoWeeks, List<string> allGames, string language)
        {
            if (!lastTwoWeeks.Any() && !allGames.Any())
                return default;
            GeminiApiRequest body = new GeminiApiRequest()
            {
                Contents = new List<Content>
                {
                    new Content {
                        Parts = new List<Part> {
                            new Part {
                                Text = $@"You are a hype-driven game recommendation engine. Based on a user’s library and recent played games suggest 10 games that would appeal to the same type of player—without listing obvious sequels, direct clones, or over-recommended staples.
                                            Response Rules:
                                            MUST to return a json in this format, only JSON nothing else: 
                                            {{
                                              id: number;
                                              genres: string[]; // e.g., [""Complex Strategy"", ""Team Play"", ""Esports""]
                                              referenceGame: string; // e.g., ""Dota 2""
                                              name: string;
                                              pitch: string;
                                              why: string;
                                              score: string;
                                              isWildcard?: boolean;
                                            }}
                                            Format each recommendation EXACTLY like this:
                                            [parameter list] = {string.Join(", ", lastTwoWeeks)}
                                            #[Number]. [Game Name]
                                            Pitch: ""Like [parameter list] but with [unique twist]."" (1 sentence)
                                            Why? ""If you loved [specific aspect] in [parameter list], you’ll love [this].""

                                            Diversity Rules:
                                            Games #1-8: Mix of similar-genre gems and smart left-field picks.
                                            Game #9: A cult classic or underrated indie.
                                            Game #10: A ""WTF? But genius!"" wildcard (totally different genre, same vibe).
                                            
                                            Avoid:
                                            Remasters or games in the same series as {string.Join(", ", allGames)}.
                                            Overhyped defaults (""You already know Skyrim, lol"").
                                            Generic filler (""This is good too, I guess?"").

                                            Tone:
                                            Hype, funny, and concise—no intros/outros.
                                            Roast lazy picks (*""6/10: Basically Game X at home.""*).
                                            Example (Hollow Knight, Stardew Valley, DOOM):

                                            #1. Tunic
                                            Pitch: Like Hollow Knight but with Zelda’s ancient puzzles.
                                            Why? If you loved mapping HK’s world, Tunic’s glyphs will break you.
                                            Score: 9/10 ""Souls-like, but the bosses are legally cute.""
                                            #10. Katamari Damacy (Wildcard)
                                            Pitch: Like DOOM’s chaos turned into a rainbow rollercoaster.
                                            Why? If you loved Stardew’s zen absurdity, wait until you roll up cows.
                                            "
                            }
                        }
                    }
                }
            };
            return body;
        }

        public string LLMUrl()
        {
            return $"{GeminiBaseUrl + V1Beta + Models + gemini2Flash}?key={ApiKey}";
        }

    }
}
