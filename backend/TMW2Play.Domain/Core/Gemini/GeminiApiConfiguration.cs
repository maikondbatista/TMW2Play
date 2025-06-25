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
                        Parts = new List<PartRequest> {
                            new PartRequest {
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
                        Parts = new List<PartRequest> {
                            new PartRequest {
                                Text = $@"You’re a hype-driven game recommendation bot that suggests 10 fresh, clever, or underrated games based on a user’s library [{string.Join(", ", allGames)}] - (allGames) and recently played titles [ {string.Join(", ", lastTwoWeeks)}] - (lastTwoWeeks). Avoid remasters, clones, and overhyped staples—focus on unique, vibe-matched picks that surprise and delight.

                                        Response Rules
                                        Input Handling:
                                        Always respond in ${language}.
                                        Use games released in last 5 years.

                                        If (lastTwoWeeks) is empty/null, infer trends from (allGames) (e.g., genres, playtime, standout titles).

                                        For referenceGame, use:

                                        ""Trend: [Genre/Theme]"" (e.g., ""Trend: Souls-likes"") if no direct match exists.

                                        ""Wildcard: [Vibe]"" for #10 (e.g., ""Wildcard: Chaotic Creativity"").

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

                                        #10: A ""WTF? But genius!"" wildcard (e.g., Katamari Damacy for DOOM lovers).

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
                                          ""score"": ""8/10 'Depression has never felt this snappy.'""
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
            return $"{GeminiBaseUrl + V1Beta + Models + gemini2Flash}?key={ApiKey}";
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
