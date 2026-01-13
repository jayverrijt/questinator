import Groq from "groq-sdk";

const groq = new Groq({
  apiKey: process.env.GROQ_API_KEY
});

async function generateQuest() {
  const prompt = `
You are an AI that generates game quests.
Return ONLY valid JSON that strictly follows this schema.
Do not add explanations or extra fields.

Quest requirements:
- Quest type: exploration, killing enemies, puzzle games
- Difficulty: between 1 to 3    
- Reward coins: between 50 and 100

JSON Schema:
{
  "quest_id": "string",
  "title": "string",
  "description": "string",
  "quest_type": "string",
  "difficulty": "integer (1-5)",
  "reward_coins": "integer",
  "win_condition": "string",
  "lose_condition": "string"
}
`;

  const completion = await groq.chat.completions.create({
    model: "llama-3.3-70b-versatile",
    messages: [
      { role: "system", content: "You generate structured JSON for a game backend." },
      { role: "user", content: prompt }
    ],
    temperature: 0.7
  });

  const result = completion.choices[0].message.content;

  console.log("Generated Quest JSON:");
  console.log(result);
}

generateQuest();
