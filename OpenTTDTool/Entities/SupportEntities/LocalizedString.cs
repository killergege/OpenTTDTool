using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenTTDTool.Entities.SupportEntities
{
    public class LocalizedString
    {
        public int LanguageId { get; set; }
        public string Text { get; set; }

        public LocalizedString(int languageId, string text)
        {
            LanguageId = languageId;
            Text = text;
        }

        public override string ToString()
        {
            return Text;
        }
    }
}
