using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Threading.Tasks;

namespace app.DAL.Models
{
    public class JobSkillConverter : JsonConverter<ICollection<JobSkill>>
    {
        public override ICollection<JobSkill> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            //we are converting the string of arrays to the collection of jobskill object
            var jobSkills = new List<JobSkill>();

            if (reader.TokenType == JsonTokenType.StartArray)
            {
                while (reader.Read())
                {
                    if (reader.TokenType == JsonTokenType.EndArray)
                        break;

                    if (reader.TokenType == JsonTokenType.String)
                    {
                        var skillName = reader.GetString();
                        if (!string.IsNullOrWhiteSpace(skillName))
                        {
                            jobSkills.Add(new JobSkill { SkillName = skillName });
                        }
                    }
                }
            }
            return jobSkills;
        }

        public override void Write(Utf8JsonWriter writer, ICollection<JobSkill> value, JsonSerializerOptions options)
        {
            //we are serializing the jobskill object to the JSON array of skill names for our api response
            writer.WriteStartArray();
            foreach (var jobSkill in value)
            {
                writer.WriteStringValue(jobSkill.SkillName);
            }
            writer.WriteEndArray();
        }
    }

}
