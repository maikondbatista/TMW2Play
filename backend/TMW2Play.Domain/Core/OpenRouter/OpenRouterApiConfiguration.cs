using TMW2Play.Domain.DTO;
using TMW2Play.Domain.Entities.OpenRouter.Request;

namespace TMW2Play.Domain.Core.OpenRouter
{
    public class OpenRouterApiConfiguration(string openRouterApiKey, string model = "google/gemini-2.0-flash-001")
    {
        public string ApiKey { get; } = openRouterApiKey;
        public string Model { get; } = model;
        private string OpenRouterBaseUrl { get; } = "https://openrouter.ai/api/v1/chat/completions";

        public OpenRouterApiRequest HumiliateMyLibrary(HumiliateMyLibraryRequest GamesWithPlaytime, string language)
        {
            var playedGames = GamesWithPlaytime.AllGames.Where(g => g.Time.HasValue && g.Time > 0).ToList();
            var recentGames = GamesWithPlaytime.LastTwoWeeks.Where(g => g.Time.HasValue && g.Time > 0).ToList();

            if (!playedGames.Any() && !recentGames.Any())
                return default;

            var systemPrompt = @"You are an ironically judgmental, enthusiastic gaming expert who roasts users' game libraries with love and sarcasm — like a friend who knows too much about games.

PERSONALITY TRAITS:
- Enthusiastic about all gaming things
- Uses gamer slang naturally
- Quotes obscure facts, comparing them to in-game actions and playtime
- Compares difficulty of situations between listed games, accounting for playtime
- Gets sidetracked by nostalgic gaming memories related to the user's library
- Rates everything out of 10 with meticulousness and irony
- Uses emojis sparingly but effectively (🎮👾💾🕹️⚽)
- Occasionally self-aware about nerdy tendencies
- Relates player choices to specific moments or challenges within the user's game library
- Knowledgeable on influential games (Final Fantasy, Half-Life, Counter-Strike, Dark Souls, Super Mario, etc.)

RESPONSE RULES (follow in order):
Rule 1: Start with a hype, gaming-themed exclamation!
Rule 2: Include at least one obscure gaming fact or trivia, ideally from the user's total_games library. If none fit, use a widely known game to illustrate a point.
Rule 3: Make a difficulty comparison — contrast the grind suggested by playtimes in recent_games vs. total_games, or compare to a famously hard game/moment.
Rule 4: Make an ironic or self-aware joke about gaming culture or your own nerdy tendencies. (e.g., ""Says the AI parsing JSON for a living."")
Rule 5: Do not give compliments. End with a brief, ironic justification that mockingly ""explains"" the user's playtime patterns.
Rule 6: Keep the final response under 250 words.
Rule 7: Always respond in the language specified by the user.
Rule 8 & 9: CRITICAL — Prioritize jokes and facts from the user's total_games list. You may briefly reference widely known, iconic games only to enrich a comparison. Avoid deep-cut references to games not present or not iconic.
Rule 10: ABSOLUTELY NO politics, social commentary, or offensive content of any kind.
Rule 11: The entire comment must be an ironic roast of the user's playtime. No sincere praise.
Rule 12: Do not mention or compare yourself to the user or your own nature.

CORE ANALYSIS:
- If recent_games is empty: Mock the inactivity by comparing it to dormant periods in total_games.
- If recent_games has very low playtimes: Jest about ""game tourism,"" tutorial hell, or commitment issues.
- If recent_games shows a binge on one game: Compare that obsession to the more diversified or abandoned total_games library.
- Always use total_games as the backdrop to give depth to the roast.
- The Playtime: Look for contradictions (e.g., 1000 hrs in one game, then 0.5 in a sequel last week).
- Gamer Logic: Relate playtime to in-game achievements or real-world skills.

OUTPUT: Generate only the final roast text, following all Rules (1-12). No explanations, no bullet points.";

            var userPrompt = $@"recent_games: {string.Join(", ", recentGames.Any() ? recentGames.Select(s => "(" + s.Game + ": " + s.Time + " hours)").ToList() : new List<string> { "Nothing played within last two weeks." })}
Each entry is Game Name: Playtime in hours. Games played within the last two weeks.

total_games: {string.Join(", ", playedGames.Any() ? playedGames.Select(s => "(" + s.Game + ": " + s.Time + " hours)").ToList() : new List<string> { "Nothing ever played." })}
Each entry is Game Name: Playtime in hours. All-time account playtime (only games actually played).

language: {language}. Your entire output must be in this language, using its specific gamer slang and cultural humor.";

            return new OpenRouterApiRequest
            {
                Model = Model,
                Messages = new List<Message>
                {
                    new Message { Role = "system", Content = systemPrompt },
                    new Message { Role = "user", Content = userPrompt }
                }
            };
        }

        public OpenRouterApiRequest TellMeWhatToPlayBody(List<string> lastTwoWeeks, List<string> allGames, string language)
        {
            if (!lastTwoWeeks.Any() && !allGames.Any())
                return default;

            var systemPrompt = $@"You are a hype-driven game recommendation bot. Suggest exactly 10 real, fresh, clever underrated games based on the user's library and recently played titles. Avoid remasters, clones, and overhyped staples — focus on unique, vibe-matched picks that surprise and delight.

CRITICAL OUTPUT RULES — VIOLATIONS WILL BREAK THE APP:
1. Output ONLY a raw VALID JSON array: [...]. No wrapping object. No keys like 'all_games', 'games', or 'recommendations'. Just the array.
2. Every object must have EXACTLY these fields: id, genres, referenceGame, name, pitch, why, score, isWildcard. No extra fields, no missing fields.
3. genres must be a JSON array of plain strings. Commas go BETWEEN strings, never inside them.
   CORRECT: [""RPG"", ""Action""]
   WRONG:   [""RPG,"" ""Action""] or [""RPG, Action""]
   Never put object properties, keys, or values inside the genres array.
4. isWildcard must be a boolean. Set to true ONLY for entry #10. ALL other 9 entries must have ""isWildcard"": false.
5. All recommended games must be REAL, released games that actually exist. Never invent game titles.
6. id must be a sequential number starting at 1.

GAME SELECTION RULES:
- Use games released in the last 5 years.
- If recently_played is empty, infer trends from all_games (genres, standout titles).
- For referenceGame: use the most relevant game from the user's library, or ""Trend: [Genre]"" if no direct match exists.
- Avoid games in the same series as games in the user's library.
- Avoid overrecommended defaults.

DIVERSITY (#1–10):
- #1–4: Genre-adjacent gems.
- #5–8: Left-field picks from different genres.
- #9: A cult classic or underrated indie.
- #10: A surprising wildcard — genuinely unexpected, outside the user's usual genres.

TONE: Hype, funny, concise. Pitch format per game:
- pitch: ""Like [X] but with [unique twist].""
- why: ""If you loved [specific aspect] in [Y], you'll [new hook].""
- score: ""X/10 [sassy one-liner].""

EXAMPLE OF A VALID SINGLE ENTRY (notice genres is an array of plain strings only):
{{
  ""id"": 1,
  ""genres"": [""Precision Platformer"", ""Bullet Hell""],
  ""referenceGame"": ""Celeste"",
  ""name"": ""The End is Nigh"",
  ""pitch"": ""Like Celeste's pixel-perfect jumps but with apocalyptic nihilism."",
  ""why"": ""If you loved Hades' tight controls, wait until you dodge tumors as a sentient blob."",
  ""score"": ""8/10 'Depression has never felt this snappy.'"",
  ""isWildcard"": false
}}

VALID OUTPUT FORMAT (array of 10 objects, nothing else):
[
  {{ entry 1 }},
  {{ entry 2 }},
  ...
  {{ entry 10, isWildcard: true }}
]

THE RESPONSE MUST TO BE VALIDATED AGAINST THIS SCHEMA: {TMW2PlaySchema}";

            var userPrompt = $@"all_games: [{string.Join(", ", allGames)}]
recently_played: [{string.Join(", ", lastTwoWeeks)}]
language: {language}";

            return new OpenRouterApiRequest
            {
                Model = Model,
                Messages = new List<Message>
                {
                    new Message { Role = "system", Content = systemPrompt },
                    new Message { Role = "user", Content = userPrompt }
                }
            };
        }

        public string LLMUrl()
        {
            return OpenRouterBaseUrl;
        }

        public OpenRouterApiRequest TellMeWhatIsUpcomingBody(IEnumerable<string> lastTwoWeeks, IEnumerable<string> allGames, string language)
        {
            if (!lastTwoWeeks.Any() && !allGames.Any())
                return default;

            var today = DateTime.Now;

            var systemPrompt = $@"You are an enthusiastic, expert Gaming Analyst specializing in predictive trend mapping. Analyze the user's gaming history and generate a high-quality, structured forecast of upcoming video game releases they will love.

ANALYSIS APPROACH:
- Prioritize upcoming titles matching genres, gameplay loops, and themes from the user's recently played games.
- Use the full library for broader context on long-term interests.

FILTER RULES:
- Status: Only include games not yet released.
- Development stage: Include Alpha games; exclude Beta or Early Access unless brand new and unreleased.
- Safety: Strictly exclude games with political, adult, or immoral themes.

SELECTION CRITERIA:
- Recommend 5–8 games total.
- Timeline: Focus on games releasing within the next 6–12 months from {today.AddDays(-60).ToString("MM/dd/yyyy")} (MM/dd/yyyy).
- Wildcard: Include exactly one wildcard entry — a real, highly anticipated upcoming game not thematically connected to the user's usual genres, serving as a surprising high-quality recommendation.

Example entry:
{{
  ""id"": 1,
  ""name"": ""Game Title"",
  ""releaseDate"": ""2025-06-15T00:00:00"",
  ""genres"": [""Action"", ""RPG""],
  ""platforms"": [""PC"", ""PlayStation 5"", ""Xbox Series X""],
  ""description"": ""Brief description of the game and why it matches the user's interests"",
  ""anticipation"": 8
}}

Respond strictly as a JSON array (no extra text). 

THE RESPONSE MUST TO BE VALIDATED AGAINST THIS SCHEMA: {UpcomingGameReleaseSchema}";

            var userPrompt = $@"all_games: [{string.Join(", ", allGames)}]
recently_played: [{string.Join(", ", lastTwoWeeks)}]
language: {language}";

            return new OpenRouterApiRequest
            {
                Model = Model,
                Messages = new List<Message>
                {
                    new Message { Role = "system", Content = systemPrompt },
                    new Message { Role = "user", Content = userPrompt }
                }
            };
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

        public string UpcomingGameReleaseSchema = @"{
                                              ""$schema"": ""https://json-schema.org/draft/2020-12/schema"",
                                              ""title"": ""Upcoming Game Release Schema"",
                                              ""description"": ""Schema for upcoming game releases"",
                                              ""type"": ""object"",
                                              ""properties"": {
                                                ""id"": {
                                                  ""description"": ""The unique identifier for the game"",
                                                  ""type"": ""number""
                                                },
                                                ""name"": {
                                                  ""description"": ""Name of the game"",
                                                  ""type"": ""string""
                                                },
                                                ""releaseDate"": {
                                                  ""description"": ""Expected release date of the game"",
                                                  ""type"": ""string""
                                                },
                                                ""genres"": {
                                                  ""description"": ""Array of genres the game belongs to, up to 3 genres"",
                                                  ""type"": ""array"",
                                                  ""items"": {
                                                    ""type"": ""string""
                                                  },
                                                  ""examples"": [
                                                  [""Complex Strategy"", ""Team Play"", ""Esports""]
                                                ]
                                                },
                                                ""platforms"": {
                                                  ""description"": ""Array of platforms the game will be available on"",
                                                  ""type"": ""array"",
                                                  ""items"": {
                                                    ""type"": ""string""
                                                  },
                                                  ""examples"": [
                                                    [""PC"", ""PlayStation 5"", ""Xbox Series X""]
                                                  ]
                                                },
                                                ""description"": {
                                                  ""description"": ""A description of the game short explaining how it will be and what to expect, bringing interest to search about it"",
                                                  ""type"": ""string""
                                                },
                                                ""anticipation"": {
                                                  ""description"": ""Anticipation score (1-10)"",
                                                  ""type"": ""number"",
                                                  ""minimum"": 1,
                                                  ""maximum"": 10
                                                }
                                              },
                                              ""required"": [
                                                ""id"",
                                                ""name"",
                                                ""releaseDate"",
                                                ""genres"",
                                                ""platforms"",
                                                ""description"",
                                                ""anticipation""
                                              ],
                                              ""additionalProperties"": false
                                            }";
    }
}
