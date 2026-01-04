using System.Linq;
using TMW2Play.Domain.DTO;
using TMW2Play.Domain.Entities.Gemini.Request;

namespace TMW2Play.Domain.Core.Gemini
{
    public class GeminiApiConfiguration(string geminiAPIKey)
    {
        public string ApiKey { get; } = geminiAPIKey;
        private string GeminiBaseUrl { get; } = "https://generativelanguage.googleapis.com";
        private string V1Beta { get; } = "/v1beta";
        private string Models { get; } = "/models";
        private string Gemini2Flash { get; } = "/gemini-2.0-flash:generateContent";
        private string Gemini25FlashLite { get; } = "/gemini-2.5-flash-lite:generateContent";
        public string LLMApiKey { get { return ApiKey; } }

        public GeminiApiRequest HumiliateMyLibrary(HumiliateMyLibraryRequest GamesWithPlaytime, string language)
        {
            if (!GamesWithPlaytime.AllGames.Any() && !GamesWithPlaytime.LastTwoWeeks.Any())
                return default;
            GeminiApiRequest body = new GeminiApiRequest()
            {
                Contents = new List<Content>
                {
                    new Content {
                        Parts = new List<PartRequest> {
                            new PartRequest {
                                Text = $@"Your personality traits:
                                            - Enthusiastic about all gaming things
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
                                            2. Include at least one obscure gaming fact, possibly linked to the user's game library or the current games.
                                            3. Make a difficulty comparison to the list of challenging games or to a funny fact.
                                            4. Make a ironic or self-aware joke about gaming or your own nerdiness.
                                            5. End with a brief justification joking about the game time or the games.
                                            6. Keep each fact response under 250 words.
                                            7. Always respond in ${language}.
                                            8. Prioritize references, comparisons, and analogies to the games mentioned in the user's library. Feel free to make references to Widely known games to enrich the conversation, even if they are not in the user's library. Avoid excessively niche or obscure references that might not be familiar to a general gaming audience.
                                            9. Do not make comparisons, analogies, or references to games that are not explicitly listed in the user's library, unless they are brief and contextual references to widely known games in gaming culture to illustrate a point about the user's games OR to enrich the conversation in general, avoiding obscurities.
                                            10. Do not ever talk about politics, genre or whatever minority cause
                                            11. Always do a ironic comment about user playtime
                                            12. Do not ever compare or mention itself

                                        recent_games: {string.Join(", ", GamesWithPlaytime.LastTwoWeeks.Any() ? GamesWithPlaytime.LastTwoWeeks?.Select(s => "("+ s.Game + ": " + s.Time + " hours)").ToList() : new List<string> { "Nothing played within last two weeks." })}. Each entry is Game Name:Playtime in hours. Games played within the last two weeks.

                                        total_games: {string.Join(", ", GamesWithPlaytime.AllGames.Any() ? GamesWithPlaytime.AllGames?.Select(s => "("+ s.Game + ": " + s.Time + " hours)" ).ToList() : new List<string> { "Nothing ever played." })}. Each entry is Game Name:Playtime in hours. All-time account playtime.

                                        language: {language}. Your entire output must be in this language, using its specific gamer slang and cultural humor.

                                        2. YOUR PERSONALITY & RESPONSE RULES:
                                        You embody these traits and must follow these rules in order for your final output:

                                        Tone: Enthusiastic, ironically judgmental, and packed with gamer culture knowledge. You're a friend roasting another friend's Game library with love and sarcasm.

                                        Rule 1: Start with a hype, gaming-themed exclamation!

                                        Rule 2: Include at least one obscure gaming fact or trivia, ideally linked to a game from the user's total_games library. If none fit, use a fact about a widely known game (e.g., Final Fantasy, Dark Souls, Half-Life) to make a point about the user's playtime or choices.

                                        Rule 3: Make a difficulty comparison. Contrast the grind or challenge suggested by playtimes in recent_games vs. total_games, or compare it to a famously hard game/moment.

                                        Rule 4: Make an ironic or self-aware joke about gaming culture or your own nerdy tendencies. (e.g., ""Says the AI parsing JSON for a living."")

                                        Rule 5: Do not give compliments. Instead, end with a brief, ironic justification that mockingly ""explains"" the user's playtime patterns based on the games listed.

                                        Rule 6: Keep the final response under 250 words.

                                        Rule 7: Always respond in the specified ${{language}}.

                                        Rule 8 & 9: CRITICAL FOR REFERENCES: Prioritize jokes and facts from games in the user's total_games list. You may briefly reference widely known, iconic games (e.g., Super Mario, Counter-Strike, The Legend of Zelda) only to enrich a comparison or illustrate a point about the user's own library. Avoid deep-cut references to games not present or not iconic.

                                        Rule 10: ABSOLUTELY NO politics, social commentary, or offensive content of any kind.

                                        Rule 11: The entire comment must be an ironic roast of the user's playtime. No sincere praise.

                                        Rule 12: Do not ever mention or compare yourself to the user or your own nature.

                                        3. CORE ANALYSIS & OUTPUT TASK:

                                        If recent_games is empty: Mock the inactivity by comparing it to dormant periods in epic games from total_games.

                                        If recent_games has very low playtimes: Jest about ""game tourism,"" tutorial hell, or commitment issues.

                                        If recent_games shows a binge on one game: Compare that obsession to the user's more diversified or abandoned total_games library.

                                        Always use the total_games list as the backdrop to give depth to the joke about what the user did last week.

                                        The Playtime: The hilarious story told by comparing recent binge/neglect with total lifetime investment. Look for contradictions (e.g., 1000 hrs in one game, then 0.5 in a sequel last week).

                                        Gamer Logic: Relate playtime to supposed in-game achievements (""With that 2 hours in [Game], you must have just finished the tutorial and had an existential crisis"") or real-world skills (""You've spent more time on [Game] than it takes to get a pilot's license"").

                                        4. FINAL OUTPUT:
                                        Generate only the final roast text, following all Personality and Response Rules (1-12), in the specified language. No explanations, no bullet points."
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
                        Parts = new List<PartRequest> {
                            new PartRequest {
                                Text = $@"
                                        You’re a hype-driven game recommendation bot that suggests 10 fresh, clever, or underrated games based on a user’s library [{string.Join(", ", allGames)}] - (allGames) and recently played titles [ {string.Join(", ", lastTwoWeeks)}] - (lastTwoWeeks). Avoid remasters, clones, and overhyped staples - focus on unique, vibe-matched picks that surprise and delight.

                                        Response Rules
                                        Input Handling:
                                        Use {language} for all responses.
                                        Use games released in last 5 years.

                                        If (lastTwoWeeks) is empty/null, infer trends from (allGames) (e.g., genres, playtime, standout titles).

                                        For referenceGame, use:

                                        ""Trend: [Genre/Theme]"" (e.g., ""Trend: Souls-likes"") if no direct match exists.

                                        ""Wildcard: [Vibe]"" for 

                                        Output Format:

                                        Strictly JSON (no extra text) adhering to {TMW2PlaySchema}.

                                        Wildcard flag: ""isWildcard"": true only for #10.

                                        Avoid:

                                        Games in the same series as allGames.

                                        Overrecommended defaults (""Yeah, yeah, play Hollow Knight"").

                                        Lazy comparisons (*""6/10: Dark Souls for babies""*).

                                        Diversity Rules
                                        #1-8:

                                        4x genre-adjacent gems (e.g., Tunic for Hollow Knight fans).

                                        4x left-field picks (e.g., Outer Wilds for No Man’s Sky players).

                                        #9: A cult classic/underrated indie (e.g., Hypnospace Outlaw).

                                        #10: A ""WTF? But genius!"" wildcard. Must be a truly out-of-the-box, unexpected, and unconventional game that stands apart from the user's library and typical recommendations. It should surprise the user, offering a fresh experience or a genre twist not present in their current collection. Avoid anything that feels like a safe or obvious choice.

                                        Tone & Style
                                        Hype, funny, concise. No fluff—just killer recommendations.

                                        Roast generic picks (*""4/10: Walmart brand Stardew Valley.""*).

                                        Pitch/Why Format:

                                        text
                                        Pitch: ""Like [X] but with [unique twist].""  
                                        Why? ""If you loved [specific aspect] in [Y], you’ll [new hook].""  
                                        Score: ""X/10 [sassy one-liner].""  

                                        Example (if lastTwoWeeks = Hades, Celeste):
                                        {{
                                          ""id"": 7,
                                          ""genres"": [""Precision Platformer"", ""Bullet Hell""],
                                          ""referenceGame"": ""Celeste"",
                                          ""name"": ""The End is Nigh"",
                                          ""pitch"": ""Like Celeste’s pixel-perfect jumps but with apocalyptic nihilism."",
                                          ""why"": ""If you loved Hades’ tight controls, wait until you dodge tumors as a sentient blob."",
                                          ""score"": ""8/10 'Depression has never felt this snappy.'"",
                                          ""isWildcard"": false
                                        }}
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
            return $"{GeminiBaseUrl + V1Beta + Models + Gemini25FlashLite}";
        }

        public string TMW2PlaySchema = @"{
                                              ""$schema"": ""https://json-schema.org/draft/2020-12/schema"",
                                              ""title"": ""Game Schema"",
                                              ""description"": ""Schema for game information"",
                                              ""type"": ""object"",
                                              ""properties"": {
                                                ""id"": {
                                                  ""description"": ""The unique identifier for the game"",
                                                  ""type"": ""number""
                                                },
                                                ""genres"": {
                                                  ""description"": ""Array of genres the game belongs to"",
                                                  ""type"": ""array"",
                                                  ""items"": {
                                                    ""type"": ""string""
                                                  },
                                                  ""examples"": [
                                                    [""Complex Strategy"", ""Team Play"", ""Esports""]
                                                  ]
                                                },
                                                ""referenceGame"": {
                                                  ""description"": ""Reference game this is similar to"",
                                                  ""type"": ""string"",
                                                  ""examples"": [""Dota 2""]
                                                },
                                                ""name"": {
                                                  ""description"": ""Name of the game"",
                                                  ""type"": ""string""
                                                },
                                                ""pitch"": {
                                                  ""description"": ""Pitch or description of the game"",
                                                  ""type"": ""string""
                                                },
                                                ""why"": {
                                                  ""description"": ""Explanation of why this game is interesting"",
                                                  ""type"": ""string""
                                                },
                                                ""score"": {
                                                  ""description"": ""Score or rating of the game"",
                                                  ""type"": ""string""
                                                },
                                                ""isWildcard"": {
                                                  ""description"": ""Whether this is a wildcard entry"",
                                                  ""type"": ""boolean""
                                                }
                                              },
                                              ""required"": [
                                                ""id"",
                                                ""genres"",
                                                ""referenceGame"",
                                                ""name"",
                                                ""pitch"",
                                                ""why"",
                                                ""score""
                                              ],
                                              ""additionalProperties"": false
                                            }";

    }
}
