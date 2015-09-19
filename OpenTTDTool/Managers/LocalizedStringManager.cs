using OpenTTDTool.Entities;
using OpenTTDTool.Entities.SupportEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenTTDTool.Managers
{
    public class LocalizedStringManager
    {
        protected Dictionary<int, LocalizedString> Translations = new Dictionary<int, LocalizedString>();

        public LocalizedStringManager()
        {

        }

        public void Add(LocalizedString value)
        {
            Translations.SafeAdd(value.LanguageId, value);
        }

        public LocalizedString GetDefault()
        {
            if (Translations.Count == 0)
                return null;

            var lang = Constants.SupportedLanguages.FirstOrDefault(v => Translations.ContainsKey(v.Value));
            if (lang.HasValue)
                return Translations[lang.Value];
            else
                return Translations.Values.FirstOrDefault();
        }
    }
}
